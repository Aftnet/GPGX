#include "pch.h"
#include "Renderer.h"
#include "ColorConverter.h"

using namespace LibretroRT_FrontendComponents_Renderer;

using namespace Windows::Graphics::DirectX::Direct3D11;
using namespace Windows::Foundation::Numerics;

Renderer::Renderer(CanvasAnimatedControl^ canvas) :
	Canvas(canvas),
	OpenGLESManager(OpenGLES::GetInstance())
{
	__abi_ThrowIfFailed(GetDXGIInterface(canvas->Device, Direct3DDevice.GetAddressOf()));

	ColorConverter::InitializeLookupTable();
}

Renderer::~Renderer()
{
	DestroyRenderTargets();
}


void Renderer::InitializeVideoParameters(ICore^ core)
{
	GeometryChanged(core->Geometry);
	PixelFormatChanged(core->PixelFormat);
	TimingChanged(core->Timing);
}

void Renderer::GeometryChanged(GameGeometry^ geometry)
{
	Geometry = geometry;

	if (Geometry == nullptr)
	{
		return;
	}

	critical_section::scoped_lock lock(RenderTargetCriticalSection);

	bool shouldUpdate = true;
	if (Win2DTexture)
	{
		auto size = Win2DTexture->SizeInPixels;
		shouldUpdate = (size.Width < Geometry->MaxWidth || size.Height < Geometry->MaxHeight);
	}

	if (shouldUpdate)
	{
		auto dimension = max(Geometry->MaxWidth, Geometry->MaxHeight);
		dimension = max(dimension, RenderTargetMinSize);
		dimension = ClosestGreaterPowerTwo(dimension);
		CreateRenderTargets(Canvas, dimension, dimension);
	}
}

void Renderer::PixelFormatChanged(PixelFormats format)
{
	PixelFormat = format;
}

void Renderer::RotationChanged(Rotations rotation)
{
	Rotation = rotation;
}

void Renderer::TimingChanged(SystemTiming^ timings)
{
	TimeSpan newTargetDuration = { 10000000.0 / timings->FPS };
	Canvas->TargetElapsedTime = newTargetDuration;
}

void Renderer::RenderVideoFrame(const Array<byte>^ frameBuffer, unsigned int width, unsigned int height, unsigned int pitch)
{
	//Incomplete initialization
	if (PixelFormat == PixelFormats::FormatUknown || Win2DTexture == nullptr)
	{
		return;
	}

	//Duped frame
	if (frameBuffer == nullptr || frameBuffer->Length < 1)
	{
		return;
	}

	RenderTargetViewport.Width = width;
	RenderTargetViewport.Height = height;

	critical_section::scoped_lock lock(RenderTargetCriticalSection);
	
	if (usingHardwareRendering)
	{
		OpenGLESManager->MakeCurrent(OpenGLESSurface);
		glClearColor(1.0f, 0.0f, 0.0f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT);
		glFlush();
	}
	else
	{
		ComPtr<ID3D11DeviceContext> d3dContext;
		Direct3DDevice->GetImmediateContext(d3dContext.GetAddressOf());

		D3D11_MAPPED_SUBRESOURCE mappedResource;
		__abi_ThrowIfFailed(d3dContext->Map(Direct3DTexture.Get(), 0, D3D11_MAP_WRITE_DISCARD, 0, &mappedResource));

		auto pSrc = frameBuffer->Data;
		auto pDest = (byte*)mappedResource.pData;

		switch (PixelFormat)
		{
		case PixelFormats::FormatXRGB8888:
		{
			for (auto i = 0; i < height; i++)
			{
				static const size_t rgba8888bpp = 4;
				memcpy(pDest, pSrc, width * rgba8888bpp);

				pSrc += pitch;
				pDest += mappedResource.RowPitch;
			}
		}
		break;
		case PixelFormats::FormatRGB565:
		{
			for (auto i = 0; i < height; i++)
			{
				ColorConverter::ConvertRGB565ToXRGB8888(pDest, pSrc, width);

				pSrc += pitch;
				pDest += mappedResource.RowPitch;
			}			
		}
		break;
		}
		
		d3dContext->Unmap(Direct3DTexture.Get(), 0);
	}
}

void Renderer::CanvasDraw(ICanvasAnimatedControl^ sender, CanvasAnimatedDrawEventArgs^ args)
{
	auto drawingSession = args->DrawingSession;
	auto canvasSize = sender->Size;
	auto aspectRatio = Geometry->AspectRatio;

	if (Win2DTexture == nullptr || RenderTargetViewport.Width <= 0 || RenderTargetViewport.Height <= 0)
	{
		return;
	}

	static const float piValue = 3.14159265358979323846f;
	auto rotAngle = 0.0f;
	switch (Rotation)
	{
	case Rotations::CCW90:
		rotAngle = -0.5f * piValue;
		aspectRatio = 1.0f / aspectRatio;
		break;
	case Rotations::CCW180:
		rotAngle = -piValue;
		break;
	case Rotations::CCW270:
		rotAngle = -1.5f * piValue;
		aspectRatio = 1.0f / aspectRatio;
		break;
	}

	auto destinationSize = ComputeBestFittingSize(canvasSize, aspectRatio);
	auto scaleMatrix = make_float3x2_scale(destinationSize.Width, destinationSize.Height);
	auto rotMatrix = make_float3x2_rotation(rotAngle);
	auto transMatrix = make_float3x2_translation(0.5f * canvasSize.Width, 0.5f * canvasSize.Height);
	auto transformMatrix = scaleMatrix * rotMatrix * transMatrix;

	critical_section::scoped_lock lock(RenderTargetCriticalSection);
	drawingSession->Transform = transformMatrix;
	drawingSession->DrawImage(Win2DTexture, Rect(-0.5f, -0.5f, 1.0f, 1.0f), RenderTargetViewport);
	drawingSession->Transform = float3x2::identity();
}

void Renderer::CreateRenderTargets(CanvasAnimatedControl^ canvas, unsigned int width, unsigned int height)
{
	DestroyRenderTargets();

	ComPtr<IDXGISurface> d3dSurface;

	if (usingHardwareRendering)
	{
		OpenGLESSurface = OpenGLESManager->CreateSurface(width, height, EGL_TEXTURE_RGBA);
		auto surfaceHandle = OpenGLESManager->GetSurfaceShareHandle(OpenGLESSurface);

		__abi_ThrowIfFailed(Direct3DDevice->OpenSharedResource(surfaceHandle, __uuidof(IDXGISurface), (void**)d3dSurface.GetAddressOf()));
	}
	else
	{
		D3D11_TEXTURE2D_DESC texDesc = { 0 };
		texDesc.Width = width;
		texDesc.Height = height;
		texDesc.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
		texDesc.MipLevels = 1;
		texDesc.ArraySize = 1;
		texDesc.SampleDesc.Count = 1;
		texDesc.SampleDesc.Quality = 0;
		texDesc.Usage = D3D11_USAGE_DYNAMIC;
		texDesc.BindFlags = D3D11_BIND_SHADER_RESOURCE;
		texDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
		texDesc.MiscFlags = 0;

		__abi_ThrowIfFailed(Direct3DDevice->CreateTexture2D(&texDesc, nullptr, Direct3DTexture.GetAddressOf()));
		__abi_ThrowIfFailed(Direct3DTexture.As(&d3dSurface));
	}

	auto winRTSurface = CreateDirect3DSurface(d3dSurface.Get());
	Win2DTexture = CanvasBitmap::CreateFromDirect3D11Surface(canvas->Device, winRTSurface);
}

void Renderer::DestroyRenderTargets()
{
	Win2DTexture = nullptr;
	Direct3DTexture.Reset();

	if (OpenGLESSurface != EGL_NO_SURFACE)
	{
		OpenGLESManager->DestroySurface(OpenGLESSurface);
		OpenGLESSurface = EGL_NO_SURFACE;
	}
}

Size Renderer::ComputeBestFittingSize(Size viewportSize, float aspectRatio)
{
	auto candidateWidth = std::floor(viewportSize.Height * aspectRatio);
	Size size(candidateWidth, viewportSize.Height);
	if (viewportSize.Width < candidateWidth)
	{
		auto height = viewportSize.Width / aspectRatio;
		size = Size(viewportSize.Width, height);
	}

	return size;
}

unsigned int Renderer::ClosestGreaterPowerTwo(unsigned int value)
{
	unsigned int output = 1;
	while (output < value)
	{
		output *= 2;
	}

	return output;
}
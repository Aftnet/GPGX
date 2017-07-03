#pragma once

#include "OpenGLES.h"

using namespace LibretroRT;

using namespace Microsoft::Graphics::Canvas;
using namespace Microsoft::Graphics::Canvas::UI::Xaml;
using namespace Microsoft::WRL;
using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Graphics::DirectX::Direct3D11;

namespace LibretroRT_FrontendComponents_Renderer
{
	public ref class RenderTargetManager sealed
	{
	public:
		RenderTargetManager(CanvasAnimatedControl^ canvas);
		virtual ~RenderTargetManager();

		void UpdateFormat(GameGeometry^ geometry, PixelFormats pixelFormat);
		void UpdateFromCoreOutput(const Array<byte>^ frameBuffer, unsigned int width, unsigned int height, unsigned int pitch);
		void Render(CanvasDrawingSession^ drawingSession, Size canvasSize);

	private:
		static const unsigned int RenderTargetMinSize = 1024;
		static const std::map<PixelFormats, DXGI_FORMAT> LibretroToDXGITextureFormatsMapping;

		CanvasAnimatedControl^ const Canvas;
		ComPtr<ID3D11Device> Device;
		ComPtr<ID3D11Texture2D> D3DRenderTarget;

		std::shared_ptr<OpenGLES> OpenGLESManager;
		EGLSurface OpenGLESRenderTarget;
		Rect RenderTargetViewport;
		float RenderTargetAspectRatio = 0.0f;

		void CreateLinkedTextures(ComPtr<ID3D11Device> device, unsigned int width, unsigned int height);
		static Rect ComputeBestFittingSize(Size viewportSize, float aspectRatio);
		static unsigned int ClosestGreaterPowerTwo(unsigned int value);
	};
}

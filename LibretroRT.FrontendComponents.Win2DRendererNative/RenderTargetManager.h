#pragma once

using namespace Windows::Foundation;

namespace LibretroRT_FrontendComponents_Win2DRendererNative
{
	class RenderTargetManager
	{
	public:
		RenderTargetManager();
		~RenderTargetManager();

	private:
		static const unsigned int RenderTargetMinSize = 1024;
		Rect RenderTargetViewport;
		float RenderTargetAspectRatio = 1.0f;

		static Rect ComputeBestFittingSize(Size viewportSize, float aspectRatio);
		static unsigned int ClosestGreaterPowerTwo(unsigned int value);
	};
}
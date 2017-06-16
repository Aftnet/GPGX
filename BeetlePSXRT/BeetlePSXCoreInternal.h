﻿#pragma once

#include "../LibretroRT_Tools/CoreBase.h"

using namespace Platform;
using namespace LibretroRT_Tools;
using namespace Windows::Storage;

namespace BeetlePSXRT
{
	private ref class BeetlePSXCoreInternal sealed : public CoreBase
	{
	protected private:
		BeetlePSXCoreInternal();

	internal:
		virtual bool EnvironmentHandler(unsigned cmd, void *data) override;

	public:
		property unsigned int SerializationSize { unsigned int get() override; }

		static property BeetlePSXCoreInternal^ Instance { BeetlePSXCoreInternal^ get(); }
		virtual ~BeetlePSXCoreInternal();

		bool LoadGameInternal(IStorageFile^ gameFile) override;
		void UnloadGameInternal() override;
		void RunFrameInternal() override;
		void Reset() override;

		bool Serialize(WriteOnlyArray<uint8>^ stateData) override;
		bool Unserialize(const Array<uint8>^ stateData) override;
	};
}
﻿#pragma once

#include "../LibretroRT_Tools/CoreBase.h"

using namespace Platform;
using namespace LibretroRT_Tools;
using namespace Windows::Storage;

namespace GambatteRT
{
	private ref class GambatteCoreInternal sealed : public CoreBase
	{
	protected private:
		GambatteCoreInternal();

	internal:
		virtual bool EnvironmentHandler(unsigned cmd, void *data) override;

	public:
		static property GambatteCoreInternal^ Instance { GambatteCoreInternal^ get(); }
		virtual ~GambatteCoreInternal();
	};
}
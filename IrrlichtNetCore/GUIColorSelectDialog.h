#pragma once

#include "stdafx.h"
#include "GUIElement.h"

using namespace irr;
using namespace System;

namespace IrrlichtNetCore {
namespace GUI {

public ref class GUIColorSelectDialog : GUIElement
{
public:

	// nothing

internal:

	static GUIColorSelectDialog^ Wrap(gui::IGUIColorSelectDialog* ref);
	GUIColorSelectDialog(gui::IGUIColorSelectDialog* ref);

	gui::IGUIColorSelectDialog* m_GUIColorSelectDialog;
};

} // end namespace GUI
} // end namespace IrrlichtNetCore

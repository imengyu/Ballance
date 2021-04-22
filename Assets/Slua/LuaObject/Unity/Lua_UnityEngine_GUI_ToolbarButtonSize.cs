using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_GUI_ToolbarButtonSize : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.GUI.ToolbarButtonSize");
		addMember(l,0,"Fixed");
		addMember(l,1,"FitToContents");
		LuaDLL.lua_pop(l, 1);
	}
}

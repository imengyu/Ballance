using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_UI_WindowState : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.UI.WindowState");
		addMember(l,0,"Hidden");
		addMember(l,1,"Normal");
		addMember(l,2,"Max");
		addMember(l,3,"Min");
		LuaDLL.lua_pop(l, 1);
	}
}

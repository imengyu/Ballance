using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_UI_WindowType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.UI.WindowType");
		addMember(l,0,"Normal");
		addMember(l,1,"TopWindow");
		addMember(l,2,"GlobalAlert");
		addMember(l,3,"Page");
		LuaDLL.lua_pop(l, 1);
	}
}

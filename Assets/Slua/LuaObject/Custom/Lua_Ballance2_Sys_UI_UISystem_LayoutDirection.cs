using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_UI_UISystem_LayoutDirection : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.UI.UISystem.LayoutDirection");
		addMember(l,0,"LTR");
		addMember(l,1,"RTL");
		LuaDLL.lua_pop(l, 1);
	}
}

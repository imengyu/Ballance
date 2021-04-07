using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_UI_UISystem_UIVisible : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.UI.UISystem.UIVisible");
		addMember(l,0,"Visible");
		addMember(l,1,"Hidden");
		addMember(l,2,"Gone");
		LuaDLL.lua_pop(l, 1);
	}
}

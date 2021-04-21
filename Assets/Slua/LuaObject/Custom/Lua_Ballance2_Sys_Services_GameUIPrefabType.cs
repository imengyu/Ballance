using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_Services_GameUIPrefabType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.Services.GameUIPrefabType");
		addMember(l,0,"Control");
		addMember(l,1,"Page");
		LuaDLL.lua_pop(l, 1);
	}
}

using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_Package_GamePackageRunTime : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.Package.GamePackageRunTime");
		LuaDLL.lua_pop(l, 1);
	}
}

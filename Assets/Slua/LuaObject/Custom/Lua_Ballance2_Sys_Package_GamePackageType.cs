using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_Package_GamePackageType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.Package.GamePackageType");
		addMember(l,0,"Module");
		addMember(l,1,"Asset");
		LuaDLL.lua_pop(l, 1);
	}
}

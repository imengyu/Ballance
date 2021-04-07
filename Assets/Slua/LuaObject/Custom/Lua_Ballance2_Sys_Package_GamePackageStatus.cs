using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_Package_GamePackageStatus : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.Package.GamePackageStatus");
		addMember(l,0,"NotLoad");
		addMember(l,1,"Registing");
		addMember(l,2,"Loading");
		addMember(l,3,"LoadSuccess");
		addMember(l,4,"LoadFailed");
		addMember(l,5,"UnloadWaiting");
		addMember(l,6,"Registered");
		LuaDLL.lua_pop(l, 1);
	}
}

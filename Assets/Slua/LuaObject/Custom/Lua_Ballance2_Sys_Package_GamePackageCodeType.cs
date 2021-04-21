using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_Package_GamePackageCodeType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.Package.GamePackageCodeType");
		addMember(l,0,"None");
		addMember(l,1,"Lua");
		addMember(l,2,"CSharp");
		LuaDLL.lua_pop(l, 1);
	}
}

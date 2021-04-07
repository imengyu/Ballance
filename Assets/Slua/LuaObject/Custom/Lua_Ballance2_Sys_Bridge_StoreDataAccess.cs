using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_Bridge_StoreDataAccess : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.Bridge.StoreDataAccess");
		addMember(l,0,"Get");
		addMember(l,1,"GetAndSet");
		LuaDLL.lua_pop(l, 1);
	}
}

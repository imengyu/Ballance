using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Utils_LogLevel : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Utils.LogLevel");
		addMember(l,0,"None");
		addMember(l,1,"Verbose");
		addMember(l,2,"Debug");
		addMember(l,4,"Info");
		addMember(l,8,"Warning");
		addMember(l,16,"Error");
		addMember(l,31,"All");
		LuaDLL.lua_pop(l, 1);
	}
}

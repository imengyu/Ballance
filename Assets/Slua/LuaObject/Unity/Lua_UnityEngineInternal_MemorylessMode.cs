using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngineInternal_MemorylessMode : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngineInternal.MemorylessMode");
		addMember(l,0,"Unused");
		addMember(l,1,"Forced");
		addMember(l,2,"Automatic");
		LuaDLL.lua_pop(l, 1);
	}
}

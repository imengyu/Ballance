using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_PhysicsRT_PhysicsPhantomType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"PhysicsRT.PhysicsPhantomType");
		addMember(l,0,"Aabb");
		LuaDLL.lua_pop(l, 1);
	}
}

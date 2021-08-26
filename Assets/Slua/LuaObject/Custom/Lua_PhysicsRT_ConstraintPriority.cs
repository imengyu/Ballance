using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_PhysicsRT_ConstraintPriority : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"PhysicsRT.ConstraintPriority");
		addMember(l,1,"Psi");
		addMember(l,3,"Toi");
		LuaDLL.lua_pop(l, 1);
	}
}

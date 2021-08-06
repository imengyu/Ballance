using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_PhysicsRT_MotionType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"PhysicsRT.MotionType");
		addMember(l,0,"Dynamic");
		addMember(l,1,"SphereInertia");
		addMember(l,2,"BoxInertia");
		addMember(l,3,"Keyframed");
		addMember(l,4,"Fixed");
		addMember(l,5,"ThinBoxInertia");
		addMember(l,6,"Character");
		LuaDLL.lua_pop(l, 1);
	}
}

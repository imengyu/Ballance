using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_PhysicsRT_CollidableQualityType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"PhysicsRT.CollidableQualityType");
		addMember(l,0,"Fixed");
		addMember(l,1,"Keyframed");
		addMember(l,2,"Debris");
		addMember(l,3,"DebrisSimpleTOI");
		addMember(l,4,"Moving");
		addMember(l,5,"Critical");
		addMember(l,6,"Bullet");
		addMember(l,7,"User");
		addMember(l,8,"Character");
		addMember(l,9,"KeyframedReporting");
		addMember(l,-1,"Default");
		LuaDLL.lua_pop(l, 1);
	}
}

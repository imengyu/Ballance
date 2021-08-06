using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_PhysicsRT_ShapeType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"PhysicsRT.ShapeType");
		addMember(l,0,"Box");
		addMember(l,1,"Sphere");
		addMember(l,2,"Capsule");
		addMember(l,3,"Cylinder");
		addMember(l,4,"Plane");
		addMember(l,5,"ConvexHull");
		addMember(l,6,"Mesh");
		addMember(l,7,"List");
		addMember(l,8,"StaticCompound");
		LuaDLL.lua_pop(l, 1);
	}
}

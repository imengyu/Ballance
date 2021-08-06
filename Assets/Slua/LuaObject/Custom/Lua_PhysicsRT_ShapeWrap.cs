using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_PhysicsRT_ShapeWrap : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"PhysicsRT.ShapeWrap");
		addMember(l,0,"None");
		addMember(l,1,"TranslateShape");
		addMember(l,2,"TransformShape");
		LuaDLL.lua_pop(l, 1);
	}
}

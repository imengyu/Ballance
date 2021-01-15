using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_GraphicsBuffer_Target : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.GraphicsBuffer.Target");
		addMember(l,2,"Index");
		LuaDLL.lua_pop(l, 1);
	}
}

using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Game_Utils_SmoothFly_FlyType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Game.Utils.SmoothFly.FlyType");
		addMember(l,0,"SmoothDamp");
		addMember(l,1,"Lerp");
		LuaDLL.lua_pop(l, 1);
	}
}

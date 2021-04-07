using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_UI_Utils_UIAnchor : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.UI.Utils.UIAnchor");
		addMember(l,0,"None");
		addMember(l,1,"Top");
		addMember(l,2,"Center");
		addMember(l,3,"Bottom");
		addMember(l,4,"Left");
		addMember(l,5,"Right");
		addMember(l,6,"Stretch");
		LuaDLL.lua_pop(l, 1);
	}
}

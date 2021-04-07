using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_UI_UISystem_Layout_LayoutGravity : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.UI.UISystem.Layout.LayoutGravity");
		addMember(l,0,"None");
		addMember(l,1,"Top");
		addMember(l,2,"Bottom");
		addMember(l,4,"Start");
		addMember(l,8,"End");
		addMember(l,16,"CenterHorizontal");
		addMember(l,32,"CenterVertical");
		addMember(l,48,"Center");
		addMember(l,64,"Left");
		addMember(l,128,"Right");
		LuaDLL.lua_pop(l, 1);
	}
}

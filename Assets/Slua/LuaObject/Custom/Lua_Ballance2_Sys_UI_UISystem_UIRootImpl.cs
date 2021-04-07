using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_UI_UISystem_UIRootImpl : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Ballance2.Sys.UI.UISystem.UIRootImpl");
		createTypeMetatable(l,null, typeof(Ballance2.Sys.UI.UISystem.UIRootImpl),typeof(Ballance2.Sys.UI.UISystem.UIElement));
	}
}

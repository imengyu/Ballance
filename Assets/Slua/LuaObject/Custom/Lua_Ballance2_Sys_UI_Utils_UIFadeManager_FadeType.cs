using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_UI_Utils_UIFadeManager_FadeType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.UI.Utils.UIFadeManager.FadeType");
		addMember(l,0,"None");
		addMember(l,1,"FadeIn");
		addMember(l,2,"FadeOut");
		LuaDLL.lua_pop(l, 1);
	}
}

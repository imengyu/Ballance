using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_UI_Utils_UIPivot : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.UI.Utils.UIPivot");
		addMember(l,0,"None");
		addMember(l,1,"Top");
		addMember(l,2,"Center");
		addMember(l,3,"TopCenter");
		addMember(l,4,"Bottom");
		addMember(l,6,"BottomCenter");
		addMember(l,8,"Left");
		addMember(l,9,"TopLeft");
		addMember(l,10,"CenterLeft");
		addMember(l,12,"BottomLeft");
		addMember(l,16,"Right");
		addMember(l,17,"TopRight");
		addMember(l,18,"CenterRight");
		addMember(l,20,"BottomRight");
		LuaDLL.lua_pop(l, 1);
	}
}

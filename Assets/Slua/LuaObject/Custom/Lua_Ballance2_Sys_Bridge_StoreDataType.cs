using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_Bridge_StoreDataType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.Bridge.StoreDataType");
		addMember(l,0,"NotSet");
		addMember(l,1,"Custom");
		addMember(l,2,"Array");
		addMember(l,3,"Integer");
		addMember(l,4,"Long");
		addMember(l,5,"Float");
		addMember(l,6,"String");
		addMember(l,7,"Boolean");
		addMember(l,8,"Double");
		addMember(l,9,"Color");
		addMember(l,10,"Material");
		addMember(l,11,"Texture");
		addMember(l,12,"Texture2D");
		addMember(l,13,"Vector2");
		addMember(l,14,"Vector3");
		addMember(l,15,"Vector4");
		addMember(l,16,"Quaternion");
		addMember(l,17,"Sprite");
		addMember(l,18,"Rigidbody");
		addMember(l,19,"RectTransform");
		addMember(l,20,"Transform");
		addMember(l,21,"Camera");
		addMember(l,22,"GameObject");
		addMember(l,23,"Object");
		addMember(l,24,"AudioClip");
		addMember(l,25,"AudioSource");
		addMember(l,26,"MonoBehaviour");
		LuaDLL.lua_pop(l, 1);
	}
}

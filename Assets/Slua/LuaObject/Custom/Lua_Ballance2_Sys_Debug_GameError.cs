using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_Debug_GameError : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"Ballance2.Sys.Debug.GameError");
		addMember(l,0,"None");
		addMember(l,1,"UnKnow");
		addMember(l,2,"AlreadyRegistered");
		addMember(l,3,"NotRegister");
		addMember(l,4,"NotLoad");
		addMember(l,5,"ConfigueNotRight");
		addMember(l,6,"NotImplemented");
		addMember(l,7,"ContextMismatch");
		addMember(l,8,"ParamNotProvide");
		addMember(l,9,"ParamNotFound");
		addMember(l,10,"ParamReadOnly");
		addMember(l,11,"Empty");
		addMember(l,12,"MissingAttribute");
		addMember(l,13,"PackageCanNotRun");
		addMember(l,14,"PackageIncompatible");
		addMember(l,15,"ClassNotFound");
		addMember(l,16,"FileNotFound");
		addMember(l,17,"FunctionNotFound");
		addMember(l,18,"PackageDefNotFound");
		addMember(l,19,"AssetBundleNotFound");
		addMember(l,20,"AssetNotFound");
		addMember(l,21,"FileReadFailed");
		addMember(l,22,"NotReturn");
		addMember(l,23,"InvalidPackageName");
		addMember(l,24,"RegisterPackageFailed");
		addMember(l,25,"NotSupportFileType");
		addMember(l,26,"NetworkError");
		addMember(l,27,"ExecutionFailed");
		addMember(l,28,"AccessDenined");
		addMember(l,29,"IsLoading");
		addMember(l,30,"SystemPackageNotLoad");
		addMember(l,31,"UnKnowType");
		addMember(l,32,"LuaBindCheckFailed");
		addMember(l,33,"OnlyCanUseInEditor");
		addMember(l,34,"PrefabNotFound");
		LuaDLL.lua_pop(l, 1);
	}
}

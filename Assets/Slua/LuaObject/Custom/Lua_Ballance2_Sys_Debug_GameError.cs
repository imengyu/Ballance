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
		addMember(l,9,"Empty");
		addMember(l,10,"MissingAttribute");
		addMember(l,11,"PackageCanNotRun");
		addMember(l,12,"PackageIncompatible");
		addMember(l,13,"ClassNotFound");
		addMember(l,14,"FileNotFound");
		addMember(l,15,"FunctionNotFound");
		addMember(l,16,"PackageDefNotFound");
		addMember(l,17,"AssetBundleNotFound");
		addMember(l,18,"FileReadFailed");
		addMember(l,19,"NotReturn");
		addMember(l,20,"InvalidPackageName");
		addMember(l,21,"RegisterPackageFailed");
		addMember(l,22,"NotSupportFileType");
		addMember(l,23,"NetworkError");
		addMember(l,24,"ExecutionFailed");
		addMember(l,25,"AccessDenined");
		addMember(l,26,"IsLoading");
		addMember(l,27,"SystemPackageNotLoad");
		addMember(l,28,"UnKnowType");
		addMember(l,29,"LuaBindCheckFailed");
		addMember(l,30,"OnlyCanUseInEditor");
		LuaDLL.lua_pop(l, 1);
	}
}

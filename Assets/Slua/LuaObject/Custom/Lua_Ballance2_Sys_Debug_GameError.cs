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
		addMember(l,20,"FileReadFailed");
		addMember(l,21,"NotReturn");
		addMember(l,22,"InvalidPackageName");
		addMember(l,23,"RegisterPackageFailed");
		addMember(l,24,"NotSupportFileType");
		addMember(l,25,"NetworkError");
		addMember(l,26,"ExecutionFailed");
		addMember(l,27,"AccessDenined");
		addMember(l,28,"IsLoading");
		addMember(l,29,"SystemPackageNotLoad");
		addMember(l,30,"UnKnowType");
		addMember(l,31,"LuaBindCheckFailed");
		addMember(l,32,"OnlyCanUseInEditor");
		LuaDLL.lua_pop(l, 1);
	}
}

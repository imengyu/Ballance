using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_Bridge_Lua_LuaGlobalApi : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int PackageNameByLuaFilePath_s(IntPtr l) {
		try {
			#if DEBUG
			var method = System.Reflection.MethodBase.GetCurrentMethod();
			string methodName = GetMethodName(method);
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.BeginSample(methodName);
			#else
			Profiler.BeginSample(methodName);
			#endif
			#endif
			System.String a1;
			checkType(l,1,out a1);
			var ret=Ballance2.Sys.Bridge.Lua.LuaGlobalApi.PackageNameByLuaFilePath(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
		#if DEBUG
		finally {
			#if UNITY_5_5_OR_NEWER
			UnityEngine.Profiling.Profiler.EndSample();
			#else
			Profiler.EndSample();
			#endif
		}
		#endif
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Ballance2.Sys.Bridge.Lua.LuaGlobalApi");
		addMember(l,PackageNameByLuaFilePath_s);
		addMember(l,Ballance2.Sys.Bridge.Lua.LuaGlobalApi.require,false);
		createTypeMetatable(l,null, typeof(Ballance2.Sys.Bridge.Lua.LuaGlobalApi));
	}
}

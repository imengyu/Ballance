using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_Bridge_LuaWapper_GameLuaWapperEvents_GameLuaObjecOnGUICaller : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetEventType(IntPtr l) {
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
			Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjecOnGUICaller self=(Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjecOnGUICaller)checkSelf(l);
			var ret=self.GetEventType();
			pushValue(l,true);
			pushEnum(l,(int)ret);
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetSupportEvents(IntPtr l) {
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
			Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjecOnGUICaller self=(Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjecOnGUICaller)checkSelf(l);
			var ret=self.GetSupportEvents();
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
		getTypeTable(l,"Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjecOnGUICaller");
		addMember(l,GetEventType);
		addMember(l,GetSupportEvents);
		createTypeMetatable(l,null, typeof(Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjecOnGUICaller),typeof(Ballance2.Sys.Bridge.LuaWapper.GameLuaWapperEvents.GameLuaObjectEventCaller));
	}
}

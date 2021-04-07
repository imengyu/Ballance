using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_UI_UISystem_Layout_UILayoutUtils : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GravityToAnchor_s(IntPtr l) {
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
			Ballance2.Sys.UI.UISystem.Layout.LayoutGravity a1;
			a1 = (Ballance2.Sys.UI.UISystem.Layout.LayoutGravity)LuaDLL.luaL_checkinteger(l, 1);
			UnityEngine.RectTransform.Axis a2;
			a2 = (UnityEngine.RectTransform.Axis)LuaDLL.luaL_checkinteger(l, 2);
			var ret=Ballance2.Sys.UI.UISystem.Layout.UILayoutUtils.GravityToAnchor(a1,a2);
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
	static public int AnchorToPivot_s(IntPtr l) {
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
			Ballance2.Sys.UI.Utils.UIAnchor a1;
			a1 = (Ballance2.Sys.UI.Utils.UIAnchor)LuaDLL.luaL_checkinteger(l, 1);
			UnityEngine.RectTransform.Axis a2;
			a2 = (UnityEngine.RectTransform.Axis)LuaDLL.luaL_checkinteger(l, 2);
			var ret=Ballance2.Sys.UI.UISystem.Layout.UILayoutUtils.AnchorToPivot(a1,a2);
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"Ballance2.Sys.UI.UISystem.Layout.UILayoutUtils");
		addMember(l,GravityToAnchor_s);
		addMember(l,AnchorToPivot_s);
		createTypeMetatable(l,null, typeof(Ballance2.Sys.UI.UISystem.Layout.UILayoutUtils));
	}
}

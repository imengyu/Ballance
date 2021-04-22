using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_GUILayout_ScrollViewScope : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int constructor(IntPtr l) {
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
			int argc = LuaDLL.lua_gettop(l);
			UnityEngine.GUILayout.ScrollViewScope o;
			if(argc==3){
				UnityEngine.Vector2 a1;
				checkType(l,2,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,3,out a2);
				o=new UnityEngine.GUILayout.ScrollViewScope(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Vector2),typeof(bool),typeof(bool),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Vector2 a1;
				checkType(l,2,out a1);
				System.Boolean a2;
				checkType(l,3,out a2);
				System.Boolean a3;
				checkType(l,4,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,5,out a4);
				o=new UnityEngine.GUILayout.ScrollViewScope(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Vector2),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Vector2 a1;
				checkType(l,2,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,3,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,4,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,5,out a4);
				o=new UnityEngine.GUILayout.ScrollViewScope(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==4){
				UnityEngine.Vector2 a1;
				checkType(l,2,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,3,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,4,out a3);
				o=new UnityEngine.GUILayout.ScrollViewScope(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==7){
				UnityEngine.Vector2 a1;
				checkType(l,2,out a1);
				System.Boolean a2;
				checkType(l,3,out a2);
				System.Boolean a3;
				checkType(l,4,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,5,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,6,out a5);
				UnityEngine.GUILayoutOption[] a6;
				checkParams(l,7,out a6);
				o=new UnityEngine.GUILayout.ScrollViewScope(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==8){
				UnityEngine.Vector2 a1;
				checkType(l,2,out a1);
				System.Boolean a2;
				checkType(l,3,out a2);
				System.Boolean a3;
				checkType(l,4,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,5,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,6,out a5);
				UnityEngine.GUIStyle a6;
				checkType(l,7,out a6);
				UnityEngine.GUILayoutOption[] a7;
				checkParams(l,8,out a7);
				o=new UnityEngine.GUILayout.ScrollViewScope(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			return error(l,"New object failed.");
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
	static public int get_scrollPosition(IntPtr l) {
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
			UnityEngine.GUILayout.ScrollViewScope self=(UnityEngine.GUILayout.ScrollViewScope)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.scrollPosition);
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
	static public int get_handleScrollWheel(IntPtr l) {
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
			UnityEngine.GUILayout.ScrollViewScope self=(UnityEngine.GUILayout.ScrollViewScope)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.handleScrollWheel);
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
	static public int set_handleScrollWheel(IntPtr l) {
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
			UnityEngine.GUILayout.ScrollViewScope self=(UnityEngine.GUILayout.ScrollViewScope)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.handleScrollWheel=v;
			pushValue(l,true);
			return 1;
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
		getTypeTable(l,"UnityEngine.GUILayout.ScrollViewScope");
		addMember(l,"scrollPosition",get_scrollPosition,null,true);
		addMember(l,"handleScrollWheel",get_handleScrollWheel,set_handleScrollWheel,true);
		createTypeMetatable(l,constructor, typeof(UnityEngine.GUILayout.ScrollViewScope),typeof(UnityEngine.GUI.Scope));
	}
}

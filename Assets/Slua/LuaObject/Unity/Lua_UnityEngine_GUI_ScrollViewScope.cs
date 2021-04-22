using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_GUI_ScrollViewScope : LuaObject {
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
			UnityEngine.GUI.ScrollViewScope o;
			if(argc==4){
				UnityEngine.Rect a1;
				checkValueType(l,2,out a1);
				UnityEngine.Vector2 a2;
				checkType(l,3,out a2);
				UnityEngine.Rect a3;
				checkValueType(l,4,out a3);
				o=new UnityEngine.GUI.ScrollViewScope(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rect),typeof(UnityEngine.Vector2),typeof(UnityEngine.Rect),typeof(bool),typeof(bool))){
				UnityEngine.Rect a1;
				checkValueType(l,2,out a1);
				UnityEngine.Vector2 a2;
				checkType(l,3,out a2);
				UnityEngine.Rect a3;
				checkValueType(l,4,out a3);
				System.Boolean a4;
				checkType(l,5,out a4);
				System.Boolean a5;
				checkType(l,6,out a5);
				o=new UnityEngine.GUI.ScrollViewScope(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rect),typeof(UnityEngine.Vector2),typeof(UnityEngine.Rect),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,2,out a1);
				UnityEngine.Vector2 a2;
				checkType(l,3,out a2);
				UnityEngine.Rect a3;
				checkValueType(l,4,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,5,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,6,out a5);
				o=new UnityEngine.GUI.ScrollViewScope(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==8){
				UnityEngine.Rect a1;
				checkValueType(l,2,out a1);
				UnityEngine.Vector2 a2;
				checkType(l,3,out a2);
				UnityEngine.Rect a3;
				checkValueType(l,4,out a3);
				System.Boolean a4;
				checkType(l,5,out a4);
				System.Boolean a5;
				checkType(l,6,out a5);
				UnityEngine.GUIStyle a6;
				checkType(l,7,out a6);
				UnityEngine.GUIStyle a7;
				checkType(l,8,out a7);
				o=new UnityEngine.GUI.ScrollViewScope(a1,a2,a3,a4,a5,a6,a7);
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
			UnityEngine.GUI.ScrollViewScope self=(UnityEngine.GUI.ScrollViewScope)checkSelf(l);
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
			UnityEngine.GUI.ScrollViewScope self=(UnityEngine.GUI.ScrollViewScope)checkSelf(l);
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
			UnityEngine.GUI.ScrollViewScope self=(UnityEngine.GUI.ScrollViewScope)checkSelf(l);
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
		getTypeTable(l,"UnityEngine.GUI.ScrollViewScope");
		addMember(l,"scrollPosition",get_scrollPosition,null,true);
		addMember(l,"handleScrollWheel",get_handleScrollWheel,set_handleScrollWheel,true);
		createTypeMetatable(l,constructor, typeof(UnityEngine.GUI.ScrollViewScope),typeof(UnityEngine.GUI.Scope));
	}
}

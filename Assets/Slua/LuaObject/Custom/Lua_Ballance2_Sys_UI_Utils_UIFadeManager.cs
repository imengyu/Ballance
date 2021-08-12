using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_UI_Utils_UIFadeManager : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddFadeOut(IntPtr l) {
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
			if(matchType(l,argc,2,typeof(UnityEngine.UI.Image),typeof(float),typeof(bool))){
				Ballance2.Sys.UI.Utils.UIFadeManager self=(Ballance2.Sys.UI.Utils.UIFadeManager)checkSelf(l);
				UnityEngine.UI.Image a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Boolean a3;
				checkType(l,4,out a3);
				var ret=self.AddFadeOut(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.UI.Text),typeof(float),typeof(bool))){
				Ballance2.Sys.UI.Utils.UIFadeManager self=(Ballance2.Sys.UI.Utils.UIFadeManager)checkSelf(l);
				UnityEngine.UI.Text a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Boolean a3;
				checkType(l,4,out a3);
				var ret=self.AddFadeOut(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==5){
				Ballance2.Sys.UI.Utils.UIFadeManager self=(Ballance2.Sys.UI.Utils.UIFadeManager)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				System.Boolean a3;
				checkType(l,4,out a3);
				UnityEngine.Material a4;
				checkType(l,5,out a4);
				var ret=self.AddFadeOut(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function AddFadeOut to call");
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
	static public int AddFadeIn(IntPtr l) {
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
			if(matchType(l,argc,2,typeof(UnityEngine.UI.Image),typeof(float))){
				Ballance2.Sys.UI.Utils.UIFadeManager self=(Ballance2.Sys.UI.Utils.UIFadeManager)checkSelf(l);
				UnityEngine.UI.Image a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				var ret=self.AddFadeIn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.UI.Text),typeof(float))){
				Ballance2.Sys.UI.Utils.UIFadeManager self=(Ballance2.Sys.UI.Utils.UIFadeManager)checkSelf(l);
				UnityEngine.UI.Text a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				var ret=self.AddFadeIn(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				Ballance2.Sys.UI.Utils.UIFadeManager self=(Ballance2.Sys.UI.Utils.UIFadeManager)checkSelf(l);
				UnityEngine.GameObject a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				UnityEngine.Material a3;
				checkType(l,4,out a3);
				var ret=self.AddFadeIn(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function AddFadeIn to call");
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
	static public int AddFadeOut2(IntPtr l) {
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
			Ballance2.Sys.UI.Utils.UIFadeManager self=(Ballance2.Sys.UI.Utils.UIFadeManager)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			UnityEngine.Material[] a4;
			checkArray(l,5,out a4);
			var ret=self.AddFadeOut2(a1,a2,a3,a4);
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddFadeIn2(IntPtr l) {
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
			Ballance2.Sys.UI.Utils.UIFadeManager self=(Ballance2.Sys.UI.Utils.UIFadeManager)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			UnityEngine.Material[] a3;
			checkArray(l,4,out a3);
			var ret=self.AddFadeIn2(a1,a2,a3);
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
		getTypeTable(l,"Ballance2.Sys.UI.Utils.UIFadeManager");
		addMember(l,AddFadeOut);
		addMember(l,AddFadeIn);
		addMember(l,AddFadeOut2);
		addMember(l,AddFadeIn2);
		createTypeMetatable(l,null, typeof(Ballance2.Sys.UI.Utils.UIFadeManager),typeof(UnityEngine.MonoBehaviour));
	}
}

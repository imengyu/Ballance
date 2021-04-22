using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_GUI : LuaObject {
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
			UnityEngine.GUI o;
			o=new UnityEngine.GUI();
			pushValue(l,true);
			pushValue(l,o);
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
	static public int SetNextControlName_s(IntPtr l) {
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
			UnityEngine.GUI.SetNextControlName(a1);
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetNameOfFocusedControl_s(IntPtr l) {
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
			var ret=UnityEngine.GUI.GetNameOfFocusedControl();
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
	static public int FocusControl_s(IntPtr l) {
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
			UnityEngine.GUI.FocusControl(a1);
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int DragWindow_s(IntPtr l) {
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
			if(argc==0){
				UnityEngine.GUI.DragWindow();
				pushValue(l,true);
				return 1;
			}
			else if(argc==1){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUI.DragWindow(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function DragWindow to call");
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
	static public int BringWindowToFront_s(IntPtr l) {
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
			System.Int32 a1;
			checkType(l,1,out a1);
			UnityEngine.GUI.BringWindowToFront(a1);
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int BringWindowToBack_s(IntPtr l) {
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
			System.Int32 a1;
			checkType(l,1,out a1);
			UnityEngine.GUI.BringWindowToBack(a1);
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FocusWindow_s(IntPtr l) {
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
			System.Int32 a1;
			checkType(l,1,out a1);
			UnityEngine.GUI.FocusWindow(a1);
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int UnfocusWindow_s(IntPtr l) {
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
			UnityEngine.GUI.UnfocusWindow();
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Label_s(IntPtr l) {
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
			if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UnityEngine.GUI.Label(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.GUI.Label(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIContent))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,2,out a2);
				UnityEngine.GUI.Label(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUI.Label(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUI.Label(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUI.Label(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Label to call");
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
	static public int DrawTexture_s(IntPtr l) {
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
			if(argc==2){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.GUI.DrawTexture(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.ScaleMode a3;
				a3 = (UnityEngine.ScaleMode)LuaDLL.luaL_checkinteger(l, 3);
				UnityEngine.GUI.DrawTexture(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.ScaleMode a3;
				a3 = (UnityEngine.ScaleMode)LuaDLL.luaL_checkinteger(l, 3);
				System.Boolean a4;
				checkType(l,4,out a4);
				UnityEngine.GUI.DrawTexture(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			else if(argc==5){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.ScaleMode a3;
				a3 = (UnityEngine.ScaleMode)LuaDLL.luaL_checkinteger(l, 3);
				System.Boolean a4;
				checkType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				UnityEngine.GUI.DrawTexture(a1,a2,a3,a4,a5);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture),typeof(UnityEngine.ScaleMode),typeof(bool),typeof(float),typeof(UnityEngine.Color),typeof(float),typeof(float))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.ScaleMode a3;
				a3 = (UnityEngine.ScaleMode)LuaDLL.luaL_checkinteger(l, 3);
				System.Boolean a4;
				checkType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				UnityEngine.Color a6;
				checkType(l,6,out a6);
				System.Single a7;
				checkType(l,7,out a7);
				System.Single a8;
				checkType(l,8,out a8);
				UnityEngine.GUI.DrawTexture(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture),typeof(UnityEngine.ScaleMode),typeof(bool),typeof(float),typeof(UnityEngine.Color),typeof(UnityEngine.Vector4),typeof(float))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.ScaleMode a3;
				a3 = (UnityEngine.ScaleMode)LuaDLL.luaL_checkinteger(l, 3);
				System.Boolean a4;
				checkType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				UnityEngine.Color a6;
				checkType(l,6,out a6);
				UnityEngine.Vector4 a7;
				checkType(l,7,out a7);
				System.Single a8;
				checkType(l,8,out a8);
				UnityEngine.GUI.DrawTexture(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture),typeof(UnityEngine.ScaleMode),typeof(bool),typeof(float),typeof(UnityEngine.Color),typeof(UnityEngine.Vector4),typeof(UnityEngine.Vector4))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.ScaleMode a3;
				a3 = (UnityEngine.ScaleMode)LuaDLL.luaL_checkinteger(l, 3);
				System.Boolean a4;
				checkType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				UnityEngine.Color a6;
				checkType(l,6,out a6);
				UnityEngine.Vector4 a7;
				checkType(l,7,out a7);
				UnityEngine.Vector4 a8;
				checkType(l,8,out a8);
				UnityEngine.GUI.DrawTexture(a1,a2,a3,a4,a5,a6,a7,a8);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function DrawTexture to call");
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
	static public int DrawTextureWithTexCoords_s(IntPtr l) {
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
			if(argc==3){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.Rect a3;
				checkValueType(l,3,out a3);
				UnityEngine.GUI.DrawTextureWithTexCoords(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.Rect a3;
				checkValueType(l,3,out a3);
				System.Boolean a4;
				checkType(l,4,out a4);
				UnityEngine.GUI.DrawTextureWithTexCoords(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function DrawTextureWithTexCoords to call");
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
	static public int Box_s(IntPtr l) {
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
			if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UnityEngine.GUI.Box(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.GUI.Box(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIContent))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,2,out a2);
				UnityEngine.GUI.Box(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUI.Box(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUI.Box(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUI.Box(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Box to call");
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
	static public int Button_s(IntPtr l) {
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
			if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=UnityEngine.GUI.Button(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				var ret=UnityEngine.GUI.Button(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIContent))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,2,out a2);
				var ret=UnityEngine.GUI.Button(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.Button(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.Button(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.Button(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Button to call");
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
	static public int RepeatButton_s(IntPtr l) {
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
			if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=UnityEngine.GUI.RepeatButton(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				var ret=UnityEngine.GUI.RepeatButton(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIContent))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,2,out a2);
				var ret=UnityEngine.GUI.RepeatButton(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.RepeatButton(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.RepeatButton(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.RepeatButton(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function RepeatButton to call");
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
	static public int TextField_s(IntPtr l) {
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
			if(argc==2){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=UnityEngine.GUI.TextField(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string),typeof(int))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.TextField(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.TextField(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.TextField(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function TextField to call");
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
	static public int PasswordField_s(IntPtr l) {
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
			if(argc==3){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				System.Char a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.PasswordField(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string),typeof(System.Char),typeof(int))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				System.Char a3;
				checkType(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.PasswordField(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string),typeof(System.Char),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				System.Char a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.PasswordField(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==5){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				System.Char a3;
				checkType(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.PasswordField(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function PasswordField to call");
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
	static public int TextArea_s(IntPtr l) {
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
			if(argc==2){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=UnityEngine.GUI.TextArea(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string),typeof(int))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.TextArea(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.TextArea(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.TextArea(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function TextArea to call");
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
	static public int Toggle_s(IntPtr l) {
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
			if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(bool),typeof(string))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				System.String a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.Toggle(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(bool),typeof(UnityEngine.Texture))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				UnityEngine.Texture a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.Toggle(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(bool),typeof(UnityEngine.GUIContent))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				UnityEngine.GUIContent a3;
				checkType(l,3,out a3);
				var ret=UnityEngine.GUI.Toggle(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(bool),typeof(string),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				System.String a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.Toggle(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(bool),typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				UnityEngine.Texture a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.Toggle(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(bool),typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				UnityEngine.GUIContent a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.Toggle(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==5){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.Boolean a3;
				checkType(l,3,out a3);
				UnityEngine.GUIContent a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.Toggle(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Toggle to call");
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
	static public int Toolbar_s(IntPtr l) {
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
			if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(int),typeof(System.String[]))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.String[] a3;
				checkArray(l,3,out a3);
				var ret=UnityEngine.GUI.Toolbar(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(int),typeof(UnityEngine.Texture[]))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UnityEngine.Texture[] a3;
				checkArray(l,3,out a3);
				var ret=UnityEngine.GUI.Toolbar(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(int),typeof(UnityEngine.GUIContent[]))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UnityEngine.GUIContent[] a3;
				checkArray(l,3,out a3);
				var ret=UnityEngine.GUI.Toolbar(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(int),typeof(System.String[]),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.String[] a3;
				checkArray(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.Toolbar(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(int),typeof(UnityEngine.Texture[]),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UnityEngine.Texture[] a3;
				checkArray(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.Toolbar(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(int),typeof(UnityEngine.GUIContent[]),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UnityEngine.GUIContent[] a3;
				checkArray(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.Toolbar(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==5){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UnityEngine.GUIContent[] a3;
				checkArray(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				UnityEngine.GUI.ToolbarButtonSize a5;
				a5 = (UnityEngine.GUI.ToolbarButtonSize)LuaDLL.luaL_checkinteger(l, 5);
				var ret=UnityEngine.GUI.Toolbar(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Toolbar to call");
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
	static public int SelectionGrid_s(IntPtr l) {
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
			if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(int),typeof(System.String[]),typeof(int))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.String[] a3;
				checkArray(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.SelectionGrid(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(int),typeof(UnityEngine.Texture[]),typeof(int))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UnityEngine.Texture[] a3;
				checkArray(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.SelectionGrid(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(int),typeof(UnityEngine.GUIContent[]),typeof(int))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UnityEngine.GUIContent[] a3;
				checkArray(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.SelectionGrid(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(int),typeof(System.String[]),typeof(int),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				System.String[] a3;
				checkArray(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.SelectionGrid(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(int),typeof(UnityEngine.Texture[]),typeof(int),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UnityEngine.Texture[] a3;
				checkArray(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.SelectionGrid(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(int),typeof(UnityEngine.GUIContent[]),typeof(int),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UnityEngine.GUIContent[] a3;
				checkArray(l,3,out a3);
				System.Int32 a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.SelectionGrid(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function SelectionGrid to call");
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
	static public int HorizontalSlider_s(IntPtr l) {
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
			if(argc==4){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.HorizontalSlider(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				UnityEngine.GUIStyle a6;
				checkType(l,6,out a6);
				var ret=UnityEngine.GUI.HorizontalSlider(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==7){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				UnityEngine.GUIStyle a6;
				checkType(l,6,out a6);
				UnityEngine.GUIStyle a7;
				checkType(l,7,out a7);
				var ret=UnityEngine.GUI.HorizontalSlider(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function HorizontalSlider to call");
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
	static public int VerticalSlider_s(IntPtr l) {
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
			if(argc==4){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.VerticalSlider(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				UnityEngine.GUIStyle a6;
				checkType(l,6,out a6);
				var ret=UnityEngine.GUI.VerticalSlider(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==7){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				UnityEngine.GUIStyle a6;
				checkType(l,6,out a6);
				UnityEngine.GUIStyle a7;
				checkType(l,7,out a7);
				var ret=UnityEngine.GUI.VerticalSlider(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function VerticalSlider to call");
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
	static public int Slider_s(IntPtr l) {
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
			UnityEngine.Rect a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			System.Single a5;
			checkType(l,5,out a5);
			UnityEngine.GUIStyle a6;
			checkType(l,6,out a6);
			UnityEngine.GUIStyle a7;
			checkType(l,7,out a7);
			System.Boolean a8;
			checkType(l,8,out a8);
			System.Int32 a9;
			checkType(l,9,out a9);
			UnityEngine.GUIStyle a10;
			checkType(l,10,out a10);
			var ret=UnityEngine.GUI.Slider(a1,a2,a3,a4,a5,a6,a7,a8,a9,a10);
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
	static public int HorizontalScrollbar_s(IntPtr l) {
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
			if(argc==5){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.HorizontalScrollbar(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				UnityEngine.GUIStyle a6;
				checkType(l,6,out a6);
				var ret=UnityEngine.GUI.HorizontalScrollbar(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function HorizontalScrollbar to call");
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
	static public int VerticalScrollbar_s(IntPtr l) {
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
			if(argc==5){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.VerticalScrollbar(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				System.Single a5;
				checkType(l,5,out a5);
				UnityEngine.GUIStyle a6;
				checkType(l,6,out a6);
				var ret=UnityEngine.GUI.VerticalScrollbar(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function VerticalScrollbar to call");
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
	static public int BeginClip_s(IntPtr l) {
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
			if(argc==1){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUI.BeginClip(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Vector2 a2;
				checkType(l,2,out a2);
				UnityEngine.Vector2 a3;
				checkType(l,3,out a3);
				System.Boolean a4;
				checkType(l,4,out a4);
				UnityEngine.GUI.BeginClip(a1,a2,a3,a4);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function BeginClip to call");
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
	static public int BeginGroup_s(IntPtr l) {
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
			if(argc==1){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUI.BeginGroup(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UnityEngine.GUI.BeginGroup(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.GUI.BeginGroup(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIContent))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,2,out a2);
				UnityEngine.GUI.BeginGroup(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUI.BeginGroup(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUI.BeginGroup(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUI.BeginGroup(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUI.BeginGroup(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function BeginGroup to call");
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
	static public int EndGroup_s(IntPtr l) {
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
			UnityEngine.GUI.EndGroup();
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int EndClip_s(IntPtr l) {
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
			UnityEngine.GUI.EndClip();
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int BeginScrollView_s(IntPtr l) {
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
			if(argc==3){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Vector2 a2;
				checkType(l,2,out a2);
				UnityEngine.Rect a3;
				checkValueType(l,3,out a3);
				var ret=UnityEngine.GUI.BeginScrollView(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Vector2),typeof(UnityEngine.Rect),typeof(bool),typeof(bool))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Vector2 a2;
				checkType(l,2,out a2);
				UnityEngine.Rect a3;
				checkValueType(l,3,out a3);
				System.Boolean a4;
				checkType(l,4,out a4);
				System.Boolean a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.BeginScrollView(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Vector2),typeof(UnityEngine.Rect),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Vector2 a2;
				checkType(l,2,out a2);
				UnityEngine.Rect a3;
				checkValueType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.BeginScrollView(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==7){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Vector2 a2;
				checkType(l,2,out a2);
				UnityEngine.Rect a3;
				checkValueType(l,3,out a3);
				System.Boolean a4;
				checkType(l,4,out a4);
				System.Boolean a5;
				checkType(l,5,out a5);
				UnityEngine.GUIStyle a6;
				checkType(l,6,out a6);
				UnityEngine.GUIStyle a7;
				checkType(l,7,out a7);
				var ret=UnityEngine.GUI.BeginScrollView(a1,a2,a3,a4,a5,a6,a7);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function BeginScrollView to call");
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
	static public int EndScrollView_s(IntPtr l) {
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
			if(argc==0){
				UnityEngine.GUI.EndScrollView();
				pushValue(l,true);
				return 1;
			}
			else if(argc==1){
				System.Boolean a1;
				checkType(l,1,out a1);
				UnityEngine.GUI.EndScrollView(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function EndScrollView to call");
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
	static public int ScrollTo_s(IntPtr l) {
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
			UnityEngine.Rect a1;
			checkValueType(l,1,out a1);
			UnityEngine.GUI.ScrollTo(a1);
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ScrollTowards_s(IntPtr l) {
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
			UnityEngine.Rect a1;
			checkValueType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			var ret=UnityEngine.GUI.ScrollTowards(a1,a2);
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
	static public int Window_s(IntPtr l) {
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
			if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(string))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				System.String a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.Window(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(UnityEngine.Texture))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				UnityEngine.Texture a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.Window(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(UnityEngine.GUIContent))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				UnityEngine.GUIContent a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.Window(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(string),typeof(UnityEngine.GUIStyle))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				System.String a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.Window(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				UnityEngine.Texture a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.Window(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				UnityEngine.GUIContent a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.Window(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function Window to call");
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
	static public int ModalWindow_s(IntPtr l) {
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
			if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(string))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				System.String a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.ModalWindow(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(UnityEngine.Texture))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				UnityEngine.Texture a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.ModalWindow(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(UnityEngine.GUIContent))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				UnityEngine.GUIContent a4;
				checkType(l,4,out a4);
				var ret=UnityEngine.GUI.ModalWindow(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(string),typeof(UnityEngine.GUIStyle))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				System.String a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.ModalWindow(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				UnityEngine.Texture a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.ModalWindow(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				UnityEngine.GUIContent a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				var ret=UnityEngine.GUI.ModalWindow(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function ModalWindow to call");
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
	static public int get_color(IntPtr l) {
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
			pushValue(l,true);
			pushValue(l,UnityEngine.GUI.color);
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
	static public int set_color(IntPtr l) {
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
			UnityEngine.Color v;
			checkType(l,2,out v);
			UnityEngine.GUI.color=v;
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_backgroundColor(IntPtr l) {
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
			pushValue(l,true);
			pushValue(l,UnityEngine.GUI.backgroundColor);
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
	static public int set_backgroundColor(IntPtr l) {
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
			UnityEngine.Color v;
			checkType(l,2,out v);
			UnityEngine.GUI.backgroundColor=v;
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_contentColor(IntPtr l) {
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
			pushValue(l,true);
			pushValue(l,UnityEngine.GUI.contentColor);
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
	static public int set_contentColor(IntPtr l) {
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
			UnityEngine.Color v;
			checkType(l,2,out v);
			UnityEngine.GUI.contentColor=v;
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_changed(IntPtr l) {
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
			pushValue(l,true);
			pushValue(l,UnityEngine.GUI.changed);
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
	static public int set_changed(IntPtr l) {
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
			bool v;
			checkType(l,2,out v);
			UnityEngine.GUI.changed=v;
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_enabled(IntPtr l) {
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
			pushValue(l,true);
			pushValue(l,UnityEngine.GUI.enabled);
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
	static public int set_enabled(IntPtr l) {
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
			bool v;
			checkType(l,2,out v);
			UnityEngine.GUI.enabled=v;
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_depth(IntPtr l) {
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
			pushValue(l,true);
			pushValue(l,UnityEngine.GUI.depth);
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
	static public int set_depth(IntPtr l) {
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
			int v;
			checkType(l,2,out v);
			UnityEngine.GUI.depth=v;
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_skin(IntPtr l) {
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
			pushValue(l,true);
			pushValue(l,UnityEngine.GUI.skin);
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
	static public int set_skin(IntPtr l) {
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
			UnityEngine.GUISkin v;
			checkType(l,2,out v);
			UnityEngine.GUI.skin=v;
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_matrix(IntPtr l) {
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
			pushValue(l,true);
			pushValue(l,UnityEngine.GUI.matrix);
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
	static public int set_matrix(IntPtr l) {
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
			UnityEngine.Matrix4x4 v;
			checkValueType(l,2,out v);
			UnityEngine.GUI.matrix=v;
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
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_tooltip(IntPtr l) {
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
			pushValue(l,true);
			pushValue(l,UnityEngine.GUI.tooltip);
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
	static public int set_tooltip(IntPtr l) {
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
			string v;
			checkType(l,2,out v);
			UnityEngine.GUI.tooltip=v;
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
		getTypeTable(l,"UnityEngine.GUI");
		addMember(l,SetNextControlName_s);
		addMember(l,GetNameOfFocusedControl_s);
		addMember(l,FocusControl_s);
		addMember(l,DragWindow_s);
		addMember(l,BringWindowToFront_s);
		addMember(l,BringWindowToBack_s);
		addMember(l,FocusWindow_s);
		addMember(l,UnfocusWindow_s);
		addMember(l,Label_s);
		addMember(l,DrawTexture_s);
		addMember(l,DrawTextureWithTexCoords_s);
		addMember(l,Box_s);
		addMember(l,Button_s);
		addMember(l,RepeatButton_s);
		addMember(l,TextField_s);
		addMember(l,PasswordField_s);
		addMember(l,TextArea_s);
		addMember(l,Toggle_s);
		addMember(l,Toolbar_s);
		addMember(l,SelectionGrid_s);
		addMember(l,HorizontalSlider_s);
		addMember(l,VerticalSlider_s);
		addMember(l,Slider_s);
		addMember(l,HorizontalScrollbar_s);
		addMember(l,VerticalScrollbar_s);
		addMember(l,BeginClip_s);
		addMember(l,BeginGroup_s);
		addMember(l,EndGroup_s);
		addMember(l,EndClip_s);
		addMember(l,BeginScrollView_s);
		addMember(l,EndScrollView_s);
		addMember(l,ScrollTo_s);
		addMember(l,ScrollTowards_s);
		addMember(l,Window_s);
		addMember(l,ModalWindow_s);
		addMember(l,"color",get_color,set_color,false);
		addMember(l,"backgroundColor",get_backgroundColor,set_backgroundColor,false);
		addMember(l,"contentColor",get_contentColor,set_contentColor,false);
		addMember(l,"changed",get_changed,set_changed,false);
		addMember(l,"enabled",get_enabled,set_enabled,false);
		addMember(l,"depth",get_depth,set_depth,false);
		addMember(l,"skin",get_skin,set_skin,false);
		addMember(l,"matrix",get_matrix,set_matrix,false);
		addMember(l,"tooltip",get_tooltip,set_tooltip,false);
		createTypeMetatable(l,constructor, typeof(UnityEngine.GUI));
	}
}

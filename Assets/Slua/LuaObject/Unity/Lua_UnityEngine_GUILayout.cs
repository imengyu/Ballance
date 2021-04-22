using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_GUILayout : LuaObject {
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
			UnityEngine.GUILayout o;
			o=new UnityEngine.GUILayout();
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
			if(matchType(l,argc,1,typeof(UnityEngine.Texture),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Texture a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				UnityEngine.GUILayout.Label(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				UnityEngine.GUILayout.Label(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.GUIContent a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				UnityEngine.GUILayout.Label(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Texture a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				UnityEngine.GUILayout.Label(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				UnityEngine.GUILayout.Label(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.GUIContent a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				UnityEngine.GUILayout.Label(a1,a2,a3);
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
			if(matchType(l,argc,1,typeof(UnityEngine.Texture),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Texture a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				UnityEngine.GUILayout.Box(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				UnityEngine.GUILayout.Box(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.GUIContent a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				UnityEngine.GUILayout.Box(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Texture a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				UnityEngine.GUILayout.Box(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				UnityEngine.GUILayout.Box(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.GUIContent a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				UnityEngine.GUILayout.Box(a1,a2,a3);
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
			if(matchType(l,argc,1,typeof(UnityEngine.Texture),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Texture a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				var ret=UnityEngine.GUILayout.Button(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				var ret=UnityEngine.GUILayout.Button(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.GUIContent a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				var ret=UnityEngine.GUILayout.Button(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Texture a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.Button(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.Button(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.GUIContent a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.Button(a1,a2,a3);
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
			if(matchType(l,argc,1,typeof(UnityEngine.Texture),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Texture a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				var ret=UnityEngine.GUILayout.RepeatButton(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				var ret=UnityEngine.GUILayout.RepeatButton(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.GUIContent a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				var ret=UnityEngine.GUILayout.RepeatButton(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Texture a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.RepeatButton(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.RepeatButton(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.GUIContent a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.RepeatButton(a1,a2,a3);
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
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				var ret=UnityEngine.GUILayout.TextField(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(int),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.TextField(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.TextField(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				System.String a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.TextField(a1,a2,a3,a4);
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
				System.String a1;
				checkType(l,1,out a1);
				System.Char a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.PasswordField(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.Char),typeof(int),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				System.Char a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.PasswordField(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(System.Char),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				System.Char a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.PasswordField(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==5){
				System.String a1;
				checkType(l,1,out a1);
				System.Char a2;
				checkType(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				UnityEngine.GUILayoutOption[] a5;
				checkParams(l,5,out a5);
				var ret=UnityEngine.GUILayout.PasswordField(a1,a2,a3,a4,a5);
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
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				var ret=UnityEngine.GUILayout.TextArea(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(int),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.TextArea(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.TextArea(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				System.String a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.TextArea(a1,a2,a3,a4);
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
			if(matchType(l,argc,1,typeof(bool),typeof(UnityEngine.Texture),typeof(UnityEngine.GUILayoutOption[]))){
				System.Boolean a1;
				checkType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.Toggle(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool),typeof(string),typeof(UnityEngine.GUILayoutOption[]))){
				System.Boolean a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.Toggle(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool),typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUILayoutOption[]))){
				System.Boolean a1;
				checkType(l,1,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.Toggle(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool),typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.Boolean a1;
				checkType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.Toggle(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool),typeof(string),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.Boolean a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.Toggle(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(bool),typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.Boolean a1;
				checkType(l,1,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.Toggle(a1,a2,a3,a4);
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
			if(matchType(l,argc,1,typeof(int),typeof(System.String[]),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				System.String[] a2;
				checkArray(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.Toolbar(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Texture[]),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Texture[] a2;
				checkArray(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.Toolbar(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.GUIContent[]),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.GUIContent[] a2;
				checkArray(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.Toolbar(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(System.String[]),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				System.String[] a2;
				checkArray(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.Toolbar(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Texture[]),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Texture[] a2;
				checkArray(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.Toolbar(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.GUIContent[]),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.GUIContent[] a2;
				checkArray(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.Toolbar(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(System.String[]),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUI.ToolbarButtonSize),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				System.String[] a2;
				checkArray(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUI.ToolbarButtonSize a4;
				a4 = (UnityEngine.GUI.ToolbarButtonSize)LuaDLL.luaL_checkinteger(l, 4);
				UnityEngine.GUILayoutOption[] a5;
				checkParams(l,5,out a5);
				var ret=UnityEngine.GUILayout.Toolbar(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Texture[]),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUI.ToolbarButtonSize),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Texture[] a2;
				checkArray(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUI.ToolbarButtonSize a4;
				a4 = (UnityEngine.GUI.ToolbarButtonSize)LuaDLL.luaL_checkinteger(l, 4);
				UnityEngine.GUILayoutOption[] a5;
				checkParams(l,5,out a5);
				var ret=UnityEngine.GUILayout.Toolbar(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.GUIContent[]),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUI.ToolbarButtonSize),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.GUIContent[] a2;
				checkArray(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUI.ToolbarButtonSize a4;
				a4 = (UnityEngine.GUI.ToolbarButtonSize)LuaDLL.luaL_checkinteger(l, 4);
				UnityEngine.GUILayoutOption[] a5;
				checkParams(l,5,out a5);
				var ret=UnityEngine.GUILayout.Toolbar(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.GUIContent[]),typeof(System.Boolean[]),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.GUIContent[] a2;
				checkArray(l,2,out a2);
				System.Boolean[] a3;
				checkArray(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				UnityEngine.GUILayoutOption[] a5;
				checkParams(l,5,out a5);
				var ret=UnityEngine.GUILayout.Toolbar(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.GUIContent[] a2;
				checkArray(l,2,out a2);
				System.Boolean[] a3;
				checkArray(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				UnityEngine.GUI.ToolbarButtonSize a5;
				a5 = (UnityEngine.GUI.ToolbarButtonSize)LuaDLL.luaL_checkinteger(l, 5);
				UnityEngine.GUILayoutOption[] a6;
				checkParams(l,6,out a6);
				var ret=UnityEngine.GUILayout.Toolbar(a1,a2,a3,a4,a5,a6);
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
			if(matchType(l,argc,1,typeof(int),typeof(System.String[]),typeof(int),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				System.String[] a2;
				checkArray(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.SelectionGrid(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Texture[]),typeof(int),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Texture[] a2;
				checkArray(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.SelectionGrid(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.GUIContent[]),typeof(int),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.GUIContent[] a2;
				checkArray(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.SelectionGrid(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(System.String[]),typeof(int),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				System.String[] a2;
				checkArray(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				UnityEngine.GUILayoutOption[] a5;
				checkParams(l,5,out a5);
				var ret=UnityEngine.GUILayout.SelectionGrid(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Texture[]),typeof(int),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Texture[] a2;
				checkArray(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				UnityEngine.GUILayoutOption[] a5;
				checkParams(l,5,out a5);
				var ret=UnityEngine.GUILayout.SelectionGrid(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.GUIContent[]),typeof(int),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.GUIContent[] a2;
				checkArray(l,2,out a2);
				System.Int32 a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				UnityEngine.GUILayoutOption[] a5;
				checkParams(l,5,out a5);
				var ret=UnityEngine.GUILayout.SelectionGrid(a1,a2,a3,a4,a5);
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
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.HorizontalSlider(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				UnityEngine.GUILayoutOption[] a6;
				checkParams(l,6,out a6);
				var ret=UnityEngine.GUILayout.HorizontalSlider(a1,a2,a3,a4,a5,a6);
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
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.VerticalSlider(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				UnityEngine.GUILayoutOption[] a6;
				checkParams(l,6,out a6);
				var ret=UnityEngine.GUILayout.VerticalSlider(a1,a2,a3,a4,a5,a6);
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
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				UnityEngine.GUILayoutOption[] a5;
				checkParams(l,5,out a5);
				var ret=UnityEngine.GUILayout.HorizontalScrollbar(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				UnityEngine.GUILayoutOption[] a6;
				checkParams(l,6,out a6);
				var ret=UnityEngine.GUILayout.HorizontalScrollbar(a1,a2,a3,a4,a5,a6);
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
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				UnityEngine.GUILayoutOption[] a5;
				checkParams(l,5,out a5);
				var ret=UnityEngine.GUILayout.VerticalScrollbar(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				System.Single a1;
				checkType(l,1,out a1);
				System.Single a2;
				checkType(l,2,out a2);
				System.Single a3;
				checkType(l,3,out a3);
				System.Single a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				UnityEngine.GUILayoutOption[] a6;
				checkParams(l,6,out a6);
				var ret=UnityEngine.GUILayout.VerticalScrollbar(a1,a2,a3,a4,a5,a6);
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
	static public int Space_s(IntPtr l) {
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
			System.Single a1;
			checkType(l,1,out a1);
			UnityEngine.GUILayout.Space(a1);
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
	static public int FlexibleSpace_s(IntPtr l) {
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
			UnityEngine.GUILayout.FlexibleSpace();
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
	static public int BeginHorizontal_s(IntPtr l) {
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
				UnityEngine.GUILayoutOption[] a1;
				checkParams(l,1,out a1);
				UnityEngine.GUILayout.BeginHorizontal(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==2){
				UnityEngine.GUIStyle a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				UnityEngine.GUILayout.BeginHorizontal(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				UnityEngine.GUILayout.BeginHorizontal(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Texture a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				UnityEngine.GUILayout.BeginHorizontal(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.GUIContent a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				UnityEngine.GUILayout.BeginHorizontal(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function BeginHorizontal to call");
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
	static public int EndHorizontal_s(IntPtr l) {
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
			UnityEngine.GUILayout.EndHorizontal();
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
	static public int BeginVertical_s(IntPtr l) {
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
				UnityEngine.GUILayoutOption[] a1;
				checkParams(l,1,out a1);
				UnityEngine.GUILayout.BeginVertical(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==2){
				UnityEngine.GUIStyle a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				UnityEngine.GUILayout.BeginVertical(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(string),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				System.String a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				UnityEngine.GUILayout.BeginVertical(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Texture a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				UnityEngine.GUILayout.BeginVertical(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.GUIContent a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				UnityEngine.GUILayout.BeginVertical(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function BeginVertical to call");
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
	static public int EndVertical_s(IntPtr l) {
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
			UnityEngine.GUILayout.EndVertical();
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
	static public int BeginArea_s(IntPtr l) {
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
				UnityEngine.GUILayout.BeginArea(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(string))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayout.BeginArea(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.Texture a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayout.BeginArea(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIContent))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayout.BeginArea(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayout.BeginArea(a1,a2);
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
				UnityEngine.GUILayout.BeginArea(a1,a2,a3);
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
				UnityEngine.GUILayout.BeginArea(a1,a2,a3);
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
				UnityEngine.GUILayout.BeginArea(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function BeginArea to call");
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
	static public int EndArea_s(IntPtr l) {
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
			UnityEngine.GUILayout.EndArea();
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
			if(matchType(l,argc,1,typeof(UnityEngine.Vector2),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Vector2 a1;
				checkType(l,1,out a1);
				UnityEngine.GUILayoutOption[] a2;
				checkParams(l,2,out a2);
				var ret=UnityEngine.GUILayout.BeginScrollView(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Vector2),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Vector2 a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				var ret=UnityEngine.GUILayout.BeginScrollView(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				UnityEngine.Vector2 a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUILayoutOption[] a3;
				checkParams(l,3,out a3);
				var ret=UnityEngine.GUILayout.BeginScrollView(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Vector2),typeof(bool),typeof(bool),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Vector2 a1;
				checkType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				System.Boolean a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.BeginScrollView(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(UnityEngine.Vector2),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
				UnityEngine.Vector2 a1;
				checkType(l,1,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,2,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,3,out a3);
				UnityEngine.GUILayoutOption[] a4;
				checkParams(l,4,out a4);
				var ret=UnityEngine.GUILayout.BeginScrollView(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				UnityEngine.Vector2 a1;
				checkType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				System.Boolean a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				UnityEngine.GUILayoutOption[] a6;
				checkParams(l,6,out a6);
				var ret=UnityEngine.GUILayout.BeginScrollView(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==7){
				UnityEngine.Vector2 a1;
				checkType(l,1,out a1);
				System.Boolean a2;
				checkType(l,2,out a2);
				System.Boolean a3;
				checkType(l,3,out a3);
				UnityEngine.GUIStyle a4;
				checkType(l,4,out a4);
				UnityEngine.GUIStyle a5;
				checkType(l,5,out a5);
				UnityEngine.GUIStyle a6;
				checkType(l,6,out a6);
				UnityEngine.GUILayoutOption[] a7;
				checkParams(l,7,out a7);
				var ret=UnityEngine.GUILayout.BeginScrollView(a1,a2,a3,a4,a5,a6,a7);
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
			UnityEngine.GUILayout.EndScrollView();
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
			if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(string),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				System.String a4;
				checkType(l,4,out a4);
				UnityEngine.GUILayoutOption[] a5;
				checkParams(l,5,out a5);
				var ret=UnityEngine.GUILayout.Window(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(UnityEngine.Texture),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				UnityEngine.Texture a4;
				checkType(l,4,out a4);
				UnityEngine.GUILayoutOption[] a5;
				checkParams(l,5,out a5);
				var ret=UnityEngine.GUILayout.Window(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUILayoutOption[]))){
				System.Int32 a1;
				checkType(l,1,out a1);
				UnityEngine.Rect a2;
				checkValueType(l,2,out a2);
				UnityEngine.GUI.WindowFunction a3;
				checkDelegate(l,3,out a3);
				UnityEngine.GUIContent a4;
				checkType(l,4,out a4);
				UnityEngine.GUILayoutOption[] a5;
				checkParams(l,5,out a5);
				var ret=UnityEngine.GUILayout.Window(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(string),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
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
				UnityEngine.GUILayoutOption[] a6;
				checkParams(l,6,out a6);
				var ret=UnityEngine.GUILayout.Window(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
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
				UnityEngine.GUILayoutOption[] a6;
				checkParams(l,6,out a6);
				var ret=UnityEngine.GUILayout.Window(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(int),typeof(UnityEngine.Rect),typeof(UnityEngine.GUI.WindowFunction),typeof(UnityEngine.GUIContent),typeof(UnityEngine.GUIStyle),typeof(UnityEngine.GUILayoutOption[]))){
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
				UnityEngine.GUILayoutOption[] a6;
				checkParams(l,6,out a6);
				var ret=UnityEngine.GUILayout.Window(a1,a2,a3,a4,a5,a6);
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
	static public int Width_s(IntPtr l) {
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
			System.Single a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.GUILayout.Width(a1);
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
	static public int MinWidth_s(IntPtr l) {
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
			System.Single a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.GUILayout.MinWidth(a1);
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
	static public int MaxWidth_s(IntPtr l) {
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
			System.Single a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.GUILayout.MaxWidth(a1);
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
	static public int Height_s(IntPtr l) {
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
			System.Single a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.GUILayout.Height(a1);
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
	static public int MinHeight_s(IntPtr l) {
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
			System.Single a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.GUILayout.MinHeight(a1);
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
	static public int MaxHeight_s(IntPtr l) {
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
			System.Single a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.GUILayout.MaxHeight(a1);
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
	static public int ExpandWidth_s(IntPtr l) {
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
			System.Boolean a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.GUILayout.ExpandWidth(a1);
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
	static public int ExpandHeight_s(IntPtr l) {
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
			System.Boolean a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.GUILayout.ExpandHeight(a1);
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
		getTypeTable(l,"UnityEngine.GUILayout");
		addMember(l,Label_s);
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
		addMember(l,HorizontalScrollbar_s);
		addMember(l,VerticalScrollbar_s);
		addMember(l,Space_s);
		addMember(l,FlexibleSpace_s);
		addMember(l,BeginHorizontal_s);
		addMember(l,EndHorizontal_s);
		addMember(l,BeginVertical_s);
		addMember(l,EndVertical_s);
		addMember(l,BeginArea_s);
		addMember(l,EndArea_s);
		addMember(l,BeginScrollView_s);
		addMember(l,EndScrollView_s);
		addMember(l,Window_s);
		addMember(l,Width_s);
		addMember(l,MinWidth_s);
		addMember(l,MaxWidth_s);
		addMember(l,Height_s);
		addMember(l,MinHeight_s);
		addMember(l,MaxHeight_s);
		addMember(l,ExpandWidth_s);
		addMember(l,ExpandHeight_s);
		createTypeMetatable(l,constructor, typeof(UnityEngine.GUILayout));
	}
}

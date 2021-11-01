using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_Services_GameSoundManager : LuaObject {
	[SLua.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int LoadAudioResource(IntPtr l) {
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
				Ballance2.Sys.Services.GameSoundManager self=(Ballance2.Sys.Services.GameSoundManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.LoadAudioResource(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				Ballance2.Sys.Services.GameSoundManager self=(Ballance2.Sys.Services.GameSoundManager)checkSelf(l);
				Ballance2.Sys.Package.GamePackage a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				var ret=self.LoadAudioResource(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function LoadAudioResource to call");
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
	static public int RegisterSoundPlayer(IntPtr l) {
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
				Ballance2.Sys.Services.GameSoundManager self=(Ballance2.Sys.Services.GameSoundManager)checkSelf(l);
				Ballance2.Sys.Services.GameSoundType a1;
				a1 = (Ballance2.Sys.Services.GameSoundType)LuaDLL.luaL_checkinteger(l, 2);
				UnityEngine.AudioSource a2;
				checkType(l,3,out a2);
				var ret=self.RegisterSoundPlayer(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Ballance2.Sys.Services.GameSoundType),typeof(UnityEngine.AudioClip),typeof(bool),typeof(bool),typeof(string))){
				Ballance2.Sys.Services.GameSoundManager self=(Ballance2.Sys.Services.GameSoundManager)checkSelf(l);
				Ballance2.Sys.Services.GameSoundType a1;
				a1 = (Ballance2.Sys.Services.GameSoundType)LuaDLL.luaL_checkinteger(l, 2);
				UnityEngine.AudioClip a2;
				checkType(l,3,out a2);
				System.Boolean a3;
				checkType(l,4,out a3);
				System.Boolean a4;
				checkType(l,5,out a4);
				System.String a5;
				checkType(l,6,out a5);
				var ret=self.RegisterSoundPlayer(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Ballance2.Sys.Services.GameSoundType),typeof(string),typeof(bool),typeof(bool),typeof(string))){
				Ballance2.Sys.Services.GameSoundManager self=(Ballance2.Sys.Services.GameSoundManager)checkSelf(l);
				Ballance2.Sys.Services.GameSoundType a1;
				a1 = (Ballance2.Sys.Services.GameSoundType)LuaDLL.luaL_checkinteger(l, 2);
				System.String a2;
				checkType(l,3,out a2);
				System.Boolean a3;
				checkType(l,4,out a3);
				System.Boolean a4;
				checkType(l,5,out a4);
				System.String a5;
				checkType(l,6,out a5);
				var ret=self.RegisterSoundPlayer(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function RegisterSoundPlayer to call");
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
	static public int IsSoundPlayerRegistered(IntPtr l) {
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
			Ballance2.Sys.Services.GameSoundManager self=(Ballance2.Sys.Services.GameSoundManager)checkSelf(l);
			UnityEngine.AudioSource a1;
			checkType(l,2,out a1);
			var ret=self.IsSoundPlayerRegistered(a1);
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
	static public int DestroySoundPlayer(IntPtr l) {
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
			Ballance2.Sys.Services.GameSoundManager self=(Ballance2.Sys.Services.GameSoundManager)checkSelf(l);
			UnityEngine.AudioSource a1;
			checkType(l,2,out a1);
			var ret=self.DestroySoundPlayer(a1);
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
	static public int PlayFastVoice(IntPtr l) {
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
				Ballance2.Sys.Services.GameSoundManager self=(Ballance2.Sys.Services.GameSoundManager)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				Ballance2.Sys.Services.GameSoundType a2;
				a2 = (Ballance2.Sys.Services.GameSoundType)LuaDLL.luaL_checkinteger(l, 3);
				var ret=self.PlayFastVoice(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				Ballance2.Sys.Services.GameSoundManager self=(Ballance2.Sys.Services.GameSoundManager)checkSelf(l);
				Ballance2.Sys.Package.GamePackage a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				Ballance2.Sys.Services.GameSoundType a3;
				a3 = (Ballance2.Sys.Services.GameSoundType)LuaDLL.luaL_checkinteger(l, 4);
				var ret=self.PlayFastVoice(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function PlayFastVoice to call");
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
	static public int get_TAG(IntPtr l) {
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
			pushValue(l,Ballance2.Sys.Services.GameSoundManager.TAG);
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
	static public int get_GameMainAudioMixer(IntPtr l) {
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
			Ballance2.Sys.Services.GameSoundManager self=(Ballance2.Sys.Services.GameSoundManager)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.GameMainAudioMixer);
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
	static public int set_GameMainAudioMixer(IntPtr l) {
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
			Ballance2.Sys.Services.GameSoundManager self=(Ballance2.Sys.Services.GameSoundManager)checkSelf(l);
			UnityEngine.Audio.AudioMixer v;
			checkType(l,2,out v);
			self.GameMainAudioMixer=v;
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
	static public int get_GameUIAudioMixer(IntPtr l) {
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
			Ballance2.Sys.Services.GameSoundManager self=(Ballance2.Sys.Services.GameSoundManager)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.GameUIAudioMixer);
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
	static public int set_GameUIAudioMixer(IntPtr l) {
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
			Ballance2.Sys.Services.GameSoundManager self=(Ballance2.Sys.Services.GameSoundManager)checkSelf(l);
			UnityEngine.Audio.AudioMixer v;
			checkType(l,2,out v);
			self.GameUIAudioMixer=v;
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
		getTypeTable(l,"Ballance2.Sys.Services.GameSoundManager");
		addMember(l,LoadAudioResource);
		addMember(l,RegisterSoundPlayer);
		addMember(l,IsSoundPlayerRegistered);
		addMember(l,DestroySoundPlayer);
		addMember(l,PlayFastVoice);
		addMember(l,"TAG",get_TAG,null,false);
		addMember(l,"GameMainAudioMixer",get_GameMainAudioMixer,set_GameMainAudioMixer,true);
		addMember(l,"GameUIAudioMixer",get_GameUIAudioMixer,set_GameUIAudioMixer,true);
		createTypeMetatable(l,null, typeof(Ballance2.Sys.Services.GameSoundManager),typeof(Ballance2.Sys.Services.GameService));
	}
}

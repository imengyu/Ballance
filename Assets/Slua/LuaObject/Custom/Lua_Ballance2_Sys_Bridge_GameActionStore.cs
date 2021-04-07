using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_Ballance2_Sys_Bridge_GameActionStore : LuaObject {
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
			Ballance2.Sys.Bridge.GameActionStore o;
			Ballance2.Sys.Package.GamePackage a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			o=new Ballance2.Sys.Bridge.GameActionStore(a1,a2);
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
	static public int RegisterAction(IntPtr l) {
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
				Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
				Ballance2.Sys.Package.GamePackage a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				Ballance2.Sys.Bridge.Handler.GameHandler a3;
				checkType(l,4,out a3);
				System.String[] a4;
				checkArray(l,5,out a4);
				var ret=self.RegisterAction(a1,a2,a3,a4);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==6){
				Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
				Ballance2.Sys.Package.GamePackage a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				System.String a3;
				checkType(l,4,out a3);
				Ballance2.Sys.Bridge.GameActionHandlerDelegate a4;
				checkDelegate(l,5,out a4);
				System.String[] a5;
				checkArray(l,6,out a5);
				var ret=self.RegisterAction(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==7){
				Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
				Ballance2.Sys.Package.GamePackage a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				System.String a3;
				checkType(l,4,out a3);
				SLua.LuaFunction a4;
				checkType(l,5,out a4);
				SLua.LuaTable a5;
				checkType(l,6,out a5);
				System.String[] a6;
				checkArray(l,7,out a6);
				var ret=self.RegisterAction(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function RegisterAction to call");
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
	static public int UnRegisterAction(IntPtr l) {
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
			if(matchType(l,argc,2,typeof(Ballance2.Sys.Bridge.GameAction))){
				Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
				Ballance2.Sys.Bridge.GameAction a1;
				checkType(l,2,out a1);
				self.UnRegisterAction(a1);
				pushValue(l,true);
				return 1;
			}
			else if(matchType(l,argc,2,typeof(string))){
				Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.UnRegisterAction(a1);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function UnRegisterAction to call");
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
	static public int UnRegisterActions(IntPtr l) {
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
			Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
			System.String[] a1;
			checkArray(l,2,out a1);
			self.UnRegisterActions(a1);
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
	static public int IsActionRegistered(IntPtr l) {
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
				Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.IsActionRegistered(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				Ballance2.Sys.Bridge.GameAction a2;
				var ret=self.IsActionRegistered(a1,out a2);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a2);
				return 3;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function IsActionRegistered to call");
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
	static public int GetRegisteredAction(IntPtr l) {
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
			Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetRegisteredAction(a1);
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
	static public int RegisterActions(IntPtr l) {
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
			if(matchType(l,argc,2,typeof(Ballance2.Sys.Package.GamePackage),typeof(System.String[]),typeof(System.String[]),typeof(Ballance2.Sys.Bridge.GameActionHandlerDelegate[]),typeof(System.String[][]))){
				Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
				Ballance2.Sys.Package.GamePackage a1;
				checkType(l,2,out a1);
				System.String[] a2;
				checkArray(l,3,out a2);
				System.String[] a3;
				checkArray(l,4,out a3);
				Ballance2.Sys.Bridge.GameActionHandlerDelegate[] a4;
				checkArray(l,5,out a4);
				System.String[][] a5;
				checkArray(l,6,out a5);
				var ret=self.RegisterActions(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Ballance2.Sys.Package.GamePackage),typeof(System.String[]),typeof(string),typeof(Ballance2.Sys.Bridge.GameActionHandlerDelegate[]),typeof(System.String[][]))){
				Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
				Ballance2.Sys.Package.GamePackage a1;
				checkType(l,2,out a1);
				System.String[] a2;
				checkArray(l,3,out a2);
				System.String a3;
				checkType(l,4,out a3);
				Ballance2.Sys.Bridge.GameActionHandlerDelegate[] a4;
				checkArray(l,5,out a4);
				System.String[][] a5;
				checkArray(l,6,out a5);
				var ret=self.RegisterActions(a1,a2,a3,a4,a5);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Ballance2.Sys.Package.GamePackage),typeof(System.String[]),typeof(System.String[]),typeof(SLua.LuaFunction[]),typeof(SLua.LuaTable),typeof(System.String[][]))){
				Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
				Ballance2.Sys.Package.GamePackage a1;
				checkType(l,2,out a1);
				System.String[] a2;
				checkArray(l,3,out a2);
				System.String[] a3;
				checkArray(l,4,out a3);
				SLua.LuaFunction[] a4;
				checkArray(l,5,out a4);
				SLua.LuaTable a5;
				checkType(l,6,out a5);
				System.String[][] a6;
				checkArray(l,7,out a6);
				var ret=self.RegisterActions(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Ballance2.Sys.Package.GamePackage),typeof(System.String[]),typeof(string),typeof(SLua.LuaFunction[]),typeof(SLua.LuaTable),typeof(System.String[][]))){
				Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
				Ballance2.Sys.Package.GamePackage a1;
				checkType(l,2,out a1);
				System.String[] a2;
				checkArray(l,3,out a2);
				System.String a3;
				checkType(l,4,out a3);
				SLua.LuaFunction[] a4;
				checkArray(l,5,out a4);
				SLua.LuaTable a5;
				checkType(l,6,out a5);
				System.String[][] a6;
				checkArray(l,7,out a6);
				var ret=self.RegisterActions(a1,a2,a3,a4,a5,a6);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function RegisterActions to call");
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
	static public int CallAction(IntPtr l) {
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
			if(matchType(l,argc,2,typeof(string),typeof(object[]))){
				Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.Object[] a2;
				checkParams(l,3,out a2);
				var ret=self.CallAction(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(Ballance2.Sys.Bridge.GameAction),typeof(object[]))){
				Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
				Ballance2.Sys.Bridge.GameAction a1;
				checkType(l,2,out a1);
				System.Object[] a2;
				checkParams(l,3,out a2);
				var ret=self.CallAction(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function CallAction to call");
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
			Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TAG);
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
	static public int get_Actions(IntPtr l) {
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
			Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Actions);
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
	static public int get_Name(IntPtr l) {
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
			Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Name);
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
	static public int get_KeyName(IntPtr l) {
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
			Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.KeyName);
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
	static public int get_Package(IntPtr l) {
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
			Ballance2.Sys.Bridge.GameActionStore self=(Ballance2.Sys.Bridge.GameActionStore)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Package);
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
		getTypeTable(l,"Ballance2.Sys.Bridge.GameActionStore");
		addMember(l,RegisterAction);
		addMember(l,UnRegisterAction);
		addMember(l,UnRegisterActions);
		addMember(l,IsActionRegistered);
		addMember(l,GetRegisteredAction);
		addMember(l,RegisterActions);
		addMember(l,CallAction);
		addMember(l,"TAG",get_TAG,null,true);
		addMember(l,"Actions",get_Actions,null,true);
		addMember(l,"Name",get_Name,null,true);
		addMember(l,"KeyName",get_KeyName,null,true);
		addMember(l,"Package",get_Package,null,true);
		createTypeMetatable(l,constructor, typeof(Ballance2.Sys.Bridge.GameActionStore));
	}
}

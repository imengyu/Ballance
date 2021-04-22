using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_GUI_GroupScope : LuaObject {
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
			UnityEngine.GUI.GroupScope o;
			if(argc==2){
				UnityEngine.Rect a1;
				checkValueType(l,2,out a1);
				o=new UnityEngine.GUI.GroupScope(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rect),typeof(string))){
				UnityEngine.Rect a1;
				checkValueType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				o=new UnityEngine.GUI.GroupScope(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture))){
				UnityEngine.Rect a1;
				checkValueType(l,2,out a1);
				UnityEngine.Texture a2;
				checkType(l,3,out a2);
				o=new UnityEngine.GUI.GroupScope(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIContent))){
				UnityEngine.Rect a1;
				checkValueType(l,2,out a1);
				UnityEngine.GUIContent a2;
				checkType(l,3,out a2);
				o=new UnityEngine.GUI.GroupScope(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rect),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,2,out a1);
				UnityEngine.GUIStyle a2;
				checkType(l,3,out a2);
				o=new UnityEngine.GUI.GroupScope(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rect),typeof(string),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,4,out a3);
				o=new UnityEngine.GUI.GroupScope(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(UnityEngine.Rect),typeof(UnityEngine.Texture),typeof(UnityEngine.GUIStyle))){
				UnityEngine.Rect a1;
				checkValueType(l,2,out a1);
				UnityEngine.Texture a2;
				checkType(l,3,out a2);
				UnityEngine.GUIStyle a3;
				checkType(l,4,out a3);
				o=new UnityEngine.GUI.GroupScope(a1,a2,a3);
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
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.GUI.GroupScope");
		createTypeMetatable(l,constructor, typeof(UnityEngine.GUI.GroupScope),typeof(UnityEngine.GUI.Scope));
	}
}

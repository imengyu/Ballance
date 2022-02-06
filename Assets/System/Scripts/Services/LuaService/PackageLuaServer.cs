using SLua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/*
* Copyright(c) 2021 slua
*
* 模块名：     
* PackageLuaServer.cs
* 
* 用途：
* 从 Slua中拷贝出来，为适应游戏Package模块Lua虚拟机分区而修改了一下下的Lua虚拟机。
*
* 作者：
* mengyu
*/

namespace Ballance2.Services.LuaService
{
  public class PackageLuaServer
  {
    public class PackageState : LuaState
    {
      int errorReported = 0;
      private PackageLuaServer server;

      public PackageState(PackageLuaServer server)
      {
        this.server = server;
      }

      protected override void tick()
      {
        base.tick();
#if !SLUA_STANDALONE
        LuaTimer.tick(Time.deltaTime);
#endif
        checkTop();
      }

      public PackageLuaServer getServer() { return server; }

      internal void checkTop()
      {
        if (LuaDLL.lua_gettop(L) != errorReported)
        {
          errorReported = LuaDLL.lua_gettop(L);
          SLua.Logger.LogError(string.Format("Some function not remove temp value({0}) from lua stack. You should fix it.", LuaDLL.luaL_typename(L, errorReported)));
        }
      }
    }

    private PackageState luaState;

    public PackageLuaServer(string name)
    {
      luaState = new PackageState(this);
      luaState.Name = name;
    }

    public PackageState getLuaState()
    {
      return luaState;
    }

    static List<Action<IntPtr>> collectBindInfo()
    {
      List<Action<IntPtr>> list = new List<Action<IntPtr>>();

#if !SLUA_STANDALONE
#if !USE_STATIC_BINDER
      Assembly[] ams = AppDomain.CurrentDomain.GetAssemblies();

      List<Type> bindlist = new List<Type>();
      for (int n = 0; n < ams.Length; n++)
      {
        Assembly a = ams[n];
        Type[] ts = null;
        try
        {
          ts = a.GetExportedTypes();
        }
        catch
        {
          continue;
        }
        for (int k = 0; k < ts.Length; k++)
        {
          Type t = ts[k];
          if (t.IsDefined(typeof(LuaBinderAttribute), false))
          {
            bindlist.Add(t);
          }
        }
      }

      bindlist.Sort(new Comparison<Type>((Type a, Type b) =>
      {
        LuaBinderAttribute la = Attribute.GetCustomAttribute(a, typeof(LuaBinderAttribute)) as LuaBinderAttribute;
        LuaBinderAttribute lb = Attribute.GetCustomAttribute(b, typeof(LuaBinderAttribute)) as LuaBinderAttribute;

        return la.order.CompareTo(lb.order);
      }));

      for (int n = 0; n < bindlist.Count; n++)
      {
        Type t = bindlist[n];
        var sublist = (Action<IntPtr>[])t.GetMethod("GetBindList").Invoke(null, null);
        list.AddRange(sublist);
      }
#else
			var assemblyName = "Assembly-CSharp";
			Assembly assembly = Assembly.Load(assemblyName);
			list.AddRange(getBindList(assembly,"SLua.BindUnity"));
			list.AddRange(getBindList(assembly,"SLua.BindUnityUI"));
			list.AddRange(getBindList(assembly,"SLua.BindDll"));
			list.AddRange(getBindList(assembly,"SLua.BindCustom"));
#endif
#endif

      return list;
    }

    static internal IEnumerator doBind(IntPtr L, Action<int> _tick, Action complete)
    {
      Action<int> tick = (int p) =>
      {
        if (_tick != null)
          _tick(p);
      };

      tick(0);
      var list = collectBindInfo();

      tick(2);

      int bindProgress = 2;
      int lastProgress = bindProgress;
      for (int n = 0; n < list.Count; n++)
      {
        Action<IntPtr> action = list[n];
        action(L);
        bindProgress = (int)(((float)n / list.Count) * 98.0) + 2;
        if (_tick != null && lastProgress != bindProgress && bindProgress % 5 == 0)
        {
          lastProgress = bindProgress;
          tick(bindProgress);
          yield return null;
        }
      }

      tick(100);
      complete();
    }

    protected void doinit(LuaState L, LuaSvrFlag flag)
    {
      L.openSluaLib();
      LuaValueType.reg(L.L);
      if ((flag & LuaSvrFlag.LSF_EXTLIB) != 0)
      {
        L.openExtLib();
      }

      if ((flag & LuaSvrFlag.LSF_3RDDLL) != 0)
        Lua3rdDLL.open(L.L);
    }

    public void init(Action<int> tick, Action complete, LuaSvrFlag flag = LuaSvrFlag.LSF_BASIC | LuaSvrFlag.LSF_EXTLIB)
    {
      IntPtr L = luaState.L;
      LuaObject.init(L);
      luaState.lgo.StartCoroutine(doBind(L, tick, () =>
      {
        doinit(luaState, flag);
        complete();
        luaState.checkTop();
      }));
    }
  }

}

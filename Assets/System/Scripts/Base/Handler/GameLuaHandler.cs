using SLua;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameLuaHandler.cs
 *
 * 用途：
 * Lua事件或是回调接收器。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Base.Handler
{
  class GameLuaHandler : GameHandler
  {
    public override bool CallEventHandler(string evtName, params object[] pararms)
    {
      if (Destroyed) return false;
      bool result = false;
      object rs = null;
      object[] pararms2 = new object[pararms.Length + 1];
      pararms2[0] = evtName;
      for (int i = 0; i < pararms.Length; i++)
        pararms2[i + 1] = pararms[i];
      if (luaSelf != null) rs = luaFunction.call(luaSelf, pararms2);
      else rs = luaFunction.call(pararms2);
      if (rs is bool)
        result = (bool)rs;
      return result;
    }
    public override object CallCustomHandler(params object[] pararms)
    {
      if (Destroyed) return false;
      if (luaSelf != null) return luaFunction.call(luaSelf, pararms);
      else return luaFunction.call(pararms);
    }

    public GameLuaHandler(LuaFunction function, LuaTable self)
    {
      luaFunction = function;
      luaSelf = self;
    }

    private LuaFunction luaFunction = null;
    private LuaTable luaSelf = null;

    public override void Dispose()
    {
      if (luaFunction != null)
      {
        luaFunction.Dispose();
        luaFunction = null;
      }
      if (luaSelf != null)
      {
        luaSelf.Dispose();
        luaSelf = null;
      }
      base.Dispose();
    }
  }
}

using Ballance2.System.Bridge.LuaWapper;
using SLua;
using System;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameLuaHandler.cs
 * 用途：
 * Lua 接收器 Handler，用于为LUA层提供事件与回调的接收能力。
 * 
 * 作者：
 * mengyu
 * 
 * 更改历史：
 * 2020-1-1 创建
 * 2021-1-17 imengyu 修改 GameLuaHandler 为 GameLuaStaticHandler 优化了逻辑
 *
 */

namespace Ballance2.System.Bridge.Handler
{
    public class GameLuaStaticHandler : GameHandler
    {
        public GameLuaStaticHandler(string luaPackageulHandlerString)
        {
            InitHandler(luaPackageulHandlerString);
        }

        private GameLuaObjectHost targetObject = null;

        private string[] handlerArgs = new string[0];
        private string handlerFuncName = "";
        private LuaFunction handlerFunc = null;
        private bool initSuccess = false;

        private void InitHandler(string luaPackageulHandlerString)
        {
            if (string.IsNullOrEmpty(luaPackageulHandlerString))
                throw new Exception("luaPackageulHandlerString is null");
            string[] strs = luaPackageulHandlerString.Split(':');
            if (strs.Length >= 2)
            {
                if (strs[0].ToLower() == "main")
                {
                    handlerFuncName = strs[1];
                    handlerFunc = BelongPackage.GetLuaFun(handlerFuncName);
                    initSuccess = true;
                }
                else if (BelongPackage.FindLuaObject(strs[0], out targetObject))
                {
                    handlerFuncName = strs[1];
                    handlerFunc = targetObject.GetLuaFun(handlerFuncName);
                    initSuccess = true;
                }

                if(strs.Length >= 3)
                {
                    handlerArgs = new string[strs.Length - 2];
                    for(int i = 3; i  < strs.Length; i++)
                        handlerArgs[i - 2] = strs[i];
                }
            }
        }

        public override void Dispose()
        {
            handlerArgs = null;
            handlerFunc = null;
            base.Dispose();
        }

        public override bool CallEventHandler(string evtName, params object[] pararms)
        {
            if (Destroyed) return false;
            if (initSuccess)
            {
                bool rs = false;
                object rso = null;
                object[] pararms2 = new object[pararms.Length + 1 + handlerArgs.Length];
                pararms2[0] = evtName;
                for (int i = 0; i < pararms.Length; i++)
                    pararms2[i + 1] = pararms[i];
                for (int i = 0; i < handlerArgs.Length; i++)
                    pararms2[i + 1 + pararms.Length] = handlerArgs[i];
                if (targetObject != null) rso = handlerFunc.call(targetObject.LuaSelf, pararms2);
                else rso = handlerFunc.call(pararms2);
                if (rso is bool) rs = (bool)rso;
                return rs;
            }
            return false;
        }
        public override GameActionCallResult CallActionHandler(params object[] pararms)
        {
            if (Destroyed) return null;
            if (initSuccess)
            {
                GameActionCallResult rs = null;
                object rso = null;
                object[] pararms2 = new object[pararms.Length + handlerArgs.Length];
                for (int i = 0; i < pararms.Length; i++)
                    pararms2[i] = pararms[i];
                for (int i = 0; i < handlerArgs.Length; i++)
                    pararms2[i + pararms.Length] = handlerArgs[i];
                if (targetObject != null) rso = handlerFunc.call(targetObject.LuaSelf, pararms2);
                else rso = handlerFunc.call(pararms2);
                if (rso is GameActionCallResult)
                    rs = rso as GameActionCallResult;
                return rs;
            }
            return null;
        }
        public override object CallCustomHandler(params object[] pararms)
        {
            if (Destroyed) return false;
            if (initSuccess)
            {
                object[] pararms2 = new object[pararms.Length + handlerArgs.Length];
                for (int i = 0; i < pararms.Length; i++)
                    pararms2[i] = pararms[i];
                for (int i = 0; i < handlerArgs.Length; i++)
                    pararms2[i + pararms.Length] = handlerArgs[i];
                if (targetObject != null) return handlerFunc.call(targetObject.LuaSelf, pararms2);
                else return handlerFunc.call(pararms2);
            }
            return false;
        }
    }



}

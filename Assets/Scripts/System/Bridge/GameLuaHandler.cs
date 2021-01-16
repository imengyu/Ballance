using SLua;

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
 *
 */

namespace Ballance2.System.Bridge
{
    /// <summary>
    /// Lua 接收器 Handler
    /// </summary>
    public class GameLuaHandler
    {
        /// <summary>
        /// 创建 Lua 接收器
        /// </summary>
        /// <param name="luaModulHandlerString">接收器标识符字符串</param>
        /// <remarks>
        /// 接收器标识符字符串:
        ///    [格式] 模块标识符:对象名称:函数名称
        ///    
        ///     [模块标识符] 模组包包名 或  模组包UID
        ///     [对象名称]   已注册的 GameLuaObjectHost 名称  或  Main (模组主代码)
        ///     [函数名称]
        ///     [附带参数]   可选：要传给接收器的附带参数，参数将放在结尾
        /// </remarks>
        public GameLuaHandler(string luaModulHandlerString)
        {
            ModManager = (ModManager)GameManager.GetManager(ModManager.TAG);
            InitHandler(luaModulHandlerString);
        }

        private ModManager ModManager = null;

        private GameMod targetMod = null;
        private GameLuaObjectHost targetObject = null;

        private string[] handlerArgs = new string[0];
        private string handlerFuncName = "";
        private LuaFunction handlerFunc = null;
        private bool initSuccess = false;

        private void InitHandler(string luaModulHandlerString)
        {
            if (string.IsNullOrEmpty(luaModulHandlerString))
                throw new System.Exception("luaModulHandlerString is null");
            string[] strs = luaModulHandlerString.Split(':');
            if (strs.Length >= 3)
            {
                targetMod = ModManager.FindGameModByAssetStr(strs[0]);
                if (targetMod == null) return;
                if (strs[1].ToLower() == "main")
                {
                    handlerFuncName = strs[2];
                    handlerFunc = targetMod.GetLuaFun(handlerFuncName);
                    initSuccess = true;
                }
                else if (targetMod.FindLuaObject(strs[1], out targetObject))
                {
                    handlerFuncName = strs[2];
                    handlerFunc = targetObject.GetLuaFun(handlerFuncName);
                    initSuccess = true;
                }

                if(strs.Length >= 4)
                {
                    handlerArgs = new string[strs.Length - 3];
                    for(int i = 3; i  < strs.Length; i++)
                        handlerArgs[i - 3] = strs[i];
                }
            }
        }

        public bool RunCommandHandler(string keyword, int argCount, string fullCmd, string[] args)
        {
            if(initSuccess)
            {
                bool rs = false;
                object rso = null;
                if (targetObject != null)
                    rso = handlerFunc.call(targetObject.LuaSelf, keyword, argCount, fullCmd, args);
                else
                    rso = handlerFunc.call(keyword, argCount, fullCmd, args);
                if (rso is bool) rs = (bool)rso;
                return rs;
            }
            return false;
        }
        public bool RunEventHandler(string evtName, params object[] pararms)
        {
            if (initSuccess)
            {
                bool rs = false;
                object rso = null;
                object[] pararms2 = new object[pararms.Length + 1 + handlerArgs.Length];
                pararms2[0] = evtName;
                for (int i = 0; i < pararms.Length;i++)
                    pararms2[i + 1] = pararms[i];
                for (int i = 0; i < handlerArgs.Length; i++)
                    pararms2[i + 1 + pararms.Length] = handlerArgs[i];
                if (targetObject != null)  rso = handlerFunc.call(targetObject.LuaSelf, pararms2);
                else rso = handlerFunc.call(pararms2);
                if (rso is bool) rs = (bool)rso;
                return rs;
            }
            return false;
        }
        public GameActionCallResult RunActionHandler(params object[] pararms)
        {
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
        public bool RunCustomHandler(params object[] pararms)
        {
            if (initSuccess)
            {
                object[] pararms2 = new object[pararms.Length + handlerArgs.Length];
                for (int i = 0; i < pararms.Length; i++)
                    pararms2[i] = pararms[i];
                for (int i = 0; i < handlerArgs.Length; i++)
                    pararms2[i + pararms.Length] = handlerArgs[i];

                if (targetObject != null)
                    handlerFunc.call(targetObject.LuaSelf, pararms2);
                else
                    handlerFunc.call(pararms2);
            }
            return false;
        }

    }
}

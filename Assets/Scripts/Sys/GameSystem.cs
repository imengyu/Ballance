﻿using Ballance2.Sys.Bridge;
using Ballance2.Sys.Debug;
using Ballance2.Sys.Package;
using Ballance2.Sys.Services;
using Ballance2.Sys.Utils;
using Ballance2.Utils;
using System.Collections.Generic;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameSystem.cs
* 
* 用途：
* 游戏的基础系统与入口管理。
* 此管理器用来管理基础系统初始化和几个基础服务，与GameManager不同，
* GameManager管理的是上层的服务，而此服务管理的是基础服务。
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-15 创建
*
*/

namespace Ballance2.Sys
{
    /// <summary>
    /// 基础系统
    /// </summary>
    public static class GameSystem
    {
        private const string TAG = "GameSystem";

        #region 系统入口

        /// <summary>
        /// 系统接管器回调
        /// </summary>
        /// <param name="act">当前操作</param>
        public delegate void SysHandler(int act);
        private static SysHandler sysHandler = null;

        public const int ACTION_INIT = 1;
        public const int ACTION_DESTROY = 2;
        public const int ACTION_FORCE_INT = 3;

        /// <summary>
        /// 注册系统接管器
        /// </summary>
        /// <param name="handler">系统接管器</param>
        public static void RegSysHandler(SysHandler handler)
        {
            if (sysHandler != null)
            {
                Log.E("GameSystemInit", "SysHandler already set ");
                return;
            }
            sysHandler = handler;
        }

        #endregion

        #region 系统服务

        private static Dictionary<string, GameService> systemService = new Dictionary<string, GameService>();

        /// <summary>
        /// 注册系统服务
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <param name="classObject">服务对象</param>
        /// <returns></returns>
        public static bool RegSystemService(string name, GameService classObject)
        {
            if(systemService.ContainsKey(name))
            {
                GameErrorChecker.LastError = GameError.AlreadyRegistered;
                return false;
            }

            //init
            if(!classObject.Initialize())
            {
                Log.E(TAG, "Service {0} init failed ! {1}({2})", classObject.Name, 
                    GameErrorChecker.LastError, GameErrorChecker.GetLastErrorMessage());
                return false;
            }

            systemService.Add(name, classObject);

            return true;
        }
        /// <summary>
        /// 取消注册系统服务
        /// </summary>
        /// <param name="name">服务</param>
        /// <returns></returns>
        public static bool UnRegSystemService(string name)
        {
            if (!systemService.ContainsKey(name))
            {
                GameErrorChecker.LastError = GameError.NotRegister;
                return false;
            }

            //释放
            GameService gameService = systemService[name];
            gameService.Destroy();

            return systemService.Remove(name);
        }
        /// <summary>
        /// 获取系统服务
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public static GameService GetSystemService(string name)
        {
            if(!systemService.TryGetValue(name, out GameService o))
                GameErrorChecker.LastError = GameError.ClassNotFound;
            return o;
        }

        #endregion

        #region 调试提供

        /// <summary>
        /// 系统调试提供者
        /// </summary>
        public interface SysDebugProvider
        {
            bool StartDebug();
        }
        public delegate SysDebugProvider SysDebugProviderCheck();

        private static SysDebugProvider sysDebugProvider = null;
        private static SysDebugProviderCheck sysDebugProviderCheck = null;

        /// <summary>
        /// 注册调试提供者
        /// </summary>
        public static void RegSysDebugProvider(SysDebugProviderCheck providerCheck) {
            sysDebugProviderCheck = providerCheck;
        }
        private static void StartRunDebugProvider()
        {
            if (sysDebugProviderCheck != null)
            {
                sysDebugProvider = sysDebugProviderCheck.Invoke();
                if (sysDebugProvider != null)
                    sysDebugProvider.StartDebug();
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary> 
        public static void Init()
        {
            //Init system
            UnityLogCatcher.Init();
            
            //Call game init
            if(sysHandler == null) {
                Log.D(TAG, "Not found SysHandler, did you call RegSysHandler first?");
                GameErrorChecker.ThrowGameError(GameError.ConfigueNotRight, null);
                return;
            }

            //Init system services
            RegSystemService("GameMediator", new GameMediator());

            GameManager.GameMediator = (GameMediator)GetSystemService("GameMediator");
            GameManager.GameMediator.RegisterEventHandler(GamePackage.GetSystemPackage(),
                GameEventNames.EVENT_BASE_INIT_FINISHED, "DebuggerHandler", (evtName, param) =>
                {
                    StartRunDebugProvider();
                    return false;
                });

            //Init
            RegSystemService("GamePackageManager", new GamePackageManager());
            RegSystemService("GameUIManager", new GameUIManager());

            //Call init
            sysHandler(ACTION_INIT);

            Log.D(TAG, "System init ok");
        }
        /// <summary>
        /// 消毁
        /// </summary>
        public static void Destroy()
        {
            sysHandler?.Invoke(ACTION_DESTROY);

            //Destroy system service
            List<string> serviceNames = new List<string>(systemService.Keys);
            for(int i = serviceNames.Count - 1; i >= 0; i--)
                systemService[serviceNames[i]].Destroy();
            serviceNames.Clear();
            systemService.Clear();

            UnityLogCatcher.Destroy();

            Log.D(TAG, "System destroy");
        }
        /// <summary>
        /// 强制停止游戏
        /// </summary>
        public static void ForceInterruptGame()
        {
            sysHandler?.Invoke(ACTION_FORCE_INT);
        }

        #endregion
    }
}
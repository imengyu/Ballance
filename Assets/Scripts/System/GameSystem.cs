using Ballance2.System.Debug;
using Ballance2.System.Services;
using Ballance2.System.Utils;
using Ballance2.Utils;
using System.Collections.Generic;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameSystem.cs
* 
* 用途：
* 游戏的基础系统。
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

namespace Ballance2.System
{
    /// <summary>
    /// 基础系统
    /// </summary>
    public static class GameSystem
    {
        private const string TAG = "GameSystem";
        
        /// <summary>
        /// 系统接管器回调
        /// </summary>
        /// <param name="act">当前操作</param>
        public delegate void SysHandler(int act);

        private static SysHandler sysHandler = null;

        public const int ACTION_INIT = 1;
        public const int ACTION_DESTROY = 2;
        public const int ACTION_FORCE_INT = 3;

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
            object o = null;
            if(!systemService.TryGetValue(name, out o))
                GameErrorChecker.LastError = GameError.NotRegister;
            return o;
        }

        /// <summary>
        /// 注册系统接管器
        /// </summary>
        /// <param name="handler">系统接管器</param>
        public static void RegSysHandler(SysHandler handler)
        {
            if(sysHandler != null)
            {
                Log.E("GameSystemInit", "SysHandler already set ");
                return;
            }
            sysHandler = handler;
        }

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
            RegSystemService("GameUIManager", new GameUIManager());
            RegSystemService("GamePackageManager", new GamePackageManager());

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
            foreach(string name in systemService.Keys)
                systemService[name].Destroy();
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

using Ballance2.System.Bridge;
using Ballance2.System.Entry;
using Ballance2.System.Services;
using Ballance2.Utils;
using SLua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ballance2.System
{
    /// <summary>
    /// 游戏管理器
    /// </summary>
    [CustomLuaClass]
    public partial class GameManager : MonoBehaviour
    {
        /// <summary>
        /// 实例
        /// </summary>
        public static GameManager Instance { get; private set; }
        /// <summary>
        /// GameMediator 实例
        /// </summary>
        public static GameMediator GameMediator { get; internal set; }


        /// <summary>
        /// 游戏全局Lua虚拟机
        /// </summary>
        public LuaSvr.MainState GameMainLuaState { get; private set; }
        /// <summary>
        /// 基础摄像机
        /// </summary>
        public Camera GameBaseCamera { get; private set; }
        /// <summary>
        /// 根Canvas
        /// </summary>
        public RectTransform GameCanvas { get; private set; }

        private readonly string TAG = "GameManager";

        /// <summary>
        /// 清空整个场景
        /// </summary>
        internal void ClearScense()
        {
            foreach (Camera c in Camera.allCameras)
                c.gameObject.SetActive(false);
            for (int i = 0, c = GameCanvas.transform.childCount; i < c; i++)
            {
                GameObject go = GameCanvas.transform.GetChild(i).gameObject;
                if (go.name == "GameUIWindow")
                {
                    GameObject go1 = null;
                    for (int j = 0, c1 = go.transform.childCount; j < c1; j++)
                    {
                        go1 = go.transform.GetChild(j).gameObject;
                        if (go1.name != "GameUIWindow_Debug Window")
                            go1.SetActive(false);
                    }
                }
                else if (go.name != "GameUIDebugToolBar" && go.name != "GameUIWindow_Debug Window")
                    go.SetActive(false);
            }
            for (int i = 0, c = transform.childCount; i < c; i++)
            {
                GameObject go = transform.GetChild(i).gameObject;
                if (go.name != "GameManager")
                    go.SetActive(false);
            }
            GameBaseCamera.gameObject.SetActive(true);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        internal void Initialize(GameEntry gameEntryInstance)
        {
            Log.D(TAG, "Initialize");

            Instance = this;
            GameBaseCamera = gameEntryInstance.GameBaseCamera;
            GameCanvas = gameEntryInstance.GameCanvas;

            GameMainLuaState = new LuaSvr.MainState();
            GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_BASE_INIT_FINISHED, "*");

            //Run init
            StartCoroutine(InitAsysc());
        }

        private IEnumerator InitAsysc()
        {
            //LoadSystem packages
            yield return StartCoroutine(LoadSystemPackages());



            yield break;
        }
        private IEnumerator LoadSystemPackages()
        {
            yield break;
        }

        /// <summary>
        /// 释放
        /// </summary>
        internal void Destroy()
        {
            Log.D(TAG, "Destroy");

            if (GameMainLuaState != null)
                GameMainLuaState = null;
        }

        /// <summary>
        /// 开始退出游戏流程
        /// </summary>
        public void QuitGame()
        {
            Log.D(TAG, "QuitGame start");
        }

        /// <summary>
        /// 获取系统服务
        /// </summary>
        /// <typeparam name="T">继承于GameService的服务类型</typeparam>
        /// <param name="name">服务名称</param>
        /// <returns>返回服务实例，如果没有找到，则返回null</returns>
        public T GetSystemService<T>(string name) where T : GameService
        {
            return (T)GameSystem.GetSystemService(name);
        }
        /// <summary>
        /// 获取系统服务
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <returns>返回服务实例，如果没有找到，则返回null</returns>
        public GameService GetSystemService(string name)
        {
            return GameSystem.GetSystemService(name);
        }
        /// <summary>
        /// 设置基础摄像机状态
        /// </summary>
        /// <param name="visible">是否显示</param>
        public void SetGameBaseCameraVisible(bool visible)
        {
            GameBaseCamera.gameObject.SetActive(visible);
        }



    }
}

using Ballance2.Config;
using Ballance2.Config.Settings;
using Ballance2.Sys.Bridge;
using Ballance2.Sys.Debug;
using Ballance2.Sys.Entry;
using Ballance2.Sys.Package;
using Ballance2.Sys.Res;
using Ballance2.Sys.Services;
using Ballance2.Utils;
using SLua;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace Ballance2.Sys
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
        /// 游戏全局Lua虚拟机
        /// </summary>
        public LuaSvr GameMainLuaSvr { get; private set; }
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
                else if (go.name != "DebugToolbar" && go.name != "GameUIWindow_Debug Window")
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


            GameMainLuaSvr = new LuaSvr();
            GameMainLuaSvr.init(null, () =>
            {
                GameMainLuaState = LuaSvr.mainState;

                GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_BASE_INIT_FINISHED, "*");

                //Run init
                StartCoroutine(InitAsysc());
            });
        }

        private IEnumerator InitAsysc()
        {
            //检测lua绑定状态
            object o = GameMainLuaState.doString("return Ballance2.Sys.GameManager.LuaBindingCallback()");
            if (o != null &&  (
                    (o.GetType() == typeof(int) && (int)o == GameConst.GameBulidVersion)
                    || (o.GetType() == typeof(double) && (double)o == GameConst.GameBulidVersion)
                ))
                Log.D(TAG, "Game Lua bind check ok.");
            else
            {
                Log.E(TAG, "Game Lua bind check failed, did you bind lua functions?");
                GameErrorChecker.LastError = GameError.LuaBindCheckFailed;
#if UNITY_EDITOR // 编辑器中
                GameErrorChecker.ThrowGameError(GameError.LuaBindCheckFailed,
                    "Lua接口没有绑定。请点击“SLua”>“All”>“Make”生成 Lua 接口绑定。");
#else
                GameErrorChecker.ThrowGameError(GameError.LuaBindCheckFailed, "错误的发行配置，请检查。");  
#endif
                yield break;
            }

            //LoadSystem packages
            yield return StartCoroutine(LoadSystemInit());



            yield break;
        }
        private IEnumerator LoadSystemInit()
        {
            //读取读取SystemInit文件
            XmlDocument systemInit = new XmlDocument();
#if UNITY_EDITOR
            if (DebugSettings.Instance.SystemInitLoadWay == LoadResWay.InUnityEditorProject
                && File.Exists("Assets/Packages/system_SystemInit.xml"))
            {
                Log.D(TAG, "Load SystemInit in editor.");
                systemInit.Load("Assets/Packages/system_SystemInit.xml");
            }
            else
            {
#else
            if(true) {
#endif
                string url = GamePathManager.GetResRealPath("systeminit", "");
                UnityWebRequest request = UnityWebRequest.Get(url);
                yield return request.SendWebRequest();
                if (!string.IsNullOrEmpty(request.error))
                {
                    //加载失败
                    StopAllCoroutines();
                    if (request.responseCode == 404)
                        GameErrorChecker.ThrowGameError(GameError.FileNotFound, "未找到 SystemInit\n您可尝试重新安装游戏");
                    else
                        GameErrorChecker.ThrowGameError(GameError.FileNotFound, "读取 SystemInit 失败：" + request.responseCode + "\n您可尝试重新安装游戏");
                    yield break;
                }
                systemInit.LoadXml(request.downloadHandler.text);
            }

            
            var pm = GetSystemService<GamePackageManager>("GamePackageManager");

            //加载SystemPackages中定义的包
            XmlNode nodeSystemPackages = systemInit.SelectSingleNode("System/SystemPackages");
            for(int i = 0; i < nodeSystemPackages.ChildNodes.Count; i++)
            {
                XmlNode nodePackage = nodeSystemPackages.ChildNodes[i];

                if (nodePackage.Name == "Package" && nodePackage.Attributes["name"] != null)
                {
                    string packageName = nodePackage.Attributes["name"].InnerText;
                    int minVer = 0;
                    bool mustLoad = false;
                    foreach (XmlAttribute attribute in nodePackage.Attributes)
                    {
                        if (attribute.Name == "minVer")
                            minVer = ConverUtils.StringToInt(attribute.Value, 0, "Package/minVer");
                        else if (attribute.Name == "mustLoad")
                            mustLoad = ConverUtils.StringToBoolean(attribute.Value, false, "Package/mustLoad");
                    }
                    if(string.IsNullOrEmpty(packageName))
                    {
                        Log.W(TAG, "The Package node {0} name is empty!", i);
                        continue;
                    }

                    //加载包
                    Task<bool> task = pm.LoadPackage(packageName);
                    yield return new WaitUntil(() => task.IsCompleted);

                    if (task.Result)
                    {
                        GamePackage package = pm.FindPackage(packageName);
                        if (package == null)
                        {
                            StopAllCoroutines();
                            GameErrorChecker.ThrowGameError(GameError.UnKnow, packageName + " not found!\n请尝试重启游戏");
                            yield break;
                        }
                        if (package.PackageVersion < minVer)
                        {
                            StopAllCoroutines();
                            GameErrorChecker.ThrowGameError(GameError.SystemPackageNotLoad,
                                string.Format("模块 {0} 版本过低：{1} 小于所需版本 {2}, 您可尝试重新安装游戏", 
                                packageName, package.PackageVersion, minVer));
                            yield break;
                        }
                    }
                    else
                    {
                        if (mustLoad)
                        {
                            StopAllCoroutines();
                            GameErrorChecker.ThrowGameError(GameError.SystemPackageNotLoad,
                                "系统定义的模块：" + packageName + " 未能加载成功\n错误：" +
                                GameErrorChecker.GetLastErrorMessage() + " (" + GameErrorChecker.LastError + ")\n请尝试重启游戏");
                            yield break;
                        }
                    }
                }
            }
            yield return new WaitForSeconds(1);

            //全部加载完毕之后通知所有模块初始化
            pm.NotifyAllPackageRun("*");
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

        /// <summary>
        /// 获取游戏版本
        /// </summary>
        /// <returns></returns>
        public static int LuaBindingCallback()
        {
            return GameConst.GameBulidVersion;
        }

    }
}

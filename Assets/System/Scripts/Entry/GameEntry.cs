using Ballance2.Config;
using Ballance2.Services.Debug;
using Ballance2.Services.Init;
using Ballance2.Services.InputManager;
using Ballance2.Tests;
using Ballance2.UI.CoreUI;
using SubjectNerd.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameEntry.cs
* 
* 用途：
* 此模块为整个游戏的入口，负责启动整个游戏框架，以及全局的配置、调试、命令行设置。
* 并且负责用户协议对话框的显示，用户点击用户协议相关。
*
* 作者：
* mengyu
*/

namespace Ballance2.Entry
{
  public class GameEntry : MonoBehaviour
  {
    public static GameEntry Instance { get; private set; }

    #region 全局静态配置属性

    [Tooltip("启用调试模式。Editor下默认为调试模式")]
    public bool DebugMode = false;
    [Tooltip("目标帧率（DebugSetFrameRate 为 true 时有效）")]
    [Range(10, 120)]
    public int DebugTargetFrameRate = 60;
    [Tooltip("是否设置固定帧率（仅Editor中有效）")]
    public bool DebugSetFrameRate = true;
    [Tooltip("是否启用Lua调试器")]
    public bool DebugEnableLuaDebugger = true;
    [Tooltip("Lua调试器类型")]
    public GameLuaDebuggerType DebugLuaDebugger = GameLuaDebuggerType.VscodeDebuggee;
    [Tooltip("调试类型")]
    public GameDebugType DebugType = GameDebugType.NoDebug;
    [Tooltip("当前调试中需要初始化的包名")]
    [Reorderable("DebugInitPackages", true, "PackageName")]
    public List<GameDebugPackageInfo> DebugInitPackages = null;
    [Tooltip("自定义调试用例入口事件名称。进入调试之后会发送一个指定的全局事件，自定义调试用例可以根据这个事件作为调试入口。")]
    public string DebugCustomEntryEvent = "DebugEntry";
    [SerializeField]
    [Tooltip("自定义调试用例入口事件名称预设")]
    [Reorderable("DebugCustomEntries", true, "", true, true)]
    public List<string> DebugCustomEntries = new List<string>();
    [Tooltip("是否在系统或自定义调试模式中加载用户自定义模块包")]
    public bool DebugLoadCustomPackages = true;
    [Tooltip("是否在系统或自定义调试模式中跳过Intro")]
    public bool DebugSkipIntro = false;

    #endregion

    #region 静态引入

    public Camera GameBaseCamera = null;
    public RectTransform GameCanvas = null;

    public GameGlobalErrorUI GameGlobalErrorUI = null;
    public Text GameDebugBeginStats;

    public GameObject GlobalGamePermissionTipDialog = null;
    public GameObject GlobalGameUserAgreementTipDialog = null;
    public GameObject GlobalSplash = null;
    public GlobalGameScriptErrDialog GlobalGameScriptErrDialog = null;
    public GameObject GlobalGameSysErrMessageDebuggerTipDialog = null;
    public Text GlobalGameSysErrMessageDebuggerTipDialogText = null;

    private bool GlobalGamePermissionTipDialogClosed = false;
    private bool GlobalGameUserAgreementTipDialogClosed = false;

    #endregion

    void Start()
    {
      Instance = this;

      InitCommandLine();
      InitBaseSettings();

      if (DebugMode && DebugSetFrameRate) Application.targetFrameRate = DebugTargetFrameRate;

      StartCoroutine(InitMain());
    }
    private void OnDestroy()
    {
      GameSystem.Destroy();
    }
    public static void Destroy()
    {
      UnityEngine.Object.Destroy(Instance.gameObject);
    }

    #region 用户许可相关

    /// <summary>
    /// 显示许可对话框
    /// </summary>
    /// <returns></returns>
    private bool ShowUserArgeement()
    {
      if (PlayerPrefs.GetInt("UserAgreementAgreed", 0) == 0)
      {
        GlobalGameUserAgreementTipDialog.SetActive(true);
        return true;
      }
      return false;
    }
    /// <summary>
    /// 检查android权限是否申请
    /// </summary>
    /// <returns></returns>
    private bool TestAndroidPermission()
    {
#if UNITY_ANDROID
      if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        return true;
#endif
      return false;
    }

    /// <summary>
    /// 用户同意许可
    /// </summary>
    public void ArgeedUserArgeement()
    {
      PlayerPrefs.SetInt("UserAgreementAgreed", 1);
      GlobalGameUserAgreementTipDialogClosed = true;
    }
    /// <summary>
    /// 请求安卓权限
    /// </summary>
    public void RequestAndroidPermission()
    {
#if UNITY_ANDROID
      Permission.RequestUserPermission(Permission.ExternalStorageWrite);
#endif
      GlobalGamePermissionTipDialogClosed = true;
    }
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
      StopAllCoroutines();
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
    /// <summary>
    /// 不同意Android权限
    /// </summary>
    public void DisallowAndroidPermission()
    {
      GlobalGameUserAgreementTipDialogClosed = true;
    }

    #endregion

    private void InitCommandLine()
    {
      string[] CommandLineArgs = Environment.GetCommandLineArgs();
      int len = CommandLineArgs.Length;
      if (len > 1)
      {
        for (int i = 0; i < len; i++)
        {
          if (CommandLineArgs[i] == "-debug")
            PlayerPrefs.SetInt("core.DebugMode", 1);
        }
      }
    }
    private void InitBaseSettings()
    {
#if UNITY_EDITOR
      DebugMode = true;
#else
      if(PlayerPrefs.GetInt("core.DebugMode", 0) > 0) DebugMode = true;
      else DebugMode = false;
#endif
    }
    private void InitUi() {
      GlobalSplash.SetActive(true);
    }

    private IEnumerator InitMain()
    {
      GameSystem.PreInit();
      InitUi();

      if (TestAndroidPermission())
      {
        GlobalGamePermissionTipDialog.SetActive(true);

        yield return new WaitUntil(() => GlobalGamePermissionTipDialogClosed);
      }
      if (ShowUserArgeement())
      {
        yield return new WaitUntil(() => GlobalGameUserAgreementTipDialogClosed);
      }

      GameErrorChecker.SetGameErrorUI(GameGlobalErrorUI);
      GameSystemInit.FillStartParameters(this);

      if (DebugMode && DebugType == GameDebugType.SystemDebug)
        GameSystem.RegSysDebugProvider(GameSystemDebugTests.RequestDebug);
      else if (!DebugMode || DebugType != GameDebugType.SystemDebug)
      {
        GameSystem.RegSysHandler(GameSystemInit.GetSysHandler());
        GameSystem.Init();
      }
      else
        GameErrorChecker.ThrowGameError(GameError.ConfigueNotRight, "DebugMode not right.");
    }
  }
  /// <summary>
  /// Lua调试器类型
  /// </summary>
  public enum GameLuaDebuggerType
  {
    /// <summary>
    /// mobdebug
    /// </summary>
    Mobdebug,
    /// <summary>
    /// vscode-debuggee
    /// </summary>
    VscodeDebuggee,
  }
  /// <summary>
  /// 调试类型
  /// </summary>
  public enum GameDebugType
  {
    /// <summary>
    /// 正常运行。
    /// </summary>
    NoDebug,
    /// <summary>
    /// 完整的调试，包括系统调试和自定义调试，包含完整的游戏运行环境。
    /// </summary>
    FullDebug,
    /// <summary>
    /// 自定义调试。不包含系统测试，包含半完整的游戏运行环境。
    /// </summary>
    CustomDebug,
    /// <summary>
    /// 系统调试。此模式不会加载游戏运行环境。
    /// </summary>
    SystemDebug
  }
  [Serializable]
  public class GameDebugPackageInfo
  {
    public bool Enable;
    public string PackageName;
    public override string ToString() { return PackageName; }
  }
}

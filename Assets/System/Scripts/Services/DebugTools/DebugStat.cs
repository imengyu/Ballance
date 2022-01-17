using Ballance2.Base;
using Ballance2.Config;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.UI.Core;
using Ballance2.Utils;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* DebugStat.cs
* 
* 用途：
* 调试信息显示工具类。
*
* 作者：
* mengyu
*/

class DebugStat : MonoBehaviour
{
  private int tick = 0;
  private string allocatedMemory;
  private string unusedReservedMemory;
  private string reservedMemory;
  private string maxUsedMemory;
  private string usedHeapSizeLong;
  private string monoUsedSizeLong;
  private string monoHeapSizeLong;
  private string allocatedMemoryForGraphicsDriver;

  public Text StatText = null;

  private StringBuilder sb = new StringBuilder();

  private Window WindowSystemInfo = null;
  private Window WindowStats = null;

  private Text DebugStatsText;
  private Text DebugSysinfoText;

  private GameUIManager GameUIManager;

  private void Start()
  {
    var SystemPackage = GamePackage.GetSystemPackage();
    GameUIManager = GameManager.Instance.GetSystemService<GameUIManager>();
    //Profiler.enabled = true;

    //操作
    GameManager.Instance.GameActionStore.RegisterAction(SystemPackage, "DbgStatShowSystemInfo", "DebugStat", (param) => {
      WindowSystemInfo.SetVisible((bool)param[0]);
      return GameActionCallResult.SuccessResult;
    }, new string[] { "bool" });
      GameManager.Instance.GameActionStore.RegisterAction(SystemPackage, "DbgStatShowStats", "DebugStat", (param) => {
      WindowStats.SetVisible((bool)param[0]);
      return GameActionCallResult.SuccessResult;
    }, new string[] { "bool" });

    //Window
    var DebugSysinfoWindow = CloneUtils.CloneNewObject(GameStaticResourcesPool.FindStaticPrefabs("DebugSysinfoWindow"), "DebugSysinfoWindow").GetComponent<RectTransform>();
    var DebugStatsWindow = CloneUtils.CloneNewObject(GameStaticResourcesPool.FindStaticPrefabs("DebugStatsWindow"), "DebugStatsWindow").GetComponent<RectTransform>();

    WindowSystemInfo = GameUIManager.CreateWindow("System info", DebugSysinfoWindow, false, 9, -309, 385, 306);
    WindowSystemInfo.CloseAsHide = true;
    WindowStats = GameUIManager.CreateWindow("Statistics", DebugStatsWindow, false, 9, -71, 250, 240);
    WindowStats.CloseAsHide = true;
    WindowStats.CanResize = false;

    DebugStatsText = DebugStatsWindow.transform.Find("DebugStatsText").GetComponent<Text>();
    DebugStatsWindow.transform.Find("CheckBoxBetterMem").GetComponent<Toggle>().onValueChanged.AddListener((b) => betterMemorySize = b);
    DebugSysinfoText = DebugSysinfoWindow.transform.Find("DebugSysinfoText").GetComponent<Text>();

    LoadSysinfoWindowData();
  }
  private void OnDestroy()
  {
    if (GameManager.Instance != null)
    {
      GameManager.Instance.GameActionStore.UnRegisterAction("DbgStatShowSystemInfo");
      GameManager.Instance.GameActionStore.UnRegisterAction("DbgStatShowStats");
    }
    if (WindowSystemInfo != null)
    {
      WindowSystemInfo.Destroy();
      WindowSystemInfo = null;
    }
    if (WindowStats != null)
    {
      WindowStats.Destroy();
      WindowStats = null;
    }
  }
  private void Update()
  {
    if (tick < 60) tick++;
    else
    {
      tick = 0;

      if (betterMemorySize)
      {
        allocatedMemory = FileUtils.GetBetterFileSize(Profiler.GetTotalAllocatedMemoryLong());
        unusedReservedMemory = FileUtils.GetBetterFileSize(Profiler.GetTotalUnusedReservedMemoryLong());
        reservedMemory = FileUtils.GetBetterFileSize(Profiler.GetTotalReservedMemoryLong());
        maxUsedMemory = FileUtils.GetBetterFileSize(Profiler.maxUsedMemory);
        usedHeapSizeLong = FileUtils.GetBetterFileSize(Profiler.usedHeapSizeLong);
        monoUsedSizeLong = FileUtils.GetBetterFileSize(Profiler.GetMonoUsedSizeLong());
        monoHeapSizeLong = FileUtils.GetBetterFileSize(Profiler.GetMonoHeapSizeLong());
        allocatedMemoryForGraphicsDriver = FileUtils.GetBetterFileSize(Profiler.GetAllocatedMemoryForGraphicsDriver());
      }
      else
      {
        maxUsedMemory = Profiler.maxUsedMemory.ToString();
        allocatedMemory = Profiler.GetTotalAllocatedMemoryLong().ToString();
        unusedReservedMemory = Profiler.GetTotalUnusedReservedMemoryLong().ToString();
        reservedMemory = Profiler.GetTotalReservedMemoryLong().ToString();
        usedHeapSizeLong = Profiler.usedHeapSizeLong.ToString();
        monoUsedSizeLong = Profiler.GetMonoUsedSizeLong().ToString();
        monoHeapSizeLong = Profiler.GetMonoHeapSizeLong().ToString();
        allocatedMemoryForGraphicsDriver = Profiler.GetAllocatedMemoryForGraphicsDriver().ToString();
      }

      if (WindowStats.GetVisible())
        UpdateStatsWindow();

      sb.Clear();
      sb.Append("Mem A:");
      sb.Append(allocatedMemory);
      sb.Append("/U:");
      sb.Append(usedHeapSizeLong);
      sb.Append("/R:");
      sb.Append(monoHeapSizeLong);

      if (StatText != null) StatText.text = sb.ToString();
    }
  }

  private bool betterMemorySize = true;

  private void UpdateStatsWindow()
  {
    StringBuilder sb = new StringBuilder();

    sb.AppendLine("ProfilerEnabled : " + Profiler.enabled.ToString());
    sb.AppendLine("ProfilerSupported : " + Profiler.supported.ToString());
    sb.AppendLine("MaxUsedMemory: " + maxUsedMemory);
    sb.AppendLine("UsedHeapSize: " + usedHeapSizeLong);
    sb.AppendLine("MonoUsedSize: " + monoUsedSizeLong);
    sb.AppendLine("MonoHeapSize: " + monoHeapSizeLong);
    sb.AppendLine("AllocatedMemoryForGraphicsDriver: " + allocatedMemoryForGraphicsDriver);
    sb.AppendLine("TotalAllocatedMemory: " + allocatedMemory);
    sb.AppendLine("TotalReservedMemory: " + reservedMemory);
    sb.AppendLine("TotalUnusedReservedMemory: " + unusedReservedMemory);

    DebugStatsText.text = sb.ToString();
  }
  private void LoadSysinfoWindowData()
  {
    StringBuilder sb = new StringBuilder();

    sb.AppendLine("GameVersion: " + GameConst.GameVersion);
    sb.AppendLine("GameBulidVersion: " + GameConst.GameBulidVersion);
    sb.AppendLine("GamePlatform: " + GameConst.GamePlatform + " (" + GameConst.GamePlatformIdentifier + ")");
    sb.AppendLine("GameScriptBackend: " + GameConst.GameScriptBackend);

    sb.AppendLine("buildGUID: " + Application.buildGUID);
    sb.AppendLine("dataPath: " + Application.dataPath);
    sb.AppendLine("isBatchMode: " + Application.isBatchMode);
    sb.AppendLine("isEditor: " + Application.isEditor);
    sb.AppendLine("isFocused: " + Application.isFocused);
    sb.AppendLine("isMobilePlatform: " + Application.isMobilePlatform);
    sb.AppendLine("platform: " + Application.platform);
    sb.AppendLine("systemLanguage: " + Application.systemLanguage);
    sb.AppendLine("unityVersion: " + Application.unityVersion);

    DebugSysinfoText.text = sb.ToString();
  }
}

using Ballance2.Config;
using Ballance2.Sys.UI.Utils;
using Ballance2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

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
    public FPSManager fPSManager = null;

    private StringBuilder sb = new StringBuilder();

    private void Start()
    {
        //Profiler.enabled = true;
        EventTriggerListener.Get(StatText.gameObject).onClick = (go) => showStats = true;
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

            sb.Clear();
            sb.Append("Mem A:");
            sb.Append(allocatedMemory);
            sb.Append("/U:");
            sb.Append(unusedReservedMemory);
            sb.Append("/R:");
            sb.Append(reservedMemory);

            if (StatText != null) StatText.text = sb.ToString();
        }
    }
    private void OnGUI()
    {
        if (showSystemInfo)
            sysinfoWindowRect = GUI.Window(2, sysinfoWindowRect, SysinfoWindowFun, "System Info");
        if (showStats)
            statsWindowRect = GUI.Window(3, statsWindowRect, StatsWindowFun, "Statistics");
    }

    private Vector2 scrollPosition2;
    private Vector2 scrollPosition3;
    private bool showSystemInfo = false;
    private bool showStats = false;
    private Rect sysinfoWindowRect = new Rect(220, 32, 400, 350);
    private Rect statsWindowRect = new Rect(220, 32, 350, 280);

    private bool betterMemorySize = true;

    readonly Rect titleBarRect = new Rect(0, 0, 10000, 20);

    void StatsWindowFun(int windowid)
    {
        scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2);

        GUIStyle styleLeft = new GUIStyle();
        styleLeft.alignment = TextAnchor.MiddleRight;
        styleLeft.normal.textColor = new Color(0.733f, 0.733f, 0.733f);

        GUILayout.Space(15);

        GUILayout.BeginHorizontal(); GUILayout.Label("FPS: ", styleLeft, GUILayout.MaxWidth(216f)); GUILayout.Label(fPSManager.fps.ToString("0.00"), GUILayout.MaxWidth(100f)); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.Label("ProfilerEnabled : ", styleLeft, GUILayout.MaxWidth(216f)); GUILayout.Label(Profiler.enabled.ToString(), GUILayout.MaxWidth(100f)); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.Label("ProfilerSupported : ", styleLeft, GUILayout.MaxWidth(216f)); GUILayout.Label(Profiler.supported.ToString(), GUILayout.MaxWidth(100f)); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.Label("MaxUsedMemory: ", styleLeft, GUILayout.MaxWidth(216f)); GUILayout.Label(maxUsedMemory, GUILayout.MaxWidth(100f)); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.Label("UsedHeapSize: ", styleLeft, GUILayout.MaxWidth(216f)); GUILayout.Label(usedHeapSizeLong, GUILayout.MaxWidth(100f)); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.Label("MonoUsedSize: ", styleLeft, GUILayout.MaxWidth(216f)); GUILayout.Label(monoUsedSizeLong, GUILayout.MaxWidth(100f)); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.Label("MonoHeapSize: ", styleLeft, GUILayout.MaxWidth(216f)); GUILayout.Label(monoHeapSizeLong, GUILayout.MaxWidth(100f)); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.Label("AllocatedMemoryForGraphicsDriver: ", styleLeft, GUILayout.MaxWidth(216f)); GUILayout.Label(allocatedMemoryForGraphicsDriver, GUILayout.MaxWidth(100f)); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.Label("TotalAllocatedMemory: ", styleLeft, GUILayout.MaxWidth(216f)); GUILayout.Label(allocatedMemory, GUILayout.MaxWidth(100f)); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.Label("TotalReservedMemory: ", styleLeft, GUILayout.MaxWidth(216f)); GUILayout.Label(reservedMemory, GUILayout.MaxWidth(100f)); GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(); GUILayout.Label("TotalUnusedReservedMemory: ", styleLeft, GUILayout.MaxWidth(216f)); GUILayout.Label(unusedReservedMemory, GUILayout.MaxWidth(100f)); GUILayout.EndHorizontal();

        GUILayout.Space(15);
        betterMemorySize = GUILayout.Toggle(betterMemorySize, "BetterMemorySize");
        GUILayout.Space(5);
        if (GUILayout.Button("Sysinfo"))
            showSystemInfo = true;
        GUILayout.Space(5);
        if (GUILayout.Button("Close"))
            showStats = false;
        GUILayout.Space(5);

        GUILayout.EndScrollView();
        GUI.DragWindow(titleBarRect);
    }
    void SysinfoWindowFun(int windowid)
    {
        scrollPosition3 = GUILayout.BeginScrollView(scrollPosition3);

        GUILayout.Label("GameVersion: " + GameConst.GameVersion);
        GUILayout.Label("GameBulidVersion: " + GameConst.GameBulidVersion);
        GUILayout.Label("GamePlatform: " + GameConst.GamePlatform + " (" + GameConst.GamePlatformIdentifier + ")");
        GUILayout.Label("GameScriptBackend: " + GameConst.GameScriptBackend);

        GUILayout.Label("buildGUID: " + Application.buildGUID);
        GUILayout.Label("dataPath: " + Application.dataPath);
        GUILayout.Label("isBatchMode: " + Application.isBatchMode);
        GUILayout.Label("isEditor: " + Application.isEditor);
        GUILayout.Label("isFocused: " + Application.isFocused);
        GUILayout.Label("isMobilePlatform: " + Application.isMobilePlatform);
        GUILayout.Label("platform: " + Application.platform);
        GUILayout.Label("systemLanguage: " + Application.systemLanguage);
        GUILayout.Label("unityVersion: " + Application.unityVersion);

        GUILayout.Space(20);
        GUILayout.Space(20);

        if (GUILayout.Button("Close"))
            showSystemInfo = false;

        GUILayout.EndScrollView();
        GUI.DragWindow(titleBarRect);
    }

}

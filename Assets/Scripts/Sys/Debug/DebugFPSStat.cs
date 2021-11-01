using System.Text;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* FPSManager.cs
* 
* 用途：
* FPS显示工具类。
*
* 作者：
* mengyu
*/

public class DebugFPSStat : MonoBehaviour
{
    public float updateInterval = 0.5F;
    private double lastInterval;
    private double lastFpsInterval;
    private int frames = 0;
    private int tick = 0;
    public float fps;
    public Text FpsText;

    private StringBuilder sb = new StringBuilder();

    void Awake()
    {
        //Application.runInBackground = true;
        //Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //Application.targetFrameRate = 30;
    }
    void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }
    void Update()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            fps = (float)(frames / (timeNow - lastInterval));
            frames = 0;
            lastInterval = timeNow;
        }
        lastFpsInterval = timeNow;
        if (tick < 60) tick++;
        else
        {
            tick = 0;

            sb.Clear();
            sb.Append("FPS:");
            sb.Append(fps.ToString("0.0"));
            sb.Append(" (");
            sb.Append((1000 * Time.deltaTime).ToString("0.00"));
            sb.Append("ms)");

            if (FpsText != null) FpsText.text = sb.ToString(); 
        }
    }
}

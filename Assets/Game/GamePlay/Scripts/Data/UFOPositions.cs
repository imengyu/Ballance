
using System.Collections.Generic;
using UnityEngine;

namespace Ballance2.Game.GamePlay
{
  public struct UFOPositionItem {
    public Vector3 pos;
    public float flyTime;
    public float waitTime;
    public bool catchBall;
    public bool startBall;

    public UFOPositionItem(
      Vector3 pos,
      float flyTime,
      float waitTime,
      bool catchBall = false,
      bool startBall = false)
    {
      this.pos = pos;
      this.flyTime = flyTime;
      this.catchBall = catchBall;
      this.waitTime = waitTime;
      this.startBall = startBall;
    }
  }
  
  /// <summary>
  /// UFO 动画的位置参数
  /// </summary>
  public static class UFOPositions
  {
    /// <summary>
    /// 获取 UFO 动画的位置参数
    /// </summary>
    /// <returns></returns>
    public static List<UFOPositionItem> Data;

    static UFOPositions() {
      Data = new List<UFOPositionItem>();
      Data.Add(new UFOPositionItem(new Vector3(-500, -30, -50), 0.01f, 3));
      Data.Add(new UFOPositionItem(new Vector3(-30, 7, -20), 1.8f, 2));
      Data.Add(new UFOPositionItem(new Vector3(-30, 7, 20), 1.8f, 2));
      Data.Add(new UFOPositionItem(new Vector3(8,0,3), 1.8f, 3));
      Data.Add(new UFOPositionItem(new Vector3(0,6,0), 1.8f, 3));
      Data.Add(new UFOPositionItem(new Vector3(0,6,0), 0.5f, 1.0f, false, true));
      Data.Add(new UFOPositionItem(new Vector3(0,0,0), 0.5f, 1.2f, true));
      Data.Add(new UFOPositionItem(new Vector3(0,0,0), 0.1f, 0.02f));
      Data.Add(new UFOPositionItem(new Vector3(0,5,0), 0.6f, 1));
      Data.Add(new UFOPositionItem(new Vector3(-25,15,0), 1.1f, 2));
      Data.Add(new UFOPositionItem(new Vector3(-20,7,20), 1.0f, 1.5f));
      Data.Add(new UFOPositionItem(new Vector3(-25,10,-50), 1.0f, 1.5f));
      Data.Add(new UFOPositionItem(new Vector3(50,-10,-50), 2.5f, 4));
      Data.Add(new UFOPositionItem(new Vector3(-500,30,250), 1.1f, 1.1f));
      Data.Add(new UFOPositionItem(new Vector3(-200,0,0), 0.9f, 1.8f));
    }
  }
}
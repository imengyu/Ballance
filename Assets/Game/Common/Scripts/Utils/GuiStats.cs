using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/*
 * Copyright (c) 2022 mengyu
 * 
 * 模块名：     
 * GuiStats.cs
 * 
 * 用途：
 * 一个用于GUI显示状态的脚本。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Game
{
  /// <summary>
  /// 一个用于GUI显示状态的脚本。
  /// </summary>
  public class GuiStats : MonoBehaviour {
    private LinkedList<GuiStatsValue> stats = new LinkedList<GuiStatsValue>();

    [Tooltip("如果使用Text模式，请设置要设置数据文字的目标Text")]
    public Text text;
    [Tooltip("是否使用Text模式，否则使用GUI模式，Text模式会将最终数据文字设置到Text上，而GUI模式是直接调用GUI显示在屏幕上")]
    public bool SetToText = false;

    /// <summary>
    /// 添加状态条目
    /// </summary>
    /// <param name="name">条目名称</param>
    /// <returns>返回条目的实例，可以使用它来更新数据</returns>
    public GuiStatsValue AddStat(string name) {
      GuiStatsValue stat = new GuiStatsValue(this, name);
      stats.AddLast(stat);
      return stat;
    }
    /// <summary>
    /// 删除指定条目
    /// </summary>
    /// <param name="stat">条目</param>
    public void DeleteStat(GuiStatsValue stat) {
      stats.Remove(stat);
    }

    public float UpdateTime = 1;

    [Tooltip("如果使用GUI模式，你可以通过设置这个来控制GUI显示区域")]
    public Rect DisplayArea = new Rect(0, 200, 300, 500);

    private float tick = 0;

    private void Update() {
      if(SetToText) {
        tick += Time.deltaTime;
        if(tick >= UpdateTime) {
          StringBuilder sb = new StringBuilder();

          LinkedListNode<GuiStatsValue> value = stats.First;
          while(value != null) {
            sb.AppendLine(value.Value.NameValue);
            value = value.Next;
          }

          text.text = sb.ToString();
          tick = 0;
        }
      }
    }
    private void OnGUI() {
      if(!SetToText) {
        GUILayout.BeginArea(DisplayArea);
        GUILayout.BeginVertical();

        LinkedListNode<GuiStatsValue> value = stats.First;
        while(value != null) {
          GUILayout.Label(value.Value.NameValue);
          value = value.Next;
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
      }
    }
  }

  public class GuiStatsValue {

    private GuiStats stats;
    
    public GuiStatsValue(GuiStats stats, string name) {
      this.stats = stats;
      Name = name;
    }

    /// <summary>
    /// 获取当前条目的名称
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// 获取当前条目的最终字符串
    /// </summary>
    public string NameValue { get; set; } 
    /// <summary> 
    /// 设置当前条目的数据
    /// </summary>
    public string Value {
      set {
        NameValue = Name + ": " + value;
      }
      get {
        return NameValue;
      }
    }
    
    public void SetVector4Value(Vector4 v) { Value = v.ToString(); }
    public void SetVector3Value(Vector3 v) { Value = v.ToString(); }
    public void SetVector2Value(Vector2 v) { Value = v.ToString(); }

    /// <summary>
    /// 删除当前条目
    /// </summary>
    public void Delete() {
      stats.DeleteStat(this);
    }
  }
}
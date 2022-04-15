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
  [SLua.CustomLuaClass]
  [LuaApiDescription("一个用于GUI显示状态的脚本。")]
  [LuaApiNotes(@"此类的作用是，因为在 Lua 中有时需要显示一些调试参数，显示调试参数一般在 C#中是使用 GUI.Label 显示一些数据的，
但在 Lua 中调用 GUI 相关函数是非常消耗性能的，所以就有了这个小工具。

它的作用是，当Lua需要显示某些调试数据在UI上是，Lua只需要告诉它需要显示什么数据，
然后它就会帮助你在C#端显示GUI或者设置Text，Lua可以等到数据变化时才更新到UI上，也可以选择定时更新，
不需要在 Lua 中频繁调用 GUI 而造成性能开销。
", @"
使用示例，例如，我可以在UI上绑定一个 GuiStats 组件，然后在代码中获取它，添加相应的数据条目：
```lua
---添加相应的数据条目
local statPos = self._GuiStats:AddStat('Pos')

---当数据有更新，调用更新显示状态
statPos:SetVector3Value(self.transform.position)

---当不需要再显示这个数据，可以调用删除
statPos:Delete()
```
")]
  public class GuiStats : MonoBehaviour {
    private LinkedList<GuiStatsValue> stats = new LinkedList<GuiStatsValue>();

    [Tooltip("如果使用Text模式，请设置要设置数据文字的目标Text")]
    [LuaApiDescription("如果使用Text模式，请设置要设置数据文字的目标Text")]
    public Text text;
    [Tooltip("是否使用Text模式，否则使用GUI模式，Text模式会将最终数据文字设置到Text上，而GUI模式是直接调用GUI显示在屏幕上")]
    [LuaApiDescription("是否使用Text模式，否则使用GUI模式，Text模式会将最终数据文字设置到Text上，而GUI模式是直接调用GUI显示在屏幕上")]
    public bool SetToText = false;

    /// <summary>
    /// 添加状态条目
    /// </summary>
    /// <param name="name">条目名称</param>
    /// <returns>返回条目的实例，可以使用它来更新数据</returns>
    [LuaApiDescription("添加状态条目", "返回条目的实例，可以使用它来更新数据")]
    [LuaApiParamDescription("name", "条目名称")]
    public GuiStatsValue AddStat(string name) {
      GuiStatsValue stat = new GuiStatsValue(this, name);
      stats.AddLast(stat);
      return stat;
    }
    /// <summary>
    /// 删除指定条目
    /// </summary>
    /// <param name="stat">条目</param>
    [LuaApiDescription("删除指定条目")]
    [LuaApiParamDescription("name", "条目")]
    public void DeleteStat(GuiStatsValue stat) {
      stats.Remove(stat);
    }

    [LuaApiDescription("每隔多少秒更新一次文字")]
    [Tooltip("每隔多少秒更新一次文字")]
    public float UpdateTime = 1;

    [Tooltip("如果使用GUI模式，你可以通过设置这个来控制GUI显示区域")]
    [LuaApiDescription("如果使用GUI模式，你可以通过设置这个来控制GUI显示区域")]
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

  [SLua.CustomLuaClass]
  [LuaApiDescription("GUI显示状态的条目")]
  public class GuiStatsValue {

    private GuiStats stats;

    [SLua.DoNotToLua]
    public GuiStatsValue(GuiStats stats, string name) {
      this.stats = stats;
      Name = name;
    }

    /// <summary>
    /// 获取当前条目的名称
    /// </summary>
    [LuaApiDescription("获取当前条目的名称")]
    public string Name { get; }
    /// <summary>
    /// 获取当前条目的最终字符串
    /// </summary>
    [LuaApiDescription("获取当前条目的最终字符串")]
    public string NameValue { get; set; } 
    /// <summary> 
    /// 设置当前条目的数据
    /// </summary>
    [LuaApiDescription("设置当前条目的数据")]
    public string Value {
      set {
        NameValue = Name + ": " + value;
      }
      get {
        return NameValue;
      }
    }
    
    [LuaApiDescription("设置当前条目的 Vector4 数据，数据会自动转为字符串表示形式")]
    [LuaApiParamDescription("v", "条目的 Vector4 数据")]
    public void SetVector4Value(Vector4 v) { Value = v.ToString(); }
    [LuaApiDescription("设置当前条目的 Vector3 数据，数据会自动转为字符串表示形式")]
    [LuaApiParamDescription("v", "条目的 Vector3 数据")]
    public void SetVector3Value(Vector2 v) { Value = v.ToString(); }
    [LuaApiDescription("设置当前条目的 Vector2 数据，数据会自动转为字符串表示形式")]
    [LuaApiParamDescription("v", "条目的 Vector2 数据")]
    public void SetVector2Value(Vector2 v) { Value = v.ToString(); }

    /// <summary>
    /// 删除当前条目
    /// </summary>
    [LuaApiDescription("删除当前条目")]
    public void Delete() {
      stats.DeleteStat(this);
    }
  }
}
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ballance2.UI.Core.Controls
{
  /// <summary>
  /// 一个Tab组件
  /// </summary>
  [SLua.CustomLuaClass]
  [Serializable]
  [LuaApiNoDoc]
  public class TabContent : UIBehaviour
  {
    public RectTransform Tab;
    public Image TabImage;
    public RectTransform TabContentArea;
    public RectTransform TabContentRect;
    public string Name;
    public string Title;
  }
}
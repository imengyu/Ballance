using System;
using System.Collections;
using System.Collections.Generic;
using Ballance2.Services.InputManager;
using Ballance2.UI.Utils;
using Ballance2.Utils;
using SubjectNerd.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* Tab.cs
* 
* 用途：
* 一个Tab组件
* 
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core.Controls
{
  /// <summary>
  /// 一个Tab组件
  /// </summary>
  [ExecuteInEditMode]
  [JSExport]
  [AddComponentMenu("Ballance/UI/Controls/Tab")]
  public class Tab : UIBehaviour
  {
    [Serializable]
    public class TabContent {
      public RectTransform Tab;
      public Image TabImage;
      public RectTransform TabContentArea;
      public RectTransform TabContentRect;
      public string Name;
      public string Title;
    }
    
    [SerializeField]
    public Dictionary<string, TabContent> tabs = new Dictionary<string, TabContent>();
    [SerializeField]
    [HideInInspector]
    public TabContent tabActive = null;

    public Sprite TabImageActive;
    public Sprite TabImageNormal;
    public RectTransform PanelArea;
    public RectTransform PanelPrefab;
    public RectTransform TabPrefab;
    public RectTransform TabArea;
    
    /// <summary>
    /// 获取指定名称的Tab内容
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns>如果找到，则返回Tab内容，否则返回null</returns>
    public RectTransform GetTabContent(string name) {
      if(tabs.TryGetValue(name, out var tab)) 
        return tab.TabContentRect;
      return null;
    }
    /// <summary>
    /// 获取当前Tab数量
    /// </summary>
    /// <returns></returns>
    public int GetTabCount() { return tabs.Count; }  
    /// <summary>
    /// 添加Tab
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="title">标题</param>
    /// <param name="content">内容</param>
    public bool AddTab(string name, string title, RectTransform content) {
      if(tabs.ContainsKey(name))
        return false;
      var tab = new TabContent();
      tab.Name = name;
      tab.Title = title;
      tab.Tab = CloneUtils.CloneNewObjectWithParent(TabPrefab.gameObject, TabArea, "Tab_" + name).transform as RectTransform;
      tab.TabImage = tab.Tab.GetComponent<Image>();

      EventTriggerListener.Get(tab.Tab.gameObject).onClick += (go) => {
        ActiveTab(name);
      };

      var text = tab.Tab.Find("Text");
      if(text != null)
        text.GetComponent<Text>().text = title;

      tab.TabContentArea = CloneUtils.CloneNewObjectWithParent(PanelPrefab.gameObject, PanelArea, "TabPanel_" + name).transform as RectTransform;
      tab.TabContentRect = content;
      if(content != null) {
        content.SetParent(tab.TabContentArea);
        content.localScale = Vector3.one;
        UIAnchorPosUtils.SetUIPos(content, 0, 0, 0, 0);
      }
      tabs.Add(name, tab);
      if(tabActive == null)
        ActiveTab(name);
      return true;
    }    
    /// <summary>
    /// 删除Tab
    /// </summary>
    /// <param name="name">名称</param>
    public bool DeleteTab(string name) {
      if(tabs.ContainsKey(name)) {
        var tab = tabs[name];
        tabs.Remove(name);

        if(tabActive == tab) 
          if(tabs.Count > 0)
            ActiveTab(tabs.Keys.GetEnumerator().Current);
          else
            tabActive = null;
        return true;
      }
      return false;
    } 
    /// <summary>
    /// 激活指定名称Tab
    /// </summary>
    /// <param name="name">名称</param>
    public bool ActiveTab(string name) {
      if(tabs.TryGetValue(name, out var tab)) {
        if(tabActive != tab) {
          if(tabActive != null) {
            tabActive.TabContentArea.gameObject.SetActive(false);
            tabActive.Tab.gameObject.SetActive(false);
            tabActive.TabImage.sprite = TabImageNormal;
          }

          tabActive = tab;
          tabActive.TabContentArea.gameObject.SetActive(true);
          tabActive.Tab.gameObject.SetActive(true);
          tabActive.TabImage.sprite = TabImageActive;
          return true;
        }
      }
      return false;
    }
  }
}

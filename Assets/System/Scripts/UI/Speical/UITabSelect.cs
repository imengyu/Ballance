using System;
using System.Collections.Generic;
using SubjectNerd.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using Ballance2.Utils;
using Ballance2.UI.Core.Controls;
using System.Collections.ObjectModel;
using Ballance2.Services;

namespace Ballance2.UI 
{ 
  public class UITabSelect : MonoBehaviour 
  {
    [Serializable]
    public class TabItem
    {
      public string Title;
      public RectTransform Content;
      public Action OnHideContent;
      public Action OnShowContent;
      public UITabHeaderItem Header;
    }    
   
    [SerializeField]
    private bool useTab = false;
    [SerializeField]
    private GameObject tabPrefab;
    [SerializeField]
    private RectTransform tabHost;
    [SerializeField]
    private UIText text;
    [SerializeField]
    private int startActiveIndex = 0;
    [SerializeField]
    [Reorderable]
    private List<TabItem> tabs = new List<TabItem>();

    public InputAction LeftAction;
    public InputAction RightAction;

    private TabItem currentActiveTab = null;

    private void Start() 
    {
      if (tabs.Count > 0)
      {
        if (useTab)
          UpdateTabItem();
        for (int i = 0; i < tabs.Count; i++) 
        {
          if (tabs[i].Content != null)
            tabs[i].Content.gameObject.SetActive(false);
        }
        JumpToItem(tabs[startActiveIndex]);
      }
      LeftAction.performed += (c) => Prev(true);
      RightAction.performed += (c) => Next(true);
    }

    public TabItem CurrentActiveTab => currentActiveTab;
    public ReadOnlyCollection<TabItem> Tabs => tabs.AsReadOnly();

    public void Next(bool playSound = false)
    {
      var activeIndex = tabs.IndexOf(currentActiveTab);
      if (activeIndex < Tabs.Count - 1)
        JumpToItem(Tabs[activeIndex + 1], playSound);
    }
    public void Prev(bool playSound = false)
    {
      var activeIndex = tabs.IndexOf(currentActiveTab);
      if (activeIndex > 0)
        JumpToItem(Tabs[activeIndex - 1], playSound);
    }

    private void UpdateTabItem()
    {
      tabHost.DestroyAllChildren();
      foreach (var _item in tabs)
      {
        var item = _item;
        var tabHeader = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<UITabHeaderItem>(tabPrefab, tabHost);
        tabHeader.GetComponent<Button>().onClick.AddListener(() => JumpToItem(item));
        tabHeader.SetTitle(item.Title);
        item.Header = tabHeader;
      }
    }

    public void SetTabs(TabItem[] _tabs)
    {
      tabs = new List<TabItem>(_tabs);
      if (useTab)
        UpdateTabItem();
    }
    public void JumpToItem(TabItem tab, bool playSound = false)
    {
      if (currentActiveTab!= null) {
        if (currentActiveTab.Content != null)
          currentActiveTab.Content.gameObject.SetActive(false);
        else if (currentActiveTab.OnHideContent != null)
          currentActiveTab.OnHideContent();
        if (useTab)
          currentActiveTab.Header?.SetActive(false);
      }
      
      if (tab.Content != null)
        tab.Content.gameObject.SetActive(true);
      else if (tab.OnShowContent != null)
        tab.OnShowContent();
      if (!useTab)
        text.text = tab.Title;
      currentActiveTab = tab;
      if (useTab)
        currentActiveTab.Header?.SetActive(true);
      
      if (playSound)
        GameSoundManager.Instance.PlayFastVoice("core.sounds:Menu_click.wav", GameSoundType.UI);
    }

    private void OnEnable() 
    {
      LeftAction.Enable();
      RightAction.Enable();
    }
    private void OnDisable()
    {
      LeftAction.Disable();
      RightAction.Disable();
    }
  }
}
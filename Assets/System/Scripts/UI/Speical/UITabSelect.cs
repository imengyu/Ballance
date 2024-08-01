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
    }    
   
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
        for (int i = 0; i < tabs.Count; i++) 
          tabs[i].Content.gameObject.SetActive(false);
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

    public void SetTabs(TabItem[] _tabs)
    {
      tabs = new List<TabItem>(_tabs);
    }
    public void JumpToItem(TabItem tab, bool playSound = false)
    {
      if (currentActiveTab!= null)
          currentActiveTab.Content.gameObject.SetActive(false);
      tab.Content.gameObject.SetActive(true);
      text.text = tab.Title;
      currentActiveTab = tab;
      
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
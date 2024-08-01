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
    private GameObject tabPrefab;
    [SerializeField]
    private int startActiveIndex;
    [SerializeField]
    [Reorderable]
    private List<TabItem> tabs = new List<TabItem>();
    private RectTransform rectTransform;

    public InputAction LeftAction;
    public InputAction RightAction;

    private TabItem currentActiveTab = null;
    private List<UITabHeaderItem> headers = new List<UITabHeaderItem>();

    private void Awake() 
    {
      rectTransform = GetComponent<RectTransform>();
      UpdateHeader();
      LeftAction.performed += (c) => {
        var activeIndex = tabs.IndexOf(currentActiveTab);
        if (activeIndex > 0)
          JumpToItem(Tabs[activeIndex - 1]);
      };
      RightAction.performed += (c) => {
        var activeIndex = tabs.IndexOf(currentActiveTab);
        if (activeIndex < Tabs.Count - 1)
          JumpToItem(Tabs[activeIndex + 1]);
      };
    }
    private void UpdateHeader()
    {
      rectTransform.DestroyAllChildren();
      headers.Clear();
      foreach (var item in tabs)
      {
        var _item = item;
        var trans = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<UITabHeaderItem>(tabPrefab, rectTransform);
        trans.SetTitle(item.Title);
        trans.GetComponent<Button>().onClick.AddListener(() => JumpToItem(_item));
        headers.Add(trans);
      }
      if (tabs.Count > 0)
        JumpToItem(tabs[startActiveIndex], false);
    }
    public TabItem CurrentActiveTab => currentActiveTab;
    public ReadOnlyCollection<TabItem> Tabs => tabs.AsReadOnly();

    public void SetTabs(TabItem[] _tabs)
    {
      tabs = new List<TabItem>(_tabs);
      UpdateHeader();
    }
    public void JumpToItem(TabItem tab, bool playSound = true)
    {
      var activeIndex = tabs.IndexOf(tab);
      for (int i = 0; i < tabs.Count; i++) {
        if (i != activeIndex)
        {
          tabs[i].Content.gameObject.SetActive(false);
          headers[i].SetActive(false);
        }
      }
      tab.Content.gameObject.SetActive(true);
      headers[activeIndex].SetActive(true);
      
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
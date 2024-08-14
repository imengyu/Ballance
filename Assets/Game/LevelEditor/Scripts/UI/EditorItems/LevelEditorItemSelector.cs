using System;
using System.Collections.Generic;
using Ballance2.Services;
using Ballance2.UI;
using Ballance2.UI.Core.Controls;
using Ballance2.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemSelector : LevelEditorItemBase 
  {
    public RectTransform ContentView;
    public GameObject Prefab;
    public ScrollRect ScrollRect;

    public class LevelEditorItemSelectorItem
    {
      public LevelEditorItemSelectorItem(string title)
      {
        Title = title;
      }
      public LevelEditorItemSelectorItem(string title, Sprite icon)
      {
        Title = title;
        Icon = icon;
      }

      public string Title = "";
      public Sprite Icon = null;
    }

    private List<UIButtonActiveState> SectorButtons = new List<UIButtonActiveState>();

    private void OnEnable() 
    {
      var paramsDict = (Dictionary<string, object>)Params;
      var disableSelect = (bool)paramsDict["disableSelect"];
      var options = (List<LevelEditorItemSelectorItem>)paramsDict["options"];
      var manager = LevelEditorManager.Instance;
      var sectors = manager.LevelCurrent.GetSectorCount();
      SectorButtons.Clear();
      ContentView.DestroyAllChildren();
      for (int i = 0; i < options.Count; i++)
      {
        var item = options[i];
        var index = i;
        var btn = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<UIButtonActiveState>(Prefab, ContentView);
        btn.gameObject.SetActive(true);
        btn.SetActive(currentValue == index);
        btn.onClick = () => {
          if (disableSelect)
            return;
          foreach (var item in SectorButtons)
            item.SetActive(false);
          currentValue = index;
          btn.SetActive(true);
          EmitNewValue(currentValue);
        }; 
        btn.transform.Find("Text").GetComponent<TMP_Text>().text = item.Title;
        var image = btn.transform.Find("Image").GetComponent<Image>();
        image.gameObject.SetActive(item.Icon != null);
        image.sprite = item.Icon;
        SectorButtons.Add(btn);
      }
      GameObject go = new GameObject();
      var space = go.AddComponent<RectTransform>();
      space.sizeDelta = new Vector2(200, 10);
      space.SetParent(ContentView);
      LayoutRebuilder.ForceRebuildLayoutImmediate(ContentView);
      GameManager.Instance.Delay(0.2f, () => ScrollRect.horizontalNormalizedPosition = 0);
    }

    private int currentValue = -1;
    
    public override void UpdateValue(object _value) {
      lockValueChanged = true;

      currentValue = (int)_value;
      foreach (var item in SectorButtons)
        item.SetActive(false);
      if (currentValue < SectorButtons.Count)
        SectorButtons[currentValue].SetActive(true);

      lockValueChanged = false;
    }

    public override string GetEditableType()
    {
      return "SelectorEditor";
    }
  }
}
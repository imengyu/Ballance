using System;
using System.Collections.Generic;
using Ballance2.UI;
using Ballance2.UI.Core.Controls;
using Ballance2.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemSectors : LevelEditorItemBase 
  {
    public RectTransform ContentView;
    public GameObject Prefab;

    private List<UIButtonActiveState> SectorButtons = new List<UIButtonActiveState>();

    private void OnEnable() 
    {
      var paramsDict = (Dictionary<string, bool>)Params;
      var singleSelect = paramsDict["singleSelect"];
      var disableSelect = paramsDict["disableSelect"];
      var manager = LevelEditorManager.Instance;
      var sectors = manager.LevelCurrent.LevelInfo.level.sectorCount;
      SectorButtons.Clear();
      ContentView.DestroyAllChildren();
      for (int i = 1; i <= sectors; i++)
      {
        var sector = i;
        var btn = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<UIButtonActiveState>(Prefab, ContentView);
        btn.gameObject.SetActive(true);
        btn.SetColor(Color.gray, manager.LevelEditorUIControl.GetSectorColor(sector));
        btn.SetActive(currentValue.Contains(sector));
        btn.onClick = () => {
          if (disableSelect)
            return;
          if (singleSelect)
          {
            foreach (var item in SectorButtons)
              item.SetActive(false);
            currentValue.Clear();
            currentValue.Add(sector);
            btn.SetActive(true);
          }
          else {
            if (currentValue.Contains(sector))
            {
              currentValue.Remove(sector);
              btn.SetActive(false);
            }
            else
            {
              currentValue.Add(sector);
              btn.SetActive(true);
            }
          }
          EmitNewValue(currentValue);
        }; 
        SectorButtons.Add(btn);
      }
      GameObject go = new GameObject();
      var space = go.AddComponent<RectTransform>();
      space.sizeDelta = new Vector2(200, 10);
      space.SetParent(ContentView);
      LayoutRebuilder.ForceRebuildLayoutImmediate(ContentView);
    }

    private List<int> currentValue = new List<int>();
    
    public override void UpdateValue(object _value) {
      lockValueChanged = true;

      currentValue = (List<int>)_value;
      foreach (var item in SectorButtons)
        item.SetActive(false);
      foreach (var sector in currentValue)
      {
        var i = sector - 1;
        if (i < SectorButtons.Count)
          SectorButtons[i].SetActive(true);
      }

      lockValueChanged = false;
    }

    public override string GetEditableType()
    {
      return "SectorsEditor";
    }
  }
}
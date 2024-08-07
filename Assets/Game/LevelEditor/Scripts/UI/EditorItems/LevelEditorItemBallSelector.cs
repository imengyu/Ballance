using System;
using System.Collections.Generic;
using Ballance2.Game.GamePlay;
using Ballance2.Services;
using Ballance2.UI;
using Ballance2.UI.Core.Controls;
using Ballance2.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemBallSelector : LevelEditorItemBase 
  {
    public RectTransform ContentView;
    public GameObject Prefab;

    private List<UIButtonActiveState> SectorButtons = new List<UIButtonActiveState>();

    private void OnEnable() 
    {
      var balls = GamePlayManager.Instance.BallManager.GetRegisteredBalls();
      SectorButtons.Clear();
      ContentView.DestroyAllChildren();
      foreach (var ball in balls)
      {
        var ballCurrent = ball;
        var btn = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<UIButtonActiveState>(Prefab, ContentView);
        btn.data = ballCurrent.name;
        btn.gameObject.SetActive(true);
        btn.SetActive(currentValue == ballCurrent.name);
        btn.onClick = () => {
          foreach (var item in SectorButtons)
            item.SetActive(false);
          currentValue = ballCurrent.name;
          btn.SetActive(true);
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

    private string currentValue = "";
    
    public override void UpdateValue(object _value) {
      lockValueChanged = true;

      currentValue = (string)_value;
      foreach (var item in SectorButtons)
        item.SetActive((string)item.data == currentValue);

      lockValueChanged = false;
    }

    public override string GetEditableType()
    {
      return "BallSelector";
    }
  }
}
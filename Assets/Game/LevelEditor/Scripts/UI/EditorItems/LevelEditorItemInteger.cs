using System;
using System.Collections.Generic;
using Ballance2.UI.Core.Controls;
using Ballance2.Utils;
using TMPro;
using UnityEngine;
  
namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemInteger : LevelEditorItemBase 
  {
    public TMP_InputField Value;

    private int currentValue = 0;
    private int stepValue = 0;
    private int maxValue = 0;
    private int minValue = 0;

    private void Awake() 
    {      
      var paramsDict = (Dictionary<string, object>)Params;
      if (paramsDict != null)
      {
        if (paramsDict.TryGetValue("maxValue", out var v)) maxValue = (int)v;
        else maxValue = int.MaxValue;
        if (paramsDict.TryGetValue("maxValue", out v)) minValue = (int)v;
        else minValue = int.MinValue;
        if (paramsDict.TryGetValue("stepValue", out v)) stepValue = (int)v;
        else stepValue = 0;
      }

      Value.onSelect.AddListener((value) => {
        disableTimingUpdate = true;
      });
      Value.onEndEdit.AddListener((value) => {
        disableTimingUpdate = false;
        if (int.TryParse(value, out var v))
          LimitValue(v);
      });
    }

    private void LimitValue(int v)
    {
      v = CommonUtils.LimitNumber(v, minValue, maxValue);
      if (stepValue > 0)
        v = v - (v % stepValue);
      EmitNewValue(v);
    }

    public void UpValue()
    {
      LimitValue(currentValue + stepValue);
    }
    public void DownValue()
    {
      LimitValue(currentValue - stepValue);
    }

    public override string GetEditableType()
    {
      return "Integer";
    }
    public override void UpdateValue(object _value) {
      lockValueChanged = true;
      currentValue = (int)_value;
      Value.text = currentValue.ToString();
      lockValueChanged = false;
    }
  }
}
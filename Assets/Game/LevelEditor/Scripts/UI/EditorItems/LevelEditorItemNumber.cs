using System;
using System.Collections.Generic;
using Ballance2.UI.Core.Controls;
using Ballance2.Utils;
using TMPro;
using UnityEngine;
  
namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemNumber : LevelEditorItemBase 
  {
    public TMP_InputField Value;

    private float currentValue = 0;
    private float stepValue = 0;
    private float maxValue = 0;
    private float minValue = 0;

    private void Awake() 
    {       
      var paramsDict = (Dictionary<string, object>)Params; 
      if (paramsDict != null)
      {
        if (paramsDict.TryGetValue("maxValue", out var v)) maxValue = (float)v;
        else maxValue = float.MaxValue;
        if (paramsDict.TryGetValue("maxValue", out v)) minValue = (float)v;
        else minValue = float.MinValue;
        if (paramsDict.TryGetValue("stepValue", out v)) stepValue = (float)v;
        else stepValue = 0;
      }

      Value.onSelect.AddListener((value) => {
        disableTimingUpdate = true;
      });
      Value.onEndEdit.AddListener((value) => {
        disableTimingUpdate = false;
        if (float.TryParse(value, out var v))
          LimitValue(v);
      });
    }

    private void LimitValue(float v)
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
      return "Float";
    }
    public override void UpdateValue(object _value) {
      lockValueChanged = true;
      currentValue = (float)_value;
      Value.text = currentValue.ToString("F4");
      lockValueChanged = false;
    }
  }
}
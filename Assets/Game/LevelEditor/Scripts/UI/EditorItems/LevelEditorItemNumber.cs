using System;
using Ballance2.UI.Core.Controls;
using TMPro;
using UnityEngine;
  
namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemNumber : LevelEditorItemBase 
  {
    public TMP_InputField Value;

    private void Awake() 
    {      
      Value.onSelect.AddListener((value) => {
        disableTimingUpdate = true;
      });
      Value.onEndEdit.AddListener((value) => {
        disableTimingUpdate = false;
        if (float.TryParse(value, out var v))
          EmitNewValue(v);
      });
    }
    
    public override string GetEditableType()
    {
      return "System.Single";
    }
    public override void UpdateValue(object _value) {
      lockValueChanged = true;
      Value.text = ((float)_value).ToString();
      lockValueChanged = false;
    }
  }
}
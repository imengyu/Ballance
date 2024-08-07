using System;
using Ballance2.UI.Core.Controls;
using TMPro;
using UnityEngine;
  
namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemInteger : LevelEditorItemBase 
  {
    public TMP_InputField Value;

    private void Awake() 
    {      
      Value.onSelect.AddListener((value) => {
        disableTimingUpdate = true;
      });
      Value.onEndEdit.AddListener((value) => {
        disableTimingUpdate = false;
        if (int.TryParse(value, out var v))
          EmitNewValue(v);
      });
    }
    public override string GetEditableType()
    {
      return "System.Int32";
    }
    public override void UpdateValue(object _value) {
      lockValueChanged = true;
      Value.text = ((int)_value).ToString();
      lockValueChanged = false;
    }
  }
}
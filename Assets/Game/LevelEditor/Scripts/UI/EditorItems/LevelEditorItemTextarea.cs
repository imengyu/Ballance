using System;
using Ballance2.UI.Core.Controls;
using TMPro;
using UnityEngine;
  
namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemTextarea : LevelEditorItemBase 
  {
    public TMP_InputField Value;

    private void Awake() 
    {      
      Value.onSelect.AddListener((value) => {
        disableTimingUpdate = true;
      });
      Value.onEndEdit.AddListener((value) => {
        disableTimingUpdate = false;
        EmitNewValue(value);
      });
    }

    public override string GetEditableType()
    {
      return "Textarea";
    }
    public override void UpdateValue(object _value) {
      lockValueChanged = true;
      Value.text = (string)_value;
      lockValueChanged = false;
    }
  }
}
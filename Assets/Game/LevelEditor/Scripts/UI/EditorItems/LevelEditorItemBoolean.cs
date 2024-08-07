using System;
using Ballance2.UI.Core.Controls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemBoolean : LevelEditorItemBase 
  {
    public Toggle Toggle;

    public void OnToggleSwitch(bool v) 
    {
      EmitNewValue(v);
    }
    
    public override string GetEditableType()
    {
      return "System.Boolean";
    }
    public override void UpdateValue(object _value) {
      lockValueChanged = true;
      Toggle.isOn = (bool)_value;
      lockValueChanged = false;
    }
  }
}
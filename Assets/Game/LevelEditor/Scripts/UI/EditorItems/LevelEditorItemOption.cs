using System;
using Ballance2.UI.Core.Controls;
using TMPro;
using UnityEngine;
  
namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemOption : LevelEditorItemBase 
  {
    public TMP_Dropdown Dropdown;

    public void DropdownValueChanged(int c)
    {
      EmitNewValue(c);
    }

    public override string GetEditableType()
    {
      return "OptionEditor";
    }
    public override void UpdateValue(object _value) {
      lockValueChanged = true;
      Dropdown.value = (int) _value;
      lockValueChanged = false;
    }
  }
}
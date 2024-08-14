using System;
using Ballance2.UI.Core.Controls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemAction : LevelEditorItemBase 
  {
    public Button Button;
    public UIText ButtonText;

    public override string GetEditableType()
    {
      return "Action";
    }
  }
}
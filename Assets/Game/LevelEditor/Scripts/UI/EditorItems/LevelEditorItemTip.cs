using Ballance2.UI.Core.Controls;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemTip : LevelEditorItemBase 
  {
    public Image Icon;

    public Sprite IconNone;
    public Sprite IconInfo;
    public Sprite IconWarning;
    public Sprite IconError;
    public Sprite IconSuccess;

    private void Awake()
    {
      Icon.sprite = IconNone;
      var paramsDict = (Dictionary<string, object>)Params;
      if (paramsDict != null)
      {
        if (paramsDict.TryGetValue("type", out var v))
        {
          switch ((LevelEditorConfirmIcon)v)
          {
            case LevelEditorConfirmIcon.Success:
              Icon.sprite = IconSuccess;
              break;
            case LevelEditorConfirmIcon.Warning:
              Icon.sprite = IconWarning;
              break;
            case LevelEditorConfirmIcon.Error:
              Icon.sprite = IconError;
              break;
            case LevelEditorConfirmIcon.Info:
              Icon.sprite = IconInfo;
              break;
            case LevelEditorConfirmIcon.None:
              Icon.sprite = IconNone;
              break;
          }
        }
      }
    }
    public override string GetEditableType()
    {
      return "Tip";
    }
  }
}
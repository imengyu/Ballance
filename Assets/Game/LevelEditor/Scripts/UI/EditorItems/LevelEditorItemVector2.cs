using System;
using Ballance2.UI.Core.Controls;
using TMPro;
using UnityEngine;
  
namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemVector2 : LevelEditorItemBase 
  {
    public TMP_InputField inputFieldX;
    public TMP_InputField inputFieldY;

    private void Awake() 
    {
      inputFieldX.onSelect.AddListener((value) => {
        disableTimingUpdate = true;
      });
      inputFieldY.onSelect.AddListener((value) => {
        disableTimingUpdate = true;
      });
      inputFieldX.onEndEdit.AddListener((v) => {
        disableTimingUpdate = false;
        if (float.TryParse(v, out var f))
        {
          value = new Vector2(f, value.y);
          EmitNewValue(value);
        }
      });
      inputFieldY.onEndEdit.AddListener((v) => {
        disableTimingUpdate = false;
        if (float.TryParse(v, out var f))
        {
          value = new Vector2(value.x, f);
          EmitNewValue(value);
        }
      });
    }

    private Vector2 value = Vector3.zero; 

    public override string GetEditableType()
    {
      return "UnityEngine.Vector2";
    }
    public override void UpdateValue(object _value) {
      lockValueChanged = true;
      value = (Vector2)_value;

      inputFieldX.text = value.x.ToString();
      inputFieldY.text = value.y.ToString();

      lockValueChanged = false;
    }
  }
}
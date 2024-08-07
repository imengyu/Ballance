using System;
using Ballance2.UI.Core.Controls;
using TMPro;
using UnityEngine;
  
namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemVector3 : LevelEditorItemBase 
  {
    public TMP_InputField inputFieldX;
    public TMP_InputField inputFieldY;
    public TMP_InputField inputFieldZ;

    private void Awake() 
    {
      inputFieldX.onSelect.AddListener((value) => {
        disableTimingUpdate = true;
      });
      inputFieldY.onSelect.AddListener((value) => {
        disableTimingUpdate = true;
      });
      inputFieldZ.onSelect.AddListener((value) => {
        disableTimingUpdate = true;
      });
      inputFieldX.onEndEdit.AddListener((v) => {
        disableTimingUpdate = false;
        if (float.TryParse(v, out var f))
        {
          value = new Vector3(f, value.y, value.z);
          EmitNewValue(value);
        }
      });
      inputFieldY.onEndEdit.AddListener((v) => {
        disableTimingUpdate = false;
        if (float.TryParse(v, out var f))
        {
          value = new Vector3(value.x, f, value.z);
          EmitNewValue(value);
        }
      });
      inputFieldZ.onEndEdit.AddListener((v) => {
        disableTimingUpdate = false;
        if (float.TryParse(v, out var f))
        {
          value = new Vector3(value.x, value.y, f);
          EmitNewValue(value);
        }
      });
    }

    private Vector3 value = Vector3.zero; 

    public override string GetEditableType()
    {
      return "UnityEngine.Vector3";
    }
    public override void UpdateValue(object _value) {
      lockValueChanged = true;
      value = (Vector3)_value;

      inputFieldX.text = value.x.ToString();
      inputFieldY.text = value.y.ToString();
      inputFieldZ.text = value.z.ToString();

      lockValueChanged = false;
    }
  }
}
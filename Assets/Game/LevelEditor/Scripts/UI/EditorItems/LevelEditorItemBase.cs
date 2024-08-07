using System;
using Ballance2.UI.Core.Controls;
using UnityEngine;
  
namespace Ballance2.Game.LevelEditor.EditorItems
{
  public class LevelEditorItemBase : MonoBehaviour 
  {
    public UIText Title;

    protected bool lockValueChanged = false;
    protected bool disableTimingUpdate = false;

    protected virtual void EmitNewValue(object value) {
      if (!lockValueChanged)
        OnValueChanged?.Invoke(value);
    }
    
    public virtual string GetEditableType()
    {
      throw new Exception("This LevelEditorItem does not override GetEditableType!");
    }
    public virtual void UpdateValue(object value) {

    }
    
    public object Params = null;
    public Action<object> OnValueChanged;
    public Action OnTimingUpdateValue;

    private int UpdateTick = 0;
    private void FixedUpdate() 
    {
      if (UpdateTick < 60)
        UpdateTick++;
      else
      {
        if (!disableTimingUpdate)
          OnTimingUpdateValue?.Invoke();
        UpdateTick = 0;
      }
    }
  }
}
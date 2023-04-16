using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* ToggleExValueBinder.cs
* 
* 用途：
* ToggleEx控件数据绑定器
*
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core.ValueBinder
{
  [AddComponentMenu("Ballance/UI/ValueBinder/ToggleValueBinder")]
  [RequireComponent(typeof(Toggle))]
  public class ToggleValueBinder : GameUIControlValueBinder
  {
    private Toggle toggle = null;
    
    protected override bool OnBinderSupplierHandle(object value) {
      toggle.isOn = (bool)value;
      return true;
    }

    protected override void BinderBegin()
    {
      toggle = GetComponent<Toggle>();
      toggle.onValueChanged.AddListener((on) =>
      {
        NotifyUserUpdate(on);
      });
    }
  }
}
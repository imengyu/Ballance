using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* DropdownValueBinder.cs
* 
* 用途：
* Dropdown 控件数据绑定器
*
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core.ValueBinder
{
  [AddComponentMenu("Ballance/UI/ValueBinder/DropdownValueBinder")]
  [RequireComponent(typeof(Dropdown))]
  public class DropdownValueBinder : GameUIControlValueBinder
  {
    private TMP_Dropdown dropdown = null;

    protected override bool OnBinderSupplierHandle(object value) {
      dropdown.value = (int)value;
      return true;
    }

    protected override void BinderBegin()
    {
      dropdown = GetComponent<TMP_Dropdown>();
      dropdown.onValueChanged.AddListener((v) =>
      {
        NotifyUserUpdate(v);
      });
    }
  }
}
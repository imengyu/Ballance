using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* InputFieldValueBinder.cs
* 
* 用途：
* InputField 控件数据绑定器
*
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core.ValueBinder
{
  [AddComponentMenu("Ballance/UI/ValueBinder/InputFieldValueBinder")]
  [RequireComponent(typeof(TMP_InputField))]
  public class InputFieldValueBinder : GameUIControlValueBinder
  {
    private TMP_InputField input = null;

    protected override bool OnBinderSupplierHandle(object value)
    {
      input.text = (string)value;
      return true;
    }
    protected override void BinderBegin()
    {
      input = GetComponent<TMP_InputField>();
      input.onValueChanged.AddListener((v) =>
      {
        NotifyUserUpdate(v);
      });
    }
  }
}
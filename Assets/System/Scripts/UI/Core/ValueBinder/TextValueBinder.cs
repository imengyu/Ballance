using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* TextValueBinder.cs
* 
* 用途：
* Texe控件数据绑定器
*
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core.ValueBinder
{
  [AddComponentMenu("Ballance/UI/ValueBinder/TextValueBinder")]
  [RequireComponent(typeof(TMP_Text))]
  public class TextValueBinder : GameUIControlValueBinder
  {
    private TMP_Text text = null;
    protected override bool OnBinderSupplierHandle(object value)
    {
      text.text = (string)value;
      return true;
    }
    protected override void BinderBegin()
    {
      text = GetComponent<TMP_Text>();
    }
  }
}

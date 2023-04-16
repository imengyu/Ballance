using UnityEngine;
using Ballance2.UI.Core.Controls;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* SliderValueBinder.cs
* 
* 用途：
* Slider控件数据绑定器
*
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core.ValueBinder
{
  [AddComponentMenu("Ballance/UI/ValueBinder/KeyChooseValueBinder")]
  [RequireComponent(typeof(KeyChoose))]
  public class KeyChooseValueBinder : GameUIControlValueBinder
  {
    private KeyChoose keyChoose = null;

    protected override bool OnBinderSupplierHandle(object value)
    {
      if (value.GetType() == typeof(int))
        keyChoose.value = (KeyCode)(int)value;
      else if (value.GetType() == typeof(float))
        keyChoose.value = (KeyCode)(int)value;
      else if (value.GetType() == typeof(double))
        keyChoose.value = (KeyCode)(int)(double)value;
      else if (value.GetType() == typeof(KeyCode))
        keyChoose.value = (KeyCode)value;
      else
        UnityEngine.Debug.Log("Unknow type:  " + value.GetType().Name + " " + value);
      return true;
    }
    protected override void BinderBegin()
    {
      keyChoose = gameObject.GetComponent<KeyChoose>();
      keyChoose.onValueChanged.AddListener((v) =>
      {
        NotifyUserUpdate(v);
      });
    }
  }
}
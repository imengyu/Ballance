using UnityEngine;
using UnityEngine.UI;

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

namespace Ballance2.Sys.UI.ValueBinder
{ 
    [AddComponentMenu("Ballance/UI/ValueBinder/SliderValueBinder")]
    [RequireComponent(typeof(Slider))]
    public class SliderValueBinder : GameUIControlValueBinder
    {
        private Slider slider = null;

        protected override void BinderBegin() {
            slider = GetComponent<Slider>();
            BinderSupplierCallback = (value) => {
                if(value.GetType() == typeof(int))
                    slider.value = (float)value;
                else if(value.GetType() == typeof(float)) 
                    slider.value = (float)(int)value;
                else if(value.GetType() == typeof(double)) 
                    slider.value = (float)(double)value;
                else
                    UnityEngine.Debug.Log("Unknow type:  " + value.GetType().Name + " " + value);
                return true;
            };
            slider.onValueChanged.AddListener((v) => {
                NotifyUserUpdate(v);
            });
        }
    }
}
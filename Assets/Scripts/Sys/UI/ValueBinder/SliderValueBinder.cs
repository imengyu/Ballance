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
*
* 更改历史：
* 2021-4-20 创建
*
*/

namespace Ballance2.Sys.UI.ValueBinder
{ 
    [AddComponentMenu("Ballance/UI/ValueBinder/Slider")]
    [RequireComponent(typeof(Slider))]
    public class SliderValueBinder : GameUIControlValueBinder
    {
        private Slider slider = null;

        protected override void BinderBegin() {
            slider = GetComponent<Slider>();
            BinderSupplierCallback = (value) => {
                slider.value = (float)value;
                return true;
            };
            slider.onValueChanged.AddListener((v) => {
                NotifyUserUpdate(v);
            });
        }
    }
}
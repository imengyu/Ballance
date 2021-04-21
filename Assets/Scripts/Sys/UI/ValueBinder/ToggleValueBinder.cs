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
*
* 更改历史：
* 2021-4-20 创建
*
*/

namespace Ballance2.Sys.UI.ValueBinder
{ 
    [AddComponentMenu("Ballance/UI/ValueBinder/ToggleEx")]
    [RequireComponent(typeof(Toggle))]
    public class ToggleExValueBinder : GameUIControlValueBinder
    {
        private ToggleEx toggle = null;

        protected override void BinderBegin() {
            toggle = GetComponent<ToggleEx>();
            BinderSupplierCallback = (value) => {
                toggle.isOn = (bool)value;
                return true;
            };
            toggle.onValueChanged.AddListener((on) => NotifyUserUpdate(on));
        }
    }
}
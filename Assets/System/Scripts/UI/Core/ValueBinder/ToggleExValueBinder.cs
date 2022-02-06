using UnityEngine;
using Ballance2.UI.Core.Controls;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* ToggleValueBinder.cs
* 
* 用途：
* Toggle控件数据绑定器
*
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core.ValueBinder
{ 
    [AddComponentMenu("Ballance/UI/ValueBinder/ToggleExValueBinder")]
    [RequireComponent(typeof(ToggleEx))]
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
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
*
* 更改历史：
* 2021-4-20 创建
*
*/

namespace Ballance2.Sys.UI.ValueBinder
{
    [AddComponentMenu("Ballance/UI/ValueBinder/Dropdown")]
    [RequireComponent(typeof(Dropdown))]
    public class DropdownValueBinder : GameUIControlValueBinder
    {
        private Dropdown dropdown = null;

        protected override void BinderBegin() {
            dropdown = GetComponent<Dropdown>();
            BinderSupplierCallback = (value) => {
                dropdown.value = (int)value;
                return true;
            };
            dropdown.onValueChanged.AddListener((v) => {
                NotifyUserUpdate(v);
            });
        }
    }
}
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* ProgressValueBinder.cs
* 
* 用途：
* Progress控件数据绑定器
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
    [AddComponentMenu("Ballance/UI/ValueBinder/Progress")]
    [RequireComponent(typeof(Progress))]
    public class ProgressValueBinder : GameUIControlValueBinder
    {
        private Progress progress = null;

        protected override void BinderBegin() {
            progress = GetComponent<Progress>();
            BinderSupplierCallback = (value) => {
                progress.value = (float)value;
                return true;
            };
        }
    }
}
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

namespace Ballance2.Sys.UI.ValueBinder
{
    [AddComponentMenu("Ballance/UI/ValueBinder/TextValueBinder")]
    [RequireComponent(typeof(Text))]
    public class TextValueBinder : GameUIControlValueBinder
    {
        private Text text = null;

        protected override void BinderBegin() {
            text = GetComponent<Text>();
            BinderSupplierCallback = (value) => {
                text.text = (string)value;
                return true;
            };
        }
    }
}

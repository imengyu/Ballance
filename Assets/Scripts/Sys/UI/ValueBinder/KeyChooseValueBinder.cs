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
* 
* 
*
*/

namespace Ballance2.Sys.UI.ValueBinder
{ 
    [AddComponentMenu("Ballance/UI/ValueBinder/KeyChooseValueBinder")]
    [RequireComponent(typeof(KeyChoose))]
    public class KeyChooseValueBinder : GameUIControlValueBinder
    {
        private KeyChoose keyChoose = null;

        protected override void BinderBegin() {
            keyChoose = gameObject.GetComponent<KeyChoose>();
            BinderSupplierCallback = (value) => {
                if(value.GetType() == typeof(int))
                    keyChoose.value = (KeyCode)(int)value;
                else if(value.GetType() == typeof(float)) 
                    keyChoose.value = (KeyCode)(int)value;
                else if(value.GetType() == typeof(double)) 
                    keyChoose.value = (KeyCode)(int)(double)value;
                else
                    UnityEngine.Debug.Log("Unknow type:  " + value.GetType().Name + " " + value);
                return true;
            };
            keyChoose.onValueChanged.AddListener((v) => {
                NotifyUserUpdate(v);
            });
        }
    }
}
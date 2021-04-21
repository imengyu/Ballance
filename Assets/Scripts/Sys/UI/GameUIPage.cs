using System.Collections;
using System.Collections.Generic;
using Ballance.LuaHelpers;
using SLua;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameUIPage.cs
* 
* 用途：
* UI页实例
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-4-20 创建
*
*/

namespace Ballance2
{
    /// <summary>
    /// UI页实例
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("UI页实例")]
    public class GameUIPage : MonoBehaviour
    {
        public RectTransform ContentHost;
        public VerticalLayoutGroup VerticalLayoutGroup;
        public HorizontalLayoutGroup HorizontalLayoutGroup;
        public string PageName;

        public void Show() {
            gameObject.SetActive(true);
        }
        public void Hide() {
            gameObject.SetActive(false);
        }
        
    }
}

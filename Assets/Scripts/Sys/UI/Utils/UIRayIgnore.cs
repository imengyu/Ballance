using UnityEngine;
using UnityEngine.EventSystems;

/*

* Copyright(c) 2021  mengyu
*
* 模块名：     
* SkyBoxUtils.cs
* 
* 用途：
* UI 穿透脚本。添加此脚本至UI从而使控件穿透事件。
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-1 创建
*
*/

namespace Ballance2.Sys.UI.Utils
{
    /// <summary>
    /// UI 穿透脚本。添加此脚本至UI从而使控件穿透事件。
    /// </summary>
    [SLua.CustomLuaClass]
    [AddComponentMenu("Ballance/UI/UIRayIgnore")]
    [Ballance.LuaHelpers.LuaApiDescription("UII 穿透脚本。添加此脚本至UI从而使控件穿透事件")]
    public class UIRayIgnore : UIBehaviour, ICanvasRaycastFilter
    {
        private bool isEnabled = true;

        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            return !isEnabled;
        }

        protected override void OnDisable()
        {
            isEnabled = false;
        }
        protected override void OnEnable()
        {
            isEnabled = true;
        }
    }
}

using Ballance2.LuaHelpers;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* UIContentSizeUtils.cs
* 
* 用途：
* UI 组件内容大小计算工具
*
* 作者：
* mengyu
*/

namespace Ballance2.Sys.UI.Utils
{
    /// <summary>
    /// UI 组件内容大小计算工具
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("UI 组件内容大小计算工具")]
    public static class UIContentSizeUtils
    {
        /// <summary>
        /// 计算一个ContentSizeFitter的合理大小
        /// </summary>
        /// <param name="rect">RectTransform</param>
        /// <param name="contentSizeFitter">ContentSizeFitter</param>
        /// <returns>返回计算后的大小</returns>
        [LuaApiDescription("计算一个ContentSizeFitter的合理大小", "返回计算后的大小")]
        [LuaApiParamDescription("rect", "RectTransform")]
        [LuaApiParamDescription("contentSizeFitter", "ContentSizeFitter")]
        public static Vector2 GetContentSizeFitterPreferredSize(this RectTransform rect, ContentSizeFitter contentSizeFitter)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            return new Vector2(HandleSelfFittingAlongAxis(0, rect, contentSizeFitter), HandleSelfFittingAlongAxis(1, rect, contentSizeFitter));
        }

        private static float HandleSelfFittingAlongAxis(int axis, RectTransform rect, ContentSizeFitter contentSizeFitter)
        {
            ContentSizeFitter.FitMode fitting = (axis == 0 ? contentSizeFitter.horizontalFit : contentSizeFitter.verticalFit);
            if (fitting == ContentSizeFitter.FitMode.MinSize)
            {
                return LayoutUtility.GetMinSize(rect, axis);
            }
            else
            {
                return LayoutUtility.GetPreferredSize(rect, axis);
            }
        }
    }
}

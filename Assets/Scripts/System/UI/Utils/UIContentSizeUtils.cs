using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.System.UI.Utils
{
    [SLua.CustomLuaClass]
    public static class UIContentSizeUtils
    {
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

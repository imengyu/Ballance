using Ballance2.LuaHelpers;
using Ballance2.Sys.UI.Utils;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* Progress.cs
* 
* 用途：
* 一个进度条组件组件
* 
*
* 作者：
* mengyu
*
* 
* 
*
*/

namespace Ballance2.Sys.UI
{
    /// <summary>
    /// 进度条组件
    /// </summary>
    [ExecuteInEditMode]
    [SLua.CustomLuaClass]
    [LuaApiDescription("进度条组件")]
    [AddComponentMenu("Ballance/UI/Controls/Progress")]
    public class Progress : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private float val = 0;
        public bool lightByVal = false;

        public RectTransform fillArea;
        public RectTransform fillRect;
        public Image fillImage;

        public void UpdateVal()
        {
            UIAnchorPosUtils.SetUIRightTop(fillRect, ((1.0f - val) * fillArea.rect.size.x), 0);
            if(lightByVal)
                fillImage.color = new Color(fillImage.color.r, fillImage.color.g, fillImage.color.b, val);
        }

        /// <summary>
        /// 进度条数值（0-1）
        /// </summary>
        /// <value></value>
        [LuaApiDescription("进度条数值（0-1）")]
        public float value
        {
            get { return val; }
            set
            {
                val = value;
                UpdateVal();
            }
        }
    }
}

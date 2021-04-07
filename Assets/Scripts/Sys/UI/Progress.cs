﻿using Ballance2.Sys.UI.Utils;
using UnityEngine;

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
* 更改历史：
* 2021-1-13 创建
*
*/

namespace Ballance2.Sys.UI
{
    /// <summary>
    /// 进度条组件
    /// </summary>
    [ExecuteInEditMode]
    [SLua.CustomLuaClass]
    public class Progress : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private float val = 0;

        public RectTransform fillArea;
        public RectTransform fillRect;

        public void UpdateVal()
        {
            UIAnchorPosUtils.SetUIRightTop(fillRect, ((1.0f - val) * fillArea.rect.size.x), 0);
        }

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
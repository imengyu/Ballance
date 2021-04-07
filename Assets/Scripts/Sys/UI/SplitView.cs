using Ballance2.Sys.UI.Utils;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* Toggle.cs
* 
* 用途：
* 一个分割两个视图的控件，用户可以手动拖动调整两个视图的大小
* 
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-21 创建
*
*/

namespace Ballance2.Sys.UI
{
    /// <summary>
    /// 分割两个视图控件，用户可以手动拖动调整两个视图的大小
    /// </summary>
    [ExecuteInEditMode]
    [SLua.CustomLuaClass]
    public class SplitView : MonoBehaviour
    {
        void Start()
        {
            self = GetComponent<RectTransform>();
            if (splitViewDragger != null)
                BindDragger();
            UpdateVal();
        }

        [SerializeField, HideInInspector]
        private float _value = 0;
        [SerializeField, HideInInspector]
        private float _min = 0;
        [SerializeField, HideInInspector]
        private float _max = 1;
        [SerializeField, HideInInspector]
        private SplitViewDirection _direction = SplitViewDirection.LeftRight;

        public RectTransform self;
        public RectTransform dragger;
        public RectTransform panelOne;
        public RectTransform panelTwo;

        public SplitViewDragger splitViewDragger;
        public void BindDragger()
        {
            splitViewDragger.onValueChanged.AddListener((v) =>
            {
                value = v;
            });
        }


        public void UpdateVal()
        {
            float val = value;
            if (val < _min) val = _min;
            else if (val > _max) val = _max;

            if(splitViewDragger.direction != _direction)
                splitViewDragger.direction = _direction;

            if (_direction == SplitViewDirection.LeftRight)
            {
                float leftWidth = val * self.rect.width;
                float rightWidth = (1 - val) * self.rect.width;

                
                dragger.anchoredPosition = new Vector2(leftWidth - dragger.rect.width / 2, -20);

                UIAnchorPosUtils.SetUIPos(panelOne, 0, 0, rightWidth, 0);
                UIAnchorPosUtils.SetUIPos(panelTwo, leftWidth, 0, 0, 0);
            }
            else if (_direction == SplitViewDirection.TopBottom)
            {
                float topWidth = val * self.rect.height;
                float bottomWidth = (1 - val) * self.rect.height;

                dragger.anchoredPosition = new Vector2(20, -topWidth + dragger.rect.height / 2);

                UIAnchorPosUtils.SetUIPos(panelOne, 0, 0, 0, bottomWidth);
                UIAnchorPosUtils.SetUIPos(panelTwo, 0, topWidth, 0, 0);
            }
        }

        /// <summary>
        /// 最小的分割比例（相当于左边或是上部最小大小）
        /// </summary>
        public float min
        {
            get { return _min; }
            set
            {
                _min = value;
                UpdateVal();
            }
        }
        /// <summary>
        /// 最大的分割比例（相当于右边或是下部最小大小）
        /// </summary>
        public float max
        {
            get { return _max; }
            set
            {
                _max = value;
                UpdateVal();
            }
        }
        /// <summary>
        /// 分割方向
        /// </summary>
        public SplitViewDirection direction
        {
            get { 
                return _direction; 
            }
            set
            {
                _direction = value;
                UpdateVal();
            }
        }
        /// <summary>
        /// 分割比例(0-1,相当于两个视图的大小比例)
        /// </summary>
        public float value
        {
            get { return _value; }
            set
            {
                _value = value;
                UpdateVal();
            }
        }
    }
    /// <summary>
    /// 分割方向
    /// </summary>
    public enum SplitViewDirection
    {
        /// <summary>
        /// 左右
        /// </summary>
        LeftRight,
        /// <summary>
        /// 上下
        /// </summary>
        TopBottom
    }
}

using Ballance2.Sys.UI.UISystem.Layout;
using Ballance2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ballance2.Sys.UI.UISystem
{
    /// <summary>
    /// UI布局系统元素
    /// </summary>
    [SLua.CustomLuaClass]
    public class UIElement : UIBehaviour
    {
        protected RectTransform _RectTransform;
        protected LayoutParams _LayoutParams = new LayoutParams();
        protected bool _IsInLayout = false;
        private ContentSizeFitter _ContentSizeFitter;
        private UIVisible _Visible;

        protected override void Start()
        {
            OnInit();
        }
        protected override void OnDestroy()
        {
            OnUnInit();
        }

        public UIElement()
        {
            Id = CommonUtils.GenAutoIncrementID();
        }

        #region 属性参数

        /// <summary>
        /// 元素ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 元素名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取元素的父元素
        /// </summary>
        public UIElement Parent { get; set; }
        /// <summary>
        /// RectTransform
        /// </summary>
        public RectTransform RectTransform
        {
            get { return _RectTransform; }
        }
        /// <summary>
        /// 获取或设置显示状态
        /// </summary>
        public UIVisible Visible
        {
            get { return _Visible; }
            set { _Visible = value; }
        }
        /// <summary>
        /// 布局属性
        /// </summary>
        public LayoutParams LayoutParams
        {
            get { return _LayoutParams; }
            set { _LayoutParams = value; }
        }
        /// <summary>
        /// 获取是否正在布局
        /// </summary>
        public bool IsInLayout { get { return _IsInLayout; } }
        /// <summary>
        /// 获取当前元素宽度
        /// </summary>
        /// <returns></returns>
        public float Width
        {
            get { return _Right - _Left; }
            set {
                _LayoutParams.width = value;
            }
        }
        /// <summary>
        /// 获取当前元素高度
        /// </summary>
        /// <returns></returns>
        public float Height
        {
            get { return _Bottom - _Top; }
            set
            {
                _LayoutParams.height = value;
            }
        }

        protected float _MinWidth = 0;
        protected float _MinHeight = 0;
        protected float _PaddingAll = 0;
        protected float _PaddingLeft = 0;
        protected float _PaddingRight = 0;
        protected float _PaddingTop = 0;
        protected float _PaddingBottom = 0;

        public float Padding
        {
            get { return _PaddingAll; }
            set
            {
                _PaddingAll = value;
                _PaddingLeft = value;
                _PaddingRight = value;
                _PaddingTop = value;
                _PaddingBottom = value;
            }
        }
        public float PaddingLeft
        {
            get { return _PaddingLeft; }
            set { _PaddingLeft = value; }
        }
        public float PaddingRight
        {
            get { return _PaddingRight; }
            set { _PaddingRight = value; }
        }
        public float PaddingTop
        {
            get { return _PaddingTop; }
            set { _PaddingTop = value; }
        }
        public float PaddingBottom
        {
            get { return _PaddingBottom; }
            set { _PaddingBottom = value; }
        }
        /// <summary>
        /// 最小宽度（如果设置了ContentSizeFitter则此属性无效）
        /// </summary>
        public float MinWidth { get { return _MinWidth; } set { _MinWidth = value; } }
        /// <summary>
        /// 最小高度（如果设置了ContentSizeFitter则此属性无效）
        /// </summary>
        public float MinHeight { get { return _MinHeight; } set { _MinHeight = value; } }

        protected override void OnRectTransformDimensionsChange() 
        {
            if (_ContentSizeFitter != null)
            {
                if (_ContentSizeFitter.horizontalFit == ContentSizeFitter.FitMode.MinSize)
                    _MinWidth = LayoutUtility.GetMinWidth(_RectTransform);
                else if (_ContentSizeFitter.horizontalFit == ContentSizeFitter.FitMode.PreferredSize)
                    _MinWidth = LayoutUtility.GetFlexibleWidth(_RectTransform);

                if (_ContentSizeFitter.verticalFit == ContentSizeFitter.FitMode.MinSize)
                    _MinHeight = LayoutUtility.GetMinHeight(_RectTransform);
                else if (_ContentSizeFitter.verticalFit == ContentSizeFitter.FitMode.PreferredSize)
                    _MinHeight = LayoutUtility.GetFlexibleHeight(_RectTransform);
            }
            if (!IsInLayout)
            {

            }
        }

        public float GetSuggestedMinimumWidth() { return _MinWidth; }
        public float GetSuggestedMinimumHeight() { return _MinHeight; }

        #endregion

        #region Measure



        #endregion

        #region Layout

        protected float _Left = 0;
        protected float _Top = 0;
        protected float _Bottom = 0;
        protected float _Right = 0;

        private bool forceLayout = false;
        private bool layoutRequired = false;

        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="l"></param>
        /// <param name="t"></param>
        /// <param name="r"></param>
        /// <param name="b"></param>
        public void Layout(float l, float t, float r, float b)
        {
            _IsInLayout = true;

            float oldL = _Left;
            float oldT = _Top;
            float oldB = _Bottom;
            float oldR = _Right;

            bool changed = SetFrame(l, t, r, b);
            if (changed || layoutRequired)
            {
                OnLayout(changed, l, t, r, b);
                layoutRequired = false;
            }

            forceLayout = false;
            _IsInLayout = false;
        }
        /// <summary>
        /// 请求当前视图进行布局
        /// </summary>
        public void RequestLayout()
        {
            if (_IsInLayout)
                return; 

            if (Parent != null && !Parent.IsLayoutRequested())
                Parent.RequestLayout();

            forceLayout = true;
        }
        /// <summary>
        /// 指示是否在下一个层次结构布局过程中请求此视图的布局。
        /// </summary>
        /// <returns>如果在下一个布局过程中强制布局，则为true</returns>
        public bool IsLayoutRequested()
        {
            return forceLayout;
        }
        /// <summary>
        /// 强制在下一个布局过程中布局此视图。此方法不对父对象调用RequestLayout() 或 ForceLayout()。
        /// </summary>
        public void ForceLayout()
        {

            forceLayout = true;
        }

        protected bool SetFrame(float left, float top, float right, float bottom)
        {
            bool changed = false;

            if (_Left != left || _Right != right || _Top != top || _Bottom != bottom)
            {
                changed = true;

                float oldWidth = _Right - _Left;
                float oldHeight = _Bottom - _Top;
                float newWidth = right - left;
                float newHeight = bottom - top;

                bool sizeChanged = (newWidth != oldWidth) || (newHeight != oldHeight);

                _Left = left;
                _Top = top;
                _Right = right;
                _Bottom = bottom;
                _RectTransform.anchoredPosition = new Vector2(_Left, _Top);
                _RectTransform.sizeDelta = new Vector2(newWidth, newHeight);

                if (sizeChanged)
                    OnSizeChanged(newWidth, newHeight, oldWidth, oldHeight);
            }
            return changed;
        }

        #endregion

        protected virtual void OnInit()
        {
            _ContentSizeFitter = GetComponent<ContentSizeFitter>();
        }
        protected virtual void OnUnInit()
        {

        }
        protected virtual void OnSizeChanged(float newW, float newH, float oldW, float oldH)
        {

        }
        /// <summary>
        /// 测量事件
        /// </summary>
        /// <param name="widthMeasureSpec">宽测量规格</param>
        /// <param name="heightMeasureSpec">高测量规格</param>
        protected virtual void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
        }
        /// <summary>
        /// 布局事件
        /// </summary>
        /// <param name="changed"></param>
        /// <param name="l"></param>
        /// <param name="t"></param>
        /// <param name="r"></param>
        /// <param name="b"></param>
        protected virtual void OnLayout(bool changed, float left, float top, float right, float bottom)
        {

        }

        public virtual void SetProp(string name, string val, bool intital = false)
        {
            switch (name)
            {
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// UI元素显示状态
    /// </summary>
    [SLua.CustomLuaClass]
    public enum UIVisible
    {
        Visible,
        Hidden,
        Gone,
    }
}

using Ballance2.Sys.Res;
using Ballance2.Sys.Services;
using Ballance2.Sys.UI.Utils;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* Window.cs
* 
* 用途：
* UI 窗口承载
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
    /// 基础 UI 窗口
    /// </summary>
    [SLua.CustomLuaClass]
    [ExecuteInEditMode]
    public class Window : MonoBehaviour
    {
        private int windowId = 0;

        /// <summary>
        /// 获取窗口是否显示
        /// </summary>
        /// <returns></returns>
        public bool GetVisible()
        {
            return gameObject.activeSelf;
        }
        /// <summary>
        /// 设置窗口是否显示
        /// </summary>
        /// <param name="visible">是否显示</param>
        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
            if (visible) Show();
            else Hide();
        }
        /// <summary>
        /// 销毁窗口
        /// </summary>
        public void Destroy()
        {
            Destroy(gameObject);
        }
        /// <summary>
        /// 获取窗口ID
        /// </summary>
        public int GetWindowId() { return windowId; }

        private RectTransform WindowRectTransform;
        private UIDragControl WindowTitleDragger;

        private Sprite MinIcon;
        private Sprite MinRestoreIcon;
        private Sprite WindowMaxIcon;
        private Sprite WindowRestoreIcon;

        public Button WindowButtonMax;
        public Button WindowButtonMin;
        public RectTransform WindowButtonMinRectTransform;
        public Button WindowButtonClose;
        public Image WindowIconImage;
        public Text WindowTitleText;
        public RectTransform WindowClientArea;
        public RectTransform WindowTitle;
        public UISizeDrag SizeDrag;

        private GameUIManager UIManager;
        private Vector2 minSize = new Vector2(150, 35);

        private void Awake()
        {
            WindowRectTransform = GetComponent<RectTransform>();
            WindowTitleDragger = WindowTitle.gameObject.GetComponent<UIDragControl>();
        }
        private void Start()
        {
            MinIcon = GameStaticResourcesPool.FindStaticAssets<Sprite>("MinIcon");
            MinRestoreIcon = GameStaticResourcesPool.FindStaticAssets<Sprite>("MinRestoreIcon");
            WindowMaxIcon = GameStaticResourcesPool.FindStaticAssets<Sprite>("WindowMaxIcon");
            WindowRestoreIcon = GameStaticResourcesPool.FindStaticAssets<Sprite>("WindowRestoreIcon");

            if (GameManager.Instance != null)
                UIManager = (GameUIManager)GameManager.Instance.GetSystemService("GameUIManager");
            if (UIManager != null)
                windowId = UIManager.GenWindowId();

            WindowButtonClose.onClick.AddListener(() =>
            {
                if (CloseAsHide) Hide();
                else Close();
            });
            WindowButtonMax.onClick.AddListener(() =>
            {
                if (WindowState == WindowState.Max)
                    WindowState = WindowState.Normal;
                else
                    WindowState = WindowState.Max;
            });
            WindowButtonMin.onClick.AddListener(() =>
            {
                WindowState = WindowState.Min;
            });

            EventTriggerListener.Get(WindowTitleDragger.gameObject).onClick = (g) => {
                WindowRectTransform.transform.SetAsLastSibling();
            };
        }

        /// <summary>
        /// 获取或设置窗口大小
        /// </summary>
        /// <returns></returns>
        public Vector2 Size
        {
            get { return WindowRectTransform.sizeDelta; }
            set
            {
                if (minSize.x != 0 && value.x > minSize.x)
                    value.x = minSize.x;
                if (minSize.y != 0 && value.y > minSize.y)
                    value.y = minSize.y;
                WindowRectTransform.sizeDelta = value;
            }
        }
        /// <summary>
        /// 获取或设置窗口最小大小
        /// </summary>
        /// <returns></returns>
        public Vector2 MinSize
        {
            get { return minSize; }
            set { minSize = value; }
        }
        /// <summary>
        /// 获取或设置窗口位置
        /// </summary>
        /// <returns></returns>
        public Vector2 Position {
            get { return WindowRectTransform.anchoredPosition; }
            set { WindowRectTransform.anchoredPosition = value; }
        }

        /// <summary>
        /// 设置窗口的自定义区域视图
        /// </summary>
        /// <param name="view">要绑定的子视图</param>
        /// <returns></returns>
        public RectTransform SetView(RectTransform view)
        {
            RectTransform oldView = GetView();
            view.SetParent(WindowClientArea.transform);
            UIAnchorPosUtils.SetUIPos(view, 0, 0, 0, 0);

            if (oldView != null)
            {
                oldView.gameObject.SetActive(false);
                UIManager.SetViewToTemporarily(oldView);
            }
            return oldView;
        }
        /// <summary>
        /// 获取窗口的自定义区域已绑定的视图
        /// </summary>
        /// <returns></returns>
        public RectTransform GetView()
        {
            if (WindowClientArea.transform.childCount > 0)
                return WindowClientArea.transform.GetChild(0).GetComponent<RectTransform>();
            return null;
        }
        /// <summary>
        /// 获取窗口本体的 RectTransform
        /// </summary>
        /// <returns></returns>
        public RectTransform GetRectTransform()
        {
            return WindowRectTransform;
        }

        #region 窗口操作属性

        private bool _CanResize = true;
        private bool _CanMin = true;
        private bool _CanDrag = true;
        private bool _CanMax = true;
        private bool _CanClose = true;
        private string _Title = ""; 
        private Sprite _Icon = null;

        /// <summary>
        /// 窗口的图标
        /// </summary>
        public Sprite Icon
        {
            get { return _Icon; }
            set {
                _Icon = value;
                if (WindowIconImage != null && WindowTitleText != null)
                {
                    if (value == null)
                    {
                        WindowIconImage.gameObject.SetActive(false);
                        UIAnchorPosUtils.SetUILeftBottom(WindowTitleText.rectTransform, 9, 0);
                    }
                    else
                    {
                        WindowIconImage.gameObject.SetActive(true);
                        UIAnchorPosUtils.SetUILeftBottom(WindowTitleText.rectTransform, 23, 0);
                    }
                }
            }
        }
        /// <summary>
        /// 窗口是否可以拖动改变大小
        /// </summary>
        public bool CanResize
        {
            get { return _CanResize; }
            set
            {
                _CanResize = value;
                if (SizeDrag != null)
                    SizeDrag.gameObject.SetActive(value);
            }
        }
        /// <summary>
        /// 窗口是否可以拖动
        /// </summary>
        public bool CanDrag
        {
            get { return _CanDrag; }
            set
            {
                _CanDrag = value;
                if(WindowTitleDragger != null)
                    WindowTitleDragger.gameObject.SetActive(_CanDrag);
            }
        }
        /// <summary>
        /// 窗口是否可关闭
        /// </summary>
        public bool CanClose
        {
            get { return _CanClose; }
            set
            {
                _CanClose = value;
                if (WindowButtonClose != null)
                    WindowButtonClose.gameObject.SetActive(_CanClose);
            }
        }
        /// <summary>
        /// 窗口是否可以拖动改变大小
        /// </summary>
        public bool CanMin
        {
            get { return _CanMin; }
            set
            {
                _CanMin = value;
                if (WindowButtonMin != null)
                    WindowButtonMin.gameObject.SetActive(value);
            }
        }
        /// <summary>
        /// 窗口是否可以拖动改变大小
        /// </summary>
        public bool CanMax
        {
            get { return _CanMax; }
            set
            {
                _CanMax = value;
                if (WindowButtonMax != null) 
                    WindowButtonMax.gameObject.SetActive(value);
                if (WindowButtonMinRectTransform != null)
                {
                    if (_CanMax)
                    {
                        WindowButtonMinRectTransform.anchoredPosition =
                            new Vector2(-64, WindowButtonMinRectTransform.anchoredPosition.y);
                    }
                    else
                    {
                        WindowButtonMinRectTransform.anchoredPosition =
                            new Vector2(-38, WindowButtonMinRectTransform.anchoredPosition.y);
                    }
                }
            }
        }
        /// <summary>
        /// 点击窗口关闭按钮是否替换为隐藏窗口
        /// </summary>
        public bool CloseAsHide { get; set; }
        /// <summary>
        /// 窗口标题
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                if (WindowTitleText != null)
                    WindowTitleText.text = value;
            }
        }

        /// <summary>
        /// 关闭并销毁窗口
        /// </summary>
        public void Close()
        {
            onClose?.Invoke(windowId);
        }
        /// <summary>
        /// 显示窗口
        /// </summary>
        public void Show()
        {
            if (_WindowState == WindowState.Hidden)
                WindowState = oldWindowState;
            else
                WindowState = WindowState.Normal;
            onShow?.Invoke(windowId);
        }
        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public void Hide()
        {
            oldWindowState = _WindowState;
            WindowState = WindowState.Hidden;
            onHide?.Invoke(windowId);
        }
        /// <summary>
        /// 窗口剧中
        /// </summary>
        public void MoveToCenter()
        {
            Position = new Vector2(-WindowRectTransform.sizeDelta.x / 2, 
                WindowRectTransform.sizeDelta.y / 2);
        }

        public delegate void WindowEventDelegate(int windowId);

        public WindowEventDelegate onClose;
        public WindowEventDelegate onShow;
        public WindowEventDelegate onHide;

        private Vector2 oldWindowPos = Vector2.zero;
        private Vector2 oldWindowSize = Vector2.zero;
        private WindowState oldWindowState = WindowState.Hidden;
        private WindowState _WindowState = WindowState.Hidden;
        private void UpdateWindowState()
        {
            switch (_WindowState)
            {
                case WindowState.Hidden:
                    SetVisible(false);
                    break;
                case WindowState.Normal:
                    SetVisible(true);
                    WindowClientArea.gameObject.SetActive(true);
                    WindowButtonMin.image.sprite = MinIcon;
                    WindowButtonMax.image.sprite = WindowMaxIcon;
                    if (_CanResize) SizeDrag.gameObject.SetActive(true);
                    if (oldWindowSize.x > 0 && oldWindowSize.y > 0)
                    {
                        Size = oldWindowSize;
                        oldWindowSize = Vector2.zero;
                    }
                    if (oldWindowPos.x > 0 && oldWindowPos.y > 0)
                    {
                        Position = oldWindowPos;
                        oldWindowPos = Vector2.zero;
                    }
                    break;
                case WindowState.Max:
                    SetVisible(true);
                    WindowClientArea.gameObject.SetActive(true);
                    WindowButtonMin.image.sprite = MinIcon;
                    WindowButtonMax.image.sprite = WindowRestoreIcon;
                    if (_CanResize) SizeDrag.gameObject.SetActive(true);
                    if (oldWindowState == WindowState.Normal)
                    {
                        oldWindowSize = Size;
                        oldWindowPos = Position;
                    }
                    Position = Vector2.zero;
                    Size = new Vector2(Screen.width, Screen.height);
                    break;
                case WindowState.Min:
                    SetVisible(true); 
                    WindowClientArea.gameObject.SetActive(false);
                    WindowButtonMin.image.sprite = MinRestoreIcon;
                    WindowButtonMax.image.sprite = WindowMaxIcon;
                    if (_CanResize) SizeDrag.gameObject.SetActive(false);
                    if (oldWindowState == WindowState.Normal)
                        oldWindowSize = Size;
                    Size = new Vector2(120, 30);
                    break;
            }
        }

        /// <summary>
        /// 获取窗口当前状态
        /// </summary>
        public WindowState WindowState { 
            get { return _WindowState; } 
            set
            {
                if (_WindowState != value)
                {
                    if (value == WindowState.Min && !_CanMin)
                        return;
                    if (value == WindowState.Max && !_CanMax)
                        return;
                    oldWindowState = _WindowState;
                    _WindowState = value;
                    UpdateWindowState();
                }
            }
        }
        /// <summary>
        /// 获取窗口类型
        /// </summary>
        /// <returns></returns>
        public WindowType WindowType { get; set; } = WindowType.Normal;

        #endregion
    }

    [SLua.CustomLuaClass]
    /// <summary>
    /// 窗口状态
    /// </summary>
    public enum WindowState
    {
        /// <summary>
        /// 窗口未显示
        /// </summary>
        Hidden,
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 已经最大化
        /// </summary>
        Max,
        /// <summary>
        /// 已经最小化
        /// </summary>
        Min
    }
    [SLua.CustomLuaClass]
    /// <summary>
    /// 窗口类型
    /// </summary>
    public enum WindowType
    {
        /// <summary>
        /// 正常窗口
        /// </summary>
        Normal,
        /// <summary>
        /// 全局弹出窗口
        /// </summary>
        GlobalAlert,
        /// <summary>
        /// 页
        /// </summary>
        Page
    }
}

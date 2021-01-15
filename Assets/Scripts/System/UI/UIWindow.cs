using Ballance2.System.UI.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.System.UI
{
    /// <summary>
    /// 基础 UI 窗口
    /// </summary>
    [SLua.CustomLuaClass]
    public class UIWindow : MonoBehaviour
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
            if (visible) OnShow();
            else OnHide();
        }
        /// <summary>
        /// 销毁窗口
        /// </summary>
        public void Destroy()
        {
            Destroy(gameObject);
            OnClose();
        }
        /// <summary>
        /// 获取窗口ID
        /// </summary>
        public int GetWindowId() { return windowId;  }

        private RectTransform UIWindowRectTransform;
        private UIDragControl UIWindowTitleDragger;

        public Button UIWindowButtonClose;
        public Text UIWindowTitleText;
        public RectTransform UIWindowClientArea;
        public RectTransform UIWindowTitle;
        public UISizeDrag UISizeDrag;

        //private UIManager UIManager;
        private Vector2 minSize = new Vector2(150, 35);

        void Awake()
        {
            //UIManager = (UIManager)GameManager.GetManager(UIManager.TAG);
           // windowId = UIManager.GenWindowId();
            UIWindowRectTransform = GetComponent<RectTransform>();
            UIWindowTitleDragger = UIWindowTitle.gameObject.GetComponent<UIDragControl>();
            EventTriggerListener.Get(UIWindowButtonClose.gameObject).onClick = (g) => {
                if (CloseAsHide) Hide();
                else Close();
            };
            EventTriggerListener.Get(UIWindowTitleDragger.gameObject).onClick = (g) => {
                UIWindowRectTransform.transform.SetAsLastSibling();
            };
        }

        /// <summary>
        /// 设置窗口大小
        /// </summary>
        /// <param name="w">宽</param>
        /// <param name="h">高</param>
        public void SetSize(float w, float h)
        {
            UIWindowRectTransform.sizeDelta = new Vector2(w, h);
        }
        /// <summary>
        /// 设置窗口最小大小
        /// </summary>
        /// <param name="w">宽</param>
        /// <param name="h">高</param>
        public void SetMinSize(float w, float h)
        {
            minSize = new Vector2(w, h);
        }
        /// <summary>
        /// 设置窗口位置
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public void SetPos(float x, float y)
        {
            UIWindowRectTransform.anchoredPosition = new Vector2(x, y);
        }
        /// <summary>
        /// 获取窗口大小
        /// </summary>
        /// <returns></returns>
        public Vector2 GetSize()
        {
            return UIWindowRectTransform.sizeDelta;
        }
        /// <summary>
        /// 获取窗口最小大小
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMinSize()
        {
            return minSize;
        }
        /// <summary>
        /// 获取窗口位置
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPos()
        {
            return UIWindowRectTransform.anchoredPosition;
        }

        /// <summary>
        /// 设置窗口的自定义区域视图
        /// </summary>
        /// <param name="view">要绑定的子视图</param>
        /// <returns></returns>
        public RectTransform SetView(RectTransform view)
        {
            RectTransform oldView = GetView();
            view.SetParent(UIWindowClientArea.transform);
            UIAnchorPosUtils.SetUIPos(view, 0, 0, 0, 0);

            if (oldView != null)
            {
                oldView.gameObject.SetActive(false);
                //UIManager.SetViewToTemporarily(oldView);
            }
            return oldView;
        }
        /// <summary>
        /// 获取窗口的自定义区域已绑定的视图
        /// </summary>
        /// <returns></returns>
        public RectTransform GetView()
        {
            if (UIWindowClientArea.transform.childCount > 0)
                return UIWindowClientArea.transform.GetChild(0).GetComponent<RectTransform>();
            return null;
        }
        /// <summary>
        /// 获取窗口本体的 RectTransform
        /// </summary>
        /// <returns></returns>
        public RectTransform GetRectTransform()
        {
            return UIWindowRectTransform;
        }

        /// <summary>
        /// 窗口是否可以拖动改变大小
        /// </summary>
        public bool CanResize
        {
            get { return UISizeDrag.gameObject.activeSelf; }
            set { UISizeDrag.gameObject.SetActive(value); }
        }
        /// <summary>
        /// 窗口是否可以拖动
        /// </summary>
        public bool CanDrag
        {
            get { return UIWindowTitleDragger.enabled; }
            set { UIWindowTitleDragger.enabled = value; }
        }
        /// <summary>
        /// 窗口是否可关闭
        /// </summary>
        public bool CanClose
        {
            get { return UIWindowButtonClose.gameObject.activeSelf; }
            set { UIWindowButtonClose.gameObject.SetActive(value);  }
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
            get { return UIWindowTitleText.text; }
            set { UIWindowTitleText.text = value; }
        }

        /// <summary>
        /// 关闭并销毁窗口
        /// </summary>
        public void Close()
        {
            //UIManager.CloseWindow(this);
        }
        /// <summary>
        /// 显示窗口
        /// </summary>
        public void Show()
        {
            //UIManager.ShowWindow(this);
        }
        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public void Hide()
        {
            //UIManager.HideWindow(this);
        }
        /// <summary>
        /// 窗口剧中
        /// </summary>
        public void MoveToCenter()
        {
            SetPos(-UIWindowRectTransform.sizeDelta.x / 2, UIWindowRectTransform.sizeDelta.y / 2);
        }


        private WindowType windowType = WindowType.Normal;

        /// <summary>
        /// 获取窗口类型
        /// </summary>
        /// <returns></returns>
        public WindowType GetWindowType() { return windowType; }
        /// <summary>
        /// 设置窗口类型
        /// </summary>
        /// <param name="type"></param>
        public void SetWindowType(WindowType type) { windowType = type; }

        #region 窗口事件

        public delegate void WindowEventDelegate(int windowId);

        public WindowEventDelegate onClose;
        public WindowEventDelegate onShow;
        public WindowEventDelegate onHide;

        public virtual void OnClose()
        {
            if(onClose != null) onClose.Invoke(windowId);
        }
        public virtual void OnHide()
        {
            if (onHide != null) onHide.Invoke(windowId);
        }
        public virtual void OnShow()
        {
            UIWindowRectTransform.SetAsLastSibling();
            if (onShow != null) onShow.Invoke(windowId);
        }

        #endregion
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

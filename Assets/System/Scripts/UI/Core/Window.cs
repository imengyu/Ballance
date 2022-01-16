using Ballance2.Services;
using Ballance2.Services.InputManager;
using Ballance2.UI.Utils;
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
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core
{
  /// <summary>
  /// 基础 UI 窗口
  /// </summary>
  [ExecuteInEditMode]
  [AddComponentMenu("Ballance/UI/Controls/Window")]
  [LuaApiDescription("基础 UI 窗口")]
  [SLua.CustomLuaClass]
  public class Window : MonoBehaviour
  {
    private void OnDestroy()
    {
      if (UIManager != null)
        UIManager.InternalRemoveWindow(this);
    }

    internal int windowId = 0;
    private bool firstShow = true;

    /// <summary>
    /// 获取窗口是否显示
    /// </summary>
    /// <returns></returns>
    [LuaApiDescription("获取窗口是否显示")]
    public bool GetVisible()
    {
      return gameObject.activeSelf;
    }
    /// <summary>
    /// 设置窗口是否显示
    /// </summary>
    /// <param name="visible">是否显示</param>
    [LuaApiDescription("设置窗口是否显示")]
    [LuaApiParamDescription("visible", "是否显示")]
    public void SetVisible(bool visible)
    {

      if (gameObject.activeSelf != visible)
      {
        gameObject.SetActive(visible);
        if (visible) onShow?.Invoke(windowId);
        else onHide?.Invoke(windowId);
      }
      if (visible && UIManager != null)
      {
        if (firstShow)
        {
          firstShow = false;
          UIManager.WindowFirshAdd(this);
        }
        UIManager.ActiveWindow(this);
      }
    }
    /// <summary>
    /// 销毁窗口
    /// </summary>
    [LuaApiDescription("销毁窗口")]
    public void Destroy()
    {
      Destroy(gameObject);
    }
    /// <summary>
    /// 获取窗口ID
    /// </summary>
    [LuaApiDescription("获取窗口ID")]
    public int GetWindowId() { return windowId; }

    public RectTransform WindowRectTransform;
    public UIDragControl WindowTitleDragger;

    public Color TitleDefaultColor;
    public Color TitleActiveColor;
    public Sprite TitleDefaultSprite;
    public Sprite TitleMinSprite;

    public Button WindowButtonClose;
    public Button WindowButtonMin;
    public Image WindowIconImage;
    public Image WindowTitleImage;
    public Image MinButtonImage;
    public Text WindowTitleText;
    public RectTransform WindowClientArea;
    public RectTransform WindowTitle;
    public UISizeDrag SizeDrag;

    private GameUIManager _UIManager;
    private GameUIManager UIManager
    {
      get
      {
        if(GameManager.Instance != null) {
          if (_UIManager == null)
            _UIManager = (GameUIManager)GameManager.Instance.GetSystemService("GameUIManager");
        }
        return _UIManager;
      }
    }
    private Vector2 minSize = new Vector2(150, 32);

    private void Awake()
    {
      WindowRectTransform = GetComponent<RectTransform>();
      WindowTitleDragger = WindowTitle.gameObject.GetComponent<UIDragControl>();
    }
    private void Start()
    {
      WindowButtonClose.onClick.AddListener(() =>
      {
        if (CloseAsHide) Hide();
        else Close();
      });
      WindowButtonMin.onClick.AddListener(() =>
      {
        if (WindowState == WindowState.Normal) WindowState = WindowState.Min;
        else WindowState = WindowState.Normal;
      });
      EventTriggerListener.Get(gameObject).onDown = (g) =>
      {
        if (UIManager.GetCurrentActiveWindow() != this)
          UIManager.ActiveWindow(this);
      };
    }

    /// <summary>
    /// 获取或设置窗口大小
    /// </summary>
    /// <returns></returns>
    [LuaApiDescription("获取或设置窗口大小")]
    public Vector2 Size
    {
      get { return WindowRectTransform.sizeDelta; }
      set
      {
        if (minSize.x != 0 && value.x < minSize.x)
          value.x = minSize.x;
        if (minSize.y != 0 && value.y < minSize.y)
          value.y = minSize.y;
        WindowRectTransform.sizeDelta = value;
      }
    }
    /// <summary>
    /// 获取或设置窗口最小大小
    /// </summary>
    /// <returns></returns>
    [LuaApiDescription("获取或设置窗口最小大小")]
    public Vector2 MinSize
    {
      get { return minSize; }
      set { minSize = value; }
    }
    /// <summary>
    /// 获取或设置窗口位置
    /// </summary>
    /// <returns></returns>
    [LuaApiDescription("获取或设置窗口位置")]
    public Vector2 Position
    {
      get { return WindowRectTransform.anchoredPosition; }
      set { WindowRectTransform.anchoredPosition = value; }
    }

    /// <summary>
    /// 设置窗口的自定义区域视图
    /// </summary>
    /// <param name="view">要绑定的子视图</param>
    /// <returns></returns>
    [LuaApiDescription("设置窗口的自定义区域视图")]
    [LuaApiParamDescription("view", "要绑定的子视图")]
    public RectTransform SetView(RectTransform view)
    {
      RectTransform oldView = GetView();
      view.SetParent(WindowClientArea.transform);
      view.localScale = Vector3.one;
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
    [LuaApiDescription("获取窗口的自定义区域已绑定的视图")]
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
    [LuaApiDescription("获取窗口本体的 RectTransform")]
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
    [LuaApiDescription("窗口的图标")]
    public Sprite Icon
    {
      get { return _Icon; }
      set
      {
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
    [LuaApiDescription("窗口是否可以拖动改变大小")]
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
    [LuaApiDescription("窗口是否可以拖动")]
    public bool CanDrag
    {
      get { return _CanDrag; }
      set
      {
        _CanDrag = value;
        if (WindowTitleDragger != null)
          WindowTitleDragger.gameObject.SetActive(_CanDrag);
      }
    }
    /// <summary>
    /// 窗口是否可关闭
    /// </summary>
    [LuaApiDescription("窗口是否可关闭")]
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
    /// 窗口是否可以最小化
    /// </summary>
    [LuaApiDescription("窗口是否可以最小化")]
    public bool CanMin
    {
      get { return _CanMin; }
      set
      {
        _CanMin = value;
        WindowButtonMin.gameObject.SetActive(_CanMin);
        UIAnchorPosUtils.SetUILeft(WindowTitleText.rectTransform, _CanMin ? 46 : 25);
      }
    }
    /// <summary>
    /// 窗口是否可以最大化
    /// </summary>
    [LuaApiDescription("窗口是否可以最大化")]
    public bool CanMax
    {
      get { return _CanMax; }
      set { _CanMax = value; }
    }
    /// <summary>
    /// 点击窗口关闭按钮是否替换为隐藏窗口
    /// </summary>
    [LuaApiDescription("点击窗口关闭按钮是否替换为隐藏窗口")]
    public bool CloseAsHide { get; set; }
    /// <summary>
    /// 窗口标题
    /// </summary>
    [LuaApiDescription("窗口标题")]
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
    [LuaApiDescription("关闭并销毁窗口")]
    public void Close()
    {
      onClose?.Invoke(windowId);
    }
    /// <summary>
    /// 显示窗口
    /// </summary>
    [LuaApiDescription("显示窗口")]
    public void Show()
    {
      if (_WindowState == WindowState.Hidden && oldWindowState != WindowState.Hidden)
        WindowState = oldWindowState;
      else
        WindowState = WindowState.Normal;

      if (UIManager != null)
        UIManager.ActiveWindow(this);
    }
    /// <summary>
    /// 隐藏窗口
    /// </summary>
    [LuaApiDescription("隐藏窗口")]
    public void Hide()
    {
      oldWindowState = _WindowState;
      WindowState = WindowState.Hidden;

      if (gameObject.activeSelf)
        SetVisible(false);
    }
    /// <summary>
    /// 窗口居中
    /// </summary>
    [LuaApiDescription("窗口居中")]
    public void MoveToCenter()
    {
      Position = new Vector2(
          Screen.width / 2 - WindowRectTransform.sizeDelta.x / 2,
          -(Screen.height / 2 - WindowRectTransform.sizeDelta.y / 2));
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
          MinButtonImage.rectTransform.eulerAngles = new Vector3(0, 0, -90);
          WindowTitleImage.sprite = TitleDefaultSprite;
          WindowClientArea.gameObject.SetActive(true);
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
          WindowTitleImage.sprite = TitleDefaultSprite;
          if (_CanResize) SizeDrag.gameObject.SetActive(true);
          if (oldWindowState == WindowState.Normal)
          {
            oldWindowSize = Size;
            oldWindowPos = Position;
          }
          Position = Vector2.zero;
          WindowRectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
          break;
        case WindowState.Min:
          SetVisible(true);
          MinButtonImage.rectTransform.eulerAngles = new Vector3(0, 0, 0);
          WindowTitleImage.sprite = TitleMinSprite;
          WindowClientArea.gameObject.SetActive(false);
          if (_CanResize) SizeDrag.gameObject.SetActive(false);
          if (oldWindowState == WindowState.Normal)
          {
            oldWindowSize = Size;
          }
          WindowRectTransform.sizeDelta = new Vector2(250, 20);
          break;
      }
    }

    /// <summary>
    /// 获取窗口当前状态
    /// </summary>
    [LuaApiDescription("获取窗口当前状态")]
    public WindowState WindowState
    {
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
    [LuaApiDescription("获取窗口类型")]
    public WindowType WindowType { get; set; } = WindowType.Normal;

    #endregion
  }


  /// <summary>
  /// 窗口状态
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("窗口状态")]
  public enum WindowState
  {
    /// <summary>
    /// 窗口未显示
    /// </summary>
    [LuaApiDescription("窗口未显示")]
    Hidden,
    /// <summary>
    /// 正常
    /// </summary>
    [LuaApiDescription("正常")]
    Normal,
    /// <summary>
    /// 已经最大化
    /// </summary>
    [LuaApiDescription("已经最大化")]
    Max,
    /// <summary>
    /// 已经最小化
    /// </summary>
    [LuaApiDescription("已经最小化")]
    Min
  }

  /// <summary>
  /// 窗口类型
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("窗口类型")]
  public enum WindowType
  {
    /// <summary>
    /// 正常窗口
    /// </summary>
    [LuaApiDescription("正常窗口")]
    Normal,
    /// <summary>
    /// 置顶窗口
    /// </summary>
    [LuaApiDescription("置顶窗口")]
    TopWindow,
    /// <summary>
    /// 全局弹出窗口
    /// </summary>
    [LuaApiDescription("全局弹出窗口")]
    GlobalAlert,
    /// <summary>
    /// 页
    /// </summary>
    [LuaApiDescription("页")]
    Page
  }
}

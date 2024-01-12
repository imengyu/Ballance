using Ballance2.Base;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services.Debug;
using Ballance2.Services.InputManager;
using Ballance2.UI.Core;
using Ballance2.UI.Utils;
using Ballance2.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using static Ballance2.Services.GameManager;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameUIManager.cs
* 
* 用途：
* UI 管理器，用于管理UI通用功能
*
* 作者：
* mengyu
*/

namespace Ballance2.Services
{
  /// <summary>
  /// UI 管理器，用于管理UI通用功能
  /// </summary>
  /// <Remarks>
  /// UI 管理器提供了者几种种UI通用功能：
  /// * Window 为游戏提供了一个可以拖拽，调整大小的窗口，用于游戏内部某些UI的使用。要创建窗口，可以调用 `GameUIManager.CreateWindow` 函数。
  /// * Page 页与窗口不太一样，窗口可以同时打开多个，页只能同时显示一个，相当于独占的全屏窗口。要创建页，可以调用 `GameUIManager.RegisterPage` 函数。
  /// * MaskBlack 全局的黑色转场渐变遮罩。
  /// * MaskWhite 全局的白色转场渐变遮罩。
  /// * GlobalAlert 全局的弹出独占对话框。
  /// </Remarks>
  public class GameUIManager : GameService<GameUIManager>
  {
    #region 基础

    private static readonly string TAG = "GameUIManager";

    public GameUIManager() : base(TAG) { }

    private GameObject GameUICommonHost;
 
    public override void Destroy()
    {
      Object.Destroy(uiManagerGameObject);

      if(UIRoot != null) {
        GameObject go = null;
        int count = 0;
        for (int i = 0, c = UIRoot.transform.childCount; i < c; i++) {
          go = UIRoot.transform.GetChild(i).gameObject;
          if(go.tag != "Speical" && go.tag != "GameController") {
            Object.Destroy(go);
            count ++;
          }
        }
        Log.D(TAG, "Destroy {0} ui objects", count);
      }
    } 
    
    public override bool Initialize()
    {
      UIRoot = GameManager.Instance.GameCanvas;
      UIFadeManager = UIRoot.gameObject.AddComponent<UIFadeManager>();

      GameManager.GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_UI_MANAGER_INIT_FINISHED);
      //等待基础加载完成
      GameManager.GameMediator.RegisterEventHandler(GamePackage.GetSystemPackage(),
          GameEventNames.EVENT_BASE_INIT_FINISHED, TAG, (evtName, param) =>
          {
            var GameGlobalMask = UIRoot.transform.Find("GameGlobalMask");
            var GameGlobalDialog = UIRoot.transform.Find("GameGlobalDialog");
            GlobalFadeMaskWhite = GameGlobalMask.Find("GlobalFadeMaskWhite").gameObject.GetComponent<Image>();
            GlobalFadeMaskBlack = GameGlobalMask.Find("GlobalFadeMaskBlack").gameObject.GetComponent<Image>();

            //隐藏遮住初始化的遮罩  
            var GlobalBlackMask = GameGlobalMask.Find("GlobalBlackMask");
            if (GlobalBlackMask != null) GlobalBlackMask.gameObject.SetActive(false);

            //黑色遮罩
            GlobalFadeMaskBlack.gameObject.SetActive(true);
            MaskBlackSet(true);

            //Init all
            InitAllObects();
            InitCommands();
            InitF9CaptureScreenshot();
            InitF8SwitchViews();

            //更新主管理器中的Canvas变量
            GameManager.Instance.GameCanvas = ViewsRectTransform;
            GameGlobalMask.SetAsLastSibling();
            GameGlobalDialog.SetAsLastSibling();
            TopViewsRectTransform.SetAsLastSibling();
            TopWindowsRectTransform.SetAsLastSibling();
            GlobalWindowRectTransform.SetAsLastSibling();
            UIToast.SetAsLastSibling();
            if(Entry.GameEntry.Instance != null && Entry.GameEntry.Instance.GameGlobalIngameLoading != null)
              Entry.GameEntry.Instance.GameGlobalIngameLoading.transform.SetAsLastSibling();
            InitF10FpsSwitch();
            InitGlobalPage();

            //发送就绪事件
            GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_UI_MANAGER_INIT_FINISHED);
            return false;
          });
      //退出时的黑
      GameManager.GameMediator.RegisterEventHandler(GamePackage.GetSystemPackage(),
          GameEventNames.EVENT_BEFORE_GAME_QUIT, TAG, (evtName, param) =>
      {
        MaskBlackFadeIn(0.25f);
        return false;
      });
      return true;
    }

    #region 特殊按键的控制

    //F10切换FPS显示
    private void InitF10FpsSwitch() {
      var GameFpsStat = UIRoot.transform.Find("GameFpsStat");
      if(GameFpsStat != null) {
        GameFpsStat.SetAsLastSibling();
        if(!DebugMode) {
          ListenKey(KeyCode.F10, (k, d) => {
            if(d)
              GameFpsStat.gameObject.SetActive(!GameFpsStat.gameObject.activeSelf);
          });
        }
      } 
    }
    //F9截屏
    private void InitF9CaptureScreenshot() {
      ListenKey(KeyCode.F9, (k, d) => { if(d) GameManager.Instance.CaptureScreenshot(); });
    }
    //F8切换顶层视图的显示
    private void InitF8SwitchViews() {
      ListenKey(KeyCode.F8, (k, d) => {
        if(d) SetUIOverlayVisible(!TopViewsRectTransform.gameObject.activeSelf);
      });
    }

    /// <summary>
    /// 切换遮罩UI是否显示
    /// </summary>
    /// <param name="visible">是否显示</param>
    public void SetUIOverlayVisible(bool visible) {
      if(visible) {
        TopViewsRectTransform.gameObject.SetActive(true);
        ViewsRectTransform.gameObject.SetActive(true);
      } else {
        TopViewsRectTransform.gameObject.SetActive(false);
        ViewsRectTransform.gameObject.SetActive(false);
      }
    }

    #endregion

    /// <summary>
    /// UI 根 Canvas 的 RectTransform
    /// </summary>
    public RectTransform UIRoot;
    /// <summary>
    /// 渐变管理器
    /// </summary>
    public UIFadeManager UIFadeManager;

    private GameObject uiManagerGameObject = null;

    protected override void Update()
    {
      UpdateToastShow();
    }

    //根管理
    private RectTransform TemporarilyRectTransform;
    private RectTransform GlobalWindowRectTransform;
    private RectTransform PagesRectTransform;
    private RectTransform WindowsRectTransform;
    private RectTransform TopWindowsRectTransform;
    private RectTransform ViewsRectTransform;
    private RectTransform TopViewsRectTransform;
    private RectTransform OthersRectTransform;

    /// <summary>
    /// UI 根 RectTransform
    /// </summary>
    public RectTransform UIRootRectTransform { get; private set; }

    private void InitAllObects()
    {
      UIRootRectTransform = UIRoot.GetComponent<RectTransform>();
      TemporarilyRectTransform = CloneUtils.CreateEmptyUIObjectWithParent(UIRoot.transform, "GameUITemporarily").GetComponent<RectTransform>();
      TemporarilyRectTransform.gameObject.SetActive(false);
      GlobalWindowRectTransform = CloneUtils.CreateEmptyUIObjectWithParent(UIRoot.transform, "GameUIGlobalWindow").GetComponent<RectTransform>();
      PagesRectTransform = CloneUtils.CreateEmptyUIObjectWithParent(UIRoot.transform, "GameUIPages").GetComponent<RectTransform>();
      ViewsRectTransform = CloneUtils.CreateEmptyUIObjectWithParent(UIRoot.transform, "GameViewsRectTransform").GetComponent<RectTransform>();
      TopViewsRectTransform = CloneUtils.CreateEmptyUIObjectWithParent(UIRoot.transform, "GameTopViewsRectTransform").GetComponent<RectTransform>();
      WindowsRectTransform = CloneUtils.CreateEmptyUIObjectWithParent(UIRoot.transform, "GameUIWindow").GetComponent<RectTransform>();
      TopWindowsRectTransform = CloneUtils.CreateEmptyUIObjectWithParent(UIRoot.transform, "GameTopUIWindow").GetComponent<RectTransform>();
      OthersRectTransform = CloneUtils.CreateEmptyUIObjectWithParent(UIRoot.transform, "GameUIOthers").GetComponent<RectTransform>();

      InitAllPrefabs();

      UIAnchorPosUtils.SetUIAnchor(TopViewsRectTransform, UIAnchor.Stretch, UIAnchor.Stretch);
      UIAnchorPosUtils.SetUIPos(TopViewsRectTransform, 0, 0, 0, 0);
      UIAnchorPosUtils.SetUIAnchor(ViewsRectTransform, UIAnchor.Stretch, UIAnchor.Stretch);
      UIAnchorPosUtils.SetUIPos(ViewsRectTransform, 0, 0, 0, 0);
      UIAnchorPosUtils.SetUIAnchor(PagesRectTransform, UIAnchor.Stretch, UIAnchor.Stretch);
      UIAnchorPosUtils.SetUIPos(PagesRectTransform, 0, 0, 0, 0);
      UIAnchorPosUtils.SetUIAnchor(GlobalWindowRectTransform, UIAnchor.Stretch, UIAnchor.Stretch);
      UIAnchorPosUtils.SetUIPos(GlobalWindowRectTransform, 0, 0, 0, 0);
      UIAnchorPosUtils.SetUIAnchor(WindowsRectTransform, UIAnchor.Stretch, UIAnchor.Stretch);
      UIAnchorPosUtils.SetUIPos(WindowsRectTransform, 0, 0, 0, 0);
      UIAnchorPosUtils.SetUIAnchor(TopWindowsRectTransform, UIAnchor.Stretch, UIAnchor.Stretch);
      UIAnchorPosUtils.SetUIPos(TopWindowsRectTransform, 0, 0, 0, 0);

      UIToast = CloneUtils.CloneNewObjectWithParent(GameStaticResourcesPool.FindStaticPrefabs("PrefabToast"), UIRoot.transform, "GlobalUIToast").GetComponent<RectTransform>();
      UIToastImage = UIToast.GetComponent<Image>();
      UIToastText = UIToast.Find("Text").GetComponent<Text>();
      UIToast.gameObject.SetActive(false);
      UIToast.SetAsLastSibling();
      EventTriggerListener.Get(UIToast.gameObject).onClick = (g) => { toastTimeTick = 1; };

      keyListener = KeyListener.Get(UIRoot.gameObject);
      keyListener.DisableWhenUIFocused = false;
    }

    private KeyListener keyListener = null;

    /// <summary>
    /// 侦听某个键盘按键一次
    /// </summary>
    /// <param name="code">按键值</param>
    /// <param name="pressedOrReleased">如果为true，则侦听按下事件，否则侦听松开事件</param>
    /// <param name="callback">回调</param>
    /// <returns>返回一个ID, 可使用 DeleteKeyListen 删除侦听</returns>
    public int WaitKey(KeyCode code, bool pressedOrReleased, VoidDelegate callback,params GamepadButton[] gamepadButtons)
    {
      int id = 0;
      id = keyListener.AddKeyListen(code, (key, down) =>
      {
        if (down == pressedOrReleased)
        {
          keyListener.DeleteKeyListen(id);
          if(callback != null) 
            callback();
        }
      }, gamepadButtons);
      return id;
    }

    /// <summary>
    /// 添加键盘按键侦听
    /// </summary>
    /// <param name="key">键值</param>
    /// <param name="callBack">回调函数</param>
    /// <returns>返回一个ID, 可使用 DeleteKeyListen 删除侦听</returns>
    public int ListenKey(KeyCode key, KeyListener.KeyDelegate callBack, params GamepadButton[] gamepadButtons)
    {
      return keyListener.AddKeyListen(key, callBack, gamepadButtons);
    }

    /// <summary>
    /// 删除侦听按键
    /// </summary>
    /// <param name="id">AddKeyListen 返回的ID</param>
    public void DeleteKeyListen(int id)
    {
      keyListener.DeleteKeyListen(id);
    }

    /// <summary>
    /// 获取当前鼠标是否在UI上
    /// </summary>
    /// <returns></returns>
    public bool IsUiFocus()
    {
      return EventSystem.current.IsPointerOverGameObject() || GUIUtility.hotControl != 0;
    }

    #endregion

    #region UI Prefab

    private struct UIPrefab
    {
      public GameObject prefab;
      public GameUIPrefabType type;
    }
    private Dictionary<string, UIPrefab> uIPrefabs = new Dictionary<string, UIPrefab>();

    /// <summary>
    /// 获取 UI 控件预制体
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns>如果找到指定名称预制体，则返回其实例，如果未找到，则返回 null</returns>
    public GameObject GetUIPrefab(string name, GameUIPrefabType type)
    {
      if (uIPrefabs.ContainsKey(name))
      {
        uIPrefabs.TryGetValue(name, out UIPrefab prefab);
        return prefab.type == type ? prefab.prefab : null;
      }
      return null;
    }

    /// <summary>
    /// 注册 UI 控件预制体
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="perfab">预制体</param>
    /// <returns>返回注册是否成功</returns>
    public bool RegisterUIPrefab(string name, GameUIPrefabType type, GameObject perfab)
    {
      if (uIPrefabs.ContainsKey(name))
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, "UI Prefab {0} 已经注册", name);
        return false;
      }
      UIPrefab uiprefab = new UIPrefab();
      uiprefab.type = type;
      uiprefab.prefab = perfab;
      uIPrefabs[name] = uiprefab;
      return true;
    }

    /// <summary>
    /// 清除已注册的 UI 控件预制体
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns>返回是否成功</returns>
    public bool RemoveUIPrefab(string name)
    {
      if (uIPrefabs.ContainsKey(name))
      {
        uIPrefabs.Remove(name);
        return true;
      }
      else
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG, "UI控件Prefab {0} 未注册", name);
        return false;
      }
    }

    private void InitAllPrefabs()
    {
    }

    #endregion

    #region 页管理

    private Dictionary<string, GameUIPage> pages = new Dictionary<string, GameUIPage>();
    private List<GameUIPage> pageStack = new List<GameUIPage>();
    private GameUIPage currentPage = null;

    /// <summary>
    /// 查找页
    /// </summary>
    /// <param name="name">页名称</param>
    /// <returns>返回找到的页实例，如果找不到则返回null</returns>
    public GameUIPage FindPage(string name)
    {
      pages.TryGetValue(name, out GameUIPage w);
      return w;
    }

    /// <summary>
    /// 注册页
    /// </summary>
    /// <param name="name">页名称</param>
    /// <param name="prefabName">页模板名称</param>
    /// <returns>返回新创建的页实例，如果失败则返回null，请查看LastError</returns>
    public GameUIPage RegisterPage(string name, string prefabName)
    {
      if (pages.TryGetValue(name, out GameUIPage pageOld))
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, "页 {0} 已经注册", name);
        return pageOld;
      }
      GameObject prefab = GetUIPrefab(prefabName, GameUIPrefabType.Page);
      if (prefab == null)
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.PrefabNotFound, TAG, "未找到页模板 {0}", prefabName);
        return null;
      }
      GameObject go = CloneUtils.CloneNewObjectWithParent(prefab, PagesRectTransform, name);
      GameUIPage page = go.GetComponent<GameUIPage>();
      if (page == null)
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.ClassNotFound, TAG, "页模板上未找到 GameUIPage 类");
        return null;
      }
      page.PageName = name;
      pages.Add(name, page);
      GameManager.Instance.StartCoroutine(LateHidePage(go));
      return page;
    }

    private IEnumerator LateHidePage(GameObject go)
    {
      yield return new WaitForSeconds(0.5f);
      go.SetActive(false);
    }

    /// <summary>
    /// 跳转到页
    /// </summary>
    /// <param name="name">页名称</param>
    /// <returns>返回跳转是否成功</returns>
    public bool GoPage(string name)
    {
      return GoPageWithOptions(name, new Dictionary<string, string>());
    }

    /// <summary>
    /// 跳转到页并携带参数
    /// </summary>
    /// <param name="name">页名称</param>
    /// <param name="options">打开页所需要携带的参数，在页中可以使用 `GameUIPage.LastOption` 读取到传递进入页的参数。</param>
    /// <returns>返回跳转是否成功</returns>
    public bool GoPageWithOptions(string name, Dictionary<string, string> options)
    {
      if (currentPage != null && currentPage.name == name)
        return true;
      if (!pages.TryGetValue(name, out GameUIPage page))
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG, "页 {0} 未注册", name);
        return false;
      }

      //Hide old
      if (pageStack.Count > 0) pageStack[pageStack.Count - 1].Hide();
      if (!pageStack.Contains(page)) pageStack.Add(page);

      page.LastOptions = options;
      page.Show();
      currentPage = page;
      return true;
    }

    /// <summary>
    /// 获取当前显示的页实例
    /// </summary>
    /// <returns></returns>
    public GameUIPage GetCurrentPage() { return currentPage; }

    /// <summary>
    /// 隐藏当前显示的页
    /// </summary>
    /// <returns></returns>
    public void HideCurrentPage()
    {
      if (currentPage != null)
        currentPage.Hide();
    }

    /// <summary>
    /// 关闭所有显示的页
    /// </summary>
    public void CloseAllPage()
    {
      foreach (var p in pageStack)
        p.Hide();
      currentPage = null;
      pageStack.Clear();
    }

    /// <summary>
    /// 返回上一页
    /// </summary>
    /// <returns>如果可以返回，则返回true，否则返回false</returns>
    public bool BackPreviusPage()
    {
      return BackPreviusPageWithOptions(new Dictionary<string, string>());
    }

    /// <summary>
    /// 返回上一页并携带参数
    /// </summary>
    /// <param name="options">传递返回上一页参数，页中可以使用 `GameUIPage.LastBackOptions` 读取到传递进入上一页的参数。</param>
    /// <returns>如果可以返回，则返回true，否则返回false</returns>
    public bool BackPreviusPageWithOptions(Dictionary<string, string> options)
    {
      if (pageStack.Count > 0)
      {
        //隐藏当前
        var page = pageStack[pageStack.Count - 1];
        pageStack.RemoveAt(pageStack.Count - 1);

        if(page != null) {
          page.Hide();
          if (pageStack.Count > 0)
          {
            //显示前一个
            page = pageStack[pageStack.Count - 1];
            page.LastBackOptions = options;
            page.OnBackFromChild?.Invoke(options);
            page.Show();
            currentPage = page;
          }
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// 取消注册页
    /// </summary>
    /// <param name="name">页名称</param>
    /// <returns>返回是否成功</returns>
    public bool UnRegisterPage(string name)
    {
      if (pages.TryGetValue(name, out GameUIPage page))
      {
        if (page == currentPage && pageStack.Count > 1) BackPreviusPage();
        if (pageStack.Contains(page)) pageStack.Remove(page);
        if(page != null) Object.Destroy(page.gameObject);

        pages.Remove(name);
        return true;
      }
      GameErrorChecker.LastError = GameError.NotRegister;
      return false;
    }

    #endregion

    #region 全局对话框

    private RectTransform UIToast;
    private Image UIToastImage;
    private Text UIToastText;
    private FadeObject UIToastImageFadeObject;
    private FadeObject UIToastTextFadeObject;

    private float toastTimeTick = 0;

    /// <summary>
    /// 显示全局土司提示
    /// </summary>
    /// <param name="text">提示文字</param>
    public void GlobalToast(string text)
    {
      GlobalToast(text, text.Length / 30.0f);
    }

    /// <summary>
    /// 显示全局土司提示
    /// </summary>
    /// <param name="text">提示文字</param>
    /// <param name="showSec">显示时长（秒）</param>
    public void GlobalToast(string text, float showSec)
    {
      if (showSec <= 0.5f) showSec = 0.5f;
      ShowToast(text, showSec);
    }
    
    private void ShowToast(string text, float time)
    {
      UIToastText.text = text;
      float h = UIToastText.preferredHeight;
      UIToast.sizeDelta = new Vector2(UIToast.sizeDelta.x, h > 50 ? h : 50);
      UIToast.gameObject.SetActive(true);
      UIToast.SetAsLastSibling();
      toastTimeTick = time + 0.25f;

      if(UIToastTextFadeObject != null) {
        UIToastTextFadeObject.ResetTo(1);
        UIToastTextFadeObject.Delete();
        UIToastTextFadeObject = null;
      } else {
        UIToastTextFadeObject = UIFadeManager.AddFadeIn(UIToastText, 0.26f);
      }
      if(UIToastImageFadeObject != null) {
        UIToastImageFadeObject.ResetTo(1);
        UIToastImageFadeObject.Delete();
        UIToastImageFadeObject = null;
      } else {
        UIToastImageFadeObject = UIFadeManager.AddFadeIn(UIToastImage, 0.26f);
      }
    }
    private void UpdateToastShow()
    {
      if (toastTimeTick > 0)
      {
        toastTimeTick -= Time.deltaTime;
        if (toastTimeTick <= 0)
        {
          if(UIToastTextFadeObject != null) {
            UIToastTextFadeObject.ResetTo(0);
            UIToastTextFadeObject.Delete();
            UIToastTextFadeObject = null;
          } else {
            UIToastTextFadeObject = UIFadeManager.AddFadeOut(UIToastText, 0.4f, false);
          }
          if(UIToastImageFadeObject != null) {
            UIToastImageFadeObject.ResetTo(0);
            UIToastImageFadeObject.Delete();
            UIToastImageFadeObject = null;
          } else {
            UIToastImageFadeObject = UIFadeManager.AddFadeOut(UIToastImage, 0.4f, false);
          }
        }
      }
    }

    private VoidDelegate GlobalConfirmWindoOnConfirm = null;
    private VoidDelegate GlobalConfirmWindoOnCancel = null;

    private void InitGlobalPage() {

    }

    internal void GlobalConfirmWindowCallback(bool isConfirm) {
      if (isConfirm) {
        if (GlobalConfirmWindoOnConfirm != null) {
          GlobalConfirmWindoOnConfirm.Invoke();
          GlobalConfirmWindoOnConfirm = null;
        }
      } else {
        if (GlobalConfirmWindoOnCancel != null) {
          GlobalConfirmWindoOnCancel.Invoke();
          GlobalConfirmWindoOnCancel = null;
        }
      }
    }

    /// <summary>
    /// 显示全局 Confirm 独占对话框
    /// </summary>
    /// <param name="text">内容</param>
    /// <param name="okText">OK 按钮文字</param>
    /// <param name="cancelText">Cancel 按钮文字</param>
    /// <returns></returns>
    public void GlobalConfirmWindow(string text, VoidDelegate onConfirm, VoidDelegate onCancel, string okText = null, string cancelText = null)
    {
      GlobalConfirmWindoOnConfirm = onConfirm;
      GlobalConfirmWindoOnCancel = onCancel;
      var options = new Dictionary<string, string>();
      options.Add("text", text);
      options.Add("okText", okText ?? I18N.I18N.Tr("core.ui.Ok"));
      options.Add("cancelText", cancelText ??  I18N.I18N.Tr("core.ui.Cancel"));
      GoPageWithOptions("PageGlobalConfirm", options);
    }



    #endregion

    #region 全局渐变遮罩

    private Image GlobalFadeMaskWhite;
    private Image GlobalFadeMaskBlack;

    /// <summary>
    /// 全局黑色遮罩控制（无渐变动画）
    /// </summary>
    /// <param name="show">为true则显示遮罩，否则隐藏</param>
    public void MaskBlackSet(bool show)
    {
      GlobalFadeMaskBlack.color = new Color(GlobalFadeMaskBlack.color.r, GlobalFadeMaskBlack.color.g, GlobalFadeMaskBlack.color.b, show ? 1.0f : 0f);
      GlobalFadeMaskBlack.gameObject.SetActive(show);
      GlobalFadeMaskBlack.transform.SetAsLastSibling();
    }

    /// <summary>
    /// 全局白色遮罩控制（无渐变动画）
    /// </summary>
    /// <param name="show">为true则显示遮罩，否则隐藏</param>
    public void MaskWhiteSet(bool show)
    {
      GlobalFadeMaskWhite.color = new Color(GlobalFadeMaskWhite.color.r, GlobalFadeMaskWhite.color.g, GlobalFadeMaskWhite.color.b, show ? 1.0f : 0f);
      GlobalFadeMaskWhite.gameObject.SetActive(show);
      GlobalFadeMaskWhite.transform.SetAsLastSibling();
    }

    /// <summary>
    /// 全局黑色遮罩渐变淡入
    /// </summary>
    /// <param name="second">耗时（秒）</param>
    public void MaskBlackFadeIn(float second)
    {
      UIFadeManager.AddFadeIn(GlobalFadeMaskBlack, second);
      GlobalFadeMaskBlack.transform.SetAsLastSibling();
    }

    /// <summary>
    /// 全局白色遮罩渐变淡入
    /// </summary>
    /// <param name="second">耗时（秒）</param>
    public void MaskWhiteFadeIn(float second)
    {
      UIFadeManager.AddFadeIn(GlobalFadeMaskWhite, second);
      GlobalFadeMaskWhite.transform.SetAsLastSibling();
    }

    /// <summary>
    /// 全局黑色遮罩渐变淡出
    /// </summary>
    /// <param name="second">耗时（秒）</param>
    public void MaskBlackFadeOut(float second)
    {
      UIFadeManager.AddFadeOut(GlobalFadeMaskBlack, second, true);
      GlobalFadeMaskBlack.transform.SetAsLastSibling();
    }

    /// <summary>
    /// 全局白色遮罩渐变淡出
    /// </summary>
    /// <param name="second">耗时（秒）</param>
    public void MaskWhiteFadeOut(float second)
    {
      UIFadeManager.AddFadeOut(GlobalFadeMaskWhite, second, true);
      GlobalFadeMaskWhite.transform.SetAsLastSibling();
    }

    #endregion

    #region 通用管理

    /// <summary>
    /// 设置一个UI至临时区域
    /// </summary>
    /// <param name="view">指定UI</param>
    public void SetViewToTemporarily(RectTransform view)
    {
      view.SetParent(TemporarilyRectTransform.gameObject.transform);
    }
    /// <summary>
    /// 将一个UI附加到主Canvas
    /// </summary>
    /// <param name="view">指定UI</param>
    public void AttatchViewToCanvas(RectTransform view)
    {
      view.SetParent(UIRoot.gameObject.transform);
    }
    /// <summary>
    /// 使用Prefab初始化一个对象并附加到主Canvas
    /// </summary>
    /// <param name="prefab">Prefab</param>
    /// <param name="name">新对象名称</param>
    /// <returns>返回新对象的RectTransform</returns>
    public RectTransform InitViewToCanvas(GameObject prefab, string name, bool topMost)
    {
      GameObject go = CloneUtils.CloneNewObjectWithParent(prefab, (topMost ? TopViewsRectTransform : ViewsRectTransform).transform, name);
      RectTransform view = go.GetComponent<RectTransform>();
      view.SetParent((topMost ? TopViewsRectTransform : ViewsRectTransform).gameObject.transform);
      return view;
    }

    /// <summary>
    /// 创建一个UI消息中心
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns>返回UI消息中心实例</returns>
    public GameUIMessageCenter CreateUIMessageCenter(string name)
    {
      var old = GameUIMessageCenter.FindGameUIMessageCenter(name);
      if (old == null)
      {
        var go = CloneUtils.CreateEmptyObjectWithParent(OthersRectTransform.transform, "GameUIMessageCenter:" + name);
        old = go.AddComponent<GameUIMessageCenter>();
        old.Name = name;
      }
      return old;
    }
    /// <summary>
    /// 销毁一个UI消息中心
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns>返回是否成功</returns>
    public bool DestroyUIMessageCenter(string name)
    {
      var old = GameUIMessageCenter.FindGameUIMessageCenter(name);
      if (old == null)
      {
        Object.Destroy(old);
        return true;
      }
      return false;
    }
    #endregion

    #region 调试命令

    private void InitCommands()
    {
      GameManager.Instance.GameDebugCommandServer.RegisterCommand("um", (keyword, full, argsCount, args) =>
      {
        switch (args[0])
        {
          case "page":
            {
              switch (args[1])
              {
                case "current":
                  Log.V(TAG, "CurrentPage: {0}", currentPage == null ? "null" : currentPage.PageName);
                  break;
                case "stack":
                  {
                    StringBuilder sb = new StringBuilder("CurrentPage: Stack {0}");
                    sb.Append(pageStack.Count);
                    sb.Append("\n");
                    foreach (var p in pageStack)
                      sb.AppendLine(string.Format("{0} => Visible: {1}", p.PageName, p.gameObject.activeInHierarchy));
                    Log.V(TAG, sb.ToString());
                    break;
                  }
                case "hide-cur":
                  HideCurrentPage();
                  Log.V(TAG, "OK");
                  break;
                case "close-all":
                  CloseAllPage();
                  Log.V(TAG, "OK");
                  break;
                case "back":
                  Log.V(TAG, BackPreviusPage() ? "OK" : "No page can back");
                  break;
                case "go":
                  string name = "";
                  if (!DebugUtils.CheckDebugParam(2, args, out name)) break;
                  if (GoPage(name)) Log.V(TAG, "OK");
                  break;
              }
              break;
            }
          case "toast":
            {
              string text; float sec;
              if (!DebugUtils.CheckDebugParam(1, args, out text)) return false;
              DebugUtils.CheckFloatDebugParam(2, args, out sec, false, 1);
              GlobalToast(text, sec);
              return true;
            }
        }
        return false;
      }, 2, "um <page/toast>\n" +
              "  page\n" +
              "    current 显示当前显示的页\n" +
              "    stack 显示当前显示页的栈\n" +
              "    go <name:string> 跳转到指定页\n" +
              "    hide-cur 隐藏当前页\n" +
              "    close-all 关闭所有显示的页\n" +
              "    back 返回上一页\n" +
              "  toast <text:string> [showSecond:number] 测试全局弹出土司提示\n"
      );
    }

    #endregion
  }

  /// <summary>
  /// UIPrefab的类型
  /// </summary>
  public enum GameUIPrefabType
  {
    /// <summary>
    /// 控件
    /// </summary>
    Control,
    /// <summary>
    /// 页
    /// </summary>
    Page
  }
}

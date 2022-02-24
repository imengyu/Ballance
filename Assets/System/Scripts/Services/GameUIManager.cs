using Ballance2.Base;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services.Debug;
using Ballance2.Services.InputManager;
using Ballance2.UI.Core;
using Ballance2.UI.Core.Side;
using Ballance2.UI.Utils;
using Ballance2.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
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
  /// UI 管理器
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("UI 管理器")]
  public class GameUIManager : GameService
  {
    #region 基础

    private static readonly string TAG = "GameUIManager";

    public GameUIManager() : base(TAG) { }

    private GameObject GameUICommonHost;

    [SLua.DoNotToLua]
    public override void Destroy()
    {
      DestroyWindowManagement();
      Object.Destroy(uiManagerGameObject);

      if(UIRoot != null) {
        Log.D(TAG, "Destroy {0} ui objects", UIRoot.transform.childCount);
        for (int i = 0, c = UIRoot.transform.childCount; i < c; i++)
          Object.Destroy(UIRoot.transform.GetChild(i).gameObject);
      }
    } 
    [SLua.DoNotToLua]
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
            InitWindowManagement();
            InitCommands();

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

            //发送就绪事件
            GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_UI_MANAGER_INIT_FINISHED, "*");
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

    /// <summary>
    /// UI 根
    /// </summary>
    [LuaApiDescription("UI 根")]
    public RectTransform UIRoot;
    /// <summary>
    /// 渐变管理器
    /// </summary>
    [LuaApiDescription("渐变管理器")]
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
    [LuaApiDescription("UI 根 RectTransform")]
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
    /// 侦听某个按键一次
    /// </summary>
    /// <param name="code">按键值</param>
    /// <param name="pressedOrReleased">如果为true，则侦听按下事件，否则侦听松开事件</param>
    /// <param name="callback">回调</param>
    /// <returns>返回一个ID, 可使用 DeleteKeyListen 删除侦听器</returns>
    [LuaApiDescription("侦听某个按键一次", "返回一个ID, 可使用 DeleteKeyListen 删除侦听器")]
    [LuaApiParamDescription("key", "键值")]
    [LuaApiParamDescription("pressedOrReleased", "如果为true，则侦听按下事件，否则侦听松开事件")]
    [LuaApiParamDescription("callBack", "回调函数")]
    public int WaitKey(KeyCode code, bool pressedOrReleased, VoidDelegate callback)
    {
      if(callback != null) {
        int id = 0;
        id = keyListener.AddKeyListen(code, (key, down) =>
        {
          if (down == pressedOrReleased)
          {
            keyListener.DeleteKeyListen(id);
            if(callback != null) callback();
          }
        });
        return id;
      }
      return 0;
    }
    /// <summary>
    /// 添加侦听器侦听键。
    /// </summary>
    /// <param name="key">键值。</param>
    /// <param name="callBack">回调函数。</param>
    /// <returns>返回一个ID, 可使用 DeleteKeyListen 删除侦听器</returns>
    [LuaApiDescription("添加侦听器侦听键。", "返回一个ID, 可使用 DeleteKeyListen 删除侦听器")]
    [LuaApiParamDescription("key", "键值")]
    [LuaApiParamDescription("callBack", "回调函数")]
    public int ListenKey(KeyCode key, KeyListener.KeyDelegate callBack)
    {
      return keyListener.AddKeyListen(key, callBack);
    }
    /// <summary>
    /// 删除侦听按键
    /// </summary>
    /// <param name="id">AddKeyListen 返回的ID</param>
    [LuaApiDescription("删除侦听按键")]
    [LuaApiParamDescription("id", "AddKeyListen 返回的ID")]
    public void DeleteKeyListen(int id)
    {
      keyListener.DeleteKeyListen(id);
    }

    /// <summary>
    /// 获取当前鼠标是否在UI上
    /// </summary>
    /// <returns></returns>
    [LuaApiDescription("获取当前鼠标是否在UI上")]
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
    /// <returns></returns>
    [LuaApiDescription("获取 UI 控件预制体")]
    [LuaApiParamDescription("name", "名称")]
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
    [LuaApiDescription("注册 UI 控件预制体", "返回注册是否成功")]
    [LuaApiParamDescription("name", "名称")]
    [LuaApiParamDescription("perfab", "预制体")]
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
    [LuaApiDescription("清除已注册的 UI 控件预制体", "返回是否成功")]
    [LuaApiParamDescription("name", "名称")]
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
      RegisterUIPrefab("PageFull", GameUIPrefabType.Page, GameStaticResourcesPool.FindStaticPrefabs("PrefabPageFull"));
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
    [LuaApiDescription("查找页", "返回找到的页实例，如果找不到则返回null")]
    [LuaApiParamDescription("name", "页名称")]
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
    [LuaApiDescription("注册页", "返回新创建的页实例，如果失败则返回null，请查看LastError")]
    [LuaApiParamDescription("name", "页名称")]
    [LuaApiParamDescription("prefabName", "页模板名称")]
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
      yield return new WaitForSeconds(0.3f);
      go.SetActive(false);
    }

    /// <summary>
    /// 跳转到页
    /// </summary>
    /// <param name="name">页名称</param>
    /// <returns></returns>
    [LuaApiDescription("跳转到页")]
    [LuaApiParamDescription("name", "页名称")]
    public bool GoPage(string name)
    {
      return GoPageWithOptions(name, new Dictionary<string, string>());
    }
    /// <summary>
    /// 跳转到页并携带参数
    /// </summary>
    /// <param name="name">页名称</param>
    /// <param name="options">参数</param>
    /// <returns></returns>
    [LuaApiDescription("跳转到页并携带参数")]
    [LuaApiParamDescription("name", "页名称")]
    [LuaApiParamDescription("options", "参数")]
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
    /// 获取当前显示页
    /// </summary>
    /// <returns></returns>
    [LuaApiDescription("获取当前显示页")]
    public GameUIPage GetCurrentPage() { return currentPage; }
    /// <summary>
    /// 隐藏当前显示页
    /// </summary>
    /// <returns></returns>
    [LuaApiDescription("隐藏当前显示页")]
    public void HideCurrentPage()
    {
      if (currentPage != null)
        currentPage.Hide();
    }
    /// <summary>
    /// 关闭所有显示的页
    /// </summary>
    [LuaApiDescription("关闭所有显示的页")]
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
    [LuaApiDescription("返回上一页", "如果可以返回，则返回true，否则返回false")]
    public bool BackPreviusPage()
    {
      return BackPreviusPageWithOptions(new Dictionary<string, string>());
    }
    /// <summary>
    /// 返回上一页并携带参数
    /// </summary>
    /// <returns>如果可以返回，则返回true，否则返回false</returns>
    [LuaApiDescription("返回上一页并携带参数", "如果可以返回，则返回true，否则返回false")]
    [LuaApiParamDescription("options", "参数")]
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
    [LuaApiDescription("取消注册页", "返回是否成功")]
    [LuaApiParamDescription("name", "页名称")]
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

    private struct ToastData
    {
      public string text;
      public float showTime;

      public ToastData(string t, float i)
      {
        text = t;
        showTime = i;
      }
    }

    private List<ToastData> toastDatas = new List<ToastData>();

    private float toastTimeTick = 0;
    private float toastNextDelayTimeTick = 0;

    /// <summary>
    /// 显示全局土司提示
    /// </summary>
    /// <param name="text">提示文字</param>
    [LuaApiDescription("显示全局土司提示")]
    [LuaApiParamDescription("text", "提示文字")]
    public void GlobalToast(string text)
    {
      GlobalToast(text, text.Length / 30.0f);
    }
    /// <summary>
    /// 显示全局土司提示
    /// </summary>
    /// <param name="text">提示文字</param>
    /// <param name="showSec">显示时长（秒）</param>
    [LuaApiDescription("显示全局土司提示")]
    [LuaApiParamDescription("text", "提示文字")]
    [LuaApiParamDescription("showSec", "显示时长（秒）")]
    public void GlobalToast(string text, float showSec)
    {
      if (showSec <= 0.5f) showSec = 0.5f;
      if (toastTimeTick <= 0) ShowToast(text, showSec);
      else toastDatas.Add(new ToastData(text, showSec));
    }

    private void ShowPendingToast()
    {
      if (toastDatas.Count > 0)
      {
        ShowToast(toastDatas[0].text, toastDatas[0].showTime);
        toastDatas.RemoveAt(0);
      }
    }
    private void ShowToast(string text, float time)
    {
      UIToastText.text = text;
      float h = UIToastText.preferredHeight;
      UIToast.sizeDelta = new Vector2(UIToast.sizeDelta.x, h > 50 ? h : 50);
      UIToast.gameObject.SetActive(true);
      UIToast.SetAsLastSibling();

      UIFadeManager.AddFadeIn(UIToastImage, 0.26f);
      UIFadeManager.AddFadeIn(UIToastText, 0.25f);
      toastTimeTick = time + 0.25f;
    }
    private void UpdateToastShow()
    {
      if (toastTimeTick >= 0)
      {
        toastTimeTick -= Time.deltaTime;
        if (toastTimeTick <= 0)
        {
          UIFadeManager.AddFadeOut(UIToastImage, 0.4f, true);
          UIFadeManager.AddFadeOut(UIToastText, 0.4f, false);
          toastNextDelayTimeTick = 1.0f;
        }
      }
      if (toastNextDelayTimeTick >= 0)
      {
        toastNextDelayTimeTick -= Time.deltaTime;
        if (toastNextDelayTimeTick <= 0) ShowPendingToast();
      }
    }

    /// <summary>
    /// 显示全局 Alert 对话框（窗口模式）
    /// </summary>
    /// <param name="text">内容</param>
    /// <param name="title">标题</param>
    /// <param name="okText">OK 按钮文字</param>
    /// <returns>返回对话框ID</returns>
    [LuaApiDescription("显示全局 Alert 对话框（窗口模式）", "返回对话框ID")]
    [LuaApiParamDescription("text", "内容")]
    [LuaApiParamDescription("title", "标题")]
    [LuaApiParamDescription("onConfirm", "OK 按钮点击回调")]
    [LuaApiParamDescription("okText", "OK 按钮文字")]
    public int GlobalAlertWindow(string text, string title, VoidDelegate onConfirm, string okText = null)
    {
      if(okText == null) okText = I18N.I18N.Tr("core.ui.Ok");

      RectTransform rectTransform = InitViewToCanvas(PrefabUIAlertWindow, "AlertWindow", true);
      Button btnOk = rectTransform.Find("Button").GetComponent<Button>();
      rectTransform.Find("DialogText").GetComponent<Text>().text = text;
      rectTransform.Find("Button/Text").GetComponent<Text>().text = okText;

      btnOk.onClick.AddListener(() =>
      {
        onConfirm?.Invoke();
        PagesRectTransform.gameObject.SetActive(true);
        WindowsRectTransform.gameObject.SetActive(true);
        GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_GLOBAL_ALERT_CLOSE, "*", 0, false);
      });
      
      PagesRectTransform.gameObject.SetActive(false);
      WindowsRectTransform.gameObject.SetActive(false);
      return 0;
    }
    /// <summary>
    /// 显示全局 Confirm 对话框（窗口模式）
    /// </summary>
    /// <param name="text">内容</param>
    /// <param name="title">标题</param>
    /// <param name="okText">OK 按钮文字</param>
    /// <param name="cancelText">Cancel 按钮文字</param>
    /// <returns></returns>
    [LuaApiDescription("显示全局 Confirm 对话框（窗口模式）", "返回对话框ID")]
    [LuaApiParamDescription("text", "内容")]
    [LuaApiParamDescription("title", "标题")]
    [LuaApiParamDescription("onConfirm", "OK 按钮点击回调")]
    [LuaApiParamDescription("onCancel", "Cancel 按扭点击回调")]
    [LuaApiParamDescription("okText", "OK 按钮文字")]
    [LuaApiParamDescription("cancelText", "Cancel 按钮文字")]
    public int GlobalConfirmWindow(string text, string title, VoidDelegate onConfirm, VoidDelegate onCancel, string okText = null, string cancelText = null)
    {
      if(okText == null) okText = I18N.I18N.Tr("core.ui.Ok");
      if(cancelText == null) cancelText = I18N.I18N.Tr("core.ui.Cancel");

      RectTransform rectTransform = InitViewToCanvas(PrefabUIConfirmWindow, "ConfirmWindow", true);
      Button btnYes = rectTransform.Find("ButtonConfirm").GetComponent<Button>();
      Button btnNo = rectTransform.Find("ButtonCancel").GetComponent<Button>();
      rectTransform.Find("ButtonConfirm/Text").GetComponent<Text>().text = okText;
      rectTransform.Find("ButtonCancel/Text").GetComponent<Text>().text = cancelText;
      rectTransform.Find("DialogText").GetComponent<Text>().text = text;

      btnYes.onClick.AddListener(() =>
      {
        onConfirm?.Invoke();
        PagesRectTransform.gameObject.SetActive(true);
        WindowsRectTransform.gameObject.SetActive(true);
        GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_GLOBAL_ALERT_CLOSE, "*", 1, true);
      });
      btnNo.onClick.AddListener(() =>
      {
        onCancel?.Invoke();
        PagesRectTransform.gameObject.SetActive(true);
        WindowsRectTransform.gameObject.SetActive(true);
        GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_GLOBAL_ALERT_CLOSE, "*", 1, false);
      });

      PagesRectTransform.gameObject.SetActive(false);
      WindowsRectTransform.gameObject.SetActive(false);
      return 1;
    }

    #endregion

    #region 窗口管理

    private List<int> returnWindowIds = new List<int>();
    private int startWindowId = 16;

    private void ReturnWindowId(int id)
    {
      if (!returnWindowIds.Contains(id))
        returnWindowIds.Add(id);
    }
    internal int GenWindowId()
    {
      if (returnWindowIds.Count > 0)
      {
        int result = returnWindowIds[0];
        returnWindowIds.RemoveAt(0);
        return result;
      }
      if (startWindowId < 0xffff)
        startWindowId++;
      else
        startWindowId = 0;
      return startWindowId;
    }

    private GameObject PrefabUIAlertWindow;
    private GameObject PrefabUIConfirmWindow;
    private GameObject PrefabUIWindow;

    internal SideTabBar SideTabBar;

    private void InitWindowManagement()
    {
      managedWindows = new Dictionary<int, Window>();

      PrefabUIAlertWindow = GameStaticResourcesPool.FindStaticPrefabs("PrefabAlertWindow");
      PrefabUIConfirmWindow = GameStaticResourcesPool.FindStaticPrefabs("PrefabConfirmWindow");
      PrefabUIWindow = GameStaticResourcesPool.FindStaticPrefabs("PrefabWindow");

      SideTabBar = InitViewToCanvas(GameStaticResourcesPool.FindStaticPrefabs("PrefabSideTabBar"), "SideTabBar", true).GetComponent<SideTabBar>();

      if(GameManager.GameMediator)
        GameManager.GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_GLOBAL_ALERT_CLOSE);
    }
    private void DestroyWindowManagement()
    {
      if(GameManager.GameMediator)
        GameManager.GameMediator.UnRegisterGlobalEvent(GameEventNames.EVENT_GLOBAL_ALERT_CLOSE);

      if (managedWindows != null)
      {
        var list = new List<Window>(managedWindows.Values);
        foreach (var w in list)
          w.Destroy();
        managedWindows.Clear();
        managedWindows = null;
      }
    }

    internal void InternalRemoveWindow(Window window)
    {
      if (managedWindows != null)
        managedWindows.Remove(window.windowId);
    }

    //窗口

    private Dictionary<int, Window> managedWindows = null;

    /// <summary>
    /// 创建自定义窗口（默认不显示）
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="customView">窗口自定义View</param>
    /// <returns>返回窗口实例</returns>
    [LuaApiDescription("创建自定义窗口（默认不显示）", "返回窗口实例")]
    [LuaApiParamDescription("title", "标题")]
    [LuaApiParamDescription("customView", "窗口自定义View")]
    public Window CreateWindow(string title, RectTransform customView)
    {
      return CreateWindow(title, customView, false);
    }
    /// <summary>
    /// 创建自定义窗口
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="show">创建后是否立即显示</param>
    /// <param name="customView">窗口自定义View</param>
    /// <returns>返回窗口实例</returns>
    [LuaApiDescription("创建自定义窗口", "返回窗口实例")]
    [LuaApiParamDescription("title", "标题")]
    [LuaApiParamDescription("show", "创建后是否立即显示")]
    [LuaApiParamDescription("title", "标题")]
    public Window CreateWindow(string title, RectTransform customView, bool show)
    {
      return CreateWindow(title, customView, show, 0, 0, 0, 0);
    }
    /// <summary>
    /// 创建自定义窗口
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="show">创建后是否立即显示</param>
    /// <param name="customView">窗口自定义View</param>
    /// <param name="x">X 坐标</param>
    /// <param name="y">Y 坐标</param>
    /// <param name="w">宽度，0 使用默认</param>
    /// <param name="h">高度，0 使用默认</param>
    /// <returns>返回窗口实例</returns>
    [LuaApiDescription("创建自定义窗口", "返回窗口实例")]
    [LuaApiParamDescription("title", "标题")]
    [LuaApiParamDescription("show", "创建后是否立即显示")]
    [LuaApiParamDescription("title", "标题")]
    [LuaApiParamDescription("x", "X 坐标")]
    [LuaApiParamDescription("y", "Y 坐标")]
    [LuaApiParamDescription("w", "宽度，0 使用默认")]
    [LuaApiParamDescription("h", "高度，0 使用默认")]
    public Window CreateWindow(string title, RectTransform customView, bool show, float x, float y, float w, float h)
    {
      GameObject windowGo = CloneUtils.CloneNewObjectWithParent(PrefabUIWindow, WindowsRectTransform.transform);
      Window window = windowGo.GetComponent<Window>();
      window.Title = title;
      window.SetView(customView);
      window.windowId = GenWindowId();
      window.enabled = true;
      RegisterWindow(window);

      window.onClose += (id) =>
      {
        ReturnWindowId(id);
        window.Destroy();
        managedWindows.Remove(window.GetWindowId());
      };

      if (w != 0 && h != 0) window.Size = new Vector2(w, h);
      if (x == 0 && y == 0) window.MoveToCenter();
      else window.Position = new Vector2(x, y);

      if (show) window.Show();
      else window.SetVisible(false);

      return window;
    }
    /// <summary>
    /// 注册窗口到管理器中
    /// </summary>
    /// <param name="window">窗口实例</param>
    /// <returns></returns>
    [LuaApiDescription("注册窗口到管理器中")]
    [LuaApiParamDescription("window", "窗口实例")]
    public Window RegisterWindow(Window window)
    {
      int id = window.GetWindowId();
      window.name = "GameUIWindow_" + id;
      if (!managedWindows.ContainsKey(id))
        managedWindows.Add(id, window);
      else
        managedWindows[id] = window;
      return window;
    }
    /// <summary>
    /// 通过 ID 查找窗口
    /// </summary>
    /// <param name="windowId">窗口ID</param>
    /// <returns>返回找到的窗口实例，如果找不到则返回null</returns>
    [LuaApiDescription("通过 ID 查找窗口", "返回找到的窗口实例，如果找不到则返回null")]
    [LuaApiParamDescription("windowId", "窗口ID")]
    public Window FindWindowById(int windowId)
    {
      managedWindows.TryGetValue(windowId, out Window w);
      return w;
    }

    private IEnumerator LateShowWindow(Window w)
    {
      yield return new WaitForSeconds(0.1f);
      w.Show();
    }

    internal void WindowFirshAdd(Window window)
    {
      switch (window.WindowType)
      {
        case WindowType.GlobalAlert:
          window.GetRectTransform().transform.SetParent(GlobalWindowRectTransform.transform);
          PagesRectTransform.gameObject.SetActive(false);
          currentVisibleWindowAlert = window;
          break;
        case WindowType.Normal:
          window.GetRectTransform().transform.SetParent(WindowsRectTransform.transform);
          break;
        case WindowType.TopWindow:
          window.GetRectTransform().transform.SetParent(TopWindowsRectTransform.transform);
          break;
      }
    }

    private Window currentVisibleWindowAlert = null;
    private Window currentActiveWindow = null;

    /// <summary>
    /// 获取当前激活的窗口
    /// </summary>
    /// <returns></returns>
    [LuaApiDescription("获取当前激活的窗口")]
    public Window GetCurrentActiveWindow() { return currentActiveWindow; }
    /// <summary>
    /// 显示窗口
    /// </summary>
    /// <param name="window">窗口实例</param>
    [LuaApiDescription("显示窗口")]
    [LuaApiParamDescription("window", "窗口实例")]
    public void ShowWindow(Window window)
    {
      window.Show();
    }
    /// <summary>
    /// 隐藏窗口
    /// </summary>
    /// <param name="window">窗口实例</param>
    [LuaApiDescription("隐藏窗口")]
    [LuaApiParamDescription("window", "窗口实例")]
    public void HideWindow(Window window) { window.Hide(); }
    /// <summary>
    /// 关闭窗口
    /// </summary>
    /// <param name="window">窗口实例</param>
    [LuaApiDescription("关闭窗口")]
    [LuaApiParamDescription("window", "窗口实例")]
    public void CloseWindow(Window window)
    {
      window.Close();
    }
    /// <summary>
    /// 激活窗口至最顶层
    /// </summary>
    /// <param name="window">窗口实例</param>
    [LuaApiDescription("激活窗口至最顶层")]
    [LuaApiParamDescription("window", "窗口实例")]
    public void ActiveWindow(Window window)
    {
      if (currentActiveWindow != null) 
        currentActiveWindow.WindowTitleImage.color = currentActiveWindow.TitleDefaultColor;
      currentActiveWindow = window;
      currentActiveWindow.WindowTitleImage.color = currentActiveWindow.TitleActiveColor;
      currentActiveWindow.WindowRectTransform.transform.SetAsLastSibling();
      SideTabBar.ActiveTab(window);
    }

    #endregion

    #region 全局渐变遮罩

    private Image GlobalFadeMaskWhite;
    private Image GlobalFadeMaskBlack;

    /// <summary>
    /// 全局黑色遮罩控制（无渐变动画）
    /// </summary>
    /// <param name="show">为true则显示遮罩，否则隐藏</param>
    [LuaApiDescription("全局黑色遮罩隐藏（无渐变动画）")]
    [LuaApiParamDescription("全局黑色遮罩控制", "为true则显示遮罩，否则隐藏")]
    public void MaskBlackSet(bool show)
    {
      GlobalFadeMaskBlack.color = new Color(GlobalFadeMaskBlack.color.r,
             GlobalFadeMaskBlack.color.g, GlobalFadeMaskBlack.color.b, show ? 1.0f : 0f);
      GlobalFadeMaskBlack.gameObject.SetActive(show);
      GlobalFadeMaskBlack.transform.SetAsLastSibling();
    }
    /// <summary>
    /// 全局白色遮罩控制（无渐变动画）
    /// </summary>
    /// <param name="show">为true则显示遮罩，否则隐藏</param>
    [LuaApiDescription("全局白色遮罩控制（无渐变动画）")]
    [LuaApiParamDescription("show", "为true则显示遮罩，否则隐藏")]
    public void MaskWhiteSet(bool show)
    {
      GlobalFadeMaskWhite.color = new Color(GlobalFadeMaskWhite.color.r,
          GlobalFadeMaskWhite.color.g, GlobalFadeMaskWhite.color.b, show ? 1.0f : 0f);
      GlobalFadeMaskWhite.gameObject.SetActive(show);
      GlobalFadeMaskWhite.transform.SetAsLastSibling();
    }
    /// <summary>
    /// 全局黑色遮罩渐变淡入
    /// </summary>
    /// <param name="second">耗时（秒）</param>
    [LuaApiDescription("全局黑色遮罩渐变淡入")]
    [LuaApiParamDescription("second", "耗时（秒）")]
    public void MaskBlackFadeIn(float second)
    {
      UIFadeManager.AddFadeIn(GlobalFadeMaskBlack, second);
      GlobalFadeMaskBlack.transform.SetAsLastSibling();
    }
    /// <summary>
    /// 全局白色遮罩渐变淡入
    /// </summary>
    /// <param name="second">耗时（秒）</param>
    [LuaApiDescription("全局白色遮罩渐变淡入")]
    [LuaApiParamDescription("second", "耗时（秒）")]
    public void MaskWhiteFadeIn(float second)
    {
      UIFadeManager.AddFadeIn(GlobalFadeMaskWhite, second);
      GlobalFadeMaskWhite.transform.SetAsLastSibling();
    }
    /// <summary>
    /// 全局黑色遮罩渐变淡出
    /// </summary>
    /// <param name="second">耗时（秒）</param>
    [LuaApiDescription("全局黑色遮罩渐变淡出")]
    [LuaApiParamDescription("second", "耗时（秒）")]
    public void MaskBlackFadeOut(float second)
    {
      UIFadeManager.AddFadeOut(GlobalFadeMaskBlack, second, true);
      GlobalFadeMaskBlack.transform.SetAsLastSibling();
    }
    /// <summary>
    /// 全局白色遮罩渐变淡出
    /// </summary>
    /// <param name="second">耗时（秒）</param>
    [LuaApiDescription("全局白色遮罩渐变淡出")]
    [LuaApiParamDescription("second", "耗时（秒）")]
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
    [LuaApiDescription("设置一个UI至临时区域")]
    [LuaApiParamDescription("view", "指定UI")]
    public void SetViewToTemporarily(RectTransform view)
    {
      view.SetParent(TemporarilyRectTransform.gameObject.transform);
    }
    /// <summary>
    /// 将一个UI附加到主Canvas
    /// </summary>
    /// <param name="view">指定UI</param>
    [LuaApiDescription("将一个UI附加到主Canvas")]
    [LuaApiParamDescription("view", "指定UI")]
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
    [LuaApiDescription("使用Prefab初始化一个对象并附加到主Canvas", "返回新对象的RectTransform")]
    [LuaApiParamDescription("prefab", "Prefab")]
    [LuaApiParamDescription("name", "新对象名称")]
    [LuaApiParamDescription("topMost", "是否置顶，置顶后会在遮罩层上出现，不会被遮挡")]
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
    [LuaApiDescription("创建一个UI消息中心", "返回UI消息中心实例")]
    [LuaApiParamDescription("name", "名称")]
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
    [LuaApiDescription("销毁一个UI消息中心", "返回是否成功")]
    [LuaApiParamDescription("name", "名称")]
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
          case "window":
            {
              switch (args[1])
              {
                case "all":
                  StringBuilder sb = new StringBuilder("All window: {0}");
                  sb.Append(managedWindows.Count);
                  sb.Append("\n");
                  sb.AppendLine("Id Title WindowState WindowType");
                  foreach (var w in managedWindows)
                    sb.AppendLine(string.Format("{0} => {1} > {2} {3}",
                        w.Value.windowId, w.Value.Title, w.Value.WindowState, w.Value.WindowType));
                  Log.V(TAG, sb.ToString());
                  break;
                case "current":
                  Log.V(TAG, "Current window: {0}", currentActiveWindow == null ? "null" : currentActiveWindow.windowId.ToString());
                  break;
                default:
                  if (int.TryParse(args[1], out int windowId))
                  {

                    string act = "";
                    if (!DebugUtils.CheckDebugParam(2, args, out act)) break;
                    Window w = FindWindowById(windowId);
                    if (w == null)
                    {
                      Log.E(TAG, "未找到指定窗口id", windowId);
                      break;
                    }

                    switch (act)
                    {
                      case "show":
                        ShowWindow(w);
                        Log.V(TAG, "OK");
                        break;
                      case "hide":
                        HideWindow(w);
                        Log.V(TAG, "OK");
                        break;
                      case "close":
                        CloseWindow(w);
                        Log.V(TAG, "OK");
                        break;
                      case "active":
                        ActiveWindow(w);
                        Log.V(TAG, "OK");
                        break;
                      default:
                        Log.E(TAG, "参数 [3] 不是有效的操作类型", act);
                        break;
                    }
                  }
                  else Log.E(TAG, "参数 [2] 不是有效的窗口id", args[1]);
                  break;
              }
              break;
            }
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
          case "alert":
            {
              string text;
              string title;
              if (!DebugUtils.CheckDebugParam(1, args, out text)) return false;
              if (!DebugUtils.CheckDebugParam(2, args, out title)) return false;
              GlobalAlertWindow(text, title, null);
              return true;
            }
        }
        return false;
      }, 2, "um <window/page/toast/alert>\n" +
              "  window\n" +
              "    all 显示所有受管理的窗口\n" +
              "    <windowId:number>\n" +
              "      show 显示指定窗口\n" +
              "      hide 隐藏指定窗口\n" +
              "      close 关闭指定窗口\n" +
              "      active 激活指定窗口\n" +
              "  page\n" +
              "    current 显示当前显示的页\n" +
              "    stack 显示当前显示页的栈\n" +
              "    go <name:string> 跳转到指定页\n" +
              "    hide-cur 隐藏当前页\n" +
              "    close-all 关闭所有显示的页\n" +
              "    back 返回上一页\n" +
              "  toast <text:string> [showSecond:number] 测试全局弹出土司提示\n" +
              "  alert <text:string> <title:string> 测试全局弹出提示窗口"
      );
    }

    #endregion
  }

  /// <summary>
  /// UIPrefab的类型
  /// </summary>
  [SLua.CustomLuaClass]
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

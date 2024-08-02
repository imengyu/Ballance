using UnityEngine;
using UnityEngine.UI;
using Ballance2.Services;
using static Ballance2.Services.GameManager;
using Ballance2.Package;
using Ballance2.Utils;
using Ballance2.Services.Debug;
using Ballance2.UI.Utils;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using SubjectNerd.Utilities;
using System.Xml.Serialization;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameUIPage.cs
* 
* 用途：
* UI页实例
* 页与窗口不太一样，窗口可以同时打开多个，页只能同时显示一个，相当于独占的全屏窗口。
* 要创建页，可以调用 GameUIManager.RegisterPage 函数。
* 
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core
{
  /// <summary>
  /// UI页实例
  /// </summary>
  public class GameUIPage : MonoBehaviour
  {
    public RectTransform Content;
    public RectTransform ContentHost;
    public VerticalLayoutGroup VerticalLayoutGroup;
    public HorizontalLayoutGroup HorizontalLayoutGroup;

    /// <summary>
    /// 返回上一页的按钮
    /// </summary>
    [Tooltip("返回上一页的按钮")]
    public InputAction BackAction;
    /// <summary>
    /// 页名称
    /// </summary>
    [Tooltip("页名称")]
    public string PageName;
    /// <summary>
    /// 控制器与键盘快捷键UI显示器
    /// </summary>
    public UIKeyButtons KeyButtons;
    /// <summary>
    /// 设定用户是否可以返回上一页
    /// </summary>
    public bool CanBack = true;
    /// <summary>
    /// 设定是否显示默认控制器控制按钮
    /// </summary>
    public bool ShowStaticDisplayAction = true;
    /// <summary>
    /// 设定是否在无控制器时依然显示控制按钮
    /// </summary>
    public bool ForceShowDisplayAction = false;
    public bool DelayShowDisplayAction = false;
    /// <summary>
    /// 设定是否不隐藏上一页显示
    /// </summary>
    public bool NoPopPreviousPage = false;
    /// <summary>
    /// 显示页事件
    /// </summary>
    public OptionsDelegate OnShow;
    /// <summary>
    /// 页由上一页返回时事件
    /// </summary>
    public OptionsDelegate OnBackFromChild;
    /// <summary>
    /// 隐藏页事件
    /// </summary>
    public VoidDelegate OnHide;
    [SerializeField]
    [Reorderable]
    public List<UIKeyButtons.StaticDisplayAction> StaticDisplayActions = new List<UIKeyButtons.StaticDisplayAction>();

    private DefaultSelection defaultSelection = null;

    /// <summary>
    /// 页面上一次打开时所设置的参数
    /// </summary>
    public Dictionary<string, string> LastOptions;
    /// <summary>
    /// 页面上由子页所返回设置的参数
    /// </summary>
    public Dictionary<string, string> LastBackOptions;

    public delegate void OptionsDelegate(Dictionary<string, string> options);

    private void Start() 
    {
      BackAction.performed += (e) => DoBack();
    }

    private void DoBack()
    {
      if (CanBack)
        GameUIManager.Instance.BackPreviusPage();
    }

    public void ShowKeyButtons()
    {
      KeyButtons.gameObject.SetActive(true);
    }
    public void HideKeyButtons()
    {
      KeyButtons.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示页。直接调用此函数会导致脱离 GameUIManager 的栈管理，推荐使用 GameUIManager 来控制页的显示与隐藏。
    /// </summary>
    internal void Show()
    {
      gameObject.SetActive(true);
      OnShow?.Invoke(LastOptions);

#if UNITY_EDITOR || UNITY_STANDALONE
      //选择默认按扭
      if(defaultSelection != null) {
        //默认选择的时候不要播放声音
        var sound = defaultSelection.select.GetComponent<ClickSound>();
        if(sound != null)
          sound.enabled = false;
        EventSystem.current.SetSelectedGameObject(defaultSelection.select);
        if(sound != null)
          sound.enabled = true;
      }
#endif
      //接入控制器后显示按键快捷键
      if (ForceShowDisplayAction || Gamepad.all.Count > 0)
      {
        if (CanBack && BackAction.bindings.Count > 0)
          KeyButtons.AddDisplayActions(BackAction, "I18N:core.ui.Back", () => DoBack());
        if (ShowStaticDisplayAction)
        {
          var staticActions = Content.GetComponent<StaticDisplayActionSetter>();
          var finalStaticDisplayActions = new List<UIKeyButtons.StaticDisplayAction>();
          finalStaticDisplayActions.AddRange(StaticDisplayActions);
          if (staticActions != null && staticActions.StaticDisplayActions != null)
          {
            if (staticActions.Override)
              finalStaticDisplayActions = staticActions.StaticDisplayActions;
            else
              finalStaticDisplayActions.AddRange(staticActions.StaticDisplayActions);
          }
            foreach (var item in finalStaticDisplayActions)
              KeyButtons.AddStaticDisplayActions(item);
        }
        GameManager.Instance.Delay(0.2f, () => {
          if (KeyButtons != null)
          {
            KeyButtons.EnableAllDisplayActions();
            if (!DelayShowDisplayAction)
              ShowKeyButtons();
          }
          if (CanBack && BackAction.bindings.Count > 0)
            BackAction.Enable();
        });
      }
    }

    /// <summary>
    /// 隐藏页。直接调用此函数会导致脱离 GameUIManager 的栈管理，推荐使用 GameUIManager 来控制页的显示与隐藏。
    /// </summary>
    internal void Hide()
    {
      gameObject.SetActive(false);
      OnHide?.Invoke();
      BackAction.Disable();
      if (KeyButtons != null) {
        KeyButtons.DeleteAllDisplayActions();
        HideKeyButtons();
      }
    }

    /// <summary>
    /// 创建指定资源的内容
    /// </summary>
    /// <param name="package">资源所在模块</param>
    /// <param name="resourceName">资源模板路径</param>
    public void CreateContent(GamePackage package, string resourceName)
    {
      var go = CloneUtils.CloneNewObjectWithParent(package.GetPrefabAsset(resourceName), ContentHost, PageName);
      if (go == null)
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.PrefabNotFound, "GameUIPage", "Can not create page content for {0}, The prefab {1} not found", PageName, resourceName);
        return;
      }
      var content = go.GetComponent<RectTransform>();
      if (content != null)
      {
        Content = content;
        UIAnchorPosUtils.SetUIAnchor(content, UIAnchor.Stretch, UIAnchor.Stretch);
        UIAnchorPosUtils.SetUIPos(content, 0, 0, 0, 0);

        defaultSelection = content.GetComponent<DefaultSelection>();
      }
    }

    /// <summary>
    /// 使用页名称自动创建指定资源的内容
    /// </summary>
    /// <param name="package">资源所在模块</param>
    public void CreateContent(GamePackage package)
    {
      CreateContent(package, PageName + ".prefab");
    }
    /// <summary>
    /// 使用页预制体自动创建指定资源的内容
    /// </summary>
    /// <param name="prefab">预制体</param>
    public void CreateContent(GameObject prefab)
    {
      var go = CloneUtils.CloneNewObjectWithParent(prefab, ContentHost, PageName);
      var content = go.GetComponent<RectTransform>();
      if (content != null)
      {
        Content = content;
        UIAnchorPosUtils.SetUIAnchor(content, UIAnchor.Stretch, UIAnchor.Stretch);
        UIAnchorPosUtils.SetUIPos(content, 0, 0, 0, 0);
      }
    }
    /// <summary>
    /// 将内容设置至当前页的内容容器中
    /// </summary>
    /// <param name="content">内容RectTransform</param>
    public void SetContent(RectTransform content)
    {
      content.transform.SetParent(ContentHost);
      UIAnchorPosUtils.SetUIAnchor(content, UIAnchor.Stretch, UIAnchor.Stretch);
      UIAnchorPosUtils.SetUIPos(content, 0, 0, 0, 0);
    }
  }
}

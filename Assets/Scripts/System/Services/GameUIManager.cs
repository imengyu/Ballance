using Ballance2.System.Bridge;
using Ballance2.System.Package;
using Ballance2.System.Res;
using Ballance2.System.UI;
using Ballance2.System.UI.Parts;
using Ballance2.System.UI.Utils;
using Ballance2.System.Utils;
using Ballance2.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.System.Services
{
    /// <summary>
    /// UI 管理器
    /// </summary>
    [SLua.CustomLuaClass]
    public class GameUIManager : GameService
    {
        private static readonly string TAG = "GameUIManager";

        public GameUIManager() : base(TAG) {}

        [SLua.DoNotToLua]
        public override void Destroy()
        {
            Object.Destroy(uiManagerGameObject);
            DestroyWindowManagement();

            Log.D(TAG, "Destroy {0} ui objects", UIRoot.transform.childCount);
            for (int i = 0, c = UIRoot.transform.childCount; i < c; i++)
                Object.Destroy(UIRoot.transform.GetChild(i).gameObject);
        }
        [SLua.DoNotToLua]
        public override bool Initialize()
        {
            //等待基础加载完成
            GameManager.GameMediator.RegisterEventHandler(GamePackage.GetSystemPackage(),
                GameEventNames.EVENT_BASE_INIT_FINISHED, TAG, (evtName, param) =>
                {
                    UIRoot = GameManager.Instance.GameCanvas;
                    UIFadeManager = UIRoot.gameObject.AddComponent<UIFadeManager>();
                    GlobalFadeMaskWhite = UIRoot.transform.Find("GlobalFadeMaskWhite").gameObject.GetComponent<Image>();
                    GlobalFadeMaskBlack = UIRoot.transform.Find("GlobalFadeMaskBlack").gameObject.GetComponent<Image>();
                    GlobalFadeMaskBlack.gameObject.SetActive(true);

                    //Add object
                    GameObject uiManagerGameObject = CloneUtils.CreateEmptyObjectWithParent(UIRoot.transform, "GameUIManager");
                    GameUIManagerObject gameUIManagerObject = uiManagerGameObject.AddComponent<GameUIManagerObject>();
                    gameUIManagerObject.GameUIManagerUpdateDelegate = Update;

                    //Init all
                    InitAllObects();
                    InitWindowManagement();
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
        public RectTransform UIRoot;
        /// <summary>
        /// 渐变管理器
        /// </summary>
        public UIFadeManager UIFadeManager;

        private GameObject uiManagerGameObject = null;

        private void Update()
        {
            UpdateToastShow();
        }

        //根管理
        private RectTransform TemporarilyRectTransform;
        private RectTransform GlobalWindowRectTransform;
        private RectTransform PagesRectTransform;
        private RectTransform WindowsRectTransform;
        private RectTransform ViewsRectTransform;

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
            WindowsRectTransform = CloneUtils.CreateEmptyUIObjectWithParent(UIRoot.transform, "GameUIWindow").GetComponent<RectTransform>();
            ViewsRectTransform = CloneUtils.CreateEmptyUIObjectWithParent(UIRoot.transform, "ViewsRectTransform").GetComponent<RectTransform>();
            UIAnchorPosUtils.SetUIAnchor(ViewsRectTransform, UIAnchor.Stretch, UIAnchor.Stretch);
            UIAnchorPosUtils.SetUIPos(ViewsRectTransform, 0, 0, 0, 0);
            UIAnchorPosUtils.SetUIAnchor(PagesRectTransform, UIAnchor.Stretch, UIAnchor.Stretch);
            UIAnchorPosUtils.SetUIPos(PagesRectTransform, 0, 0, 0, 0);

            UIToast = CloneUtils.CloneNewObjectWithParent(GameStaticResourcesPool.FindStaticPrefabs("PrefabToast"), UIRoot.transform, "GlobalUIToast").GetComponent<RectTransform>();
            UIToastImage = UIToast.GetComponent<Image>();
            UIToastText = UIToast.Find("Text").GetComponent<Text>();
            UIToast.gameObject.SetActive(false);
            UIToast.SetAsLastSibling();
            EventTriggerListener.Get(UIToast.gameObject).onClick = (g) => { toastTimeTick = 1; };
        }

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
        /// <returns></returns>
        public int GlobalAlertWindow(string text, string title, string okText = "确定")
        {
            GameObject windowGo = CloneUtils.CloneNewObjectWithParent(PrefabUIAlertWindow, WindowsRectTransform.transform, "");
            RectTransform rectTransform = windowGo.GetComponent<RectTransform>();
            Button btnOk = rectTransform.Find("Button").GetComponent<Button>();
            rectTransform.Find("DialogText").GetComponent<Text>().text = text;
            rectTransform.Find("Button/Text").GetComponent<Text>().text = okText;
            Window window = CreateWindow(title, rectTransform);
            window.WindowType = WindowType.GlobalAlert;
            window.CanClose = true;
            window.CanDrag = true;
            window.CanResize = true;
            window.CanMax = false;
            window.CanMin = false;
            window.MinSize = new Vector2(300, 250);
            window.Show();
            btnOk.onClick.AddListener(() => {
                window.Close();
            });
            window.onClose += (id) =>
            {
                PagesRectTransform.gameObject.SetActive(true);
                WindowsRectTransform.gameObject.SetActive(true);
                GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_GLOBAL_ALERT_CLOSE, "*",
                    id, false);
            };
            return window.GetWindowId();
        }
        /// <summary>
        /// 显示全局 Confirm 对话框（窗口模式）
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="title">标题</param>
        /// <param name="okText">OK 按钮文字</param>
        /// <param name="cancelText">Cancel 按钮文字</param>
        /// <returns></returns>
        public int GlobalConfirmWindow(string text, string title, string okText = "确定", string cancelText = "取消")
        {
            GameObject windowGo = CloneUtils.CloneNewObjectWithParent(PrefabUIConfirmWindow, WindowsRectTransform.transform, "");
            RectTransform rectTransform = windowGo.GetComponent<RectTransform>();
            Button btnYes = rectTransform.Find("ButtonConfirm").GetComponent<Button>();
            Button btnNo = rectTransform.Find("ButtonCancel").GetComponent<Button>();
            rectTransform.Find("ButtonConfirm/Text").GetComponent<Text>().text = okText;
            rectTransform.Find("ButtonCancel/Text").GetComponent<Text>().text = cancelText;
            rectTransform.Find("DialogText").GetComponent<Text>().text = text;
            Window window = CreateWindow(title, rectTransform);
            window.WindowType = WindowType.GlobalAlert;
            window.CanClose = false;
            window.CanDrag = true;
            window.CanResize = false;
            window.CanMax = false;
            window.CanMin = false;
            window.Show();
            window.onClose += (id) =>
            {
                PagesRectTransform.gameObject.SetActive(true);
                WindowsRectTransform.gameObject.SetActive(true);
            };
            btnYes.onClick.AddListener(() =>
            {
                window.Close();
                GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_GLOBAL_ALERT_CLOSE, "*",
                    window.GetWindowId(), true);
            });
            btnNo.onClick.AddListener(() => {
                window.Close();
                GameManager.GameMediator.DispatchGlobalEvent(GameEventNames.EVENT_GLOBAL_ALERT_CLOSE, "*",
                    window.GetWindowId(), false);
            });
            return window.GetWindowId();
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
            if(returnWindowIds.Count > 0)
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

        private void InitWindowManagement()
        {
            managedWindows = new Dictionary<int, Window>();

            PrefabUIAlertWindow = GameStaticResourcesPool.FindStaticPrefabs("PrefabAlertWindow");
            PrefabUIConfirmWindow = GameStaticResourcesPool.FindStaticPrefabs("PrefabConfirmWindow");
            PrefabUIWindow = GameStaticResourcesPool.FindStaticPrefabs("PrefabWindow");

            GameManager.GameMediator.RegisterGlobalEvent(GameEventNames.EVENT_GLOBAL_ALERT_CLOSE);
        }
        private void DestroyWindowManagement()
        {
            GameManager.GameMediator.UnRegisterGlobalEvent(GameEventNames.EVENT_GLOBAL_ALERT_CLOSE);

            if (managedWindows != null)
            {
                foreach (var w in managedWindows)
                    w.Value.Destroy();
                managedWindows.Clear();
                managedWindows = null;
            }
        }

        //窗口

        private Dictionary<int, Window> managedWindows = null;

        /// <summary>
        /// 创建自定义窗口（默认不显示）
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="customView">窗口自定义View</param>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
        public Window CreateWindow(string title, RectTransform customView, bool show, float x, float y, float w, float h)
        {
            GameObject windowGo = CloneUtils.CloneNewObjectWithParent(PrefabUIWindow, WindowsRectTransform.transform);
            Window window = windowGo.GetComponent<Window>();
            window.Title = title;
            window.Position = new Vector2(x, y);
            window.SetView(customView);
            if (w != 0 && h != 0) window.Size = new Vector2(w, h);
            if (show) window.Show();
            RegisterWindow(window);
            window.MoveToCenter();
            window.onClose += (id) =>
            {
                ReturnWindowId(id);
                window.Destroy();
                managedWindows.Remove(window.GetWindowId());
            };
            return window;
        }
        /// <summary>
        /// 注册窗口到管理器中
        /// </summary>
        /// <param name="window">窗口</param>
        /// <returns></returns>
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
        /// <returns></returns>
        public Window FindWindowById(int windowId)
        {
            managedWindows.TryGetValue(windowId, out Window w);
            return w;
        }

        private Window currentVisibleWindowAlert = null;

        public void ShowWindow(Window window)
        {
            switch (window.WindowType)
            {
                case WindowType.GlobalAlert:
                    window.GetRectTransform().transform.SetParent(GlobalWindowRectTransform.transform);
                    PagesRectTransform.gameObject.SetActive(false);
                    WindowsRectTransform.gameObject.SetActive(false);
                    WindowsRectTransform.SetAsLastSibling();
                    currentVisibleWindowAlert = window;
                    break;
                case WindowType.Normal:
                    window.GetRectTransform().transform.SetParent(WindowsRectTransform.transform);
                    WindowsRectTransform.SetAsLastSibling();
                    break;
            }
            window.SetVisible(true);
        }
        public void HideWindow(Window window) { window.Hide(); }
        public void CloseWindow(Window window) { 
            window.Close(); 
        }


        #endregion

        #region 全局渐变遮罩

        private Image GlobalFadeMaskWhite;
        private Image GlobalFadeMaskBlack;

        /// <summary>
        /// 全局黑色遮罩隐藏
        /// </summary>
        public void MaskBlackSet(bool show)
        {
            GlobalFadeMaskBlack.color = new Color(GlobalFadeMaskBlack.color.r,
                   GlobalFadeMaskBlack.color.g, GlobalFadeMaskBlack.color.b, show ? 1.0f : 0f);
            GlobalFadeMaskBlack.gameObject.SetActive(show);
            GlobalFadeMaskBlack.transform.SetAsLastSibling();
        }
        /// <summary>
        /// 全局黑色遮罩隐藏
        /// </summary>
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

        public void SetViewToTemporarily(RectTransform view)
        {
            view.SetParent(TemporarilyRectTransform.gameObject.transform);
        }
        public void AttatchViewToCanvas(RectTransform view)
        {
            view.SetParent(UIRoot.gameObject.transform);
        }
        public RectTransform InitViewToCanvas(GameObject prefab, string name)
        {
            GameObject go = CloneUtils.CloneNewObjectWithParent(prefab,
                ViewsRectTransform.transform, name);
            RectTransform view = go.GetComponent<RectTransform>();
            view.SetParent(ViewsRectTransform.gameObject.transform);
            return view;
        }

        #endregion
    }
}

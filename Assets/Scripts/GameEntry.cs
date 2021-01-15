using Ballance2.Config;
using Ballance2.Game;
using Ballance2.System;
using Ballance2.System.Res;
using Ballance2.System.UI.Utils;
using Ballance2.UI.Parts;
using SubjectNerd.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameEntry.cs
* 
* 用途：
* 整个游戏的入口
* 以及用户协议对话框的显示
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-14 创建
*
*/

namespace Ballance2
{
    class GameEntry : MonoBehaviour
    {
        /// <summary>
        /// 启动时暂停游戏，在控制台中继续（通常用于调试）
        /// </summary>
        public bool BreakAtStart = false;
        /// <summary>
        /// 目标帧率
        /// </summary>
        public int TargetFrameRate = 60;
        /// <summary>
        /// 是否设置帧率
        /// </summary>
        public bool SetFrameRate = true;

        /// <summary>
        /// 静态 Prefab 资源引入
        /// </summary>
        [Reorderable("GamePrefab", true, "Name")]
        public List<GameObjectInfo> GamePrefab = null;
        /// <summary>
        /// 静态资源引入
        /// </summary>
        [Reorderable("GameAssets", true, "Name")]
        public List<GameAssetsInfo> GameAssets = null;
        public Camera GameBaseCamera = null;
        public RectTransform GameCanvas = null;

        public GameGlobalErrorUI GameGlobalErrorUI = null;
        public GameObject GlobalGamePermissionTipDialog = null;
        public GameObject GlobalGameUserAgreementTipDialog = null;
        public Button ButtonUserAgreementAllow = null;
        public Toggle CheckBoxAllowUserAgreement = null;
        public GameObject LinkPrivacyPolicy = null;
        public GameObject LinkUserAgreement = null;

        private bool GlobalGamePermissionTipDialogClosed = false;
        private bool GlobalGameUserAgreementTipDialogClosed = false;

        void Start()
        {
            if (SetFrameRate) Application.targetFrameRate = TargetFrameRate;

            InitCommandLine();
            InitUI();

            StartCoroutine(InitMain());
        }
        private void OnDestroy()
        {
            GameSystem.Destroy();
        }

        /// <summary>
        /// 显示许可对话框
        /// </summary>
        /// <returns></returns>
        private bool ShowUserArgeement()
        {
            if (PlayerPrefs.GetInt("UserAgreementAgreed", 0) != 0)
            {
                GlobalGameUserAgreementTipDialog.SetActive(true);
                GlobalGameUserAgreementTipDialog.GetComponent<Animator>().Play("GlobalTipDialogShowAnimation");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 检查android权限是否申请
        /// </summary>
        /// <returns></returns>
        private bool TestAndroidPermission()
        {
#if UNITY_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
                return true;
#endif
            return false;
        }

        /// <summary>
        /// 用户同意许可
        /// </summary>
        public void ArgeedUserArgeement()
        {
            PlayerPrefs.SetInt("UserAgreementAgreed", 1);
            GlobalGameUserAgreementTipDialogClosed = true;
            GlobalGameUserAgreementTipDialog.SetActive(false);
        }
        /// <summary>
        /// 同意选择框改变
        /// </summary>
        /// <param name=""></param>
        public void ArgeedUserArgeementChackChinged(bool check)
        {
            ButtonUserAgreementAllow.interactable = check;
        }
        /// <summary>
        /// 请求安卓权限
        /// </summary>
        public void RequestAndroidPermission()
        {
#if UNITY_ANDROID
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
#endif
            GlobalGamePermissionTipDialogClosed = true;
            GlobalGamePermissionTipDialog.SetActive(false);
        }
        /// <summary>
        /// 退出游戏
        /// </summary>
        public void QuitGame()
        {
            StopAllCoroutines();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// 初始化命令行
        /// </summary>
        private void InitCommandLine()
        {
            string[] CommandLineArgs = Environment.GetCommandLineArgs();
            int len = CommandLineArgs.Length;
            if (len > 1)
            {
                for (int i = 0; i < len; i++)
                {
                    if (CommandLineArgs[i] == "-debug")
                        PlayerPrefs.SetString("core.debug", "true");
                }
            }
        }
        private void InitUI()
        {
            CheckBoxAllowUserAgreement.onValueChanged.AddListener(ArgeedUserArgeementChackChinged);

            EventTriggerListener.Get(LinkPrivacyPolicy).onClick += (go) =>
                Application.OpenURL(GameConst.BallancePrivacyPolicy);
            EventTriggerListener.Get(LinkUserAgreement).onClick += (go) =>
                Application.OpenURL(GameConst.BallanceUserAgreement);
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitMain()
        {
            if (TestAndroidPermission())
            {
                GlobalGamePermissionTipDialog.SetActive(true);
                GlobalGamePermissionTipDialog.GetComponent<Animator>().Play("GlobalTipDialogShowAnimation");

                yield return new WaitUntil(() => GlobalGamePermissionTipDialogClosed);
            }
            if (ShowUserArgeement())
            {
                yield return new WaitUntil(() => GlobalGameUserAgreementTipDialogClosed);
            }

            GameErrorChecker.SetGameErrorUI(GameGlobalErrorUI);
            GameInit.FillStartParameters(this);
            //GameSystem.RegSysHandler(GameInit.GetSysHandler());
            GameSystem.Init();
        }
    }
}

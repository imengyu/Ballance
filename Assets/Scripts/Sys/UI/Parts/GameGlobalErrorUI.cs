using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameGlobalErrorUI.cs
* 
* 用途：
* 全局错误UI逻辑控制
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-1 创建
*
*/

namespace Ballance2.UI.Parts
{
    class GameGlobalErrorUI : MonoBehaviour
    {
        public Text TextGameErrorTitle = null;
        public Text TextGameErrorHelp = null;
        public Text TextGameErrorText = null;
        public Button ButtonShowErrorContent = null;
        public Button ButtonHideErrorContent = null;
        public GameObject ScrollViewErrorContent = null;
        public Button ButtonCloseGame = null;
        public Button ButtonRestartGame = null;

        private void Start()
        {
            ButtonShowErrorContent.onClick.AddListener(() =>
            {
                ButtonShowErrorContent.gameObject.SetActive(false);
                ButtonHideErrorContent.gameObject.SetActive(true);
                TextGameErrorHelp.gameObject.SetActive(false);
                ScrollViewErrorContent.gameObject.SetActive(true);
            });
            ButtonHideErrorContent.onClick.AddListener(() =>
            {
                ButtonShowErrorContent.gameObject.SetActive(true);
                ButtonHideErrorContent.gameObject.SetActive(false);
                TextGameErrorHelp.gameObject.SetActive(true);
                ScrollViewErrorContent.gameObject.SetActive(false);
            });
        }

        public void ShowErrorUI(string err)
        {
            gameObject.SetActive(true);
            TextGameErrorText.text = err;
        }
    }
}

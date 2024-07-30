using Ballance2;
using Ballance2.UI.Core.Controls;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2022 mengyu
*
* 模块名：     
* GlobalGameScriptErrDialog.cs
* 
* 用途：
* 全局游戏脚本错误的对话框逻辑。
*
* 作者：
* mengyu
*/

public class GlobalGameScriptErrDialog : MonoBehaviour
{
  public TMP_Text GlobalGameScriptErrContent;
  public Button GlobalGameScriptErrQuitButton;
  public Button GlobalGameScriptErrContinueButton;

  private void Start() {
    GlobalGameScriptErrQuitButton.onClick.AddListener(() => {
      GameSystem.QuitPlayer();
    });
    GlobalGameScriptErrContinueButton.onClick.AddListener(() => {
      gameObject.SetActive(false);
    });
  }
  public void Show(string s) {
    GlobalGameScriptErrContent.text = s;
    gameObject.SetActive(true);
  }
}

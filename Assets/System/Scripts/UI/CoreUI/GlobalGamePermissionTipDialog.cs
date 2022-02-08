using Ballance2.Entry;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2022 mengyu
*
* 模块名：     
* GlobalGamePermissionTipDialog.cs
* 
* 用途：
* 全局游戏开始时请求权限的对话框逻辑。
*
* 作者：
* mengyu
*/

public class GlobalGamePermissionTipDialog : MonoBehaviour {
  public Button ButtonAllow;
  public Button ButtonDisallow;

  void Start() {
    ButtonAllow.onClick.AddListener(() => {
      gameObject.SetActive(false);
      GameEntry.Instance.RequestAndroidPermission();
    });
    ButtonDisallow.onClick.AddListener(() => {
      gameObject.SetActive(false);
      GameEntry.Instance.DisallowAndroidPermission();
    });
  }
}
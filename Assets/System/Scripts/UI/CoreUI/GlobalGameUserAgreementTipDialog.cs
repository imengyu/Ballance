using Ballance2.Config;
using Ballance2.Entry;
using Ballance2.Services.InputManager;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2022 mengyu
*
* 模块名：     
* GlobalGameUserAgreementTipDialog.cs
* 
* 用途：
* 全局开始时的用户协议的对话框逻辑。
*
* 作者：
* mengyu
*/

public class GlobalGameUserAgreementTipDialog : MonoBehaviour {
  public Button ButtonUserAgreementAllow = null;
  public Button ButtonUserAgreementDisallow = null;
  public Toggle CheckBoxAllowUserAgreement = null;
  public GameObject LinkPrivacyPolicy = null;
  public GameObject LinkUserAgreement = null;

  void Start() {
    EventTriggerListener.Get(LinkPrivacyPolicy).onClick += (go) => Application.OpenURL(ConstStrings.BallancePrivacyPolicy);
    EventTriggerListener.Get(LinkUserAgreement).onClick += (go) => Application.OpenURL(ConstStrings.BallanceUserAgreement);
    CheckBoxAllowUserAgreement.onValueChanged.AddListener((v) => ButtonUserAgreementAllow.interactable =  v);
    ButtonUserAgreementAllow.onClick.AddListener(() => {
      gameObject.SetActive(false);
      GameEntry.Instance.ArgeedUserArgeement();
    });
    ButtonUserAgreementDisallow.onClick.AddListener(() => GameEntry.Instance.QuitGame());
  }

}
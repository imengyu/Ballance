using Ballance2.Config;
using Ballance2.Entry;
using Ballance2.Services.InputManager;
using UnityEngine;
using UnityEngine.UI;

public class GlobalGameUserAgreementTipDialog : MonoBehaviour {
  public Button ButtonUserAgreementAllow = null;
  public Button ButtonUserAgreementDisallow = null;
  public Toggle CheckBoxAllowUserAgreement = null;
  public GameObject LinkPrivacyPolicy = null;
  public GameObject LinkUserAgreement = null;

  void Start() {
    EventTriggerListener.Get(LinkPrivacyPolicy).onClick += (go) => Application.OpenURL(GameConst.BallancePrivacyPolicy);
    EventTriggerListener.Get(LinkUserAgreement).onClick += (go) => Application.OpenURL(GameConst.BallanceUserAgreement);
    CheckBoxAllowUserAgreement.onValueChanged.AddListener((v) => ButtonUserAgreementAllow.interactable =  v);
    ButtonUserAgreementDisallow.onClick.AddListener(() => {
      gameObject.SetActive(false);
      GameEntry.Instance.ArgeedUserArgeement();
    });
    ButtonUserAgreementDisallow.onClick.AddListener(() => GameEntry.Instance.QuitGame());
  }

}
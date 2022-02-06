using Ballance2.Entry;
using UnityEngine;
using UnityEngine.UI;

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
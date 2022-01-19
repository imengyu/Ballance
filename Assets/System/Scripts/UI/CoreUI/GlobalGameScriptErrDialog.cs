using Ballance2;
using UnityEngine;
using UnityEngine.UI;

public class GlobalGameScriptErrDialog : MonoBehaviour
{
  public Text GlobalGameScriptErrContent;
  public Button GlobalGameScriptErrQuitButton;

  private void Start() {
    GlobalGameScriptErrQuitButton.onClick.AddListener(() => {
      GameSystem.QuitPlayer();
    });
  }
  public void Show(string s) {
    GlobalGameScriptErrContent.text = s;
    gameObject.SetActive(true);
  }
}

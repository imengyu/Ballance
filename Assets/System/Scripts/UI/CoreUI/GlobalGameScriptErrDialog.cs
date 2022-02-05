using Ballance2;
using UnityEngine;
using UnityEngine.UI;

public class GlobalGameScriptErrDialog : MonoBehaviour
{
  public Text GlobalGameScriptErrContent;
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

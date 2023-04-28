using Ballance2.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  public class KeypadActionButtons : MonoBehaviour {
    public Image ButtonUp;
    public Image ButtonDown;
    public Image ButtonCamLeft;
    public Image ButtonCamRight;
    public Image ButtonSpace;

    private void Start() {
      InitKeys();
    }
    private void OnDestroy()
    {
      GameManager.Instance.GameSettings.UnRegisterSettingsUpdateCallback(settingsUpdateCallbackId);
    }

    //键盘按键大小
    private int settingsUpdateCallbackId = 0;
    private float keySize = 80;

    private void InitKeys() {
      var GameSettings = GameManager.Instance.GameSettings;
      settingsUpdateCallbackId = GameSettings.RegisterSettingsUpdateCallback("control", (groupName, action) => {
        SetKeysSize(GameSettings.GetFloat("control.key.size", keySize));
        return false;
      });
      SetKeysSize(GameSettings.GetFloat("control.key.size", keySize));
    }
    private void SetKeysSize(float newKeySize) {
      if (keySize != newKeySize) {
        keySize = newKeySize;
        (transform as RectTransform).sizeDelta = new Vector2(keySize * 3, keySize * 2);
        ButtonSpace.rectTransform.anchoredPosition = new Vector2(keySize * 0.5f, 0);
        ButtonSpace.rectTransform.sizeDelta = new Vector2(keySize, keySize);
        ButtonCamLeft.rectTransform.anchoredPosition = new Vector2(0, -keySize);
        ButtonCamLeft.rectTransform.sizeDelta = new Vector2(keySize, keySize);
        ButtonCamRight.rectTransform.anchoredPosition = new Vector2(keySize, -keySize);
        ButtonCamRight.rectTransform.sizeDelta = new Vector2(keySize, keySize);
        ButtonUp.rectTransform.anchoredPosition = new Vector2(keySize * 2, 0);
        ButtonUp.rectTransform.sizeDelta = new Vector2(keySize, keySize);
        ButtonDown.rectTransform.anchoredPosition = new Vector2(keySize * 2, -keySize);
        ButtonDown.rectTransform.sizeDelta = new Vector2(keySize, keySize);
      }
    }
  
  
  }
}
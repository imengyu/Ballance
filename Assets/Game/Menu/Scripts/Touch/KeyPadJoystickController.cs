using Ballance2.Game;
using Ballance2.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Menu.Touch
{
  public enum KeyPadJoystickDirection {
    None = 0,
    Left = 1,
    Right = 2,
    Forward = 4,
    Back = 8,
    LeftForward = Left | Forward,
    LeftBack = Left | Back,
    RightForward = Right | Forward,
    RightBack = Right | Back,
  }

  public delegate void KeyPadJoystickDirectionChanged(bool stat, KeyPadJoystickDirection dir);
  public delegate void KeyPadJoystickValueChanged(float valX, float valY);
  
  /// <summary>
  /// 模拟摇杆UI控制
  /// </summary>
  [RequireComponent(typeof(SimpleTouchController))]
  public class KeyPadJoystickController : MonoBehaviour
  {
    public KeyPadJoystickValueChanged ValueChanged;
    public Image JoystickImage;
    public Image Joystick;

    public float SpaceSize = 0.2f;

    private SimpleTouchController controller;
    private KeyPadJoystickDirection currentState;
    private bool state = false;

    private void Start()
    {
      InitKeys();
      controller = GetComponent<SimpleTouchController>();
      controller.TouchEvent += (value) => {
        if(state)
          ValueChanged?.Invoke(value.x, value.y);
      };      
      controller.TouchStateEvent += (state) => {
        this.state = state;
        ValueChanged?.Invoke(0, 0);
      };
    }
    private void OnDestroy()
    {
      GameManager.Instance?.GameSettings.UnRegisterSettingsUpdateCallback(settingsUpdateCallbackId);
    }

    private int settingsUpdateCallbackId = 0;
    private float keySize = 80;

    private void InitKeys() {
      var GameSettings = GameManager.Instance.GameSettings;
      settingsUpdateCallbackId = GameSettings.RegisterSettingsUpdateCallback(SettingConstants.SettingsControl, (groupName, action) => {
        SetKeysSize(GameSettings.GetFloat(SettingConstants.SettingsControlKeySize));
        return false;
      });
      SetKeysSize(GameSettings.GetFloat(SettingConstants.SettingsControlKeySize));
    }
    private void SetKeysSize(float newKeySize) {
      if (keySize != newKeySize) {
        keySize = newKeySize;
        Joystick.rectTransform.sizeDelta = new Vector2(keySize * 2.5f, keySize * 2.5f);
        JoystickImage.rectTransform.sizeDelta = new Vector2(keySize, keySize);
      }
    }

  }
}

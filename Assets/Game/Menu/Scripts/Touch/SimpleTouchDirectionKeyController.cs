using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;
using Ballance2.UI.Utils;
using Ballance2.Services;
using Ballance2.Game;

namespace Ballance2.Menu.Touch
{
  /// <summary>
  /// 手机键盘UI控制
  /// </summary>
  [RequireComponent(typeof(KeypadGridLayout))]
  public class SimpleTouchDirectionKeyController : UIBehaviour
  {
    public Image ImageForward;
    public Image ImageForwardLeft;
    public Image ImageForwardRight;
    public Image ImageBack;
    public Image ImageBackLeft;
    public Image ImageBackRight;
    public Image ImageLeft;
    public Image ImageRight;
    public Vector2 SplitCount = new Vector2(3, 3);
    public bool RectInRight = false;
    public Vector2 SplitStart = Vector2.zero;
    public KeyPadJoystickDirectionChanged DirectionChanged;

    private void CallKeyPadJoystickDirectionChanged(bool stat, KeyPadJoystickDirection dir) {
      if(stat)
        currentState |= dir;
      else
        currentState ^= dir;

      ImageForwardLeft.enabled = (currentState & KeyPadJoystickDirection.LeftForward) == KeyPadJoystickDirection.LeftForward;
      ImageForwardRight.enabled = (currentState & KeyPadJoystickDirection.RightForward) == KeyPadJoystickDirection.RightForward;
      ImageBackLeft.enabled = (currentState & KeyPadJoystickDirection.LeftBack) == KeyPadJoystickDirection.LeftBack;
      ImageBackRight.enabled = (currentState & KeyPadJoystickDirection.RightBack) == KeyPadJoystickDirection.RightBack;

      DirectionChanged?.Invoke(stat, dir);
    }
    private void ClearState(KeyPadJoystickDirection without = KeyPadJoystickDirection.None) {
      if((without & KeyPadJoystickDirection.Back) != KeyPadJoystickDirection.Back && (currentState & KeyPadJoystickDirection.Back) != KeyPadJoystickDirection.None) CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Back);
      if((without & KeyPadJoystickDirection.Forward) != KeyPadJoystickDirection.Forward && (currentState & KeyPadJoystickDirection.Forward) != KeyPadJoystickDirection.None) CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Forward);
      if((without & KeyPadJoystickDirection.Left) != KeyPadJoystickDirection.Left && (currentState & KeyPadJoystickDirection.Left) != KeyPadJoystickDirection.None) CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Left);
      if((without & KeyPadJoystickDirection.Right) != KeyPadJoystickDirection.Right && (currentState & KeyPadJoystickDirection.Right) != KeyPadJoystickDirection.None) CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Right);
    }
    private void HandleTouchPos(Vector2 pos) {
      float spx = (rect.width / SplitCount.x);
      float spy = (rect.height / SplitCount.y);
      //9格，直接算出是哪个按扭
      float x = pos.x - rect.x - SplitStart.x * spx;
      float y = pos.y - rect.y - SplitStart.y * spy;
      int xg = (int)Mathf.Floor(x / spx);
      int yg = (int)Mathf.Floor(y / spy);
      switch(yg) {//y 上中下
        case 2: //上
          if(xg == 0) { //x 左右
            ClearState(KeyPadJoystickDirection.LeftForward);
            if((currentState & KeyPadJoystickDirection.Left) == KeyPadJoystickDirection.None)
              CallKeyPadJoystickDirectionChanged(true, KeyPadJoystickDirection.Left);
          }
          else if(xg == 2) {
            ClearState(KeyPadJoystickDirection.RightForward);
            if((currentState & KeyPadJoystickDirection.Right) == KeyPadJoystickDirection.None)
              CallKeyPadJoystickDirectionChanged(true, KeyPadJoystickDirection.Right);
          } else {
            ClearState(KeyPadJoystickDirection.Forward);
          }
          if((currentState & KeyPadJoystickDirection.Forward) == KeyPadJoystickDirection.None)
            CallKeyPadJoystickDirectionChanged(true, KeyPadJoystickDirection.Forward);
          break;
        case 1: //中
          if(xg == 0) { //x 左右
            ClearState(KeyPadJoystickDirection.Left);
            if((currentState & KeyPadJoystickDirection.Left) == KeyPadJoystickDirection.None)
              CallKeyPadJoystickDirectionChanged(true, KeyPadJoystickDirection.Left);
          }
          else if(xg == 2) {
            ClearState(KeyPadJoystickDirection.Right);
            if((currentState & KeyPadJoystickDirection.Right) == KeyPadJoystickDirection.None)
              CallKeyPadJoystickDirectionChanged(true, KeyPadJoystickDirection.Right);
          } else {
            ClearState();
          }
          break;
        case 0: //下
          if(xg == 0) { //x 左右
            ClearState(KeyPadJoystickDirection.LeftBack);
            if((currentState & KeyPadJoystickDirection.Left) == KeyPadJoystickDirection.None)
              CallKeyPadJoystickDirectionChanged(true, KeyPadJoystickDirection.Left);
          }
          else if(xg == 2) {
            ClearState(KeyPadJoystickDirection.RightBack);
            if((currentState & KeyPadJoystickDirection.Right) == KeyPadJoystickDirection.None)
              CallKeyPadJoystickDirectionChanged(true, KeyPadJoystickDirection.Right);
          } else {
            ClearState(KeyPadJoystickDirection.Back);
          }
          if((currentState & KeyPadJoystickDirection.Back) == KeyPadJoystickDirection.None)
            CallKeyPadJoystickDirectionChanged(true, KeyPadJoystickDirection.Back);
          break;
      }
    }

    private KeyPadJoystickDirection currentState;
    private KeypadGridLayout keypadGridLayout;
    private bool state = false;
    private Rect rect = new Rect();
    private Canvas Canvas;
    private int settingsUpdateCallbackId = 0;

    private void InitRect() {
      rect = (transform as RectTransform).rect;
      if (RectInRight)
        rect.x = Screen.width + ((transform as RectTransform).anchoredPosition.x - rect.width) * Canvas.scaleFactor;
      else 
        rect.x = (transform as RectTransform).anchoredPosition.x * Canvas.scaleFactor;
      
      rect.y = (transform as RectTransform).anchoredPosition.y * Canvas.scaleFactor;
      rect.width = rect.width * Canvas.scaleFactor;
      rect.height = rect.height * Canvas.scaleFactor;
    }
    private void InitKeys() {
      var GameSettings = GameManager.Instance.GameSettings;
      settingsUpdateCallbackId = GameSettings.RegisterSettingsUpdateCallback(SettingConstants.SettingsControl, (groupName, action) => {
        SetKeysSize(GameSettings.GetFloat(SettingConstants.SettingsControlKeySize));
        InitRect();
        return false;
      });
      SetKeysSize(GameSettings.GetFloat(SettingConstants.SettingsControlKeySize));
    }
    private void SetKeysSize(float newKeySize) {
      keypadGridLayout.DoLayout(newKeySize);
    }

    protected override void Start()
    {
      keypadGridLayout = GetComponent<KeypadGridLayout>();
      currentState = KeyPadJoystickDirection.None;
      Canvas = GetComponentInParent<Canvas>().rootCanvas;
      InitKeys();
      InitRect();
      base.Start();
    }
    protected override void OnRectTransformDimensionsChange() {
      InitRect();
    }
    protected override void OnDestroy()
    {
      base.OnDestroy();
      GameManager.Instance.GameSettings.UnRegisterSettingsUpdateCallback(settingsUpdateCallbackId);
    }

    private void Update()
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
      for (int i = 0; i < Input.touchCount; i++)
      {
        var touch =  Input.GetTouch(i);
        if(rect.Contains(touch.position)) {
          HandleTouchPos(touch.position);
          state = true;
          return;
        }
      }
      if(state == true) {
        state = false;
        ClearState();
      }
#else
      if(!state) {
        if(Input.GetMouseButtonDown(0)) {
          var pos = Input.mousePosition;
          if(rect.Contains(pos)) {
            HandleTouchPos(pos);
            state = true;
          }
        }
      } else if(state) {
        var pos = Input.mousePosition;
        if(Input.GetMouseButtonUp(0)) {
          state = false;
          ClearState();
        }
        else if(rect.Contains(pos)) 
          HandleTouchPos(pos);
        else 
          ClearState();
      }
#endif
    }
  }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Ballance2.UI.Utils;

namespace Ballance2.Menu
{
  /// <summary>
  /// 手机键盘UI控制
  /// </summary>
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
      //9格，直接算出是哪个按扭
      float x = pos.x - rect.x;
      float y = pos.y - rect.y;
      int xg = (int)Mathf.Floor(x / (rect.width / 3));
      int yg = (int)Mathf.Floor(y / (rect.height / 3));
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
    private bool state = false;
    private Rect rect = new Rect();
    private Canvas Canvas;

    private void InitRect() {
      rect = (transform as RectTransform).rect;
      rect.x = UIAnchorPosUtils.GetUILeft(transform as RectTransform) * Canvas.scaleFactor;
      rect.y = UIAnchorPosUtils.GetUIBottom(transform as RectTransform) * Canvas.scaleFactor;
      rect.width = rect.width * Canvas.scaleFactor;
      rect.height = rect.height * Canvas.scaleFactor;
    }

    protected override void Start()
    {
      currentState = KeyPadJoystickDirection.None;
      Canvas = GetComponentInParent<Canvas>().rootCanvas;
      InitRect();
    }
    protected override void OnRectTransformDimensionsChange() {
      InitRect();
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

using Ballance2.Services;
using Ballance2.Services.InputManager;
using UnityEngine;

namespace Ballance2.Menu
{
  
  public class KeypadUIControl : MonoBehaviour {
    
    private int shiftPressCount = 0;
    //private bool shiftPressOne = false;
    private bool lastCameraSpaceByShift = false;

    private void Start() {
      //自动扫描开头为Button的对象作为按钮，最多扫描2级
      for (int i = 0; i < transform.childCount; i++) {

        var child = transform.GetChild(i);
        if(
          child.gameObject.name.StartsWith("Button") 
          || child.gameObject.name == "Joystick"
          || child.gameObject.name == "DirectionKey"
        ) {
          AddButton(child.gameObject);
        }
        else if (child.childCount > 0) {
          for (int j = 0; j < child.childCount; j++) {
            var child2 = child.GetChild(j).gameObject;
            if(child2.name.StartsWith("Button"))
              AddButton(child2);
          }
        }
      }
    }

    //添加按钮
    private void AddButton(GameObject go) {
      var name = go.name;
      var GamePlayManager = Game.GamePlay.GamePlayManager.Instance;
      var CamManager = GamePlayManager.CamManager;
      var BallManager = GamePlayManager.BallManager;

      if (name == "Joystick") {

        var Joystick = go.GetComponent<KeyPadJoystickController>();
        Joystick.ValueChanged = (x, y) => BallManager.SetBallPushValue(x, y);

      }
      else if (name == "DirectionKey") {

        var DirectionKey = go.GetComponent<SimpleTouchDirectionKeyController>();
        DirectionKey.DirectionChanged = (state, dir) => {
          switch(dir) {
            case KeyPadJoystickDirection.Back:
              BallManager.KeyStateBack = state;
              BallManager.FlushBallPush();
              break;
            case KeyPadJoystickDirection.Forward:
              BallManager.KeyStateForward = state;
              BallManager.FlushBallPush();
              break;
            case KeyPadJoystickDirection.Left:
              BallManager.KeyStateLeft = state;
              BallManager.FlushBallPush();
              break;
            case KeyPadJoystickDirection.Right:
              BallManager.KeyStateRight = state;
              BallManager.FlushBallPush();
              break;
          }
        };
      } 
      else {
        var listener = EventTriggerListener.Get(go);
        if (name == "ButtonUp") {

          //只有调试模式才显示
          if (!GameManager.DebugMode)
            go.SetActive(false);
          else {
            //上升键
            listener.onDown = (_) => {
              BallManager.KeyStateUp = true;
              BallManager.FlushBallPush();
            };
            listener.onUp = (_) => {
              BallManager.KeyStateUp = false;
              BallManager.FlushBallPush();
            };
          }

        }
        else if (name == "ButtonDown") {

          //只有调试模式才显示
          if (!GameManager.DebugMode)
            go.SetActive(false);
          else {
            //下降键
            listener.onDown = (_) => {
              BallManager.KeyStateDown = true;
              BallManager.FlushBallPush();
            };
            listener.onUp = (_) => {
              BallManager.KeyStateDown = false;
              BallManager.FlushBallPush();
            };
          }

        }
        else if (name == "ButtonSpaceShift") {

          //shift和space二合一键
          listener.onDown = (_) => {
            shiftPressCount++;
            if (shiftPressCount >= 2 && GamePlayManager.BallManager.CanControllCamera) {
              lastCameraSpaceByShift = true;
              CamManager.RotateUp(true);
            }
          };
          listener.onUp = (_) => {
            shiftPressCount--;
            if (lastCameraSpaceByShift && GamePlayManager.BallManager.CanControllCamera) {
              lastCameraSpaceByShift = false;
              CamManager.RotateUp(false);
            }
          };
        }
        else if (name == "ButtonSpace") {

          //空格键
          listener.onDown = (_) => {
            if (GamePlayManager.BallManager.CanControllCamera) 
              CamManager.RotateUp(true);
          };
          listener.onUp = (_) => CamManager.RotateUp(false);

        }
        else if (name == "ButtonShift") {

          //hift键
          //listener.onDown = (_) => shiftPressOne = true;
          //listener.onUp = (_) => shiftPressOne = false;

        }
        else if (name == "ButtonCamLeft") {

          //摄像机左转键
          listener.onClick = (_) => {
            if (GamePlayManager.BallManager.CanControllCamera)
              CamManager.RotateLeft();
          };

        }
        else if (name == "ButtonCamRight") {

          //摄像机右转键
          listener.onClick = (_) => {
            if (GamePlayManager.BallManager.CanControllCamera)
              CamManager.RotateRight();
          };

        }
      }
    }

  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ballance2.Game
{
  [SLua.CustomLuaClass]
  public enum KeyPadJoystickDirection {
    None = 0,
    Left = 1,
    Right = 2,
    Forward = 4,
    Back = 8,
    LeftForward = Left | Forward,
    LeftBack = Left | Forward,
    RightForward = Right | Forward,
    RightBack = Right | Back,
  }

  [SLua.CustomLuaClass]
  public delegate void KeyPadJoystickDirectionChanged(bool stat, KeyPadJoystickDirection dir);

  [SLua.CustomLuaClass]
  [RequireComponent(typeof(SimpleTouchController))]
  public class KeyPadJoystickController : MonoBehaviour
  {
    public float DirectionFBMinValue = 0.7f;
    public float DirectionLRMinValue = 0.5f;

    public KeyPadJoystickDirectionChanged DirectionChanged;

    private void CallKeyPadJoystickDirectionChanged(bool stat, KeyPadJoystickDirection dir) {
      if(stat)
        currentState |= dir;
      else
        currentState ^= dir;
      DirectionChanged?.Invoke(stat, dir);
    }
 
    private SimpleTouchController controller;
    private KeyPadJoystickDirection currentState;
    private bool state = false;

    void Start()
    {
      controller = GetComponent<SimpleTouchController>();
      controller.TouchEvent += (value) => {
        if(state) {
          if(value.x <= -DirectionLRMinValue) {
            if((currentState & KeyPadJoystickDirection.Right) == KeyPadJoystickDirection.Right)
              CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Right);
            if((currentState & KeyPadJoystickDirection.Left) != KeyPadJoystickDirection.Left)
              CallKeyPadJoystickDirectionChanged(true, KeyPadJoystickDirection.Left);
          } else if(value.x >= DirectionLRMinValue) {

            if((currentState & KeyPadJoystickDirection.Left) == KeyPadJoystickDirection.Left)
              CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Left);
            if((currentState & KeyPadJoystickDirection.Right) != KeyPadJoystickDirection.Right)
              CallKeyPadJoystickDirectionChanged(true, KeyPadJoystickDirection.Right);
          } else {
            if((currentState & KeyPadJoystickDirection.Left) == KeyPadJoystickDirection.Left)
              CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Left);
            if((currentState & KeyPadJoystickDirection.Right) == KeyPadJoystickDirection.Right)
              CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Right);
          }
          if(value.y >= DirectionFBMinValue) {
            if((currentState & KeyPadJoystickDirection.Back) == KeyPadJoystickDirection.Back)
              CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Back);
            if((currentState & KeyPadJoystickDirection.Forward) != KeyPadJoystickDirection.Forward)
              CallKeyPadJoystickDirectionChanged(true, KeyPadJoystickDirection.Forward);
          } else if(value.y <= -DirectionFBMinValue) {

            if((currentState & KeyPadJoystickDirection.Forward) == KeyPadJoystickDirection.Forward)
              CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Forward);
            if((currentState & KeyPadJoystickDirection.Back) != KeyPadJoystickDirection.Back)
              CallKeyPadJoystickDirectionChanged(true, KeyPadJoystickDirection.Back);
          } else {
            if((currentState & KeyPadJoystickDirection.Forward) == KeyPadJoystickDirection.Forward)
              CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Forward);
            if((currentState & KeyPadJoystickDirection.Back) == KeyPadJoystickDirection.Back)
              CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Back);
          }
        }
      };      
      controller.TouchStateEvent += (state) => {
        this.state = state;
        if(!state) {
          if((currentState & KeyPadJoystickDirection.Forward) == KeyPadJoystickDirection.Forward)
            CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Forward);
          if((currentState & KeyPadJoystickDirection.Back) == KeyPadJoystickDirection.Back)
            CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Back);
          if((currentState & KeyPadJoystickDirection.Left) == KeyPadJoystickDirection.Left)
            CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Left);
          if((currentState & KeyPadJoystickDirection.Right) == KeyPadJoystickDirection.Right)
            CallKeyPadJoystickDirectionChanged(false, KeyPadJoystickDirection.Right);
        }
      };
    }
  }
}

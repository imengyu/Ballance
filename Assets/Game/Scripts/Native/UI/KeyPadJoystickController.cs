using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Ballance2.Services.GameManager;

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
    LeftBack = Left | Back,
    RightForward = Right | Forward,
    RightBack = Right | Back,
  }

  [SLua.CustomLuaClass]
  public delegate void KeyPadJoystickDirectionChanged(bool stat, KeyPadJoystickDirection dir);
  [SLua.CustomLuaClass]
  public delegate void KeyPadJoystickValueChanged(float valX, float valY);
  
  /// <summary>
  /// 模拟摇杆UI控制
  /// </summary>
  [SLua.CustomLuaClass]
  [RequireComponent(typeof(SimpleTouchController))]
  public class KeyPadJoystickController : MonoBehaviour
  {
    public KeyPadJoystickValueChanged ValueChanged;
    public VoidDelegate AddForce;
    public VoidDelegate DeleteForce;

    public float SpaceSize = 0.2f;

    private SimpleTouchController controller;
    private KeyPadJoystickDirection currentState;
    private bool state = false;

    void Start()
    {
      controller = GetComponent<SimpleTouchController>();
      controller.TouchEvent += (value) => {
        if(state)
          ValueChanged?.Invoke(value.x, value.y);
      };      
      controller.TouchStateEvent += (state) => {
        this.state = state;
        if(state) AddForce?.Invoke();
        else DeleteForce?.Invoke();
      };
    }
  }
}

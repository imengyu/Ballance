using UnityEngine;

namespace Ballance2.Menu
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
        ValueChanged?.Invoke(0, 0);
      };
    }
  }
}

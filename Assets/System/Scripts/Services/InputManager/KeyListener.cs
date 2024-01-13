using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* KeyListener.cs
* 
* 用途：
* 键盘按键事件事件侦听器
* 
* 此类可以方便的侦听键盘按键事件。
* 
* 使用示例：
* ```csharp
* var lisnster = KeyListener.Get(this.gameObject);
* lisnster.AddKeyListen(KeyCode.A, (key, downed) => { 
*   if (downed) {
*     //当 A 键按下时将会发出此事件
*   }
* });
* ```
*
* 如果你只需要单次监听某个按键，无须单独使用此类，可以直接使用 GameUIManager 上的 WaitKey，具体参见 Ballance2.Services.GameUIManager 。
*
* 作者：
* mengyu
*
*/

namespace Ballance2.Services.InputManager
{
    /// <summary>
    /// 键盘按键事件事件侦听器
    /// </summary>
    public class KeyListener : MonoBehaviour
    {
        private Gamepad currentGamepad;
        private Joystick currentJoystick;
        private DpadControl currentJoystickDpad;
        private ButtonControl[] currentJoystickButtonControls;
        private AxisControl currentJoystickRightStickAxisY;

        /// <summary>
        /// 键盘按键事件回调
        /// </summary>
        /// <param name="downed">是否按下</param>
        public delegate void KeyDelegate(KeyCode key, bool downed);


        /// <summary>
        /// 从 指定 GameObject 创建键事件侦听器
        /// </summary>
        /// <param name="go">指定 GameObject</param>
        /// <returns>返回事件侦听器实例</returns>
        public static KeyListener Get(GameObject go)
        {
            KeyListener listener = go.GetComponent<KeyListener>();
            if (listener == null) listener = go.AddComponent<KeyListener>();
            return listener;
        }

        private class KeyListenerItem
        {
            public KeyCode key;
            public bool downed = false;
            public KeyDelegate callBack;
            public int id;
            public GamepadButton[] gamepadButtons;
        }

        private LinkedList<KeyListenerItem> items = new LinkedList<KeyListenerItem>();
        [SerializeField]
        private bool isListenKey = true;
        private int listenKeyId = 0;

        /// <summary>
        /// 是否开启监听
        /// </summary>
        public bool IsListenKey { get { return isListenKey; } set { isListenKey = value; } }

        /// <summary>
        /// 如果UI激活时是否禁用键盘事件
        /// </summary>
        public bool DisableWhenUIFocused = true;

        /// <summary>
        /// 指定是否允许同时发出1个以上的键盘事件，否则同时只能发送一个键盘事件。以后注册的先发送
        /// </summary>
        public bool AllowMultipleKey = false;

        public KeyListener()
        {
            currentGamepad = Gamepad.current;
            currentJoystick = Joystick.current;
            if (currentJoystick != null)
            {
                var joystickChildren = currentJoystick.children;
                currentJoystickDpad = joystickChildren.FirstOrDefault(t => t is DpadControl) as DpadControl;
                currentJoystickRightStickAxisY = joystickChildren.FirstOrDefault(t => t.path.EndsWith("rz")) as AxisControl;
                currentJoystickButtonControls = joystickChildren.Where(t => t is ButtonControl).Cast<ButtonControl>().ToArray();
                if (currentJoystickButtonControls.Length < 15)
                    currentJoystickButtonControls = currentJoystickButtonControls.Concat(new ButtonControl[15 - currentJoystickButtonControls.Length]).ToArray();
            }
        }

        /// <summary>
        /// 重新发送当前已按下的按键事件
        /// </summary>
        public void ReSendPressingKey()
        {
            LinkedListNode<KeyListenerItem> cur = items.Last;
            while (cur != null)
            {
                var item = cur.Value;
                if (item.downed)
                {
                    var isKeydown = IsKeyOrGamepadButtonsDown(item);
                    if (isKeydown)
                    {
                        item.callBack(item.key, true);
                    }
                    else
                    {
                        item.downed = false;
                        item.callBack(item.key, false);
                    }
                }
                else
                    item.callBack(item.key, false);
                cur = cur.Previous;
            }
        }

        /// <summary>
        /// 添加侦听器侦听键
        /// </summary>
        /// <param name="key">键值。</param>
        /// <param name="callBack">回调函数。</param>
        /// <returns>返回一个ID, 可使用 DeleteKeyListen 删除侦听器</returns>
        public int AddKeyListen(KeyCode key, KeyDelegate callBack, params GamepadButton[] gamepadButtons)
        {
            listenKeyId++;

            KeyListenerItem item = new KeyListenerItem();
            item.callBack = callBack;
            item.key = key;
            item.gamepadButtons = gamepadButtons;
            item.id = listenKeyId;

            //逆序遍历链表。添加按键至相同按键位置
            LinkedListNode<KeyListenerItem> cur = items.Last;
            while (cur != null)
            {
                if (cur.Value.key == key)
                {
                    items.AddAfter(cur, item);
                    return listenKeyId;
                }
                cur = cur.Previous;
            }
            //没有找到相同按键，则添加到末尾
            items.AddLast(item);
            return listenKeyId;
        }

        /// <summary>
        /// 删除指定侦听器。
        /// </summary>
        /// <param name="id">AddKeyListen 返回的ID</param>
        public void DeleteKeyListen(int id)
        {
            //链表移除
            int count = 0;
            LinkedListNode<KeyListenerItem> cur = items.First;
            while (cur != null)
            {
                if (cur.Value.id == id)
                {
                    items.Remove(cur);
                    return;
                }
                cur = cur.Next;
                count++;

                if (count > items.Count)
                    break;
            }
        }
        /// <summary>
        /// 清空事件侦听器所有侦听键。
        /// </summary>
        public void ClearKeyListen()
        {
            items.Clear();
        }

        private ButtonControl GetGamepadButtonControl(GamepadButton button)
        {
            if (currentGamepad != null)
            {
                switch (button)
                {
                    case GamepadButton.LeftStick:
                        return currentGamepad.leftStickButton;
                    case GamepadButton.RightStick:
                        return currentGamepad.rightStickButton;
                    case GamepadButton.LeftShoulder:
                        return currentGamepad.leftShoulder;
                    case GamepadButton.RightShoulder:
                        return currentGamepad.rightShoulder;
                    case GamepadButton.LeftTrigger:
                        return currentGamepad.leftTrigger;
                    case GamepadButton.RightTrigger:
                        return currentGamepad.rightTrigger;
                    case GamepadButton.DpadUp:
                        return currentGamepad.dpad.up;
                    case GamepadButton.DpadDown:
                        return currentGamepad.dpad.down;
                    case GamepadButton.DpadLeft:
                        return currentGamepad.dpad.left;
                    case GamepadButton.DpadRight:
                        return currentGamepad.dpad.right;
                    case GamepadButton.North:
                        return currentGamepad.yButton;
                    case GamepadButton.South:
                        return currentGamepad.aButton;
                    case GamepadButton.West:
                        return currentGamepad.xButton;
                    case GamepadButton.East:
                        return currentGamepad.bButton;
                    case GamepadButton.Select:
                        return currentGamepad.selectButton;
                    case GamepadButton.Start:
                        return currentGamepad.startButton;
                }
            }
            else if (currentJoystick != null)
            {
                if (currentJoystickButtonControls == null)
                    return null;
                switch (button)
                {
                    case GamepadButton.LeftStick:
                        return currentJoystickButtonControls[13];
                    case GamepadButton.RightStick:
                        return currentJoystickButtonControls[14];
                    case GamepadButton.LeftShoulder:
                        return currentJoystickButtonControls[6];
                    case GamepadButton.RightShoulder:
                        return currentJoystickButtonControls[7];
                    case GamepadButton.LeftTrigger:
                        return currentJoystickButtonControls[8];
                    case GamepadButton.RightTrigger:
                        return currentJoystickButtonControls[9];
                    case GamepadButton.DpadUp:
                        return currentJoystickDpad?.up;
                    case GamepadButton.DpadDown:
                        return currentJoystickDpad?.down;
                    case GamepadButton.DpadLeft:
                        return currentJoystickDpad?.left;
                    case GamepadButton.DpadRight:
                        return currentJoystickDpad?.right;
                    case GamepadButton.North:
                        return currentJoystickButtonControls[4];
                    case GamepadButton.South:
                        return currentJoystickButtonControls[0];
                    case GamepadButton.West:
                        return currentJoystickButtonControls[3];
                    case GamepadButton.East:
                        return currentJoystickButtonControls[1];
                    case GamepadButton.Select:
                        return currentJoystickButtonControls[10];
                    case GamepadButton.Start:
                        return currentJoystickButtonControls[11];
                }
            }
            return null;
        }

        private bool IsKeyOrGamepadButtonsDown(KeyListenerItem item)
        {
            var isKeydown = false;
            //先判断键盘
            isKeydown = Input.GetKey(item.key);
            if (isKeydown)
                return isKeydown;
            //再判断手柄按键
            if (item.gamepadButtons != null && item.gamepadButtons.Length > 0)
            {
                foreach (var button in item.gamepadButtons)
                {
                    isKeydown = IsGamepadButtonPressed(button);
                    if (isKeydown)
                        break;
                }
            }
            if (isKeydown)
                return isKeydown;
            //最后再判断左摇杆操作
            Vector2 leftStickValue = default;
            if (currentGamepad != null)
            {
                leftStickValue = currentGamepad.leftStick.value;
            }
            else if (currentJoystick != null)
            {
                leftStickValue = currentJoystick.stick.value;
            }
            switch (item.key)
            {
                case KeyCode.LeftArrow:
                    isKeydown = leftStickValue.x < -0.4;
                    break;
                case KeyCode.RightArrow:
                    isKeydown = leftStickValue.x > 0.4;
                    break;
                case KeyCode.UpArrow:
                    isKeydown = leftStickValue.y > 0.4;
                    break;
                case KeyCode.DownArrow:
                    isKeydown = leftStickValue.y < -0.4;                    
                    break;
            }
            return isKeydown;
        }

        private bool IsGamepadButtonPressed(GamepadButton button)
        {
            var buttonControl = GetGamepadButtonControl(button);
            if (buttonControl == null)
                return false;
            return buttonControl.isPressed;
        }

        private void Update()
        {
            if (isListenKey)
            {
                //排除GUI激活
                if (DisableWhenUIFocused && (EventSystem.current.IsPointerOverGameObject() || GUIUtility.hotControl != 0))
                    return;

                //逆序遍历链表。后添加的按键事件最先处理
                LinkedListNode<KeyListenerItem> cur = items.Last;
                KeyCode lastPressedKey = KeyCode.None;
                while (cur != null)
                {
                    var item = cur.Value;
                    var isKeydown = IsKeyOrGamepadButtonsDown(item);

                    if (isKeydown && !item.downed)
                    {
                        Log.D(tag, item.key + " down");
                        if (!AllowMultipleKey && lastPressedKey == item.key)
                        {
                            //相同的按键，并且不允许发送相同按键，则不发送按键
                            if (item.downed)
                            {
                                item.downed = false;
                                item.callBack(item.key, false);
                            }
                        }
                        else
                        {
                            item.downed = true;
                            item.callBack(item.key, true);

                            if (!AllowMultipleKey)
                                lastPressedKey = item.key;
                        }
                    }
                    if (!isKeydown && item.downed)
                    {
                        Log.D(tag, item.key + " up");
                        item.downed = false;
                        item.callBack(item.key, false);
                    }
                    cur = cur.Previous;
                }
            }
        }
    }
}

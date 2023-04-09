
using System.Collections.Generic;
using Ballance2.Services.Debug;
using UnityEngine;

namespace Ballance2.Menu
{
  /// <summary>
  /// 手机键盘管理器。管理不同种类的键盘，你可以注册自己的键盘
  /// </summary>
  public class KeypadUIManager
  {
    public struct KeypadUIInfo {
      public KeypadUIInfo(GameObject prefab, Sprite image) {
        this.prefab = prefab;
        this.image = image;
      }
      public GameObject prefab;
      public Sprite image;
    }

    private const string TAG = "KeypadUIManager";
    private static Dictionary<string, KeypadUIInfo> keypadData = new Dictionary<string, KeypadUIInfo>();

    /// <summary>
    /// 注册键盘
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="prefab">键盘对象预制体</param>
    /// <param name="image">这个键盘的菜单图片，用于菜单显示</param>
    /// <returns>返回true注册成功，返回false失败，可能是已经注册过同名键盘</returns>
    public static bool AddKeypad(string name, GameObject prefab, Sprite image)
    {
      if (keypadData.ContainsKey(name)) {
        GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, $"Keypad {name} already registered!");
        return false;
      }

      if (prefab == null) {
        GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, $"AddKeypad {name} fail because prefab is null");
        return false;
      }

      keypadData.Add(name, new KeypadUIInfo(prefab, image));  
      return true;
    }
    /// <summary>
    /// 获取键盘是否已注册
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    public static bool GetKeypadRegistered(string name)
    {
      return keypadData.ContainsKey(name);
    } 
    /// <summary>
    /// 获取已注册键盘，如果没有找到则返回nil
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    public static KeypadUIInfo GetKeypad(string name)
    {
      if(keypadData.TryGetValue(name, out var result))
        return result;
      return default(KeypadUIInfo);
    } 
    /// <summary>
    /// 获取所有已注册键盘
    /// </summary>
    public static Dictionary<string, KeypadUIInfo>.ValueCollection GetAllKeypad()
    {
      return keypadData.Values;
    } 
    /// <summary>
    /// 取消键盘
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns>返回true注册成功，返回false失败</returns>
    public static bool DeletetKeypad(string name)  
    {
      if (!keypadData.ContainsKey(name)) {
        GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG, $"Keypad {name} not register!");
        return false;
      }
      keypadData.Remove(name);
      return true;
    } 
  }
}
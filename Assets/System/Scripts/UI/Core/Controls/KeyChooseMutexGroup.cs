using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;

/*
* Copyright(c) 2023 mengyu
*
* 模块名：     
* KeyChooseMutexGroup.cs
* 
* 用途：
* 用于键盘按键选择组件的互斥。
*
* 作者：
* mengyu
*/

namespace Ballance2.UI.Core.Controls
{
  /// <summary>
  /// 用于键盘按键选择组件的互斥。
  /// </summary>
  public class KeyChooseMutexGroup : MonoBehaviour
  {
    public KeyChoose[] KeyChooses = new KeyChoose[0];
    private Dictionary<KeyChoose, UnityAction<KeyCode>> KeyChooseListeners = new Dictionary<KeyChoose, UnityAction<KeyCode>>();

    private void Start() {
      foreach (var item in KeyChooses)
        AddKeyChooseMutexItem(item);
    }

    private void CheckMutexAndReset(KeyCode code, KeyChoose self) { 
      if (code == KeyCode.None)
        return;
      foreach (var item in KeyChooses)
      {
        if (item.value == code && item != self)
          item.value = KeyCode.None;
      }
    }

    public void AddKeyChooseMutexItem(KeyChoose choose) {
      UnityAction<KeyCode> listener = (code) => CheckMutexAndReset(code, choose);
      choose.onValueChanged.AddListener(listener);
      KeyChooseListeners.Add(choose, listener);
    }
    public void RemoveKeyChooseMutexItem(KeyChoose choose) {
      if (KeyChooseListeners.TryGetValue(choose, out var listener)) {
        choose.onValueChanged.RemoveListener(listener);
        KeyChooseListeners.Remove(choose);
      }
    }
  }
}

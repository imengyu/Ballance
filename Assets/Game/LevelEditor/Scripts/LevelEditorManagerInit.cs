using Ballance2.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ballance2.Package;
using Ballance2.Services;
using System;
using System.Collections;

namespace Ballance2.Game.LevelEditor {

  public static class LevelEditorManagerInit {
    
    public static void Init(string nextLoadLevel) 
    {
      GameManager.Instance.StartCoroutine(_Init(nextLoadLevel));
    }
    public static void Destroy() 
    {
    }
        
    private static IEnumerator _Init(string nextLoadLevel) 
    {
      yield return SceneManager.LoadSceneAsync(1);
      LevelEditorManager.Instance.Init(nextLoadLevel);
    }
  }
}

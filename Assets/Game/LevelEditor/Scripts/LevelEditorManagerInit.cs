using Ballance2.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ballance2.Package;
using Ballance2.Services;
using System;
using System.Collections;

namespace Ballance2.Game.LevelEditor {

  public static class LevelEditorManagerInit {
    
    public static GameObject EditorManagerGameObject = null;

    public static GameObject Init() {
      EditorManagerGameObject = CloneUtils.CloneNewObjectWithParent(GamePackage.GetSystemPackage().GetPrefabAsset("EditorManager"), null, "GameEditorManager");
      EditorManagerGameObject.SetActive(false);
      return EditorManagerGameObject;
    }
    public static void Destroy() {
      if (EditorManagerGameObject != null) 
        UnityEngine.Object.Destroy(EditorManagerGameObject);
      EditorManagerGameObject = null;
    }
    public static void Load(string nextLoadLevel)
    {
      EditorManagerGameObject.SetActive(true);
      LevelEditorManager.Instance.Init(nextLoadLevel);
    }
    public static void Unload()
    {
      EditorManagerGameObject.SetActive(false);
    }
  }
}

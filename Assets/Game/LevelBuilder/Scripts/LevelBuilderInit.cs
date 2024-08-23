using Ballance2.Utils;
using UnityEngine;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Menu.LevelManager;

namespace Ballance2.Game.LevelBuilder {

  public static class LevelBuilderInit {
    
    public static GameObject LevelBuilderGameObject = null;

    public static GameObject Init() {
      LevelBuilderGameObject = CloneUtils.CloneNewObject(GamePackage.GetSystemPackage().GetPrefabAsset("LevelBuilder"), "GameLevelBuilder");
      return LevelBuilderGameObject;
    }
    public static void Destroy() {
      if (LevelBuilderGameObject != null) 
        Object.Destroy(LevelBuilderGameObject);
      LevelBuilderGameObject = null;
    }

    public static void CoreDebugLevelBuliderEntry() {
      GameManager.GameMediator.NotifySingleEvent("CoreStartLoadLevel", LevelManager.Instance.GetInternalLevel("Level01"));
    }
  }
}

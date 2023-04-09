using Ballance2.Utils;
using UnityEngine;
using Ballance2.Package;
using Ballance2.Services;

namespace Ballance2.Game.LevelBuilder {

  public static class LevelBuilderInit {
    
    public static GameObject LevelBuilderGameObject = null;

    public static GameObject Init() {
      LevelBuilderGameObject = CloneUtils.CloneNewObject(GamePackage.GetSystemPackage().GetPrefabAsset("LevelBuilder.prefab"), "GameLevelBuilder");
      return LevelBuilderGameObject;
    }
    public static void Destroy() {
      if (LevelBuilderGameObject != null) 
        UnityEngine.Object.Destroy(LevelBuilderGameObject);
      LevelBuilderGameObject = null;
    }

    public static void CoreDebugLevelBuliderEntry() {
      GameManager.GameMediator.NotifySingleEvent("CoreStartLoadLevel", "Level01");
    }
    public static void CoreDebugLevelEnvironmentEntry() {
      GameManager.GameMediator.NotifySingleEvent("CoreStartLoadLevel", "测试关卡");
    }
  }
}

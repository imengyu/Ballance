using Ballance2.Game.GamePlay;
using Ballance2.Package;
using static Ballance2.Game.GamePlay.Balls.BallSoundManager;

namespace Ballance2.Game.LevelBuilder
{
  internal static class LevelInternal {

    private static bool BulitInModulsLoaded = false;

    public static void UnInitBulitInModuls() {
      BulitInModulsLoaded = false;
    }
    public static void InitBulitInModuls() {
      var GameLevelBuilder = LevelBuilder.Instance;
      var SystemPackage = GamePackage.GetSystemPackage();

      if (BulitInModulsLoaded)
        return;

      GameLevelBuilder.RegisterModul("P_Ball_Paper", SystemPackage.GetPrefabAsset("P_Ball_Paper.prefab"));
      GameLevelBuilder.RegisterModul("P_Ball_Stone", SystemPackage.GetPrefabAsset("P_Ball_Stone.prefab"));
      GameLevelBuilder.RegisterModul("P_Ball_Wood", SystemPackage.GetPrefabAsset("P_Ball_Wood.prefab"));
      GameLevelBuilder.RegisterModul("P_Box", SystemPackage.GetPrefabAsset("P_Box.prefab"));
      GameLevelBuilder.RegisterModul("P_Dome", SystemPackage.GetPrefabAsset("P_Dome.prefab"));
      GameLevelBuilder.RegisterModul("P_Modul_01", SystemPackage.GetPrefabAsset("P_Modul_01.prefab"));
      GameLevelBuilder.RegisterModul("P_Modul_03", SystemPackage.GetPrefabAsset("P_Modul_03.prefab"));
      GameLevelBuilder.RegisterModul("P_Modul_08", SystemPackage.GetPrefabAsset("P_Modul_08.prefab"));
      GameLevelBuilder.RegisterModul("P_Modul_17", SystemPackage.GetPrefabAsset("P_Modul_17.prefab"));
      GameLevelBuilder.RegisterModul("P_Modul_18", SystemPackage.GetPrefabAsset("P_Modul_18.prefab"));
      GameLevelBuilder.RegisterModul("P_Modul_19", SystemPackage.GetPrefabAsset("P_Modul_19.prefab"));
      GameLevelBuilder.RegisterModul("P_Modul_25", SystemPackage.GetPrefabAsset("P_Modul_25.prefab"));
      GameLevelBuilder.RegisterModul("P_Modul_26", SystemPackage.GetPrefabAsset("P_Modul_26.prefab"));
      GameLevelBuilder.RegisterModul("P_Modul_29", SystemPackage.GetPrefabAsset("P_Modul_29.prefab"));
      GameLevelBuilder.RegisterModul("P_Modul_30", SystemPackage.GetPrefabAsset("P_Modul_30.prefab"));
      GameLevelBuilder.RegisterModul("P_Modul_34", SystemPackage.GetPrefabAsset("P_Modul_34.prefab"));
      GameLevelBuilder.RegisterModul("P_Modul_37", SystemPackage.GetPrefabAsset("P_Modul_37.prefab"));
      GameLevelBuilder.RegisterModul("P_Modul_41", SystemPackage.GetPrefabAsset("P_Modul_41.prefab"));
      GameLevelBuilder.RegisterModul("P_Trafo_Paper", SystemPackage.GetPrefabAsset("P_Trafo_Paper.prefab"));
      GameLevelBuilder.RegisterModul("P_Trafo_Stone", SystemPackage.GetPrefabAsset("P_Trafo_Stone.prefab"));
      GameLevelBuilder.RegisterModul("P_Trafo_Wood", SystemPackage.GetPrefabAsset("P_Trafo_Wood.prefab"));
      GameLevelBuilder.RegisterModul("P_Extra_Life", SystemPackage.GetPrefabAsset("P_Extra_Life.prefab"));
      GameLevelBuilder.RegisterModul("P_Extra_Point", SystemPackage.GetPrefabAsset("P_Extra_Point.prefab"));
      GameLevelBuilder.RegisterModul("PC_CheckPoints", SystemPackage.GetPrefabAsset("PC_TwoFlames.prefab"));
      GameLevelBuilder.RegisterModul("PE_LevelEnd", SystemPackage.GetPrefabAsset("PE_Balloon.prefab"));
      GameLevelBuilder.RegisterModul("PS_LevelStart", SystemPackage.GetPrefabAsset("PS_FourFlames.prefab"));

      BulitInModulsLoaded = true;
    }
    public static void InitBulitInModulCustomSounds() {
      var BallSoundManager = GamePlayManager.Instance.BallSoundManager;
      BallSoundManager.AddSoundCollData(BallSoundManager.GetSoundCollIDByName("Dome"), new BallSoundCollData {
        MinSpeed = 5,
        MaxSpeed = 15,
        SleepAfterwards = 0.6f,
        SpeedThreadhold = 10,
        TimeDelayStart = 0.3f,
        TimeDelayEnd = 0.3f,
        HasRollSound = false,
        HitSoundName = "Dome"
      });
      BallSoundManager.AddSoundCollData(BallSoundManager.GetSoundCollIDByName("WoodOnlyHit"), new BallSoundCollData {
        MinSpeed = 5,
        MaxSpeed = 20,
        SleepAfterwards = 0.6f,
        SpeedThreadhold = 20,
        TimeDelayStart = 0.3f,
        TimeDelayEnd = 0.3f,
        HasRollSound = false,
        HitSoundName = "Wood"
      });
      BallSoundManager.AddSoundCollData(BallSoundManager.GetSoundCollIDByName("StoneOnlyHit"), new BallSoundCollData {
        MinSpeed = 5,
        MaxSpeed = 20,
        SleepAfterwards = 0.6f,
        SpeedThreadhold = 20,
        TimeDelayStart = 0.3f,
        TimeDelayEnd = 0.3f,
        HasRollSound = false,
        HitSoundName = "Stone"
      });
      BallSoundManager.AddSoundCollData(BallSoundManager.GetSoundCollIDByName("PaperOnlyHit"), new BallSoundCollData {
        MinSpeed = 5,
        MaxSpeed = 20,
        SleepAfterwards = 0.6f,
        SpeedThreadhold = 20,
        TimeDelayStart = 0.3f,
        TimeDelayEnd = 0.3f,
        HasRollSound = false,
        HitSoundName = "Paper"
      });
    }
  }
}
using System.Collections.Generic;
using Ballance2.Game.GamePlay.Balls;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Services.Debug;
using Ballance2.Utils;
using BallancePhysics.Wapper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.GamePlay.Debug
{
  /// <summary>
  /// 迷你机关调试环境
  /// </summary>
  public static class GamePlayModulDebugManager
  {
    private static TMP_Text stateText;
    private static GameObject Modul_Instace;

    public static void Init() {
      var GameDebugEntry = Ballance2.Entry.GameDebugEntry.Instance;
      //检查参数
      if (GameDebugEntry.ModulTestFloor == null) {
        GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "ModulTestFloor 未设置！");
        return;
      }
      if (GameDebugEntry.ModulTestUI == null) {
        GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "ModulTestUI 未设置！");
        return;
      }
      if (GameDebugEntry.ModulInstance) {
        GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "ModulInstance 未设置！");
        return;
      }
      if (GameDebugEntry.ModulName == "") {
        GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "ModulName 未设置！");
        return;
      }

      GamePlayInitManager.GamePlayInit(false, () => {
        GameObject PR_Resetpoint = null;
        GameObject Modul_Placeholder = null;
        GamePlayManager GamePlayManager = GamePlay.GamePlayManager.Instance;
        
        //加载物理环境
        GamePlayManager.GamePhysicsWorld.Create();

        //克隆路面
        var physicsData = GamePhysFloor.Data["Phys_Floors"];
        var physicsDataStopper = GamePhysFloor.Data["Phys_FloorStopper"];
        var group = GameManager.Instance.InstancePrefab(GameDebugEntry.ModulTestFloor, "ModulTestFloor");
        var childCount = group.transform.childCount;
        for(int i = 0; i < childCount; i++) {
          var go = group.transform.GetChild(i).gameObject;
          if (go.activeSelf) {
            if (go.name.StartsWith("Floor")) {
              var meshFilter = go.GetComponent<MeshFilter>();
              if (meshFilter != null && meshFilter.mesh != null) {
                var body = go.AddComponent<PhysicsObject>();
                body.DoNotAutoCreateAtAwake = true;
                body.Fixed = true;
                body.BuildRootConvexHull = false;
                body.Concave.Add(meshFilter.mesh);
                body.Friction = physicsData.Friction;
                body.Elasticity = physicsData.Elasticity;
                body.Layer = physicsData.Layer;
                body.UseExistsSurface = true;
                body.CollisionID = GamePlayManager.BallSoundManager.GetSoundCollIDByName(physicsData.CollisionLayerName);
                body.Physicalize();
              }
            } 
            else if (go.name.StartsWith("FloorStopper")) {
              var meshFilter = go.GetComponent<MeshFilter>();
              if (meshFilter != null && meshFilter.mesh != null) {
                var body = go.AddComponent<PhysicsObject>();
                body.DoNotAutoCreateAtAwake = true;
                body.Fixed = true;
                body.BuildRootConvexHull = false;
                body.Concave.Add(meshFilter.mesh);
                body.Friction = physicsDataStopper.Friction;
                body.Elasticity = physicsDataStopper.Elasticity;
                body.Layer = physicsDataStopper.Layer;
                body.UseExistsSurface = true;
                body.CollisionID = GamePlayManager.BallSoundManager.GetSoundCollIDByName(physicsDataStopper.CollisionLayerName);
                body.Physicalize();
              }
            }
            else if (go.name == "PR_Resetpoint") {
              PR_Resetpoint = go;
            } 
            else if (go.name == GameDebugEntry.ModulName) {
              Modul_Placeholder = go;
            }
          }
        }

        if (PR_Resetpoint == null) {
          GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "没有找到PR_Resetpoint, 请在测试路面预制体中添加一个");
          return;
        }
        if (Modul_Placeholder == null) {
          GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, $"没有找到{GameDebugEntry.ModulName}, 请在测试路面预制体中添加一个");
          return;
        }

        //克隆机关
        Modul_Instace = GameManager.Instance.InstancePrefab(GameDebugEntry.ModulInstance, "ModulInstance");
        Modul_Instace.transform.position = Modul_Placeholder.transform.position;
        Modul_Instace.transform.rotation = Modul_Placeholder.transform.rotation;
        Modul_Placeholder.SetActive(false);

        var Modul_Classes = Modul_Instace.GetComponents<ModulBase>();
        if (Modul_Classes.Length == 0) {
          GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, $"没有在{Modul_Instace.name}上找到 ModulBase ！请确定已绑定脚本");
          return;
        }
        var Modul_Class = Modul_Classes[0];
        
        //克隆UI
        var ui = GameManager.Instance.InstancePrefab(GameDebugEntry.ModulTestUI, GameManager.Instance.GameCanvas, "ModulTestUI");
        stateText = ui.transform.Find("Text").GetComponent<TMP_Text>();
        var button = ui.transform.Find("Text").GetComponent<Button>();

        var GameUIManager = Ballance2.Services.GameUIManager.Instance;

        button = ui.transform.Find("ButtonActive").GetComponent<Button>();
        button.onClick.AddListener(() => {
          Modul_Class.Active();
          UpdateText("Active");
        });
        button = ui.transform.Find("ButtonDeactive").GetComponent<Button>();
        button.onClick.AddListener(() => {
          Modul_Class.Deactive();
          UpdateText("Deactive");
        });
        button = ui.transform.Find("ButtonResetLevel").GetComponent<Button>();
        button.onClick.AddListener(() => {
          Modul_Class.Reset(ModulBaseResetType.LevelRestart);
          UpdateText("Reset levelRestart");
        });    
        button = ui.transform.Find("ButtonResetSector").GetComponent<Button>();
        button.onClick.AddListener(() => {
          Modul_Class.Reset(ModulBaseResetType.SectorRestart);
          UpdateText("Reset sectorRestart");
        });
        button = ui.transform.Find("ButtonQuit").GetComponent<Button>();
        button.onClick.AddListener(() => {
          ui.gameObject.SetActive(false);
          GameManager.Instance.QuitGame();
        });
        button = ui.transform.Find("ButtonBackup").GetComponent<Button>();
        button.onClick.AddListener(() => {
          Modul_Class.Backup();
          GameUIManager.GlobalToast("Modul_Class.Backup() !");
        });
        button = ui.transform.Find("ButtonCustom1").GetComponent<Button>();
        button.onClick.AddListener(() => {
          Modul_Class.DebugTestCommand(1);
          GameUIManager.GlobalToast("Modul_Class.DebugTestCommand(1) !");
        });
        button = ui.transform.Find("ButtonCustom2").GetComponent<Button>();
        button.onClick.AddListener(() => {
          Modul_Class.DebugTestCommand(2);
          GameUIManager.GlobalToast("Modul_Class.DebugTestCommand(2) !");
        });

        PR_Resetpoint.SetActive(false);

        Modul_Class.gameObject.SetActive(true);
        GameTimer.Delay(0.3f, () => {
          //初始备份数据
          Modul_Class.Backup();
          Modul_Class.Deactive();

          //构建数据
          GamePlayManager.SectorManager.CurrentLevelSectorCount = 1;
          GamePlayManager.SectorManager.CurrentLevelSectors[1] = new SectorManager.SectorDataStorage{ moduls = { Modul_Class } };
          GamePlayManager.SectorManager.CurrentLevelRestPoints[1] = new SectorManager.RestPointsDataStorage {
            point = PR_Resetpoint,
            flame = null
          };
          
          GamePlayManager.CreateSkyAndLight("L", null, StringUtils.StringToColor("#B09D89"));

          UpdateText("Active");
          //开始关卡
          GameMediator.Instance.NotifySingleEvent("CoreGamePlayManagerInitAndStart", null);
        });
      });
    } 

    private static void UpdateText(string state) {
      stateText.text = $"Modul state tools.\n{Modul_Instace.name} state. {state}";
    }
  }
}
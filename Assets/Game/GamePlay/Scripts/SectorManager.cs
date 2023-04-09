using System.Collections;
using System.Collections.Generic;
using Ballance2.Base;
using Ballance2.Game.GamePlay.Moduls;
using Ballance2.Menu;
using Ballance2.Services;
using Ballance2.Utils;
using UnityEngine;

namespace Ballance2.Game.GamePlay
{
  /// <summary>
  /// 节管理器，负责控制关卡游戏中每个小节机关的状态。
  /// </summary>
  public class SectorManager : MonoBehaviour 
  {
    private const string TAG = "SectorManager";

    /// <summary>
    /// 出生点数据存储结构
    /// </summary>
    public class RestPointsDataStorage {
      /// <summary>
      /// 出生点占位符对象
      /// </summary>
      public GameObject point;
      /// <summary>
      /// 火焰机关 PS_FourFlames/PC_TwoFlames
      /// </summary>
      public ModulBase flame;
    }
    /// <summary>
    /// 小节数据存储结构
    /// </summary>
    public class SectorDataStorage {
      /// <summary>
      /// 当前小节的所有机关实例
      /// </summary>
      public List<ModulBase> moduls = new List<ModulBase>();
    }
    /// <summary>
    /// 机关数据存储结构
    /// </summary>
    public class ModulDataStorage {
      /// <summary>
      /// 机关实例
      /// </summary>
      public ModulBase modul;
      /// <summary>
      /// 占位符原件
      /// </summary>
      public GameObject go;
    }

    public const int MAX_SECTOR_COUNT = 32;

    public int CurrentLevelSectorCount = 0;
    public int CurrentLevelModulCount = 0;
    public SectorDataStorage[] CurrentLevelSectors = new SectorDataStorage[MAX_SECTOR_COUNT + 1];
    public Dictionary<string, LevelBuilder.LevelBuilder.LevelBuilderModulStorage>.ValueCollection CurrentLevelModuls  
      => LevelBuilder.LevelBuilder.Instance.CurrentLevelModuls.Values;
    public RestPointsDataStorage[] CurrentLevelRestPoints = new RestPointsDataStorage[MAX_SECTOR_COUNT + 1];
    public PE_Balloon CurrentLevelEndBalloon = null;

    private void Start() {
      InitCommand();
      InitEvents();
    }
    private void OnDestroy() {
      DestroyCommand();
      DestroyEvent();
    }
      
    #region 事件

    /// <summary>
    /// 小节结束事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventSectorDeactive;
    /// <summary>
    /// 节更改事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventSectorChanged;
    /// <summary>
    /// 小节激活事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventSectorActive;
    /// <summary>
    /// 所有节重置事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventResetAllSector;

    private void InitEvents() {
      var events = GameMediator.Instance.RegisterEventEmitter("SectorManager");
      this.EventSectorDeactive = events.RegisterEvent("SectorDeactive");
      this.EventSectorChanged = events.RegisterEvent("SectorChanged");
      this.EventSectorActive = events.RegisterEvent("SectorActive");
      this.EventResetAllSector = events.RegisterEvent("ResetAllSector");
    }
    private void DestroyEvent() {
      GameMediator.Instance.UnRegisterEventEmitter("SectorManager");
    }

    #endregion

    #region 指令

    private int _CommandId = 0;
    private void InitCommand() {
      this._CommandId = GameManager.Instance.GameDebugCommandServer.RegisterCommand("sector", (eyword, fullCmd, argsCount, args) => {
        var type = args[0];
        if (type == "next")
          this.NextSector();
        else if (type == "set") {
          var o = DebugUtils.CheckIntDebugParam(1, args, out var n, true, 1);
          if (!o) return false;
          this.SetCurrentSector(n);
        }
        else if (type == "reset")
          this.ResetCurrentSector(true);
        else if (type == "reset-all")
          this.ResetAllSector(true);
        return true;
      }, 1, "sector <next/set/reset/reset-all> 节管理器命令" +
          "  next                  ▶ 进入下一小节" +
          "  set <sector:number>   ▶ 设置当前激活的小节" +
          "  reset                 ▶ 重置当前小节机关" + 
          "  reset-all             ▶ 重置所有小节"
      );
    }
    private void DestroyCommand() {
      GameManager.Instance.GameDebugCommandServer.UnRegisterCommand(this._CommandId);
    }
  
    #endregion

    internal void DoInitAllModuls() 
    {
      this.CurrentLevelModulCount = 0;
      //初次加载后通知每个modul进行备份
      foreach(var value in CurrentLevelModuls) {
        if (value != null) {
          value.modul.Backup();
          value.modul.Deactive();
          CurrentLevelModulCount++;
        }
      }
    }
    internal void DoUnInitAllModuls() 
    {
      DeactiveAllModuls(true, false, false);
    }
    internal void ClearAll() 
    {
      this.CurrentLevelSectorCount = 0;

      for (int i = 0; i < CurrentLevelRestPoints.Length; i++)
        CurrentLevelRestPoints[i] = null;
      for (int i = 0; i < CurrentLevelSectors.Length; i++)
        CurrentLevelSectors[i] = null;
      this.CurrentLevelEndBalloon = null;
    }
    internal void ActiveAllModulsForPreview() 
    {
      //通知每个modul卸载
      foreach (var item in CurrentLevelModuls)
        if (item != null)
          item.modul.ActiveForPreview();
    }
    
    private void DeactiveAllModuls(bool afterUnload, bool afterActive, bool reset) {
      var preview = LevelBuilder.LevelBuilder.Instance.IsPreviewMode;
      //通知每个modul卸载
      foreach (var item in CurrentLevelModuls)
      {
        if (item == null)
          continue;
        if (preview)
          item.modul.DeactiveForPreview();
        else
          item.modul.Deactive();

        if (afterUnload)
          item.modul.UnLoad();
        if (afterActive)
          item.modul.Active();
        if (reset)
          item.modul.Reset(ModulBaseResetType.LevelRestart);
      }
    }

    /// <summary>
    /// 进入下一小节
    /// </summary>
    public void NextSector() 
    {
      if (GamePlayManager.Instance.CurrentSector < this.CurrentLevelSectorCount)
        this.SetCurrentSector(GamePlayManager.Instance.CurrentSector + 1);
    }
    /// <summary>
    /// 设置当前激活的小节
    /// </summary>
    /// <param name="sector"></param>
    public void SetCurrentSector(int sector) 
    {
      var oldSector = GamePlayManager.Instance.CurrentSector;
      if (oldSector != sector) {
        //禁用之前一节的所有机关
        if (oldSector > 0) {
          var s = this.CurrentLevelSectors[oldSector];
          foreach (var value in s.moduls)
            value.Deactive();

          //设置火焰状态
          var flame = this.CurrentLevelRestPoints[oldSector].flame;
          if (flame != null) {
            if (flame is PC_TwoFlames)
              ((PC_TwoFlames)flame).CheckPointActived = true;
            flame.Deactive();
          }
          else {
            Log.D(TAG, "No flame found for sector " + oldSector);
          }

          this.EventSectorDeactive.Emit(oldSector);
        }

        if (sector > 0) { 
          GamePlayManager.Instance.CurrentSector = sector;
          this.ActiveCurrentSector(true);
        }

        this.EventSectorChanged.Emit(sector, oldSector);
      }
    }
    /// <summary>
    /// 激活当前节的机关
    /// </summary>
    /// <param name="playCheckPointSound">是否播放节点音乐</param>
    public void ActiveCurrentSector(bool playCheckPointSound) 
    {
      var sector = GamePlayManager.Instance.CurrentSector;
      var nowSector = this.CurrentLevelRestPoints[sector];

      //设置火焰状态

      if (nowSector.flame != null)
        nowSector.flame.Active();
      else
        Log.D(TAG, "No flame found for sector " + sector);
      if (sector < this.CurrentLevelSectorCount) {
        nowSector = this.CurrentLevelRestPoints[sector + 1];
        //下一关的火焰
        var flameNext = nowSector.flame;
        if (flameNext != null && flameNext is PC_TwoFlames)
          ((PC_TwoFlames)flameNext).InternalActive();
      }

      //播放音乐
      if (playCheckPointSound && sector > 1)
        GameSoundManager.Instance.PlayFastVoice("core.sounds:Misc_Checkpoint.wav", GameSoundType.Normal);

      //如果是最后一个小节，则激活飞船
      if (this.CurrentLevelEndBalloon != null) {
        if (sector == this.CurrentLevelSectorCount)
          this.CurrentLevelEndBalloon.Active();
        else
          this.CurrentLevelEndBalloon.Deactive();
      }
      else
        Log.W(TAG, "No found CurrentLevelEndBalloon !");

      Log.D(TAG, "Active Sector " + sector);

      //激活当前节的机关
      StartCoroutine(ActiveSectorModuls(sector));

      this.EventSectorActive.Emit(sector, playCheckPointSound);
    }

    private IEnumerator ActiveSectorModuls(int sector) {
      var s = this.CurrentLevelSectors[sector];
      if (s == null && sector != 0) {
        Log.E(TAG, $"Sector {sector} not found");
        GamePlayManager.Instance.CurrentSector = 0;
        yield break;
      }
      var count = 0;
      for (int i = 0; i < s.moduls.Count; i++)
      {
        s.moduls[i].Active();

        //延时下防止一下生成过多机关
        count++;
        if (count >= 2) {
          count = 0;
          yield return new WaitForSeconds(0.05f);
        }
      }

      if (GameManager.DebugMode) { 
        GamePlayUIControl.Instance.DebugStatValues["Sector"].Value = sector + "/" + this.CurrentLevelSectorCount;
        GamePlayUIControl.Instance.DebugStatValues["Moduls"].Value = s.moduls.Count + "/" + this.CurrentLevelModulCount;
      }
    }

    /// <summary>
    /// 禁用当前节的机关
    /// </summary>
    public void DeactiveCurrentSector()  
    {
      var sector = GamePlayManager.Instance.CurrentSector;
      if (sector > 0) {
        var s = this.CurrentLevelSectors[sector];
        foreach (var value in s.moduls) {
          value.Deactive();
          value.Reset(ModulBaseResetType.SectorRestart);
        }
      }

      //调试信息
      if (GameManager.DebugMode) { 
        GamePlayUIControl.Instance.DebugStatValues["Sector"].Value = sector + "(Deactive)/" + this.CurrentLevelSectorCount;
        GamePlayUIControl.Instance.DebugStatValues["Moduls"].Value = "0";
      }

      Log.D(TAG, $"Deactive current sector {sector}");

      this.EventSectorDeactive.Emit(sector);
    }
    /// <summary>
    /// 重置当前节的机关
    /// </summary>
    /// <param name="active">重置机关后是否重新激活</param>
    public void ResetCurrentSector(bool active)  
    {
      this.DeactiveCurrentSector();
      if (active) {
        this.ActiveCurrentSector(false);
      }
    }
    /// <summary>
    /// 重置所有机关
    /// </summary>
    /// <param name="active">重置机关后是否重新激活</param>
    public void ResetAllSector(bool active) 
    {
      //通知每个modul卸载
      DeactiveAllModuls(false, active, true);

      //重置飞船
      this.CurrentLevelEndBalloon.Reset(ModulBaseResetType.LevelRestart);

      Log.D(TAG, "Reset all sector");

      this.EventSectorDeactive.Emit(active);
    }
  }
}
using Ballance2.Base;
using Ballance2.Game.Utils;
using Ballance2.Menu;
using Ballance2.Package;
using Ballance2.Services;
using BallancePhysics.Wapper;
using UnityEngine;

namespace Ballance2.Game.GamePlay
{
  /// <summary>
  /// 关卡预览管理器，负责关卡预览模式时的一些控制行为。
  /// </summary>
  public class GamePreviewManager : GameSingletonBehavior<GamePreviewManager>
  {
    private const string TAG = "GamePreviewManager";

    private bool _IsGamePlaying = false;
    private bool _IsLoaded = false;

    public Camera GamePreviewCamera;
    public Camera GamePreviewMinimapCamera;
    public FreeCamera GamePreviewFreeCamera;
    public Skybox GamePreviewCameraSkyBox;
    public GameObject[] GameDepthTestCubes;
    public MusicManager MusicManager;
    public CamManager CamManager;
    public SectorManager SectorManager;

    private int escKeyId = 0;
    private GameMediator Mediator;
    private GameUIManager UIManager;
    private GameSoundManager SoundManager;
    private GamePlayPreviewUIControl GamePreviewUI;

    private void Awake() {
      Mediator = GameMediator.Instance;
      UIManager = GameUIManager.Instance;
      SoundManager = GameSoundManager.Instance;
      var SystemPackage = GamePackage.GetSystemPackage();

      //注册全局事件
      Mediator.SubscribeSingleEvent(SystemPackage, "CoreGamePreviewManagerInitAndStart", "GamePreviewManager", (evtName, param) => {
        GamePreviewUI = GamePlayPreviewUIControl.Instance;
        SoundManager.PlayFastVoice("core.sounds:Misc_StartLevel.wav", GameSoundType.Normal); //播放开始音乐
        GamePreviewUI.gameObject.SetActive(true);
        GamePreviewUI.SetLevelInfo(param[0] as string, param[1] as string, param[2] as string);
        GamePreviewUI.SetFreeCamera(GamePreviewFreeCamera);
        CamManager.gameObject.SetActive(false);
        CamManager._CamTarget.gameObject.SetActive(false);
        MusicManager.EnableBackgroundMusic();
        GamePreviewMinimapCamera.gameObject.SetActive(true);
        GamePreviewCamera.gameObject.SetActive(true);
        SectorManager.ActiveAllModulsForPreview(); //激活机关

        UIManager.CloseAllPage();
        UIManager.MaskBlackFadeOut(1);
        _IsLoaded = true;

        //ESC键
        escKeyId = UIManager.ListenKey(KeyCode.Escape, (key, down) => {
          if (down) {
            if (_IsGamePlaying)
              PauseLevel();
            else
              ResumeLevel();
          }
        });
        return false;
      });
    }
    protected override void OnDestroy() {
      if (escKeyId > 0) {
        GameUIManager.Instance.DeleteKeyListen(escKeyId);
        escKeyId = 0;
      }
      //取消注册全局事件
      Mediator.UnRegisterSingleEvent("CoreGamePreviewManagerInitAndStart");
    }

    public void PauseLevel() {
      SoundManager.PlayFastVoice("core.sounds:Menu_click.wav", GameSoundType.UI);
      UIManager.GoPage("PageGamePreviewPause") ;
      _IsGamePlaying = false;
    }
    public void QuitLevel() {
      if (_IsLoaded) {
        _IsLoaded = false;
        SoundManager.PlayFastVoice("core.sounds:Menu_load.wav", GameSoundType.Normal);
        UIManager.MaskBlackFadeIn(1);
        GameTimer.Delay(1, () => {
          GamePreviewUI.gameObject.SetActive(false);
          MusicManager.DisableBackgroundMusic();
          CamManager.gameObject.SetActive(true);
          LevelBuilder.LevelBuilder.Instance.UnLoadLevel(null);
          GamePreviewMinimapCamera.gameObject.SetActive(false);
          GamePreviewCamera.gameObject.SetActive(false);
        });
      }
    }
    public void ResumeLevel() {
      _IsGamePlaying = true;
      SoundManager.PlayFastVoice("core.sounds:Menu_click.wav", GameSoundType.UI);
      UIManager.CloseAllPage();
    }

    /// <summary>
    /// 初始化灯光和天空盒
    /// </summary>
    /// <param name="skyBoxPre">A-K 或者空，为空则使用 customSkyMat 材质</param>
    /// <param name="customSkyMat">自定义天空盒材质</param>
    /// <param name="lightColor">灯光颜色</param>
    public void CreateSkyAndLight(string skyBoxPre, Material customSkyMat, Color lightColor) {
      GamePreviewCameraSkyBox.material = customSkyMat == null ? SkyBoxUtils.MakeSkyBox(skyBoxPre) : customSkyMat;
      Ballance2.Services.GameManager.GameLight.color = lightColor;
    }
    /// <summary>
    /// 隐藏天空盒和关卡灯光
    /// </summary>
    public void HideSkyAndLight() {
      GamePreviewCameraSkyBox.material = null;
    }

    public void ToggleDethTethCubesVisible(bool visible) {
      foreach (var value in GameDepthTestCubes)
        value.SetActive(visible);
    }
    public void ToggleSkylayerVisible(bool visible) {
      if (LevelBuilder.LevelBuilder.Instance.CurrentLevelSkyLayer)
        LevelBuilder.LevelBuilder.Instance.CurrentLevelSkyLayer.SetActive(visible);
    }
  }
}
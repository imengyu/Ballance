using Ballance2.Game.Utils;
using Ballance2.Package;
using Ballance2.Services;
using UnityEngine;

namespace Ballance2.Game 
{
  public class MenuLevelCameraControl : MonoBehaviour 
  {
    public const string EVENT_SWITCH_LIGHTZONE = "swicth_menulevel_lightzone";

    public GameObject I_Zone;
    public GameObject I_Zone_SuDu;
    public GameObject I_Zone_NenLi;
    public GameObject I_Zone_LiLiang;
    public GameObject I_Dome;
    public Skybox skyBox;

    private Material skyBoxNight;
    private Material skyBoxDay;
    private AudioSource menuSound;
    private float menuSoundTime = 0;
    private float speed = -10;
    private bool isInLightZone = false;
    private bool isRoatateCam = true;
    private Vector3 domePosition;
    private Transform transformI_Zone_SuDu = null;
    private Transform transformI_Zone_NenLi = null;
    private Transform transformI_Zone_LiLiang = null;

    private bool _Stared = false;
    private GameUIManager GameUIManager;
    private GameSoundManager GameSoundManager;

    private void Start() {
      GameUIManager = GameManager.GetSystemService<GameUIManager>();
      GameSoundManager = GameManager.GetSystemService<GameSoundManager>();

      this.domePosition = this.I_Dome.transform.position;
      this.transformI_Zone_SuDu = this.I_Zone_SuDu.transform;
      this.transformI_Zone_NenLi = this.I_Zone_NenLi.transform;
      this.transformI_Zone_LiLiang = this.I_Zone_LiLiang.transform;
      this.skyBoxNight = SkyBoxUtils.MakeSkyBox("M");
      this.skyBoxDay = SkyBoxUtils.MakeSkyBox("C");
      this.menuSound = GameSoundManager.RegisterSoundPlayer(
        GameSoundType.Background, 
        GameSoundManager.LoadAudioResource("core.sounds.music:Menu_atmo.wav"), 
        false, 
        true, 
        "MenuSound"
      );
      this.menuSound.Play();

      this.SwitchLightZone(false, false);
      this._Stared = true;

      GameManager.GameMediator.RegisterSingleEvent(EVENT_SWITCH_LIGHTZONE);
      GameManager.GameMediator.SubscribeSingleEvent(GamePackage.GetSystemPackage(), EVENT_SWITCH_LIGHTZONE, "CameraControl", (evtName, param) => {
        if ((bool)param[0])
          this.SwitchLightZone(true, true);
        else
          this.SwitchLightZone(false, true);
        return false;
      });
    }

    private void Update() {
      if (!this.menuSound.isPlaying) {
        this.menuSoundTime -= Time.deltaTime;
        if (this.menuSoundTime < 0) {
          this.menuSound.Play();
          //随机时间播放Menu_atmo
          this.menuSoundTime = Random.Range(1, 10);
        }
      }
      if(this.isRoatateCam) {
        this.transform.RotateAround(this.domePosition, Vector3.up, Time.deltaTime * this.speed);
        this.transform.LookAt(this.domePosition);
      }
      if(this.isInLightZone) { 
        this.transformI_Zone_SuDu.LookAt(this.transform.position, Vector3.up);
        this.transformI_Zone_NenLi.LookAt(this.transform.position, Vector3.up);
        this.transformI_Zone_LiLiang.LookAt(this.transform.position, Vector3.up);
        this.transformI_Zone_SuDu.eulerAngles = new Vector3(0, this.transformI_Zone_SuDu.eulerAngles.y, 0);
        this.transformI_Zone_NenLi.eulerAngles = new Vector3(0, this.transformI_Zone_NenLi.eulerAngles.y, 0);
        this.transformI_Zone_LiLiang.eulerAngles = new Vector3(0, this.transformI_Zone_LiLiang.eulerAngles.y, 0);
      }
    }
    private void OnDisable() {
      RenderSettings.ambientLight = new Color32(15, 15, 15, 255);
      this.menuSound.Stop();
    }
    private void OnEnable() {
      if (this._Stared) { 
        this.menuSound.Play();
        this.SwitchLightZone(false, false);
      }
    }

    private void SetFog(bool isLz) {
      RenderSettings.fog = true;
      RenderSettings.fogDensity = 0.03f;
      RenderSettings.fogStartDistance = 100;
      RenderSettings.fogEndDistance = 800;
      RenderSettings.fogMode = FogMode.Linear;
      if(isLz)
        RenderSettings.fogColor = new Color(0.180f, 0.254f, 0.301f);
      else
        RenderSettings.fogColor = new Color(0.827f, 0.784f, 0.581f);
    }
    /// <summary>
    /// 切换主菜单关卡LightZone模式
    /// </summary>
    /// <param name="on">是否是LightZone模式</param>
    /// <param name="isClick">是否是用户点击所触发的</param>
    private void SwitchLightZone(bool on, bool isClick) {
      if(on) 
      {
        GameSoundManager.PlayFastVoice("core.sounds.music:Music_thunder.wav", GameSoundType.Background);
        GameUIManager.MaskBlackSet(true);
        GameUIManager.MaskBlackFadeOut(1);
        GameManager.GameLight.color = new Color(0.2f,0.4f,0.6f);
        RenderSettings.ambientLight = new Color32(15, 15, 15, 255);
        this.I_Zone.SetActive(true);
        this.skyBox.material = this.skyBoxNight;
        this.isInLightZone = true;
        this.SetFog(true);
      }
      else
      {
        if (isClick) {
          GameUIManager.MaskBlackSet(true);
          GameUIManager.MaskBlackFadeOut(1);
        }
        GameManager.GameLight.color = new Color(1,1,1);
        RenderSettings.ambientLight = new Color32(90, 90, 90, 255);
        this.I_Zone.SetActive(false);
        this.skyBox.material = this.skyBoxDay;
        this.isInLightZone = false;
        this.SetFog(false);
      }
    }
  }
}
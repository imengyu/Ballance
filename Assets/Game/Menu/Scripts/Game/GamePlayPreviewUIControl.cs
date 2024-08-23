using Ballance2.Base;
using Ballance2.Game;
using Ballance2.Game.GamePlay;
using Ballance2.Game.LevelBuilder;
using Ballance2.Services.I18N;
using RuntimeSceneGizmo;
using TMPro;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  /// <summary>
  /// 主游戏菜单控制器类
  /// </summary>
  public class GamePlayPreviewUIControl : GameSingletonBehavior<GamePlayPreviewUIControl> 
  {
    public TMP_Text _TextLevelInfo;
    public TMP_Text _TextCameraSpeed;
    public Toggle _CheckBoxSkyBox;
    public Toggle _CheckBoxWireFrame;
    public Toggle _CheckBoxAudio;
    public Toggle _CheckBoxFog;
    public Toggle _CheckBoxDethTethCubes;
    public Toggle _CheckBoxSkylayer;
    public SceneGizmoRenderer _GizmoRenderer;

    private FreeCamera _FreeCamera;

    private void Start() {
      _CheckBoxSkyBox.onValueChanged.AddListener((check) => 
        _FreeCamera.SkyBox = check
      );
      _CheckBoxWireFrame.onValueChanged.AddListener((check) =>  
        _FreeCamera.Wireframe = check
      );
      _CheckBoxFog.onValueChanged.AddListener((check) =>  
        _FreeCamera.Fog = check
      );
      _CheckBoxAudio.onValueChanged.AddListener((check) =>  
        _FreeCamera.Audio = check
      );
      _CheckBoxDethTethCubes.onValueChanged.AddListener((check) =>  
        GamePreviewManager.Instance.ToggleDethTethCubesVisible(check)
      );
      _CheckBoxSkylayer.onValueChanged.AddListener((check) =>  
        GamePreviewManager.Instance.ToggleSkylayerVisible(check)
      );

    }

    /// <summary>
    /// 设置预览关卡信息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="author"></param>
    /// <param name="version"></param>
    public void SetLevelInfo(string name, string author, string version) {
      var json = LevelBuilder.Instance.CurrentLevelJson;
      var level = json.level;
      _TextLevelInfo.text = name + "\n"
         + "\nAuthor:" + author
         + "\nVersion:" + version + "\n"
         + "\nFirstBall:" + level.firstBall
         + "\nLevelScore:" + level.levelScore
         + "\nStartPoint:" + level.startPoint
         + "\nStartLife:" + level.startLife
         + "\nSector count:" + level.sectorCount
         + "\nLightColor:" + level.lightColor
         + "\nSkyBox:" + level.skyBox
         + "\nMusicTheme:" + level.musicTheme;
    }
    /// <summary>
    /// 设置FreeCamera引用
    /// </summary>
    /// <param name="freeCam"></param>
    public void SetFreeCamera(FreeCamera freeCam) {
      _FreeCamera = freeCam;
      _GizmoRenderer.ReferenceTransform = freeCam.transform;
      freeCam.onCamSpeedChanged = () => {
        _TextCameraSpeed.text = I18N.TrF("CORE.UI.PREVIEWUI.CAMERASPEED", "", string.Format("{0:F2}", freeCam.cameraSpeed));
      };
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ballance2;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Utils;
using SimpleFileBrowser;
using TMPro;
using TriLibCore.SFB;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorEnvInfoControl : MonoBehaviour
  {
    private const string TAG = "LevelEditorEnvInfoControl";
    private LevelDynamicAssembe level;

    public Toggle ToggleUFO;
    public TMP_Dropdown DropdownMusic;
    public TMP_Dropdown DropdownSkyBox;
    public Image LightColor;
    public Image ImageSkyBoxF;
    public Image ImageSkyBoxB;
    public Image ImageSkyBoxL;
    public Image ImageSkyBoxR;
    public Image ImageSkyBoxD;

    private List<string> musics = new List<string>() {
      "1", "2", "3", "4", "5"
    };
    private List<string> skys = new List<string>() {
      "custom", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M"
    };

    private AudioSource musicPreviewPlayer;
    private bool noMusicChange = true;
    private bool noSkyChange = true;

    private void Awake() {
      musicPreviewPlayer = GameSoundManager.Instance.RegisterSoundPlayer(GameSoundType.Background, "");
      musicPreviewPlayer.loop = false;

      ToggleUFO.onValueChanged.AddListener((v) => {
        level.LevelInfo.level.endWithUFO = v;
      });
      DropdownSkyBox.options.Clear();
      DropdownMusic.options.Clear();
      foreach (var item in musics)
        DropdownMusic.options.Add(new TMP_Dropdown.OptionData() { text = item });
      foreach (var item in skys)
        DropdownSkyBox.options.Add(new TMP_Dropdown.OptionData() { text = item });
      DropdownSkyBox.onValueChanged.AddListener((v) => {
        if (noSkyChange)
          return;
        level.LevelInfo.level.skyBox = skys[v];
        StartCoroutine(DelayUpdateSky());
      });
      DropdownMusic.onValueChanged.AddListener((v) => {
        if (noMusicChange)
          return;
        level.LevelInfo.level.musicTheme = int.Parse(musics[v]);
        if (musicPreviewPlayer.isPlaying)
          musicPreviewPlayer.Stop();
        musicPreviewPlayer.clip = GameSoundManager.Instance.LoadAudioResource($"core.sounds.music:Music_Theme_{level.LevelInfo.level.musicTheme}_1.wav");
        musicPreviewPlayer.Play();
      });
    }
    private void OnEnable()
    {
      noMusicChange = true;
      noSkyChange = true;
      level = LevelEditorManager.Instance.LevelCurrent;
      ToggleUFO.isOn = level.LevelInfo.level.endWithUFO;
      DropdownMusic.value = musics.IndexOf(level.LevelInfo.level.musicTheme.ToString());
      DropdownSkyBox.value = skys.IndexOf(level.LevelInfo.level.skyBox);
      LightColor.color = StringUtils.StringToColor(level.LevelInfo.level.lightColor);
      UpdateSkySelectImages();
      noMusicChange = false;
      noSkyChange = false;
    }
    private void OnDestroy() {
      GameSoundManager.Instance?.DestroySoundPlayer(musicPreviewPlayer);
    }


    private IEnumerator DelayUpdateSky()
    {
      yield return StartCoroutine(LevelEditorManager.Instance.CreateSky());
      UpdateSkySelectImages();
    }
    private IEnumerator LoadSkyImage(string path, string type)
    {
      var targetPath = $"{level.LevelDirPath}/assets/CustomSkyBox{type}.png";
      try
      {
        if (!File.Exists(path))
          throw null;
        File.Copy(path, targetPath, true);
      }
      catch (System.Exception e)
      {
        Log.E(TAG, "Copy file to asset failed: " + e.ToString());
        LevelEditorManager.Instance.LevelEditorUIControl.Alert("I18N:core.ui.Tip", "I18N:core.editor.messages.LoadSkyTexFailed", LevelEditorConfirmIcon.Error);
        yield break;
      }

      var result = new EnumeratorResultPacker<Texture2D>();
      yield return StartCoroutine(TextureUtils.LoadTexture2dFromFile(targetPath, result));
      if (result.Result == null)
      {
        Log.E(TAG, "LoadTexture2dFromFile failed: " + result.Error);
        LevelEditorManager.Instance.LevelEditorUIControl.Alert("I18N:core.ui.Tip", "I18N:core.editor.messages.LoadSkyTexFailed", LevelEditorConfirmIcon.Error);
        yield break;
      }
    
      switch (type)
      {
        case "F": ImageSkyBoxF.sprite = TextureUtils.CreateSpriteFromTexture(result.Result); break;
        case "B": ImageSkyBoxB.sprite = TextureUtils.CreateSpriteFromTexture(result.Result); break;
        case "L": ImageSkyBoxL.sprite = TextureUtils.CreateSpriteFromTexture(result.Result); break;
        case "R": ImageSkyBoxR.sprite = TextureUtils.CreateSpriteFromTexture(result.Result); break;
        case "D": ImageSkyBoxD.sprite = TextureUtils.CreateSpriteFromTexture(result.Result); break;
      }
      yield return StartCoroutine(LevelEditorManager.Instance.CreateSky());
    }
    public void ChooseSkyImageClick(string type)
    {
      if (DropdownSkyBox.value == 0)
      {
        var result = StandaloneFileBrowser.OpenFilePanel("Pick image file", "", new[] { new ExtensionFilter("Png image file", ".png") }, false);
        if (result.Count > 0)
        {
          LevelEditorManager.Instance.LevelEditorUIControl.ShowLoading();
          StartCoroutine(LoadSkyImage(result[0].Name, type));
          LevelEditorManager.Instance.LevelEditorUIControl.HideLoading();
        }
      }
    }
    public void ChooseColorButtonClick()
    {
      ColorPicker.Create(StringUtils.StringToColor(level.LevelInfo.level.lightColor), "ColorPicker", SetColor, (_) => {}, true);
    }
    
    private void UpdateSkySelectImages()
    {
      var skyMat = LevelEditorManager.Instance.LevelEditorCameraSkyBox.material;
      var _FrontTex = TextureUtils.TextureToTexture2D(skyMat.GetTexture("_FrontTex"));
      var _BackTex = TextureUtils.TextureToTexture2D(skyMat.GetTexture("_BackTex"));
      var _LeftTex = TextureUtils.TextureToTexture2D(skyMat.GetTexture("_LeftTex"));
      var _RightTex = TextureUtils.TextureToTexture2D(skyMat.GetTexture("_RightTex"));
      var _DownTex = TextureUtils.TextureToTexture2D(skyMat.GetTexture("_DownTex"));
      ImageSkyBoxF.sprite = _FrontTex != null ? TextureUtils.CreateSpriteFromTexture(_FrontTex) : null;
      ImageSkyBoxB.sprite = _BackTex != null ? TextureUtils.CreateSpriteFromTexture(_BackTex) : null;
      ImageSkyBoxL.sprite = _LeftTex != null ? TextureUtils.CreateSpriteFromTexture(_LeftTex) : null;
      ImageSkyBoxR.sprite = _RightTex != null ? TextureUtils.CreateSpriteFromTexture(_RightTex) : null;
      ImageSkyBoxD.sprite = _DownTex != null ? TextureUtils.CreateSpriteFromTexture(_DownTex) : null;
    }
    private void SetColor(Color currentColor)
    {
      LightColor.color = currentColor;
      LevelEditorManager.Instance.SetLightColor(currentColor);
    }
  }
}

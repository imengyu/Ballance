using Ballance2.Base;
using Ballance2.Config;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services.Debug;
using Ballance2.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameSoundManager.cs
* 
* 用途：
* 声音管理器，用于管理声音部分通用功能
*
* 作者：
* mengyu
*/

namespace Ballance2.Services
{
  /// <summary>
  /// 声音管理器
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("声音管理器")]
  [LuaApiNotes("声音管理器，用于管理声音部分通用功能，支持快速使用名称播放一个声音。")]
  public class GameSoundManager : GameService
  {
    #region 初始化和定义

    public const string TAG = "GameSoundManager";

    public GameSoundManager() : base(TAG) { }

    private GameSettingsActuator GameSettings = null;
    private GamePackageManager GamePackageManager;
    private GameObject GameSoundManagerObject;

    [SLua.DoNotToLua]
    public override bool Initialize()
    {

      audioSourcePrefab = GameStaticResourcesPool.FindStaticPrefabs("AudioSource");
      fastPlayVoices = new Dictionary<string, AudioSource>();

      InitGameAudioMixer();

      GameManager.GameMediator.RegisterEventHandler(GamePackage.GetSystemPackage(), GameEventNames.EVENT_BASE_INIT_FINISHED, TAG, (e, p) =>
          {
            GamePackageManager = (GamePackageManager)GameSystem.GetSystemService("GamePackageManager");
            GameSoundManagerObject = CloneUtils.CreateEmptyObjectWithParent(GameManager.Instance.gameObject.transform, "GameSoundManager");
            InitCommands();
            return false;
          }
      );

      //设置更新事件
      GameSettings = GameSettingsManager.GetSettings("core");
      GameSettings.RegisterSettingsUpdateCallback("voice", OnVoiceSettingsUpdated);
      GameSettings.RequireSettingsLoad("voice");
      return true;
    }
    [SLua.DoNotToLua]
    public override void Destroy()
    {
      if (null != fastPlayVoices)
      {
        foreach (var v in fastPlayVoices)
          DestroySoundPlayer(v.Value);
        fastPlayVoices.Clear();
        fastPlayVoices = null;
      }
      if (null != audios)
      {
        for (int i = audios.Count - 1; i >= 0; i--)
          DestroySoundPlayer(audios[i].Audio);
        audios.Clear();
        audios = null;
      }
    }

    private List<AudioGlobalControl> audios = new List<AudioGlobalControl>();
    private GameObject audioSourcePrefab = null;
    private class AudioGlobalControl
    {
      public AudioSource Audio;
      public GameSoundType Type;
    }

    #endregion

    #region AudioMixer

    /// <summary>
    /// 游戏主AudioMixer
    /// </summary>
    [LuaApiDescription("游戏主AudioMixer")]
    public AudioMixer GameMainAudioMixer;
    /// <summary>
    /// 游戏UI AudioMix
    /// </summary>
    [LuaApiDescription("游戏UI AudioMixer")]
    public AudioMixer GameUIAudioMixer;

    private AudioMixerGroup GameUIAudioMixerGroupMaster;

    private AudioMixerGroup GameMainAudioMixerGroupMaster;
    private AudioMixerGroup GameMainAudioMixerGroupBallEffect;
    private AudioMixerGroup GameMainAudioMixerGroupModulEffect;
    private AudioMixerGroup GameMainAudioMixerGroupBackgroundMusic;

    private void InitGameAudioMixer()
    {
      GameMainAudioMixer = GameStaticResourcesPool.FindStaticAssets<AudioMixer>("GameMainAudioMixer");
      GameUIAudioMixer = GameStaticResourcesPool.FindStaticAssets<AudioMixer>("GameUIAudioMixer");

      GameUIAudioMixerGroupMaster = GameUIAudioMixer.FindMatchingGroups("Master")[0];
      GameMainAudioMixerGroupMaster = GameUIAudioMixer.FindMatchingGroups("Master")[0];
      GameMainAudioMixerGroupBallEffect = GameMainAudioMixer.FindMatchingGroups("Master/BallEffect")[0];
      GameMainAudioMixerGroupModulEffect = GameMainAudioMixer.FindMatchingGroups("Master/ModulEffect")[0];
      GameMainAudioMixerGroupBackgroundMusic = GameMainAudioMixer.FindMatchingGroups("Master/BackgroundMusic")[0];
    }

    #endregion

    #region Sound Player

    /// <summary>
    /// 加载模块中的音乐资源
    /// </summary>
    /// <param name="assets">资源路径（模块:音乐路径）</param>
    /// <returns>如果加载失败则返回null，否则返回AudioClip实例</returns>
    [LuaApiDescription("加载模块中的音乐资源", "如果加载失败则返回null，否则返回AudioClip实例")]
    [LuaApiParamDescription("assets", "资源路径（模块:音乐路径）")]
    public AudioClip LoadAudioResource(string assets)
    {
      string[] names = assets.Split(':');

      GamePackage package = GamePackageManager.FindPackage(names[0]);
      if (package == null)
      {
        Log.W(TAG, "无法加载声音文件 {0} ，因为未找到模块 {1}", assets, names[0]);
        GameErrorChecker.LastError = GameError.NotRegister;
        return null;
      }
      if (package.Status != GamePackageStatus.LoadSuccess)
      {
        Log.W(TAG, "无法加载声音文件 {0} ，因为模块 {1} 未初始化", assets, package.PackageName);
        GameErrorChecker.LastError = GameError.NotLoad;
        return null;
      }

      AudioClip clip = package.GetAsset<AudioClip>(names[1]);
      if (clip != null) clip.name = PathUtils.GetFileNameWithoutExt(names[1]);
      else
      {
        Log.W(TAG, "未找到声音文件 {0} ，在模块 {1}", assets, names[0]);
        GameErrorChecker.LastError = GameError.AssetNotFound;
      }

      return clip;
    }
    /// <summary>
    /// 加载模块中的音乐资源
    /// </summary>
    /// <param name="package">所属模块</param>
    /// <param name="assets">音乐路径</param>
    /// <returns>如果加载失败则返回null，否则返回AudioClip实例</returns>
    [LuaApiDescription("加载模块中的音乐资源", "如果加载失败则返回null，否则返回AudioClip实例")]
    [LuaApiParamDescription("package", "所属模块")]
    [LuaApiParamDescription("assets", "音乐路径")]
    public AudioClip LoadAudioResource(GamePackage package, string assets)
    {
      if (package == null)
      {
        Log.W(TAG, "无法加载声音文件 {0} ，因为未找到模块未提供");
        GameErrorChecker.LastError = GameError.ParamNotProvide;
        return null;
      }
      AudioClip clip = package.GetAsset<AudioClip>(assets);
      if (clip != null) clip.name = PathUtils.GetFileNameWithoutExt(assets);
      else
      {
        Log.W(TAG, "在模块 {1} 未找到声音文件 {0}", assets, package.PackageName);
        GameErrorChecker.LastError = GameError.AssetNotFound;
      }

      return clip;
    }
    /// <summary>
    /// 注册一个声音，并设置声音资源文件
    /// </summary>
    /// <param name="assets">音频资源字符串</param>
    /// <param name="playOnAwake">是否在开始时播放</param>
    /// <param name="activeStart">播放对象是否开始时激活</param>
    /// <param name="name">播放对象的名称</param>
    /// <returns>返回 AudioSource 实例</returns>
    [LuaApiDescription("注册一个声音，并设置声音资源文件", "返回 AudioSource 实例")]
    [LuaApiParamDescription("assets", "音频资源字符串")]
    [LuaApiParamDescription("playOnAwake", "是否在开始时播放")]
    [LuaApiParamDescription("activeStart", "播放对象是否开始时激活")]
    [LuaApiParamDescription("name", "播放对象的名称")]
    public AudioSource RegisterSoundPlayer(GameSoundType type, string assets, bool playOnAwake = false, bool activeStart = true, string name = "")
    {
      AudioClip audioClip = null;
      if (!string.IsNullOrEmpty(assets))
      {
        audioClip = LoadAudioResource(assets);
        if (audioClip == null)
          return null;
      }

      AudioSource audioSource = Object.Instantiate(audioSourcePrefab, GameSoundManagerObject.transform).GetComponent<AudioSource>();
      audioSource.clip = audioClip;
      audioSource.playOnAwake = playOnAwake;
      audioSource.gameObject.name = "AudioSource_" + type + "_" + (name == "" ?
          (audioClip != null ? PathUtils.GetFileNameWithoutExt(audioClip.name) : "") :
          name);

      if (!activeStart)
        audioSource.gameObject.SetActive(false);

      RegisterAudioSource(type, audioSource);
      return audioSource;
    }
    /// <summary>
    /// 注册一个声音，并设置声音资源文件
    /// </summary>
    /// <param name="audioClip">音频源文件</param>
    /// <param name="playOnAwake">是否在开始时播放</param>
    /// <param name="activeStart">播放对象是否开始时激活</param>
    /// <param name="name">播放对象的名称</param>
    /// <returns>返回 AudioSource 实例</returns>
    [LuaApiDescription("注册一个声音，并设置声音资源文件", "返回 AudioSource 实例")]
    [LuaApiParamDescription("audioClip", "音频源文件")]
    [LuaApiParamDescription("playOnAwake", "是否在开始时播放")]
    [LuaApiParamDescription("activeStart", "播放对象是否开始时激活")]
    [LuaApiParamDescription("name", "播放对象的名称")]
    public AudioSource RegisterSoundPlayer(GameSoundType type, AudioClip audioClip, bool playOnAwake = false, bool activeStart = true, string name = "")
    {
      AudioSource audioSource = Object.Instantiate(audioSourcePrefab, GameSoundManagerObject.transform).GetComponent<AudioSource>();
      audioSource.clip = audioClip;
      audioSource.playOnAwake = playOnAwake;
      audioSource.gameObject.name = "AudioSource_" + type + "_" + (name == "" ?
          (audioClip != null ? PathUtils.GetFileNameWithoutExt(audioClip.name) : "") :
          name);

      if (!activeStart)
        audioSource.gameObject.SetActive(false);

      RegisterAudioSource(type, audioSource);
      return audioSource;
    }
    /// <summary>
    /// 注册已有 AudioSource 至声音管理器
    /// </summary>
    /// <param name="type">声音所属类型</param>
    /// <param name="audioSource">AudioSource</param>
    /// <returns>返回原 AudioSource 实例</returns>
    [LuaApiDescription("注册已有 AudioSource 至声音管理器", "返回原 AudioSource 实例")]
    [LuaApiParamDescription("type", "声音所属类型")]
    [LuaApiParamDescription("audioSource", "AudioSource")]
    public AudioSource RegisterSoundPlayer(GameSoundType type, AudioSource audioSource)
    {
      if (!IsSoundPlayerRegistered(audioSource))
        RegisterAudioSource(type, audioSource);
      else GameErrorChecker.LastError = GameError.AlreadyRegistered;
      return audioSource;
    }
    /// <summary>
    /// 检查指定 AudioSource 是否已经注册至声音管理器
    /// </summary>
    /// <param name="audioSource">AudioSource</param>
    /// <returns>如果已经注册至声音管理器返回 true，否则返回 false</returns>
    [LuaApiDescription("检查指定 AudioSource 是否已经注册至声音管理器", "如果已经注册至声音管理器返回 true，否则返回 false")]
    [LuaApiParamDescription("audioSource", "AudioSource")]
    public bool IsSoundPlayerRegistered(AudioSource audioSource)
    {
      foreach (AudioGlobalControl a in audios)
      {
        if (a.Audio == audioSource)
          return true;
      }
      return false;
    }
    /// <summary>
    /// 销毁 AudioSource
    /// </summary>
    /// <param name="assets">要销毁的 AudioSource 实例</param>
    /// <returns>返回销毁是否成功</returns>
    [LuaApiDescription("销毁 AudioSource", "返回销毁是否成功")]
    [LuaApiParamDescription("audioSource", "要销毁的 AudioSource 实例")]
    public bool DestroySoundPlayer(AudioSource audioSource)
    {
      AudioGlobalControl audioGlobalControl = null;
      if (IsSoundPlayerRegistered(audioSource, out audioGlobalControl))
      {
        audios.Remove(audioGlobalControl);
        if (audioGlobalControl.Audio != null && audioGlobalControl.Audio.gameObject != null)
          Object.Destroy(audioGlobalControl.Audio.gameObject);
        return true;
      }
      else
      {
        GameErrorChecker.LastError = GameError.NotRegister;
        return false;
      }

    }

    private void RegisterAudioSource(GameSoundType type, AudioSource audioSource)
    {
      AudioGlobalControl audioGlobalControl = new AudioGlobalControl();
      audioGlobalControl.Audio = audioSource;
      audioGlobalControl.Type = type;

      switch (type)
      {
        case GameSoundType.Background:
          audioSource.outputAudioMixerGroup = GameMainAudioMixerGroupBackgroundMusic;
          break;
        case GameSoundType.BallEffect:
          audioSource.outputAudioMixerGroup = GameMainAudioMixerGroupBallEffect;
          break;
        case GameSoundType.ModulEffect:
          audioSource.outputAudioMixerGroup = GameMainAudioMixerGroupModulEffect;
          break;
        case GameSoundType.Normal:
          audioSource.outputAudioMixerGroup = GameMainAudioMixerGroupMaster;
          break;
        case GameSoundType.UI:
          audioSource.outputAudioMixerGroup = GameUIAudioMixerGroupMaster;
          break;
      }


      audios.Add(audioGlobalControl);
    }
    private bool IsSoundPlayerRegistered(AudioSource audioSource, out AudioGlobalControl audioGlobalControl)
    {
      foreach (AudioGlobalControl a in audios)
      {
        if (a.Audio == audioSource)
        {
          audioGlobalControl = a;
          return true;
        }
      }
      audioGlobalControl = null;
      return false;
    }

    #endregion

    #region 声音设置

    //加载声音设置
    private bool OnVoiceSettingsUpdated(string groupName, int action)
    {
      float volBackground = GameSettings.GetFloat("voice.background", 20);
      float volUI = GameSettings.GetFloat("voice.ui", 80);
      float volMain = GameSettings.GetFloat("voice.main", 100);

      GameUIAudioMixer.SetFloat("UIMasterVolume", volUI <= 1 ? -60 : (20.0f * Mathf.Log10(volUI / 100.0f)));
      GameMainAudioMixer.SetFloat("MasterVolume", volMain <= 1 ? -60 : (20.0f * Mathf.Log10(volMain / 100.0f)));
      GameMainAudioMixer.SetFloat("BackgroundVolume", volBackground <= 1 ? -60 : (20.0f * Mathf.Log10(volBackground / 100.0f)));

      return true;
    }

    private Dictionary<string, AudioSource> fastPlayVoices = null;

    #endregion

    #region 声音快速方法

    /// <summary>
    /// 快速播放一个短声音
    /// </summary>
    /// <param name="soundName">声音资源字符串</param>
    /// <param name="type">声音类型</param>
    /// <returns>返回播放是否成功</returns>
    [LuaApiDescription("快速播放一个短声音", "返回播放是否成功")]
    [LuaApiParamDescription("soundName", "声音资源字符串")]
    [LuaApiParamDescription("type", "声音类型")]
    public bool PlayFastVoice(string soundName, GameSoundType type)
    {
      return PlayFastVoice(null, soundName, type);
    }
    /// <summary>
    /// 快速播放一个指定模块包中的短声音资源
    /// </summary>
    /// <param name="package">所属模块</param>
    /// <param name="soundName">声音资源路径</param>
    /// <param name="type">声音类型</param>
    /// <returns>返回播放是否成功</returns>
    [LuaApiDescription("快速播放一个指定模块包中的短声音资源", "返回播放是否成功")]
    [LuaApiParamDescription("package", "所属模块")]
    [LuaApiParamDescription("soundName", "声音资源路径")]
    [LuaApiParamDescription("type", "声音类型")]
    public bool PlayFastVoice(GamePackage package, string soundName, GameSoundType type)
    {
      string key = soundName + "@" + type;
      AudioSource cache = null;
      if (fastPlayVoices.TryGetValue(key, out cache))
      {
        cache.Play();
        return true;
      }
      AudioClip audioClip = package == null ? LoadAudioResource(soundName) : LoadAudioResource(package, soundName);
      if (audioClip == null)
        return false;

      cache = RegisterSoundPlayer(type, audioClip, false, true, key);
      cache.maxDistance = 2000;
      cache.Play();
      fastPlayVoices[key] = cache;
      return true;
    }

    #endregion

    #region Command

    private void InitCommands()
    {
      var srv = GameManager.Instance.GameDebugCommandServer;
      srv.RegisterCommand("sm", (keyword, fullCmd, argsCount, args) =>
      {
        var type = (string)args[0];
        switch (type)
        {
          case "play":
            {
              string asset = "";
              GameSoundType stype;
              if (!DebugUtils.CheckDebugParam(1, args, out asset)) break;
              DebugUtils.CheckEnumDebugParam<GameSoundType>(2, args, out stype, false, GameSoundType.Normal);

              PlayFastVoice(args[1], stype);
              return true;
            }
          case "list":
            foreach (var i in audios)
              Log.V(TAG, string.Format("{0} => {1}, isPlaying: {2}, loop: {3}", i.Audio.gameObject.name, i.Type, i.Audio.isPlaying, i.Audio.loop));
            break;
        }
        return false;
      }, 1, "sm <play/list> 声音管理器命令\n" +
              "  play <asset:string> [soundType:GameSoundType] ▶ 播放一个音乐，asset路径格式为 “模块包名:音乐文件路径”；soundType指示音乐类型，默认为GameSoundType.Normal\n" +
              "  list                                          ▶ 列举出声音管理器管理的所有声音实例");
    }

    #endregion
  }

  /// <summary>
  /// 指定声音类型
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("指定声音类型")]
  public enum GameSoundType
  {
    /// <summary>
    /// 普通声音
    /// </summary>
    [LuaApiDescription("普通声音")]
    Normal,
    /// <summary>
    /// 游戏音效 关于球的
    /// </summary>
    [LuaApiDescription("游戏音效 关于球的")]
    BallEffect,
    /// <summary>
    /// 游戏音效 关于机关的
    /// </summary>
    [LuaApiDescription("游戏音效 关于机关的")]
    ModulEffect,
    /// <summary>
    /// UI 发出的声音
    /// </summary>
    [LuaApiDescription("UI 发出的声音")]
    UI,
    /// <summary>
    /// 背景音乐
    /// </summary>
    [LuaApiDescription("背景音乐")]
    Background,
  }
}

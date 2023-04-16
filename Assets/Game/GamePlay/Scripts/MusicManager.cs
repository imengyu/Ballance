using System.Collections.Generic;
using Ballance2.Base;
using Ballance2.Services;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.GamePlay {
  
  /// <summary>
  /// 背景音乐管理器，控制游戏中的背景音乐。
  /// </summary>
  public class MusicManager : MonoBehaviour 
  {
    private const string TAG = "MusicManager";

    public bool _CurrentIsAtmo = false;
    public bool  _CurrentForceAtmo = false;
    public float _CurrentAudioTick = 0;
    public float _LastMusicIndex = 0;
    private int _LastDisablerTimer = 0;
    private bool _CurrentAudioIsAtmo = false;

    public bool CurrentAudioEnabled { get; private set; }
    public AudioSource CurrentAudioSource { get; private set; }
    public MusicThemeDataStorage CurrentAudioTheme { get; private set; }
    public Dictionary<int, MusicThemeDataStorage> MusicThemes { get; private set; } 

    private void Start() {
      CurrentAudioSource = GameSoundManager.Instance.RegisterSoundPlayer(GameSoundType.Background, (AudioClip)null, false, true, "MusicManagerBackgroundMusic");
      CurrentAudioSource.maxDistance = 2000;
      MusicThemes = new Dictionary<int, MusicThemeDataStorage>();
      InitInternalMusics();
      InitEvents();
      InitCommand();
    }
    private void OnDestroy() {
      DestroyCommand();
      DestroyEvent();
    }

    //加载内置音乐
    private void InitInternalMusics() {
      List<AudioClip> atmos = new List<AudioClip>();
      atmos.Add(GameSoundManager.Instance.LoadAudioResource("core.sounds.music:Music_Atmo_1.wav"));
      atmos.Add(GameSoundManager.Instance.LoadAudioResource("core.sounds.music:Music_Atmo_2.wav"));
      atmos.Add(GameSoundManager.Instance.LoadAudioResource("core.sounds.music:Music_Atmo_3.wav"));
    
      for (var i = 1; i <= 5; i++)
      {
        MusicThemeDataStorage musicItem = new MusicThemeDataStorage();
        musicItem.atmos = atmos;
        musicItem.baseInterval = 20;
        musicItem.maxInterval = 30;
        musicItem.atmoInterval = 10;
        musicItem.atmoMaxInterval = 20;
        List<AudioClip> musics = new List<AudioClip>();
        musics.Add(GameSoundManager.Instance.LoadAudioResource("core.sounds.music:Music_Theme_" + i  + "_1.wav"));
        musics.Add(GameSoundManager.Instance.LoadAudioResource("core.sounds.music:Music_Theme_" + i  + "_2.wav"));
        musics.Add(GameSoundManager.Instance.LoadAudioResource("core.sounds.music:Music_Theme_" + i  + "_3.wav"));
        musicItem.musics = musics;
        MusicThemes[i] = musicItem;
      }
    
    }

    public class MusicThemeDataStorage {
      public int id = 0;
      public float maxInterval = 5;
      public float baseInterval = 30;
      public float atmoInterval = 6;
      public float atmoMaxInterval = 15;
      public List<AudioClip> musics = null;
      public List<AudioClip> atmos = null;
    }
  
    #region 事件

    /// <summary>
    /// 音乐主题变化事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventMusicThemeChanged;
    /// <summary>
    /// 音乐禁用事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventMusicDisable;
    /// <summary>
    /// 音乐启用事件
    /// </summary>
    [HideInInspector]
    public GameEventEmitterStorage EventMusicEnable;

    private void InitEvents() {
      var events = GameMediator.Instance.RegisterEventEmitter("MusicManager");
      this.EventMusicThemeChanged = events.RegisterEvent("MusicThemeChanged");
      this.EventMusicDisable = events.RegisterEvent("MusicDisable");
      this.EventMusicEnable = events.RegisterEvent("MusicEnable");
    }
    private void DestroyEvent() {
      GameMediator.Instance.UnRegisterEventEmitter("MusicManager");
    }

    #endregion

    #region 指令

    private int _CommandId = 0;
    private void InitCommand() {
      this._CommandId = GameManager.Instance.GameDebugCommandServer.RegisterCommand("bgm", (eyword, fullCmd, argsCount, args) => {
        var type = args[0];
        if (type == "enable")
          this.EnableBackgroundMusic();
        else if (type == "disable")
          this.DisableBackgroundMusic();
        return true;
      }, 1, "bgm <enable/disable> 背景音乐管理器命令" + 
          "  enable  ▶ 开启背景音乐" + 
          "  disable ▶ 关闭背景音乐"
      );
    }
    private void DestroyCommand() {
      GameManager.Instance.GameDebugCommandServer.UnRegisterCommand(this._CommandId);
    }
  
    #endregion

    #region 控制

    private void FixedUpdate() 
    {
      if (CurrentAudioEnabled && CurrentAudioTheme != null) 
      {
        var musicsCount = CurrentAudioTheme.musics.Count;
        //随机时间播放随机音乐
        if (_CurrentAudioTick > 0)
          _CurrentAudioTick -= Time.fixedDeltaTime;
        else
        {
          if (_CurrentIsAtmo || _CurrentForceAtmo) 
          {
            //随机播放atmo音效
            _CurrentIsAtmo = false;
            if (!CurrentAudioSource.isPlaying)
            {
              CurrentAudioSource.clip = CurrentAudioTheme.atmos[Random.Range(0, musicsCount)];
              CurrentAudioSource.volume = Random.Range(0.5f, 1);
              CurrentAudioSource.Play();
              _CurrentAudioIsAtmo = true;
            }
            _CurrentAudioTick = CurrentAudioSource.clip.length + Random.Range(0, 2);
          }
          else
          {
            _CurrentIsAtmo = Random.value > 0.3f;
            var musicIndex = Random.Range(0, musicsCount);
            if (musicIndex == _LastMusicIndex) 
            {
              if (musicIndex < musicsCount - 1)
                musicIndex++;
              else
                musicIndex = 0;
            }
            _LastMusicIndex = musicIndex;
            if (_CurrentAudioIsAtmo || !CurrentAudioSource.isPlaying)
            {
              CurrentAudioSource.Stop() ;
              CurrentAudioSource.clip = CurrentAudioTheme.musics[musicIndex];
              CurrentAudioSource.volume = 1;
              CurrentAudioSource.Play();
              _CurrentAudioIsAtmo = false;
            }
            if (_CurrentIsAtmo)
              _CurrentAudioTick = Random.Range(CurrentAudioTheme.atmoInterval, CurrentAudioTheme.atmoMaxInterval);
            else
              _CurrentAudioTick = Random.Range(CurrentAudioTheme.baseInterval, CurrentAudioTheme.maxInterval);
          }
        }
      }
    }

    /// <summary>
    /// 设置当前背景音乐预设
    /// </summary>
    /// <param name="theme"></param>
    public bool SetCurrentTheme(int theme) 
    {
      if (this.MusicThemes.ContainsKey(theme))
      {
        this.CurrentAudioTheme = this.MusicThemes[theme] ;
        this.EventMusicThemeChanged.Emit(theme);
        return true;
      }
      else
      {
        if (theme != 0) 
          Log.E(TAG, $"Not found music theme {theme} , music disabled");
        this.CurrentAudioEnabled = false;
        this.EventMusicDisable.Emit(null);
        return false;
      }
    }
    /// <summary>
    /// 开启音乐
    /// </summary>
    public void EnableBackgroundMusic() 
    {
      if (_LastDisablerTimer > 0) {
        GameTimer.DeleteDelay(_LastDisablerTimer);
        _LastDisablerTimer = 0;
      }
      this._CurrentForceAtmo = false;
      this.CurrentAudioEnabled = true;
      if (this.CurrentAudioTheme != null) 
      {
        this._CurrentAudioTick = Random.Range(2, this.CurrentAudioTheme.maxInterval / 3);

        ///淡入当前正在播放的音乐
        if (this.CurrentAudioSource.isPlaying)
          GameUIManager.Instance.UIFadeManager.AddAudioFadeIn(this.CurrentAudioSource, 1);
        else
          this.CurrentAudioSource.volume = 1;
        this.EventMusicEnable.Emit(null);
      }
    }
    /// <summary>
    /// 暂停音乐
    /// </summary>
    /// <param name="fast">是否快速停止（没有渐变）</param>
    public void DisableBackgroundMusic(bool fast = false) 
    {
      if (_LastDisablerTimer > 0) {
        GameTimer.DeleteDelay(_LastDisablerTimer);
        _LastDisablerTimer = 0;
      }
      this.CurrentAudioEnabled = false;
      if (fast) {
        this.CurrentAudioSource.volume = 0;
        this.CurrentAudioSource.Stop();
      }
      else {
        ///淡出当前正在播放的音乐
        if (this.CurrentAudioSource.isPlaying)
          GameUIManager.Instance.UIFadeManager.AddAudioFadeOut(this.CurrentAudioSource, 1);
        else
          this.CurrentAudioSource.volume = 1;
      }
      this.EventMusicDisable.Emit(null);
    }
    /// <summary>
    /// 暂停音乐（Atmo除外）
    /// </summary>
    public void DisableBackgroundMusicWithoutAtmo() 
    {
      this._CurrentForceAtmo = true;
      if (!this._CurrentIsAtmo && this.CurrentAudioSource.isPlaying)
        GameUIManager.Instance.UIFadeManager.AddAudioFadeOut(this.CurrentAudioSource, 1);
    }
    /// <summary>
    /// 从当前时间开始暂停音乐指定秒
    /// </summary>
    public void DisableInSec(float sec) 
    {
      if (sec <= 0 || !this.CurrentAudioEnabled)
        return;
      //淡出当前正在播放的音乐
      if (this.CurrentAudioSource.isPlaying)
        GameUIManager.Instance.UIFadeManager.AddAudioFadeOut(this.CurrentAudioSource, 1);

      if (_LastDisablerTimer > 0) {
        GameTimer.DeleteDelay(_LastDisablerTimer);
        _LastDisablerTimer = 0;
      }

      this.CurrentAudioEnabled = false;
      GameTimer.Delay(1000, () => {
        this.CurrentAudioSource.Stop();
        this.EventMusicDisable.Emit(null);
      });
      //定时开启
      _LastDisablerTimer = GameTimer.Delay(sec*1000, () => {
        this._LastDisablerTimer = 0;
        this.CurrentAudioEnabled = true;
        this.EventMusicEnable.Emit(null);
      });
    }
    /// <summary>
    /// 删除音乐主题
    /// </summary>
    /// <param name="theme">主题ID</param>
    public bool DeleteMusicTheme(int theme) {
      return MusicThemes.Remove(theme);
    }
    /// <summary>
    /// 添加音乐主题
    /// </summary>
    /// <param name="theme">主题</param>
    /// <param name="themeId">主题ID</param>
    public void AddMusicTheme(int themeId, MusicThemeDataStorage theme) {
      if (MusicThemes.ContainsKey(themeId))
        MusicThemes[themeId] = theme;
      else
        MusicThemes.Add(themeId, theme);
    }
    
    #endregion
  }
}
using System.Collections;
using System.Collections.Generic;
using Ballance2.Base;
using Ballance2.Game.GamePlay.Balls;
using Ballance2.Game.GamePlay.Other;
using Ballance2.Game.GamePlay.Tranfo;
using Ballance2.Game.LevelBuilder;
using Ballance2.Game.Utils;
using Ballance2.Menu;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Utils;
using BallancePhysics.Wapper;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Ballance2.Game.GamePlay
{
    public class GamePlayManager : GameSingletonBehavior<GamePlayManager>
    {
        private const string TAG = "GamePlayManager";

        #region 公共属性

        /// <summary>
        /// 当前关卡的初始生命球
        /// </summary>
        public int StartLife = 3;
        /// <summary>
        /// 当前关卡的初始时间点数
        /// </summary>
        public int StartPoint = 1000;
        /// <summary>
        /// 当前关卡的基础分数
        /// </summary>
        public int LevelScore = 100;
        /// <summary>
        /// 当前关卡的初始球名称
        /// </summary>
        public string StartBall = "BallWood";
        /// <summary>
        /// 当前关卡的下一关名称，为空表示没有下一关
        /// </summary>
        public string NextLevelName = "";
        /// <summary>
        /// 物理环境
        /// </summary>
        public PhysicsEnvironment GamePhysicsWorld { get; private set; }
        /// <summary>
        /// 摄像机管理器
        /// </summary>
        public CamManager CamManager;
        /// <summary>
        /// 球管理器
        /// </summary>
        public BallManager BallManager;
        /// <summary>
        /// 节管理器
        /// </summary>
        public SectorManager SectorManager;
        /// <summary>
        /// 背景音乐管理器
        /// </summary>
        public MusicManager MusicManager;
        /// <summary>
        /// 球声音管理器
        /// </summary>
        public BallSoundManager BallSoundManager;
        /// <summary>
        /// 球声音管理器
        /// </summary>
        public LevelBuilder.LevelBuilder LevelBuilderRef
        {
            get { return LevelBuilder.LevelBuilder.Instance; }
        }

        /// <summary>
        /// 当前关卡的名称
        /// </summary>
        public string CurrentLevelName = "";
        /// <summary>
        /// 当前时间点数
        /// </summary>
        public int CurrentPoint = 0;
        /// <summary>
        /// 当前生命数
        /// </summary>
        public int CurrentLife = 0;
        /// <summary>
        /// 当前小节
        /// </summary>
        public int CurrentSector = 0;
        /// <summary>
        /// 获取是否过关
        /// </summary>
        public bool CurrentLevelPass { get; private set; } = false;
        public bool CurrentDisableStart = false;
        public bool CurrentEndWithUFO = false;
        public bool CanEscPause = true;

        #endregion

        internal bool _IsGamePlaying = false;
        internal bool _IsCountDownPoint = false;
        internal bool _BallBirthed = false;
        internal bool _FirstStart = false;

        private float _UpdateTick = 0;
        private List<int> _CommandIds = new List<int>();
        private int _HideBalloonEndTimerID = 0;
        //Used by Tutorial
        internal bool _ShouldStartByCustom = false;
        private int[] _EscKeyIds = new int[0];

        private void Awake()
        {
            var Mediator = GameManager.GameMediator;

            _InitPhysicsWorld();
            _InitSounds();
            _InitKeyEvents();
            _InitSettings();
            _InitEvents();

            //注册全局事件
            Mediator.SubscribeSingleEvent(GamePackage.GetSystemPackage(), "CoreGamePlayManagerInitAndStart", "GamePlayManager", (evtName, param) =>
            {
                this._InitAndStart();
                return false;
            });

            //注册控制台指令
            this._AddCommands();
        }
        protected override void OnDestroy()
        {
            Log.D(TAG, "Destroy");
            if (GameUIManager.Instance != null)
                foreach (var id in _EscKeyIds)
                    GameUIManager.Instance.DeleteKeyListen(id);
            GameManager.GameMediator?.UnRegisterSingleEvent("CoreGamePlayManagerInitAndStart"); //取消注册全局事件
            this._DeleteEvents();
            this._DeleteCommands(); //删除指令
        }
        private void Update()
        {
            this._UpdateTick = this._UpdateTick + Time.deltaTime;
            if (this._UpdateTick >= 0.5)
            {
                //计数清零
                this._UpdateTick = 0;
                //分数每半秒减一
                if (this._IsCountDownPoint && this.CurrentPoint > 0)
                {
                    this.CurrentPoint = this.CurrentPoint - 1;
                    GamePlayUIControl.Instance.SetPointText(this.CurrentPoint);
                }
            }
        }

        #region Command

        private void _AddCommands()
        {
            var GameDebugCommandServer = GameManager.Instance.GameDebugCommandServer;
            //注册控制台指令

            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("pass", (keyword, fullCmd, argsCount, args) =>
            {
                this.Pass();
                return true;
            }, 0, "win > 直接过关"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("debug0", (keyword, fullCmd, argsCount, args) =>
            {
                HighscoreManager.Instance.AddItem(CurrentLevelName, "debug0", 5000);
                return true;
            }, 0, "debug0 > Add highscroll"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("fall", (keyword, fullCmd, argsCount, args) =>
            {
                this.Fall();
                return true;
            }, 0, "fall > 触发球掉落死亡"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("restart", (keyword, fullCmd, argsCount, args) =>
            {
                this.RestartLevel();
                return true;
            }, 0, "restart > 重新开始关卡"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("pause", (keyword, fullCmd, argsCount, args) =>
            {
                this.PauseLevel(false);
                return true;
            }, 0, "pause > 暂停"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("resume", (keyword, fullCmd, argsCount, args) =>
            {
                this.ResumeLevel();
                return true;
            }, 0, "resume > 恢复"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("unload", (keyword, fullCmd, argsCount, args) =>
            {
                this.QuitLevel();
                return true;
            }, 0, "unload > 卸载关卡"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("nextlev", (keyword, fullCmd, argsCount, args) =>
            {
                this.Fall();
                return true;
            }, 0, "nextlev > 加载下一关"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("gos", (keyword, fullCmd, argsCount, args) =>
            {
                var ox = DebugUtils.CheckIntDebugParam(0, args, out var nx, true, 0);
                if (!ox)
                    return false;
                BallManager.SetControllingStatus(BallControlStatus.NoControl);
                SectorManager.SetCurrentSector(nx);
                this._SetCamPos();
                this._Start(true, null);
                return true;
            }, 1, "gos <count:number> > 跳转到指定的小节"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("rebirth", (keyword, fullCmd, argsCount, args) =>
            {
                this._Rebirth();
                return true;
            }, 0, "rebirth > 重新出生（不消耗生命球）"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("addlife", (keyword, fullCmd, argsCount, args) =>
            {
                this.AddLife();
                return true;
            }, 0, "addlife > 添加一个生命球"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("addtime", (keyword, fullCmd, argsCount, args) =>
            {
                var ox = DebugUtils.CheckIntDebugParam(0, args, out var nx, true, 0);
                if (!ox)
                    return false;
                this.AddPoint(nx);
                return true;
            }, 1, "addtime <count:number> > 添加时间点 count：要添加数量"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("settime", (keyword, fullCmd, argsCount, args) =>
            {
                var ox = DebugUtils.CheckIntDebugParam(0, args, out var nx, true, 0);
                if (!ox)
                    return false;
                if (nx < 0) nx = 0;
                this.CurrentPoint = nx;
                GamePlayUIControl.Instance.SetPointText(nx);
                return true;
            }, 1, "settime <count:number> > 设置当前时间点数量 count：数量"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("set-physics-speed", (keyword, fullCmd, argsCount, args) =>
            {
                var ox = DebugUtils.CheckFloatDebugParam(0, args, out var nx, true, 0);
                if (!ox)
                    return false;
                if (nx < 0.5) nx = 0.5f;
                if (nx > 5) nx = 5;
                GamePhysicsWorld.TimeFactor = nx;
                return true;
            }, 1, "set-physics-speed <speed:number> > 设置物理引擎模拟速率 speed：速率，默认是 1，可以设置 0.5 - 5.0"));
            this._CommandIds.Add(GameDebugCommandServer.RegisterCommand("set-ball-speed", (keyword, fullCmd, argsCount, args) =>
            {
                var ox = DebugUtils.CheckFloatDebugParam(0, args, out var nx, true, 0);
                if (!ox)
                    return false;
                if (nx < 0.1f) nx = 0.1f;
                if (nx > 10) nx = 10;
                BallManager.BallSpeedFactor = nx;
                BallManager.FlushBallPush();
                return true;
            }, 1, "set-ball-speed <count:number> > 设置倍速球 speed：速率，默认是 1，可以设置 0.1 - 10.0"));
        }
        private void _DeleteCommands()
        {
            var GameDebugCommandServer = GameManager.Instance.GameDebugCommandServer;
            _CommandIds.ForEach((id) => GameDebugCommandServer.UnRegisterCommand(id));
            _CommandIds.Clear();
        }

        #endregion

        #region 初始化

        private AudioSource _SoundBallFall;
        private AudioSource _SoundAddLife;
        private AudioSource _SoundLastSector;
        private AudioSource _SoundFinnal;
        private AudioSource _SoundLastFinnal;

        internal AudioSource GetSoundLastSector() { return _SoundLastSector; }

        private void _InitSounds()
        {
            var SoundManager = GameSoundManager.Instance;
            this._SoundBallFall = SoundManager.RegisterSoundPlayer(GameSoundType.Normal, SoundManager.LoadAudioResource("core.sounds:Misc_Fall.wav"), false, true, "Misc_Fall");
            this._SoundAddLife = SoundManager.RegisterSoundPlayer(GameSoundType.UI, SoundManager.LoadAudioResource("core.sounds:Misc_extraball.wav"), false, true, "Misc_extraball");
            this._SoundLastSector = SoundManager.RegisterSoundPlayer(GameSoundType.Background, SoundManager.LoadAudioResource("core.sounds.music:Music_EndCheckpoint.wav"), false, true, "Music_EndCheckpoint");
            this._SoundFinnal = SoundManager.RegisterSoundPlayer(GameSoundType.Normal, SoundManager.LoadAudioResource("core.sounds.music:Music_Final.wav"), false, true, "Music_Final");
            this._SoundLastFinnal = SoundManager.RegisterSoundPlayer(GameSoundType.Normal, SoundManager.LoadAudioResource("core.sounds.music:Music_LastFinal.wav"), false, true, "Music_LastFinal");
            this._SoundLastSector.loop = true;
            this._SoundLastSector.dopplerLevel = 0;
            this._SoundLastSector.rolloffMode = AudioRolloffMode.Linear;
            this._SoundLastSector.minDistance = 95;
            this._SoundLastSector.maxDistance = 160;
        }
        private void _InitKeyEvents()
        {
            _EscKeyIds = new int[2];
            //ESC键
            _EscKeyIds[0] = GameUIManager.Instance.ListenKey(KeyCode.Escape, (key, down) =>
            {
                if (down
                  && this.CanEscPause
                  && this._BallBirthed
                  && !this.CurrentLevelPass)
                {
                    if (this._IsGamePlaying)
                        this.PauseLevel(true);
                    else
                        this.ResumeLevel();
                }
            });
            //手柄菜单键
            _EscKeyIds[1] = GameUIManager.Instance.ListenKey(KeyCode.Joystick1Button7, (key, down) =>
            {
                if (down
                  && this.CanEscPause
                  && this._BallBirthed
                  && !this.CurrentLevelPass)
                {
                    if (this._IsGamePlaying)
                        this.PauseLevel(true);
                    else
                        this.ResumeLevel();
                }
            });
        }
        private void _InitSettings()
        {
            var GameSettings = GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME);
            var GameLevelBuilder = LevelBuilderRef;
            GameSettings.RegisterSettingsUpdateCallback(SettingConstants.SettingsVideoCloud, (groupName, action) =>
            {
                if (GameLevelBuilder.CurrentLevelSkyLayer != null)
                {
                    if (GameSettings.GetBool(SettingConstants.SettingsVideoCloud))
                        GameLevelBuilder.CurrentLevelSkyLayer.SetActive(true);
                    else
                        GameLevelBuilder.CurrentLevelSkyLayer.SetActive(false);
                }
                return false;
            });
        }
        private void _InitPhysicsWorld()
        {
            if (GamePhysicsWorld == null)
            {
                GamePhysicsWorld = gameObject.AddComponent<PhysicsEnvironment>();
                GamePhysicsWorld.AutoCreate = false;

                //读取物理环境配置
                var dataAsset = Resources.Load<TextAsset>("GamePhysData");
                if (dataAsset != null)
                {
                    JObject o = (JObject)JToken.Parse(dataAsset.text);
                    JToken Gravity = o["Gravity"];
                    JToken SimulationRate = o["SimulationRate"];
                    JToken TimeFactor = o["TimeFactor"];
                    if (Gravity != null)
                        GamePhysicsWorld.Gravity = Gravity.ToObject<Vector3>();
                    if (SimulationRate != null)
                        GamePhysicsWorld.SimulationRate = (SimulationRate.Value<int>());
                    if (TimeFactor != null)
                        GamePhysicsWorld.TimeFactor = (TimeFactor.Value<float>());
                }
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 关卡开始之前事件
        /// </summary>
        [HideInInspector]
        public GameEventEmitterStorage EventBeforeStart;
        /// <summary>
        /// 关卡开始事件
        /// </summary>
        [HideInInspector]
        public GameEventEmitterStorage EventStart;
        /// <summary>
        /// 关卡退出事件
        /// </summary>
        [HideInInspector]
        public GameEventEmitterStorage EventQuit;
        /// <summary>
        /// 玩家球掉落事件
        /// </summary>
        [HideInInspector]
        public GameEventEmitterStorage EventFall;
        /// <summary>
        /// 关卡球掉落并且没有生命游戏结束事件（不会发出Fall）
        /// </summary>
        [HideInInspector]
        public GameEventEmitterStorage EventDeath;
        /// <summary>
        /// 继续事件
        /// </summary>
        [HideInInspector]
        public GameEventEmitterStorage EventResume;
        /// <summary>
        /// 暂停事件
        /// </summary>
        [HideInInspector]
        public GameEventEmitterStorage EventPause;
        /// <summary>
        /// 重新开始关卡事件
        /// </summary>
        [HideInInspector]
        public GameEventEmitterStorage EventRestart;
        /// <summary>
        /// UFO动画完成事件
        /// </summary>
        [HideInInspector]
        public GameEventEmitterStorage EventUfoAnimFinish;
        /// <summary>
        /// 过关事件
        /// </summary>
        [HideInInspector]
        public GameEventEmitterStorage EventPass;
        /// <summary>
        /// 过关后飞船隐藏事件
        /// </summary>
        [HideInInspector]
        public GameEventEmitterStorage EventHideBalloonEnd;
        /// <summary>
        /// 生命增加事件
        /// </summary>
        [HideInInspector]
        public GameEventEmitterStorage EventAddLife;
        /// <summary>
        /// 分数增加事件
        /// </summary>
        [HideInInspector]
        public GameEventEmitterStorage EventAddPoint;

        private void _InitEvents()
        {
            var events = GameManager.GameMediator.RegisterEventEmitter("GamePlay");
            this.EventBeforeStart = events.RegisterEvent("BeforeStart");
            this.EventStart = events.RegisterEvent("Start");
            this.EventQuit = events.RegisterEvent("Quit");
            this.EventFall = events.RegisterEvent("Fall");
            this.EventDeath = events.RegisterEvent("Death");
            this.EventResume = events.RegisterEvent("Resume");
            this.EventPause = events.RegisterEvent("Pause");
            this.EventRestart = events.RegisterEvent("Restart");
            this.EventUfoAnimFinish = events.RegisterEvent("UfoAnimFinish");
            this.EventPass = events.RegisterEvent("Pass");
            this.EventHideBalloonEnd = events.RegisterEvent("HideBalloonEnd");
            this.EventAddLife = events.RegisterEvent("AddLife");
            this.EventAddPoint = events.RegisterEvent("AddPoint");
        }
        private void _DeleteEvents()
        {
            GameManager.GameMediator.UnRegisterEventEmitter("GamePlay");
        }

        #endregion

        #region 游戏流程控制

        //停止
        internal void _Stop(BallControlStatus controlStatus)
        {
            this._IsGamePlaying = false;
            this._IsCountDownPoint = false;
            //禁用控制
            BallManager.SetControllingStatus(controlStatus);
            //禁用音乐
            MusicManager.DisableBackgroundMusic();
        }
        //开始
        internal void _Start(bool isStartBySector, GameManager.VoidDelegate customerFn)
        {
            this._IsGamePlaying = true;

            if (this.CurrentDisableStart)
                return;

            //开始音乐
            MusicManager.EnableBackgroundMusic();

            if (!this._BallBirthed)
                this._BallBirthed = true;

            if (isStartBySector)
            {
                var startPos = Vector3.zero;
                if (this.CurrentSector > 0)
                {
                    //初始位置
                    var startRestPoint = SectorManager.CurrentLevelRestPoints[this.CurrentSector].point;
                    startPos = startRestPoint.transform.position;
                }
                BallManager.SetCanControlCameraWhenStart();
                //等待闪电完成
                BallManager.PlayLighting(startPos, true, true, () =>
                {
                    //开始控制
                    BallManager.SetNextRecoverPos(startPos);
                    //发送事件
                    this.EventStart.Emit(this._FirstStart);
                    //设置标志
                    if (this._FirstStart)
                        this._FirstStart = false;

                    if (customerFn != null)
                        customerFn();
                    else
                    {
                        BallManager.SetControllingStatus(BallControlStatus.Control);
                        this._IsCountDownPoint = true;
                    }
                });
            }
            else
            {
                this._IsCountDownPoint = true;
                BallManager.SetControllingStatus(BallControlStatus.Control);
            }
        }
        //设置相机位置
        internal void _SetCamPos()
        {
            if (this.CurrentSector > 0)
            {
                var startRestPoint = SectorManager.CurrentLevelRestPoints[this.CurrentSector].point;
                CamManager.SetPosAndDirByRestPoint(startRestPoint);
                CamManager.SetTarget(startRestPoint.transform, false);
                CamManager.SetCamLook(true);
            }
        }
        //重新出生（不消耗生命球）
        internal void _Rebirth()
        {
            BallManager.SetControllingStatus(BallControlStatus.NoControl);
            this._SetCamPos();
            this._Start(true, null);
        }
        //LevelBuilder 就绪，现在GamePlayManager进行初始化
        internal void _InitAndStart()
        {
            this.CurrentLevelPass = false;
            this.CurrentDisableStart = false;
            this._IsGamePlaying = false;
            this._IsCountDownPoint = false;

            //UI
            GameUIManager.Instance.CloseAllPage();
            GamePlayUIControl.Instance.gameObject.SetActive(true);
            //设置初始分数\生命球
            this.CurrentLife = this.StartLife;
            this.CurrentPoint = this.StartPoint;
            this._BallBirthed = false;
            GamePlayUIControl.Instance.SetLifeBallCount(this.CurrentLife);
            GamePlayUIControl.Instance.SetPointText(this.CurrentPoint);
            //进入第一小节
            SectorManager.SetCurrentSector(1);
            //设置初始球
            BallManager.SetCurrentBall(this.StartBall);
            BallManager.CanControllCamera = true;
            CamManager.gameObject.SetActive(true);
            this._SetCamPos();
            GameUIManager.Instance.MaskBlackFadeOut(1);
            //播放开始音乐
            GameSoundManager.Instance.PlayFastVoice("core.sounds:Misc_StartLevel.wav", GameSoundType.Normal);
            //可控制摄像机了
            BallManager.CanControllCamera = true;
            //发出事件
            LevelBuilderRef.CallLevelCustomModEvent("beforeStart");
            //显示云层 (用于过关之后重新开始，因为之前过关的时候隐藏了云层)
            if (GameManager.Instance.GameSettings.GetBool(SettingConstants.SettingsVideoCloud))
            {
                if (LevelBuilderRef.CurrentLevelSkyLayer && !LevelBuilderRef.CurrentLevelSkyLayer.activeSelf)
                    GameUIManager.Instance.UIFadeManager.AddFadeIn(LevelBuilderRef.CurrentLevelSkyLayer, 1, null);
            }

            this._FirstStart = true;

            Log.D(TAG, "Start");

            this.EventBeforeStart.Emit();

            if (!this._ShouldStartByCustom)
            {
                GameTimer.Delay(0.7f, () =>
                {
                    //模拟
                    this.GamePhysicsWorld.Simulate = true;
                    //开始
                    this._Start(true, null);
                });
            }
            else
                Log.D(TAG, "Should Start By Custom");
        }

        private bool _DethLock = false;

        /// <summary>
        /// 触发球坠落
        /// </summary>
        public bool Fall()
        {
            if (this.CurrentLevelPass)
                return false;
            if (this.BallManager.GetControllingStatus() != BallControlStatus.Control)
                return false;
            if (this._DethLock)
                return false;
            this._DethLock = true;

            Log.D(TAG, "Fall . CurrentLife: " + this.CurrentLife);

            //下落音乐
            this._SoundBallFall.volume = 1;
            this._SoundBallFall.Play();

            //禁用键盘摄像机控制
            BallManager.CanControllCamera = false;

            if (this.CurrentLife > 0 || this.CurrentLife <= -1)
            {
                //禁用控制
                this._Stop(BallControlStatus.FreeMode);

                if (this.CurrentLife > 0)
                    this.CurrentLife = this.CurrentLife - 1;

                GameUIManager.Instance.MaskWhiteFadeIn(1);

                this.EventFall.Emit();

                GameTimer.Delay(1, () =>
                {
                    //禁用机关
                    SectorManager.DeactiveCurrentSector();
                    //禁用控制
                    this._Stop(BallControlStatus.NoControl);

                    GameTimer.Delay(1, () =>
                    {
                        GameUIManager.Instance.UIFadeManager.AddAudioFadeOut(this._SoundBallFall, 1);

                        ///重置机关和摄像机
                        SectorManager.ActiveCurrentSector(false);
                        this._SetCamPos();
                        this._Start(true, null);
                        GameUIManager.Instance.MaskWhiteFadeOut(1);

                        //延时移除生命球
                        GameTimer.Delay(1, () =>
                        {
                            GamePlayUIControl.Instance.RemoveLifeBall();
                            this._DethLock = false;
                        });
                    });
                });
            }
            else
            {
                Log.D(TAG, "Death");

                //禁用控制
                this._Stop(BallControlStatus.FreeMode);

                this.EventDeath.Emit();

                GameTimer.Delay(1.5f, () =>
                {
                    this._Stop(BallControlStatus.UnleashingMode);
                    MusicManager.DisableBackgroundMusicWithoutAtmo();

                    //延时显示失败菜单
                    GameTimer.Delay(1, () =>
                    {
                        GameUIManager.Instance.GoPage("PageGameFail");
                        this._DethLock = false;
                    });
                });
            }
            return true;
        }
        /// <summary>
        /// 触发过关
        /// </summary>
        public void Pass()
        {
            if (this.CurrentLevelPass)
                return;

            Log.D(TAG, "Pass");

            this.CurrentLevelPass = true;
            this._SoundLastSector.Stop(); //停止最后一小节的音乐
            this._Stop(BallControlStatus.FreeMode);

            GameTimer.Delay(0.6f, () => BallManager.SetControllingStatus(BallControlStatus.UnleashingMode));

            //禁用键盘摄像机控制
            BallManager.CanControllCamera = false;

            //过关后马上渐变淡出云层，因为摄像机要放平看到边界的地方了
            if (LevelBuilderRef.CurrentLevelSkyLayer && LevelBuilderRef.CurrentLevelSkyLayer.activeSelf)
                GameUIManager.Instance.UIFadeManager.AddFadeOut(LevelBuilderRef.CurrentLevelSkyLayer, 5, true, null);

            //停止背景音乐
            MusicManager.DisableBackgroundMusicWithoutAtmo();

            if (this.CurrentEndWithUFO)
            {
                //播放结尾的UFO动画
                this._SoundLastFinnal.Play(); //播放音乐
                UFOAnimController.Instance.StartSeq();
            }
            else
            {
                this._SoundFinnal.Play(); //播放音乐
                this._HideBalloonEnd(false); //开始隐藏飞船
                GameTimer.Delay(6, () =>
                {
                    WinScoreUIControl.Instance.StartSeq();
                });
            }
            this.EventPass.Emit();
        }
        /// <summary>
        /// 暂停关卡
        /// </summary>
        /// <param name="showPauseUI">是否显示暂停界面</param>
        public void PauseLevel(bool showPauseUI)
        {
            this._Stop(BallControlStatus.FreeMode);

            Log.D(TAG, "Pause");

            //停止模拟
            this.GamePhysicsWorld.Simulate = false;
            //停止摄像机跟随(防止暂停而带来的摄像机抖动)
            BallManager.StopCamMove();

            //UI
            if (showPauseUI)
            {
                GameSoundManager.Instance.PlayFastVoice("core.sounds:Menu_click.wav", GameSoundType.UI);
                GameUIManager.Instance.GoPage("PageGamePause");
            }

            this.EventPause.Emit();
        }
        /// <summary>
        /// 继续关卡
        /// </summary>
        /// <param name="forceRestart">是否强制重置，会造成当前小节重置，默认false</param>
        public void ResumeLevel(bool forceRestart = false)
        {
            Log.D(TAG, "Resume");

            //UI
            GameSoundManager.Instance.PlayFastVoice("core.sounds:Menu_click.wav", GameSoundType.UI);
            GameUIManager.Instance.CloseAllPage();

            //重新开始摄像机跟随
            BallManager.StartCamMove();
            //停止继续
            this.GamePhysicsWorld.Simulate = true;
            this._Start(forceRestart, null);

            this.EventResume.Emit();
        }

        //过关后隐藏飞船
        private void _HideBalloonEnd(bool fromUfo)
        {
            if (this._HideBalloonEndTimerID > 0)
            {
                GameTimer.DeleteDelay(this._HideBalloonEndTimerID);
                this._HideBalloonEndTimerID = 0;
            }
            //60秒后隐藏飞船
            this._HideBalloonEndTimerID = GameTimer.Delay(fromUfo ? 40 : 60, () =>
            {
                this._HideBalloonEndTimerID = 0;
                BallManager.SetControllingStatus(BallControlStatus.NoControl);
                SectorManager.CurrentLevelEndBalloon.Deactive();
                this.EventHideBalloonEnd.Emit();
            });
        }
        //UFO 动画完成回调
        internal void UfoAnimFinish()
        {
            this._SoundFinnal.Play();
            this._HideBalloonEnd(true); //开始隐藏飞船
            BallManager.SetControllingStatus(BallControlStatus.NoControl);
            WinScoreUIControl.Instance.StartSeq();
            this.EventUfoAnimFinish.Emit();
        }

        private bool _IsTranfoIn = false;

        /// <summary>
        /// 激活变球序列
        /// </summary>
        /// <param name="tranfo">变球器</param>
        /// <param name="targetType">要变成的目标球类型</param>
        public void ActiveTranfo(ITranfoBase tranfo, string targetType)
        {
            if (this._IsTranfoIn)
                return;

            this._IsTranfoIn = true;

            var targetPos = tranfo.GetTransform().TransformPoint(new Vector3(0, 2, 0));
            var oldBallType = BallManager.CurrentBallName;
            var cTranfoAminControl = tranfo.GetTranfoAminControl();

            if (cTranfoAminControl == null)
            {
                Log.E(TAG, "Not found TranfoAminControl on tranfo {0}", tranfo.GetTargetBallType());
                return;
            }

            //快速回收当前类型球碎片
            BallManager.ResetPeices(BallManager.CurrentBallName);
            BallManager.SetNextRecoverPos(targetPos);
            //快速将球锁定并移动至目标位置
            BallManager.FastMoveTo(targetPos, 0.1f, (_) =>
            {
                //播放变球动画
                cTranfoAminControl.PlayAnim(tranfo, targetType, () =>
                {

                    //先停止摄像机跟随
                    CamManager.SetCamFollow(false);
                    CamManager.SetCamLook(false);
                    //播放烟雾
                    BallManager.PlaySmoke(targetPos);
                    //先设置无球
                    BallManager.SetNoCurrentBall();
                    //切换球并且抛出碎片
                    BallManager.ThrowPeices(oldBallType, targetPos);
                    //激活新球
                    BallManager.SetCurrentBall(targetType, BallControlStatus.Control);
                    //重新开启摄像机跟随
                    CamManager.SetCamFollow(true);
                    CamManager.SetCamLook(true);

                    //重置状态
                    tranfo.ResetTranfo();
                    this._IsTranfoIn = false;
                });
            });
        }

        #endregion

        #region 关卡加载卸载下一关等等流程控制

        private void _QuitOrLoadNextLevel(bool loadNext)
        {
            Log.D(TAG, "Start Quit Level");

            GameManager.VoidDelegate callBack = null;
            if (loadNext)
            {
                callBack = () => LevelBuilder.LevelBuilder.Instance.LoadLevel(this.NextLevelName);
            }

            //停止隐藏飞船定时
            if (this._HideBalloonEndTimerID > 0)
            {
                GameTimer.DeleteDelay(this._HideBalloonEndTimerID);
                this._HideBalloonEndTimerID = 0;
            }

            //发送事件
            this.EventQuit.Emit();

            //停止背景音乐
            MusicManager.DisableBackgroundMusic(true);

            GameUIManager.Instance.CloseAllPage();
            GameUIManager.Instance.MaskBlackFadeIn(0.7f);
            GameSoundManager.Instance.PlayFastVoice("core.sounds:Menu_load.wav", GameSoundType.Normal);

            GameTimer.Delay(0.5f, () =>
            {
                //停止模拟
                this.GamePhysicsWorld.Simulate = false;
                //关闭球
                this._Stop(BallControlStatus.NoControl);
                this.CurrentSector = 0;
                //隐藏UI
                GamePlayUIControl.Instance.gameObject.SetActive(false);
                //发出事件
                LevelBuilderRef.CallLevelCustomModEvent("beforeQuit");
                LevelBuilderRef.UnLoadLevel(callBack);
            });
        }

        //-加载下一关
        public void NextLevel()
        {
            if (this.NextLevelName == "")
                return;
            this._QuitOrLoadNextLevel(true);
        }
        //-重新开始关卡
        public void RestartLevel()
        {
            //黑色进入
            GameUIManager.Instance.MaskBlackFadeIn(1);

            Log.D(TAG, "Restart Level");

            this._Stop(BallControlStatus.NoControl);

            this.EventRestart.Emit();

            //重置球管理器
            BallManager.ResetAllPeices();

            GameTimer.Delay(0.8f, () =>
            {
                //重置所有节
                SectorManager.ResetAllSector(false);
                this.CurrentSector = 0;
                GameTimer.Delay(0.5f, () =>
                {
                    //开始
                    this._InitAndStart();
                });
            });
        }
        //-退出关卡
        public void QuitLevel()
        {
            this._QuitOrLoadNextLevel(false);
        }

        #endregion

        #region 分数方法

        /// <summary>
        /// 添加生命
        /// </summary>
        /// <returns></returns>
        public void AddLife()
        {
            this.CurrentLife = this.CurrentLife + 1;

            GameTimer.Delay(0.317f, () =>
            {
                this._SoundAddLife.Play();
                GamePlayUIControl.Instance.AddLifeBall();
            });

            this.EventAddLife.Emit();
        }
        /// <summary>
        /// 添加时间点数
        /// </summary>
        /// <param name="count">时间点数，默认为10</param>
        public void AddPoint(int count = 10)
        {
            if (count < 0)
            {
                Log.E(TAG, "AddPoint count can not be negative!");
                return;
            }
            this.CurrentPoint = this.CurrentPoint + count;
            GamePlayUIControl.Instance.SetPointText(this.CurrentPoint);
            GamePlayUIControl.Instance.TwinklePoint();
            this.EventAddPoint.Emit(count);
        }

        #endregion

        #region 一些工具方法

        /// <summary>
        /// 初始化灯光和天空盒
        /// </summary>
        /// <param name="skyBoxPre">A-K 或者空，为空则使用 customSkyMat 材质</param>
        /// <param name="customSkyMat">自定义天空盒材质</param>
        /// <param name="lightColor">灯光颜色</param>
        public void CreateSkyAndLight(string skyBoxPre, Material customSkyMat, Color lightColor)
        {
            CamManager.SetSkyBox(customSkyMat != null ? customSkyMat : SkyBoxUtils.MakeSkyBox(skyBoxPre));
            GameManager.GameLight.color = lightColor;
        }
        /// <summary>
        /// 隐藏天空盒和关卡灯光
        /// </summary>
        public void HideSkyAndLight()
        {
            CamManager.SetSkyBox(null);
        }

        #endregion

    }
}
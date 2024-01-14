using System.Collections.Generic;
using Ballance2.Base;
using Ballance2.Game;
using Ballance2.Game.GamePlay;
using Ballance2.Services;
using Ballance2.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  /// <summary>
  /// 主游戏菜单控制器类
  /// </summary>
  public class GamePlayUIControl : GameSingletonBehavior<GamePlayUIControl> 
  {
    public Image _ScoreBoardActive ;
    public Text _ScoreText;
    public Image _LifeBoardLeftBaffle;
    public GameObject _LifeBoardBallPrefab;
    public GameObject _LifeBoardBallInfPrefab;
    public RectTransform _LifeBalls;
    public GameObject _TextDebugMode;
    public GuiStats GuiStats;

    private int _CurrentShowLifeBallCount = 0;
    private float _CurrentMoveBaffleTick = 0.3f;
    private float _MoveBaffleSec = 0.3f;
    private float _CurrentMoveBaffleStart = 0;
    private float _CurrentMoveBaffleTarget = 0;
    private RectTransform _CurrentMobileKeyPad = null;
    private bool _CurrentMobileKeyPadShow = false;

    public Dictionary<string, GuiStatsValue> DebugStatValues = new Dictionary<string, GuiStatsValue>();

    private void Start() {
      _ScoreBoardActive.gameObject.SetActive(false);

        //手机端还需要创建键盘
        #if (UNITY_ANDROID || UNITY_IOS)
        //当前设备存在触摸屏时，才允许显示屏幕键盘
        if (UnityEngine.InputSystem.Touchscreen.current != null)
        {
            _CurrentMobileKeyPadShow = true;
            ReBuildMobileKeyPad();
            GamePlayManager.Instance.EventBeforeStart.On((evt) =>
            {
                _CurrentMobileKeyPadShow = true;
                if (_CurrentMobileKeyPad != null)
                    _CurrentMobileKeyPad.gameObject.SetActive(true);
            });
            GamePlayManager.Instance.EventQuit.On((evt) =>
            {
                _CurrentMobileKeyPadShow = false;
                if (_CurrentMobileKeyPad != null)
                    _CurrentMobileKeyPad.gameObject.SetActive(false);
            });
        }
      #endif

      //创建调试信息
      if (GameManager.DebugMode) {
        DebugStatValues["CurrentBall"] = GuiStats.AddStat("CurrentBall");
        DebugStatValues["CurrentStatus"] = GuiStats.AddStat("CurrentStatus");
        DebugStatValues["Position"] = GuiStats.AddStat("Position");
        DebugStatValues["Rotation"] = GuiStats.AddStat("Rotation");
        DebugStatValues["PhysicsState"] = GuiStats.AddStat("PhysicsState");
        DebugStatValues["CamDirection"] = GuiStats.AddStat("CamDirection");
        DebugStatValues["CamState"] = GuiStats.AddStat("CamState");
        DebugStatValues["Velocity"] = GuiStats.AddStat("Velocity");
        DebugStatValues["PushValue"] = GuiStats.AddStat("PushValue");
        DebugStatValues["Sector"] = GuiStats.AddStat("Sector");
        DebugStatValues["Moduls"] = GuiStats.AddStat("Moduls");
        DebugStatValues["PhysicsTime"] = GuiStats.AddStat("PhysicsTime");
        DebugStatValues["PhysicsObjects"] = GuiStats.AddStat("PhysicsObjects");
        _TextDebugMode.SetActive(true);
      } else {
        _TextDebugMode.SetActive(false);
      }
    }

    private void Update()
    {
      if (_CurrentMoveBaffleTick < _MoveBaffleSec)
      {
        _CurrentMoveBaffleTick = _CurrentMoveBaffleTick + Time.deltaTime;
        _LifeBoardLeftBaffle.rectTransform.anchoredPosition = new Vector2(
          Mathf.Lerp(_CurrentMoveBaffleStart, _CurrentMoveBaffleTarget, _CurrentMoveBaffleTick / _MoveBaffleSec),
          0
        );
      }
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      this.DestroyMobileKeyPad();
    }

    private void DestroyMobileKeyPad() {
      //销毁键盘
      if (_CurrentMobileKeyPad != null) {
        Destroy(_CurrentMobileKeyPad.gameObject);
        _CurrentMobileKeyPad = null;
      }
    }
    internal void ReBuildMobileKeyPad() {
      //销毁键盘
      DestroyMobileKeyPad();
      //读取键盘设置
      var settings = GameSettingsManager.GetSettings(GamePackageManager.SYSTEM_PACKAGE_NAME);
      var controlKeypadSettting = settings.GetString(SettingConstants.SettingsControlKeypad);

      KeypadUIManager.KeypadUIInfo keyPad;
      if (!KeypadUIManager.GetKeypadRegistered(controlKeypadSettting)) 
      {
        Log.E("GamePlayUIControl", "Keypad in setting \"" + controlKeypadSettting + "\" not found, use default keypad insted.");
        keyPad = KeypadUIManager.GetKeypad("BaseCenter");
      } 
      else 
      {
        keyPad = KeypadUIManager.GetKeypad(controlKeypadSettting);
      }

      //创建键盘
      _CurrentMobileKeyPad = GameUIManager.Instance.InitViewToCanvas(keyPad.prefab, "GameMobileKeypad", false);
      //当屏幕键盘不显示
      if (!_CurrentMobileKeyPadShow)
        _CurrentMobileKeyPad.gameObject.SetActive(false);
    }

    /// <summary>
    /// 闪烁分数面板
    /// </summary>
    public void TwinklePoint() 
    {
      _ScoreBoardActive.gameObject.SetActive(true);
      GameUIManager.Instance.UIFadeManager.AddFadeOut(_ScoreBoardActive, 1, true);
    }
    /// <summary>
    /// 设置分数面板文字
    /// </summary>
    /// <param name="score">分数</param>
    public void SetPointText(int score) 
    {
      _ScoreText.text = score.ToString();
    }
    /// <summary>
    /// 当前显示的生命球数 +1
    /// </summary>
    public void AddLifeBall()
    {
      if (_CurrentShowLifeBallCount != -1) {
        _CurrentShowLifeBallCount++;

        var ball = CloneUtils.CloneNewObjectWithParent(_LifeBoardBallPrefab, _LifeBalls).GetComponent<Image>();
        ball.rectTransform.SetAsFirstSibling();
        ball.color = new Color(1,1,1,0);
        _MoveLifeLeftBaffle();
        GameTimer.Delay(0.4f, () => {
          GameUIManager.Instance.UIFadeManager.AddFadeIn(ball, 0.4f);
        });
      }
    }
    /// <summary>
    /// 当前显示的生命球数 -1
    /// </summary>
    public void RemoveLifeBall()
    {
      if (_CurrentShowLifeBallCount <= -1) 
        return;
      if (_CurrentShowLifeBallCount > 0) {
        _CurrentShowLifeBallCount--;
        var ballIndex = _LifeBalls.childCount - 1 - _CurrentShowLifeBallCount;
        if (ballIndex < 0) ballIndex = 0;
        if (ballIndex == 0 && _LifeBalls.childCount == 0) {
          _MoveLifeLeftBaffle();
          return;
        }

        var ball = _LifeBalls.GetChild(ballIndex);
        GameUIManager.Instance.UIFadeManager.AddFadeOut(ball.GetComponent<Image>(), 0.4f, true);
        GameTimer.Delay(0.4f, () => {
          _MoveLifeLeftBaffle();
          UnityEngine.Object.Destroy(ball.gameObject);
        });
      }
    }
    /// <summary>
    /// 直接设置当前显示的生命球数（无动画效果）
    /// </summary>
    /// <param name="count">数量</param>
    public void SetLifeBallCount(int count)
    {
      if (count != _CurrentShowLifeBallCount) {
        _CurrentShowLifeBallCount = count;
        if (count == -1) {
          //负1就显示无限大图标
          for (var i = _LifeBalls.childCount - 1; i >= 0; i--)
            Destroy(_LifeBalls.GetChild(i).gameObject);
          CloneUtils.CloneNewObjectWithParent(_LifeBoardBallInfPrefab, _LifeBalls);
          _MoveLifeLeftBaffle();
        }
        else {
          if (_LifeBalls.childCount > count) {
            //显示数量大于目标，删除多余的
            for (var i = _LifeBalls.childCount - 1; i >= count; i--)
              UnityEngine.Object.Destroy(_LifeBalls.GetChild(i).gameObject);
            _MoveLifeLeftBaffle();
          }
          else if (_LifeBalls.childCount < count) {
            //显示数量小于目标，添加
            for (var i = count - _LifeBalls.childCount; i > 0; i--)
              CloneUtils.CloneNewObjectWithParent(_LifeBoardBallPrefab, _LifeBalls);
            _MoveLifeLeftBaffle();
          }
        }
      }
    }

    private void _MoveLifeLeftBaffle() 
    {
      if (_CurrentShowLifeBallCount < 0)
        _CurrentMoveBaffleTarget =  -27;
      else
        _CurrentMoveBaffleTarget =  -(_CurrentShowLifeBallCount * 27);
      _CurrentMoveBaffleTick = 0;
      _CurrentMoveBaffleStart = _LifeBoardLeftBaffle.rectTransform.anchoredPosition.x;
    }
  }
}
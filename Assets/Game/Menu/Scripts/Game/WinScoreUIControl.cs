using Ballance2.Base;
using Ballance2.Game.GamePlay;
using Ballance2.Services;
using Ballance2.Services.I18N;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  public class WinScoreUIControl : GameSingletonBehavior<WinScoreUIControl> {
    public TMP_Text ScoreTotal;
    public TMP_Text ScoreExtraLives;
    public TMP_Text ScoreBouns;
    public TMP_Text ScoreTimePoints;
    public GameObject HighlightBar1;
    public GameObject HighlightBar2;
    public GameObject HighlightBar3;
    public GameObject HighlightBar4;
    public GameObject Panel;

    private AudioSource _CountSound;
    private AudioSource _HighscoreSound;
    private AudioSource _SwitchSound;
    private bool _IsInSeq = false;
    private bool _Skip = false;
    private bool _IsCountingPoint = false;
    private int _ScoreNTotal = 0;
    private int _ScoreNTimePoints = 0;
    private int _ScoreNExtraLives = 0;
    private GamePlayManager _GamePlayManager;
    private int EscKeyID = 0;
    private int ReturnKeyID = 0;

    public bool ThisTimeHasNewHighscore { get; private set; }

    private void Start() {
      GameTimer.Delay(2.4f, () => {
        var SoundManager = GameSoundManager.Instance;
        this._SwitchSound = SoundManager.RegisterSoundPlayer(GameSoundType.UI, "core.sounds:Menu_dong.wav", false, true, "WinScoreUISwitch");
        this._HighscoreSound = SoundManager.RegisterSoundPlayer(GameSoundType.UI, "core.sounds.music:Music_Highscore.wav", false, true, "WinScoreUISwitch");
        this._CountSound = SoundManager.RegisterSoundPlayer(GameSoundType.UI, "core.sounds:Menu_counter.wav", false, true, "WinScoreUICount");
      });
    }
    private void Update() {
      if (_IsCountingPoint) {
        if (_GamePlayManager.CurrentPoint > 0) {
          if (_GamePlayManager.CurrentPoint > 4000) {
            _GamePlayManager.CurrentPoint -= 20;
            _ScoreNTimePoints += 20;
            _ScoreNTotal += 20;
          }
          else if (_GamePlayManager.CurrentPoint > 2000) {
            _GamePlayManager.CurrentPoint -= 10;
            _ScoreNTimePoints += 10;
            _ScoreNTotal += 10;
          }
          else if (_GamePlayManager.CurrentPoint > 2000) {
            _GamePlayManager.CurrentPoint -= 10;
            _ScoreNTimePoints += 10;
            _ScoreNTotal += 10;
          }
          else if (_GamePlayManager.CurrentPoint > 4) {
            _GamePlayManager.CurrentPoint -= 4;
            _ScoreNTimePoints += 4;
            _ScoreNTotal += 4;
          }
          else {
            _ScoreNTimePoints += _GamePlayManager.CurrentPoint;
            _ScoreNTotal += _GamePlayManager.CurrentPoint;
            _GamePlayManager.CurrentPoint = 0;
          }
          
          GamePlayUIControl.Instance.SetPointText(_GamePlayManager.CurrentPoint);
          _CountSound.Stop();
          _CountSound.Play();
          ScoreTimePoints.text = _ScoreNTimePoints.ToString();
          ScoreTotal.text = _ScoreNTotal.ToString();
        }
        else {
          _IsCountingPoint = false;
          StartCoroutine(CountingPointEnd());
        }
      }
    }
    protected override void OnDestroy() 
    {
      Destroy(this._SwitchSound);
      Destroy(this._HighscoreSound);
      Destroy(this._CountSound);
    }

    /// <summary>
    /// 开始分数统计序列
    /// </summary>
    public void StartSeq() {
      _IsInSeq = true;
      _ScoreNExtraLives = 0;
      _ScoreNTimePoints = 0;
      _ScoreNTotal = 0;
      _Skip = false;
      _GamePlayManager = GamePlayManager.Instance;
      HighlightBar1.SetActive(false);
      HighlightBar2.SetActive(false);
      HighlightBar3.SetActive(false);
      HighlightBar4.SetActive(false);

      Panel.SetActive(false);
      GameUIManager.Instance.GoPage("PageEndScore");

      StartCoroutine(StartSeqInner());
    }
    private IEnumerator StartSeqInner() {
      yield return new WaitForSeconds(5);

      Panel.SetActive(true);
    
      EscKeyID = GameUIManager.Instance.WaitKey(KeyCode.Escape, false, Skip);
      ReturnKeyID = GameUIManager.Instance.WaitKey(KeyCode.Return, false, Skip);

      yield return new WaitForSeconds(1.7f);
      if (_Skip) 
        yield break;
      
      //关卡分数
      _SwitchSound.Play();
      HighlightBar1.SetActive(true);
      ScoreBouns.text = _GamePlayManager.LevelScore.ToString();
      _ScoreNTotal = _GamePlayManager.LevelScore;
      ScoreTotal.text = _ScoreNTotal.ToString();

      yield return new WaitForSeconds(1.7f);
      if (_Skip) 
        yield break;

      //额外时间点
      _SwitchSound.Play();
      HighlightBar1.SetActive(false);
      HighlightBar2.SetActive(true);
      _IsCountingPoint = true;
    }
    private IEnumerator CountingPointEnd() {
      if (_Skip) 
        yield break;

      yield return new WaitForSeconds(1.5f);

      var GamePlayUI = GamePlayUIControl.Instance;

      //额外生命点
      _SwitchSound.Play();
      HighlightBar2.SetActive(false);
      HighlightBar3.SetActive(true);

      for (var i = _GamePlayManager.CurrentLife; i > 0; i--)
      {
        yield return new WaitForSeconds(0.6f);
        if (_Skip) 
          yield break;

        GamePlayUI.RemoveLifeBall();
        _ScoreNExtraLives += 200;
        _ScoreNTotal += 200;
        ScoreExtraLives.text = _ScoreNExtraLives.ToString();
        ScoreTotal.text = _ScoreNTotal.ToString();
      }

      yield return new WaitForSeconds(1.5f);

      //完整分数
      if (_Skip) 
        yield break;

      _SwitchSound.Play();
      HighlightBar3.SetActive(false);
      HighlightBar4.SetActive(true);

      yield return new WaitForSeconds(5);
      if (_Skip) 
        yield break;

      _IsInSeq = false;
      _ShowHighscore();
    }

    /// <summary>
    /// 获取是否在分数统计序列中
    /// </summary>
    public void Skip() {
      if (_Skip)
        return;
  
      _IsInSeq = false;
      _Skip = true;
      _IsCountingPoint = false;
      _SwitchSound.Play();
      HighlightBar1.SetActive(false);
      HighlightBar2.SetActive(false);
      HighlightBar3.SetActive(false);
      HighlightBar4.SetActive(true);

      _ScoreNTimePoints += _GamePlayManager.CurrentPoint;
      _ScoreNExtraLives = _ScoreNExtraLives + 200 * _GamePlayManager.CurrentLife;
      _ScoreNTotal = _GamePlayManager.LevelScore + _ScoreNTimePoints + _ScoreNExtraLives;

      _GamePlayManager.CurrentLife = 0;
      _GamePlayManager.CurrentPoint = 0;
      GamePlayUIControl.Instance.SetPointText(_GamePlayManager.CurrentPoint);
      GamePlayUIControl.Instance.SetLifeBallCount(0);
      
      ScoreTimePoints.text = _ScoreNTimePoints.ToString();
      ScoreBouns.text = _GamePlayManager.LevelScore.ToString();
      ScoreExtraLives.text = _ScoreNExtraLives.ToString();
      ScoreTotal.text = _ScoreNTotal.ToString();

      GameTimer.Delay(3.5f, _ShowHighscore);
    }    
    /// <summary>
    /// 跳过分数统计序列
    /// </summary>
    /// <returns></returns>
    public bool IsInSeq() { return _IsInSeq; }

    //保存高分数据
    public void SaveHighscore(string entryName) {
      HighscoreManager.Instance.AddItem(_GamePlayManager.CurrentLevelName, entryName, _ScoreNTotal);
      HighscoreManager.Instance.AddLevelPassState(_GamePlayManager.NextLevelName);
    }

    //按ESC键直接进入高分界面
    private void _ShowHighscore() {
      if (EscKeyID != 0) 
      {
        GameUIManager.Instance.DeleteKeyListen(EscKeyID);
        EscKeyID = 0;
      }
      if (ReturnKeyID != 0) {
        GameUIManager.Instance.DeleteKeyListen(ReturnKeyID);
        ReturnKeyID = 0;
      }

      GameUIManager.Instance.GoPage("PageHighscoreEntry");

      //重置文字为0
      ScoreTimePoints.text = "0";
      ScoreBouns.text = "0";
      ScoreExtraLives.text = "0";
      ScoreTotal.text = "0";

      //检查是不是新的高分
      var PageHighscoreEntry = GameUIManager.Instance.GetCurrentPage();
      var HighscoreEntryNameTextScore = PageHighscoreEntry.Content.Find("TextScore").GetComponent<TMP_Text>();
      
      HighscoreEntryNameTextScore.text = $"{_ScoreNTotal} <size=20>{I18N.Tr("core.ui.WinUIPoints")}</size>";
      
      _HighscoreSound.Play();
      if(HighscoreManager.Instance.CheckLevelHighScore(_GamePlayManager.CurrentLevelName, _ScoreNTotal))
      {
        PageHighscoreEntry.Content.Find("TextNewHighScore").gameObject.SetActive(true);
        PageHighscoreEntry.Content.Find("TextWin").gameObject.SetActive(false);
        ThisTimeHasNewHighscore = true;
      }
      else
      {
        PageHighscoreEntry.Content.Find("TextNewHighScore").gameObject.SetActive(false);
        PageHighscoreEntry.Content.Find("TextWin").gameObject.SetActive(true);
        ThisTimeHasNewHighscore = false;
      }
    }
  }
}
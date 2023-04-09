using System.Collections;
using Ballance2.Base;
using Ballance2.Game.Utils;
using Ballance2.Package;
using Ballance2.Services;
using UnityEngine;

namespace Ballance2.Game.GamePlay.Other
{
  /// <summary>
  /// 闪电控制管理器
  /// </summary>
  public class LevelBrizController : MonoBehaviour {

    public AnimationCurve _BrizCurve;
    public Light _BrizLight;
    public float _BrizTime;

    private bool _LightEnable = false;
    private bool _LightFlash = false;
    private float _LightTick = 5;
    private float _LightFlashTick = 0;

    private GameEventHandler entryEvent;

    private void Start() {
      _BrizLight.color = new Color(0,0,0,1);

      entryEvent = GameManager.GameMediator.RegisterEventHandler(
        GamePackage.GetSystemPackage(), 
        "CoreBrizLevelEventHandler",
        "LevelBrizHandler",
        (evtName, param) => {
          var action = param[0] as string;
          if (action == "beforeStart")
            _LightEnable = true;
          else if (action == "beforeQuit")
            _LightEnable = false;
          return false;
        }
      );
    }
    private void OnDestroy() {
      entryEvent.UnRegister();
    }
    private void Update() {
      if (_LightFlash) {
        _LightFlashTick += Time.deltaTime;
        var v = _BrizCurve.Evaluate(_LightFlashTick / _BrizTime);
        if (v > 1)
          _LightFlash = false;
        else
          _BrizLight.color = new Color(v, v, v, 1);
      }
    }
    private void FixedUpdate() {
      if (_LightEnable) {
        if (_LightTick > 0)
          _LightTick -= Time.fixedDeltaTime;
        else {
          LightFlash();
          _LightTick = Random.Range(10, 90);
        }
      }
    }
    private void LightFlash() {
      _LightFlash = true;
      _LightFlashTick = 0;
      GameSoundManager.Instance.PlayFastVoice("core.sounds.music:Music_thunder.wav", GameSoundType.Normal);
    }
  }
}

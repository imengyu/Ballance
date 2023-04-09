using System.Collections;
using Ballance2.Base;
using Ballance2.Services;
using UnityEngine;
using static Ballance2.Services.GameManager;

namespace Ballance2.Game.GamePlay.Tranfo
{
  /// <summary>
  /// 变球器接口
  /// </summary>
  public interface ITranfoAminControl
  {
    /// <summary>
    /// 变球器动画触发逻辑
    /// </summary>
    /// <param name="tranfo">变球器实例</param>
    /// <param name="targetType">目标类型</param>
    /// <param name="ballChangeCallback">变球动画完成，开始实际变球时需要调用此回调</param>
    void PlayAnim(ITranfoBase tranfo, string targetType, VoidDelegate ballChangeCallback);
  }

  /// <summary>
  /// 默认的变球器动画控制器
  /// </summary>
  public class TranfoAminControl : GameSingletonBehavior<TranfoAminControl>, ITranfoAminControl
  {
    public GameObject _AnimTrafo_Flashfield;
    public GameObject _AnimTrafo_Ringpart1;
    public GameObject _AnimTrafo_Ringpart2;
    public GameObject _AnimTrafo_Ringpart3;
    public GameObject _AnimTrafo_Ringpart4;
    public Animator _AnimTrafo_Animation;
    public GameObject _AnimTrafo;
    public Material _AnimTrafo_FlashfieldMat;

    private AudioSource _Misc_Trafo = null;
    private bool _Flashfield = false;
    private int _Flashfield_Tick = 0;
    private Material _AnimTrafo_RingParts_Color1;
    private Material _AnimTrafo_RingParts_Color2;
    private Material _AnimTrafo_RingParts_Color3;
    private Material _AnimTrafo_RingParts_Color4;

    private void Start() {
      _Misc_Trafo = GameSoundManager.Instance.RegisterSoundPlayer(
        GameSoundType.BallEffect,
        GameSoundManager.Instance.LoadAudioResource("core.sounds:Misc_Trafo.wav"), 
        false, 
        true, 
        "Misc_Trafo"
      );
  
      //获取变球器的颜色点材质
      Renderer renderer = _AnimTrafo_Ringpart1.GetComponent<Renderer>();
      _AnimTrafo_RingParts_Color1 = renderer.materials[1];
      renderer = _AnimTrafo_Ringpart2.GetComponent<Renderer>();
      _AnimTrafo_RingParts_Color2 = renderer.materials[1];
      renderer = _AnimTrafo_Ringpart3.GetComponent<Renderer>();
      _AnimTrafo_RingParts_Color3 = renderer.materials[1];
      renderer = _AnimTrafo_Ringpart4.GetComponent<Renderer>();
      _AnimTrafo_RingParts_Color4 = renderer.materials[1];

      //获取变球器电流材质
      renderer = _AnimTrafo_Flashfield.GetComponent<Renderer>();
      _AnimTrafo_FlashfieldMat = renderer.material;
      _AnimTrafo.SetActive(false);
    }
    private void Update() {
      if (_Flashfield) 
      {
        _Flashfield_Tick++;
        if (_Flashfield_Tick > 20)
          _Flashfield_Tick = 0;
        if (_Flashfield_Tick > 10)
          _AnimTrafo_FlashfieldMat.SetTextureOffset("_MainTex", Vector2.zero);
        else
          _AnimTrafo_FlashfieldMat.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.0f));
      }
    }
    protected override void OnDestroy() {
      if (_Misc_Trafo != null) {
        GameSoundManager.Instance.DestroySoundPlayer(_Misc_Trafo);
        _Misc_Trafo = null;
      }
    }

    public void PlayAnim(ITranfoBase tranfo, string targetType, VoidDelegate ballChangeCallback)
    {
      _AnimTrafo.SetActive(true);

      var tranfoTransform = tranfo.GetTransform();
      var color = tranfo.GetTranfoColor();
      var placeholder = tranfoTransform.gameObject;
      
      transform.position = tranfoTransform.position;
      transform.eulerAngles = tranfoTransform.eulerAngles;

      //隐藏占位变球器
      if (placeholder != null)
        placeholder.SetActive(false);

      _AnimTrafo_Flashfield.SetActive(true);
      _Flashfield = true;

      //设置变球器颜色
      _AnimTrafo_RingParts_Color1.color = color;
      _AnimTrafo_RingParts_Color2.color = color;
      _AnimTrafo_RingParts_Color3.color = color;
      _AnimTrafo_RingParts_Color4.color = color;

      //播放动画和声音
      _Misc_Trafo.Play();
      _AnimTrafo_Animation.speed = 1;
      _AnimTrafo_Animation.Play("TranfoAnimation");

      //延时关闭
      StartCoroutine(PlayAnimCoroutine(placeholder, ballChangeCallback));
    }

    private IEnumerator PlayAnimCoroutine(GameObject placeholder, VoidDelegate ballChangeCallback) {
      yield return new WaitForSeconds(2.3f);

      _AnimTrafo_Flashfield.SetActive(false);
      _Flashfield = false;

      ballChangeCallback();

      yield return new WaitForSeconds(0.2f);

      _AnimTrafo_Animation.speed = 0;
      //隐藏本体，显示占位变球器
      _AnimTrafo.SetActive(false);
      if (placeholder != null) 
        placeholder.SetActive(true);
    }
  }
}
using System.Collections;
using Ballance2.Services;
using UnityEngine;
using static Ballance2.Services.GameManager;

namespace Ballance2.Game.GamePlay.Balls 
{
  /// <summary>
  /// 闪电球动画控制脚本
  /// </summary>
  public class BallLightningSphere : MonoBehaviour 
  {
    private const string TAG = "BallLightningSphere";

    public Light Ball_Light = null;
    public GameObject Ball_Smoke = null;
    public GameObject Ball_LightningSphereInnernA = null;
    public GameObject Ball_LightningSphereInnernB = null;
    public Texture BallLightningSphereTexture1 = null;
    public Texture BallLightningSphereTexture2 = null;
    public Texture BallLightningSphereTexture3 = null;
    public AnimationCurve BallLightningBallBigCurve = null;
    public AnimationCurve BallLightningCurveEnd = null;
    public AnimationCurve BallLightningCurve = null;
    public float BallLightBallBigSec = 1.5f;
    public float BallLightSec = 1.5f;
    public float BallLightEndSec = 1.5f;

    private bool lighing = false;
    private bool lighingLight = false;
    private bool lighingLightEnd = false;
    private float lighingLightBigTick = 0;
    private float lighingLightTick = 0;
    private float lighingLightEndTick = 0; 
    private float lighingLightColorAlpha = 0.6f; 
    private bool lighingBig = false; 
    private float lighingControlTick = 0;
    private Material ballLightningSphereMaterialA = null;
    private Material ballLightningSphereMaterialB = null;
    private float ballLightingRoateSpeed1 = 400;
    private float ballLightingRoateSpeed2 = 400;
    private int ballLightningSphereTextureCurrent = 1;
    private AudioSource ballLightningMusic = null;

    private void Start() {
      this.ballLightningMusic = GameSoundManager.Instance.RegisterSoundPlayer(GameSoundType.BallEffect, GameSoundManager.Instance.LoadAudioResource("core.sounds:Misc_Lightning.wav"), false, true, "Misc_Lightning");
      var meshRenderer = this.Ball_LightningSphereInnernA.gameObject.GetComponent<UnityEngine.MeshRenderer>();
      this.ballLightningSphereMaterialA = meshRenderer.material;
      meshRenderer = this.Ball_LightningSphereInnernB.gameObject.GetComponent<UnityEngine.MeshRenderer>();
      this.ballLightningSphereMaterialB = meshRenderer.material;
    }
    private void Update() {
      //闪电球
      if (lighing)
      {
        if (Ball_LightningSphereInnernA.transform.localEulerAngles.z > 360) Ball_LightningSphereInnernA.transform.localEulerAngles = new Vector3(0, 0, 0);
        if (Ball_LightningSphereInnernB.transform.localEulerAngles.z < -360) Ball_LightningSphereInnernB.transform.localEulerAngles = new Vector3(0, 0, 0);
        Ball_LightningSphereInnernA.transform.localEulerAngles = new Vector3(0, Ball_LightningSphereInnernA.transform.localEulerAngles.y + ballLightingRoateSpeed1 * Time.deltaTime, 0);
        Ball_LightningSphereInnernB.transform.localEulerAngles = new Vector3(0, Ball_LightningSphereInnernB.transform.localEulerAngles.y - ballLightingRoateSpeed2 * Time.deltaTime, 0);

        //更换闪电球贴图
        if (lighingControlTick < 0.1f) 
          lighingControlTick += Time.deltaTime;
        else
        {
          if (ballLightningSphereTextureCurrent >= 3) ballLightningSphereTextureCurrent = 1;
          else ballLightningSphereTextureCurrent++;
          
          //按数字更换贴图
          if (ballLightningSphereTextureCurrent == 1) 
          {
            ballLightningSphereMaterialA.mainTexture = BallLightningSphereTexture1;
            ballLightningSphereMaterialB.mainTexture = BallLightningSphereTexture1;
          }
          else if (ballLightningSphereTextureCurrent == 2) 
          {
            ballLightningSphereMaterialA.mainTexture = BallLightningSphereTexture2;
            ballLightningSphereMaterialB.mainTexture = BallLightningSphereTexture2;
          }
          else if (ballLightningSphereTextureCurrent == 3) 
          {
            ballLightningSphereMaterialA.mainTexture = BallLightningSphereTexture3;
            ballLightningSphereMaterialB.mainTexture = BallLightningSphereTexture3;
          }
          lighingControlTick = 0;
        }
      }
      //闪电球 放大
      if (lighingBig) 
      {
        if(lighingLightBigTick < BallLightBallBigSec) 
        {
          lighingLightBigTick += Time.deltaTime;
          var v = BallLightningBallBigCurve.Evaluate(lighingLightBigTick / BallLightBallBigSec);
          Ball_LightningSphereInnernA.transform.localScale = new Vector3(v, v, v);
          Ball_LightningSphereInnernB.transform.localScale = new Vector3(v, v, v);
        }
        else
        {
          Ball_LightningSphereInnernA.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
          Ball_LightningSphereInnernB.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
          lighingBig = false;
        }
      }
      //闪电light
      if (lighingLight) 
      {
        lighingLightTick += Time.deltaTime;
        Ball_Light.color = new Color(Ball_Light.color.r, Ball_Light.color.g, BallLightningCurve.Evaluate(lighingLightTick / BallLightSec), lighingLightColorAlpha);

        if (lighingLightTick > BallLightSec) 
        {
          lighingLightEndTick = 0;
          lighingLightEnd = true;
          lighingLight = false;
        }
      }
      //闪电light结尾的一闪
      if (lighingLightEnd) 
      {        
        lighingLightEndTick += Time.deltaTime;
        
        var v = BallLightningCurveEnd.Evaluate(lighingLightEndTick / BallLightEndSec);
        Ball_Light.color = new Color(v, v, v, lighingLightColorAlpha);
        if (lighingLightEndTick > BallLightEndSec) 
        {
          Ball_Light.gameObject.SetActive(false);
          lighingLightEnd = false;
        }
      }
    }

    /// <summary>
    /// 开始播放闪电球动画
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="smallToBig">是否由小变大</param>
    /// <param name="callback">完成回调</param>
    /// <param name="lightAnim">是否播放相对应的 Light 灯光</param>
    public void PlayLighting(Vector3 position, bool smallToBig, VoidDelegate callback, bool lightAnim)
    {
      if (lighing) return;

      //播放闪电声音
      lighing = true;
      lighingLight = false;
      lighingLightEnd = false;
      if (ballLightningMusic != null)
          ballLightningMusic.Play();
      
      Ball_Light.transform.position = position;
      //显示球
      Ball_LightningSphereInnernA.gameObject.SetActive(true);
      Ball_LightningSphereInnernA.transform.position = position;
      Ball_LightningSphereInnernB.gameObject.SetActive(true);
      Ball_LightningSphereInnernB.transform.position = position;

      if (smallToBig) 
      {
        Ball_LightningSphereInnernA.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        Ball_LightningSphereInnernB.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        lighingLightBigTick = 0;
        lighingBig = true;
      }

      if (lightAnim) 
      {
        lighingLight = true;
        lighingLightTick = 0;
        Ball_Light.gameObject.SetActive(true);
      }
      else 
      {
        Ball_Light.gameObject.SetActive(false);
      }

      //延时关闭
      StartCoroutine(LightingDelayHide(position, callback, lightAnim));
    }
    /// <summary>
    /// 获取当前是否正在进行闪电动画
    /// </summary>
    /// <returns></returns>
    public bool IsLighting() {
      return lighing;
    }

    private IEnumerator LightingDelayHide(Vector3 position, VoidDelegate callback, bool lightAnim) {
      yield return new WaitForSeconds(BallLightSec);
          
      Ball_LightningSphereInnernA.transform.localScale = new Vector3(1, 1, 1);
      Ball_LightningSphereInnernB.transform.localScale = new Vector3(1, 1, 1);
      Ball_LightningSphereInnernB.gameObject.SetActive(false);
      Ball_LightningSphereInnernA.gameObject.SetActive(false);
      Ball_Smoke.transform.position = position;
      Ball_Smoke.SetActive(true);
      lighing = false;

      if (callback != null)
        callback();
      
      if (lightAnim) 
      {
        yield return new WaitForSeconds(BallLightEndSec);
        Ball_Light.gameObject.SetActive(false);
      }
    }
  }
}
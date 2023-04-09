using Ballance2.Services;
using UnityEngine;

namespace Ballance2.Game.GamePlay.Moduls
{
  /// <summary>
  /// 生命球组件
  /// </summary>
  public class P_Extra_Life : ModulBase 
  {
    public GameObject P_Extra_Life_Particle_Fizz;
    public GameObject P_Extra_Life_Particle_Blob;
    public GameObject P_Extra_Life_Sphere;
    public GameObject P_Extra_Life_Shadow;
    public Animator P_Extra_Life_Animator;
    public TiggerTester P_Extra_Life_Tigger;

    private bool _Actived = false;
    private bool _OnFloor = false;

    protected override void Start()
    {
      if (!IsPreviewMode) {
        P_Extra_Life_Tigger.onTriggerEnter = (body, otherBody) => 
        {
          if (!_Actived && otherBody.tag == "Ball") {
            _Actived = true;
            P_Extra_Life_Sphere.SetActive(false);
            P_Extra_Life_Shadow.SetActive(false);
            P_Extra_Life_Particle_Fizz.SetActive(true);
            P_Extra_Life_Particle_Blob.SetActive(true);
            GameSoundManager.Instance.PlayFastVoice("core.sounds:Extra_Life_Blob.wav", GameSoundType.Normal);
            GamePlayManager.Instance.AddLife();
          }
        };
      }
      //触发射线，检查当前下方是不是路面，如果是，则显示 Shadow
      RaycastHit hitinfo;
      if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hitinfo, 5) && hitinfo.collider != null) {
        var parentName = hitinfo.collider.gameObject.tag;
        if (parentName == "Phys_Floors" || (parentName == "Phys_FloorWoods" && parentName != "Phys_FloorRails"))
          _OnFloor = true;
        else
          _OnFloor = false;
      }
      else _OnFloor = false;
      base.Start();
    }

    public override void Active()
    {
      //如果在路面上，还要播放上下的动画
      if (_OnFloor)
        P_Extra_Life_Animator.Play("P_ExtraLife_Updown_Animation", 1);

      P_Extra_Life_Shadow.SetActive(_OnFloor);
      P_Extra_Life_Animator.speed = 1;
      P_Extra_Life_Animator.Play("P_ExtraLife_Animation");
      P_Extra_Life_Sphere.SetActive(true);

      base.Active();
    }

    public override void ActiveForPreview()
    {
      this.Active();
      base.ActiveForPreview();
    }

    public override void Deactive()
    {
      P_Extra_Life_Sphere.SetActive(false);
      P_Extra_Life_Shadow.SetActive(false);
      P_Extra_Life_Particle_Fizz.SetActive(false);
      P_Extra_Life_Particle_Blob.SetActive(false);
      P_Extra_Life_Animator.speed = 0;
      base.Deactive();
    }

    public override void DeactiveForPreview()
    {
      this.Deactive();
      base.DeactiveForPreview();
    }

    public override void Reset(ModulBaseResetType type)
    {
      _Actived = false;
      P_Extra_Life_Particle_Fizz.SetActive(false);
      P_Extra_Life_Particle_Blob.SetActive(false);
      P_Extra_Life_Animator.speed = 0;
      base.Reset(type);
    }
  }
}
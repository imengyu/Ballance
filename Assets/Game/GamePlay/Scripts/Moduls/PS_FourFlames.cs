using UnityEngine;

namespace Ballance2.Game.GamePlay.Moduls
{
  /// <summary>
  /// 起点四火焰组件
  /// </summary>
  public class PS_FourFlames : ModulBase 
  {
    public GameObject Flame_A;
    public GameObject Flame_B;
    public GameObject Flame_C;
    public GameObject Flame_D;

    public override void Active()
    {
      base.Active();
      this.Flame_A.SetActive(true);
      this.Flame_B.SetActive(true);
      this.Flame_C.SetActive(true);
      this.Flame_D.SetActive(true);
    }
    public override void Deactive()
    {
      base.Deactive();
      this.Flame_A.SetActive(false);
      this.Flame_B.SetActive(false);
      this.Flame_C.SetActive(false);
      this.Flame_D.SetActive(false);
    }

    public override void ActiveForPreview()
    {
      this.Flame_A.SetActive(true);
      this.Flame_B.SetActive(true);
      this.Flame_C.SetActive(true);
      this.Flame_D.SetActive(true);
    }
    public override void DeactiveForPreview()
    {
      this.Flame_A.SetActive(false);
      this.Flame_B.SetActive(false);
      this.Flame_C.SetActive(false);
      this.Flame_D.SetActive(false);
    }
  }
}
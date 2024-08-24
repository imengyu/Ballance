using Ballance2.Services;
using Ballance2.Utils;
using BallancePhysics.Wapper;
using UnityEngine;

namespace Ballance2.Game.GamePlay.Balls
{
  /// <summary>
  /// 纸球定义
  /// </summary>
  public class BallPaper : Ball
  {
    private Vector3 paperPeicesForceDir = new Vector3(0.03f, 0, -0.03f);

    public BallPaper() {
      _PiecesSoundName = "core.sounds:Pieces_Wood.wav";
      _SoundConfig.HitSound.Names.Add("All", "core.sounds:Hit_Paper.wav");
      _SoundConfig.RollSound.Names.Add("All", "core.sounds:Roll_Paper.wav");
      _SoundConfig.RollSoundSpeedReference = 12f;
      _SoundConfig.RollSound.TimeDelayStart = 0.8f;
      _SoundConfig.RollSound.TimeDelayEnd = 0.1f;
      //自定义物理化碎片
      _PiecesPhysCallback = (go, data) => {
        var body = go.AddComponent<PhysicsObject>();
        body.Mass = CommonUtils.RandomFloat(0.02f, 0.09f);
        body.Elasticity = data.Elasticity;
        body.Friction = CommonUtils.RandomFloat(1, 5);
        body.LinearSpeedDamping = data.LinearDamp;
        body.RotSpeedDamping = data.RotDamp;
        body.Layer = GameLayers.LAYER_PHY_BALL_PEICES;
        body.UseExistsSurface = true;
        body.ExtraRadius = 0;
        body.BuildRootConvexHull = false;
        return body;
      };
    }

    private AudioSource _PaperPiecesSound;

    protected override void Start()
    {
      base.Start();
      _PaperPiecesSound = GameSoundManager.Instance.RegisterSoundPlayer(GameSoundType.BallEffect,
        GameSoundManager.Instance.LoadAudioResource("core.sounds:Pieces_Paper.wav"), false, true, "Pieces_Paper");
    }

    public override void ThrowPieces(Vector3 pos)
    {
      _PaperPiecesSound.Play();
      base.ThrowPieces(pos);

      //纸球碎片将施加一个恒力，以达到被风吹走的效果
      if (_PiecesData != null) {
        foreach(var body in _PiecesData.bodys) {
          body.ClearConstantForce();
          body.AddConstantForceWithPositionAndRef(1, paperPeicesForceDir, Vector3.zero, null, body.transform); //加力
        }
      }
    }

    public override void ResetPieces()
    {
      //去掉纸球碎片恒力
      if (_PiecesData != null) {
        foreach(var body in _PiecesData.bodys) {
          body.ClearConstantForce();
        }
      }

      base.ResetPieces();
    }
  }
}
namespace Ballance2.Game.GamePlay.Balls
{
  /// <summary>
  /// 石球定义
  /// </summary>
  public class BallStone : Ball
  {
    public BallStone() {
      _PiecesSoundName = "core.sounds:Pieces_Stone.wav";
      _PiecesHaveColSound.Add("Ball_Stone_piece01");
      _PiecesHaveColSound.Add("Ball_Stone_piece04");
      _PiecesHaveColSound.Add("Ball_Stone_piece07");
      _PiecesHaveColSound.Add("Ball_Stone_piece11");
      _PiecesHaveColSound.Add("Ball_Stone_piece14");
      _PiecesHaveColSound.Add("Ball_Stone_piece17"); 
      _SoundConfig.HitSound.Names.Add("Dome", "core.sounds:Hit_Stone_Kuppel.wav");
      _SoundConfig.HitSound.Names.Add("Metal", "core.sounds:Hit_Stone_Metal.wav");
      _SoundConfig.HitSound.Names.Add("Stone", "core.sounds:Hit_Stone_Stone.wav");
      _SoundConfig.HitSound.Names.Add("Wood", "core.sounds:Hit_Stone_Wood.wav");
      _SoundConfig.HitSound.Names.Add("Paper", "core.sounds:Hit_Paper.wav");
      _SoundConfig.RollSound.Names.Add("Metal", "core.sounds:Roll_Stone_Metal.wav");
      _SoundConfig.RollSound.Names.Add("Stone", "core.sounds:Roll_Stone_Stone.wav");
      _SoundConfig.RollSound.Names.Add("Wood", "core.sounds:Roll_Stone_Wood.wav");
      _SoundConfig.RollSoundSpeedReference = 15f;
    }
  }
}
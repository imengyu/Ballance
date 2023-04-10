namespace Ballance2.Game.GamePlay.Balls
{
  /// <summary>
  /// 石球定义
  /// </summary>
  public class BallStone : Ball
  {
    public BallStone() {
      this._PiecesSoundName = "core.sounds:Pieces_Stone.wav";
      this._PiecesHaveColSound.Add("Ball_Stone_piece01");
      this._PiecesHaveColSound.Add("Ball_Stone_piece04");
      this._PiecesHaveColSound.Add("Ball_Stone_piece07");
      this._PiecesHaveColSound.Add("Ball_Stone_piece11");
      this._PiecesHaveColSound.Add("Ball_Stone_piece14");
      this._PiecesHaveColSound.Add("Ball_Stone_piece17"); 
      this._SoundConfig.HitSound.Names.Add("Dome", "core.sounds:Hit_Stone_Kuppel.wav");
      this._SoundConfig.HitSound.Names.Add("Metal", "core.sounds:Hit_Stone_Metal.wav");
      this._SoundConfig.HitSound.Names.Add("Stone", "core.sounds:Hit_Stone_Stone.wav");
      this._SoundConfig.HitSound.Names.Add("Wood", "core.sounds:Hit_Stone_Wood.wav");
      this._SoundConfig.HitSound.Names.Add("Paper", "core.sounds:Hit_Paper.wav");
      this._SoundConfig.RollSound.Names.Add("Metal", "core.sounds:Roll_Stone_Metal.wav");
      this._SoundConfig.RollSound.Names.Add("Stone", "core.sounds:Roll_Stone_Stone.wav");
      this._SoundConfig.RollSound.Names.Add("Wood", "core.sounds:Roll_Stone_Wood.wav");
      this._SoundConfig.RollSoundSpeedReference = 15f;
    }
  }
}
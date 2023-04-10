namespace Ballance2.Game.GamePlay.Balls
{
  public class BallWood : Ball
  {
    public BallWood() {
      this._PiecesSoundName = "core.sounds:Pieces_Wood.wav";
      this._PiecesHaveColSound.Add("Ball_Wood_piece01");
      this._PiecesHaveColSound.Add("Ball_Wood_piece04");
      this._PiecesHaveColSound.Add("Ball_Wood_piece07");
      this._PiecesHaveColSound.Add("Ball_Wood_piece11");
      this._PiecesHaveColSound.Add("Ball_Wood_piece14");
      this._PiecesHaveColSound.Add("Ball_Wood_piece17"); 
      this._SoundConfig.HitSound.Names.Add("Dome", "core.sounds:Hit_Wood_Dome.wav");
      this._SoundConfig.HitSound.Names.Add("Metal", "core.sounds:Hit_Wood_Metal.wav");
      this._SoundConfig.HitSound.Names.Add("Stone", "core.sounds:Hit_Wood_Stone.wav");
      this._SoundConfig.HitSound.Names.Add("Wood", "core.sounds:Hit_Wood_Wood.wav");
      this._SoundConfig.HitSound.Names.Add("Paper", "core.sounds:Hit_Paper.wav");
      this._SoundConfig.RollSound.Names.Add("Metal", "core.sounds:Roll_Wood_Metal.wav");
      this._SoundConfig.RollSound.Names.Add("Stone", "core.sounds:Roll_Wood_Stone.wav");
      this._SoundConfig.RollSound.Names.Add("Wood", "core.sounds:Roll_Wood_Wood.wav");
      this._SoundConfig.RollSoundSpeedReference = 9f;
    }
  }
}
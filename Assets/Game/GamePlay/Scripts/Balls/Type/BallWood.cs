namespace Ballance2.Game.GamePlay.Balls
{
  public class BallWood : Ball
  {
    public BallWood() {
      _PiecesSoundName = "core.sounds:Pieces_Wood.wav";
      _PiecesHaveColSound.Add("Ball_Wood_piece01");
      _PiecesHaveColSound.Add("Ball_Wood_piece04");
      _PiecesHaveColSound.Add("Ball_Wood_piece07");
      _PiecesHaveColSound.Add("Ball_Wood_piece11");
      _PiecesHaveColSound.Add("Ball_Wood_piece14");
      _PiecesHaveColSound.Add("Ball_Wood_piece17"); 
      _SoundConfig.HitSound.Names.Add("Dome", "core.sounds:Hit_Wood_Dome.wav");
      _SoundConfig.HitSound.Names.Add("Metal", "core.sounds:Hit_Wood_Metal.wav");
      _SoundConfig.HitSound.Names.Add("Stone", "core.sounds:Hit_Wood_Stone.wav");
      _SoundConfig.HitSound.Names.Add("Wood", "core.sounds:Hit_Wood_Wood.wav");
      _SoundConfig.HitSound.Names.Add("Paper", "core.sounds:Hit_Paper.wav");
      _SoundConfig.RollSound.Names.Add("Metal", "core.sounds:Roll_Wood_Metal.wav");
      _SoundConfig.RollSound.Names.Add("Stone", "core.sounds:Roll_Wood_Stone.wav");
      _SoundConfig.RollSound.Names.Add("Wood", "core.sounds:Roll_Wood_Wood.wav");
      _SoundConfig.RollSoundSpeedReference = 16f;
    }
  }
}
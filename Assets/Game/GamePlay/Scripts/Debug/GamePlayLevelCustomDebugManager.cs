using Ballance2.Entry;
using Ballance2.Services;
using Ballance2.Services.Debug;

namespace Ballance2.Game.GamePlay.Debug
{
  public static class GamePlayLevelCustomDebugManager
  {
    public static void Init() {
      //检查参数
      if (string.IsNullOrEmpty(GameDebugEntry.Instance.LevelName)) {
        GameErrorChecker.ThrowGameError(GameError.ParamNotProvide, "LevelName 未设置！");
        return;
      }
      GameManager.GameMediator.NotifySingleEvent("CoreStartLoadLevel", GameDebugEntry.Instance.LevelName);
    } 
  }
}
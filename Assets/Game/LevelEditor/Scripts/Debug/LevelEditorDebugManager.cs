using Ballance2.Entry;
using Ballance2.Menu.LevelManager;
using Ballance2.Services;

namespace Ballance2.Game.LevelEditor
{
  public static class LevelEditorDebugManager
  {
    public static void Init() {
      GameManager.GameMediator.NotifySingleEvent("CoreStartEditLevel", LevelManager.Instance.GetLevelByName(GameDebugEntry.Instance.EditLevelName));
    } 
  }
}
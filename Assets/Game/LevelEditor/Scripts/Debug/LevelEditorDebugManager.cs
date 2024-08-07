using Ballance2.Entry;
using Ballance2.Services;
using Ballance2.Services.Debug;

namespace Ballance2.Game.LevelEditor
{
  public static class LevelEditorDebugManager
  {
    public static void Init() {
      GameManager.GameMediator.NotifySingleEvent("CoreStartEditLevel", GameDebugEntry.Instance.EditLevelName);
    } 
  }
}
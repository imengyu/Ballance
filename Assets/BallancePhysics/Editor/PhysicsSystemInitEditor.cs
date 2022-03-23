using BallancePhysics.Wapper;
using UnityEditor;

namespace BallancePhysics.Editor
{
  [InitializeOnLoad]
  public class PhysicsSystemInitEditor
  {
    static PhysicsSystemInitEditor()
    {
      EditorApplication.pauseStateChanged += PauseStateChanged;
      EditorApplication.quitting += Quitting;
      if (EditorApplication.isPlayingOrWillChangePlaymode)
        PauseStateChanged(PauseState.Unpaused);
    }

    static void PauseStateChanged(PauseState state)
    {
      PhysicsSystemInit.DoInit();
      if(state == PauseState.Paused)
        PhysicsEnvironment.HandleEditorPause();
      else if(state == PauseState.Unpaused)
        PhysicsEnvironment.HandleEditorPlay();
    }
    static void Quitting()
    {
      PhysicsSystemInit.DoDestroy();
    }
  }
}
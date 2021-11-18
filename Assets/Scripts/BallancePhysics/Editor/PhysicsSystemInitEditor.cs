using UnityEditor;
using UnityEngine;

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
    }
    static void Quitting()
    {
      PhysicsSystemInit.DoDestroy();
    }
  }
}
using UnityEditor;
using UnityEngine;

namespace PhysicsRT
{
    [InitializeOnLoad]
    public class PhysicsSystemInitEditor
    {
        static bool firstPlay = false;

        static PhysicsSystemInitEditor()
        {
            EditorApplication.pauseStateChanged += PauseStateChanged;
            EditorApplication.quitting += Quitting;

            if(EditorApplication.isPlayingOrWillChangePlaymode)
                PauseStateChanged(PauseState.Unpaused);
        }

        static void PauseStateChanged(PauseState state)
        {
            if (firstPlay)
            {
                return;
            }
            firstPlay = true;
            PhysicsSystemInit.DoInit();
        }
        static void Quitting() {
            PhysicsSystemInit.DoDestroy();
        }
    }
}
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicControlSnap : MonoBehaviour 
  {
    public bool EnableSnap = true;

    public static LevelDynamicControlSnap Instance;

    public LevelDynamicControlSnap()
    {
      Instance = this;
    }
  }
}
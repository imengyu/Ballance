using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicControlSnap : MonoBehaviour
  {
    public static int IdPool = 0;

    public bool IsMoveTool = true;

    public bool EnableRotSnap = true;
    public bool EnableSnap = true;
    public LevelDynamicControlPoint ActiveSnapPoint = null;

    public static LevelDynamicControlSnap Instance;

    public static bool CheckSnap(LevelDynamicControlPoint point)
    {
      return Instance.IsMoveTool && Instance.EnableSnap && Instance.ActiveSnapPoint == point;
    }
    public static bool CheckSnapListener()
    {
      return Instance.IsMoveTool && Instance.EnableSnap && Instance.ActiveSnapPoint == null;
    }
    public static void ResetSnapCheck()
    {
      Instance.ActiveSnapPoint = null;
    }

    public LevelDynamicControlSnap()
    {
      Instance = this;
    }
  }
}
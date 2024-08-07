using UnityEngine;

namespace Ballance2.Utils
{
  public static class GameObjectUtils
  {
    public static void SetLayerRecursively(GameObject obj, int layerIndex)
    {
      // 设置当前对象的层
      obj.layer = layerIndex;

      // 遍历所有子对象
      foreach (Transform child in obj.transform)
      {
          SetLayerRecursively(child.gameObject, layerIndex);
      }
    }
  }
}
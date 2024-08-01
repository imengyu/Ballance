using UnityEngine;

namespace Ballance2.Utils
{
  public static class TransformUtils
  {
    public static void DestroyAllChildren(this Transform transform) 
    {
      DestroyTransformAllChildren(transform);
    }
    public static void DestroyTransformAllChildren(Transform transform) 
    {
      for (int i = transform.childCount - 1; i >= 0; i--)
        Object.Destroy(transform.GetChild(i).gameObject);
    }
  }
}
using System;
using UnityEngine;

namespace Ballance2.Utils
{
  public static class TransformUtils
  {
    public static void ForeachAllChildren(this Transform transform, Action<Transform> child) 
    {
      ForeachTransformAllChildren(transform, child);
    }
    public static void DestroyAllChildren(this Transform transform) 
    {
      DestroyTransformAllChildren(transform);
    }
    public static void ForeachTransformAllChildren(Transform transform, Action<Transform> child) 
    {
      for (int i = transform.childCount - 1; i >= 0; i--)
        child(transform.GetChild(i));
    }
    public static void DestroyTransformAllChildren(Transform transform) 
    {
      for (int i = transform.childCount - 1; i >= 0; i--)
        UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
    }
  }
}
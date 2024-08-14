using UnityEngine;

namespace Ballance2.Utils
{
  public static class GameObjectUtils
  {
    /// <summary>
    /// 循环设置对象与子对象的layer
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="layerIndex"></param>
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

    /// <summary>
    /// 获取或者添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
      var comp = obj.GetComponent<T>();
      if (comp != null)
        return comp;
        comp = obj.AddComponent<T>();
      return comp;
    }

    /// <summary>
    /// 获取或者添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this MonoBehaviour obj) where T : Component
    {
      return GetOrAddComponent<T>(obj.gameObject);
    }

  }
}
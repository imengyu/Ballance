using Ballance2.Sys.Res;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* CloneUtils.cs
* 
* 用途：
* 克隆游戏对象快速工具类
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-15 创建
*
*/

namespace Ballance2.Sys.Utils
{
    /// <summary>
    /// 克隆工具类
    /// </summary>
    [SLua.CustomLuaClass]
    public static class CloneUtils
    {
        public static GameObject CloneNewObject(GameObject prefab, string name)
        {
            GameObject go = Object.Instantiate(prefab, GameManager.Instance.transform);
            go.name = name;
            return go;
        }
        public static GameObject CloneNewObjectWithParent(GameObject prefab, Transform parent, string name)
        {
            GameObject go = Object.Instantiate(prefab, parent);
            go.name = name;
            return go;
        }
        public static GameObject CloneNewObjectWithParent(GameObject prefab, Transform parent)
        {
            return Object.Instantiate(prefab, parent);
        }
        public static GameObject CloneNewObjectWithParent(GameObject prefab, Transform parent, string name, bool active)
        {
            GameObject go = Object.Instantiate(prefab, parent);
            go.name = name;
            go.SetActive(active);
            return go;
        }

        public static GameObject CreateEmptyObject(string name)
        {
            GameObject go = Object.Instantiate(GameStaticResourcesPool.PrefabEmpty, GameManager.Instance.transform);
            go.name = name;
            return go;
        }
        public static GameObject CreateEmptyObjectWithParent(Transform parent, string name)
        {
            GameObject go = Object.Instantiate(GameStaticResourcesPool.PrefabEmpty, parent);
            go.name = name;
            return go;
        }
        public static GameObject CreateEmptyObjectWithParent(Transform parent)
        {
            return Object.Instantiate(GameStaticResourcesPool.PrefabEmpty, parent);
        }

        public static GameObject CreateEmptyUIObject(string name)
        {
            GameObject go = Object.Instantiate(GameStaticResourcesPool.PrefabUIEmpty, GameManager.Instance.GameCanvas.transform);
            go.name = name;
            return go;
        }
        public static GameObject CreateEmptyUIObjectWithParent(Transform parent, string name)
        {
            GameObject go = Object.Instantiate(GameStaticResourcesPool.PrefabUIEmpty, parent);
            go.name = name;
            return go;
        }
        public static GameObject CreateEmptyUIObjectWithParent(Transform parent)
        {
            return Object.Instantiate(GameStaticResourcesPool.PrefabUIEmpty, parent);
        }
    }
}

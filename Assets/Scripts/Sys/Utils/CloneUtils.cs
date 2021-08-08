using Ballance.LuaHelpers;
using Ballance2.Sys.Debug;
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
* 
* 
*
*/

namespace Ballance2.Sys.Utils
{
    /// <summary>
    /// 克隆工具类
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("克隆工具类")]
    public static class CloneUtils
    {
        /// <summary>
        /// 使用 Prefab 克隆一个新的对象
        /// </summary>
        /// <param name="prefab">Prefab</param>
        /// <param name="name">新的对象的名字</param>
        /// <returns>返回生成的新对象</returns>
        [LuaApiDescription("使用 Prefab 克隆一个新的对象", "返回生成的新对象")]
        [LuaApiParamDescription("prefab", "Prefab")]
        [LuaApiParamDescription("name", "新的对象的名字")]
        public static GameObject CloneNewObject(GameObject prefab, string name)
        {
            if(prefab == null) {
                GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, "CloneUtils", "Can not clone {0}, The prefab is null", name);
                return null;
            }
            GameObject go = Object.Instantiate(prefab, GameManager.Instance.transform);
            go.name = name;
            return go;
        }
        /// <summary>
        /// 使用 Prefab 克隆一个新的对象，并添加至指定变换的子级
        /// </summary>
        /// <param name="prefab">Prefab</param>
        /// <param name="parent">父级对象</param>
        /// <param name="name">新的对象的名字</param>
        /// <returns>返回生成的新对象</returns>
        [LuaApiDescription("使用 Prefab 克隆一个新的对象，并添加至指定变换的子级", "返回生成的新对象")]
        [LuaApiParamDescription("prefab", "Prefab")]
        [LuaApiParamDescription("parent", "父级对象")]
        [LuaApiParamDescription("name", "新的对象的名字")]
        public static GameObject CloneNewObjectWithParent(GameObject prefab, Transform parent, string name)
        {
            if(prefab == null) {
                GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, "CloneUtils", "Can not clone {0}, The prefab is null", name);
                return null;
            }
            GameObject go = Object.Instantiate(prefab, parent);
            go.name = name;
            return go;
        }
        /// <summary>
        /// 使用 Prefab 克隆一个新的对象，并添加至指定变换的子级
        /// </summary>
        /// <param name="prefab">Prefab</param>
        /// <param name="parent">父级对象</param>
        /// <returns>返回生成的新对象</returns>
        [LuaApiDescription("使用 Prefab 克隆一个新的对象，并添加至指定变换的子级", "返回生成的新对象")]
        [LuaApiParamDescription("prefab", "Prefab")]
        [LuaApiParamDescription("parent", "父级对象")]
        public static GameObject CloneNewObjectWithParent(GameObject prefab, Transform parent)
        {
            if(prefab == null) {
                GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, "CloneUtils", "Can not clone object, The prefab is null");
                return null;
            }
            return Object.Instantiate(prefab, parent);
        }
        /// <summary>
        /// 使用 Prefab 克隆一个新的对象，并添加至指定变换的子级
        /// </summary>
        /// <param name="prefab">Prefab</param>
        /// <param name="parent">父级对象</param>
        /// <param name="name">新的对象的名字</param>
        /// <param name="active">是否激活该对象</param>
        /// <returns>返回生成的新对象</returns>
        [LuaApiDescription("使用 Prefab 克隆一个新的对象，并添加至指定变换的子级", "返回生成的新对象")]
        [LuaApiParamDescription("prefab", "Prefab")]
        [LuaApiParamDescription("parent", "父级对象")]
        [LuaApiParamDescription("name", "新的对象的名字")]
        [LuaApiParamDescription("active", "是否激活该对象")]
        public static GameObject CloneNewObjectWithParent(GameObject prefab, Transform parent, string name, bool active)
        {
            if(prefab == null) {
                GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotProvide, "CloneUtils", "Can not clone {0}, The prefab is null", name);
                return null;
            }
            GameObject go = Object.Instantiate(prefab, parent);
            go.name = name;
            go.SetActive(active);
            return go;
        }

        /// <summary>
        /// 克隆一个新的空对象
        /// </summary>
        /// <param name="name">新的对象的名字</param>
        /// <returns>返回生成的新对象</returns>
        [LuaApiDescription("克隆一个新的空对象", "返回生成的新对象")]
        [LuaApiParamDescription("name", "新的对象的名字")]
        public static GameObject CreateEmptyObject(string name)
        {
            GameObject go = Object.Instantiate(GameStaticResourcesPool.PrefabEmpty, GameManager.Instance.transform);
            go.name = name;
            return go;
        }
        /// <summary>
        /// 克隆一个新的空对象，并添加至指定变换的子级
        /// </summary>
        /// <param name="parent">指定父级对象</param>
        /// <param name="name">新的对象的名字</param>
        /// <returns>返回生成的新对象</returns>
        [LuaApiDescription("克隆一个新的空对象，并添加至指定变换的子级", "返回生成的新对象")]
        [LuaApiParamDescription("parent", "指定父级对象")]
        [LuaApiParamDescription("name", "新的对象的名字")]
        public static GameObject CreateEmptyObjectWithParent(Transform parent, string name)
        {
            GameObject go = Object.Instantiate(GameStaticResourcesPool.PrefabEmpty, parent);
            go.name = name;
            return go;
        }
        /// <summary>
        /// 克隆一个新的空对象，并添加至指定变换的子级
        /// </summary>
        /// <param name="parent">指定父级对象</param>
        /// <returns>返回生成的新对象</returns>
        [LuaApiDescription("克隆一个新的空对象，并添加至指定变换的子级", "指定父级对象")]
        [LuaApiParamDescription("parent", "指定父级对象")]
        public static GameObject CreateEmptyObjectWithParent(Transform parent)
        {
            return Object.Instantiate(GameStaticResourcesPool.PrefabEmpty, parent);
        }

        /// <summary>
        /// 生成一个空的UI对象
        /// </summary>
        /// <param name="name">指定新对象的名称</param>
        /// <returns>返回生成的新对象</returns>
        [LuaApiDescription("生成一个空的UI对象", "返回生成的新对象")]
        [LuaApiParamDescription("name", "指定新对象的名称")]
        public static GameObject CreateEmptyUIObject(string name)
        {
            GameObject go = Object.Instantiate(GameStaticResourcesPool.PrefabUIEmpty, GameManager.Instance.GameCanvas.transform);
            go.name = name;
            return go;
        }
        /// <summary>
        /// 生成一个空的UI对象并添加至指定变换的子级
        /// </summary>
        /// <param name="parent">指定父级对象</param>
        /// <param name="name">指定新对象的名称</param>
        /// <returns>返回生成的新对象</returns>
        [LuaApiDescription("生成一个空的UI对象并添加至指定变换的子级", "返回生成的新对象")]
        [LuaApiParamDescription("parent", "指定父级对象")]
        [LuaApiParamDescription("name", "指定新对象的名称")]
        public static GameObject CreateEmptyUIObjectWithParent(Transform parent, string name)
        {
            GameObject go = Object.Instantiate(GameStaticResourcesPool.PrefabUIEmpty, parent);
            go.name = name;
            return go;
        }
        /// <summary>
        /// 生成一个空的UI对象并添加至指定变换的子级
        /// </summary>
        /// <param name="parent">指定父级对象</param>
        /// <returns>返回生成的新对象</returns>
        [LuaApiDescription("生成一个空的UI对象并添加至指定变换的子级", "返回生成的新对象")]
        [LuaApiParamDescription("parent", "指定父级对象")]
        public static GameObject CreateEmptyUIObjectWithParent(Transform parent)
        {
            return Object.Instantiate(GameStaticResourcesPool.PrefabUIEmpty, parent);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Ballance.LuaHelpers;
using Ballance2.Sys.Debug;
using Ballance2.Sys.Package;
using Ballance2.Sys.UI.Utils;
using Ballance2.Sys.Utils;
using SLua;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameUIPage.cs
* 
* 用途：
* UI页实例
*
* 作者：
* mengyu
*
*/

namespace Ballance2
{
    /// <summary>
    /// UI页实例
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("UI页实例")]
    public class GameUIPage : MonoBehaviour
    {
        public RectTransform ContentHost;
        public VerticalLayoutGroup VerticalLayoutGroup;
        public HorizontalLayoutGroup HorizontalLayoutGroup;
        public string PageName;

        public void Show() {
            gameObject.SetActive(true);
        }
        public void Hide() {
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// 创建指定资源的内容
        /// </summary>
        /// <param name="package">资源所在模块</param>
        /// <param name="resourceName">资源模板路径</param>
        [LuaApiDescription("创建指定资源的内容")]
        [LuaApiParamDescription("package", "资源所在模块")]
        [LuaApiParamDescription("resourceName", "资源模板路径")]
        public void CreateContent(GamePackage package, string resourceName) {
            var go = CloneUtils.CloneNewObjectWithParent(package.GetPrefabAsset(resourceName), ContentHost, PageName);
            if(go == null) {
                GameErrorChecker.SetLastErrorAndLog(GameError.PrefabNotFound, "GameUIPage", "Can not create page content for {0}, The prefab {1} not found", PageName, resourceName);
                return;
            }
            var content = go.GetComponent<RectTransform>();
            if(content != null) {
                UIAnchorPosUtils.SetUIAnchor(content, UIAnchor.Stretch, UIAnchor.Stretch);
                UIAnchorPosUtils.SetUIPos(content, 0, 0, 0, 0);
            }
        }


        /// <summary>
        /// 使用页名称自动创建指定资源的内容
        /// </summary>
        /// <param name="package">资源所在模块</param>
        [LuaApiDescription("使用页名称自动创建指定资源的内容")]
        [LuaApiParamDescription("package", "资源所在模块")]
        public void CreateContent(GamePackage package) {
            CreateContent(package, PageName + ".prefab");
        }
        
        
        /// <summary>
        /// 将内容设置至当前页的内容容器中
        /// </summary>
        /// <param name="content">内容RectTransform</param>
        [LuaApiDescription("将内容设置至当前页的内容容器中")]
        [LuaApiParamDescription("content", "内容RectTransform")]
        public void SetContent(RectTransform content) {
            content.transform.SetParent(ContentHost);
            UIAnchorPosUtils.SetUIAnchor(content, UIAnchor.Stretch, UIAnchor.Stretch);
            UIAnchorPosUtils.SetUIPos(content, 0, 0, 0, 0);
        }
    }
}

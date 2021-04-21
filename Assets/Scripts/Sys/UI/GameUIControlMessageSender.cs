using System.Collections.Generic;
using Ballance.LuaHelpers;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameUIControlMessageSender.cs
* 
* 用途：
* UI事件发送器。该绑定器用在需要发送事件的UI控件上。
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-4-20 创建
*
*/

namespace Ballance2.Sys.UI
{
    /// <summary>
    /// UI事件发送器。该绑定器用在需要发送事件的UI控件上。
    /// </summary>
    [SLua.CustomLuaClass]
    [AddComponentMenu("Ballance/UI/MessageSender")]
    [RequireComponent(typeof(RectTransform))]
    [LuaApiDescription("UI事件发送器。该绑定器用在需要发送事件的UI控件上")]
    public class GameUIControlMessageSender : MonoBehaviour
    {
        /// <summary>
        /// 指定对应UI消息中心名字
        /// </summary>
        [Tooltip("指定对应UI消息中心名字")]
        [LuaApiDescription("指定对应UI消息中心名字")]
        public string MessageCenterName = null;

        public GameUIMessageCenter MessageCenter = null;

        private void Awake() {
            MessageCenter = GameUIMessageCenter.FindGameUIMessageCenter(MessageCenterName);
        }
        private void Start() {
            MessageCenter = GameUIMessageCenter.FindGameUIMessageCenter(MessageCenterName);
        }
        private void OnDestroy() {
        }
        
        /// <summary>
        /// 发送事件至消息中心
        /// </summary>
        /// <param name="name">事件名称</param>
        [LuaApiDescription("发送事件至消息中心")]
        [LuaApiParamDescription("name", "事件名称")]
        public void NotifyEvent(string name) {
            if(MessageCenter != null)
                MessageCenter.NotifyEvent(name);      
        }
    }
}

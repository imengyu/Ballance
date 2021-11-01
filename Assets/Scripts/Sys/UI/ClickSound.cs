using Ballance2.Sys;
using Ballance2.Sys.Services;
using UnityEngine.EventSystems;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* ClickSound.cs
* 
* 用途：
* UI点击声音发生器。用在需要点击声音的UI控件上。
*
* 作者：
* mengyu
*/

namespace Ballance2.Sys.UI
{
    public class ClickSound : UIBehaviour, IPointerDownHandler {

        [Tooltip("声音资源名称。与 GameSoundManager 约定的声音资源路径格式一致。")]
        public string SoundName = "";

        private GameSoundManager GameSoundManager;

        public void OnPointerDown(PointerEventData eventData)
        {
            if(!string.IsNullOrEmpty(SoundName)) 
                GameSoundManager.PlayFastVoice(SoundName, GameSoundType.UI);
        }
        protected override void Start() {
            GameSoundManager = (GameSoundManager)GameSystem.GetSystemService("GameSoundManager");
        } 
    }
}
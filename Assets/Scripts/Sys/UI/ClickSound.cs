using Ballance2.Sys;
using Ballance2.Sys.Services;
using UnityEngine.EventSystems;

namespace Ballance2.Sys.UI
{
    public class ClickSound : UIBehaviour, IPointerDownHandler {
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
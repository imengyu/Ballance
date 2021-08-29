using UnityEngine;
using Ballance2.Sys.Services;

namespace Ballance2.Sys.Tools
{
    [RequireComponent(typeof(AudioSource))]
    public class DyamicLoadAudioResource : MonoBehaviour {
        public string AssetString = "";
        public GameSoundType SoundType = GameSoundType.Normal;

        private AudioSource AudioSource;

        private void Awake() {
            AudioSource = GetComponent<AudioSource>();
            if(GameManager.Instance == null) {
                UnityEngine.Debug.LogError("LoadAudioResource failed: GameSystem not init!");
                return;
            }

            var SoundManager = GameManager.Instance.GetSystemService<GameSoundManager>();
            AudioSource.clip = SoundManager.LoadAudioResource(AssetString);
            SoundManager.RegisterSoundPlayer(SoundType, AudioSource);
        }
    }
}
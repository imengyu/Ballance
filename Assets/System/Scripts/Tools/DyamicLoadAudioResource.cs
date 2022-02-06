using UnityEngine;
using Ballance2.Services;

/*
* Copyright(c) 2021 imengyu
*
* 模块名：     
* ObjectStateBackupUtils.cs
* 
* 用途：
* 动态从 GameSoundManager 载入声音资源。
*
* 作者：
* mengyu
*/

namespace Ballance2.Sys.Tools
{
    [RequireComponent(typeof(AudioSource))]
    public class DyamicLoadAudioResource : MonoBehaviour {

        [Tooltip("声音资源路径，与 GameSoundManager 约定的声音资源路径格式一致。")]
        public string AssetString = "";
        [Tooltip("指定声音所属类型")]
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
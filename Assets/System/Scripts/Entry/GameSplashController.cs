using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using SubjectNerd.Utilities;
using Ballance2.Services.InputManager;
using UnityEngine.UI;
using Ballance2.Services;
using System.Collections;

namespace Ballance2.Entry {

  public class GameSplashController : MonoBehaviour {
    
    public static GameSplashController Instance;

    [Tooltip("视频")]
    [Reorderable("Clips")]
    public List<VideoClip> Clips = null;
    [Tooltip("播放器")]
    public VideoPlayer VideoPlayer = null;
    [Tooltip("播放器")]
    public RawImage GameSplashVideoPlayerImage = null;

    private int currentPlayIndex = 0;
    private bool currentPlaying = false;

    public Services.GameManager.VoidDelegate OnSplashFinish;

    public bool IsPlaying() {
      return currentPlaying;
    }

    private void Start() {
      Instance = this;
      StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart() {
      yield return new WaitForSeconds(0.5f); 

      //如果设置跳过了，则不播放
      if(GameEntry.Instance != null && GameEntry.Instance.DebugMode && GameEntry.Instance.DebugSkipSplash) {
        yield break;
      }

      //跳过按键
      var keyListener = KeyListener.Get(gameObject);
      keyListener.AddKeyListen(KeyCode.Escape, KeyCode.Backspace, (key, down) => PlayNext());

      //开始播放视频
      if(Clips != null && Clips.Count > 0) {
        currentPlaying = true;

        yield return new WaitForSeconds(0.5f); 
        
        GameSplashVideoPlayerImage.gameObject.SetActive(true);
        //开始播放视频
        VideoPlayer.loopPointReached += (player) => PlayNext();
        VideoPlayer.clip = Clips[currentPlayIndex];
        VideoPlayer.Play();
      } else {
        gameObject.SetActive(false);
      }
    }

    private void PlayNext() {
      VideoPlayer.Stop();
      if(currentPlayIndex < Clips.Count - 1) {
        currentPlayIndex++;
        VideoPlayer.clip = Clips[currentPlayIndex];
        VideoPlayer.Play();
      } else {
        currentPlaying = false;
        if(GameManager.Instance != null) {
          try {
            GameManager.Instance.GetSystemService<GameUIManager>().MaskBlackFadeIn(0.2f);
            GameManager.Instance.Delay(0.3f, () => {
              gameObject.SetActive(false);
              OnSplashFinish?.Invoke();
            });
          } catch(System.Exception e) {
            Log.E("GameSplashController", "OnSplashFinish invoke failed " + e.ToString());
            gameObject.SetActive(false);
          }
        }
        else {
          gameObject.SetActive(false);
          OnSplashFinish?.Invoke();
        }
        
      }
    }
  }
}
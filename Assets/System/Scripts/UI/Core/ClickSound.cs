using UnityEngine.EventSystems;
using UnityEngine;
using Ballance2.Services;

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

namespace Ballance2.UI.Core
{
  /// <summary>
  /// 点击触发声音组件
  /// </summary>
  [Tooltip("点击触发声音组件")]
  [AddComponentMenu("Ballance/UI/Controls/ClickSound")]
  [SLua.CustomLuaClass]
  public class ClickSound : UIBehaviour, ISelectHandler
  {
    [Tooltip("声音资源名称。与 GameSoundManager 约定的声音资源路径格式一致。")]
    public string SoundName = "";

    private GameSoundManager GameSoundManager;

    protected override void Start()
    {
      GameSoundManager = (GameSoundManager)GameSystem.GetSystemService("GameSoundManager");
    }

    [SLua.DoNotToLua]
    public void OnSelect(BaseEventData eventData)
    {
      if (!string.IsNullOrEmpty(SoundName))
        GameSoundManager.PlayFastVoice(SoundName, GameSoundType.UI);
    }
  }
}
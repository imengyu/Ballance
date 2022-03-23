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
  [LuaApiNoDoc]
  public class ClickSound : UIBehaviour, ISelectHandler, IPointerClickHandler
  {
    [Tooltip("声音资源名称。与 GameSoundManager 约定的声音资源路径格式一致。")]
    public string SoundName = "";
    [Tooltip("是否在选择时播放声音。")]
    public bool HasSelectSound = true;
    [Tooltip("是否在点击时播放声音。")]
    public bool HasClickSound = true;

    private GameSoundManager GameSoundManager;
    private bool LastSelect = false;

    protected override void Start()
    {
      GameSoundManager = (GameSoundManager)GameSystem.GetSystemService("GameSoundManager");
    }

    [SLua.DoNotToLua]
    public void OnSelect(BaseEventData eventData)
    {
      if (HasSelectSound && !string.IsNullOrEmpty(SoundName)) {
        GameSoundManager.PlayFastVoice(SoundName, GameSoundType.UI);
        LastSelect = true;
      }
    }
    [SLua.DoNotToLua]
    public void OnPointerClick(PointerEventData eventData)
    {
      if(LastSelect) {
        LastSelect = false;
        return;
      }
      if (HasClickSound && !string.IsNullOrEmpty(SoundName))
        GameSoundManager.PlayFastVoice(SoundName, GameSoundType.UI);
    }
  }
}
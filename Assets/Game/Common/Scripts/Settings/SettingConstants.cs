using System.Collections.Generic;
using Ballance2.Services;
using UnityEngine;
using UnityEngine.Animations;

/*
 * Copyright (c) 2023  mengyu
 * 
 * 模块名：     
 * SettingConstants.cs
 * 
 * 用途：
 * 游戏内设置的名称与默认值定义。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Game
{
  /// <summary>
  /// 游戏内设置的名称与默认值定义。
  /// </summary>
  public class SettingConstants : MonoBehaviour
  {
    public static string SettingsVideo = "video";
    public static string SettingsVideoResolution = "video.resolution";
    public static string SettingsVideoFullScreen = "video.fullScreen";
    public static string SettingsVideoQuality = "video.quality";
    public static string SettingsVideoVsync = "video.vsync";
    public static string SettingsVideoCloud = "video.cloud";
    public static string SettingsVoice = "voice";
    public static string SettingsVoiceBackground = "voice.background";
    public static string SettingsVoiceUI = "voice.ui";
    public static string SettingsVoiceMain = "voice.main";
    public static string SettingsControl = "control";
    public static string SettingsControlKeySize = "control.key.size";
    public static string SettingsControlKeyFront = "control.key.front";
    public static string SettingsControlKeyUp = "control.key.up";
    public static string SettingsControlKeyDown = "control.key.dwon";
    public static string SettingsControlKeyBack = "control.key.back";
    public static string SettingsControlKeyLeft = "control.key.left";
    public static string SettingsControlKeyRight = "control.key.right";
    public static string SettingsControlKeyUpCam = "control.key.up_cam";
    public static string SettingsControlKeyRoate = "control.key.roate";
    public static string SettingsControlKeyScreenShort = "control.key.screen_short";
    public static string SettingsControlKeyToggleView = "control.key.toggle_view";
    public static string SettingsControlKeyToggleFPS = "control.key.toggle_fps";
    public static string SettingsControllerActionMove = "controller.action.move";
    public static string SettingsControllerActionFly = "controller.action.fly";
    public static string SettingsControllerActionOverlook = "controller.action.overlook";
    public static string SettingsControllerActionRotateCamLeft = "controller.action.camleft";
    public static string SettingsControllerActionRotateCamRight = "controller.action.camright";
    public static string SettingsControlKeyReverse = "control.key.reverse";
    public static string SettingsControlKeypad = "control.keypad";

    /// <summary>
    /// 获取设置默认值
    /// </summary>
    /// <typeparam name="int">键</typeparam>
    /// <typeparam name="object">值</typeparam>
    /// <returns></returns>
    public static Dictionary<string, object> SettingsDefaultValues = new Dictionary<string, object>() {
      { SettingsVoiceBackground, 20.0f },
      { SettingsVoiceUI, 80.0f },
      { SettingsVoiceMain, 100.0f },
      { SettingsControlKeypad, "BaseCenter" },
      { SettingsControlKeyReverse, false },
      { SettingsVideoCloud, true },
      { SettingsControlKeySize, 90.0f },
    };
  }
}
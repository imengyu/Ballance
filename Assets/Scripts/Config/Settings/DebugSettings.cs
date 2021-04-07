#if UNITY_EDITOR
using System;
using UnityEditor;
#endif
using UnityEngine;

/*
* Copyright (c) 2020  mengyu
* 
* 模块名： 
* DebugSettings.cs
* 
* 用途：
* 获取调试设置。
* 你可以在Unity编辑器中点击 Ballance/开发设置/Debug Settings 菜单来配置自己的开发设置。
* 
* 作者：
* mengyu
* 
* 更改历史：
* 2020-6-12 创建
* 
*/

namespace Ballance2.Config.Settings
{
	/// <summary>
	/// 调试设置
	/// </summary>
	public class DebugSettings : ScriptableObject
	{
		[Tooltip("设置 Ballance 调试输出 组与模块的文件夹路径")]
		public string DebugFolder = "";
		[Tooltip("设置是否启用System调试测试")]
		public bool EnableSystemDebugTests = true;
		[Tooltip("设置系统初始化文件加载方式")]
		public LoadResWay SystemInitLoadWay = LoadResWay.InDebugFolder;
		[Tooltip("设置模块文件加载方式")]
		public LoadResWay PackageLoadWay = LoadResWay.InDebugFolder;

		private static DebugSettings _instance = null;

		/// <summary>
		/// 获取调试设置实例
		/// </summary>
		public static DebugSettings Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = Resources.Load<DebugSettings>("DebugSettings");
#if UNITY_EDITOR
					if (_instance == null)
					{
						_instance = CreateInstance<DebugSettings>();
						try
						{
							AssetDatabase.CreateAsset(_instance, "Assets/Resources/DebugSettings.asset");
						}
						catch(Exception e)
						{
							Debug.LogError("CreateInstance DebugSetting.asset failed!" + e.Message + "\n\n" + e.ToString());
						}
					}
#endif
				}
				return _instance;
			}
		}

#if UNITY_EDITOR 
		[MenuItem("Ballance/设置/开发设置", priority = 298)]
		public static void Open()
		{
			Selection.activeObject = Instance;
		}
#endif

	}

	public enum LoadResWay
    {
		InDebugFolder,
		InUnityEditorProject,
    }
}
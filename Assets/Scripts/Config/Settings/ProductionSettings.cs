#if UNITY_EDITOR
using System;
using UnityEditor;
#endif
using UnityEngine;

/*
* Copyright (c) 2020  mengyu
* 
* 模块名： 
* ProductionSettings.cs
* 
* 用途：
* 发布所用静态设置
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
	/// 发布所用静态设置
	/// </summary>
	public class ProductionSettings : ScriptableObject
	{





		private static ProductionSettings _instance = null;

		/// <summary>
		/// 获取发布设置实例
		/// </summary>
		public static ProductionSettings Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = Resources.Load<ProductionSettings>("ProductionSettings");
#if UNITY_EDITOR
					if (_instance == null)
					{
						_instance = CreateInstance<ProductionSettings>();
						try
						{
							AssetDatabase.CreateAsset(_instance, "Assets/Resources/ProductionSettings.asset");
						}
						catch(Exception e)
						{
							Debug.LogError("CreateInstance ProductionSettings.asset failed!" + e.Message + "\n\n" + e.ToString());
						}
					}
#endif
				}
				return _instance;
			}
		}

#if UNITY_EDITOR 
		[MenuItem("Ballance/设置/Production Settings", priority = 298)]
		public static void Open()
		{
			Selection.activeObject = Instance;
		}
#endif

	}
}
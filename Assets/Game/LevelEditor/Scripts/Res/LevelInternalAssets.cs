using AYellowpaper.SerializedCollections;
using Ballance2.Base;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Ballance2.Game.LevelEditor
{
  public class LevelInternalAssets : GameSingletonBehavior<LevelInternalAssets> 
  {
    [SerializeField]
    public List<LevelInternalProviderAsset> InternalAssets;
    [SerializeField]
    public SerializedDictionary<string, LevelInternalFixedProviderAsset> InternalModuls;

    public LevelDynamicModelAsset[] LoadAll()
    {
      var result = new List<LevelDynamicModelAsset>();
      foreach (var item in InternalAssets)
      {
        var key = item.Prefab.name;
        result.Add(new LevelDynamicModelAsset() {
          SourceType = LevelDynamicModelSource.Package,
          SourcePath = $"core:{key}",
          Category = item.Category,
          SubCategory = item.SubCategory,
          SubModelRefs = item.SubModelRefs,
          ObjName = key,
          OnlyOne = item.OnlyOne,
          HiddenInContentSelector = item.HiddenInContentSelector,
          Loaded = true,
          Prefab = item.Prefab,
          Name = $"I18N:core.editor.internalmodul.{key}.Name",
          Desc = $"I18N:core.editor.internalmodul.{key}.Desc",
          PreviewImage = item.Preview,
        });
      }
      foreach (var item in InternalModuls)
      {
        result.Add(new LevelDynamicModelAsset() {
          SourceType = LevelDynamicModelSource.Package,
          SourcePath = $"core:{item.Key}",
          Category = LevelDynamicModelCategory.Moduls,
          SubCategory = item.Value.SubCategory,
          SubModelRefs = item.Value.SubModelRefs,
          ObjName = item.Key,
          OnlyOne = item.Value.OnlyOne,
          HiddenInContentSelector = item.Value.HiddenInContentSelector,
          Loaded = true,
          Prefab = item.Value.Prefab,
          Name = $"I18N:core.editor.internalmodul.{item.Key}.Name",
          Desc = $"I18N:core.editor.internalmodul.{item.Key}.Desc",
          PreviewImage = item.Value.Preview,
        });
      }
      return result.ToArray();
    }
  }

  /// <summary>
  /// 定义模组公开的资源，例如路面，钢轨，机关等等
  /// </summary>
  [Serializable]
  public class LevelProviderAsset
  {
    /// <summary>
    /// 名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 说明文字
    /// </summary>
    public string Desc;
    /// <summary>
    /// 创建物体时使用的名称
    /// </summary>
    public string ObjName;
    /// <summary>
    /// 预览图片，如果不提供，则系统尝试使用预览实时截图
    /// </summary>
    public Sprite Preview;
    /// <summary>
    /// 预制体
    /// </summary>
    public GameObject Prefab;
    /// <summary>
    /// 资源类型
    /// </summary>
    public LevelDynamicModelCategory Category;
    /// <summary>
    /// 资源分组，为空则为默认
    /// </summary>
    public string SubCategory;
    /// <summary>
    /// 是否在关卡中只允许添加一个实例
    /// </summary>
    public bool OnlyOne = false;
    /// <summary>
    /// 是否在内容选择器中隐藏
    /// </summary>
    public bool HiddenInContentSelector = false;
    /// <summary>
    /// 子模型引用
    /// </summary>
    public List<LevelProviderSubModelRefAsset> SubModelRefs = new List<LevelProviderSubModelRefAsset>();
  }
  [Serializable]
  public class LevelProviderSubModelRefAsset
  {
    public Vector3 Position = Vector3.zero;
    public Vector3 EulerAngles = Vector3.zero;
    public Vector3 Scale = Vector3.one;
    public string Path;
    public bool Enable = true;
  }
 
  [Serializable]
  public class LevelInternalProviderAsset
  {
    public Sprite Preview;
    public GameObject Prefab;
    public string SubCategory;
    public LevelDynamicModelCategory Category;
    public List<LevelProviderSubModelRefAsset> SubModelRefs = new List<LevelProviderSubModelRefAsset>();
    public bool OnlyOne = false;
    public bool HiddenInContentSelector = false;
  }  
  [Serializable]
  public class LevelInternalFixedProviderAsset
  {
    public Sprite Preview;
    public GameObject Prefab;
    public string SubCategory;
    public List<LevelProviderSubModelRefAsset> SubModelRefs = new List<LevelProviderSubModelRefAsset>();
    public bool OnlyOne = false;
    public bool HiddenInContentSelector = false;
  }


}
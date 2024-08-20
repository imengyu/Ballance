using AYellowpaper.SerializedCollections;
using Ballance2.Base;
using UnityEngine;
using System;
using System.Collections.Generic;
using SubjectNerd.Utilities;
using Newtonsoft.Json;

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
          Tag = item.Tag,
          SubModelRefs = item.SubModelRefs,
          ObjName = key,
          OnlyOne = item.OnlyOne,
          HiddenInContentSelector = item.HiddenInContentSelector,
          HiddenPlaceholderRender = item.HiddenPlaceholderRender,
          ObjTarget = item.ObjTarget,
          IntitalScale = item.IntitalScale,
          IntitalEulerAngles = item.IntitalEulerAngles,
          Loaded = true,
          Prefab = item.Prefab,
          Name = $"I18N:core.editor.internalmodul.{key}.Name",
          Desc = $"I18N:core.editor.internalmodul.{key}.Desc",
          PreviewImage = item.Preview,
          ScenseGizmePreviewImage = item.ScenseGizmePreviewImage,
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
          ScenseGizmePreviewImage = item.Value.ScenseGizmePreviewImage,
          ObjName = item.Key,
          OnlyOne = item.Value.OnlyOne,
          ObjTarget = item.Key,
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
    /// 显示在选择条目右上角的醒目标记文字，为空则不显示
    /// </summary>
    public string Tag;
    /// <summary>
    /// 创建物体时使用的名称
    /// </summary>
    public string ObjName;
    /// <summary>
    /// 设置当前物体在关卡中起到什么作用
    /// </summary>
    public string ObjTarget;
    /// <summary>
    /// 预览图片，如果不提供，则系统尝试使用预览实时截图
    /// </summary>
    public Sprite Preview;
    /// <summary>
    /// 机关类型在关卡编辑器中的预览图标，不提供则为默认
    /// </summary>
    public Sprite ScenseGizmePreviewImage;
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
    /// 隐藏渲染占位符
    /// </summary>
    public bool HiddenPlaceholderRender = false;
    /// <summary>
    /// 子模型引用
    /// </summary>
    public List<LevelProviderSubModelRefAsset> SubModelRefs = new List<LevelProviderSubModelRefAsset>();
    /// <summary>
    /// 创建时的初始缩放
    /// </summary>
    public Vector3 IntitalScale;
  }
  [Serializable]
  [JsonObject]
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
    public Sprite ScenseGizmePreviewImage;
    public GameObject Prefab;
    public string SubCategory;
    public string Tag;
    public LevelDynamicModelCategory Category;
    public List<LevelProviderSubModelRefAsset> SubModelRefs = new List<LevelProviderSubModelRefAsset>();
    public bool OnlyOne = false;
    public bool HiddenInContentSelector = false;
    public bool HiddenPlaceholderRender = false;
    public string ObjTarget;
    public Vector3 IntitalScale = Vector3.one;
    public Vector3 IntitalEulerAngles = Vector3.zero;
  }  
  [Serializable]
  public class LevelInternalFixedProviderAsset
  {
    public Sprite Preview;
    public Sprite ScenseGizmePreviewImage;
    public GameObject Prefab;
    public string SubCategory;
    public List<LevelProviderSubModelRefAsset> SubModelRefs = new List<LevelProviderSubModelRefAsset>();
    public bool OnlyOne = false;
    public bool HiddenInContentSelector = false;
  }


}
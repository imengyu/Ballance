using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{  
  /// <summary>
  /// 动态关卡的资源定义
  /// </summary>
  public class LevelDynamicModelAsset
  {
    public LevelDynamicModelCategory Category;
    public string SubCategory;
    public LevelDynamicModelSource SourceType;
    public string SourcePath;
    public List<LevelProviderSubModelRefAsset> SubModelRefs = new List<LevelProviderSubModelRefAsset>();

    public string Name;
    public string Desc;
    public string ObjName;
    public Sprite PreviewImage;
    public Sprite ScenseGizmePreviewImage;
    public bool OnlyOne = false;
    public bool CanDelete = true;
    public bool HiddenInContentSelector = false;

    public GameObject Prefab;
    public bool Loaded = false;
    public bool PreviewImageLoaded = false;

    public override string ToString() 
    {
      return "LevelDynamicModelAsset:" + SourcePath;
    }
  }
  /// <summary>
  /// 资源模型来源
  /// </summary>
  public enum LevelDynamicModelSource
  {
    Package,
    Embed,
  }
  /// <summary>
  /// 资源模型分类
  /// </summary>
  public enum LevelDynamicModelCategory
  {
    UnSet,
    Floors,
    Rails,
    Moduls,
    Decoration,
  }
}

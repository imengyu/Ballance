using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{  
  /// <summary>
  /// 资源自定义配置脚本，继承它并重写 GetConfigueItems 返回自己的配置项。
  /// </summary>
  public class LevelDynamicModelAssetConfigue : MonoBehaviour
  {
    public virtual void OnInit(LevelDynamicModel modelInstance, bool isEditor, bool isNew)
    {

    }
    /// <summary>
    /// 获取可编辑时回调
    /// </summary>
    public virtual List<LevelDynamicModelAssetConfigueItem> GetConfigueItems(LevelDynamicModel modelInstance)
    {
      return new List<LevelDynamicModelAssetConfigueItem>() {
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.sideedit.props.Name",
          Key = "Name",
          Type = "System.String",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => modelInstance.InstanceHost.name,
          OnValueChanged = (v) => modelInstance.InstanceHost.name = (string)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.sideedit.props.Position",
          Key = "Position",
          Type = "UnityEngine.Vector3",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => modelInstance.InstanceHost.transform.localPosition,
          OnValueChanged = (v) => modelInstance.InstanceHost.transform.localPosition = (Vector3)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.sideedit.props.Rotation",
          Key = "Rotation",
          Type = "UnityEngine.Vector3",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => modelInstance.InstanceHost.transform.localEulerAngles,
          OnValueChanged = (v) => modelInstance.InstanceHost.transform.localEulerAngles = (Vector3)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.sideedit.props.Scale",
          Key = "Scale",
          Type = "UnityEngine.Vector3",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => modelInstance.InstanceHost.transform.localScale,
          OnValueChanged = (v) => modelInstance.InstanceHost.transform.localScale = (Vector3)v,
        },
      };
    }
    /// <summary>
    /// 获取机关的特殊处理
    /// </summary>
    /// <returns></returns>
    public virtual LevelDynamicModelAssetModulConfig GetModulConfigue(LevelDynamicModel modelInstance)
    {
      return new LevelDynamicModelAssetModulConfig() {
        NeedActiveSector = modelInstance.ModulRef != null, //有机关实例则允许节数据
      };
    }
  } 
  /// <summary>
  /// 资源自定义配置条目定义
  /// </summary>
  public class LevelDynamicModelAssetConfigueItem
  {
    /// <summary>
    /// 显示在界面中的名称，可翻译
    /// </summary>
    public string Name;
    /// <summary>
    /// 显示在界面中的分组, 翻译字符串由 I18N:core.editor.sideedit.{Group} 获取
    /// </summary>
    public string Group;
    /// <summary>
    /// 用于保存数据用的唯一键值
    /// </summary>
    public string Key;
    /// <summary>
    /// 设置编辑器类型
    /// </summary>
    public string Type;
    /// <summary>
    /// 设置不定时更新值
    /// </summary>
    public bool NoTimingUpdate = false;
    /// <summary>
    /// 设置初始不更新值
    /// </summary>
    public bool NoIntitalUpdate = false;
    /// <summary>
    /// 设置是否当前设置不保存至 Configues 中
    /// </summary>
    public bool NoSaveToConfigues = false;
    /// <summary>
    /// 初始值
    /// </summary>
    public object IntitalValue = null;
    /// <summary>
    /// 传递给编辑器的数据
    /// </summary>
    public object EditorParams = null;
    /// <summary>
    /// 界面获取数据回调
    /// </summary>
    public Func<object> OnGetValue;
    /// <summary>
    /// 数据更改回调
    /// </summary>
    public Action<object> OnValueChanged;
  }
  
  public class LevelDynamicModelAssetModulConfig
  {
    public int FixedActiveSector = 0;
    public bool ActiveSectorSingle = false;
    public bool NeedActiveSector = false;
  }
}

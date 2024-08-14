using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{  
  /// <summary>
  /// 资源自定义配置脚本，继承它并重写各个回调函数，并返回自己的配置项。
  /// </summary>
  public class LevelDynamicModelAssetConfigue : MonoBehaviour
  {
    /// <summary>
    /// 元件初始化事件(编辑器模式与游戏模式都会触发)
    /// </summary>
    /// <param name="modelInstance">实例</param>
    /// <param name="isEditor">确定当前是不是在编辑器模式</param>
    /// <param name="isNew">在编辑器模式时，确定现在是不是第一次把当前对象添加入关卡</param>
    public virtual void OnInit(LevelDynamicModel modelInstance, bool isEditor, bool isNew)
    {

    }
    /// <summary>
    /// 当对象被保存时触发（编辑器模式触发)
    /// </summary>
    /// <param name="modelInstance"></param>
    public virtual void OnSave(LevelDynamicModel modelInstance)
    {

    }
    /// <summary>
    /// 自定义配置项目反序列化（编辑器模式触发)
    /// </summary>
    /// <param name="modelInstance"></param>
    public virtual object OnConfigueLoad(string key, object value)
    {
      return value;
    }
    /// <summary>
    /// 自定义配置项目序列化（编辑器模式触发)
    /// </summary>
    /// <param name="modelInstance"></param>
    public virtual object OnConfigueSave(string key, object value)
    {
      return value;
    }
    /// <summary>
    /// 当对象被选中触发（编辑器模式触发)
    /// </summary>
    /// <param name="onlySelf">是否只有自己</param>
    public virtual void OnEditorSelected(bool onlySelf)
    {

    }
    /// <summary>
    /// 当对象取消选中触发（编辑器模式触发)
    /// </summary>
    public virtual void OnEditorDeselect()
    {

    }
    /// <summary>
    /// 当对象被添加到关卡后触发（编辑器模式触发)
    /// </summary>
    /// <param name="modelInstance"></param>
    public virtual void OnEditorAdd(LevelDynamicModel modelInstance)
    {

    }
    /// <summary>
    /// 当对象被添加到关卡之前触发（编辑器模式触发。注：仅在Prefab上触发，此时未真正加入！）
    /// </summary>
    /// <returns>返回非空字符串，则表示取消添加，返回的字符串会显示给用户</returns>
    public virtual string OnBeforeEditorAdd()
    {
      return "";
    }  
    /// <summary>
    /// 编辑器模式下，进入测试玩模式
    /// </summary>
    public virtual void OnEditorIntoTest(LevelDynamicModel modelInstance)
    {
    }
    /// <summary>
    /// 编辑器模式下，退出测试玩模式
    /// </summary>
    public virtual void OnEditorQuitTest(LevelDynamicModel modelInstance)
    {
    }
    /// <summary>
    /// 当用户尝试删除对象时触发（编辑器模式触发）
    /// </summary>
    /// <param name="modelInstance">实例</param>
    /// <returns>返回非空字符串，则表示取消删除，返回的字符串会显示给用户</returns>
    public virtual string OnBeforeEditorDelete(LevelDynamicModel modelInstance)
    {
      return "";
    }
    /// <summary>
    /// 获取可编辑时回调
    /// </summary>
    public virtual List<LevelDynamicModelAssetConfigueItem> GetConfigueItems(LevelDynamicModel modelInstance, LevelDynamicModelAssetModulConfig modulConfig)
    {
      var list = new List<LevelDynamicModelAssetConfigueItem>() {
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.sideedit.props.Name",
          Key = "Name",
          Type = "String",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => modelInstance.InstanceHost.name,
          OnValueChanged = (v) => modelInstance.InstanceHost.name = (string)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.sideedit.props.Position",
          Key = "Position",
          Type = "Vector3",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => modelInstance.InstanceHost.transform.localPosition,
          OnValueChanged = (v) => modelInstance.InstanceHost.transform.localPosition = (Vector3)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.sideedit.props.Rotation",
          Key = "Rotation",
          Type = "Vector3",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => modelInstance.InstanceHost.transform.localEulerAngles,
          OnValueChanged = (v) => modelInstance.InstanceHost.transform.localEulerAngles = (Vector3)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.sideedit.props.Scale",
          Key = "Scale",
          Type = "Vector3",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => modelInstance.InstanceHost.transform.localScale,
          OnValueChanged = (v) => modelInstance.InstanceHost.transform.localScale = (Vector3)v,
        },
      };
      //小节编辑
      if (modulConfig.NeedActiveSector)
      {
        list.Add(new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.sideedit.props.ActiveSectors",
          Key = "ActiveSectors",
          Type = "SectorsEditor",
          Group = "Extra",
          NoTimingUpdate = true,
          NoSaveToConfigues = true,
          EditorParams = new Dictionary<string, bool>() {
            { "singleSelect", modulConfig.ActiveSectorSingle },
            { "disableSelect", modulConfig.FixedActiveSector != 0 }
          },
          OnGetValue = () => modelInstance.ActiveSectors,
          OnValueChanged = (v) => { 
            modelInstance.ActiveSectors = (List<int>)v;
            modelInstance.ObjectScenseIconRef.UpdateBindModel();
          },
        });
      }
      return list;
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
  /// <summary>
  /// 机关的特殊配置
  /// </summary>
  public class LevelDynamicModelAssetModulConfig
  {
    /// <summary>
    /// 是否固定在某个小节激活，为0则不启用
    /// </summary>
    public int FixedActiveSector = 0;
    /// <summary>
    /// 是否只允许在一个小节激活
    /// </summary>
    public bool ActiveSectorSingle = false;
    /// <summary>
    /// 是否需要选择激活的小节
    /// </summary>
    public bool NeedActiveSector = false;
  }
}

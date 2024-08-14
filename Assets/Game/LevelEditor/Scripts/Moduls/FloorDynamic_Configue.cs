using System.Collections.Generic;
using Ballance2.Game.GamePlay;
using Ballance2.Services.I18N;
using Ballance2.Utils;
using Newtonsoft.Json;
using UnityEngine;
using static Ballance2.Game.LevelEditor.EditorItems.LevelEditorItemSelector;

namespace Ballance2.Game.LevelEditor.Moduls
{
  public class FloorDynamic_Configue : LevelDynamicModelAssetConfigue
  {    
    private LevelDynamicFloorBlockComponent component;
    [JsonObject]
    private class LevelDynamicFloorBlockConfig
    {
      public Vector3 ControlPoint1;
      public Vector3 ControlPoint2;
      public Vector3 ControlPoint3;
      public Vector3 ControlPoint4;
      public float Width;
      public LevelDynamicComponentType Type;
      public LevelDynamicComponentArcType ArcDirection;
    }

    public override void OnInit(LevelDynamicModel modelInstance, bool isEditor, bool isNew)
    {
      component = modelInstance.InstanceRef.GetComponent<LevelDynamicFloorBlockComponent>();
      if (!isNew)
      {
        var config = (LevelDynamicFloorBlockConfig)modelInstance.Configues["Config"];
        component.ControlPoint1 = config.ControlPoint1;
        component.ControlPoint2 = config.ControlPoint2;
        component.ControlPoint3 = config.ControlPoint3;
        component.ControlPoint4 = config.ControlPoint4;
        component.Width = config.Width;
        component.Type = config.Type;
        component.ArcDirection = config.ArcDirection;
      }
    }
    public override object OnConfigueLoad(string key, object value)
    {
      switch (key)
      {
        case "Config":
          return JsonConvert.DeserializeObject<LevelDynamicFloorBlockConfig>((string)value);
      }
      return base.OnConfigueLoad(key, value);
    }
    public override object OnConfigueSave(string key, object value)
    {
      switch(key)
      {
        case "Config":
          return JsonConvert.SerializeObject(value, new VectorConverter());
      }
      return base.OnConfigueSave(key, value);
    }
    public override void OnSave(LevelDynamicModel modelInstance)
    {
      var config = new LevelDynamicFloorBlockConfig();
      config.ControlPoint1 = component.ControlPoint1;
      config.ControlPoint2 = component.ControlPoint2;
      config.ControlPoint3 = component.ControlPoint3;
      config.ControlPoint4 = component.ControlPoint4;
      config.ArcDirection = component.ArcDirection;
      config.Type = component.Type;
      config.Width = component.Width;
      modelInstance.Configues["Config"] = config;
    }
    public override void OnEditorAdd(LevelDynamicModel modelInstance)
    {
      component.Editor = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<LevelDynamicFloorBlockEditor>(component.EditorPrefab, component.transform);
      component.Editor.Floor = component;
      component.EnableEdit = true;
    }

    public override void OnEditorIntoTest(LevelDynamicModel modelInstance)
    {
      component.EnableEdit = false;
    }
    public override void OnEditorQuitTest(LevelDynamicModel modelInstance)
    {
      component.EnableEdit = true;
    }

    public override List<LevelDynamicModelAssetConfigueItem> GetConfigueItems(LevelDynamicModel modelInstance, LevelDynamicModelAssetModulConfig modulConfig)
    {
      var list = base.GetConfigueItems(modelInstance, modulConfig);
      list.Add(new LevelDynamicModelAssetConfigueItem() {
        Name = "I18N:core.editor.sideedit.props.Floor.Type.Name",
        Key = "Type",
        Type = "SelectorEditor",
        Group = "Dynamic",
        EditorParams = new Dictionary<string, object>() {
          { "disableSelect", false },
          { "options", new List<LevelEditorItemSelectorItem>() {
            new LevelEditorItemSelectorItem(I18N.Tr("core.editor.sideedit.props.Floor.Type.Line"), LevelEditorStaticAssets.GetAssetByName<Sprite>("ToolIconsStrait")),
            new LevelEditorItemSelectorItem(I18N.Tr("core.editor.sideedit.props.Floor.Type.Arc"), LevelEditorStaticAssets.GetAssetByName<Sprite>("ToolIconsArc")),
            new LevelEditorItemSelectorItem(I18N.Tr("core.editor.sideedit.props.Floor.Type.Bezier"), LevelEditorStaticAssets.GetAssetByName<Sprite>("ToolIconsBerizer")),
          } },
        },
        NoIntitalUpdate = true,
        OnGetValue = () => (int)component.Type,
        OnValueChanged = (v) => {
          if (component != null && component.Type != (LevelDynamicComponentType)v)
          {
            component.Type = (LevelDynamicComponentType)v;
            component?.Editor.UpdateControllers();
            component.UpdateShape();
          }
        },
      });
      list.Add(new LevelDynamicModelAssetConfigueItem() {
        Name = "I18N:core.editor.sideedit.props.Floor.ArcType.Name",
        Key = "ArcType",
        Type = "SelectorEditor",
        Group = "Dynamic",
        EditorParams = new Dictionary<string, object>() {
          { "disableSelect", false },
          { "options", new List<LevelEditorItemSelectorItem>() {
            new LevelEditorItemSelectorItem("X", LevelEditorStaticAssets.GetAssetByName<Sprite>("ToolIconsNone")),
            new LevelEditorItemSelectorItem("Y", LevelEditorStaticAssets.GetAssetByName<Sprite>("ToolIconsNone")),
          } },
        },
        NoIntitalUpdate = true,
        OnGetValue = () => (int)component.ArcDirection,
        OnValueChanged = (v) => {
          if (component != null && component.ArcDirection != (LevelDynamicComponentArcType)v)
          {
            component.ArcDirection = (LevelDynamicComponentArcType)v;
            component?.Editor.UpdateControllers();
            component.UpdateShape();
          }
        },
      });
      list.Add(new LevelDynamicModelAssetConfigueItem() {
        Name = "I18N:core.editor.sideedit.props.Floor.Width",
        Key = "Width",
        Type = "Float",
        Group = "Dynamic",
        EditorParams = new Dictionary<string, object>() {
          { "minValue", 5f },
          { "stepValue", 2.5f },
        },
        NoIntitalUpdate = true,
        OnGetValue = () => component.Width,
        OnValueChanged = (v) => {
          if (component != null)
          {
            component.Width = (float)v;
            component.UpdateShape();
          }
        },
      });   
      return list;
    }
  }
}
using System.Collections.Generic;
using System.ComponentModel;
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
      public float ControlPoint1ConnectHoleWidth = 0;
      public float ControlPoint2ConnectHoleWidth = 0;
      public int SpiralLevelCount = 3;
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
        component.ControlPoint1ConnectHoleWidth = config.ControlPoint1ConnectHoleWidth;
        component.ControlPoint2ConnectHoleWidth = config.ControlPoint2ConnectHoleWidth;
        component.Width = config.Width;
        component.Type = config.Type;
        component.ArcDirection = config.ArcDirection;
        component.SpiralLevelCount = config.SpiralLevelCount;
        component.UpdateShape();
      }
    }
    public override void OnAfterInit(LevelDynamicModel modelInstance, bool isEditor, bool isNew)
    {
      if (isEditor)
      {
        component.Editor = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<LevelDynamicFloorBlockEditor>(component.EditorPrefab, component.transform.parent, "Editor");
        component.Editor.Floor = component;
        component.Editor.ApplyValueToControllers();
        component.EnableEdit = true;
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
      config.ControlPoint1ConnectHoleWidth = component.ControlPoint1ConnectHoleWidth;
      config.ControlPoint2ConnectHoleWidth = component.ControlPoint2ConnectHoleWidth;
      config.SpiralLevelCount = component.SpiralLevelCount;
      config.ArcDirection = component.ArcDirection;
      config.Type = component.Type;
      config.Width = component.Width;
      modelInstance.Configues["Config"] = config;
    }
    public override void OnClone(LevelDynamicModel modelInstance, LevelDynamicModel reference)
    {
      var refComp = reference.InstanceRef.GetComponent<LevelDynamicFloorBlockComponent>();
      component.Width = refComp.Width;
      component.Type = refComp.Type;
      component.ArcDirection = refComp.ArcDirection;
      component.ControlPoint1 = refComp.ControlPoint1;
      component.ControlPoint2 = refComp.ControlPoint2;
      component.ControlPoint3 = refComp.ControlPoint3;
      component.ControlPoint4 = refComp.ControlPoint4;
      component.ControlPoint1ConnectHoleWidth = refComp.ControlPoint1ConnectHoleWidth;
      component.ControlPoint2ConnectHoleWidth = refComp.ControlPoint2ConnectHoleWidth;
      component.SpiralLevelCount = refComp.SpiralLevelCount;
      component.Editor.ApplyValueToControllers();
      component.Editor.UpdateControllers();
      component.UpdateShape();
    }
    public override void OnCloneed(LevelDynamicModel modelInstance)
    {
      component.Editor.UpdateSnapEnable(false);
    }
    public override void OnEditorIntoTest(LevelDynamicModel modelInstance)
    {
      component.EnableEdit = false;
    }
    public override void OnEditorQuitTest(LevelDynamicModel modelInstance)
    {
      component.EnableEdit = true;
    }

    public override void OnEditorSelected(bool onlySelf)
    {
      component.Editor.UpdateSnapEnable(true);
    }
    public override void OnEditorDeselect()
    {
      component.Editor.UpdateSnapEnable(false);
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
            new LevelEditorItemSelectorItem(I18N.Tr("core.editor.sideedit.props.Floor.Type.Spiral"), LevelEditorStaticAssets.GetAssetByName<Sprite>("ToolIconsSprial")),
          } },
        },
        NeedFlushVisible = true,
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
            new LevelEditorItemSelectorItem("X"),
            new LevelEditorItemSelectorItem("Y"),
          } },
        },
        NoIntitalUpdate = true,
        OnGetValue = () => (int)component.ArcDirection,
        OnGetVisible = () => component.Type == LevelDynamicComponentType.Arc,
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
      list.Add(new LevelDynamicModelAssetConfigueItem()
      {
        Name = "I18N:core.editor.sideedit.props.Floor.ArcRadius",
        Key = "ArcRadius",
        Type = "Float",
        Group = "Dynamic",
        EditorParams = new Dictionary<string, object>() {
          { "minValue", 1.0f },
          { "stepValue", 1.0f },
        },
        NoIntitalUpdate = true,
        OnGetVisible = () => component.Type == LevelDynamicComponentType.Arc || component.Type == LevelDynamicComponentType.Spiral,
        OnGetValue = () => {
          if (component.Type == LevelDynamicComponentType.Arc)
            return Mathf.Abs(component.arcRadius);
          if (component.Type == LevelDynamicComponentType.Spiral)
            return Mathf.Abs(component.spiralRadius);
          return 0.0f;
        },
        OnValueChanged = (v) => {
          if (component != null)
          {
            switch(component.Type)
            {
              case LevelDynamicComponentType.Arc:
                switch(component.ArcDirection)
                {
                  case LevelDynamicComponentArcType.X:
                    component.ControlPoint3.x = (float)v * (component.ControlPoint3.x < 0 ? -1 : 1);
                    component.Editor.ApplyValueToControllers(false);
                    break;
                  case LevelDynamicComponentArcType.Y:
                    component.ControlPoint3.y = (float)v * (component.ControlPoint3.y < 0 ? -1 : 1);
                    component.Editor.ApplyValueToControllers(false);
                    break;
                }
                break;
              case LevelDynamicComponentType.Spiral:
                component.ControlPoint3.x = (float)v * (component.ControlPoint3.x < 0 ? -1 : 1); ;
                component.Editor.ApplyValueToControllers();
                break;
            }
            component.UpdateShape();
          }
        },
      });
      list.Add(new LevelDynamicModelAssetConfigueItem()
      {
        Name = "I18N:core.editor.sideedit.props.Floor.SpiralLevelCount",
        Key = "SpiralLevelCount",
        Type = "Integer",
        Group = "Dynamic",
        EditorParams = new Dictionary<string, object>() {
          { "minValue", 1 },
          { "stepValue", 1 },
        },
        NoIntitalUpdate = true,
        OnGetVisible = () => component.Type == LevelDynamicComponentType.Spiral,
        OnGetValue = () => component.SpiralLevelCount,
        OnValueChanged = (v) => {
          if (component != null)
          {
            component.SpiralLevelCount = (int)v;
            component.UpdateShape();
          }
        },
      });
      return list;
    }
  }
}
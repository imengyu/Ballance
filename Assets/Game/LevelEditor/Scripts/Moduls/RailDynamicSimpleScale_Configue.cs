using Ballance2.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.LevelEditor.Moduls
{
  public class RailDynamicSimpleScale_Configue : LevelDynamicModelAssetConfigue
  {    
    private LevelDynamicRailSimpleScaleComponent component;
    [JsonObject]
    private class RailDynamicSimpleScaleConfig
    {
      public Vector3 ControlPoint1;
      public Vector3 ControlPoint2;
      public Vector3 ControlPoint3;
    }

    public override void OnInit(LevelDynamicModel modelInstance, bool isEditor, bool isNew)
    {
      component = modelInstance.InstanceRef.GetComponent<LevelDynamicRailSimpleScaleComponent>();
      if (!isNew)
      {
        var config = (RailDynamicSimpleScaleConfig)modelInstance.Configues["Config"];
        component.ControlPoint1 = config.ControlPoint1;
        component.ControlPoint2 = config.ControlPoint2;
        component.ControlPoint3 = config.ControlPoint3;
      }
    }
    public override void OnAfterInit(LevelDynamicModel modelInstance, bool isEditor, bool isNew)
    {
      if (isEditor)
      {
        component.Editor = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<LevelDynamicRailSimpleScaleEditor>(component.EditorPrefab, component.transform.parent, "Editor");
        component.Editor.Rail = component;
        component.Editor.ApplyValueToControllers();
        component.EnableEdit = true;
      }
    }
    public override object OnConfigueLoad(string key, object value)
    {
      switch (key)
      {
        case "Config":
          return JsonConvert.DeserializeObject<RailDynamicSimpleScaleConfig>((string)value);
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
      var config = new RailDynamicSimpleScaleConfig();
      config.ControlPoint1 = component.ControlPoint1;
      config.ControlPoint2 = component.ControlPoint2;
      config.ControlPoint3 = component.ControlPoint3;
      modelInstance.Configues["Config"] = config;
    }
    public override void OnClone(LevelDynamicModel modelInstance, LevelDynamicModel reference)
    {
      var refComp = reference.InstanceRef.GetComponent<LevelDynamicRailSimpleScaleComponent>();
      component.ControlPoint1 = refComp.ControlPoint1;
      component.ControlPoint2 = refComp.ControlPoint2;
      component.ControlPoint3 = refComp.ControlPoint3;
      component.Editor.ApplyValueToControllers();
      component.Editor.UpdateControllers();
      component.UpdateShape();
    }
    public override void OnEditorIntoTest(LevelDynamicModel modelInstance)
    {
      component.EnableEdit = false;
    }
    public override void OnEditorQuitTest(LevelDynamicModel modelInstance)
    {
      component.EnableEdit = true;
    }
  }
}
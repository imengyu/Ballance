using Ballance2.Utils;

namespace Ballance2.Game.LevelEditor.Moduls
{
  public class FloorDynamicStatic_Configue : LevelDynamicModelAssetConfigue
  {    
    private LevelDynamicFloorStaticComponent component;

    public override void OnInit(LevelDynamicModel modelInstance, bool isEditor, bool isNew)
    {
      component = modelInstance.InstanceRef.GetComponent<LevelDynamicFloorStaticComponent>();
    }
    public override void OnAfterInit(LevelDynamicModel modelInstance, bool isEditor, bool isNew)
    {
      if (isEditor)
      {
        component.Editor = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<LevelDynamicFloorStaticEditor>(component.EditorPrefab, component.transform.parent, "Editor");
        component.Editor.Floor = component;
        component.Editor.InitControllers();
        component.Editor.UpdateControllers();
        component.EnableEdit = true;
      }
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
  }
}
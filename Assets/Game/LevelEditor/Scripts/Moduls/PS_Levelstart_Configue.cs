using System.Collections.Generic;

namespace Ballance2.Game.LevelEditor.Moduls
{
  public class PS_Levelstart_Configue : LevelDynamicModelAssetConfigue
  {
    public override void OnEditorAdd(LevelDynamicModel modelInstance)
    {
      //设置关卡小节数量
      LevelEditorManager.Instance.SetSectorCountToFitModuls();
    }
    public override List<LevelDynamicModelAssetConfigueItem> GetConfigueItems(LevelDynamicModel modelInstance, LevelDynamicModelAssetModulConfig modulConfig)
    {
      var list = base.GetConfigueItems(modelInstance, modulConfig);
      list.Add(new LevelDynamicModelAssetConfigueItem() {
        Name = "I18N:core.editor.sideedit.props.WithFloor",
        Key = "WithFloor",
        Type = "Checkbox",
        Group = "Extra",
        IntitalValue = true,
        OnValueChanged = (v) => {
          var floor = modelInstance.SubModelRef.Find(p => "Floor_Block_Start" == p.SubObjName);
          if (floor != null)
            floor.InstanceHost.gameObject.SetActive((bool)v);
        },
      });
      return list;
    }
    public override LevelDynamicModelAssetModulConfig GetModulConfigue(LevelDynamicModel modelInstance)
    {
      return new LevelDynamicModelAssetModulConfig() {
        NeedActiveSector = true,
        FixedActiveSector = 1
      };
    } 
  }
}
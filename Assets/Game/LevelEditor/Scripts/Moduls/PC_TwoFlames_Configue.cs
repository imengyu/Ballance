using System.Collections.Generic;

namespace Ballance2.Game.LevelEditor.Moduls
{
  public class PC_TwoFlames_Configue : LevelDynamicModelAssetConfigue
  {    
    public override void OnInit(LevelDynamicModel modelInstance, bool isEditor, bool isNew)
    {
      if (isEditor)
      {
        //设置关卡小节数量
        LevelEditorManager.Instance.SetSectorCountToFitModuls();
      }
    }
    public override List<LevelDynamicModelAssetConfigueItem> GetConfigueItems(LevelDynamicModel modelInstance)
    {
      var list = base.GetConfigueItems(modelInstance);
      list.Add(new LevelDynamicModelAssetConfigueItem() {
        Name = "I18N:core.editor.sideedit.props.WithFloor",
        Key = "WithFloor",
        Type = "System.Boolean",
        Group = "Extra",
        IntitalValue = true,
        OnValueChanged = (v) => {
          var floor = modelInstance.SubModelRef.Find(p => "Floor_Block_CheckPoint" == p.SubObjName);
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
        ActiveSectorSingle = true,
      };
    } 
  }
}
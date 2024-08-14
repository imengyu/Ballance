using System.Collections.Generic;
using Ballance2.Game.GamePlay;

namespace Ballance2.Game.LevelEditor.Moduls
{
  public class PC_TwoFlames_Configue : LevelDynamicModelAssetConfigue
  {    
    public override void OnEditorAdd(LevelDynamicModel modelInstance)
    {
      //设置关卡小节数量
      LevelEditorManager.Instance.SetSectorCountToFitModuls();
    }
    public override string OnBeforeEditorDelete(LevelDynamicModel modelInstance)
    {
      //设置关卡小节数量
      LevelEditorManager.Instance.SetSectorCountToFitModuls();
      return "";
    }
    public override string OnBeforeEditorAdd()
    {
      //检查，如果小节数超出上限则无法继续添加
      if (LevelEditorManager.Instance.LevelCurrent.GetSectorCount() > SectorManager.MAX_SECTOR_COUNT)
        return "I18N:core.editor.messages.TooManySectors";
      return "";
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
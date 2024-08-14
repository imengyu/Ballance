using System.Collections.Generic;
using Ballance2.Game.GamePlay.Moduls;

namespace Ballance2.Game.LevelEditor.Moduls
{
  public class PE_Balloon_Configue : LevelDynamicModelAssetConfigue
  {
    public override List<LevelDynamicModelAssetConfigueItem> GetConfigueItems(LevelDynamicModel modelInstance, LevelDynamicModelAssetModulConfig modulConfig)
    {
      var list = base.GetConfigueItems(modelInstance, modulConfig);
      list.Add(new LevelDynamicModelAssetConfigueItem() {
        Name = "I18N:core.editor.sideedit.props.BallonSound",
        Key = "BallonSound",
        Type = "Checkbox",
        Group = "Extra",
        IntitalValue = true,
        OnValueChanged = (v) => ((PE_Balloon)modelInstance.ModulRef).EnableMusic = (bool)v,
      });
      return list;
    }
  }
}
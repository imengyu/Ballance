
namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorLevelInfoControl : LevelEditorSmallInfoControl 
  {
    protected override LevelDynamicModelAssetConfigueItem[] GetEditItems()
    {
      var editor = LevelEditorManager.Instance;
      return new LevelDynamicModelAssetConfigueItem[] {
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.info.props.firstBall",
          Key = "firstBall",
          Type = "BallSelector",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          NoTimingUpdate = true,
          OnGetValue = () => editor.LevelCurrent.LevelInfo.level.firstBall,
          OnValueChanged = (v) => editor.LevelCurrent.LevelInfo.level.firstBall = (string)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.info.props.levelScore",
          Key = "levelScore",
          Type = "Integer",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          NoTimingUpdate = true,
          OnGetValue = () => editor.LevelCurrent.LevelInfo.level.levelScore,
          OnValueChanged = (v) => editor.LevelCurrent.LevelInfo.level.levelScore = (int)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.info.props.startPoint",
          Key = "startPoint",
          Type = "Integer",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          NoTimingUpdate = true,
          OnGetValue = () => editor.LevelCurrent.LevelInfo.level.startPoint,
          OnValueChanged = (v) => editor.LevelCurrent.LevelInfo.level.levelScore = (int)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.info.props.startLife",
          Key = "startLife",
          Type = "Integer",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          NoTimingUpdate = true,
          OnGetValue = () => editor.LevelCurrent.LevelInfo.level.startLife,
          OnValueChanged = (v) => editor.LevelCurrent.LevelInfo.level.startLife = (int)v,
        },
      };
    }
  }
}

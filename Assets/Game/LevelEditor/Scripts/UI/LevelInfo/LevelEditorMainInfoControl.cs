
namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorMainInfoControl : LevelEditorSmallInfoControl 
  {
    protected override LevelDynamicModelAssetConfigueItem[] GetEditItems()
    {
      var editor = LevelEditorManager.Instance;
      return new LevelDynamicModelAssetConfigueItem[] {
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.info.props.Name",
          Key = "Name",
          Type = "System.String",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => editor.LevelCurrent.LevelInfo.name,
          OnValueChanged = (v) => editor.LevelCurrent.LevelInfo.name = (string)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.info.props.Introduction",
          Key = "Introduction",
          Type = "System.String",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => editor.LevelCurrent.LevelInfo.introduction,
          OnValueChanged = (v) => editor.LevelCurrent.LevelInfo.introduction = (string)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.info.props.Author",
          Key = "Author",
          Type = "System.String",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => editor.LevelCurrent.LevelInfo.author,
          OnValueChanged = (v) => editor.LevelCurrent.LevelInfo.author = (string)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.info.props.Version",
          Key = "Version",
          Type = "System.String",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => editor.LevelCurrent.LevelInfo.version,
          OnValueChanged = (v) => editor.LevelCurrent.LevelInfo.version = (string)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.info.props.Url",
          Key = "Url",
          Type = "System.String",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => editor.LevelCurrent.LevelInfo.url,
          OnValueChanged = (v) => editor.LevelCurrent.LevelInfo.url = (string)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.info.props.AllowPreview",
          Key = "AllowPreview",
          Type = "System.Boolean",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => editor.LevelCurrent.LevelInfo.allowPreview,
          OnValueChanged = (v) => editor.LevelCurrent.LevelInfo.allowPreview = (bool)v,
        },
        new LevelDynamicModelAssetConfigueItem() {
          Name = "I18N:core.editor.info.props.AllowEdit",
          Key = "AllowEdit",
          Type = "System.Boolean",
          Group = "Basic",
          NoIntitalUpdate = true,
          NoSaveToConfigues = true,
          OnGetValue = () => editor.LevelCurrent.LevelInfo.allowEdit,
          OnValueChanged = (v) => editor.LevelCurrent.LevelInfo.allowEdit = (bool)v,
        },
      };
    }
  }
}

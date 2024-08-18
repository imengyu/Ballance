using Ballance2.Services.I18N;
using Ballance2.UI;
using Ballance2.UI.Core.Controls;
using Ballance2.Utils;
using System.Collections.Generic;
using TriLibCore;
using UnityEngine;
using UnityEngine.UI;
using static Ballance2.Game.LevelEditor.EditorItems.LevelEditorItemSelector;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorImportControl : MonoBehaviour 
  {
    public GameObject ImportPreview;
    public Camera PreviewCamera;
    public GameObject CurrentObject;
    public GameObject ChoisePrefab;
    public GameObject ChoiseListContentView;

    private bool mapMat = true;
    private bool error = false;
    private string target = "";

    private AssetLoaderOptions assetLoaderOptions;
    private AssetLoaderFilePicker assetLoaderFilePicker;

    private void Start()
    {
      InitChoiseList();
      LoadChoiseList();
    }

    public List<LevelEditorItemSelectorItem> Choises = new List<LevelEditorItemSelectorItem>();
    public List<string> ChoiseValues = new List<string>();
    private List<UIButtonActiveState> Buttons = new List<UIButtonActiveState>();

    private void InitChoiseList()
    {
      Choises.Add(new LevelEditorItemSelectorItem("I18N:core.editor.categoryNames.Stone", LevelEditorStaticAssets.GetAssetByName<Sprite>("ToolIconsFloors")));
      Choises.Add(new LevelEditorItemSelectorItem("I18N:core.editor.categoryNames.Wood", LevelEditorStaticAssets.GetAssetByName<Sprite>("ToolIconsFloorWoods")));
      Choises.Add(new LevelEditorItemSelectorItem("I18N:core.editor.toolbar.Rails", LevelEditorStaticAssets.GetAssetByName<Sprite>("ToolIconsRails")));
      Choises.Add(new LevelEditorItemSelectorItem("I18N:core.editor.toolbar.Others", LevelEditorStaticAssets.GetAssetByName<Sprite>("ToolIconsOthers")));
      ChoiseValues.Add("Phys_Floors");
      ChoiseValues.Add("Phys_FloorWoods");
      ChoiseValues.Add("Phys_FloorRails");
      ChoiseValues.Add("");
    }
    private void LoadChoiseList()
    {
      Buttons.Clear();
      ChoiseListContentView.transform.DestroyAllChildren();
      for (int i = 0; i < Choises.Count; i++)
      {
        var item = Choises[i];
        var index = i;
        var btn = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<UIButtonActiveState>(ChoisePrefab, ChoiseListContentView.transform);
        btn.gameObject.SetActive(true);
        btn.SetActive(target == ChoiseValues[index]);
        btn.onClick = () => {
          foreach (var item in Buttons)
            item.SetActive(false);
          target = ChoiseValues[index];
          btn.SetActive(true);
        };
        btn.transform.Find("Text").GetComponent<UIText>().text = item.Title;
        var image = btn.transform.Find("Image").GetComponent<Image>();
        image.gameObject.SetActive(item.Icon != null);
        image.sprite = item.Icon;
        Buttons.Add(btn);
      }
    }

    public void StartImport()
    {
      error = false;
      CurrentObject = null;
      if (assetLoaderOptions == null)
        assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions(supressWarning: true);
      if (assetLoaderFilePicker == null)
        assetLoaderFilePicker = AssetLoaderFilePicker.Create();

      assetLoaderFilePicker.LoadModelFromFilePickerAsync(I18N.Tr("core.editor.import.ChooseAModelFile"), OnLoad, OnMaterialsLoad, OnProgress, OnBeginLoad, OnError, null, assetLoaderOptions);
    }


    public void CancelImport()
    {
      CurrentObject = null;
      gameObject.SetActive(false);
      PreviewCamera.gameObject.SetActive(false);
      ImportPreview.transform.DestroyAllChildren();
    }

    public void SetScale(string a)
    {
      if (float.TryParse(a, out var scale) && scale > 0.01f && scale < 100f)
      {
        if (CurrentObject != null)
          CurrentObject.transform.localScale = new Vector3 (scale, scale, scale);
      }
    }
    public void SetMapMaterial(bool enable)
    {
      mapMat = enable;
    }
    public void ConfirmImport()
    {

    }

    // This event is called when the model is about to be loaded.
    // You can use this event to do some loading preparation, like showing a loading screen in platforms without threading support.
    // This event receives a Boolean indicating if any file has been selected on the file-picker dialog.
    private void OnBeginLoad(bool anyModelSelected)
    {
      if (anyModelSelected)
      {
        gameObject.SetActive(true);
        LevelEditorManager.Instance.LevelEditorUIControl.ShowLoading();
      }
    }

    // This event is called when the model loading progress changes.
    // You can use this event to update a loading progress-bar, for instance.
    // The "progress" value comes as a normalized float (goes from 0 to 1).
    // Platforms like UWP and WebGL don't call this method at this moment, since they don't use threads.
    private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
    {
      LevelEditorManager.Instance.LevelEditorUIControl.UpdateLoading(
        I18N.TrF("core.editor.import.LoadingModel", "", Mathf.Floor(progress * 100))
      );
    }

    // This event is called when there is any critical error loading your model.
    // You can use this to show a message to the user.
    private void OnError(IContextualizedError contextualizedError)
    {
      LevelEditorManager.Instance.LevelEditorUIControl.HideLoading();
      LevelEditorManager.Instance.LevelEditorUIControl.Alert(
        "I18N:core.editor.import.LoadingModelFailedTitle", 
        I18N.TrF("core.editor.import.LoadingModelFaileDesc", "", contextualizedError.GetInnerException().ToString()), 
        LevelEditorConfirmIcon.Error
      );
    }

    // This event is called when all model GameObjects and Meshes have been loaded.
    // There may still Materials and Textures processing at this stage.
    private void OnLoad(AssetLoaderContext assetLoaderContext)
    {
      PreviewCamera.gameObject.SetActive(true);
      ImportPreview.transform.DestroyAllChildren();
      CurrentObject = assetLoaderContext.RootGameObject;
      assetLoaderContext.RootGameObject.transform.SetParent(ImportPreview.transform);
      GameObjectUtils.SetLayerRecursively(assetLoaderContext.RootGameObject, 12);
    }

    // This event is called after OnLoad when all Materials and Textures have been loaded.
    // This event is also called after a critical loading error, so you can clean up any resource you want to.
    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
      if (!error)
      {
        //
        LevelEditorManager.Instance.LevelEditorUIControl.HideLoading();
      }
    }
  }
}
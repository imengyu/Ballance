using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.UI;
using Ballance2.UI.Core.Controls;
using Ballance2.Utils;
using System.Collections.Generic;
using System.IO;
using TMPro;
using TriLibCore;
using UnityEngine;
using UnityEngine.UI;
using static Ballance2.Game.LevelEditor.EditorItems.LevelEditorItemSelector;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorImportControl : MonoBehaviour 
  {
    public GameObject ImportPreview;
    public GameObject PreviewCameraHost;
    public GameObject CurrentObject;
    public GameObject ChoisePrefab;
    public GameObject ChoiseListContentView;
    public TMP_Text TextStatus;
    public TMP_InputField InputFieldScale;
    public TMP_InputField InputFieldName;
    public GameObject PanelWarn;
    public TMP_Text TextWarn;
    public GameObject ScaleRefBall;
    public GameObject BorderBox;

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
      GameManager.Instance.Delay(0.5f, () => LayoutRebuilder.ForceRebuildLayoutImmediate(ChoiseListContentView.transform as RectTransform));
    }

    public void StartImport()
    {
      error = false; 
      duplicateChecked = false;
      CurrentObject = null;
      TextStatus.text = string.Empty;
      InputFieldName.text = string.Empty;
      InputFieldScale.text = "1";
      PanelWarn.gameObject.SetActive(false);
      BorderBox.gameObject.SetActive(false);
      if (assetLoaderOptions == null)
      {
        assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions(supressWarning: true);
        assetLoaderOptions.ReadEnabled = true;
        assetLoaderOptions.MarkTexturesNoLongerReadable = false;
        assetLoaderOptions.ImportCameras = false;
        assetLoaderOptions.ImportLights = false;
        assetLoaderOptions.ImportVisibility = false;
        assetLoaderOptions.UseUnityNativeNormalCalculator = true;
      }
      if (assetLoaderFilePicker == null)
        assetLoaderFilePicker = AssetLoaderFilePicker.Create();
      assetLoaderFilePicker.LoadModelFromFilePickerAsync(I18N.Tr("core.editor.import.ChooseAModelFile"), OnLoad, OnMaterialsLoad, OnProgress, OnBeginLoad, OnError, null, assetLoaderOptions);
    }

    private bool duplicateChecked = false;
    private int lastModelsCount = 0;
    private int lastLoadedMaterialsCount = 0;
    private int lastLoadedCompoundTexturesCount = 0;

    private void CalcModelBorderBox()
    {
      //计算模型包围盒
      string boundBoxString;
      var boundBox = new Bounds();
      for (var i = 0; i < CurrentObject.transform.childCount; i++)
      {
        var child = CurrentObject.transform.GetChild(i);
        var render = child.gameObject.GetComponent<Renderer>();
        if (render != null)
          boundBox.Encapsulate(render.bounds);
      }
      boundBoxString = boundBox.ToString();

      //模型大小过小或者过大，进行提示
      if (boundBox.extents.x <= 0.1f || boundBox.extents.y <= 0.1f || boundBox.extents.z <= 0.1f)
        ShowWarn(I18N.Tr("core.editor.import.ModelTooSmall"));
      else if (boundBox.extents.x > 1000f || boundBox.extents.y >= 1000f || boundBox.extents.z >= 1000f)
        ShowWarn(I18N.Tr("core.editor.import.ModelTooSmall"));
      else
        HideWarn();

      //显示外框
      BorderBox.SetActive(true);
      BorderBox.transform.localScale = boundBox.size / 2;
      BorderBox.transform.localPosition = boundBox.center;

      //更新状态
      TextStatus.text = I18N.TrF("core.editor.import.ModelStatus", "",
        lastModelsCount,
        lastLoadedMaterialsCount,
        lastLoadedCompoundTexturesCount,
        boundBoxString
      );
    }

    private void HideWarn()
    {
      PanelWarn.gameObject.SetActive(false);
    }
    private void ShowWarn(string text)
    {
      PanelWarn.gameObject.SetActive(true);
      TextWarn.text = text;
    }

    public void CancelImport()
    {
      if (CurrentObject != null)
      {
        Object.Destroy(CurrentObject);
        CurrentObject = null;
      }
      gameObject.SetActive(false);
      PreviewCameraHost.gameObject.SetActive(false);
    }

    public void SetScale(string a)
    {
      if (CurrentObject != null)
      {
        if (float.TryParse(a, out var scale) && scale > 0.01f && scale <= 10000f)
        {
          CurrentObject.transform.localScale = new Vector3(scale, scale, scale);
          CalcModelBorderBox();
        }
        else if (InputFieldScale.text != "1")
        {
          InputFieldScale.text = "1";
        }
      }
    }
    public void SetMapMaterial(bool enable)
    {
      mapMat = enable;
    }
    public void SetRefBallVisible(bool enable)
    {
      ScaleRefBall.SetActive(enable);
    }
    public void ConfirmImport()
    {
      var levelEditor = LevelEditorManager.Instance;
      if (CurrentObject != null)
      {
        //检查关卡中是否存在同名资产，如果是，则提示
        var name = InputFieldName.text;
        var existsAsset = levelEditor.LevelCurrent.LevelData.LevelAssets.Find(
          (asset) => asset.SourcePath == $"levelasset:{StringUtils.RemoveSpeicalChars(name)}"
        );
        if (existsAsset != null && !duplicateChecked)
        {
          levelEditor.LevelEditorUIControl.Confirm(
            "",
            I18N.TrF("core.editor.messages.DuplicateImportAsset", "", name), LevelEditorConfirmIcon.Warning,
            onConfirm: (p) =>
            {
              duplicateChecked = true;
              ConfirmImport();
            }
          );
          return;
        }

        //模型从预览中脱节
        CurrentObject.transform.SetParent(null);
        CurrentObject.SetActive(false);
        GameObjectUtils.SetLayerRecursively(CurrentObject, 0);
        //导入保存资源交给LevelEditorManager处理
        gameObject.SetActive(false);
        PreviewCameraHost.gameObject.SetActive(false);
        levelEditor.SaveCustomModelAsset(CurrentObject, name, target, mapMat, CurrentObject.transform.localScale);
        CurrentObject = null;
      }
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
      CancelImport();
    }

    // This event is called when all model GameObjects and Meshes have been loaded.
    // There may still Materials and Textures processing at this stage.
    private void OnLoad(AssetLoaderContext assetLoaderContext)
    {
      PreviewCameraHost.gameObject.SetActive(true);
      CurrentObject = assetLoaderContext.RootGameObject;
      assetLoaderContext.RootGameObject.transform.SetParent(ImportPreview.transform);
      InputFieldName.text = Path.GetFileNameWithoutExtension(assetLoaderContext.Filename);
      GameObjectUtils.SetLayerRecursively(assetLoaderContext.RootGameObject, 12);

      //如果模型只有一个子模型，则移动模型到正中
      if (assetLoaderContext.RootGameObject.transform.childCount == 1)
      {
        var child = assetLoaderContext.RootGameObject.transform.GetChild(0);
        child.position = Vector3.zero;
        child.localScale = Vector3.one;
      }

      TextStatus.text = I18N.TrF("core.editor.import.ModelStatus", "", 
        assetLoaderContext.Models.Count, 
        assetLoaderContext.LoadedMaterials.Count, 
        assetLoaderContext.LoadedCompoundTextures.Count,
        "-"
      );
    }

    // This event is called after OnLoad when all Materials and Textures have been loaded.
    // This event is also called after a critical loading error, so you can clean up any resource you want to.
    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
      if (!error)
      {
        lastModelsCount = assetLoaderContext.Models.Count;
        lastLoadedMaterialsCount = assetLoaderContext.LoadedMaterials.Count;
        lastLoadedCompoundTexturesCount = assetLoaderContext.LoadedCompoundTextures.Count;

        //计算模型包围盒
        CalcModelBorderBox();

        foreach ( var tex in assetLoaderContext.LoadedCompoundTextures)
        {
          Debug.Log(tex.Key + " : " + tex.Value.UnityTexture.isReadable);
        }

        //
        LevelEditorManager.Instance.LevelEditorUIControl.HideLoading();
      }
    }
  }
}
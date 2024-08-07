using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ballance2.Base;
using Ballance2.Game.GamePlay;
using Ballance2.Game.Utils;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.Utils;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ballance2.Game.LevelEditor
{
  /// <summary>
  /// 关卡编辑器
  /// </summary>
  public class LevelEditorManager : GameSingletonBehavior<LevelEditorManager>
  {
    public const string TAG = "LevelEditorManager";

    public LevelEditorUIControl LevelEditorUIControl;
    public Camera LevelEditorCamera;
    public GameObject TransformHandles;
    public GameObject ScenseRoot;
    public GameObject ErrorPrefab;
    public GameObject LevelEditorObjectScenseIconPrefab;
    public Material TransparentMaterial;
    public TextAsset LevelNewJson;

    /// <summary>
    /// 当前加载的关卡实例
    /// </summary>
    public LevelDynamicAssembe LevelCurrent { get; private set; } = null;
    /// <summary>
    /// 当前系统中加载的所有资源（已分类）
    /// </summary>
    public Dictionary<LevelDynamicModelCategory, Dictionary<string, List<LevelDynamicModelAsset>>> LevelAssetsGrouped { get; } = new Dictionary<LevelDynamicModelCategory, Dictionary<string, List<LevelDynamicModelAsset>>>();
    /// <summary>
    /// 当前系统中加载的所有资源
    /// </summary>
    public Dictionary<string, LevelDynamicModelAsset> LevelAssets { get; } = new Dictionary<string, LevelDynamicModelAsset>();

    private bool isFirstInit = true;

    private void Awake()
    {
      LevelEditorUIControl.LevelEditorContentSelection.onPrefabDrop = OnAssetDropDone;
      LevelEditorUIControl.LevelEditorContentSelection.onPrefabInstantiate = OnAssetDropInstance;
      LevelEditorUIControl.LevelEditorContentSelection.onPrefabDragStart = OnAssetDropStart;
      LevelEditorUIControl.LevelEditorContentSelection.onPrefabDragEnd = OnAssetDropEnd;
      LevelEditorUIControl.LevelEditorTransformToolControl.OnDelete = OnObjectDelete;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="levelName"></param>
    public void Init(string levelName)
    {      
      Log.D(TAG, $"Entry with {levelName}");

      if (isFirstInit)
      {
        isFirstInit = false;
        Log.D(TAG, $"Start load all assets");
        LoadAllAssets();
      }
      //如果为空，则让用户新建关卡
      if (string.IsNullOrEmpty(levelName))
        ShowNewLevelDialog();
      else
        LoadLevel(levelName);
      GameManager.Instance.SetGameBaseCameraVisible(false);
      LevelEditorCamera.gameObject.SetActive(true);
      LevelEditorUIControl.SetToolBarMode(LevelEditorUIControl.ToolBarMode.Edit);
      TransformHandles.SetActive(true);
      GameUIManager.Instance.MaskBlackFadeOut(1);
    }
    /// <summary>
    /// 加载关卡
    /// </summary>
    /// <param name="levelName">关卡名称</param>
    public void LoadLevel(string levelName)
    {
      var levelPath = GamePathManager.GetLevelRealPath(levelName, false, true);
      if (!Directory.Exists(levelPath))
      {
        ReportLoadError(I18N.Tr("core.editor.messages.NotExists"));
        return;
      }
      var level = new LevelDynamicAssembe(levelPath);
      StartCoroutine(_Load(level));
    }
    /// <summary>
    /// 卸载关卡
    /// </summary>
    public void UnloadLevel(Action finish) {
      StartCoroutine(_Unload(finish));
    }
    /// <summary>
    /// 新关卡
    /// </summary>
    /// <param name="levelName">关卡名称</param>
    public void NewLevel(string levelName)
    {
      var levelPath = GamePathManager.GetLevelRealPath(levelName, false, true);
      LevelCurrent = new LevelDynamicAssembe(levelPath);
      LevelCurrent.New();
      StartCoroutine(_Load(LevelCurrent));
    }
    /// <summary>
    /// 退出编辑器
    /// </summary>
    public void Quit() {
      UnloadLevel(() => {
        LevelEditorCamera.gameObject.SetActive(false);
        GameManager.Instance.SetGameBaseCameraVisible(true);
        //通知回到menulevel
        GameManager.Instance.RequestEnterLogicScense("MenuLevel");
      });
    }
    /// <summary>
    /// 保存关卡
    /// </summary>
    public void Save() {
      try 
      {
        LevelCurrent.Save();
        LevelEditorUIControl.Alert("I18N:core.editor.messages.SaveSuccess", "");
      } 
      catch(Exception e)
      {
        LevelEditorUIControl.Alert("I18N:core.editor.messages.SaveFailed", e.ToString(), LevelEditorConfirmIcon.Error);
      }
    }

    private void ReportLoadError(string message)
    {
      Log.E(TAG, message);
      LevelEditorUIControl.Alert("I18N:core.editor.messages.LoadFailed", message, LevelEditorConfirmIcon.Error, onConfirm: () => Quit());
    }
    private void ShowNewLevelDialog() {
      Log.D(TAG, $"New Level Dialog");

      LevelEditorUIControl.Confirm(
        "I18N:core.editor.load.New", 
        "I18N:core.editor.load.NewDesc", 
        LevelEditorConfirmIcon.Info, 
        showInput: true, 
        onConfirm: (text) => 
      {
        var finalName = StringUtils.RemoveSpeicalChars(text);
        var levelPath = GamePathManager.GetLevelRealPath(finalName, false, true);
        if (Directory.Exists(levelPath))
          LevelEditorUIControl.Confirm("I18N:core.editor.load.New", "I18N:core.editor.load.NewDesc", LevelEditorConfirmIcon.Info, showInput: true, onConfirm: (_) => {
            NewLevel(finalName);
          }, onCancel: () => {
            ShowNewLevelDialog();
          });
        else
          NewLevel(finalName);
      });
    }

    public void QuitAndAsk()
    {
      LevelEditorUIControl.Confirm(
        "", 
        "I18N:core.editor.messages.QuitAsk", 
        LevelEditorConfirmIcon.Warning, onConfirm: (_) => {
        Quit();
      });
    }
    public string GetUseableName(string baseName)
    {
      if (LevelCurrent == null)
        return baseName;
      for (var i = 1; i < 100; i++)
      {
        var name = $"{baseName}_{i.ToString("D2")}";
        if (LevelCurrent.LevelData.LevelModels.Find((a) => a.Name == name) == null)
          return baseName;
      }
      return $"{baseName}_{CommonUtils.GenRandomID()}";
    }
    public void SetSectorCountToFitModuls()
    {
      //根据系统中拥有的检查点数量设置节数量
      if (LevelCurrent != null)
      {
        var sectorCount = 0;
        foreach (var model in LevelCurrent.LevelData.LevelModels)
        {
          if (model.Asset == "core:PS_FourFlames")
            sectorCount++;
          else if (model.Asset == "core:PC_TwoFlames")
            sectorCount++;
        }
        LevelCurrent.LevelInfo.level.sectorCount = sectorCount;
        LevelEditorUIControl.UpdateStatusText();
      }
    }

    private IEnumerator _LoadAsset(LevelDynamicModelAsset asset) 
    {
      //TODO
      yield break;
    }
    private IEnumerator _Load(LevelDynamicAssembe level) 
    {
      LevelCurrent = level;
      LevelEditorUIControl.ShowLoading();

      Log.D(TAG, $"Start load level {level.LevelDirPath}");

      try {
        level.Load();
      } catch(Exception e) {
        ReportLoadError($"Failed to load info " + e.ToString());
        yield break;
      }
          
      yield return new WaitForSeconds(1);

      var loadCount = 0;
      var loadMissingAssets = new List<LevelDynamicModel>();
      //加载内嵌资源至系统中
      foreach (var item in level.LevelData.LevelAssets)
      {
        if (item.SourceType == LevelDynamicModelSource.Embed)
          AddAssetToList(item);
        loadCount++;
        if (loadCount % 8 == 0)
          yield return new WaitForEndOfFrame();
      }
      //加载资源实例
      var modelsTempMap = new Dictionary<int, LevelDynamicModel>();
      foreach (var item in level.LevelData.LevelModels)
      {
        if (LevelAssets.TryGetValue(item.Asset, out var asset))
          item.AssetRef = asset;
        else
          loadMissingAssets.Add(item);

        //如果对象没有加载，则现在加载
        if (!asset.Loaded && asset.SourceType == LevelDynamicModelSource.Embed)
          yield return StartCoroutine(_LoadAsset(asset));

        item.InstantiateModul(ScenseRoot.transform, true, false);
        item.CanDelete = asset.CanDelete;
        modelsTempMap.Add(item.Uid, item);
        loadCount++;
        if (loadCount % 16 == 0)
          yield return new WaitForEndOfFrame();
      }
      foreach (var item in level.LevelData.LevelModels)
      {
        if (item.ParentUid != 0 && modelsTempMap.TryGetValue(item.ParentUid, out var parentModel))
        {
          parentModel.SubModelRef.Add(item);
          item.SubObjName = item.AssetRef.ObjName;
          item.IsSubObj = true;
          item.CanDelete = false;
          item.InstanceHost.transform.SetParent(parentModel.InstanceHost.transform);
        }
      }
      modelsTempMap.Clear();

      yield return new WaitForEndOfFrame();

      Log.D(TAG, $"Load objects done. All {loadCount} objects");

      //加载天空盒子和灯光颜色
      Material customSkyMat = null;
      if (level.LevelInfo.level.skyBox == "custom")
      {
        var B = TextureUtils.LoadTexture2dFromFile($"{level.LevelDirPath}/CustomSkyBoxB.png", 1024, 1024);
        var F = TextureUtils.LoadTexture2dFromFile($"{level.LevelDirPath}/CustomSkyBoxF.png", 1024, 1024);
        var L = TextureUtils.LoadTexture2dFromFile($"{level.LevelDirPath}/CustomSkyBoxL.png", 1024, 1024);
        var R = TextureUtils.LoadTexture2dFromFile($"{level.LevelDirPath}/CustomSkyBoxR.png", 1024, 1024);
        var T = TextureUtils.LoadTexture2dFromFile($"{level.LevelDirPath}/CustomSkyBoxT.png", 1024, 1024);
        var D = TextureUtils.LoadTexture2dFromFile($"{level.LevelDirPath}/CustomSkyBoxD.png", 1024, 1024);
        if (B == null) Log.W(TAG, "Failed to load customSkyBox.B texture");
        if (F == null) Log.W(TAG, "Failed to load customSkyBox.F texture");
        if (L == null) Log.W(TAG, "Failed to load customSkyBox.L texture");
        if (R == null) Log.W(TAG, "Failed to load customSkyBox.R texture");
        if (D == null) Log.W(TAG, "Failed to load customSkyBox.D texture");

        customSkyMat = SkyBoxUtils.MakeCustomSkyBox(L, R, F, B, D, T);
      }

      GamePlayManager.Instance.CreateSkyAndLight(level.LevelInfo.level.skyBox, customSkyMat, StringUtils.StringToColor(level.LevelInfo.level.lightColor));

      LevelEditorUIControl.Init();
      LevelEditorUIControl.HideLoading();
      LevelEditorUIControl.UpdateStatusText();

      Log.D(TAG, "Load level done");
      yield break;
    }
    private IEnumerator _Unload(Action finish) 
    {
      //界面与控件归位
      LevelEditorUIControl.SetToolBarMode(LevelEditorUIControl.ToolBarMode.None);
      TransformHandles.SetActive(true);

      //移除所有场景元素
      foreach (var item in LevelCurrent.LevelData.LevelModels)
        item.DestroyModul();
      for (int i = 0; i < ScenseRoot.transform.childCount; i++)
      {
        Destroy(ScenseRoot.transform.GetChild(i).gameObject);

        if (i % 128 == 0)
          yield return new WaitForEndOfFrame();
      }
      //关卡元素重置
      if (LevelCurrent != null)
      {
        //移除所有内嵌资源
        foreach (var item in LevelCurrent.LevelData.LevelAssets)
        {
          if (item.SourceType == LevelDynamicModelSource.Embed)
            RemoveAssetFromList(item);
        }
        //移除所有内嵌资源
        foreach (var item in LevelCurrent.LevelData.LevelModels)
          item.InstanceRef = null;
        LevelCurrent = null;
      }

      //天空盒子和灯光颜色重置
      GamePlayManager.Instance.CreateSkyAndLight("A", null, Color.white);

      finish.Invoke();
      yield break;
    }
    private IEnumerator _DropAssetSolve(LevelDynamicModelAsset asset, GameObject go) 
    {
      var model = new LevelDynamicModel() {
        Name = GetUseableName(asset.ObjName),
        Asset = asset.SourcePath,
        AssetRef = asset,
        Position = go.transform.position,
        EulerAngles = go.transform.eulerAngles,
        Scale = go.transform.localScale,
        Uid = ++LevelCurrent.LevelData.LevelObjectId,
        CanDelete = asset.CanDelete,
      };

      if (!asset.Loaded && asset.SourceType == LevelDynamicModelSource.Embed)
        yield return StartCoroutine(_LoadAsset(asset));

      //删除编辑器创建的无用对象
      Destroy(go);

      LevelCurrent.LevelData.LevelModels.Add(model);

      model.InstantiateModul(ScenseRoot.transform, true, true);

      //创建子对象
      foreach (var _subAssetRef in asset.SubModelRefs)
      {
        if (LevelAssets.TryGetValue(_subAssetRef.Path, out var subAsset))
        {
          if (!subAsset.Loaded && subAsset.SourceType == LevelDynamicModelSource.Embed)
            yield return StartCoroutine(_LoadAsset(subAsset));

          var subModel = new LevelDynamicModel() {
            Name = GetUseableName(subAsset.ObjName),
            SubObjName = subAsset.ObjName,
            IsSubObj = true,
            Asset = subAsset.SourcePath,
            AssetRef = subAsset,
            Position = model.InstanceHost.transform.TransformPoint(_subAssetRef.Position),
            EulerAngles = model.InstanceHost.transform.TransformVector(_subAssetRef.EulerAngles),
            Scale = model.InstanceHost.transform.TransformVector(_subAssetRef.Scale),
            Uid = ++LevelCurrent.LevelData.LevelObjectId,
            ParentUid = model.Uid,
            CanDelete = false,
          };
          model.SubModelRef.Add(subModel);
          LevelCurrent.LevelData.LevelModels.Add(subModel);
          subModel.InstantiateModul(model.InstanceHost.transform, true, true);
          if (!_subAssetRef.Enable)
            subModel.InstanceHost.SetActive(false);
        }
        else
        {
          Log.W(TAG, $"Failed to load SubAsset {_subAssetRef.Path} of model {asset}!");
        }
      }

      LevelEditorUIControl.UpdateStatusText();
    }

    private void RemoveAssetFromList(LevelDynamicModelAsset asset)
    {
      if (LevelAssetsGrouped.TryGetValue(asset.Category, out var catgory))
      {
        var SubCategoryName = string.IsNullOrEmpty(asset.SubCategory) ? "Default" : asset.SubCategory;
        if (catgory.TryGetValue(SubCategoryName, out var subCatgory))
          subCatgory.Remove(asset);
      }
      LevelAssets.Remove(asset.SourcePath);
    }
    private void AddAssetToList(LevelDynamicModelAsset asset)
    {
      if (!LevelAssetsGrouped.TryGetValue(asset.Category, out var catgory))
      {
        catgory = new Dictionary<string, List<LevelDynamicModelAsset>>();
        LevelAssetsGrouped.Add(asset.Category, catgory);
      }
      var SubCategoryName = string.IsNullOrEmpty(asset.SubCategory) ? "Default" : asset.SubCategory;
      if (!catgory.TryGetValue(SubCategoryName, out var subCatgory))
      {
        subCatgory = new List<LevelDynamicModelAsset>();
        catgory.Add(SubCategoryName, subCatgory);
      }
      subCatgory.Add(asset);
      LevelAssets.Add(asset.SourcePath, asset);
    }
    private void LoadAllAssets()
    {
      var internalAssets = LevelInternalAssets.Instance.LoadAll();
      foreach (var item in internalAssets)
        if (!item.HiddenInContentSelector)
          AddAssetToList(item);
      var packages = GamePackageManager.Instance.GetLoadedPackages();
      if (packages != null)
      {
        foreach (var package in packages)
        {
          try {
            var assets = package.PackageEntry.OnLevelEditorLoadAssets?.Invoke(package, this);
            if (assets != null)
            {
              foreach (var item in assets)
                if (!item.HiddenInContentSelector)
                  AddAssetToList(new LevelDynamicModelAsset() {
                    SourcePath = $"{package.PackageName}:{item.Name}",
                    SourceType = LevelDynamicModelSource.Package,
                    Category = item.Category,
                    SubCategory = item.SubCategory,
                    SubModelRefs = item.SubModelRefs,
                    OnlyOne = item.OnlyOne,
                    HiddenInContentSelector = item.HiddenInContentSelector,
                    Loaded = true,
                    Prefab = item.Prefab,
                    Name = item.Name,
                    Desc = item.Desc,
                    PreviewImage = item.Preview,
                  });
            }
          } 
          catch (Exception e)
          {
            Log.E("LevelEditorManager.LoadAllAssets", "Failed load assets for mod " + package.PackageName + " :" + e.ToString());
          }
        }
      }
    }
  
    private void OnObjectDelete(LevelDynamicModel[] models)
    {
      foreach (var item in models)
        LevelCurrent.LevelData.LevelModels.Remove(item);
      LevelEditorUIControl.ClearSelectionWhenDelete();
      LevelEditorUIControl.UpdateStatusText();
    }
    private void OnAssetDropInstance(LevelDynamicModelAsset asset, GameObject go)
    {
    }
    private void OnAssetDropEnd()
    {
      LevelEditorUIControl.HideMouseTip();
    }
    private bool OnAssetDropStart(LevelDynamicModelAsset asset)
    {
      if (asset.OnlyOne && LevelCurrent.LevelData.LevelModels.Find((p) => p.AssetRef == asset) != null)
      {
        LevelEditorUIControl.ShowMouseTip("I18N:core.editor.messages.DuplicateObject");
        return true;
      }
      return false;
    }
    private void OnAssetDropDone(LevelDynamicModelAsset asset, GameObject go)
    {
      StartCoroutine(_DropAssetSolve(asset, go));
    }
  }
}

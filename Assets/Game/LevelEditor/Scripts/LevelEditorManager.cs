using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ballance2.Base;
using Ballance2.Game.GamePlay;
using Ballance2.Game.GamePlay.Moduls;
using Ballance2.Game.LevelBuilder;
using Ballance2.Game.Utils;
using Ballance2.Menu;
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
    public Skybox LevelEditorCameraSkyBox;
    public GameObject TransformHandles;
    public GameObject ScenseRoot;
    public GameObject ErrorPrefab;
    public GameObject LevelEditorObjectScenseIconPrefab;
    public Material TransparentMaterial;
    public Material TransparentMaterial2;
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
      GameUIManager.Instance.MaskBlackSet(true);
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

    public void ReportLoadError(string message)
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
        LevelCurrent.SetSectorCountToFitModuls();
        LevelEditorUIControl.UpdateStatusText();
      }
    }
    public void SetLightColor(Color currentColor)
    {
      LevelCurrent.LevelInfo.level.lightColor = "#" + ColorUtility.ToHtmlStringRGB(currentColor);
      GameManager.GameLight.color = currentColor;
    }
    public IEnumerator CreateSky() {
      Material customSkyMat = null;
      if (LevelCurrent.LevelInfo.level.skyBox == "custom")
      {
        Texture B = null, F = null, L = null, R = null, T = null, D = null;
        var result = new EnumeratorResultPacker<Texture2D>();
        yield return StartCoroutine(TextureUtils.LoadTexture2dFromFile($"{LevelCurrent.LevelDirPath}/assets/CustomSkyBoxB.png", result));
        B = result.Result;
        yield return StartCoroutine(TextureUtils.LoadTexture2dFromFile($"{LevelCurrent.LevelDirPath}/assets/CustomSkyBoxF.png", result));
        F = result.Result;
        yield return StartCoroutine(TextureUtils.LoadTexture2dFromFile($"{LevelCurrent.LevelDirPath}/assets/CustomSkyBoxL.png", result));
        L = result.Result;
        yield return StartCoroutine(TextureUtils.LoadTexture2dFromFile($"{LevelCurrent.LevelDirPath}/assets/CustomSkyBoxR.png", result));
        R = result.Result;
        yield return StartCoroutine(TextureUtils.LoadTexture2dFromFile($"{LevelCurrent.LevelDirPath}/assets/CustomSkyBoxT.png", result));
        T = result.Result;
        yield return StartCoroutine(TextureUtils.LoadTexture2dFromFile($"{LevelCurrent.LevelDirPath}/assets/CustomSkyBoxD.png", result));
        D = result.Result;
        customSkyMat = SkyBoxUtils.MakeCustomSkyBox(L, R, F, B, D, T);
      }

      LevelEditorCameraSkyBox.material = GamePlayManager.Instance.CreateSkyAndLight(
        LevelCurrent.LevelInfo.level.skyBox, 
        customSkyMat, 
        StringUtils.StringToColor(LevelCurrent.LevelInfo.level.lightColor)
      );
    }
    public void StartTestMode() 
    {
      LevelEditorUIControl.ShowLoading("I18N:core.editor.messages.PrepareTestMode");
      LevelEditorUIControl.SetToolBarMode(LevelEditorUIControl.ToolBarMode.None);
      StartCoroutine(_IntoTest());
    }
    public void ExitTestMode() 
    {      
      LevelEditorUIControl.Confirm(
        "", 
        "I18N:core.editor.messages.ExitTestModeAsk", 
        LevelEditorConfirmIcon.Warning, onConfirm: (_) => 
        {
          LevelEditorUIControl.ShowLoading("I18N:core.editor.messages.PrepareEditor");
          LevelEditorUIControl.SetToolBarMode(LevelEditorUIControl.ToolBarMode.None);
          LevelEditorUIControl.SetPauseTipShow(false);
          StartCoroutine(_QuitTest());
        }
      );
    }
    public void TestResetSector(bool restart)
    {
      if (restart)
        GamePlayManager.Instance.GoSector(GamePlayManager.Instance.CurrentSector);
      else
        GamePlayManager.Instance.SectorManager.ResetCurrentSector(true);
      GameUIManager.Instance.GlobalToast("I18N:core.editor.messages.ResetedSector");
    }
    public void TestResetLevel()
    {
      GamePlayManager.Instance.RestartLevel();
    }
    public void TestGoSector()
    {
      LevelEditorUIControl.Confirm(
        "", 
        I18N.TrF("core.editor.messages.EnterSector", "", GamePlayManager.Instance.SectorManager.CurrentLevelSectorCount), 
        LevelEditorConfirmIcon.Warning, onConfirm: (value) => 
        {
          if (int.TryParse(value, out var sector) && sector >= 1 && sector < GamePlayManager.Instance.SectorManager.CurrentLevelSectorCount)
          {
            GamePlayManager.Instance.GoSector(sector);
            GameUIManager.Instance.GlobalToast("I18N:core.editor.messages.JumpedSector");
          }
          else
          {
            GameUIManager.Instance.GlobalToast("I18N:core.editor.messages.EnterSectorInvalid");
          }
        },
        showInput: true
      );
    }
    public void TestScreenshort()
    {
      if (isTakingScreenshort)
        return;
      isTakingScreenshort = true;
      StartCoroutine(_Screenshort());
    }
    public void TestFallShowAlert()
    {
      LevelEditorUIControl.Confirm(
        "", 
        "I18N:core.editor.messages.TestFail", 
        LevelEditorConfirmIcon.Warning, 
        onConfirm: (_) => 
        {
          TestResetSector(true);
        },
        onCancel: () => {
          TestResetLevel();
        },
        confirmText: "I18N:core.editor.RestartSector",
        cancelText: "I18N:core.editor.RestartLevel"
      );
    }
    public void TestSwitchPauseAlert(bool show)
    {
      LevelEditorUIControl.SetPauseTipShow(show);
    }
    public void TestPass()
    {
      LevelEditorUIControl.Confirm(
        "", 
        "I18N:core.editor.messages.TestPass", 
        LevelEditorConfirmIcon.Warning, 
        onConfirm: (_) => 
        {
          ExitTestMode();
        },
        onCancel: () => {
          TestResetLevel();
        },
        confirmText: "I18N:core.editor.Edit",
        cancelText: "I18N:core.editor.RestartLevel"
      );
    }

    private bool isTakingScreenshort = false;
    private IEnumerator _Screenshort()
    {
      string saveDir = LevelCurrent.LevelDirPath + "/screenshot/";
      string savePath = saveDir + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
      if (!Directory.Exists(saveDir))
        Directory.CreateDirectory(saveDir);
      
      LevelEditorUIControl.BottomBarTest.gameObject.SetActive(false);
      GamePlayUIControl.Instance.gameObject.SetActive(false);

      yield return new WaitForEndOfFrame();

      ScreenCapture.CaptureScreenshot(savePath);
      GamePlayUIControl.Instance.gameObject.SetActive(true);

      LevelEditorUIControl.BottomBarTest.gameObject.SetActive(true);

      Log.D(TAG, "CaptureScreenshot to " + savePath);
      GameUIManager.Instance.GlobalToast(I18N.TrF("global.CaptureScreenshotSuccess", "", ""));
      isTakingScreenshort = false;
    }
    private IEnumerator _IntoTest()
    {
      yield return new WaitForSeconds(1);

      //当选择了检查点时，则从检查点选择的节开始
      var startSector = 1;
      if (LevelEditorUIControl.SelectedObject.Count > 0)
      {
        var firstSelObj = LevelEditorUIControl.SelectedObject[0];
        if (firstSelObj.AssetRef.ObjName == "PC_TwoFlames" && firstSelObj.ActiveSectors.Count > 0)
          startSector = firstSelObj.ActiveSectors[0];
      }
      GamePlayManager.Instance.StartSector = startSector;

      //所有元素切换状态
      foreach (var item in LevelCurrent.LevelData.LevelModels)
      {
        if (item.ConfigueRef != null)
          item.ConfigueRef.OnEditorIntoTest(item);
      } 

      LevelEditorCamera.gameObject.SetActive(false);
      GamePlayManager.Instance.CamManager.SetCameraEnable(true);
      yield return StartCoroutine(LevelBuilder.LevelBuilder.Instance.LoadDynamicLevelInternal(LevelCurrent, true));

      LevelEditorUIControl.SetToolBarMode(LevelEditorUIControl.ToolBarMode.Test);
      LevelEditorUIControl.HideLoading();
    }
    private IEnumerator _QuitTest()
    {
      yield return new WaitForSeconds(1);

      GamePlayManager.Instance._Stop(BallControlStatus.NoControl);

      yield return StartCoroutine(LevelBuilder.LevelBuilder.Instance.UnLoadDynamicLevelInternal(LevelCurrent, true, () => {}));

      //恢复所有机关显示
      foreach (var item in LevelCurrent.LevelData.LevelModels)
      {
        if (item.ModulRef != null)
        {
          item.ModulRef.Reset(ModulBaseResetType.LevelRestart);
          item.ModulRef.ActiveForPreview();
        }
        if (item.ConfigueRef != null)
          item.ConfigueRef.OnEditorQuitTest(item);
      } 

      GamePlayManager.Instance.CamManager.SetCameraEnable(false);
      LevelEditorCamera.gameObject.SetActive(true);

      LevelEditorUIControl.SetToolBarMode(LevelEditorUIControl.ToolBarMode.Edit);
      LevelEditorUIControl.HideLoading();
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
      yield return StartCoroutine(CreateSky());
      
      LevelEditorUIControl.Init();
      LevelEditorUIControl.HideLoading();
      LevelEditorUIControl.UpdateStatusText();

      Log.D(TAG, "Load level done");
      yield break;
    }
    private IEnumerator _Unload(Action finish) 
    {
      yield return new WaitForSeconds(1f);
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
        EulerAngles = asset.IntitalEulerAngles,
        Scale = asset.IntitalScale,
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
                    Tag = item.Tag,
                    SubModelRefs = item.SubModelRefs,
                    OnlyOne = item.OnlyOne,
                    HiddenInContentSelector = item.HiddenInContentSelector,
                    HiddenPlaceholderRender = item.HiddenPlaceholderRender,
                    Loaded = true,
                    Prefab = item.Prefab,
                    Name = item.Name,
                    Desc = item.Desc,
                    PreviewImage = item.Preview,
                    ObjName = item.ObjName,
                    ObjTarget = item.ObjTarget,
                    ScenseGizmePreviewImage = item.ScenseGizmePreviewImage,
                    IntitalScale = item.IntitalScale,
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
      var comp = asset.Prefab.GetComponent<LevelDynamicModelAssetConfigue>();
      if (comp != null)
      {
        var res = comp.OnBeforeEditorAdd();
        if (!string.IsNullOrEmpty(res))
        {
          LevelEditorUIControl.ShowMouseTip(res);
          return true;
        }
      }
      return false;
    }
    private void OnAssetDropDone(LevelDynamicModelAsset asset, GameObject go)
    {
      StartCoroutine(_DropAssetSolve(asset, go));
    }
  }
}

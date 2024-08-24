using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ballance2.Base;
using Ballance2.Game.GamePlay;
using Ballance2.Game.Utils;
using Ballance2.Menu;
using Ballance2.Menu.LevelManager;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ballance2.Game.LevelEditor
{
  /// <summary>
  /// 关卡编辑器
  /// </summary>
  public class LevelEditorManager : GameSingletonBehavior<LevelEditorManager>
  {
    public const string TAG = "LevelEditorManager";

    public LevelEditorUIControl LevelEditorUIControl;
    public LevelDynamicControlSnap LevelDynamicControlSnap;
    public Camera LevelEditorCamera;
    public Skybox LevelEditorCameraSkyBox;
    public GameObject TransformHandles;
    public GameObject ScenseRoot;
    public GameObject ErrorPrefab;
    public GameObject LevelEditorObjectScenseIconPrefab;
    public Material TransparentMaterial;
    public TextAsset LevelNewJson;
    public AudioSource ScreenShortAudio;

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
    private bool showNameTag = true;
    public bool ShowNameTag
    {
      get => showNameTag;
      set
      {
        if (showNameTag != value)
        {
          showNameTag = value;
          foreach (var item in LevelCurrent.LevelData.LevelModels)
            item.SetNameTagVisible(showNameTag);
        }
      }
    }

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
    public void Init(LevelRegistedItem level)
    {
      if (level != null && level.Type != LevelRegistedType.Mine)
      {
        LevelEditorUIControl.Alert("", "I18N:core.editor.messages.CantEditPackedLevel", LevelEditorConfirmIcon.Error, onConfirm: () =>
        {
          Quit();
        });
        return;
      }

      var levelName = level == null ? "" : Path.GetFileNameWithoutExtension(((LevelRegistedLocallItem)level).path);
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
      StartCoroutine(_Load(LevelCurrent, true));
    }
    /// <summary>
    /// 退出编辑器
    /// </summary>
    public void Quit() {
      GameUIManager.Instance.MaskBlackSet(true);
      UnloadLevel(QuitToMenu);
    }
    /// <summary>
    /// 保存关卡
    /// </summary>
    public void Save() {
      StartCoroutine(_Save(LevelCurrent));
    }

    private void QuitToMenu()
    {
      LevelEditorCamera.gameObject.SetActive(false);
      GameManager.Instance.SetGameBaseCameraVisible(true);
      //通知回到menulevel
      GameManager.Instance.RequestEnterLogicScense("MenuLevel");
    }
    private void ReportCreateAssetFailed(string err)
    {
      LevelEditorUIControl.Alert("", err, LevelEditorConfirmIcon.Error);
    }
    public void ReportLoadError(string message)
    {
      Log.E(TAG, message);
      LevelEditorUIControl.HideLoading();
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
      }, onCancel: () =>
      {
        QuitToMenu();
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
        throw new Exception("LevelCurrent == null !");
      for (var i = 1; i < 100; i++)
      {
        var name = $"{baseName}_{i.ToString("D2")}";
        if (LevelCurrent.LevelData.LevelModels.Find((a) => a.Name == name) == null)
          return name;
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
        Texture B, F, L, R, T, D;
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
      if (!show)
        EventSystem.current.SetSelectedGameObject(null); //禁止选中，以防止UI与控制按键冲突
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
    public void CloneModels(LevelDynamicModel[] model)
    {
      StartCoroutine(_CloneModels(model));
    }
    public void SaveCustomModelAsset(GameObject root, string name, string objTarget, bool reMapMat, Vector3 intitalScale)
    {
      LevelEditorUIControl.ShowLoading("I18N:core.editor.import.SavingAssets");
      StartCoroutine(_SaveCustomModelAsset(root, name, objTarget, reMapMat, intitalScale));
    }
    public void DeleteCustomModelAsset(LevelDynamicModelAsset asset)
    {
      if (LevelCurrent.LevelData.LevelAssets.Contains(asset))
      {
        var result = new LevelDynamicLoader.LevelDynamicLoaderResult();
        LevelDynamicLoader.Instance.DeleteAsset(result, asset);
        if (!result.Success)
        {
          LevelEditorUIControl.Alert("", result.Error, LevelEditorConfirmIcon.Error);
          return;
        }

        //移除所有引用了当前资产的模型实例
        foreach (var model in LevelCurrent.LevelData.LevelModels)
        {
          if (model.AssetRef == asset)
          {
            model.AssetRef = null;
            model.ReInstantiateModul(ScenseRoot.transform, true);
          }
        }

        //从列表中移除
        LevelCurrent.LevelData.LevelAssets.Remove(asset);
        RemoveAssetFromList(asset, true);
      }
    }


    private bool isTakingScreenshort = false;
    private IEnumerator _Screenshort()
    {
      ScreenShortAudio.Play();

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

      GameMediator.Instance.DispatchGlobalEvent(GameEventNames.EVENT_LEVEL_BUILDER_BEFORE_START);

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
        item.SetModulPlaceholdeMode(false);
        if (item.ConfigueRef != null)
          item.ConfigueRef.OnEditorIntoTest(item);
      }

      LevelEditorCamera.gameObject.SetActive(false);
      GamePlayManager.Instance.CamManager.SetCameraEnable(true);
      yield return StartCoroutine(LevelBuilder.LevelBuilder.Instance.LoadDynamicLevelInternal(LevelCurrent, true));

      LevelEditorUIControl.SetToolBarMode(LevelEditorUIControl.ToolBarMode.Test);
      LevelEditorUIControl.HideLoading();

      EventSystem.current.SetSelectedGameObject(null);
    }
    private IEnumerator _QuitTest()
    {
      yield return new WaitForSeconds(1);

      GamePlayManager.Instance._Stop(BallControlStatus.NoControl);

      yield return StartCoroutine(LevelBuilder.LevelBuilder.Instance.UnLoadDynamicLevelInternal(LevelCurrent, true, () => { }));

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

      GamePlayUIControl.Instance.gameObject.SetActive(false);
      GamePlayManager.Instance.CamManager.SetCameraEnable(false);
      LevelEditorCamera.gameObject.SetActive(true);

      LevelEditorUIControl.SetToolBarMode(LevelEditorUIControl.ToolBarMode.Edit);
      LevelEditorUIControl.HideLoading();
    }
    private IEnumerator _Save(LevelDynamicAssembe level) 
    {
      LevelEditorUIControl.ShowLoading("I18N:core.editor.messages.Saving");

      //收集使用了的第三方模组，写入关卡信息中
      var packs = new HashSet<string>();
      foreach (var model in level.LevelData.LevelModels)
      {
        var pack = model.Asset.Split(':')[0];
        if (pack != "core" && pack != "levelasset" && !packs.Contains(pack))
          packs.Add(pack);
      }
      var pm = GameManager.GetSystemService<GamePackageManager>();
      foreach (var pack in packs)
      {
        var p = pm.FindPackage(pack);
        if (p != null)
        {
          var define = level.LevelInfo.requiredPackages.Find(d => d.name == pack);
          if (define == null)
          {
            define = new LevelBuilder.GameLevelDependencies();
            level.LevelInfo.requiredPackages.Add(define);
          }
          define.name = pack;
          define.minVersion = p.PackageVersion;
        }
      }

      //保存
      var task = level.Save();
      yield return task.AsIEnumerator();

      LevelEditorUIControl.HideLoading();

      if (!task.Result.Success)
      {
        LevelEditorUIControl.Alert("", I18N.Tr("core.editor.messages.SaveFailed") + " " + task.Result.Error, LevelEditorConfirmIcon.Error);
        yield break;
      }
    }
    private IEnumerator _Load(LevelDynamicAssembe level, bool newLevel = false) 
    {
      LevelCurrent = level;
      LevelEditorUIControl.ShowLoading();

      if (newLevel)
      {
        var task = LevelCurrent.New();
        yield return task.AsIEnumerator();
      }

      yield return new WaitForSeconds(1);

      var result = new LevelDynamicLoader.LevelDynamicLoaderResult();
      yield return StartCoroutine(LevelDynamicLoader.Instance.LoadLevel(
        result, 
        level,
        ScenseRoot.transform, 
        LevelAssets,
        (asset) => AddAssetToList(asset),
        true
      ));
      if (!result.Success)
      {
        ReportLoadError(result.Error);
        yield break;
      }

      //加载天空盒子和灯光颜色
      yield return StartCoroutine(CreateSky());
      
      LevelEditorUIControl.Init();
      LevelEditorUIControl.HideLoading();
      LevelEditorUIControl.UpdateStatusText();

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

      LevelDynamicLoader.Instance.DestroyAllTempDynamicAsset();

      finish.Invoke();
      yield break;
    }
    private IEnumerator _CloneModels(LevelDynamicModel[] models)
    {
      LevelEditorUIControl.ClearSelectedObjects();

      yield return new WaitForSeconds(0.1f);

      var result = new List<LevelDynamicModel>();
      foreach (var model in models)
      {
        var newModel = new LevelDynamicModel()
        {
          Name = GetUseableName(model.AssetRef.ObjName),
          Asset = model.Asset,
          AssetRef = model.AssetRef,
          Position = model.Position,
          EulerAngles = model.EulerAngles,
          Scale = model.Scale,
          Uid = ++LevelCurrent.LevelData.LevelObjectId,
          CanDelete = model.CanDelete,
        };
        yield return StartCoroutine(_CreateModelSolve(newModel));
        newModel.ConfigueRef.OnClone(newModel, model);
        result.Add(newModel);
      }

      LevelEditorUIControl.SetSelectedObjects(result.ToArray());

      foreach (var model in result)
      {
        model.ConfigueRef.OnCloneed(model);
      }
      yield return new WaitForSeconds(1f);
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

      yield return StartCoroutine(_CreateModelSolve(model));

      LevelEditorUIControl.SetSelectedObjects(new LevelDynamicModel[] { model });

      //删除编辑器创建的无用对象
      if (go != null)
        Destroy(go);
    }
    private IEnumerator _CreateModelSolve(LevelDynamicModel model)
    {
      if (!model.AssetRef.Loaded && model.AssetRef.SourceType == LevelDynamicModelSource.Embed)
      {
        var result = new LevelDynamicLoader.LevelDynamicLoaderResult();
        yield return StartCoroutine(LevelDynamicLoader.Instance.LoadAsset(result, LevelCurrent, model.AssetRef));
        if (!result.Success)
          ReportCreateAssetFailed(I18N.TrF("core.editor.messages.LoadModelAssetFailed", "", result.Error));
      }

      LevelCurrent.LevelData.LevelModels.Add(model);

      try
      {
        model.InstantiateModul(ScenseRoot.transform, true, true);
      } 
      catch (Exception e)
      {
        ReportCreateAssetFailed(I18N.TrF("core.editor.messages.InstanceModelFailed", "", e.ToString()));
        LevelCurrent.LevelData.LevelModels.Remove(model);

        yield break;
      }

      //创建子对象
      foreach (var _subAssetRef in model.AssetRef.SubModelRefs)
      {
        if (LevelAssets.TryGetValue(_subAssetRef.Path, out var subAsset))
        {
          if (!subAsset.Loaded && subAsset.SourceType == LevelDynamicModelSource.Embed)
          {
            var result = new LevelDynamicLoader.LevelDynamicLoaderResult();
            yield return StartCoroutine(LevelDynamicLoader.Instance.LoadAsset(result, LevelCurrent, subAsset));
            if (!result.Success)
              ReportCreateAssetFailed(I18N.TrF("core.editor.messages.LoadModelAssetFailed", "", result.Error));
          }

          var subModel = new LevelDynamicModel()
          {
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

          try
          {
            subModel.InstantiateModul(model.InstanceHost.transform, true, true);
          }
          catch (Exception e)
          {
            ReportCreateAssetFailed(I18N.TrF("core.editor.messages.InstanceModelFailed", "", e.ToString()));
            yield break;
          }
          LevelCurrent.LevelData.LevelModels.Add(subModel);
          if (!_subAssetRef.Enable)
            subModel.InstanceHost.SetActive(false);
        }
        else
        {
          ReportCreateAssetFailed(I18N.TrF(
            "core.editor.messages.InstanceModelFailed", "", 
            $"Failed to load SubAsset {_subAssetRef.Path} of model {model.AssetRef.Name}!"
          ));
          yield break;
        }
      }

      LevelEditorUIControl.UpdateStatusText();
    }
    private IEnumerator _SaveCustomModelAsset(GameObject root, string name, string objTarget, bool reMapMat, Vector3 intitalScale)
    {
      //保存用户导入的模型作为资产
      var newAsset = new LevelDynamicModelAsset();

      newAsset.Name = name;
      newAsset.Desc = I18N.Tr("CORE.EDITOR.IMPORT.ImportSource");
      newAsset.ObjName = root.name;
      newAsset.ObjTarget = objTarget;
      newAsset.Tag = "I18N:CORE.EDITOR.IMPORT.CustomAsset";
      newAsset.SubCategory = "MyAsset";

      switch (objTarget)
      {
        case "Phys_Floors":
        case "Phys_FloorWoods":
          newAsset.Category = LevelDynamicModelCategory.Floors;
          break;
        case "Phys_FloorRails":
          newAsset.Category = LevelDynamicModelCategory.Rails;
          break;
        case "":
          newAsset.Category = LevelDynamicModelCategory.Decoration;
          break;
        default:
          try
          {
            if (CustomModelAssetSaveTargetHandler.TryGetValue(objTarget, out var cb))
              cb(newAsset, objTarget, root);
          }
          catch (Exception e)
          {
            ReportCreateAssetFailed(I18N.TrF("core.editor.import.SavingAssetsFailed", "", e.ToString()));
            yield break;
          }
          break;
      }

      newAsset.Prefab = root;
      newAsset.Loaded = true;
      newAsset.SourcePath = null;

      var result = new LevelDynamicLoader.LevelDynamicLoaderResult();

      yield return StartCoroutine(LevelDynamicLoader.Instance.SaveAsset(result, newAsset, root, reMapMat, intitalScale.x));

      LevelEditorUIControl.HideLoading();

      if (!result.Success)
      {
        LevelEditorUIControl.Alert("", I18N.TrF("core.editor.import.ImportFailed", "", result.Error));
        yield break;
      }

      AddAssetToList(newAsset, true);

      //对于缺失资源的模型实例，重新匹配
      foreach (var model in LevelCurrent.LevelData.LevelModels)
      {
        if (model.Asset == newAsset.SourcePath)
        {
          model.AssetRef = newAsset;
          model.ReInstantiateModul(ScenseRoot.transform, true);
        }
      }

      LevelEditorUIControl.Alert("", "I18N:core.editor.import.ImportSuccess");
    }

    /// <summary>
    /// 在导入自定义模型时，允许对自定义 objTarget 进行预处理。同时在 LevelEditorImportControl.Choises 中增加自定义类型的选项。
    /// </summary>
    public Dictionary<string, Action<LevelDynamicModelAsset, string, GameObject>> CustomModelAssetSaveTargetHandler = new Dictionary<string, Action<LevelDynamicModelAsset, string, GameObject>>();

    private void RemoveAssetFromList(LevelDynamicModelAsset asset, bool refreshList = false)
    {
      if (LevelAssetsGrouped.TryGetValue(asset.Category, out var catgory))
      {
        var SubCategoryName = string.IsNullOrEmpty(asset.SubCategory) ? "Default" : asset.SubCategory;
        if (catgory.TryGetValue(SubCategoryName, out var subCatgory))
          subCatgory.Remove(asset);
      }
      LevelAssets.Remove(asset.SourcePath);
      if (refreshList) RefreshAllAssetsList();
    }
    private void AddAssetToList(LevelDynamicModelAsset asset, bool refreshList = false)
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
      var oldItem = subCatgory.Find(p => p.SourcePath == asset.SourcePath);
      if (oldItem != null)
        subCatgory.Remove(oldItem);
      subCatgory.Add(asset);

      if (!asset.HiddenInContentSelector)
      {
        if (LevelAssets.ContainsKey(asset.SourcePath))
          LevelAssets[asset.SourcePath] = asset;
        else
          LevelAssets.Add(asset.SourcePath, asset);
      }
      if (refreshList) RefreshAllAssetsList();
    }
    private void LoadAllAssets()
    {
      LevelDynamicLoader.Instance.LoadAllInternalAsset((item) =>
      {
        if (!item.HiddenInContentSelector)
          AddAssetToList(item);
      });
    }
    private void RefreshAllAssetsList()
    {
      LevelEditorUIControl.LevelEditorContentSelection.Refresh();
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

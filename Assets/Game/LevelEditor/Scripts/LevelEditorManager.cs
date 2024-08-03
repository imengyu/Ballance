using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ballance2.Base;
using Ballance2.Package;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorManager : GameSingletonBehavior<LevelEditorManager>
  {
    public LevelEditorUIControl LevelEditorUIControl;
    public Camera LevelEditorCamera;

    public LevelDynamicAssembe LevelCurrent = null;

    public void Init(string levelName)
    {
      LoadLevel(levelName);
      GameManager.Instance?.SetGameBaseCameraVisible(false);
      LevelEditorCamera.gameObject.SetActive(true);
      GameUIManager.Instance?.MaskBlackFadeOut(1);
    }
    public void LoadLevel(string levelName)
    {
      StartCoroutine(_Load(levelName));
    }
    public void UnloadLevel() {
      StartCoroutine(_Unload());
    }
    public void NewLevel(string levelName)
    {
      var levelPath = GamePathManager.GetLevelRealPath(levelName, false);
      LevelCurrent = new LevelDynamicAssembe();
      LevelCurrent.New(levelPath);
      LoadLevel(levelName);
    }

    private void ShowNewLevelDialog() {
      LevelEditorUIControl.Confirm(
        "I18N:core.editor.load.New", 
        "I18N:core.editor.load.NewDesc", 
        LevelEditorConfirmIcon.Info, 
        showInput: true, 
        onConfirm: (text) => 
      {
        var finalName = StringUtils.RemoveSpeicalChars(text);
        var levelPath = GamePathManager.GetLevelRealPath(finalName, false);
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

    private IEnumerator _Load(string levelName) 
    {
      //如果为空，则让用户新建关卡
      if (string.IsNullOrEmpty(levelName))
      {
        ShowNewLevelDialog();
        yield break;
      }

      var levelPath = GamePathManager.GetLevelRealPath(levelName, false);



      yield break;
    }
    private IEnumerator _Unload() 
    {
      yield return SceneManager.UnloadSceneAsync(1);
      //通知回到menulevel
      GameManager.Instance.RequestEnterLogicScense("MenuLevel");
    }


  }
}

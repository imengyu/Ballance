using Ballance2.Config;
using Ballance2.Services.Debug;
using Ballance2.Services.Init;
using Ballance2.Services.InputManager;
using Ballance2.Tests;
using Ballance2.UI.CoreUI;
using SubjectNerd.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/*
* Copyright(c) 2022 mengyu
*
* 模块名：     
* GameEntryCopySystemFiles.cs
* 
* 用途：
* 机关调试场景的配置。
*
* 作者：
* mengyu
*/

namespace Ballance2.Entry
{
  public class GameEntryCopySystemFiles : MonoBehaviour
  {
    private string[] coreFiles = new string[] {
      "core.ballance"
    };
    private string[] packagesFiles = new string[] {
      "core.assets.skys.ballance",
      "core.sounds.ballance",
      "core.sounds.music.ballance",
    };

    private void Start() 
    {
      //复制内置文件至临时目录
#if UNITY_ANDROID
      StartCoroutine(AndroidWorker());   
#elif UNITY_IOS

#endif
    }

    private IEnumerator AndroidWorker() {
      string dir = Application.streamingAssetsPath + "/BuiltInPackages";
      string targetCore = Application.persistentDataPath + "/Core";
      if(!Directory.Exists(targetCore))
        Directory.CreateDirectory(targetCore);

      foreach(string name in coreFiles) {
        string targetPath = targetCore + "/" + name;
        if(!File.Exists(targetPath)) {
          UnityWebRequest request = UnityWebRequest.Get(dir + "/Core/" + name);
          yield return request.SendWebRequest();

          if (request.result == UnityWebRequest.Result.Success)
          {
            try{
              File.WriteAllBytes(targetPath, request.downloadHandler.data);
              Debug.Log("Save core res \"" + targetPath + "\"");
            } catch(Exception e) {
              Debug.LogWarning("Save core res \"" + name + "\" failed! " + e.ToString());
            }
          }
          else
            Debug.LogWarning("Load core res \"" + name + "\" failed! " + request.error);
        }
      }

      string targetPackages = Application.persistentDataPath + "/Packages";
      if(!Directory.Exists(targetPackages))
        Directory.CreateDirectory(targetPackages);

      foreach(string name in packagesFiles) {
        string targetPath = targetPackages + "/" + name;
        if(!File.Exists(targetPath)) {
          UnityWebRequest request = UnityWebRequest.Get(dir + "/Packages/" + name);
          yield return request.SendWebRequest();

          if (request.result == UnityWebRequest.Result.Success)
          {
            try{
              File.WriteAllBytes(targetPath, request.downloadHandler.data);
              Debug.Log("Save packages res \"" + targetPath + "\"");
            } catch(Exception e) {
              Debug.LogWarning("Save packages res \"" + name + "\" failed! " + e.ToString());
            }
          }
          else
            Debug.LogWarning("Load packages res \"" + name + "\" failed! " + request.error);
        }
      }

      yield break;
    }
  }
}

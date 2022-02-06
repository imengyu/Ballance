using Ballance2.Config.Settings;
using Ballance2.Services;
using Ballance2.Utils;
using System.Collections;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameSystemDebugTests.cs
* 
* 用途：
* 主系统调试测试类。
*
* 作者：
* mengyu
*
*/

namespace Ballance2.Tests
{
  class GameSystemDebugTests : MonoBehaviour, GameSystem.SysDebugProvider
  {
    private readonly string TAG = "GameSystemDebugTests";

    public static GameSystem.SysDebugProvider RequestDebug()
    {
      if (DebugSettings.Instance.EnableSystemDebugTests)
      {
        GameObject go = CloneUtils.CreateEmptyObjectWithParent(GameManager.Instance.transform);
        go.name = "GameSystemDebugTests";
        return go.AddComponent<GameSystemDebugTests>();
      }
      return null;
    }

    public bool StartDebug()
    {
      Log.I(TAG, "StartDebug! ");
      shouldStartDebugLine = true;
      return true;
    }

    private IEnumerator DebugLine()
    {
      //yield return new WaitForSeconds(2.0f);
      //gameUIManager.GlobalToast("GameManager.Instance.GetSystemService<GameUIManager");
      //yield return new WaitForSeconds(3.0f);
      //gameUIManager.GlobalAlertWindow("测试 GlobalAlertWindow： gameUIManager.GlobalAlertWindow(\"测试\"" +
      //    "); 长文字", "提示文字");
      yield break;
    }

    private bool shouldStartDebugLine = false;

    private void Start()
    {

    }
    private void Update()
    {
      if (shouldStartDebugLine)
      {
        StartCoroutine(DebugLine());
        shouldStartDebugLine = false;
      }
    }
  }
}

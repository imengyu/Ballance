using Ballance2.Entry;
using Ballance2.Res;
using Ballance2.Utils;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameSystemInit.cs
* 
* 用途：
* 框架上层初始化
*
* 作者：
* mengyu
*
*/

namespace Ballance2.Services.Init
{

  /// <summary>
  /// 上层初始化器
  /// </summary>
  class GameSystemInit : MonoBehaviour
  {
    internal static GameSystem.SysHandler GetSysHandler()
    {
      return SysHandler;
    }
    internal static void FillStartParameters(GameEntry gameEntry)
    {
      gameEntryInstance = gameEntry;
    }

    private static GameEntry gameEntryInstance = null;
    private static GameManager gameManagerInstance = null;
    private static readonly string TAG = "GameSystemInit";

    /// <summary>
    /// 处理System的消息
    /// </summary>
    /// <param name="act"></param>
    private static void SysHandler(int act)
    {
      if (act == GameSystem.ACTION_INIT)
      {
        GameObject newGameManager = CloneUtils.CreateEmptyObjectWithParent(gameEntryInstance.transform);
        newGameManager.name = "GameSystemInit";
        newGameManager.AddComponent<GameSystemInit>();
      }
      else if (act == GameSystem.ACTION_DESTROY)
      {
        if (gameManagerInstance != null)
        {
          Destroy(gameManagerInstance);
          gameManagerInstance = null;
        }

        GameStaticResourcesPool.ReleaseAll();
      }
      else if (act == GameSystem.ACTION_FORCE_INT)
      {
        Log.D(TAG, "Force interrupt game");

        if (gameManagerInstance != null)
        {
          gameManagerInstance.ClearScense();
        }
      }
    }
  }
}

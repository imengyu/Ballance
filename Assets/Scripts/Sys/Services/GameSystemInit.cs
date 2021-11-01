using Ballance2.Sys.Entry;
using Ballance2.Sys.Res;
using Ballance2.Sys.Utils;
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

namespace Ballance2.Sys.Services {

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
        private static GameSystemInit gameInitInstance = null;
        private static readonly string TAG = "GameSystemInit";

        /// <summary>
        /// 处理System的消息
        /// </summary>
        /// <param name="act"></param>
        private static void SysHandler(int act)
        {
            if(act == GameSystem.ACTION_INIT)
            {

                GameObject newGoThis = CloneUtils.CreateEmptyObjectWithParent(gameEntryInstance.transform);
                newGoThis.name = "GameInit";
                gameInitInstance = newGoThis.AddComponent<GameSystemInit>();

                GameObject newGameManager = CloneUtils.CreateEmptyObjectWithParent(gameEntryInstance.transform);
                newGameManager.name = "GameManager";
                gameManagerInstance = newGameManager.AddComponent<GameManager>();
                gameManagerInstance.Initialize(gameEntryInstance);
            }
            else if (act == GameSystem.ACTION_DESTROY)
            {
                if (gameInitInstance != null)
                {
                    Destroy(gameInitInstance);
                    gameInitInstance = null;
                }
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

                if (gameInitInstance != null)
                {
                    gameInitInstance.StopAllCoroutines();
                }
                if (gameManagerInstance != null)
                {
                    gameManagerInstance.ClearScense();
                }
            }
        }
    }
}

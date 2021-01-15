using Ballance2.System;
using Ballance2.System.Res;
using Ballance2.System.Utils;
using Ballance2.Utils;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameInit.cs
* 
* 用途：
* 上层初始化
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-1-15 创建
*
*/

namespace Ballance2.Game {

    /// <summary>
    /// 上层初始化器
    /// </summary>
    class GameInit : MonoBehaviour
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
        private static GameInit gameInitInstance = null;
        private static readonly string TAG = "GameInit";

        /// <summary>
        /// 处理System的消息
        /// </summary>
        /// <param name="act"></param>
        private static void SysHandler(int act)
        {
            if(act == GameSystem.ACTION_INIT)
            {
                //初始化静态资源
                GameStaticResourcesPool.InitStaticPrefab(gameEntryInstance.GamePrefab, 
                    gameEntryInstance.GameAssets);

                GameObject newGoThis = CloneUtils.CreateEmptyObjectWithParent(gameEntryInstance.transform);
                newGoThis.name = "GameInit";
                gameInitInstance = newGoThis.AddComponent<GameInit>();
                gameInitInstance.InstanceInitialize();

                GameObject newGameManager = CloneUtils.CreateEmptyObjectWithParent(gameEntryInstance.transform);
                newGameManager.name = "GameManager";
                gameManagerInstance = newGameManager.AddComponent<GameManager>();
                gameManagerInstance.Initialize(gameEntryInstance);
            }
            else if (act == GameSystem.ACTION_DESTROY)
            {
                if (gameManagerInstance != null)
                {
                    gameManagerInstance.Destroy();
                    Destroy(gameManagerInstance);
                    gameManagerInstance = null;
                }
                if (gameInitInstance != null)
                {
                    gameInitInstance.InstanceDestroy();
                    Destroy(gameInitInstance);
                    gameInitInstance = null;
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

        void Start()
        {

        }
        void Update()
        {

        }

        void InstanceInitialize()
        {

        }
        void InstanceDestroy()
        {

        }
    }
}

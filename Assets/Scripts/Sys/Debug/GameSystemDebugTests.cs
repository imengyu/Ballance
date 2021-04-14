using Ballance2.Config.Settings;
using Ballance2.Sys.Services;
using Ballance2.Sys.Utils;
using Ballance2.Utils;
using System.Collections;
using UnityEngine;

namespace Ballance2.Sys.Debug
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

        private GameUIManager gameUIManager = null;

        public bool StartDebug()
        {
            Log.I(TAG, "StartDebug! ");

            gameUIManager = GameManager.Instance.GetSystemService<GameUIManager>("GameUIManager");
            gameUIManager.MaskBlackFadeOut(0.5f);
            
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
            if(shouldStartDebugLine)
            {
                StartCoroutine(DebugLine());
                shouldStartDebugLine = false;
            }
        }
    }
}

using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameUIManagerObject.cs
* 
* 用途：
* GameUIManager 所用的Update实例。
*
* 作者：
* mengyu
*/

namespace Ballance2.Sys.UI.Parts
{
    class GameUIManagerObject : MonoBehaviour
    {
        public delegate void UpdateDelegate();

        public UpdateDelegate GameUIManagerUpdateDelegate;

        private void Update()
        {
            GameUIManagerUpdateDelegate?.Invoke();
        }
    }
}

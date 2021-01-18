using Ballance2.System.Res;
using Ballance2.System.Services;
using SubjectNerd.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Ballance2.System.Entry
{
    public class GameStaticResEntry : MonoBehaviour
    {
        /// <summary>
        /// 静态 Prefab 资源引入
        /// </summary>
        [Reorderable("GamePrefab", true, "Name")]
        public List<GameObjectInfo> GamePrefab = null;
        /// <summary>
        /// 静态资源引入
        /// </summary>
        [Reorderable("GameAssets", true, "Name")]
        public List<GameAssetsInfo> GameAssets = null;

        private void Start()
        {
            GameSystemInit.FillResEntry(this);
        }
    }
}

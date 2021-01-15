using System;
using UnityEngine;

namespace Ballance2.System.Res
{
    [Serializable]
    public class GameObjectInfo
    {
        public GameObject Object;
        public string Name;

        public override string ToString() { return Name; }
    }
}

using System;

namespace Ballance2.System.Res
{
    [Serializable]
    public class GameAssetsInfo
    {
        public UnityEngine.Object Object;
        public string Name;

        public override string ToString() { return Name; }
    }
}

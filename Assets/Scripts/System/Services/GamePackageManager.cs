using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballance2.System.Services
{
    /// <summary>
    /// UI 管理器
    /// </summary>
    [SLua.CustomLuaClass]
    public class GamePackageManager : GameService
    {
        public GamePackageManager() : base("GamePackageManager")
        {

        }

        public override void Destroy()
        {
            
        }
        public override bool Initialize()
        {
            return true;
        }


    }
}

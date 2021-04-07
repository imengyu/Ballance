
using System;
using System.Collections.Generic;
namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal void Lua_Ballance2_Utils_LogObserver(LuaFunction ld ,Ballance2.Utils.LogLevel a1,string a2,string a3,string a4) {
            IntPtr l = ld.L;
            int error = pushTry(l);

			pushValue(l,a1);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
			ld.pcall(4, error);
			LuaDLL.lua_settop(l, error-1);
		}
	}
}

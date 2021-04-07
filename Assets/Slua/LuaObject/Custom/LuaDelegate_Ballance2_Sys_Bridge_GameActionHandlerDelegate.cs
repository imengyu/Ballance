
using System;
using System.Collections.Generic;
namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal Ballance2.Sys.Bridge.GameActionCallResult Lua_Ballance2_Sys_Bridge_GameActionHandlerDelegate(LuaFunction ld ,object[] a1) {
            IntPtr l = ld.L;
            int error = pushTry(l);

			pushValue(l,a1);
			ld.pcall(1, error);
			Ballance2.Sys.Bridge.GameActionCallResult ret;
			checkType(l,error+1,out ret);
			LuaDLL.lua_settop(l, error-1);
			return ret;
		}
	}
}

using SLua;
using System.Text;

namespace Ballance2.Utils
{
    [CustomLuaClass]
    public class LuaUtils
    {
        public static object[] LuaTableArrayToObjectArray(object[] param)
        {
            if (param == null)
                return null;
            if (param.Length == 1 && param[0] is LuaTable)
            {
                LuaTable arr = param[0] as LuaTable;
                object[] arrFixed = new object[arr.length()];
                for (int i = 0, c = arrFixed.Length; i < c; i++)
                    arrFixed[i] = arr[i + 1];
                return arrFixed;
            }
            else
                return param;
        }
        public static bool CheckParamIsLuaTable(object[] param)
        {
            return param != null && param.Length == 1 && param[0] != null && param[0].GetType() == typeof(LuaTable);
        }
        public static string BooleanToString(bool param)
        {
            return param ? "true" : "false";
        }
    }
}

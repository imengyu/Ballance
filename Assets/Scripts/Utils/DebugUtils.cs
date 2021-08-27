using System.Diagnostics;
using System.Text;
using Ballance2.LuaHelpers;
using SLua;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* DebugUtils.cs
* 
* 用途：
* 调试工具类
*
* 作者：
* mengyu
*
* 
* 
*
*/

namespace Ballance2.Utils
{
    /// <summary>
    /// 调试工具类
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("游戏中介者")]
    public class DebugUtils
    {
        /// <summary>
        /// 获取当前调用堆栈
        /// </summary>
        /// <param name="skipFrame">要跳过的帧，为0不跳过</param>
        /// <returns></returns>
        [LuaApiDescription("获取当前调用堆栈", "")]
        [LuaApiParamDescription("skipFrame", "要跳过的帧，为0不跳过")]
        public static string GetStackTrace(int skipFrame)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var stacktrace = new StackTrace(skipFrame + 1, true);
            for (var i = 0; i < stacktrace.FrameCount; i++)
            {
                var frame = stacktrace.GetFrame(i);
                var method = stacktrace.GetFrame(i).GetMethod();
                stringBuilder.Append("\n[");
                stringBuilder.Append(i);
                stringBuilder.Append("] ");
                stringBuilder.Append(method.Name);

                var lineNum = frame.GetFileLineNumber();
                if(lineNum > 0) {
                    stringBuilder.Append(" in ");
                    stringBuilder.Append(frame.GetFileName());
                    stringBuilder.Append(" (");
                    stringBuilder.Append(lineNum);
                    stringBuilder.Append(":");
                    stringBuilder.Append(frame.GetFileColumnNumber());
                    stringBuilder.Append(")");
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 打印出格式化过的 byte 数组，以十六进制显示
        /// </summary>
        /// <param name="vs">byte 数组</param>
        /// <returns></returns>
        [LuaApiDescription("打印出格式化过的 byte 数组，以十六进制显示", "")]
        [LuaApiParamDescription("vs", "byte 数组")]
        public static string PrintBytes(byte[] vs)
        {
            StringBuilder sb = new StringBuilder();
            int col = 0, line = 0;
            sb.Append("Byte Array Length: ");
            sb.Append(vs.Length);
            sb.Append('\n');
            sb.Append(
                "00000000 | 00 01 02 03  04 05 06 07  08 09 0a 0b  0c 0d 0e 0f\n" +
                "-------- | --------------------------------------------------\n" +
                "00000000 | ");
            for (int i = 0; i < vs.Length; i++)
            {
                sb.Append(vs[i].ToString("X2"));
                sb.Append(' ');
                if (col == 3 || col == 7 || col == 11)
                    sb.Append(' ');

                if (col < 0xf)
                    col++;
                else
                {
                    col = 0;
                    line++;
                    sb.Append('\n');
                    sb.Append((line * 0xf).ToString("X8"));
                    sb.Append(" | ");
                }

                if (i > 0xE01F)
                {
                    sb.Append('\n');
                    sb.Append((vs.Length - i).ToString("X8"));
                    sb.Append(" bytes data left...");
                    break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 打印出带行号的代码
        /// </summary>
        /// <param name="code">代码字符串</param>
        /// <returns></returns>
        [LuaApiDescription("打印出带行号的代码", "")]
        [LuaApiParamDescription("code", "代码字符串")]
        public static string PrintCodeWithLine(string code)
        {
            string[] lines = code.Split('\n');
            StringBuilder sb = new StringBuilder();
            int lineNum = 0;
            int lineCount = lines.Length, lineCountMumLength = lineCount.ToString().Length + 1;
            string lineNumStr;

            sb.Append(lineCount);
            sb.Append(" Lines ");
            sb.Append(code.Length);
            sb.Append(" Chars\n");

            foreach (string line in lines)
            {
                lineNum++;
                lineNumStr = lineNum.ToString();

                sb.Append(lineNumStr);
                for (int i = lineCountMumLength; i > lineNumStr.Length; i--)
                    sb.Append(' ');

                sb.Append(line);
                sb.Append('\n');

                if (lineNum > 2048)
                {
                    sb.Append(lineCount - lineNum);
                    sb.Append(" lines more...\n");
                }
            }

            return sb.ToString();
        }
        public static string PrintCodeWithLine(byte[] code)
        {
            return PrintCodeWithLine(Encoding.UTF8.GetString(code));
        }
    
        /// <summary>
        /// 打印出数组的代码
        /// </summary>
        /// <param name="any">要转换的数组</param>
        /// <returns></returns>
        [LuaApiDescription("打印出数组的代码", "")]
        [LuaApiParamDescription("any", "要转换的数组")]
        public static string PrintArrVar(object[] any) {
            return StringUtils.ValueArrayToString(any);
        }

        /// <summary>
        /// 打印LUA变量
        /// </summary>
        /// <param name="any">LUA变量</param>
        /// <returns></returns>
        [LuaApiDescription("打印LUA变量", "")]
        [LuaApiParamDescription("any", "LUA变量")]
        [LuaApiParamDescription("max_level", "打印最大级")]
        public static string PrintLuaVarAuto(object any, int max_level) {

            StringBuilder sb = new StringBuilder();
            PrintLuaVarAutoLoop(sb, any, 0, max_level);
            return sb.ToString();
        }

        private static void PrintLuaVarAutoLoop(StringBuilder stringBuilder, object any, int level, int max_level, string prefix = "") {
            if(max_level > 0 && level > max_level)
                return;
            var type = any.GetType() ;
            if(type == typeof(LuaTable)) {
                var t = (LuaTable)any;

                stringBuilder.Append(prefix);
                stringBuilder.Append("table (length: ");
                stringBuilder.Append(t.length());
                stringBuilder.Append(" empty: ");
                stringBuilder.Append(t.IsEmpty);
                stringBuilder.Append(")");
                stringBuilder.AppendLine("{");
                
                foreach(var v in t) {

                    string key = "";
                    if(type == typeof(int)) key = ((int)any).ToString();
                    else if(type == typeof(char)) key = "'" + ((string)any) + "'";
                    else if(type == typeof(string)) key = "\"" + ((string)any) + "\"";
                    
                    StringBuilder sb = new StringBuilder();
                    PrintLuaVarAutoLoop(sb, any, level + 1, max_level, prefix + "  ");
                    stringBuilder.AppendFormat("{0}   {1} = {2}\n", prefix, key, sb.ToString());
                }
                stringBuilder.Append(prefix);
                stringBuilder.AppendLine("}");
            } else if(type == typeof(LuaFunction)) {
                var f = (LuaFunction)any;
                stringBuilder.AppendFormat("{0} function: 0x{1:X} ({1})\n", prefix, f.L, f.Ref);
            } 
            else if(type == typeof(int)) 
                stringBuilder.AppendFormat("{0} int: {1}\n", prefix, (int)any);
            else if(type == typeof(long)) 
                stringBuilder.AppendFormat("{0} long: {1}\n", prefix, (long)any);
            else if(type == typeof(double)) 
                stringBuilder.AppendFormat("{0} double: {1}\n", prefix, (double)any);
            else if(type == typeof(float)) 
                stringBuilder.AppendFormat("{0} float: {1}\n", prefix, (float)any);
            else if(type == typeof(bool))
                stringBuilder.AppendFormat("{0} bool: {1}\n", prefix, (bool)any);
            else if(type == typeof(char))
                stringBuilder.AppendFormat("{0} char: \'{1}\'\n", prefix, (char)any);
            else if(type == typeof(string)) 
                stringBuilder.AppendFormat("{0} string: \"{1}\"\n", prefix, (string)any);
            else if(type == typeof(LuaVar)) {
                var v = (LuaVar)any;
                stringBuilder.AppendFormat("{0} unknow var: 0x{1:X}\n", prefix, v.L);
            }
            else
                stringBuilder.AppendFormat("{0} unknow var: {1}\n", prefix, any);
        }

        public static bool CheckDebugParam(int index, string[] arr, out string value, bool required = true, string defaultValue = "") {
            if(arr.Length <= index) {
                if(required)
                    Log.E("Debug", "缺少参数 [{0}]", index);
                value = defaultValue;
                return false;
            }
            value = arr[index];
            return true; 
        }
        public static bool CheckIntDebugParam(int index, string[] arr, out int value, bool required = true, int defaultValue = 0) {
            if(arr.Length <= index) {
                if(required)
                    Log.E("Debug", "缺少参数 [{0}]", index);
                value = defaultValue;
                return false;
            }
            if(!int.TryParse(arr[index], out value)) {
                Log.E("Debug", "参数 [{0}] 不是有效的 int", index);
                return false;
            } 
            return true;
        }
        public static bool CheckFloatDebugParam(int index, string[] arr, out float value, bool required = true, float defaultValue = 0) {
            if(arr.Length <= index) {
                if(required)
                    Log.E("Debug", "缺少参数 [{0}]", index);
                value = defaultValue;
                return false;
            }
            if(!float.TryParse(arr[index], out value)) {
                Log.E("Debug", "参数 [{0}] 不是有效的 float", index);
                return false;
            } 
            return true;
        }
        public static bool CheckBoolDebugParam(int index, string[] arr, out bool value, bool required = true, bool defaultValue = false) {
            if(arr.Length <= index) {
                if(required)
                    Log.E("Debug", "缺少参数 [{0}]", index);
                value = defaultValue;
                return false;
            }
            if(!bool.TryParse(arr[index], out value)) {
                Log.E("Debug", "参数 [{0}] 不是有效的 bool", index);
                return false;
            } 
            return true;
        }
        public static bool CheckDoubleDebugParam(int index, string[] arr, out double value, bool required = true, double defaultValue = 0) {
            if(arr.Length <= index) {
                if(required)
                    Log.E("Debug", "缺少参数 [{0}]", index);
                value = defaultValue;
                return false;
            }
            if(!double.TryParse(arr[index], out value)) {
                Log.E("Debug", "参数 [{0}] 不是有效的 double", index);
                return false;
            } 
            return true;
        }
        public static bool CheckStringDebugParam(int index, string[] arr, bool required = true) {
            if(arr.Length <= index) {
                if(required)
                    Log.E("Debug", "缺少参数 [{0}]", index);
                return false;
            }
            if(string.IsNullOrEmpty(arr[index])) {
                Log.E("Debug", "参数 [{0}] 不是有效的字符串为空", index);
                return false;
            } 
            return true;
        }
        public static bool CheckEnumDebugParam<T>(int index, string[] arr, out T value, bool required = true, T defaultValue = default(T)) where T : struct {
            if(arr.Length <= index) {
                if(required)
                    Log.E("Debug", "缺少参数 [{0}]", index);
                value = defaultValue;
                return false;
            }
            if(!System.Enum.TryParse<T>(arr[index], out value)) {
                Log.E("Debug", "参数 [{0}] 不是有效的 {1}", index, typeof(T).Name);
                return false;
            } 
            return true;
        }
    }
}

using System.Diagnostics;
using System.Text;

namespace Ballance2.Utils
{
    /// <summary>
    /// 调试工具类
    /// </summary>
    [SLua.CustomLuaClass]
    public class DebugUtils
    {
        /// <summary>
        /// 获取当前调用堆栈
        /// </summary>
        /// <param name="skipFrame">要跳过的帧，为0不跳过</param>
        /// <returns></returns>
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
                stringBuilder.Append("\n");
                stringBuilder.Append(frame.GetFileName());
                stringBuilder.Append(" (");
                stringBuilder.Append(frame.GetFileLineNumber());
                stringBuilder.Append(":");
                stringBuilder.Append(frame.GetFileColumnNumber());
                stringBuilder.Append(")");
            }
            return stringBuilder.ToString();
        }
    }
}

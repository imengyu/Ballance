using System.Diagnostics;
using System.Text;

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
*/

namespace Ballance2.Utils
{
  /// <summary>
  /// 调试工具类
  /// </summary>
  public class DebugUtils
  {
    /// <summary>
    /// 获取当前调用堆栈
    /// </summary>
    /// <param name="skipFrame">要跳过的帧，为0不跳过</param>
    /// <returns>返回调用堆栈详细信息</returns>
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
        if (lineNum > 0)
        {
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
    /// 格式化的 byte 数组，以十六进制显示
    /// </summary>
    /// <param name="vs">byte 数组</param>
    /// <returns>返回字符串，请输出或者显示至控制台。</returns>
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
    /// 格式化出带行号的代码
    /// </summary>
    /// <param name="code">代码字符串</param>
    /// <returns>返回字符串，请输出或者显示至控制台。</returns>
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

    /// <summary>
    /// 格式化出带行号的代码
    /// </summary>
    /// <param name="code">代码字符串直接数组</param>
    /// <returns>返回字符串，请输出或者显示至控制台。</returns>
    public static string PrintCodeWithLine(byte[] code)
    {
      return PrintCodeWithLine(Encoding.UTF8.GetString(code));
    }

    /// <summary>
    /// 格式化数组为字符串
    /// </summary>
    /// <param name="any">要转换的数组</param>
    /// <returns>返回字符串，请输出或者显示至控制台。</returns>
    public static string PrintArrVar(object[] any)
    {
      return StringUtils.ValueArrayToString(any);
    }

    /// <summary>
    /// 从用户输入的参数数组中检查并获取指定位的字符串参数。
    /// </summary>
    /// <remarks>
    /// 此函数为自定义控制台调试命令获取参数而设计，如果你需要在控制台调试命令中获取参数，这一个非常有用的方法。
    /// 
    /// 如果你设置了 required 必填，而用户没有输入参数，方法会自动输出错误信息。
    /// </remarks>
    /// <example>
    /// //注册一个测试控制台指令，获取用户输入的参数：
    /// GameDebugCommandServer.RegisterCommand("mycommand", (keyword, fullCmd, argsCount, args) => {
    ///   //第一个返回值表示返回参数是否成功获取
    ///   //第二个返回值是参数的值
    ///   var ox = DebugUtils.CheckIntDebugParam(0, args, out var nx, true, 0)
    ///   if (!ox) return false;
    ///     Debug.Log("用户输入了：" + nx);
    ///   return true;
    /// }, 1, "mycommand <count:number> > 测试控制台指令")
    /// </example>
    /// <param name="index">设置当前需要获取的参数是参数数组的第几个, 索引从 0 开始</param>
    /// <param name="arr">用户输入的参数数组</param>
    /// <param name="value">获取到的参数</param>
    /// <param name="required">是否必填，必填则如果无输入参数，会返回false</param>
    /// <param name="defaultValue">默认值，若非必填且无输入参数，则会返回默认值</param>
    /// <returns>返回参数是否成功获取</returns>
    public static bool CheckDebugParam(int index, string[] arr, out string value, bool required = true, string defaultValue = "")
    {
      if (arr.Length <= index)
      {
        if (required)
          Log.E("Debug", "缺少参数 [{0}]", index);
        value = defaultValue;
        return false;
      }
      value = arr[index];
      return true;
    }

    /// <summary>
    /// 从用户输入的参数数组中检查并获取指定位的整形参数。
    /// </summary>
    /// <param name="index">设置当前需要获取的参数是参数数组的第几个</param>
    /// <param name="arr">用户输入的参数数组</param>
    /// <param name="value">获取到的参数</param>
    /// <param name="required">是否必填，必填则如果无输入参数，会返回false</param>
    /// <param name="defaultValue">默认值，若非必填且无输入参数，则会返回默认值</param>
    /// <returns>返回参数是否成功获取</returns>
    public static bool CheckIntDebugParam(int index, string[] arr, out int value, bool required = true, int defaultValue = 0)
    {
      if (arr.Length <= index)
      {
        if (required)
          Log.E("Debug", "缺少参数 [{0}]", index);
        value = defaultValue;
        return false;
      }
      if (!int.TryParse(arr[index], out value))
      {
        Log.E("Debug", "参数 [{0}] 不是有效的 int", index);
        return false;
      }
      return true;
    }

    /// <summary>
    /// 从用户输入的参数数组中检查并获取指定位的浮点型参数。
    /// </summary>
    /// <param name="index">设置当前需要获取的参数是参数数组的第几个</param>
    /// <param name="arr">用户输入的参数数组</param>
    /// <param name="value">获取到的参数</param>
    /// <param name="required">是否必填，必填则如果无输入参数，会返回false</param>
    /// <param name="defaultValue">默认值，若非必填且无输入参数，则会返回默认值</param>
    /// <returns>返回参数是否成功获取</returns>
    public static bool CheckFloatDebugParam(int index, string[] arr, out float value, bool required = true, float defaultValue = 0)
    {
      if (arr.Length <= index)
      {
        if (required)
          Log.E("Debug", "缺少参数 [{0}]", index);
        value = defaultValue;
        return false;
      }
      if (!float.TryParse(arr[index], out value))
      {
        Log.E("Debug", "参数 [{0}] 不是有效的 float", index);
        return false;
      }
      return true;
    }

    /// <summary>
    /// 从用户输入的参数数组中检查并获取指定位的布尔型参数。
    /// </summary>
    /// <param name="index">设置当前需要获取的参数是参数数组的第几个</param>
    /// <param name="arr">用户输入的参数数组</param>
    /// <param name="value">获取到的参数</param>
    /// <param name="required">是否必填，必填则如果无输入参数，会返回false</param>
    /// <param name="defaultValue">默认值，若非必填且无输入参数，则会返回默认值</param>
    /// <returns>返回参数是否成功获取</returns>
    public static bool CheckBoolDebugParam(int index, string[] arr, out bool value, bool required = true, bool defaultValue = false)
    {
      if (arr.Length <= index)
      {
        if (required)
          Log.E("Debug", "缺少参数 [{0}]", index);
        value = defaultValue;
        return false;
      }
      if (!bool.TryParse(arr[index], out value))
      {
        Log.E("Debug", "参数 [{0}] 不是有效的 bool", index);
        return false;
      }
      return true;
    }

    /// <summary>
    /// 从用户输入的参数数组中检查并获取指定位的双精浮点数类型参数。
    /// </summary>
    /// <param name="index">设置当前需要获取的参数是参数数组的第几个</param>
    /// <param name="arr">用户输入的参数数组</param>
    /// <param name="value">获取到的参数</param>
    /// <param name="required">是否必填，必填则如果无输入参数，会返回false</param>
    /// <param name="defaultValue">默认值，若非必填且无输入参数，则会返回默认值</param>
    /// <returns>返回参数是否成功获取</returns>
    public static bool CheckDoubleDebugParam(int index, string[] arr, out double value, bool required = true, double defaultValue = 0)
    {
      if (arr.Length <= index)
      {
        if (required)
          Log.E("Debug", "缺少参数 [{0}]", index);
        value = defaultValue;
        return false;
      }
      if (!double.TryParse(arr[index], out value))
      {
        Log.E("Debug", "参数 [{0}] 不是有效的 double", index);
        return false;
      }
      return true;
    }

    /// <summary>
    /// 从用户输入的参数数组中检查并获取指定位的字符串参数。
    /// </summary>
    /// <param name="index">设置当前需要获取的参数是参数数组的第几个</param>
    /// <param name="arr">用户输入的参数数组</param>
    /// <param name="value">获取到的参数</param>
    /// <param name="required">是否必填，必填则如果无输入参数，会返回false</param>
    /// <param name="defaultValue">默认值，若非必填且无输入参数，则会返回默认值</param>
    /// <returns>返回参数是否成功获取</returns>
    public static bool CheckStringDebugParam(int index, string[] arr, bool required = true)
    {
      if (arr.Length <= index)
      {
        if (required)
          Log.E("Debug", "缺少参数 [{0}]", index);
        return false;
      }
      if (string.IsNullOrEmpty(arr[index]))
      {
        Log.E("Debug", "参数 [{0}] 不是有效的字符串(为空)", index);
        return false;
      }
      return true;
    }

    /// <summary>
    /// 从用户输入的参数数组中检查并获取指定位的枚举型参数。
    /// </summary>
    /// <param name="index">设置当前需要获取的参数是参数数组的第几个</param>
    /// <param name="arr">用户输入的参数数组</param>
    /// <param name="value">获取到的参数</param>
    /// <param name="required">是否必填，必填则如果无输入参数，会返回false</param>
    /// <param name="defaultValue">默认值，若非必填且无输入参数，则会返回默认值</param>
    /// <returns>返回参数是否成功获取</returns>
    public static bool CheckEnumDebugParam<T>(int index, string[] arr, out T value, bool required = true, T defaultValue = default(T)) where T : struct
    {
      if (arr.Length <= index)
      {
        if (required)
          Log.E("Debug", "缺少参数 [{0}]", index);
        value = defaultValue;
        return false;
      }
      if (!System.Enum.TryParse<T>(arr[index], out value))
      {
        Log.E("Debug", "参数 [{0}] 不是有效的 {1}", index, typeof(T).Name);
        return false;
      }
      return true;
    }
  }
}

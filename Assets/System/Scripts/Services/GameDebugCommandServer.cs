using System.Collections.Generic;
using System.Text;
using Ballance2.DebugTools;
using Ballance2.Services.Debug;
using Ballance2.Utils;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameDebugCommandServer.cs
* 
* 用途：
* 调试命令服务，管理游戏自定义调试命令。
*
* 作者：
* mengyu
*/

/*

在游戏中按 <kbd>F12</kbd> 呼出调试控制台，在调试控制台中可以输入调试命令执行。

你可以使用 RegisterCommand 注册自定义调试命令，例如：
```csharp
GameDebugCommandServer.RegisterCommand("mycommand", (keyword, fullCmd, argsCount, args) => {
  //这里使用了 DebugUtils 中的调试工具函数获取参数
  var ox = DebugUtils.CheckIntDebugParam(0, args, out var nx, true, 0);
  if (ox == null) 
    return false;

  //获取参数成功后就可以执行自定义方法了
  return true;
}, 1, "mycommand <count:number> > 测试调试命令");
```
*/

namespace Ballance2.Services
{
  /// <summary>
  /// 调试命令服务，使用此服务添加自定义调试命令。
  /// </summary>
  public class GameDebugCommandServer
  {   
    private const string TAG = "DebugCommand";
    private List<CmdItem> commands = new List<CmdItem>();
    private class CmdItem
    {
      public int Id;
      public string Keyword;
      public int LimitArgCount;
      public string HelpText;
      public string Handler;
      public CommandDelegate Callback;
    }

    public GameDebugCommandServer()
    {
      RegisterSystemCommands();
    }

    /// <summary>
    /// 获取指定文字的命令筛选帮助
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string GenCommandHelp(string text) {
      StringBuilder sb = new StringBuilder();

      if (string.IsNullOrEmpty(text)) {
        for (var i = 0; i < commands.Count && i < 10; i++) {
          CmdItem cmdItem = commands[i];
          sb.Append(cmdItem.Keyword);
          sb.Append(' ');
          sb.AppendLine(cmdItem.HelpText);
        }
      } else {
        var count = 0;
        for (var i = 0; i < commands.Count && count < 10; i++) {
          CmdItem cmdItem = commands[i];
          if (cmdItem.Keyword.StartsWith(text)) {
            sb.Append("<color=#fff>");
            sb.Append(text);
            sb.Append("</color><color=#ddd>");
            sb.Append(cmdItem.Keyword.Substring(text.Length));
            sb.Append("</color>");
            sb.Append(' ');
            sb.AppendLine(cmdItem.HelpText);
          }
        }
      }
      return sb.ToString();
    }

    /// <summary>
    /// 解析并运行指定命令字符串
    /// </summary>
    /// <param name="cmd">命令字符串</param>
    /// <returns>返回是否成功</returns>
    public bool ExecuteCommand(string cmd)
    {
      if (string.IsNullOrEmpty(cmd))
        return false;

      StringSpliter sp = new StringSpliter(cmd, ' ', true);
      if (sp.Count >= 1)
      {
        foreach (CmdItem cmdItem in commands)
        {
          if (cmdItem.Keyword == sp.Result[0])
          {
            //arg
            if (cmdItem.LimitArgCount > 0 && sp.Count - 1 < cmdItem.LimitArgCount)
            {
              Log.E(TAG, "命令 {0} 至少需要 {1} 个参数", sp.Result[0], cmdItem.LimitArgCount);
              return false;
            }

            List<string> arglist = new List<string>(sp.Result);
            arglist.RemoveAt(0);

            //Kernel hander
            if (cmdItem.Callback != null)
              return cmdItem.Callback(sp.Result[0], cmd, arglist.Count, arglist.ToArray());
            else
            {
              Log.W(TAG, "命令 {0}({1}) 无接收器", cmdItem.Keyword, cmdItem.Id);
              return false;
            }
          }
        }
        Log.W(TAG, "未找到命令 {0}", sp.Result[0]);
      }
      return false;
    }

    /// <summary>
    /// 注册调试命令
    /// </summary>
    /// <param name="keyword">命令单词，不能有空格</param>
    /// <param name="callback">命令回调</param>
    /// <param name="limitArgCount">命令最低参数，默认 0 表示无参数或不限制</param>
    /// <param name="helpText">命令帮助文字</param>
    /// <returns>成功返回命令ID，不成功返回-1</returns>
    public int RegisterCommand(string keyword, CommandDelegate callback, int limitArgCount, string helpText)
    {
      if (!IsCommandRegistered(keyword))
      {
        CmdItem item = new CmdItem();
        item.Keyword = keyword;
        item.LimitArgCount = limitArgCount;
        item.HelpText = helpText;
        item.Handler = "";
        item.Callback = callback;
        item.Id = CommonUtils.GenNonDuplicateID();

        commands.Add(item);
        return item.Id;
      }
      else
      {
        GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, "Command {0} already registered", keyword);
        return -1;
      }
    }

    /// <summary>
    /// 取消注册命令
    /// </summary>
    /// <param name="cmdId">命令ID</param>
    public void UnRegisterCommand(int cmdId)
    {
      CmdItem removeItem = null;
      foreach (CmdItem cmdItem in commands)
      {
        if (cmdItem.Id == cmdId)
        {
          removeItem = cmdItem;
          break;
        }
      }
      if (removeItem != null)
        commands.Remove(removeItem);
    }

    /// <summary>
    /// 获取命令是否注册
    /// </summary>
    /// <param name="keyword">命令单词</param>
    /// <returns>返回命令是否注册</returns>
    public bool IsCommandRegistered(string keyword)
    {
      foreach (CmdItem cmdItem in commands)
      {
        if (cmdItem.Keyword == keyword)
          return true;
      }
      return false;
    }

    private bool OnCommandHelp(string keyword, string fullCmd, int argsCount, string[] args)
    {
      string helpText = "命令帮助：\n";
      foreach (CmdItem cmdItem in commands)
        helpText += cmdItem.Keyword + " <color=#adadad>" + cmdItem.HelpText + "</color>\n";
      DebugConsole.Instance.AddStringToConsole(helpText, helpText, LogLevel.Verbose);
      return true;
    }

    private void RegisterSystemCommands()
    {
      //注册基础内置命令
      RegisterCommand("help", OnCommandHelp, 0, "help 显示命令帮助");
      RegisterCommand("e", (keyword, fullCmd, argsCount, args) =>
      {
        Log.V("echo", fullCmd.Substring(2));
        return true;
      }, 1, "e [any] 测试命令");
    }
  }

  /// <summary>
  /// 调试命令回调
  /// </summary>
  /// <param name="keyword">命令单词</param>
  /// <param name="fullCmd">完整命令</param>
  /// <param name="args">命令参数</param>
  /// <returns>须返回命令是否执行成功</returns>
  public delegate bool CommandDelegate(string keyword, string fullCmd, int argsCount, string[] args);
}
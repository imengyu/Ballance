using System.Collections.Generic;
using Ballance.LuaHelpers;
using Ballance2.Sys.Bridge;
using Ballance2.Utils;
using SLua;
using UnityEngine;

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
*
* 更改历史：
* 2021-1-14 创建
*
*/

namespace Ballance2.Sys.Debug
{
    /// <summary>
    /// 调试命令服务，使用此服务添加自定义调试命令。
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("调试命令服务，使用此服务添加自定义调试命令")]
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

        /// <summary>
        /// 运行命令
        /// </summary>
        /// <param name="cmd">命令字符串</param>
        /// <returns>返回是否成功</returns>
        [LuaApiDescription("运行命令", "返回是否成功")]
        [LuaApiParamDescription("cmd", "命令字符串")]
        public bool ExecuteCommand(string cmd)
        {
            if (string.IsNullOrEmpty(cmd)) {
                Log.W(TAG, "请输入要执行的命令");
                return false;
            }

            StringSpliter sp = new StringSpliter(cmd, ' ', true);
            if (sp.Count >= 1)
            {
                foreach (CmdItem cmdItem in commands)
                {
                    if (cmdItem.Keyword == sp.Result[0])
                    {
                        //arg
                        if (cmdItem.LimitArgCount > 0 && sp.Count < cmdItem.LimitArgCount - 1)
                        {
                            Log.E(TAG, "命令 {0} 至少需要 {1} 个参数", sp.Result[0], cmdItem.LimitArgCount);
                            return false;
                        }

                        List<string> arglist = new List<string>(sp.Result);
                        arglist.RemoveAt(0);

                        //Kernel hander
                        if (cmdItem.Callback != null)
                            return cmdItem.Callback(sp.Result[0], cmd, arglist.ToArray());
                        else {
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
        /// <param name="keyword">命令单词</param>
        /// <param name="callback">命令回调</param>
        /// <param name="limitArgCount">命令最低参数，默认 0 表示无参数或不限制</param>
        /// <param name="helpText">命令帮助文字</param>
        /// <returns>成功返回命令ID，不成功返回-1</returns>
        [LuaApiDescription("注册调试命令", "成功返回命令ID，不成功返回-1")]
        [LuaApiParamDescription("keyword", "命令单词")]
        [LuaApiParamDescription("callback", "命令回调")]
        [LuaApiParamDescription("limitArgCount", "命令最低参数，默认 0 表示无参数或不限制")]
        [LuaApiParamDescription("helpText", "命令帮助文字")]
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
                item.Id = CommonUtils.GenNonDuplicateID() ;

                commands.Add(item);
                return item.Id;
            } else {
                GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, "Command {0} already registered", keyword);
                return -1;
            }
        }
        /// <summary>
        /// 取消注册命令
        /// </summary>
        /// <param name="cmdId">命令ID</param>
        [LuaApiDescription("取消注册命令")]
        [LuaApiParamDescription("cmdId", "命令ID")]
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
        [LuaApiDescription("获取命令是否注册", "返回命令是否注册")]
        [LuaApiParamDescription("keyword", "命令单词")]
        public bool IsCommandRegistered(string keyword)
        {
            foreach (CmdItem cmdItem in commands)
            {
                if (cmdItem.Keyword == keyword)
                    return true;
            }
            return false;
        }
        
        private bool OnCommandHelp(string keyword, string fullCmd, string[] args)
        {
            string helpText = "命令帮助：\n";
            foreach (CmdItem cmdItem in commands)
                helpText += cmdItem.Keyword + " <color=#adadad>" + cmdItem.HelpText + "</color>\n";
            Log.D(TAG, helpText);
            return true;
        }

        internal void RegisterSystemCommands(GameManager game) {
            //注册基础内置命令
            RegisterCommand("quit", (keyword, fullCmd, args) =>
            {
                game.QuitGame();
                return true;
            }, 0, "退出游戏");
            RegisterCommand("echo", (keyword, fullCmd, args) =>
            {
                Log.D("echo", fullCmd.Substring(3));
                return true;
            }, 1, "[any] 测试");
            RegisterCommand("lua", (keyword, fullCmd, args) =>
            {
                game.GameMainLuaState.doString(fullCmd.Substring(3));
                return true;
            }, 1, "[any] 运行 LUA 命令");
            RegisterCommand("fps", (keyword, fullCmd, args) =>
            {
                int fpsVal = 0;
                if(args.Length >= 1)
                {
                    if (int.TryParse(args[0], out fpsVal) && fpsVal > 0 && fpsVal <= 120) Application.targetFrameRate = fpsVal;
                    else Log.E(TAG, "错误的参数：{0}", args[0]);
                }
                Log.D(TAG, "Application.targetFrameRate = {0}", Application.targetFrameRate);
                return true;
            }, 0, "[targetFps:number] 获取或设置 targetFrameRate");
            RegisterCommand("help", OnCommandHelp, 1, "显示命令帮助");
        }
    }
}
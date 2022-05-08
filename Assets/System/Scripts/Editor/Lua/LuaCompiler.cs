using System.Text;
using System.IO;
using Ballance2.Utils;
using UnityEditor;
using UnityEngine;

namespace Ballance2.Editor.Lua
{
    public static class LuaCompiler
    {
#if UNITY_EDITOR_WIN
        public const string LUAC_PATH = "Tools/Lua/win32/luac5.1.exe";
#elif UNITY_EDITOR_LINUX
        public const string LUAC_PATH = "Tools/Lua/linux/luac5.1";
#elif UNITY_EDITOR_OSX
        public const string LUAC_PATH = "Tools/Lua/macos/luac5.1";
#endif

        public const string CACHE_PATH = "Temp/LuaCompilerCache";

        public static bool CompileLuaFile(string source, bool withDebugInfo, out string tempFilePath) {

            var dir = Directory.GetCurrentDirectory().Replace("\\", "/");
            var cachePath = dir + "/" + CACHE_PATH;
            if(!Directory.Exists(cachePath))
                Directory.CreateDirectory(cachePath);
            
            tempFilePath = string.Format("{0}/{1}.luac", cachePath, StringUtils.MD5String(source));
            var argument = string.Format("{0}-o \"{1}\" \"{2}\"", withDebugInfo ? "" : "-s ", tempFilePath, source).Replace("//", "/");

            
            #region Process
            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();
            bool success = false;
            try
            {
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = dir + "/" + LUAC_PATH;
                process.StartInfo.Arguments = argument;
                process.StartInfo.WorkingDirectory = dir;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                using (var outputWaitHandle = new System.Threading.AutoResetEvent(false))
                using (var errorWaitHandle = new System.Threading.AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            outputWaitHandle.Set();
                        }
                        else
                        {
                            output.AppendLine(e.Data);
                        }
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            errorWaitHandle.Set();
                        }
                        else
                        {
                            error.AppendLine(e.Data);
                        }
                    };
                    // http://stackoverflow.com/questions/139593/processstartinfo-hanging-on-waitforexit-why
                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    const int timeout = 30;
                    if (process.WaitForExit(timeout * 1000) &&
                        outputWaitHandle.WaitOne(timeout * 1000) &&
                        errorWaitHandle.WaitOne(timeout * 1000))
                    {
                        success = (process.ExitCode == 0);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Exception: " + ex.ToString());
            }
            if (!success) 
                Debug.LogError(error.ToString());
            #endregion

            return success;
        }        
    
        public static byte[] CompileLuaFile(string source, bool withDebugInfo) {

            if(!Directory.Exists(CACHE_PATH))
                Directory.CreateDirectory(CACHE_PATH);
            
            var tempFilePath = string.Format("{0}/{1}.luac", CACHE_PATH, StringUtils.MD5String(source));
            var argument = string.Format("{0}-o \"{1}\" \"{2}\"", withDebugInfo ? "" : "-s ", tempFilePath, source);

            
            #region Process
            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();
            bool success = false;
            try
            {
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = LUAC_PATH;
                process.StartInfo.Arguments = "@" + argument;
                process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                using (var outputWaitHandle = new System.Threading.AutoResetEvent(false))
                using (var errorWaitHandle = new System.Threading.AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            outputWaitHandle.Set();
                        }
                        else
                        {
                            output.AppendLine(e.Data);
                        }
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                        {
                            errorWaitHandle.Set();
                        }
                        else
                        {
                            error.AppendLine(e.Data);
                        }
                    };
                    // http://stackoverflow.com/questions/139593/processstartinfo-hanging-on-waitforexit-why
                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    const int timeout = 30;
                    if (process.WaitForExit(timeout * 1000) &&
                        outputWaitHandle.WaitOne(timeout * 1000) &&
                        errorWaitHandle.WaitOne(timeout * 1000))
                    {
                        success = (process.ExitCode == 0);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
            }
            #endregion

            Debug.Log(output.ToString());

            if (!success) {
                Debug.LogError(error.ToString());
                if(File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
                return null;
            }
            else if(File.Exists(tempFilePath)) {
                MemoryStream ms = new MemoryStream();
                FileStream fs = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read);
                fs.CopyTo(ms);
                fs.Close();
            
                File.Delete(tempFilePath);

                var rs = ms.ToArray();
                ms.Close();
                return rs;
            } else {
                throw new FileNotFoundException(tempFilePath + " not found!");
            }
        }        
    }
}
using System.Text;
using System.IO;
using Ballance2.Utils;
using UnityEditor;
using UnityEngine;

namespace Ballance2.Editor.Lua
{
  public static class CSharpCompiler
  {
    public static bool CompileToCsharpDll(string dllName, string sourceDir, bool withDebugInfo)
    {
      var argument = string.Format("/target:library /out:{0}.dll /warn:0{1} /nologo *.cs", dllName, withDebugInfo ? "" : " /debug");

      #region Process
      StringBuilder output = new StringBuilder();
      StringBuilder error = new StringBuilder();
      bool success = false;
      try
      {
        var process = new System.Diagnostics.Process();
        process.StartInfo.FileName = "csc";
        process.StartInfo.Arguments = argument;
        process.StartInfo.WorkingDirectory = sourceDir;
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
  }
}
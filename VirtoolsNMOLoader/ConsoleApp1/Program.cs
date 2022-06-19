using System;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
  internal class Program
  {
    [DllImport("Kernel32", CallingConvention = CallingConvention.StdCall)]
    public static extern int LoadLibraryA([MarshalAs(UnmanagedType.LPStr)] string dll);
    [DllImport("Kernel32", CallingConvention = CallingConvention.StdCall)]
    public static extern int SetDllDirectoryA([MarshalAs(UnmanagedType.LPStr)] string path);
    [DllImport("Kernel32", CallingConvention = CallingConvention.StdCall)]
    public static extern int SetErrorMode(int mode);
    [DllImport("Kernel32", CallingConvention = CallingConvention.StdCall)]
    public static extern int GetErrorMode();
    [DllImport("Kernel32", CallingConvention = CallingConvention.StdCall)]
    public static extern int GetLastError();

    static void Main(string[] args)
    {
      SetDllDirectoryA(@"E:\Programming\GameProjects\Ballance2\VirtoolsNMOLoader\Debug\");
      Console.WriteLine("LoadLibraryA result " + LoadLibraryA(@"E:\Programming\GameProjects\Ballance2\VirtoolsNMOLoader\Debug\VirtoolsNMOLoader.dll") + " LastError: " + GetLastError());
      Console.WriteLine("Hello World!");
      Console.ReadKey();
    }
  }
}

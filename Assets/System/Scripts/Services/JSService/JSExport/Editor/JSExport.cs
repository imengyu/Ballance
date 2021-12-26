using System;
using System.Collections.Generic;
using Puerts;

namespace Ballance2.JSService.JSExport 
{
  /// <summary>
  /// JS 自动导出配置类
  /// </summary>
  [Configure]
  public class JSExport
  {
    static string[] ExportModules = new string[] {
      "UnityEngine",
      "UnityEngine.CoreModule",
      "UnityEngine.UIModule",
      "UnityEngine.TextRenderingModule",
      "UnityEngine.UnityWebRequestWWWModule",
      "UnityEngine.Physics2DModule",
      "UnityEngine.AnimationModule",
      "UnityEngine.TextRenderingModule",
      "UnityEngine.IMGUIModule",
      "UnityEngine.UnityWebRequestModule",
      "UnityEngine.PhysicsModule", 
      "UnityEngine.UI",
      "UnityEngine.AudioModule",
      "Assembly-CSharp",
      "Assembly-CSharp-firstpass",
    };

    [Binding]
    static IEnumerable<Type> Bindings
    {
      get
      {
        return JSCodeGen.GetExportsType(ExportModules);
      }
    }

    [Filter]
    static bool Filter(System.Reflection.MemberInfo memberInfo)
    {
      if(memberInfo.IsDefined(typeof(JSNotExportAttribute), true) 
        || CustomExport.FunctionFilterList.Contains(memberInfo.DeclaringType.FullName + '.' + memberInfo.Name)) {
        return true;
      }
        
      return false;
    }
  }
}
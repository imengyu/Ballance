// The MIT License (MIT)

// Copyright 2015 Siney/Pangweiwei siney@yeah.net
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Ballance2.JSService.JSExport
{
  using UnityEngine;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.IO;
  using System;
  using System.Reflection;
  using UnityEditor;
  using System.Text;
  using System.Text.RegularExpressions;
  using System.Runtime.CompilerServices;

  public interface ICustomExportPost { }

  public class JSCodeGen : MonoBehaviour
  {    
    public delegate void ExportGenericDelegate(Type t, string ns);

#if UNITY_2017_2_OR_NEWER
    public static string[] unityModule = new string[] { "UnityEngine","UnityEngine.CoreModule","UnityEngine.UIModule","UnityEngine.TextRenderingModule","UnityEngine.TextRenderingModule",
                "UnityEngine.UnityWebRequestWWWModule","UnityEngine.Physics2DModule","UnityEngine.AnimationModule","UnityEngine.TextRenderingModule","UnityEngine.IMGUIModule","UnityEngine.UnityWebRequestModule",
            "UnityEngine.PhysicsModule", "UnityEngine.UI", "UnityEngine.AudioModule" };
#else
    public static string[] unityModule = null;
#endif

    static public bool filterType(Type t, List<string> noUseList, List<string> uselist)
    {
      if (t.IsDefined(typeof(CompilerGeneratedAttribute), false))
      {
        Debug.Log(t.Name + " is filtered out");
        return false;
      }

      // check type in uselist
      string fullName = t.FullName;
      if (uselist != null && uselist.Count > 0)
      {
        return uselist.Contains(fullName);
      }
      else
      {
        // check type not in nouselist
        foreach (string str in noUseList)
        {
          if (fullName.Contains(str))
          {
            return false;
          }
        }
        return true;
      }
    }

    static public List<object> InvokeEditorMethod<T>(string methodName, ref object[] parameters)
    {
      List<object> aReturn = new List<object>();
      System.Reflection.Assembly editorAssembly = System.Reflection.Assembly.Load("Assembly-CSharp-Editor");
      Type[] editorTypes = editorAssembly.GetExportedTypes();
      foreach (Type t in editorTypes)
      {
        if (typeof(T).IsAssignableFrom(t))
        {
          System.Reflection.MethodInfo method = t.GetMethod(methodName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
          if (method != null)
          {
            object cRes = method.Invoke(null, parameters);
            if (null != cRes)
            {
              aReturn.Add(cRes);
            }
          }
        }
      }

      return aReturn;
    }

    static public List<object> GetEditorField<T>(string strFieldName)
    {
      List<object> aReturn = new List<object>();
      System.Reflection.Assembly cEditorAssembly = System.Reflection.Assembly.Load("Assembly-CSharp-Editor");
      Type[] aEditorTypes = cEditorAssembly.GetExportedTypes();
      foreach (Type t in aEditorTypes)
      {
        if (typeof(T).IsAssignableFrom(t))
        {
          FieldInfo cField = t.GetField(strFieldName, BindingFlags.Static | BindingFlags.Public);
          if (null != cField)
          {
            object cValue = cField.GetValue(t);
            if (null != cValue)
            {
              aReturn.Add(cValue);
            }
          }
        }
      }

      return aReturn;
    }

    public static List<Type> GetExportsType(string[] asemblyNames)
    {
      List<Type> exports = new List<Type>();

      HashSet<string> namespaces = CustomExport.OnAddCustomNamespace();

      // Add custom namespaces.
      object[] aCustomExport = null;
      List<object> aCustomNs = JSCodeGen.InvokeEditorMethod<ICustomExportPost>("OnAddCustomNamespace", ref aCustomExport);
      foreach (object cNsSet in aCustomNs)
      {
        foreach (string strNs in (HashSet<string>)cNsSet)
        {
          namespaces.Add(strNs);
        }
      }

      foreach (string asemblyName in asemblyNames)
      {
        Assembly assembly;
        try { assembly = Assembly.Load(asemblyName); }
        catch (Exception) { continue; }

        Type[] types = assembly.GetExportedTypes();

        if(asemblyName.StartsWith("Assembly")) {

          foreach (Type t in types)
          {
            if ((t.IsDefined(typeof(JSExportAttribute), false) || namespaces.Contains(t.Namespace)) && !t.IsDefined(typeof(JSNotExportAttribute), true))
            {
              exports.Add(t);
            }
          }

        } else {

          List<string> uselist;
          List<string> noUseList;

          CustomExport.OnGetNoUseList(out noUseList);
          CustomExport.OnGetUseList(out uselist);

          // Get use and nouse list from custom export.
          aCustomExport = new object[1];
          InvokeEditorMethod<ICustomExportPost>("OnGetUseList", ref aCustomExport);
          if (null != aCustomExport[0])
          {
            if (null != uselist)
            {
              uselist.AddRange((List<string>)aCustomExport[0]);
            }
            else
            {
              uselist = (List<string>)aCustomExport[0];
            }
          }

          aCustomExport[0] = null;
          InvokeEditorMethod<ICustomExportPost>("OnGetNoUseList", ref aCustomExport);
          if (null != aCustomExport[0])
          {
            if ((null != noUseList))
            {
              noUseList.AddRange((List<string>)aCustomExport[0]);
            }
            else
            {
              noUseList = (List<string>)aCustomExport[0];
            }
          }

          foreach (Type t in types)
          {
            if (filterType(t, noUseList, uselist))
              exports.Add(t);
          }
        }
      }

      return exports;
    }
  }
}

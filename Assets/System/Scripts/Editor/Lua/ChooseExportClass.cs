using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SLua;


public class ChooseExportClass : EditorWindow {

  public static ChooseExportClass ShowWindow() {
    var window = GetWindow<ChooseExportClass>();
    window.titleContent = new GUIContent("ChooseExportClass");
    window.Show();

    var assembly = Assembly.Load("Assembly-CSharp");
    var types = Assembly.Load("Assembly-CSharp").GetTypes();

    foreach (Type t in types)
    {
      if (t.IsDefined(typeof(CustomLuaClassAttribute), false)) {
        var type = new ClassTypeEn();
        type.enable = false;
        type.type = t;
        type.name = t.FullName;
        window.typeNames.Add(type);
      }
    }
  
    return window;
  }

  private struct ClassTypeEn {
    public bool enable;
    public string name;
    public Type type;
  }
  private List<ClassTypeEn> typeNames = new List<ClassTypeEn>();
  private Vector2 scrollPos = new Vector2();

  public delegate void ChooseExportClassDelegate(Type[] typeNames);
  public ChooseExportClassDelegate Chooseed;

  private void OnGUI() {
    scrollPos = GUILayout.BeginScrollView(scrollPos);

    for (int i = 0, c = typeNames.Count; i < c; i++)
    {
      var vl = typeNames[i];
      var newV = GUILayout.Toggle(vl.enable, vl.name);
      if(newV != vl.enable) {
        vl.enable = newV;
        typeNames[i] = vl;
      }
    }
    
    GUILayout.EndScrollView();

    if(GUILayout.Button("生成") && Chooseed != null) {
      List<Type> types = new List<Type>();
      foreach(var v in typeNames) {
        if(v.enable)
          types.Add(v.type);
      }
      Chooseed.Invoke(types.ToArray());
    }
  }
}


using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class ChooseLuaFile : EditorWindow {

  public static ChooseLuaFile ShowWindow(string packagePath) {
    var window = GetWindow<ChooseLuaFile>();
    window.titleContent = new GUIContent("ChooseLuaFile");
    window.GenNames(packagePath);
    window.Show();
    return window;
  }

  private void GenNames(string packagePath) {
    DirectoryInfo direction = new DirectoryInfo(packagePath);
    FileInfo[] file = direction.GetFiles("*.lua", SearchOption.AllDirectories);
    for (int i = 0; i < file.Length; i++) {
      var path = file[i].FullName.Replace('\\', '/');
      var ix = path.IndexOf("Assets/");
      if(ix > 0)
        path = path.Substring(ix);
      fileNames.Add(path); 
    }
  }

  private string choosedFilePath = "";
  private List<string> fileNames = new List<string>();
  private Vector2 scrollPos = new Vector2();

  public delegate void ChooseDelegate(string filePath);
  public ChooseDelegate Chooseed;

  private void OnGUI() {
    scrollPos = GUILayout.BeginScrollView(scrollPos);

    for (int i = 0, c = fileNames.Count; i < c; i++)
    {
      var vl = fileNames[i];
      var newV = GUILayout.Toggle(choosedFilePath == vl, vl);
      if(newV) 
        choosedFilePath = vl;
    }
    
    GUILayout.EndScrollView();

    if(GUILayout.Button("确定") && Chooseed != null) 
      Chooseed.Invoke(choosedFilePath);
  }
}


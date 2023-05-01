using UnityEngine;
using UnityEditor;

public class EditorTools : Editor {
        
  [MenuItem("Tools/Export RenderTexture")]
  public static void SaveTextureToFile() { 
    EditorWindow.GetWindowWithRect(typeof(ExportRenderTexture), new Rect(200, 150, 450, 400));
  }
}
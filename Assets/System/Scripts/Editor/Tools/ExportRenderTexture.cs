using UnityEngine;
using UnityEditor;
using System.IO;

public class ExportRenderTexture : EditorWindow
{
  public ExportRenderTexture()
  {
    titleContent = new GUIContent("Export RenderTexture");
  }

  private SerializedObject serializedObject;
  private SerializedProperty pTexture = null;

  [SerializeField]
  private RenderTexture Texture = null;

  private GUIStyle groupBox = null;

  private void OnEnable()
  {
    serializedObject = new SerializedObject(this);
    pTexture = serializedObject.FindProperty("Texture");
  }
  private void OnDisable()
  {
    EditorUtility.ClearProgressBar();
  }
  private void OnGUI()
  {
    bool close = false;
    serializedObject.Update();

    if (groupBox == null)
      groupBox = GUI.skin.FindStyle("GroupBox");

    EditorGUI.BeginChangeCheck();

    EditorGUILayout.BeginVertical(groupBox);
    EditorGUILayout.PropertyField(pTexture, new GUIContent("Texture"));

    GUILayout.Space(50);

    if (GUILayout.Button("OK")) {
      close = true;
    }

    EditorGUILayout.EndVertical();

    if (EditorGUI.EndChangeCheck())
      serializedObject.ApplyModifiedProperties();

    if(close) {
      RenderTexture prev = RenderTexture.active; 
      RenderTexture.active = Texture; 
      Texture2D png = new Texture2D(Texture.width, Texture.height, TextureFormat.ARGB32, false); 
      png.ReadPixels(new Rect(0, 0, Texture.width, Texture.height), 0, 0); 
      byte[] bytes = png.EncodeToPNG(); 
      string path = string.Format("Dump/raw {0}.png", Random.Range(0, 65536).ToString("X")); 
      FileStream file = File.Open(path, FileMode.Create); 
      BinaryWriter writer = new BinaryWriter(file); 
      writer.Write(bytes); 
      file.Close(); 
      Texture2D.Destroy(png);
      RenderTexture.active = prev; 
    }
  }
}
using UnityEditor;
using UnityEngine;
using Ballance2.Entry;

[CustomEditor(typeof(GameEntry), true)]
public class GameEntryEditor : Editor
{
    private bool bDrawDebugInspector = true;
    private bool bDrawGlobalConfigInspector = true;
    private bool bDrawGlobalUIInspector = true;
    private new GameEntry target = null;

    private SerializedProperty GameBaseCamera;
    private SerializedProperty GameCanvas;
    private SerializedProperty GameGlobalErrorUI;
    private SerializedProperty GameGlobalIngameLoading;
    private SerializedProperty GlobalGameScriptErrDialog;
    private SerializedProperty DebugMode;
    private SerializedProperty DebugTargetFrameRate;
    private SerializedProperty DebugSetFrameRate;
    private SerializedProperty DebugType;
    private SerializedProperty DebugInitPackages;
    private SerializedProperty DebugCustomEntryEvent;
    private SerializedProperty DebugCustomEntryEventParamas;
    private SerializedProperty DebugCustomEntries;
    private SerializedProperty DebugLoadCustomPackages;
    private SerializedProperty DebugSkipIntro;


    private void OnEnable()
    {
        bDrawDebugInspector = EditorPrefs.GetBool("GameEntryEditor_bDrawDebugInspector", false);
        bDrawGlobalConfigInspector = EditorPrefs.GetBool("GameEntryEditor_bDrawGlobalConfigInspector", false);
        bDrawGlobalUIInspector = EditorPrefs.GetBool("GameEntryEditor_bDrawGlobalUIInspector", false);

        GameBaseCamera = serializedObject.FindProperty("GameBaseCamera");
        GameCanvas = serializedObject.FindProperty("GameCanvas");
        GameGlobalErrorUI = serializedObject.FindProperty("GameGlobalErrorUI");
        GlobalGameScriptErrDialog = serializedObject.FindProperty("GlobalGameScriptErrDialog");
        GameGlobalIngameLoading = serializedObject.FindProperty("GameGlobalIngameLoading");

        DebugMode = serializedObject.FindProperty("DebugMode");
        DebugTargetFrameRate = serializedObject.FindProperty("DebugTargetFrameRate");
        DebugSetFrameRate = serializedObject.FindProperty("DebugSetFrameRate");
        DebugType = serializedObject.FindProperty("DebugType");
        DebugInitPackages = serializedObject.FindProperty("DebugInitPackages");
        DebugCustomEntryEvent = serializedObject.FindProperty("DebugCustomEntryEvent");
        DebugCustomEntryEventParamas = serializedObject.FindProperty("DebugCustomEntryEventParamas");
        DebugCustomEntries = serializedObject.FindProperty("DebugCustomEntries");
        DebugLoadCustomPackages = serializedObject.FindProperty("DebugLoadCustomPackages");
        DebugSkipIntro = serializedObject.FindProperty("DebugSkipIntro");
    }
    private void OnDisable()
    {
        EditorPrefs.SetBool("GameEntryEditor_bDrawDebugInspector", bDrawDebugInspector);
        EditorPrefs.SetBool("GameEntryEditor_bDrawGlobalConfigInspector", bDrawGlobalConfigInspector);
        EditorPrefs.SetBool("GameEntryEditor_bDrawGlobalUIInspector", bDrawGlobalUIInspector);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        target = (GameEntry)base.target;

        EditorGUI.BeginChangeCheck();

        bDrawDebugInspector = EditorGUILayout.Foldout(bDrawDebugInspector, "全局调试配置", true);
        if (bDrawDebugInspector)
            DrawDebugInspector();

        bDrawGlobalConfigInspector = EditorGUILayout.Foldout(bDrawGlobalConfigInspector, "全局配置", true);
        if (bDrawGlobalConfigInspector)
            DrawGlobalConfigInspector();

        bDrawGlobalUIInspector = EditorGUILayout.Foldout(bDrawGlobalUIInspector, "全局静态UI配置", true);
        if (bDrawGlobalUIInspector)
            DrawGlobalUIInspector();

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void DrawDebugInspector() {   
        
        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(DebugMode);
        EditorGUILayout.PropertyField(DebugSetFrameRate);
        EditorGUI.BeginDisabledGroup(!DebugSetFrameRate.boolValue);
        EditorGUILayout.PropertyField(DebugTargetFrameRate);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(DebugType);
        EditorGUILayout.PropertyField(DebugSkipIntro);
        
        EditorGUI.BeginDisabledGroup(DebugType.enumValueIndex == (int)GameDebugType.NoDebug || DebugType.enumValueIndex == (int)GameDebugType.FullDebug);
        EditorGUILayout.PropertyField(DebugInitPackages);
        EditorGUILayout.PropertyField(DebugLoadCustomPackages);
        EditorGUI.EndDisabledGroup();

        var arr = target.DebugCustomEntries;
        var newIndex = EditorGUILayout.Popup(DebugCustomEntryEvent.displayName, arr.IndexOf(DebugCustomEntryEvent.stringValue), arr.ToArray());
        if(newIndex >= 0 && newIndex < arr.Count)
            DebugCustomEntryEvent.stringValue = arr[newIndex];

        EditorGUILayout.PropertyField(DebugCustomEntryEventParamas);
        EditorGUILayout.PropertyField(DebugCustomEntries);

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }    
    private void DrawGlobalConfigInspector() {
        
    }    
    private void DrawGlobalUIInspector() {
        
        EditorGUILayout.BeginVertical();
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(GameBaseCamera);
        EditorGUILayout.PropertyField(GameCanvas);
        EditorGUILayout.PropertyField(GameGlobalErrorUI);
        EditorGUILayout.PropertyField(GlobalGameScriptErrDialog);
        EditorGUILayout.PropertyField(GameGlobalIngameLoading);

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
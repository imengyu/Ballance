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
    private SerializedProperty GameDebugBeginStats;
    private SerializedProperty GlobalGamePermissionTipDialog;
    private SerializedProperty GlobalGameUserAgreementTipDialog;
    private SerializedProperty ButtonUserAgreementAllow;
    private SerializedProperty CheckBoxAllowUserAgreement;
    private SerializedProperty LinkPrivacyPolicy;
    private SerializedProperty LinkUserAgreement;
    private SerializedProperty DebugMode;
    private SerializedProperty DebugTargetFrameRate;
    private SerializedProperty DebugSetFrameRate;
    private SerializedProperty DebugEnableLuaDebugger;
    private SerializedProperty DebugType;
    private SerializedProperty DebugInitPackages;
    private SerializedProperty DebugCustomEntryEvent;
    private SerializedProperty DebugCustomEntries;
    private SerializedProperty DebugLoadCustomPackages;
    private SerializedProperty DebugSkipIntro;
    private SerializedProperty DebugLuaDebugger;

    private void OnEnable()
    {
        bDrawDebugInspector = EditorPrefs.GetBool("GameEntryEditor_bDrawDebugInspector", false);
        bDrawGlobalConfigInspector = EditorPrefs.GetBool("GameEntryEditor_bDrawGlobalConfigInspector", false);
        bDrawGlobalUIInspector = EditorPrefs.GetBool("GameEntryEditor_bDrawGlobalUIInspector", false);

        GameBaseCamera = serializedObject.FindProperty("GameBaseCamera");
        GameCanvas = serializedObject.FindProperty("GameCanvas");
        GameGlobalErrorUI = serializedObject.FindProperty("GameGlobalErrorUI");
        GameDebugBeginStats = serializedObject.FindProperty("GameDebugBeginStats");
        GlobalGamePermissionTipDialog = serializedObject.FindProperty("GlobalGamePermissionTipDialog");
        GlobalGameUserAgreementTipDialog = serializedObject.FindProperty("GlobalGameUserAgreementTipDialog");
        ButtonUserAgreementAllow = serializedObject.FindProperty("ButtonUserAgreementAllow");
        CheckBoxAllowUserAgreement = serializedObject.FindProperty("CheckBoxAllowUserAgreement");
        LinkPrivacyPolicy = serializedObject.FindProperty("LinkPrivacyPolicy");
        LinkUserAgreement = serializedObject.FindProperty("LinkUserAgreement");

        DebugLuaDebugger = serializedObject.FindProperty("DebugLuaDebugger");
        DebugMode = serializedObject.FindProperty("DebugMode");
        DebugTargetFrameRate = serializedObject.FindProperty("DebugTargetFrameRate");
        DebugSetFrameRate = serializedObject.FindProperty("DebugSetFrameRate");
        DebugEnableLuaDebugger = serializedObject.FindProperty("DebugEnableLuaDebugger");
        DebugType = serializedObject.FindProperty("DebugType");
        DebugInitPackages = serializedObject.FindProperty("DebugInitPackages");
        DebugCustomEntryEvent = serializedObject.FindProperty("DebugCustomEntryEvent");
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
        EditorGUILayout.PropertyField(DebugEnableLuaDebugger);
        EditorGUILayout.PropertyField(DebugLuaDebugger);
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
        EditorGUILayout.PropertyField(GameDebugBeginStats);
        EditorGUILayout.PropertyField(GlobalGamePermissionTipDialog);
        EditorGUILayout.PropertyField(GlobalGameUserAgreementTipDialog);
        EditorGUILayout.PropertyField(ButtonUserAgreementAllow);
        EditorGUILayout.PropertyField(CheckBoxAllowUserAgreement);
        EditorGUILayout.PropertyField(LinkPrivacyPolicy);
        EditorGUILayout.PropertyField(LinkUserAgreement);

        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}
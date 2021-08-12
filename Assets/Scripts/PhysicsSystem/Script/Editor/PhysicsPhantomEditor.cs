using System;
using System.Collections.Generic;
using PhysicsRT;
using Unity.Mathematics;
using Unity.Physics.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PhysicsPhantom))]
[CanEditMultipleObjects]
class PhysicsPhantomEditor : Editor
{
    private PhysicsPhantom instance;
    
    [NonSerialized]
    private string[] m_LayerNames = new string[33];
    [NonSerialized]
    private int[] m_LayerValues = new int[33];

    public PhysicsPhantomEditor() {
        for(int i = 0; i < 32; i++) 
            m_LayerValues[i] = i;
    }

    private void OnEnable() {
        pType = serializedObject.FindProperty("m_Type");
        pMax = serializedObject.FindProperty("m_Max");
        pMin = serializedObject.FindProperty("m_Min");
        pLayer = serializedObject.FindProperty("m_Layer");
        pDoNotAutoCreateAtAwake = serializedObject.FindProperty("m_DoNotAutoCreateAtAwake");
        pEnableListener = serializedObject.FindProperty("m_EnableListener"); 

        var names = AssetDatabase.LoadAssetAtPath<PhysicsLayerNames>("Assets/Resources/PhysicsLayerNames.asset");
        var tags = PhysicsLayerTags.Everything;
        for(int i = 0; i < 32; i++) 
            m_LayerNames[i] = i + ": " + (string.IsNullOrEmpty(names.LayerNames[i]) ? "(Undefined layer name)" : names.LayerNames[i]);
        m_LayerNames[32] = "Not set";
        m_LayerValues[32] = -1;
    }
    private void OnDisable() {

    }

    private SerializedProperty pType;
    private SerializedProperty pMax;
    private SerializedProperty pMin;
    private SerializedProperty pEnableListener;
    private SerializedProperty pLayer;
    private SerializedProperty pDoNotAutoCreateAtAwake;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        instance = (PhysicsPhantom)target;
        //Update value at runtime
        if(EditorApplication.isPlaying)
            instance.BackUpRuntimeCanModifieProperties();
        EditorGUI.BeginChangeCheck();

         EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit Collider"), base.target);

        EditorGUILayout.PropertyField(pMin);
        EditorGUILayout.PropertyField(pMax);

        if(EditorApplication.isPlaying)
            EditorGUILayout.HelpBox("Some values can't change at runtime.", MessageType.Warning);

        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);

        EditorGUILayout.PropertyField(pType);
        EditorGUILayout.PropertyField(pDoNotAutoCreateAtAwake);
        EditorGUILayout.PropertyField(pEnableListener);
        pLayer.intValue = EditorGUILayout.IntPopup("Layer", pLayer.intValue, m_LayerNames, m_LayerValues);

        EditorGUI.EndDisabledGroup();
        
        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
            //Update value at runtime
            if(EditorApplication.isPlaying)
                instance.ApplyModifiedProperties();
        }
    }
}
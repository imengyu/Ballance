using System;
using System.Collections.Generic;
using PhysicsRT;
using Unity.Mathematics;
using Unity.Physics.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof( PhysicsBody))]
[CanEditMultipleObjects]
class PhysicsBodyEditor : Editor
{
    private PhysicsBody instance;
    
    [NonSerialized]
    private string[] m_LayerNames = new string[33];
    [NonSerialized]
    private int[] m_LayerValues = new int[33];

    public PhysicsBodyEditor() {
        for(int i = 0; i < 32; i++) 
            m_LayerValues[i] = i;
    }

    private void OnEnable() {
        pMotionType = serializedObject.FindProperty("m_MotionType");
        pCollidableQualityType = serializedObject.FindProperty("m_CollidableQualityType");
        pMass = serializedObject.FindProperty("m_Mass");
        pLinearDamping = serializedObject.FindProperty("m_LinearDamping");
        pAngularDamping = serializedObject.FindProperty("m_AngularDamping");
        pInitialLinearVelocity = serializedObject.FindProperty("m_InitialLinearVelocity");
        pInitialAngularVelocity = serializedObject.FindProperty("m_InitialAngularVelocity");
        pGravityFactor = serializedObject.FindProperty("m_GravityFactor");
        pCenterOfMass = serializedObject.FindProperty("m_CenterOfMass");
        pCustomTags = serializedObject.FindProperty("m_CustomTags");
        pFriction = serializedObject.FindProperty("m_Friction");
        pRestitution = serializedObject.FindProperty("m_Restitution");
        pLayer = serializedObject.FindProperty("m_Layer");
        pTigger = serializedObject.FindProperty("m_IsTigger");
        pDoNotAutoCreateAtAwake = serializedObject.FindProperty("m_DoNotAutoCreateAtAwake");
        pAddContactListener = serializedObject.FindProperty("m_AddContactListener"); 
        pAutoComputeCenterOfMass = serializedObject.FindProperty("m_AutoComputeCenterOfMass"); 
        pAutoControlActive = serializedObject.FindProperty("m_AutoControlActive"); 
        pInertiaTensor1 = serializedObject.FindProperty("m_InertiaTensor1"); 
        pInertiaTensor2 = serializedObject.FindProperty("m_InertiaTensor2"); 
        pInertiaTensor3 = serializedObject.FindProperty("m_InertiaTensor3"); 
        pSystemGroupName = serializedObject.FindProperty("m_SystemGroupName"); 
        pSubSystemId = serializedObject.FindProperty("m_SubSystemId"); 
        pSubSystemDontCollideWith = serializedObject.FindProperty("m_SubSystemDontCollideWith");   
        pCustomLayer = serializedObject.FindProperty("CustomLayer");   

        bOpenCollisionFilterInfo = EditorPrefs.GetBool("PhysicsBodyEditor_bOpenCollisionFilterInfo", false);
        bInertiaTensor = EditorPrefs.GetBool("PhysicsBodyEditor_bInertiaTensor", false);

        var names = AssetDatabase.LoadAssetAtPath<PhysicsLayerNames>("Assets/Resources/PhysicsLayerNames.asset");
        var tags = PhysicsLayerTags.Everything;
        for(int i = 0; i < 32; i++) 
            m_LayerNames[i] = i + ": " + (string.IsNullOrEmpty(names.LayerNames[i]) ? "(Undefined layer name)" : names.LayerNames[i]);
        m_LayerNames[32] = "Not set";
        m_LayerValues[32] = -1;
    }
    private void OnDisable() {
        EditorPrefs.SetBool("PhysicsBodyEditor_bOpenCollisionFilterInfo", bOpenCollisionFilterInfo);
        EditorPrefs.SetBool("PhysicsBodyEditor_bInertiaTensor", bInertiaTensor);


    }

    private SerializedProperty pMotionType;
    private SerializedProperty pCollidableQualityType;
    private SerializedProperty pMass;
    private SerializedProperty pLinearDamping;
    private SerializedProperty pAngularDamping;
    private SerializedProperty pInitialLinearVelocity;
    private SerializedProperty pInitialAngularVelocity;
    private SerializedProperty pInertiaTensor1;
    private SerializedProperty pInertiaTensor2;
    private SerializedProperty pInertiaTensor3;
    private SerializedProperty pGravityFactor;
    private SerializedProperty pCenterOfMass;
    private SerializedProperty pCustomTags;
    private SerializedProperty pFriction;
    private SerializedProperty pRestitution;
    private SerializedProperty pTigger;
    private SerializedProperty pAddContactListener;
    private SerializedProperty pDoNotAutoCreateAtAwake;
    private SerializedProperty pAutoComputeCenterOfMass;
    private SerializedProperty pAutoControlActive;
    private SerializedProperty pCustomLayer;

    private SerializedProperty pLayer;
    private SerializedProperty pSystemGroupName;
    private SerializedProperty pSubSystemId;
    private SerializedProperty pSubSystemDontCollideWith;

    private bool bOpenCollisionFilterInfo = false;
    private bool bInertiaTensor = false;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        instance = (PhysicsBody)target;
        //Update value at runtime
        if(EditorApplication.isPlaying)
            instance.BackUpRuntimeCanModifieProperties();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(pMotionType);
        EditorGUILayout.PropertyField(pAutoControlActive);

        if(EditorApplication.isPlaying)
            EditorGUILayout.HelpBox("Some values can't change at runtime.", MessageType.Warning);

        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);

        EditorGUILayout.PropertyField(pDoNotAutoCreateAtAwake);
        EditorGUILayout.PropertyField(pCollidableQualityType);
        EditorGUILayout.PropertyField(pInitialLinearVelocity);
        EditorGUILayout.PropertyField(pInitialAngularVelocity);
        EditorGUILayout.PropertyField(pAutoComputeCenterOfMass);
        EditorGUILayout.PropertyField(pCustomTags);
        EditorGUILayout.PropertyField(pTigger);
        EditorGUILayout.PropertyField(pAddContactListener);

        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.PropertyField(pMass);
        EditorGUILayout.PropertyField(pCenterOfMass);
        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);

        bInertiaTensor = EditorGUILayout.Foldout(bInertiaTensor, "InertiaTensor");
        if(bInertiaTensor) {
            pInertiaTensor1.vector3Value = EditorGUILayout.Vector3Field("1", pInertiaTensor1.vector3Value);
            pInertiaTensor2.vector3Value = EditorGUILayout.Vector3Field("2", pInertiaTensor2.vector3Value);
            pInertiaTensor3.vector3Value = EditorGUILayout.Vector3Field("3", pInertiaTensor3.vector3Value);
        }

        EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(pLinearDamping);
        EditorGUILayout.PropertyField(pAngularDamping);
        EditorGUILayout.PropertyField(pGravityFactor);
        EditorGUILayout.PropertyField(pFriction);
        EditorGUILayout.PropertyField(pRestitution);

        bOpenCollisionFilterInfo = EditorGUILayout.Foldout(bOpenCollisionFilterInfo, "CollisionFilterInfo");
        if(bOpenCollisionFilterInfo) {
            pLayer.intValue = EditorGUILayout.IntPopup("Layer", pLayer.intValue, m_LayerNames, m_LayerValues);
            
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);

            EditorGUILayout.PropertyField(pSystemGroupName);
            EditorGUILayout.PropertyField(pSubSystemId);
            EditorGUILayout.PropertyField(pSubSystemDontCollideWith);

            EditorGUI.EndDisabledGroup();
        }
      
        EditorGUILayout.PropertyField(pCustomLayer);
        if(EditorApplication.isPlaying) EditorGUILayout.LabelField("Ptr: 0x" + instance.GetPtr().ToString("X"));
        
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();

            //Update value at runtime
            if(EditorApplication.isPlaying)
                instance.ApplyModifiedProperties();
        }
    }
}
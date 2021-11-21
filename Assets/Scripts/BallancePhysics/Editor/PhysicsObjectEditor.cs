using System;
using BallancePhysics.Wapper;
using UnityEditor;
using UnityEngine;

namespace BallancePhysics.Editor
{
  [CustomEditor(typeof(PhysicsObject))]
  [CanEditMultipleObjects]
  class PhysicsObjectEditor : UnityEditor.Editor
  {
    private PhysicsObject instance;

    [NonSerialized]
    private string[] m_LayerNames = new string[33];
    [NonSerialized]
    private int[] m_LayerValues = new int[33];

    public PhysicsObjectEditor()
    {
      for (int i = 0; i < 32; i++)
        m_LayerValues[i] = i;
    }

    private void OnEnable()
    {
      m_Mass = serializedObject.FindProperty("m_Mass");
      m_Friction = serializedObject.FindProperty("m_Friction");
      m_Elasticity = serializedObject.FindProperty("m_Elasticity");
      m_LinearSpeedDamping = serializedObject.FindProperty("m_LinearSpeedDamping");
      m_RotSpeedDamping = serializedObject.FindProperty("m_RotSpeedDamping");
      m_BallRadius = serializedObject.FindProperty("m_BallRadius");
      m_UseBall = serializedObject.FindProperty("m_UseBall");
      m_EnableConvexHull = serializedObject.FindProperty("m_EnableConvexHull");
      m_EnableCollision = serializedObject.FindProperty("m_EnableCollision");
      m_StartFrozen = serializedObject.FindProperty("m_StartFrozen");
      m_Fixed = serializedObject.FindProperty("m_Fixed");
      m_ShiftMassCenter = serializedObject.FindProperty("m_ShiftMassCenter");
      m_DoNotAutoCreateAtAwake = serializedObject.FindProperty("m_DoNotAutoCreateAtAwake");
      m_AutoMassCenter = serializedObject.FindProperty("m_AutoMassCenter");
      m_AutoControlActive = serializedObject.FindProperty("m_AutoControlActive");
      m_Layer = serializedObject.FindProperty("m_Layer");
      m_SystemGroupName = serializedObject.FindProperty("m_SystemGroupName");
      m_SubSystemId = serializedObject.FindProperty("m_SubSystemId");
      m_SubSystemDontCollideWith = serializedObject.FindProperty("m_SubSystemDontCollideWith");
      m_Convex = serializedObject.FindProperty("m_Convex");
      m_Concave = serializedObject.FindProperty("m_Concave");
      m_SurfaceName = serializedObject.FindProperty("m_SurfaceName");
      m_UseExistsSurface = serializedObject.FindProperty("m_UseExistsSurface");
      m_ExtraRadius = serializedObject.FindProperty("m_ExtraRadius");
      m_EnableGravity = serializedObject.FindProperty("m_EnableGravity");
      m_EnableCollisionEvent = serializedObject.FindProperty("m_EnableCollisionEvent");
      m_CollisionEventCallSleep = serializedObject.FindProperty("m_CollisionEventCallSleep");
      m_CustomLayer = serializedObject.FindProperty("m_CustomLayer");
      m_StaticConstantForceDirection = serializedObject.FindProperty("m_StaticConstantForceDirection");
      m_StaticConstantForce = serializedObject.FindProperty("m_StaticConstantForce");
      m_ConstantForceDirectionRef = serializedObject.FindProperty("m_ConstantForceDirectionRef");
      m_EnableConstantForce = serializedObject.FindProperty("m_EnableConstantForce");

      bOpenCollisionFilterInfo = EditorPrefs.GetBool("PhysicsObjectEditor_bOpenCollisionFilterInfo", false);
      bOpenConstantForce = EditorPrefs.GetBool("PhysicsObjectEditor_bOpenConstantForce", false);
      bSurface = EditorPrefs.GetBool("PhysicsObjectEditor_bSurface", false);

      var names = AssetDatabase.LoadAssetAtPath<PhysicsLayerNames>("Assets/Resources/BallancePhysicsLayerNames.asset");
      var tags = PhysicsLayerTags.Everything;
      for (int i = 0; i < 32; i++)
        m_LayerNames[i] = i + ": " + (string.IsNullOrEmpty(names.LayerNames[i]) ? "(Undefined layer name)" : names.LayerNames[i]);
      m_LayerNames[32] = "Not set";
      m_LayerValues[32] = -1;
    }
    private void OnDisable()
    {
      EditorPrefs.SetBool("PhysicsObjectEditor_bOpenCollisionFilterInfo", bOpenCollisionFilterInfo);
      EditorPrefs.SetBool("PhysicsObjectEditor_bOpenConstantForce", bOpenConstantForce);
      EditorPrefs.SetBool("PhysicsObjectEditor_bSurface", bSurface);


    }

    private SerializedProperty m_Mass;
    private SerializedProperty m_Friction;
    private SerializedProperty m_Elasticity;
    private SerializedProperty m_LinearSpeedDamping;
    private SerializedProperty m_RotSpeedDamping ;
    private SerializedProperty m_BallRadius;
    private SerializedProperty m_UseBall;
    private SerializedProperty m_EnableConvexHull;
    private SerializedProperty m_EnableCollision;
    private SerializedProperty m_StartFrozen;
    private SerializedProperty m_Fixed;
    private SerializedProperty m_ShiftMassCenter;
    private SerializedProperty m_DoNotAutoCreateAtAwake;
    private SerializedProperty m_AutoMassCenter;
    private SerializedProperty m_AutoControlActive;
    private SerializedProperty m_Layer;
    private SerializedProperty m_SystemGroupName;
    private SerializedProperty m_SubSystemId;
    private SerializedProperty m_SubSystemDontCollideWith;
    private SerializedProperty m_Convex;
    private SerializedProperty m_Concave;
    private SerializedProperty m_SurfaceName;
    private SerializedProperty m_UseExistsSurface ;
    private SerializedProperty m_ExtraRadius;
    private SerializedProperty m_EnableGravity;
    private SerializedProperty m_EnableCollisionEvent;
    private SerializedProperty m_CollisionEventCallSleep;
    private SerializedProperty m_CustomLayer;
    private SerializedProperty m_StaticConstantForce;
    private SerializedProperty m_StaticConstantForceDirection;
    private SerializedProperty m_ConstantForceDirectionRef;
    private SerializedProperty m_EnableConstantForce ;

    private bool bOpenCollisionFilterInfo = false;
    private bool bOpenConstantForce = false;
    private bool bSurface = false;

    public override void OnInspectorGUI()
    {
      serializedObject.Update();
      instance = (PhysicsObject)target;

      EditorGUI.BeginChangeCheck();

      if (EditorApplication.isPlaying)
        EditorGUILayout.HelpBox("Some values can't change at runtime.", MessageType.Warning);

      EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);

      EditorGUILayout.PropertyField(m_Fixed);
      EditorGUILayout.PropertyField(m_StartFrozen);
      EditorGUILayout.PropertyField(m_EnableCollision);
      EditorGUILayout.PropertyField(m_Mass);
      EditorGUILayout.PropertyField(m_Friction);
      EditorGUILayout.PropertyField(m_Elasticity);
      EditorGUILayout.PropertyField(m_LinearSpeedDamping);
      EditorGUILayout.PropertyField(m_RotSpeedDamping);

      bSurface = EditorGUILayout.Foldout(bSurface, "Collision Surface");
      if (bSurface)
      {
        EditorGUI.indentLevel++;
        
        EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit Collider"), base.target);
        GUILayout.Space(5f);
        EditorGUILayout.PropertyField(m_UseBall);
        if(m_UseBall.boolValue) { 
          EditorGUILayout.PropertyField(m_BallRadius);
        } else {
          EditorGUILayout.PropertyField(m_UseExistsSurface);
          EditorGUILayout.PropertyField(m_Convex);
          EditorGUILayout.PropertyField(m_Concave);
          EditorGUILayout.PropertyField(m_SurfaceName);
          EditorGUILayout.PropertyField(m_EnableConvexHull);
        }
        EditorGUILayout.PropertyField(m_ExtraRadius);
        EditorGUILayout.PropertyField(m_AutoMassCenter);
        if(!m_AutoMassCenter.boolValue)
          EditorGUILayout.PropertyField(m_ShiftMassCenter);
        EditorGUI.indentLevel--;
      }

      EditorGUILayout.PropertyField(m_DoNotAutoCreateAtAwake);
      EditorGUILayout.PropertyField(m_AutoControlActive);

      EditorGUI.EndDisabledGroup();

      bOpenCollisionFilterInfo = EditorGUILayout.Foldout(bOpenCollisionFilterInfo, "Collision Filter Info");
      if (bOpenCollisionFilterInfo)
      {
        EditorGUI.indentLevel++;
        m_Layer.intValue = EditorGUILayout.IntPopup("Layer", m_Layer.intValue, m_LayerNames, m_LayerValues);

        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);

        EditorGUILayout.PropertyField(m_SystemGroupName);
        EditorGUILayout.PropertyField(m_SubSystemId);
        EditorGUILayout.PropertyField(m_SubSystemDontCollideWith);

        EditorGUI.EndDisabledGroup();
        EditorGUI.indentLevel--;
      }

      EditorGUILayout.PropertyField(m_CustomLayer);
      EditorGUILayout.PropertyField(m_EnableCollisionEvent);
      if(m_EnableCollisionEvent.boolValue) {
        EditorGUILayout.PropertyField(m_CollisionEventCallSleep);
      }

      bOpenConstantForce = EditorGUILayout.Foldout(bOpenConstantForce, "Constant Force");
      if (bOpenConstantForce)
      {
        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(m_EnableConstantForce);
        EditorGUILayout.PropertyField(m_StaticConstantForceDirection);
        EditorGUILayout.PropertyField(m_StaticConstantForce);
        EditorGUILayout.PropertyField(m_ConstantForceDirectionRef);

        EditorGUI.indentLevel--;
      }

      if (EditorApplication.isPlaying) EditorGUILayout.LabelField("Handle: 0x" + instance.Handle.ToString("X"));

      if (EditorGUI.EndChangeCheck())
        serializedObject.ApplyModifiedProperties();
    }
  }
}
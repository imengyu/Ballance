using PhysicsRT;
using UnityEditor;

[CustomEditor(typeof(MotorConstraint), true)]
[CanEditMultipleObjects]
class PhysicsMotorConstraintEditor : Editor
{
    private MotorConstraint instance;
    
    public PhysicsMotorConstraintEditor() {
    }

    private void OnEnable() {
        motorEnable = serializedObject.FindProperty("motorEnable");
        motorSpring = serializedObject.FindProperty("motorSpring");
        motorTau = serializedObject.FindProperty("motorTau");
        motorDamping = serializedObject.FindProperty("motorDamping");
        motorProportionalRecoveryVelocity = serializedObject.FindProperty("motorProportionalRecoveryVelocity");
        motorConstantRecoveryVelocity = serializedObject.FindProperty("motorConstantRecoveryVelocity");
        motorMinForce = serializedObject.FindProperty("motorMinForce");
        motorMaxForce = serializedObject.FindProperty("motorMaxForce");
        motorSpringConstant = serializedObject.FindProperty("motorSpringConstant");
        motorSpringDamping = serializedObject.FindProperty("motorSpringDamping");

        pBreakable = serializedObject.FindProperty("m_Breakable");
        pThreshold = serializedObject.FindProperty("m_Threshold");
        pMaximumAngularImpulse = serializedObject.FindProperty("m_MaximumAngularImpulse");
        pMaximumLinearImpulse = serializedObject.FindProperty("m_MaximumLinearImpulse");
    }
    private void OnDisable() {

    }

    private SerializedProperty motorEnable;
    private SerializedProperty motorSpring;
    private SerializedProperty motorTau;
    private SerializedProperty motorDamping;
    private SerializedProperty motorProportionalRecoveryVelocity;
    private SerializedProperty motorConstantRecoveryVelocity;
    private SerializedProperty motorMinForce;
    private SerializedProperty motorMaxForce;
    private SerializedProperty motorSpringDamping;
    private SerializedProperty motorSpringConstant;

    private SerializedProperty pBreakable;
    private SerializedProperty pThreshold;
    private SerializedProperty pMaximumAngularImpulse;
    private SerializedProperty pMaximumLinearImpulse;

    private bool bMotorFoldout = false;
    private bool bBreakableFoldout = false;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        instance = (MotorConstraint)target;

        EditorGUI.BeginChangeCheck();

        if(EditorApplication.isPlaying)
            EditorGUILayout.HelpBox("Constraint values can't change at runtime.", MessageType.Warning);

        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
        DrawDefaultInspector();

        EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit"), base.target);

        EditorGUI.EndDisabledGroup();

        bBreakableFoldout = EditorGUILayout.Foldout(bBreakableFoldout, "Breakable");
        if(bBreakableFoldout) {        
            
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);

            EditorGUILayout.PropertyField(pBreakable);
            EditorGUILayout.PropertyField(pThreshold);
            EditorGUILayout.PropertyField(pMaximumAngularImpulse);
            EditorGUILayout.PropertyField(pMaximumLinearImpulse);
        
            EditorGUI.EndDisabledGroup();
        }

        bMotorFoldout = EditorGUILayout.Foldout(bMotorFoldout, "Motor");
        if(bMotorFoldout) {    
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying); 

            EditorGUILayout.PropertyField(motorEnable);
            EditorGUILayout.PropertyField(motorSpring);
            EditorGUILayout.PropertyField(motorTau);
            EditorGUILayout.PropertyField(motorDamping);
            EditorGUILayout.PropertyField(motorProportionalRecoveryVelocity);
            EditorGUILayout.PropertyField(motorConstantRecoveryVelocity);
            EditorGUILayout.PropertyField(motorMinForce);
            EditorGUILayout.PropertyField(motorMaxForce);
            EditorGUILayout.PropertyField(motorSpringDamping);
            EditorGUILayout.PropertyField(motorSpringConstant);

            EditorGUI.EndDisabledGroup();
        }

        
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
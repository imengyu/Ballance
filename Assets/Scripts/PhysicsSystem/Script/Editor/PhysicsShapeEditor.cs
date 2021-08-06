using System;
using System.Collections.Generic;
using PhysicsRT;
using Unity.Mathematics;
using Unity.Physics.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PhysicsShape))]
[CanEditMultipleObjects]
class PhysicsShapeEditor : Editor
{
    private PhysicsShape instance;
    
    [NonSerialized]
    FitToRenderMeshesDropDown m_DropDown;

    private void OnEnable() {
        bDrawMaterialInspector = EditorPrefs.GetBool("PhysicsShapeEditor_bDrawMaterialInspector", false);

        pShapeType = serializedObject.FindProperty("m_ShapeType");
        pWrap = serializedObject.FindProperty("m_Wrap");
        pTranslation = serializedObject.FindProperty("m_Translation");
        pRotation = serializedObject.FindProperty("m_Rotation");
        pScale = serializedObject.FindProperty("m_Scale");
        pShapeMesh = serializedObject.FindProperty("m_ShapeMesh");
        pShapeSize = serializedObject.FindProperty("m_ShapeSize");
        pShapeRadius = serializedObject.FindProperty("m_ShapeRadius");
        pShapeHeight = serializedObject.FindProperty("m_ShapeHeight");
        pShapeConvexRadius = serializedObject.FindProperty("m_ShapeConvexRadius");
        pShapeSideCount = serializedObject.FindProperty("m_ShapeSideCount");
        pMinimumSkinnedVertexWeight = serializedObject.FindProperty("m_MinimumSkinnedVertexWeight");
        pCustomMaterialTags = serializedObject.FindProperty("m_CustomMaterialTags");
    }
    private void OnDisable() {
        EditorPrefs.SetBool("PhysicsShapeEditor_bDrawMaterialInspector", bDrawMaterialInspector);

        if (m_DropDown != null)
            m_DropDown.CloseWithoutUndo();
    }

    private bool bDrawMaterialInspector = false;

    private SerializedProperty pShapeType;
    private SerializedProperty pWrap;
    private SerializedProperty pTranslation;
    private SerializedProperty pRotation;
    private SerializedProperty pScale;
    private SerializedProperty pShapeMesh;
    private SerializedProperty pShapeSize;
    private SerializedProperty pShapeRadius;
    private SerializedProperty pShapeHeight;
    private SerializedProperty pShapeConvexRadius;
    private SerializedProperty pShapeSideCount;
    private SerializedProperty pMinimumSkinnedVertexWeight;
    private SerializedProperty pCustomMaterialTags;
    private SerializedProperty pLayer;

    private static class Styles
    {
        const string k_Plural = "One or more selected objects";
        const string k_Singular = "This object";

        public static readonly GUIStyle Button = new GUIStyle(EditorStyles.miniButton) { padding = new RectOffset() };
        public static readonly GUIStyle ButtonDropDown = new GUIStyle(EditorStyles.popup) { alignment = TextAnchor.MiddleCenter };
        static readonly GUIContent k_FitToRenderMeshesLabel =
            EditorGUIUtility.TrTextContent("Fit to Enabled Render Meshes");
        static readonly GUIContent k_FitToRenderMeshesWarningLabelSg = new GUIContent(
            k_FitToRenderMeshesLabel.text,
            EditorGUIUtility.Load("console.warnicon") as Texture,
            L10n.Tr($"{k_Singular} has non-uniform scale. Trying to fit the shape to render meshes might produce unexpected results.")
        );
        static readonly GUIContent k_FitToRenderMeshesWarningLabelPl = new GUIContent(
            k_FitToRenderMeshesLabel.text,
            EditorGUIUtility.Load("console.warnicon") as Texture,
            L10n.Tr($"{k_Plural} has non-uniform scale. Trying to fit the shape to render meshes might produce unexpected results.")
        );

        public static GUIContent GetFitToRenderMeshesLabel(int numTargets, MessageType status) =>
            status >= MessageType.Warning
                ? numTargets == 1 ? k_FitToRenderMeshesWarningLabelSg : k_FitToRenderMeshesWarningLabelPl
                : k_FitToRenderMeshesLabel;
    }
    private class FitToRenderMeshesDropDown : EditorWindow
    {
        static class Styles
        {
            public const float WindowWidth = 400f;
            public const float LabelWidth = 200f;
            public static GUIStyle Button => PhysicsShapeEditor.Styles.Button;
        }

        static class Content
        {
            public static readonly string ApplyLabel = L10n.Tr("Apply");
            public static readonly string CancelLabel = L10n.Tr("Cancel");
        }

        bool m_ApplyChanges;
        bool m_ClosedWithoutUndo;
        int m_UndoGroup;
        SerializedProperty m_MinimumSkinnedVertexWeight;

        public static FitToRenderMeshesDropDown Show(Rect buttonRect, string title, SerializedProperty minimumSkinnedVertexWeight)
        {
            var window = CreateInstance<FitToRenderMeshesDropDown>();
            window.titleContent = EditorGUIUtility.TrTextContent(title);
            window.m_UndoGroup = Undo.GetCurrentGroup();
            window.m_MinimumSkinnedVertexWeight = minimumSkinnedVertexWeight;
            var size = new Vector2(
                math.max(buttonRect.width, Styles.WindowWidth),
                (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3f
            );
            window.maxSize = window.minSize = size;
            window.ShowAsDropDown(GUIUtility.GUIToScreenRect(buttonRect), size);
            return window;
        }

        void OnGUI()
        {
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = Styles.LabelWidth;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_MinimumSkinnedVertexWeight);
            if (EditorGUI.EndChangeCheck())
                ApplyChanges();

            EditorGUIUtility.labelWidth = labelWidth;

            GUILayout.FlexibleSpace();

            var buttonRect = GUILayoutUtility.GetRect(0f, EditorGUIUtility.singleLineHeight);

            var buttonLeft = new Rect(buttonRect)
            {
                width = 0.5f * (buttonRect.width - EditorGUIUtility.standardVerticalSpacing)
            };
            var buttonRight = new Rect(buttonLeft)
            {
                x = buttonLeft.xMax + EditorGUIUtility.standardVerticalSpacing
            };

            var close = false;

            buttonRect = Application.platform == RuntimePlatform.OSXEditor ? buttonLeft : buttonRight;
            if (
                GUI.Button(buttonRect, Content.CancelLabel, Styles.Button)
                || Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape
            )
            {
                close = true;
            }

            buttonRect = Application.platform == RuntimePlatform.OSXEditor ? buttonRight : buttonLeft;
            if (GUI.Button(buttonRect, Content.ApplyLabel, Styles.Button))
            {
                close = true;
                m_ApplyChanges = true;
            }

            if (close)
            {
                Close();
                EditorGUIUtility.ExitGUI();
            }
        }

        void ApplyChanges()
        {
            m_MinimumSkinnedVertexWeight.serializedObject.ApplyModifiedProperties();
            Undo.RecordObjects(m_MinimumSkinnedVertexWeight.serializedObject.targetObjects, titleContent.text);
            foreach (PhysicsShape shape in m_MinimumSkinnedVertexWeight.serializedObject.targetObjects)
            {
                using (var so = new SerializedObject(shape))
                {
                    shape.FitToEnabledRenderMeshes(
                        so.FindProperty(m_MinimumSkinnedVertexWeight.propertyPath).floatValue
                    );
                    EditorUtility.SetDirty(shape);
                }
            }
            m_MinimumSkinnedVertexWeight.serializedObject.Update();
        }

        public void CloseWithoutUndo()
        {
            m_ApplyChanges = true;
            Close();
        }

        void OnDestroy()
        {
            if (m_ApplyChanges)
                ApplyChanges();
            else
                Undo.RevertAllDownToGroup(m_UndoGroup);
        }
    }
    
    public override void OnInspectorGUI()
    {
        UpdateGeometryState();

        serializedObject.Update();

        UpdateStatusMessages();

        instance = (PhysicsShape)target;

        EditorGUI.BeginChangeCheck();

        serializedObject.Update();

        if (instance.ShapeMesh == null)
        {
            var m = instance.GetComponent<MeshFilter>();
            if (m != null) instance.ShapeMesh = m.sharedMesh;
        }

        EditorGUILayout.PropertyField(pShapeType);
        DrawShapeInspector();
        EditorGUILayout.PropertyField(pWrap);
        DrawWraplInspector();

        bDrawMaterialInspector = EditorGUILayout.Foldout(bDrawMaterialInspector, "Material", true);
        if (bDrawMaterialInspector)
            DrawMaterialInspector();

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    private MessageType m_MatrixStatus;
    private List<MatrixState> m_MatrixStates = new List<MatrixState>();

    private void UpdateStatusMessages() {
        m_MatrixStates.Clear();
        foreach (var t in targets)
        {
            var localToWorld = (float4x4)(t as Component).transform.localToWorldMatrix;
            m_MatrixStates.Add(ManipulatorUtility.GetMatrixState(ref localToWorld));
        }
        m_MatrixStatus = StatusMessageUtility.GetMatrixStatusMessage(m_MatrixStates, out var matrixStatusMessage);
    }
    private void UpdateGeometryState()
    {
    }
    private void AutomaticPrimitiveControls()
    {
        EditorGUI.BeginDisabledGroup(
            ((ShapeType)pShapeType.enumValueIndex >= ShapeType.ConvexHull && pShapeMesh.objectReferenceValue == null) || EditorUtility.IsPersistent(target)
        );

        var buttonLabel = Styles.GetFitToRenderMeshesLabel(targets.Length, m_MatrixStatus);

        var rect = EditorGUI.IndentedRect(
            EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, EditorStyles.miniButton)
        );

        if (GUI.Button(rect, buttonLabel, Styles.ButtonDropDown))
            m_DropDown = FitToRenderMeshesDropDown.Show(rect, buttonLabel.text, pMinimumSkinnedVertexWeight);

        EditorGUI.EndDisabledGroup();
    }

    private void DrawWraplInspector() {
        switch((ShapeWrap)pWrap.enumValueIndex) {
            case ShapeWrap.None:
                break;
            case ShapeWrap.TransformShape:
                EditorGUILayout.PropertyField(pTranslation);
                EditorGUILayout.PropertyField(pRotation);

                var shapeType = (ShapeType)pShapeType.enumValueIndex;
                if(shapeType == ShapeType.Box || shapeType == ShapeType.ConvexHull)
                    EditorGUILayout.PropertyField(pScale);

                break;
            case ShapeWrap.TranslateShape:
                EditorGUILayout.PropertyField(pTranslation);
                break;
        }
    }
    private void DrawShapeInspector() {

        EditorGUILayout.EditorToolbarForTarget(EditorGUIUtility.TrTempContent("Edit Collider"), base.target);
        GUILayout.Space(5f);
        
        switch ((ShapeType)pShapeType.enumValueIndex)
        {
            case ShapeType.Box:
            case ShapeType.Sphere:
            case ShapeType.Capsule:
            case ShapeType.Cylinder:
            case ShapeType.Plane:
                AutomaticPrimitiveControls();
                break;
        }

        switch((ShapeType)pShapeType.enumValueIndex) {
            case ShapeType.Box:
                pShapeSize.vector3Value = EditorGUILayout.Vector3Field("Half extent", pShapeSize.vector3Value);
                pShapeConvexRadius.floatValue = EditorGUILayout.FloatField("Convex radius", pShapeConvexRadius.floatValue);
                break;
            case ShapeType.Sphere:
                pShapeRadius.floatValue = EditorGUILayout.FloatField("Radius", pShapeRadius.floatValue);
                break;
            case ShapeType.Capsule:
                pShapeHeight.floatValue = EditorGUILayout.FloatField("Height", pShapeHeight.floatValue);
                pShapeRadius.floatValue = EditorGUILayout.FloatField("Radius", pShapeRadius.floatValue);
                break;
            case ShapeType.Cylinder:
                pShapeHeight.floatValue = EditorGUILayout.FloatField("Height", pShapeHeight.floatValue);
                pShapeRadius.floatValue = EditorGUILayout.FloatField("Radius", pShapeRadius.floatValue);
                pShapeConvexRadius.floatValue = EditorGUILayout.FloatField("Convex radius", pShapeConvexRadius.floatValue);
                break;
            case ShapeType.Plane:
                pShapeSize.vector3Value = new Vector3(EditorGUILayout.FloatField("Width", pShapeSize.vector3Value.x), pShapeSize.vector3Value.y, EditorGUILayout.FloatField("Height", pShapeSize.vector3Value.z));
                break;
            case ShapeType.ConvexHull:
            case ShapeType.Mesh:
                pShapeMesh.objectReferenceValue = EditorGUILayout.ObjectField("Mesh", pShapeMesh.objectReferenceValue, typeof(Mesh), false);
                pShapeConvexRadius.floatValue = EditorGUILayout.FloatField("Convex radius", pShapeConvexRadius.floatValue);
                break;
        }
    }
    private void DrawMaterialInspector() {
        EditorGUILayout.PropertyField(pCustomMaterialTags);
    }
}
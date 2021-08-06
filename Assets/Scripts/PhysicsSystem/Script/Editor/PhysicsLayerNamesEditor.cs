using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(PhysicsLayerNames))]
[CanEditMultipleObjects]
class PhysicsLayerNamesEditor : BaseEditor
{
#pragma warning disable 649
    [AutoPopulate(ElementFormatString = "Layer {0}", Resizable = false, Reorderable = false)]
    ReorderableList m_LayerNames;
#pragma warning restore 649

    private bool bGroupFilter = false;
    private Vector2 scrollPos = Vector2.zero;
    private int textGroupFilterWidth = 150;
    private int textGroupFilterHeight = 20;

    private SerializedProperty pGroupFilter;

    protected override void OnEnable() {
        base.OnEnable();
        pGroupFilter = serializedObject.FindProperty("m_GroupFilter");
    }

    protected override void DrawCustomGUI() {

        bGroupFilter = EditorGUILayout.Foldout(bGroupFilter, "GroupFilter");
        if(bGroupFilter) {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, true);

            var tagNames = ((PhysicsLayerNames)target).LayerNames;
            var rectLeft = new Rect(0, textGroupFilterWidth + 10, textGroupFilterWidth, textGroupFilterHeight);
            int showCount = 0;

            var rectTopText = new Rect(0, textGroupFilterWidth + 10, textGroupFilterWidth, textGroupFilterHeight);
            var oldMatrix = GUI.matrix;  
            GUIUtility.RotateAroundPivot(-90, new Vector2(textGroupFilterWidth / 2, textGroupFilterWidth / 2));
            for(int j = 32 - 1; j >= 0; j--) {

                if(string.IsNullOrEmpty(tagNames[j]))
                    continue;

                GUI.Label(rectTopText, tagNames[j]);
                rectTopText.y += textGroupFilterHeight;
                showCount++;
            }
            GUI.matrix = oldMatrix;

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);

            for(int i = 0, ir = 0; i < 32; i++) {
                var arrObj = pGroupFilter.GetArrayElementAtIndex(i);
                var arr = arrObj.FindPropertyRelative("m_GroupFilter");
                if(string.IsNullOrEmpty(tagNames[i]))
                    continue;

                GUI.Label(rectLeft, tagNames[i]);

                var rectTop = new Rect(textGroupFilterWidth + 10, rectLeft.y, textGroupFilterHeight, textGroupFilterHeight);

                for(int j = 0, jr = 0; j < 32 && jr < showCount - ir; j++) { 

                    if(string.IsNullOrEmpty(tagNames[31 - j]))
                        continue;

                    var item = arr.GetArrayElementAtIndex(31 - j);
                    item.boolValue = GUI.Toggle(rectTop, item.boolValue, "");
                    rectTop.x += textGroupFilterHeight;
                    jr++;
                }
                
                rectLeft.y += textGroupFilterHeight;
                ir++;
            }
            EditorGUI.EndDisabledGroup();

            if(EditorApplication.isPlaying)
                EditorGUILayout.HelpBox("Can't change GroupFilter at runtime.", MessageType.Warning);

            if(showCount == 0) 
                EditorGUILayout.HelpBox("You have not set the name of any layer. Please set the layer name first.", MessageType.Info);
            else
                EditorGUILayout.Space(512);

            EditorGUILayout.EndScrollView();
        }


    } 
}
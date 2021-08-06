using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(CustomPhysicsBodyTagNames))]
[CanEditMultipleObjects]
class CustomPhysicsBodyTagNamesEditor : BaseEditor
{
    #pragma warning disable 649
    [AutoPopulate(ElementFormatString = "Custom Physics Body Tag {0}", Resizable = false, Reorderable = false)]
    ReorderableList m_TagNames;
#pragma warning restore 649

}

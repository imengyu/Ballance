using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(CustomPhysicsMaterialTagNames))]
[CanEditMultipleObjects]
class CustomPhysicsMaterialTagNamesEditor : BaseEditor
{
#pragma warning disable 649
    [AutoPopulate(ElementFormatString = "Custom Physics Material Tag {0}", Resizable = false, Reorderable = false)]
    ReorderableList m_TagNames;
#pragma warning restore 649

}
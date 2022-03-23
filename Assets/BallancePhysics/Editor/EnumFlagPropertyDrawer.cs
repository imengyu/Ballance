using UnityEditor;
using UnityEngine;

namespace BallancePhysics.Editor {

  [CustomPropertyDrawer(typeof(EnumFlagPropertyAttribute))]
  public class EnumFlagPropertyDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
    }
  }
}
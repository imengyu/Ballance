using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
sealed class AutoPopulateAttribute : Attribute
{
    public string PropertyPath { get; set; }
    public string ElementFormatString { get; set; }
    public bool Reorderable { get; set; } = true;
    public bool Resizable { get; set; } = true;
}

abstract class BaseEditor : UnityEditor.Editor
{
    static class Content
    {
        public static readonly string UnableToLocateFormatString = L10n.Tr("Cannot find SerializedProperty {0}");
    }

    List<Action> m_AutoFieldGUIControls = new List<Action>();

    protected virtual void OnEnable()
    {
        const BindingFlags bindingFlags =
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
        var autoFields = GetType().GetFields(bindingFlags)
            .Where(f => Attribute.IsDefined(f, typeof(AutoPopulateAttribute)))
            .ToArray();

        foreach (var field in autoFields)
        {
            var attr =
                field.GetCustomAttributes(typeof(AutoPopulateAttribute)).Single() as AutoPopulateAttribute;

            var sp = serializedObject.FindProperty(attr.PropertyPath ?? field.Name);

            if (sp == null)
            {
                var message = string.Format(Content.UnableToLocateFormatString, field.Name);
                m_AutoFieldGUIControls.Add(() => EditorGUILayout.HelpBox(message, MessageType.Error));
                Debug.LogError(message);
                continue;
            }

            if (field.FieldType == typeof(SerializedProperty))
            {
                field.SetValue(this, sp);
                m_AutoFieldGUIControls.Add(() => EditorGUILayout.PropertyField(sp, true));
            }
            else if (field.FieldType == typeof(ReorderableList))
            {
                var list = new ReorderableList(serializedObject, sp);

                var label = EditorGUIUtility.TrTextContent(sp.displayName);
                list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, label);

                list.elementHeightCallback = index =>
                {
                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    return EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
                };

                var formatString = attr.ElementFormatString;
                if (formatString == null)
                {
                    list.drawElementCallback = (rect, index, active, focused) =>
                    {
                        var element = list.serializedProperty.GetArrayElementAtIndex(index);
                        EditorGUI.PropertyField(
                            new Rect(rect) { height = EditorGUI.GetPropertyHeight(element) }, element, true
                        );
                    };
                }
                else
                {
                    var noLabel = formatString == string.Empty;
                    if (!noLabel)
                        formatString = L10n.Tr(formatString);
                    var elementLabel = new GUIContent();
                    list.drawElementCallback = (rect, index, active, focused) =>
                    {
                        var element = list.serializedProperty.GetArrayElementAtIndex(index);
                        if (!noLabel)
                            elementLabel.text = string.Format(formatString, index);
                        EditorGUI.PropertyField(
                            new Rect(rect) { height = EditorGUI.GetPropertyHeight(element) },
                            element,
                            noLabel ? GUIContent.none : elementLabel,
                            true
                        );
                    };
                }

                list.draggable = attr.Reorderable;
                list.displayAdd = list.displayRemove = attr.Resizable;

                field.SetValue(this, list);
                m_AutoFieldGUIControls.Add(() => list.DoLayoutList());
            }
        }
    }
    protected virtual void DrawCustomGUI() {

    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        foreach (var guiControl in m_AutoFieldGUIControls)
            guiControl();
            
        DrawCustomGUI();

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}
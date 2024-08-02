using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using System.Linq;
using AYellowpaper.SerializedCollections.Editor.Data;
using UnityEngine;
using System.Collections;

namespace AYellowpaper.SerializedCollections.Editor
{
    internal static class SCEditorUtility
    {
        public const string EditorPrefsPrefix = "SC_";
        public const bool KeyFlag = true;
        public const bool ValueFlag = false;

        public static float CalculateHeight(SerializedProperty property, DisplayType displayType)
        {
            return CalculateHeight(property, displayType == DisplayType.List ? true : false);
        }

        public static float CalculateHeight(SerializedProperty property, bool drawAsList)
        {
            if (drawAsList)
            {
                float height = 0;
                foreach (SerializedProperty child in GetChildren(property))
                    height += EditorGUI.GetPropertyHeight(child, true);
                return height;
            }

            return EditorGUI.GetPropertyHeight(property, true);
        }

        public static IEnumerable<SerializedProperty> GetChildren(SerializedProperty property, bool recursive = false)
        {
            if (!property.hasVisibleChildren)
            {
                yield return property;
                yield break;
            }

            SerializedProperty end = property.GetEndProperty();
            property.NextVisible(true);
            do
            {
                yield return property;
            } while (property.NextVisible(recursive) && !SerializedProperty.EqualContents(property, end));
        }

        public static PropertyData GetPropertyData(SerializedProperty property)
        {
            var data = new PropertyData();
            var json = EditorPrefs.GetString(EditorPrefsPrefix + property.propertyPath, null);
            if (json != null)
                EditorJsonUtility.FromJsonOverwrite(json, data);
            return data;
        }

        public static void SavePropertyData(SerializedProperty property, PropertyData propertyData)
        {
            var json = EditorJsonUtility.ToJson(propertyData);
            EditorPrefs.SetString(EditorPrefsPrefix + property.propertyPath, json);
        }

        public static bool ShouldShowSearch(int pages)
        {
            var settings = EditorUserSettings.Get();
            return settings.AlwaysShowSearch ? true : pages >= settings.PageCountForSearch;
        }

        public static bool HasDrawerForProperty(SerializedProperty property, Type type)
        {
            Type attributeUtilityType = typeof(SerializedProperty).Assembly.GetType("UnityEditor.ScriptAttributeUtility");
            if (attributeUtilityType == null)
                return false;
            var getDrawerMethod = attributeUtilityType.GetMethod("GetDrawerTypeForPropertyAndType", BindingFlags.Static | BindingFlags.NonPublic);
            if (getDrawerMethod == null)
                return false;
            return getDrawerMethod.Invoke(null, new object[] { property, type }) != null;
        }

        internal static void AddGenericMenuItem(GenericMenu genericMenu, bool isOn, bool isEnabled, GUIContent content, GenericMenu.MenuFunction action)
        {
            if (isEnabled)
                genericMenu.AddItem(content, isOn, action);
            else
                genericMenu.AddDisabledItem(content);
        }

        internal static void AddGenericMenuItem(GenericMenu genericMenu, bool isOn, bool isEnabled, GUIContent content, GenericMenu.MenuFunction2 action, object userData)
        {
            if (isEnabled)
                genericMenu.AddItem(content, isOn, action, userData);
            else
                genericMenu.AddDisabledItem(content);
        }

        internal static bool TryGetTypeFromProperty(SerializedProperty property, out Type type)
        {
            try
            {
                var classType = typeof(EditorGUI).Assembly.GetType("UnityEditor.ScriptAttributeUtility");
                var methodInfo = classType.GetMethod("GetFieldInfoFromProperty", BindingFlags.Static | BindingFlags.NonPublic);
                var parameters = new object[] { property, null };
                methodInfo.Invoke(null, parameters);
                type = (Type) parameters[1];
                return true;
            }
            catch
            {
                type = null;
                return false;
            }
        }
        
        internal static float DoHorizontalScale(Rect rect, float value)
        {
            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            var isMovingMouse = Event.current.type == EventType.MouseDrag;
            DoButtonControl(rect, controlId, false, false, GUIContent.none, GUIStyle.none);
            
            if (controlId == GUIUtility.hotControl && isMovingMouse)
            {
                value += Event.current.delta.x;
                GUI.changed = true;
            }
            
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);

            return value;
        }
        
        internal static bool DoButtonControl(Rect rect, int id, bool on, bool hover, GUIContent content, GUIStyle style)
        {
            Event current = Event.current;
            switch (current.type)
            {
                case EventType.MouseDown:
                    if (HitTest(rect, current.mousePosition))
                    {
                        GUIUtility.hotControl = id;
                        current.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == id)
                    {
                        GUIUtility.hotControl = 0;
                        current.Use();
                        if (HitTest(rect, current.mousePosition))
                        {
                            GUI.changed = true;
                            return !on;
                        }
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == id)
                    {
                        current.Use();
                    }
                    break;
                case EventType.KeyDown:
                    bool flag = current.alt || current.shift || current.command || current.control;
                    if ((current.keyCode == KeyCode.Space || current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter) && !flag && GUIUtility.keyboardControl == id)
                    {
                        current.Use();
                        GUI.changed = true;
                        return !on;
                    }
                    break;
                case EventType.Repaint:
                    style.Draw(rect, content, id, on, hover);
                    break;
            }
            return on;
        }

        internal static bool HitTest(Rect rect, Vector2 point) => point.x >= rect.xMin && point.x < rect.xMax && point.y >= rect.yMin && point.y < rect.yMax;


        public static object GetPropertyValue(SerializedProperty prop, object target)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            var elements = path.Split('.');
            foreach (var element in elements.Take(elements.Length - 1))
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    target = GetValue(target, elementName, index);
                }
                else
                {
                    target = GetValue(target, element);
                }
            }
            return target;
        }

        public static object GetValue(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();
            var f = type.GetFieldRecursive(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (f == null)
            {
                var p = type.GetPropertyRecursive(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p == null)
                    return null;
                return p.GetValue(source, null);
            }
            return f.GetValue(source);
        }

        public static object GetValue(object source, string name, int index)
        {
            var enumerable = GetValue(source, name) as IEnumerable;
            var enm = enumerable.GetEnumerator();
            while (index-- >= 0)
                enm.MoveNext();
            return enm.Current;
        }

        private static FieldInfo GetFieldRecursive(this Type type, string name, BindingFlags bindingFlags)
        {
            var fieldInfo = type.GetField(name, bindingFlags);
            if (fieldInfo == null && type.BaseType != null)
                return type.BaseType.GetFieldRecursive(name, bindingFlags);
            return fieldInfo;
        }
        
        private static PropertyInfo GetPropertyRecursive(this Type type, string name, BindingFlags bindingFlags)
        {
            var propertyInfo = type.GetProperty(name, bindingFlags);
            if (propertyInfo == null && type.BaseType != null)
                return type.BaseType.GetPropertyRecursive(name, bindingFlags);
            return propertyInfo;
        }
    }
}
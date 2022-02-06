using Ballance2.UI.Core;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Window), true)]
class WindowInspector : Editor
{
    private Window myScript;
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        myScript = (Window)target;

        EditorGUI.BeginChangeCheck();

        bool newState = EditorGUILayout.Toggle("CanClose", myScript.CanClose);
        if (newState != myScript.CanClose)
            myScript.CanClose = newState;
        newState = EditorGUILayout.Toggle("CanDrag", myScript.CanDrag);
        if (newState != myScript.CanDrag)
            myScript.CanDrag = newState;
        newState = EditorGUILayout.Toggle("CanMax", myScript.CanMax);
        if (newState != myScript.CanMax)
            myScript.CanMax = newState;
        newState = EditorGUILayout.Toggle("CanMin", myScript.CanMin);
        if (newState != myScript.CanMin)
            myScript.CanMin = newState;
        newState = EditorGUILayout.Toggle("CanResize", myScript.CanResize);
        if (newState != myScript.CanResize)
            myScript.CanResize = newState;
        newState = EditorGUILayout.Toggle("CloseAsHide", myScript.CloseAsHide);
        if (newState != myScript.CloseAsHide)
            myScript.CloseAsHide = newState;
        string newText = EditorGUILayout.TextField("Title", myScript.Title);
        if (newText != myScript.Title)
            myScript.Title = newText;
        Sprite newIcon = (Sprite)EditorGUILayout.ObjectField("Icon", myScript.Icon, typeof(Sprite), false);
        if (newIcon != myScript.Icon)
            myScript.Icon = newIcon;

        if (EditorApplication.isPlaying)
        {
            Vector2 newV = EditorGUILayout.Vector2Field("Position", myScript.Position);
            if (newV != myScript.Position)
                myScript.Position = newV;
            newV = EditorGUILayout.Vector2Field("Size", myScript.Size);
            if (newV != myScript.Size)
                myScript.Size = newV;
        }


        Vector2 newVec2 = EditorGUILayout.Vector2Field("MinSize", myScript.MinSize);
        if (newVec2 != myScript.MinSize)
            myScript.MinSize = newVec2;

        if (EditorApplication.isPlaying)
        {
            WindowState newWindowState = (WindowState)EditorGUILayout.EnumPopup("WindowState", myScript.WindowState);
            if (newWindowState != myScript.WindowState)
                myScript.WindowState = newWindowState;
            WindowType newWindowType = (WindowType)EditorGUILayout.EnumPopup("WindowType", myScript.WindowType);
            if (newWindowType != myScript.WindowType)
                myScript.WindowType = newWindowType;
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        base.OnInspectorGUI();
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }

}


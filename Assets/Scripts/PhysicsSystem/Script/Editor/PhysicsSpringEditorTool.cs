using PhysicsRT;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[EditorTool("PhysicsSpringEditorTool", typeof(PhysicsSpring))]
public class PhysicsSpringEditorTool : EditorTool
{
  protected static class Styles
	{
		public static readonly string editAngularLimitsUndoMessage = L10n.Tr("Show povit");
	}

	public override GUIContent toolbarIcon => EditorGUIUtility.IconContent("JointAngularLimits");

	public override void OnToolGUI(EditorWindow window)
	{
		foreach (Object target in base.targets) 
		{
			PhysicsSpring val = target as PhysicsSpring;
			if (!((Object)val == (Object)null))
			{
				EditorGUI.BeginChangeCheck();
				
				Handles.color = Color.yellow;
				Handles.SphereHandleCap(122, val.PovitA.position, Quaternion.Euler(0,0,0), 0.6f, EventType.Repaint);
				Handles.color = Color.red;
				Handles.SphereHandleCap(122, val.PovitB.position, Quaternion.Euler(0,0,0), 0.6f, EventType.Repaint);

				if (EditorGUI.EndChangeCheck()) {}
			}
		}
	}
}
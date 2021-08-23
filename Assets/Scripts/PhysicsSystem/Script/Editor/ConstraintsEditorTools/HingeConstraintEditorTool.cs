using PhysicsRT;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[EditorTool("PhysicsHingeConstraintEditorTool", typeof(HingeConstraint))]
public class HingeConstraintEditorTool : EditorTool
{
  protected static class Styles
	{
		public static readonly string editAngularLimitsUndoMessage = L10n.Tr("Show Axis and povit");
	}

	public override GUIContent toolbarIcon => EditorGUIUtility.IconContent("JointAngularLimits");

	public override void OnToolGUI(EditorWindow window)
	{
		foreach (Object target in base.targets) 
		{
			HingeConstraint val = target as HingeConstraint;
			if (!((Object)val == (Object)null))
			{
				EditorGUI.BeginChangeCheck();
				var pos = val.Povit; 
				
				Handles.color = Color.yellow;
				Handles.SphereHandleCap(122, pos, Quaternion.identity, 0.6f, EventType.Repaint);
				Handles.color = Color.red;
				Handles.ArrowHandleCap(123, pos, Quaternion.identity, 2, EventType.Repaint);

				if (EditorGUI.EndChangeCheck()) {}
			}
		}
	}
}
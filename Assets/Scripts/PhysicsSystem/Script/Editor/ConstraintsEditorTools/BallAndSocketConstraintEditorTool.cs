using PhysicsRT;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[EditorTool("PhysicsBallAndSocketConstraintEditorTool", typeof(BallAndSocketConstraint))]
public class BallAndSocketConstraintEditorTool : EditorTool
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
			BallAndSocketConstraint val = target as BallAndSocketConstraint;
			if (!((Object)val == (Object)null))
			{
				EditorGUI.BeginChangeCheck();
				
				Handles.color = Color.yellow;
				Handles.SphereHandleCap(122, val.Povit, Quaternion.Euler(0,0,0), 0.6f, EventType.Repaint);

				if (EditorGUI.EndChangeCheck()) {}
			}
		}
	}
}
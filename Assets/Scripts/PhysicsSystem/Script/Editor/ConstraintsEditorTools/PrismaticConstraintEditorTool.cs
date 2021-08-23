using PhysicsRT;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[EditorTool("PhysicsPrismaticConstraintEditorTool", typeof(PrismaticConstraint))]
public class PrismaticConstraintEditorTool : EditorTool
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
			PrismaticConstraint val = target as PrismaticConstraint;
			if (!((Object)val == (Object)null))
			{
				EditorGUI.BeginChangeCheck();
				var pos = val.Povit; 
        var rot = Quaternion.FromToRotation(Vector3.right, val.Axis).eulerAngles;
				
				Handles.color = Color.yellow;
				Handles.SphereHandleCap(122, pos, Quaternion.Euler(0,0,0), 0.6f, EventType.Repaint);
				Handles.color = Color.red;
				Handles.ArrowHandleCap(123, pos, Quaternion.Euler(rot), 6, EventType.Repaint);

				if (EditorGUI.EndChangeCheck()) {}
			}
		}
	}
}
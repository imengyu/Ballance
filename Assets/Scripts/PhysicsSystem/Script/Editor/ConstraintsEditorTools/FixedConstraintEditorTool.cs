using PhysicsRT;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[EditorTool("PhysicsFixedConstraintEditorTool", typeof(FixedConstraint))]
public class FixedConstraintEditorTool : EditorTool
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
			FixedConstraint val = target as FixedConstraint;
			if (!((Object)val == (Object)null))
			{
				EditorGUI.BeginChangeCheck();
				var pos = Vector3.Scale(val.Povit, val.transform.lossyScale); 
				using (new Handles.DrawingScope(Matrix4x4.TRS(val.transform.position, val.transform.rotation, val.transform.localScale)))
				{
          Handles.color = Color.yellow;
					Handles.SphereHandleCap(122, pos, Quaternion.Euler(0,0,0), 0.2f, EventType.Repaint);
				}
				if (EditorGUI.EndChangeCheck())
				{
				}
			}
		}
	}
}
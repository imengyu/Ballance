using PhysicsRT;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[EditorTool("PhysicsLimitedHingeConstraintEditorTool", typeof(LimitedHingeConstraint))]
public class LimitedHingeConstraintEditorTool : EditorTool
{
  protected static class Styles
	{
		public static readonly string editAngularLimitsUndoMessage = L10n.Tr("Change Angular Limits");
	}

  private void OnEnable()
	{
		angularLimitHandle.yMotion = ConfigurableJointMotion.Locked;
		angularLimitHandle.zMotion = ConfigurableJointMotion.Locked;
		angularLimitHandle.yHandleColor = Color.clear;
		angularLimitHandle.zHandleColor = Color.clear;
		angularLimitHandle.xRange = new Vector2(-3.40282326E+38f, 3.40282326E+38f);
	}

	private JointAngularLimitHandle m_AngularLimitHandle = new JointAngularLimitHandle();

	public override GUIContent toolbarIcon => EditorGUIUtility.IconContent("JointAngularLimits");

	protected JointAngularLimitHandle angularLimitHandle => m_AngularLimitHandle;

	protected static float GetAngularLimitHandleSize(Vector3 position)
	{
		return HandleUtility.GetHandleSize(position);
	}

	public override void OnToolGUI(EditorWindow window)
	{
		foreach (Object target in base.targets)
		{
			LimitedHingeConstraint val = target as LimitedHingeConstraint;
			if (!((Object)val == (Object)null))
			{
				EditorGUI.BeginChangeCheck();
				using (new Handles.DrawingScope(Matrix4x4.TRS(
					val.transform.position + Vector3.Scale(new Vector3(-val.Povit.x, val.Povit.y, -val.Povit.z), 
						new Vector3(val.transform.lossyScale.y, val.transform.lossyScale.x, val.transform.lossyScale.z)), 
					Quaternion.FromToRotation(Vector3.right, val.Axis), Vector3.one)))
					DoAngularLimitHandles(val);
				using (new Handles.DrawingScope(Matrix4x4.TRS(val.transform.position, Quaternion.FromToRotation(Vector3.right, val.Axis), Vector3.one)))
				{
          Handles.color = Color.red;
          Handles.ArrowHandleCap(123, Vector3.Scale(new Vector3(val.Povit.y, val.Povit.x, -val.Povit.z), val.transform.lossyScale), Quaternion.Euler(0,90,0), 2, EventType.Repaint);
				}
				if (EditorGUI.EndChangeCheck())
				{
				}
			}
		}
	}
	private void DoAngularLimitHandles(LimitedHingeConstraint joint)
	{
		angularLimitHandle.xMotion = ConfigurableJointMotion.Limited;
		angularLimitHandle.xMin = joint.AgularLimitMin;
		angularLimitHandle.xMax = joint.AgularLimitMax;

    EditorGUI.BeginChangeCheck();
		angularLimitHandle.radius = GetAngularLimitHandleSize(Vector3.zero);
		angularLimitHandle.DrawHandle();
		if (EditorGUI.EndChangeCheck())
		{
			Undo.RecordObject(joint, Styles.editAngularLimitsUndoMessage);
			joint.AgularLimitMin = angularLimitHandle.xMin;
			joint.AgularLimitMax = angularLimitHandle.xMax;
		}
	}
}
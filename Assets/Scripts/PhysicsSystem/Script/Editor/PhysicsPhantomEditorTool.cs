using PhysicsRT;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[EditorTool("PhysicsPhantomEditorTool", typeof(PhysicsPhantom))]
public class PhysicsPhantomEditorTool : EditorTool
{
	public override GUIContent toolbarIcon => EditorGUIUtility.IconContent("EditCollider");

	public override void OnToolGUI(EditorWindow window)
	{
		foreach (Object target in base.targets) 
		{
			PhysicsPhantom val = target as PhysicsPhantom;
			if (!((Object)val == (Object)null))
			{
				var size2 = val.Max - val.Min;
				var oldCenter = val.Min + size2 / 2;
				using (new Handles.DrawingScope(Matrix4x4.TRS(val.transform.position, Quaternion.identity, Vector3.one)))
				{
					Handles.DrawWireCube(oldCenter, size2);
				}
			}
		}
	}
}
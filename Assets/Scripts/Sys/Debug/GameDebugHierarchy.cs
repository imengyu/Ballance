using Ballance2;
using RuntimeInspectorNamespace;
using UnityEngine;

public class GameDebugHierarchy : MonoBehaviour {

  public RuntimeHierarchy RuntimeHierarchy;

  private void Start() {
    DebugCamera.Instance.GameDebugHierarchy = this;
    RuntimeHierarchy.OnSelectionChanged += OnSelectionChanged;
  }
  private void OnSelectionChanged(Transform selection) {
    DebugCamera.Instance.GameDebugTools.InspectObject(selection);
    DebugCamera.Instance.AddObjectToTransformGizmo(selection);
  }
}
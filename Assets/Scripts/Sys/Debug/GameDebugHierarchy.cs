using Ballance2;
using RuntimeInspectorNamespace;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameDebugHierarchy.cs
* 
* 用途：
* 调试工具Hierarchy控制类。
*
* 作者：
* mengyu
*/

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
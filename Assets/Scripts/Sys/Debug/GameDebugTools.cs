using Ballance2;
using RuntimeGizmos;
using RuntimeInspectorNamespace;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameDebugTools.cs
* 
* 用途：
* 调试工具控制类。
*
* 作者：
* mengyu
*
* 
* 
*
*/

public class GameDebugTools : MonoBehaviour {

  public RuntimeInspector RuntimeInspector;

  public Button ButtonToolMove;
  public Button ButtonToolPos;
  public Button ButtonToolRotate;
  public Button ButtonToolScale;

  public Image ButtonToolMoveImage;

  public Button ButtonHandlePosition;
  public Button ButtonHandleRotation;
  public Image ButtonHandlePositionImage;
  public Image ButtonHandleRotationImage;
  public Text ButtonHandlePositionText;
  public Text ButtonHandleRotationText;

  public Sprite SpritePositionCenter;
  public Sprite SpritePositionPivot;
  public Sprite SpriteRotationLocal;
  public Sprite SpriteRotationGlobal;
  public Sprite SpriteIconHand;
  public Sprite SpriteIconEve;

  private Transform lastInspectTransform = null;
  private ColorBlock toolButtonOrginalColor;
  private ColorBlock toolButtonActiveColor;

  private void Start() {
    DebugCamera.Instance.GameDebugTools = this;

    toolButtonOrginalColor = ButtonToolMove.colors;
    toolButtonActiveColor = new ColorBlock();
    toolButtonActiveColor.normalColor = new Color(0.189f, 0.41f, 0.67f);
    toolButtonActiveColor.disabledColor = toolButtonOrginalColor.disabledColor;
    toolButtonActiveColor.highlightedColor = toolButtonOrginalColor.highlightedColor;
    toolButtonActiveColor.pressedColor = toolButtonOrginalColor.pressedColor;

    ButtonToolMove.onClick.AddListener(() => {
      DebugCamera.Instance.Tool = DebugCamera.DebugCameraTool.Drag;
      DelightButtonTools();
      ButtonToolMove.colors = toolButtonActiveColor;
    });
    ButtonToolRotate.onClick.AddListener(() => {
      DebugCamera.Instance.Tool = DebugCamera.DebugCameraTool.TransformGizmo;
      DebugCamera.Instance.TransformGizmo.transformType = TransformType.Rotate;
      DelightButtonTools();
      ButtonToolRotate.colors = toolButtonActiveColor;
    });
    ButtonToolScale.onClick.AddListener(() => {
      DebugCamera.Instance.Tool = DebugCamera.DebugCameraTool.TransformGizmo;
      DebugCamera.Instance.TransformGizmo.transformType = TransformType.Scale;
      DelightButtonTools();
      ButtonToolScale.colors = toolButtonActiveColor;
    });
    ButtonToolPos.onClick.AddListener(() => {
      DebugCamera.Instance.Tool = DebugCamera.DebugCameraTool.TransformGizmo;
      DebugCamera.Instance.TransformGizmo.transformType = TransformType.Move;
      DelightButtonTools();
      ButtonToolPos.colors = toolButtonActiveColor;
    });
  
    ButtonHandlePosition.onClick.AddListener(() => {
      if(DebugCamera.Instance.TransformGizmo.pivot == TransformPivot.Center) {
        DebugCamera.Instance.TransformGizmo.pivot = TransformPivot.Pivot;
        ButtonHandlePositionImage.sprite = SpritePositionPivot;
        ButtonHandlePositionText.text = "Pivot";
      } else {
        DebugCamera.Instance.TransformGizmo.pivot = TransformPivot.Center;
        ButtonHandlePositionImage.sprite = SpritePositionCenter;
        ButtonHandlePositionText.text = "Center";
      }
    });
    ButtonHandleRotation.onClick.AddListener(() => {
      if(DebugCamera.Instance.TransformGizmo.space == TransformSpace.Global) {
        DebugCamera.Instance.TransformGizmo.space = TransformSpace.Local;
        ButtonHandleRotationImage.sprite = SpriteRotationLocal;
        ButtonHandleRotationText.text = "Local";
      } else {
        DebugCamera.Instance.TransformGizmo.space = TransformSpace.Global;
        ButtonHandleRotationImage.sprite = SpriteRotationGlobal;
        ButtonHandleRotationText.text = "Global";
      }
    });
  }
  private void DelightButtonTools() {
    ButtonToolMove.colors = toolButtonOrginalColor;
    ButtonToolPos.colors = toolButtonOrginalColor;
    ButtonToolRotate.colors = toolButtonOrginalColor;
    ButtonToolScale.colors = toolButtonOrginalColor;
  }

  private bool dragging = false;

  public void ShowInHierarchy() {
    if(lastInspectTransform != null) {
      if(!DebugCamera.Instance.GameDebugHierarchyWindow.GetVisible())
        DebugCamera.Instance.GameDebugHierarchyWindow.Show();
      DebugCamera.Instance.GameDebugHierarchy.RuntimeHierarchy.Select(lastInspectTransform);
    }
  }
  public void ShowInView() {
    if(lastInspectTransform != null)
      DebugCamera.Instance.MoveCameraToTransform(lastInspectTransform);
  }
  public void InspectObject(Transform transform) {
    lastInspectTransform = transform;
    RuntimeInspector.Inspect(transform.gameObject);
  }
  public void SetFreeDragging(bool _dragging) {
    if(dragging != _dragging) {
      dragging = _dragging;
      ButtonToolMoveImage.sprite = dragging ? SpriteIconEve : SpriteIconHand;
    }
  }


}
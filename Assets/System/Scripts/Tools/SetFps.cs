using UnityEngine;

/// <summary>
/// 设置 targetFrameRate 的脚本
/// </summary>
public class SetFps : MonoBehaviour
{
  [Tooltip("目标FPS")]
  public int FPS = 60;
  
  private void Start() {
    Application.targetFrameRate = FPS;
  }
}
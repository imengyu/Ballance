using UnityEngine;

public class SetFps : MonoBehaviour {
  public int FPS = 60;
  private void Start() {
    Application.targetFrameRate = FPS;
  }
}
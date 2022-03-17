using UnityEngine;
using UnityEngine.UI;

public class DebugFps : MonoBehaviour
{
  public float showTime = 1f;
  public Text text;

  private int count = 0;
  private float deltaTime = 0f;

  public static DebugFps Instance;

  private void Start() {
    Instance = this;
  }
  void Update()
  {
    count++;
    deltaTime += Time.deltaTime;
    if (deltaTime >= showTime)
    {
      float fps = count / deltaTime;
      float milliSecond = deltaTime * 1000 / count;
      string strFpsInfo = string.Format("FPS: {0:0.0} {1:0.0} ms", fps, milliSecond);
      text.text = strFpsInfo;
      count = 0;
      deltaTime = 0f;
    }
  }
}

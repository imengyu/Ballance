using UnityEngine;

namespace Ballance2.UI.Utils
{
  public class UIFollowMouse : MonoBehaviour
  {
    /// <summary>
    /// 需要跟随的物体
    /// </summary>
    private GameObject go;

    private void Awake()
    {
      go = gameObject;
    }
    private void OnEnable()
    {
      ChoosePivot();
      go.transform.position = Input.mousePosition;
    }
    /// <summary>
    /// 根据鼠标位置选择中心点，避免出现UI到屏幕外的情况
    /// </summary>
    private void ChoosePivot()
    {
      float width = Screen.width / 2;
      float height = Screen.height / 2;
      if (Input.mousePosition.x < width)
      {
        go.GetComponent<RectTransform>().pivot = new Vector2(0, go.GetComponent<RectTransform>().pivot.y);
      }
      else
      {
        go.GetComponent<RectTransform>().pivot = new Vector2(1, go.GetComponent<RectTransform>().pivot.y);
      }
      if (Input.mousePosition.y < height)
      {
        go.GetComponent<RectTransform>().pivot = new Vector2(go.GetComponent<RectTransform>().pivot.x, 0);
      }
      else
      {
        go.GetComponent<RectTransform>().pivot = new Vector2(go.GetComponent<RectTransform>().pivot.x, 1);
      }
    }
    void Update()
    {
      ChoosePivot();
      go.transform.position = Input.mousePosition;
    }
  }
}
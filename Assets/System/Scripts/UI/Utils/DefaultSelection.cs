using UnityEngine;
using UnityEngine.EventSystems;

namespace Ballance2.UI.Utils
{
  /// <summary>
  /// 这个脚本在激活时激活指定的对象s
  /// </summary>
  public class DefaultSelection : MonoBehaviour
  {
    public GameObject select;

    public void SelectDefaultSelection()
    {
      Select(select);
    }
    public void Select(GameObject go)
    {
      EventSystem.current.SetSelectedGameObject(go);
    }
  }
}
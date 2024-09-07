using System.Collections;
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
    public GameObject selectSecondChoice;

    public void SelectDefaultSelection()
    {
      if (select.activeSelf)
        Select(select);
      else
        Select(selectSecondChoice);
    }
    public void Select(GameObject go)
    {
      StartCoroutine(DelaySelect(go));
    }

    private IEnumerator DelaySelect(GameObject go)
    {
      yield return new WaitForSeconds(0.01f);

      if (EventSystem.current.currentSelectedGameObject != go)
        EventSystem.current.SetSelectedGameObject(go);
    }
  }
}
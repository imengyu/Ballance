using UnityEngine;
using UnityEngine.EventSystems;

namespace Ballance2.UI.Utils {

  public class PreventDeselectionGroup : MonoBehaviour
  {
#if UNITY_EDITOR || UNITY_STANDALONE
    EventSystem evt;

    private void Start()
    {
      evt = EventSystem.current;
    }

    GameObject sel;

    private void Update()
    {
      if (evt.currentSelectedGameObject != null && evt.currentSelectedGameObject != sel)
        sel = evt.currentSelectedGameObject;
      else if (sel != null && evt.currentSelectedGameObject == null)
        evt.SetSelectedGameObject(sel);
    }
#endif
  }
}
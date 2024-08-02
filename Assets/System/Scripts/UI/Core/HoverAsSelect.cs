using UnityEngine;
using UnityEngine.EventSystems;

namespace Ballance2.UI.Core
{
  public class HoverAsSelect : UIBehaviour, IPointerEnterHandler
  {
    public void OnPointerEnter(PointerEventData eventData)
    {
      EventSystem.current.SetSelectedGameObject(gameObject);
    }
  }
}
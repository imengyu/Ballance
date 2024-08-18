using UnityEngine;
using UnityEngine.EventSystems;

namespace Ballance2.Game.LevelEditor.UI.Mini
{
  public class SimpleTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    public string Text;
    public SimpleTooltipHolder Holder = null; 

    private void Awake()
    {
      if (Holder == null)
        Holder = GetComponentInParent<SimpleTooltipHolder>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
      if (Holder != null)
        Holder.Show(Text);
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
      if (Holder != null)
        Holder.Hide();
    }
  }
}

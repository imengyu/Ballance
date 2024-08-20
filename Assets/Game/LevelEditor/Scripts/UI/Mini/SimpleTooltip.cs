using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ballance2.Game.LevelEditor.UI.Mini
{
  public class SimpleTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    public string Text;
    public SimpleTooltipHolder Holder = null;

    private bool enter = false;

    private void Awake()
    {
      if (Holder == null)
        Holder = GetComponentInParent<SimpleTooltipHolder>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
      enter = true;
      if (Holder != null)
        StartCoroutine(DelayShow());
    }

    private IEnumerator DelayShow()
    {
      yield return new WaitForSeconds(0.3f);
      if (enter)
        Holder.Show(Text);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
      enter = false;
      if (Holder != null)
        Holder.Hide();
    }
  }
}

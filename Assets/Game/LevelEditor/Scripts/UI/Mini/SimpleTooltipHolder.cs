using Ballance2.UI.Core.Controls;
using UnityEngine;

namespace Ballance2.Game.LevelEditor.UI.Mini
{
  public class SimpleTooltipHolder : MonoBehaviour
  {
    public UIText Text;
    public GameObject Tooltip;

    private void Awake()
    {
      Hide();
    }
    public void Show(string text)
    {
      Tooltip.SetActive(true);
      Text.text = text;
    }
    public void Hide()
    {
      Tooltip.SetActive(false);
    }
  }
}

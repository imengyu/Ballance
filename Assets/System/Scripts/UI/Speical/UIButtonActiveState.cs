using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ballance2.UI 
{
  public class UIButtonActiveState : MonoBehaviour, IPointerClickHandler
  {
    private Image image;
    private Button button;

    public Color normalColor = Color.gray;
    public Color activeColor = Color.red;
    public bool startActive = false;
    public object data = null;

    public Action onClick;

    private void Awake() 
    {
      button = GetComponent<Button>();
      if (button != null)
      {
        image = button.targetGraphic.gameObject.GetComponent<Image>();
        normalColor = button.colors.normalColor;
        activeColor = button.colors.selectedColor;
      }
      else
      {
        image = gameObject.GetComponent<Image>();
      }
      if (startActive)
        SetActive(true);
    }

    public void SetColor(Color _normalColor, Color _activeColor)
    {
      if (button != null)
      {
        var colors = new ColorBlock();
        colors.disabledColor = button.colors.disabledColor;
        colors.pressedColor = button.colors.pressedColor;
        colors.normalColor = _normalColor;
        colors.highlightedColor = _activeColor;
        colors.selectedColor = _activeColor;
        colors.colorMultiplier = button.colors.colorMultiplier;
        button.colors = colors;
      }
      normalColor = _normalColor;
      activeColor = _activeColor;
    }

    public void SetActive(bool active)
    {
      image.color = active ? activeColor : normalColor;
    }
  
    public void OnPointerClick(PointerEventData eventData)
    {
      onClick?.Invoke();
    }
  }
}
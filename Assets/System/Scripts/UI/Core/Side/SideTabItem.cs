using Ballance2.Services.InputManager;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.UI.Core.Side
{
  public class SideTabItem : MonoBehaviour
  {
    public Image Image = null;
    public Text TitleText = null;
    public GameObject TitleTip = null;
    public GameObject ActiveBackground = null;
    public Window Window = null;

    private void Start() {
      var eventTriggerListener = EventTriggerListener.Get(gameObject);
      eventTriggerListener.onEnter += OnMouseEnter;
      eventTriggerListener.onClick += OnClick;
      eventTriggerListener.onExit += OnMouseExit;
    }

    public void SetWindow(Window window) {
      Window = window;
      Image.sprite = window.Icon;
      TitleText.text = window.Title;
    }
    public void SetBgActive(bool active) {
      ActiveBackground.SetActive(active);
    }

    private void OnMouseEnter(GameObject go)
    {
      TitleTip.SetActive(true);
    }
    private void OnClick(GameObject go)
    {
      TitleTip.SetActive(false);
      Window.ActiveWindow();
    }
    private void OnMouseExit(GameObject go)
    {
      TitleTip.SetActive(false);
    }
  }
}

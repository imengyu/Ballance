using UnityEngine;
using Ballance2.UI.Core;

namespace Ballance2.Game
{
  [RequireComponent(typeof(QuickOutline))]
  public class MenuLevelMouseSelect : MonoBehaviour {
    
    QuickOutline QuickOutline = null;
      
    public Color NormalColor;
    public Color HoverColor;
    public string MessageName = "";
    public GameUIControlMessageSender MessageSender = null;

    private void Start() {
      QuickOutline = gameObject.GetComponent<QuickOutline>();
      MessageSender = gameObject.GetComponent<GameUIControlMessageSender>();
    }
    private void OnMouseEnter() {
      QuickOutline.OutlineColor = HoverColor;
    }
    private void OnMouseExit() {
      QuickOutline.OutlineColor = NormalColor;
    }
    private void OnMouseDown() {
      MessageSender.NotifyEvent(MessageName);
    }
  }
}
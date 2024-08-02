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
    public void SetLightState(bool light) {
      QuickOutline.OutlineColor = light ? HoverColor : NormalColor;
    }
    public void Select() {
      MessageSender.NotifyEvent(MessageName);
    }
  }
}
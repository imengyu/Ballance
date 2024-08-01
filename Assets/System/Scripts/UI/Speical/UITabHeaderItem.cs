using UnityEngine;
using UnityEngine.UI;
using Ballance2.UI.Core.Controls;

namespace Ballance2.UI 
{ 
  public class UITabHeaderItem : MonoBehaviour 
  {
    [SerializeField]
    private UIText text;
    [SerializeField]
    private Sprite activeImage;
    [SerializeField]
    private Sprite deactiveImage;
    [SerializeField]
    private Image image;

    public void SetTitle(string title)
    {
      text.text = title;
    }
    public void SetActive(bool active)
    {
      image.sprite = active ? activeImage : deactiveImage;
    }
  }
}
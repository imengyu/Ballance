using Ballance2.UI.Core.Controls;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Menu
{
  public class LevelSelectItem : MonoBehaviour
  {
    public UIText Title;
    public Image Image;
    public Image PassedImage;
    public RectTransform Score;
    public Image Score1;
    public Image Score2;
    public Image Score3;
    public Image Score4;
    public Image Score5;
    public Button Inner;
    public Button Delete;

    public Action onClick;
    public Action onDeleteClick;

    private void Start()
    {
      Inner.onClick.AddListener(() => onClick?.Invoke());
      Delete.onClick.AddListener(() => onDeleteClick?.Invoke());
    }
  }
}
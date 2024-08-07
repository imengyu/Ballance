using System;
using System.Collections.Generic;
using Ballance2.Services.I18N;
using Ballance2.UI;
using Ballance2.UI.Core.Controls;
using Battlehub.RTHandles;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorContentSelectionItem : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
  {
    public Image Image;
    public UIText Text;
    public PrefabSpawnPoint prefabSpawnPoint;

    public Action<LevelDynamicModelAsset> onShowTip;
    public Action onHideTip;

    private LevelDynamicModelAsset asset = null;

    public void OnDeselect(BaseEventData eventData)
    {
      onHideTip.Invoke();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
      ShowTip();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
      onHideTip.Invoke();
    }
    public void OnSelect(BaseEventData eventData)
    {
      ShowTip();
    }

    private void ShowTip()
    {
      if (Image.sprite != null && asset.PreviewImage == null)
        asset.PreviewImage = Image.sprite;
      onShowTip.Invoke(asset);
    }

    public void SetInfo(LevelDynamicModelAsset info)
    {
      asset = info;
      Image.sprite = info.PreviewImage;
      Text.text = info.Name;
      prefabSpawnPoint.UpdateInfo(info.Prefab);
    }
  }
}
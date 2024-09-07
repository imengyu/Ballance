using System;
using Ballance2.UI.Core.Controls;
using Battlehub.RTHandles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorContentSelectionItem : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
  {
    public Image Image;
    public UIText Text;
    public RectTransform Tag;
    public UIText TagText;
    public PrefabSpawnPoint prefabSpawnPoint;
    public RectTransform DeleteButton;

    public Sprite ImageMissing;

    public Action<LevelDynamicModelAsset> onShowTip;
    public Action onHideTip;
    public Action<LevelDynamicModelAsset> onDelete;

    private LevelDynamicModelAsset asset = null;
    private bool canDelete = false;



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
    public void Delete()
    {
      if (canDelete)
        onDelete.Invoke(asset);
    }

    public void SetInfo(LevelDynamicModelAsset info)
    {
      asset = info;
      Image.sprite = info.Prefab == null ? ImageMissing : info.PreviewImage;
      Text.text = info.Name;
      Tag.gameObject.SetActive(!string.IsNullOrEmpty(info.Tag));
      TagText.text = info.Tag;
      prefabSpawnPoint.UpdateInfo(info.Prefab);
      canDelete = info.SourceType == LevelDynamicModelSource.Embed && info.SourcePath.StartsWith("levelasset:");
      DeleteButton.gameObject.SetActive(canDelete);
    }
  }
}
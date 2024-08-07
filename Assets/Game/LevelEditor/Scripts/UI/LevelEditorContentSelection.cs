using System;
using System.Collections.Generic;
using Ballance2.Services.I18N;
using Ballance2.UI;
using Ballance2.UI.Core.Controls;
using Battlehub.RTHandles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorContentSelection : MonoBehaviour 
  {
    public RectTransform HoverTip;
    public Image HoverTipImage;
    public UIText HoverTipName;
    public UIText HoverTipDesc;
    public UIText HoverTipTagText;
    public UITabSelect SubCategoryTab;
    public RectTransform EmptyTip;

    public TMP_InputField InputFieldSearch;
    public ScrollView AssetSelectList;

    public Func<LevelDynamicModelAsset, bool> onPrefabDragStart;
    public Action onPrefabDragEnd;
    public Action<LevelDynamicModelAsset, GameObject> onPrefabInstantiate;
    public Action<LevelDynamicModelAsset, GameObject> onPrefabDrop;

    private void Start() 
    {
      InputFieldSearch.onEndEdit.AddListener(Search);
      AssetSelectList.SetItemCountFunc(() => currentShowAssets?.Count ?? 0);
      AssetSelectList.SetUpdateFunc((index, item) => {
        var data = currentShowAssets[index];
        var ui = item.GetComponent<LevelEditorContentSelectionItem>();
        ui.SetInfo(data);
        ui.onShowTip = ShowTip;
        ui.onHideTip = HideTip;
        var ui2 = item.GetComponent<PrefabSpawnPoint>();
        ui2.onPrefabInstantiate = (go) => onPrefabInstantiate?.Invoke(data, go);
        ui2.onPrefabDrop = (go) => onPrefabDrop?.Invoke(data, go);
        ui2.onPrefabDragStart = () => onPrefabDragStart?.Invoke(data) ?? false;
        ui2.onPrefabDragEnd = onPrefabDragEnd;
      });
    }

    public void ShowTip(LevelDynamicModelAsset asset)
    {
      HoverTipImage.sprite = asset.PreviewImage;
      HoverTipName.text = asset.Name;
      HoverTipDesc.text = asset.Desc;
      HoverTipTagText.text = "";
      HoverTip.gameObject.SetActive(true);
    }
    public void HideTip()
    {
      HoverTip.gameObject.SetActive(false);
    }

    private LevelDynamicModelCategory currentShowCategory = LevelDynamicModelCategory.UnSet;
    private Dictionary<string, List<LevelDynamicModelAsset>> currentShowCategoryData = null;
    private List<LevelDynamicModelAsset> currentShowAssets = null;
    private string currentShowSubCategory = "";

    public void Close()
    {
      gameObject.SetActive(false);
    }
    public void Search(string filter) 
    {
      if (filter == "")
        LoadSubCategory(currentShowSubCategory);
      else
      {
        currentShowAssets = new List<LevelDynamicModelAsset>();
        var category = currentShowCategoryData[currentShowSubCategory];
        foreach (var item in category)
        {
          if (item.Name.Contains(filter) || item.SourcePath.Contains(filter))
            currentShowAssets.Add(item);
        }
        AssetSelectList.UpdateData(false);
        EmptyTip.gameObject.SetActive(currentShowAssets.Count == 0);
      }
    }
    public void LoadCategory(LevelDynamicModelCategory category) 
    {
      gameObject.SetActive(true);
      if (category == currentShowCategory)
        return;
      currentShowCategory = category;
      if (LevelEditorManager.Instance.LevelAssetsGrouped.TryGetValue(category, out var v))
        currentShowCategoryData = v;
      else
      {
        SubCategoryTab.SetTabs(new UITabSelect.TabItem[0]);
        LoadSubCategory("");
        return;
      }

      var tabs = new List<UITabSelect.TabItem>();
      var firstItem = "";

      foreach (var _item in currentShowCategoryData.Keys)
      {
        var item = _item;
        if (firstItem == "")
          firstItem = _item;
        tabs.Add(new UITabSelect.TabItem() {
          Title = I18N.Tr($"core.editor.categoryNames.{item}", item),
          OnShowContent = () => LoadSubCategory(item),
        });
      }

      SubCategoryTab.SetTabs(tabs.ToArray());
      LoadSubCategory(firstItem);
    }
    public void LoadSubCategory(string subCategory) 
    {
      gameObject.SetActive(true);
      if (subCategory == "")
      {
        currentShowCategoryData = null;
        currentShowAssets = new List<LevelDynamicModelAsset>();
        EmptyTip.gameObject.SetActive(true);
      }
      else
      {
        currentShowSubCategory = subCategory;
        InputFieldSearch.text = "";
        currentShowAssets = currentShowCategoryData[subCategory];
        EmptyTip.gameObject.SetActive(currentShowAssets.Count == 0);
      }
      AssetSelectList.UpdateData(false);
    }

  }
}
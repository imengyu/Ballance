using System;
using System.Collections;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.UI;
using Ballance2.UI.Core.Controls;
using Ballance2.Menu.LevelManager;
using TMPro;
using UnityEngine;
using static Ballance2.UI.UITabSelect;
using System.Collections.Generic;
using Ballance2.Game.GamePlay;
/**
自定义关卡管理菜单的控制脚本
*/
namespace Ballance2.Menu
{
  public class LevelSelectUIManager : MonoBehaviour
  {
    private GameUIManager gameUIManager;

    public UIText EmptyText;
    public TMP_Text PagerText;
    public UITabSelect Tab;
    public ScrollView List;

    private void Start() {
      gameUIManager = GameSystem.GetSystemService<GameUIManager>();
      Tab.SetTabs(new TabItem[]
      {
        new TabItem() { Title = "I18N:core.ui.Menu.LevelSelect.Category.Hot" },
        new TabItem() { Title = "I18N:core.ui.Menu.LevelSelect.Category.Subscribe" },
        new TabItem() { Title = "I18N:core.ui.Menu.LevelSelect.Category.Local" },
        new TabItem() { Title = "I18N:core.ui.Menu.LevelSelect.Category.Mine" },
      });
      Tab.onActiveTabChanged += Tab_onActiveTabChanged;
      List.itemCountFunc = () => list.Count - currentPage * pageSize;
      List.updateFunc = (index, rect) =>
      {
        var data = list[index + currentPage * pageSize];
        var item = rect.GetComponent<LevelSelectItem>();
        item.onClick = () => OnItemClick(data);
        item.onDeleteClick = () => OnItemDeleteClick(data);
        data.CurrentHolder = item;
        data.LoadToUI();
      };

      GameManager.GameMediator.SubscribeSingleEvent(GamePackage.GetSystemPackage(), "PageStartCustomLevelLoad", "A", (evtName, param) => {
        LoadLevelList();
        return false;
      });
      gameUIManager.FindPage("PageStartLevelSelect").OnShow = (param) =>
      {
        if (param.ContainsKey("page"))
          Tab.JumpToItem(Tab.Tabs[3]);
      };
    }
    private void OnDestroy()
    {
    }
    private void Update()
    {
      if (!isLoadInfo && dataPendings.Count > 0)
      {
        isLoadInfo = true;
        StartCoroutine(LoadInfoToList(dataPendings.Pop()));
      }
    }
    private bool isLoadInfo = false;
    private IEnumerator LoadInfoToList(ListItem item)
    {
      if (item.Level.IsLoaded)
      {
        isLoadInfo = false;
        yield break;
      }
      yield return item.Level.LoadInfo().AsIEnumerator();
      item.LoadToUI();
      isLoadInfo = false;
    }

    internal class ListItem
    {
      public LevelSelectItem CurrentHolder;
      public LevelRegistedItem Level;

      public string Title = "";
      public Sprite Icon = null;
      public Sprite Preview = null;
      public int Score = -1;
      public bool Passed = false;
      public bool IsNetwork = false;
      public bool IsSubscribe = false;

      public ListItem()
      {

      }
      public ListItem(LevelRegistedItem level)
      {
        Level = level;
        LoadInfo();
      }

      public string LocalPath
      {
        get
        {
          if (!IsNetwork && Level != null)
          {
            if (Level.Type == LevelRegistedType.Local || Level.Type == LevelRegistedType.Mine)
              return I18N.TrF("core.ui.Menu.LevelSelect.LocalPath", "", ((LevelRegistedLocallItem)Level).path);
          }
          return "";
        }
      }

      public void LoadInfo()
      {
        if (Level != null)
        {
          Icon = Level.Logo;
          Preview = Level.Preview;
          if (Level.InfoJson != null)
          {
            Title = Level.InfoJson.name
              + " by " + (string.IsNullOrEmpty(Level.InfoJson.author) ? "?" : Level.InfoJson.author);
            Passed = HighscoreManager.Instance.CheckLevelPassState(Level.InfoJson.name);
          }
        }
      }
      public void LoadToUI()
      {
        LoadInfo();
        if (CurrentHolder != null)
        {
          CurrentHolder.Image.sprite = Icon;
          CurrentHolder.Title.text = Title;
          CurrentHolder.PassedImage.gameObject.SetActive(Passed);
          CurrentHolder.Score.gameObject.SetActive(Score >= 0);
          CurrentHolder.Delete.gameObject.SetActive(!IsNetwork);
        }
      }
    }

    private int pageSize = 20;
    private int currentPage = 0;
    private int allPage = 0;
    private List<ListItem> list = new List<ListItem>();
    private Stack<ListItem> dataPendings = new Stack<ListItem>();

    private void Tab_onActiveTabChanged(TabItem arg1, int arg2)
    {
      LoadLevelList();
    }
    private void LoadLevelList()
    {
      list.Clear();
      EmptyText.text = I18N.Tr("core.ui.Menu.LevelSelect.Empty");
      switch (Tab.CurrentActiveTabIndex)
      {
        case 0:

          break;
        case 1:
          EmptyText.text = I18N.Tr("core.ui.Menu.LevelSelect.EmptySubscribe");
          break;
        case 2:
          foreach (var level in LevelManager.LevelManager.Instance.LocalLevels)
          {
            var item = new ListItem(level);
            if (!level.IsLoaded)
              dataPendings.Push(item);
            list.Add(item);
          }
          break;
        case 3:
          foreach (var level in LevelManager.LevelManager.Instance.MineLevels)
          {
            var item = new ListItem(level);
            if (!level.IsLoaded)
              dataPendings.Push(item);
            list.Add(item);
          }
          break;
      }
      allPage = Math.Max(list.Count / pageSize, 1);
      EmptyText.gameObject.SetActive(list.Count == 0);
      List.UpdateData();
      UpdateListPager();
    }
    private void UpdateListPager()
    {
      PagerText.text = $"{currentPage + 1}/{allPage}";
    }

    private void OnItemClick(ListItem item)
    {
      gameUIManager.GoPageWithOptions("PageStartLevelInfo", new Dictionary<string, object>() { { "item", item } });
    }
    private void OnItemDeleteClick(ListItem item)
    {
      gameUIManager.GlobalConfirmWindow(
        item.IsSubscribe ? I18N.Tr("core.ui.Menu.LevelSelect.UnSubscribeConfirmText")
        : I18N.Tr("core.ui.Menu.LevelSelect.DeleteConfirmText"),
        () =>
        {
          if (!item.IsNetwork)
            LevelManager.LevelManager.Instance.DeleteLevel(item.Level);
          LoadLevelList();
        }
      );
    }

    public void NextPage()
    {
      currentPage = Math.Min(currentPage + 1, allPage - 1);
      List.UpdateData();
      UpdateListPager();
    }
    public void PrevPage()
    {
      currentPage = Math.Min(currentPage - 1, 0);
      List.UpdateData();
      UpdateListPager();
    }

    public void Refresh()
    {
      LoadLevelList();
    }
    public void Back() {
      gameUIManager.BackPreviusPage();
    }
  }
}
using System.Collections.Generic;
using Ballance2.Utils;
using UnityEngine;

namespace Ballance2.UI.Core.Side
{
  public class SideTabBar : MonoBehaviour
  {
    public GameObject SideTabItemPrefab;

    private Dictionary<int, SideTabItem> tabItems = new Dictionary<int, SideTabItem>();
    private SideTabItem activeTab = null;

    public void AddTab(Window w) {
      if(!tabItems.ContainsKey(w.windowId)) {
        var newSideTabItem = CloneUtils.CloneNewObjectWithParent(SideTabItemPrefab, transform, "Tab" + w.name).GetComponent<SideTabItem>();
        newSideTabItem.SetWindow(w);
        tabItems.Add(w.windowId, newSideTabItem);
      }
    }
    public void ActiveTab(Window w) {
      if(activeTab != null) 
        activeTab.SetBgActive(false);

      if(tabItems.TryGetValue(w.windowId, out var tab)) {
        activeTab = tab;
        activeTab.SetBgActive(true);
      }
    }
    public void DeactiveAllTab() {
      if(activeTab != null) 
        activeTab.SetBgActive(false);
    }
    public void RemoveTab(Window w) {
      if(tabItems.TryGetValue(w.windowId, out var tab)) {
        if(activeTab == tab)
          activeTab = null;
        UnityEngine.Object.Destroy(tab.gameObject);
        tabItems.Remove(w.windowId);
      }
    }
  }
}

using System.Collections.Generic;
using AillieoUtils;
using Ballance2.Game.LevelEditor.EditorItems;
using Ballance2.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorSmallInfoControl : MonoBehaviour 
  {
    public List<GameObject> Editors;
    public Transform Pool;
    public RectTransform ContentView;

    private void Start() 
    {
      InitUI();
      UpdateUI();
    }
    private void OnDestroy() 
    {
      DestroyUI();
    }

    protected virtual LevelDynamicModelAssetConfigueItem[] GetEditItems()
    {
      return new LevelDynamicModelAssetConfigueItem[0];
    }

    private Dictionary<string, SimplePool> editorItemPool = new Dictionary<string, SimplePool>();
    private class SimplePool : SimpleObjPool<LevelEditorItemBase>
    {
      private Transform Pool;
      private GameObject Prefab;

      public SimplePool(Transform pool, GameObject prefab) : base(32)
      {
        m_ctor = OnInit;
        m_OnRecycle = OnReset;
        m_OnClear = OnReset;
        Pool = pool;
        Prefab = prefab;
      }

      public LevelEditorItemBase OnInit()
      {
        return CloneUtils.CloneNewObjectWithParentAndGetGetComponent<LevelEditorItemBase>(Prefab, Pool);
      }
      public void OnReset(LevelEditorItemBase item)
      {
        item.transform.SetParent(Pool);
      }
      public void OnClear(LevelEditorItemBase item)
      {
        Object.Destroy(item.gameObject);
      }
    }
    private Dictionary<string, List<LevelEditorItemBase>> currentShowingEditorItems = new Dictionary<string, List<LevelEditorItemBase>>();
    private HashSet<string> currentShowingEditorKeys = new HashSet<string>();

    private void DestroyUI()
    {
      foreach (var group in editorItemPool)
        group.Value.Clear();
      editorItemPool.Clear();
    }
    private void InitUI()
    {
      foreach (var item in Editors)
      {
        var comp = item.GetComponent<LevelEditorItemBase>();
        editorItemPool.Add(comp.GetEditableType(), new SimplePool(Pool, item));
      }
    }
    public void UpdateUI()
    {
      foreach (var group in currentShowingEditorItems)
        foreach (var item in group.Value)
          editorItemPool[item.GetEditableType()].Recycle(item);
      currentShowingEditorItems.Clear();
      currentShowingEditorKeys.Clear();

      var items = GetEditItems();
      foreach (var configueItem in items)
      {
        if (!currentShowingEditorKeys.Contains(configueItem.Key))
        {
          if (editorItemPool.TryGetValue(configueItem.Type, out var editors))
          {
            if (!currentShowingEditorItems.TryGetValue(configueItem.Group, out var group))
            {
              group = new List<LevelEditorItemBase>();
              var header = editorItemPool["Group"].Get();
              header.transform.SetParent(ContentView);
              header.Title.text = $"I18N:core.editor.sideedit.{configueItem.Group}";
              group.Add(header);
              currentShowingEditorItems.Add(configueItem.Group, group);
            }

            var currentConfigueItem = configueItem;
            var editor = editors.Get();
            editor.Title.text = currentConfigueItem.Name;
            editor.Params = currentConfigueItem.EditorParams;
            editor.OnValueChanged = currentConfigueItem.OnValueChanged;
            editor.OnTimingUpdateValue = null;
            if (currentConfigueItem.OnGetValue != null)
            {
              editor.UpdateValue(currentConfigueItem.OnGetValue());
              if (!currentConfigueItem.NoTimingUpdate)
                editor.OnTimingUpdateValue = () => {
                  editor.UpdateValue(currentConfigueItem.OnGetValue());
                };
              else
                editor.OnTimingUpdateValue = null;
            }
            editor.transform.SetParent(ContentView);
            currentShowingEditorKeys.Add(currentConfigueItem.Key);
            group.Add(editor);
          }
        }
      } 
    }
  }
}
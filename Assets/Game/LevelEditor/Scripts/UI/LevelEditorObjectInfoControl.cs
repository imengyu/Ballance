using System.Collections.Generic;
using AillieoUtils;
using Ballance2.Game.LevelEditor.EditorItems;
using Ballance2.Utils;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorObjectInfoControl : MonoBehaviour 
  {
    public List<GameObject> Editors;
    public Transform Pool;
    public RectTransform ContentView;

    private List<LevelDynamicModel> editingModels = new List<LevelDynamicModel>();
    private bool dirty = false;

    private void Start() 
    {
      InitUI();
      dirty = true;
    }
    private void OnDestroy() 
    {
      DestroyUI();
    }
    private void Update()
    {
      if (dirty)
      {
        dirty = false;
        UpdateUI();
      }
    }

    public void SetSelectModel(LevelDynamicModel[] dynamicModels) 
    {
      editingModels.Clear();
      editingModels.AddRange(dynamicModels);
      dirty = true;
      gameObject.SetActive(true);
    }
    public void ClearSelectModel() 
    { 
      editingModels.Clear();
      dirty = true;
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
    private List<LevelEditorCurrentShowingItem> currentShowingItems = new List<LevelEditorCurrentShowingItem>();


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
    private void UpdateItemVisible()
    {
      foreach (var item in currentShowingItems)
      {
        if (item.ConfigueItem.OnGetVisible != null)
          item.Editor.gameObject.SetActive(item.ConfigueItem.OnGetVisible());
      }
    }
    private void UpdateUI()
    {
      foreach (var group in currentShowingEditorItems)
        foreach (var item in group.Value)
          editorItemPool[item.GetEditableType()].Recycle(item);
      currentShowingEditorItems.Clear();
      currentShowingEditorKeys.Clear();

      if (editingModels.Count == 0) {
        gameObject.SetActive(false);
        return;
      }
      gameObject.SetActive(true);
      currentShowingItems.Clear();

      var isFirst = true;
      foreach (var item in editingModels)
      {
        foreach (var configueItem in item.ConfigueItemsRef.Values)
        {
          if (isFirst || !currentShowingEditorKeys.Contains(configueItem.Key))
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
              var currentItem = item;
              var editor = editors.Get();
              editor.Title.text = currentConfigueItem.Name;
              editor.Params = currentConfigueItem.EditorParams;
              editor.OnValueChanged = (v) => {
                currentConfigueItem.OnValueChanged(v);
                if (!currentConfigueItem.NoSaveToConfigues)
                  currentItem.Configues[currentConfigueItem.Key] = v;
                if (currentConfigueItem.NeedFlushVisible)
                  UpdateItemVisible();
              };
              editor.OnTimingUpdateValue = null;
              if (currentConfigueItem.OnGetVisible != null)
              {
                editor.gameObject.SetActive(currentConfigueItem.OnGetVisible());
              }
              else
              {
                editor.gameObject.SetActive(true);
              }
              if (currentConfigueItem.OnGetValue != null)
              {
                editor.UpdateValue(currentConfigueItem.OnGetValue());
                if (!currentConfigueItem.NoTimingUpdate)
                  editor.OnTimingUpdateValue = () =>
                  {
                    editor.UpdateValue(currentConfigueItem.OnGetValue());
                  };
                else
                  editor.OnTimingUpdateValue = null;
              }
              else if (currentItem.Configues.TryGetValue(currentConfigueItem.Key, out var v))
              {
                editor.UpdateValue(v);
              }
              editor.transform.SetParent(ContentView);
              currentShowingEditorKeys.Add(currentConfigueItem.Key);
              group.Add(editor);
              currentShowingItems.Add(new LevelEditorCurrentShowingItem()
              {
                Editor = editor,
                ConfigueItem = currentConfigueItem,
              });
            }
          }
        } 
        isFirst = false;
      }
    }
  }
  public struct LevelEditorCurrentShowingItem
  {
    public LevelEditorItemBase Editor;
    public LevelDynamicModelAssetConfigueItem ConfigueItem;
  }
}
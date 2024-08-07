using System;
using System.Collections;
using System.Collections.Generic;
using Ballance2.Game.GamePlay;
using Ballance2.Res;
using Ballance2.Utils;
using Battlehub.RTCommon;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{  
  public class LevelEditorObjectSelectionData : MonoBehaviour
  {
    public LevelDynamicModel LevelDynamicModel;
  }
  [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
  public class LevelDynamicModel
  {
    [JsonProperty]
    public string Name;
    [JsonProperty]
    public Vector3 Position;
    [JsonProperty]
    public Vector3 EulerAngles;
    [JsonProperty]
    public Vector3 Scale;
    [JsonProperty]
    public string Asset;
    [JsonProperty]
    public List<int> ActiveSectors = new List<int>();
    [JsonProperty]
    public Dictionary<string, object> Configues = new Dictionary<string, object>();
    [JsonProperty]
    public int Uid = 0;
    [JsonProperty]
    public int ParentUid = 0;

    public string SubObjName;
    public bool IsSubObj = false;
    public bool CanDelete = true;
    public List<LevelDynamicModel> SubModelRef = new List<LevelDynamicModel>();
    public LevelDynamicModelAssetConfigue ConfigueRef;
    public Dictionary<string, LevelDynamicModelAssetConfigueItem> ConfigueItemsRef = new Dictionary<string, LevelDynamicModelAssetConfigueItem>();
    public LevelDynamicModelAsset AssetRef;
    public GameObject InstanceHost;
    public GameObject InstanceRef;
    public ModulBase ModulRef;
    
    private MeshRenderer MeshRenderer;
    private bool IsEditor;
    public bool IsError;
    
    public void SetModulPlaceholdeMode(bool mode)
    {
      if (IsEditor)
      {
        if (mode)
        {
          MeshRenderer.material = GameStaticResourcesPool.FindStaticAssets<Material>("DefaultMaterial");
          InstanceRef.gameObject.SetActive(false);
        }
        else
        {
          MeshRenderer.material = LevelEditorManager.Instance.TransparentMaterial;
          InstanceRef.gameObject.SetActive(true);
        }
      }
    }

    public void DestroyModul() 
    {
      if (InstanceHost != null)
      {
        UnityEngine.Object.Destroy(InstanceHost);
        ConfigueItemsRef.Clear();
        InstanceRef = null;
        ModulRef = null;
        InstanceHost = null;
        MeshRenderer = null;
      }
    }
    public void InstantiateModul(Transform ScenseRoot, bool editor, bool isNew) 
    {
      if (InstanceHost != null)
        return;
      try 
      {
        IsEditor = editor;
        InstanceHost = CloneUtils.CreateEmptyObjectWithParent(ScenseRoot, Name);
        if (AssetRef != null)
        {
          InstanceRef = CloneUtils.CloneNewObjectWithParent(AssetRef.Prefab, InstanceHost.transform, Name);
          ConfigueRef = InstanceRef.GetComponent<LevelDynamicModelAssetConfigue>();
          IsError = false;
        }
        else
        {
          InstanceRef = CloneUtils.CloneNewObjectWithParent(LevelEditorManager.Instance.ErrorPrefab, InstanceHost.transform, Name);
          IsError = true;
        }
        //设置还原
        InstanceHost.transform.position = Position;
        InstanceHost.transform.eulerAngles = EulerAngles;
        InstanceHost.transform.localScale = Scale;

        //机关，添加配套的占位
        ModulRef = InstanceRef.GetComponent<ModulBase>();

        if (ConfigueRef == null)
          ConfigueRef = InstanceRef.AddComponent<LevelDynamicModelAssetConfigue>();
        ConfigueRef.OnInit(this, editor, isNew);
        var items = ConfigueRef.GetConfigueItems(this);
        var modulConfig = ConfigueRef.GetModulConfigue(this);
        foreach (var item in items)
          ConfigueItemsRef.Add(item.Key, item);
        if (modulConfig.NeedActiveSector)
        {
          ConfigueItemsRef.Add("ActiveSectors", new LevelDynamicModelAssetConfigueItem() {
            Name = "I18N:core.editor.sideedit.props.ActiveSectors",
            Key = "ActiveSectors",
            Type = "SectorsEditor",
            Group = "Extra",
            EditorParams = new Dictionary<string, bool>() {
              { "singleSelect", modulConfig.ActiveSectorSingle },
              { "disableSelect", modulConfig.FixedActiveSector != 0 }
            },
            OnGetValue = () => ActiveSectors,
            OnValueChanged = (v) => ActiveSectors = (List<int>)v,
          });
        }
        else if (modulConfig.FixedActiveSector != 0)
        {
          ActiveSectors.Clear();
          ActiveSectors.Add(modulConfig.FixedActiveSector);
        }

        //第一次初始化，应用相关设置
        if (Configues.Count == 0)
        {
          foreach (var item in items)
            if (item.IntitalValue != null)
              Configues.Add(item.Key, item.IntitalValue);
        }
        foreach (var item in Configues)
        {
          if (ConfigueItemsRef.TryGetValue(item.Key, out var c))
            c.OnValueChanged(item.Value);
        }
        
        //编辑器特殊处理
        if (editor)
        {
          //添加碰撞器以使其可以选择
          var meshFilter = InstanceRef.GetComponent<MeshFilter>();
          Mesh mesh = null;
          if (meshFilter != null) {
            mesh = meshFilter.sharedMesh;
            InstanceHost.AddComponent<MeshFilter>().mesh = meshFilter.sharedMesh;
            //InstanceHost.AddComponent<MeshCollider>();
          }
          else
          {
            //组合原件，没有mesh，但如果是modul，则使用Modul的占位符
            if (ModulRef != null)
            {
              //在占位符寻找一个可用的mesh
              meshFilter = InstanceHost.AddComponent<MeshFilter>();
              var innerMeshFilter = ModulRef.PlaceHolderPrefab?.GetComponent<MeshFilter>();
              if (innerMeshFilter == null && ModulRef.PlaceHolderPrefab.transform.childCount > 0)
              {
                for (int i = 0; i < ModulRef.PlaceHolderPrefab.transform.childCount; i++)
                {
                  innerMeshFilter = ModulRef.PlaceHolderPrefab.transform.GetChild(i).gameObject.GetComponent<MeshFilter>();
                  if (innerMeshFilter != null)
                    break;
                }
              }
              mesh = innerMeshFilter?.sharedMesh;
              if (mesh != null)
              {
                meshFilter.mesh = mesh;
                //InstanceHost.AddComponent<MeshCollider>();
              }
              else
              {
                Log.W("LevelDynamicModel", $"Modul {Asset} that does not set PlaceHolderPrefab, it unable to select.");
              }
            }
          }
          if (mesh != null)
          {
            MeshRenderer = InstanceHost.AddComponent<MeshRenderer>();
            MeshRenderer.material = LevelEditorManager.Instance.TransparentMaterial;
          }

          //添加浮动UI控制
          var objectScenseIcon = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<LevelEditorObjectScenseIcon>(
            LevelEditorManager.Instance.LevelEditorObjectScenseIconPrefab, 
            InstanceHost.transform
          );
          objectScenseIcon.BindCamera = LevelEditorManager.Instance.LevelEditorCamera;
          objectScenseIcon.BindCanvas.worldCamera = LevelEditorManager.Instance.LevelEditorCamera;
          objectScenseIcon.BindModel = this;
          objectScenseIcon.UpdateBindModel();

          var selectionData = InstanceHost.AddComponent<LevelEditorObjectSelectionData>();
          selectionData.LevelDynamicModel = this;

          var expose = InstanceHost.AddComponent<ExposeToEditor>();
          expose.CanDelete = CanDelete;
        }
      } 
      catch (Exception e)
      {
        Log.E("LevelDynamicModel:" + Name, "InstantiateModul failed: " + e.ToString());
      }
    }

    public void Save()
    {
      Name = InstanceHost.name;
      Position = InstanceHost.transform.position;
      EulerAngles = InstanceHost.transform.eulerAngles;
      Scale = InstanceHost.transform.localScale;
    }

  }
}

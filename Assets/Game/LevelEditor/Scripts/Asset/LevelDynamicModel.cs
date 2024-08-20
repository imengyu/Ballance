using System;
using System.Collections;
using System.Collections.Generic;
using Ballance2.Game.GamePlay;
using Ballance2.Res;
using Ballance2.Utils;
using Battlehub.RTCommon;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem.HID;

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
    [JsonProperty("Configues")]
    private Dictionary<string, object> ConfiguesSave = new Dictionary<string, object>();
    [JsonIgnore]
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
    public LevelEditorObjectScenseIcon ObjectScenseIconRef;
    public int SelectFlag = 0;
    
    private MeshRenderer MeshRenderer;
    private bool IsEditor;
    public bool IsError;

    public void SetNameTagVisible(bool show)
    {
      ObjectScenseIconRef.gameObject.SetActive(show && LevelEditorManager.Instance.ShowNameTag);
    }
    public void SetModulPlaceholdeMode(bool mode)
    {
      if (IsEditor)
      {
        SetNameTagVisible(mode);
        if (mode)
        {
          if (MeshRenderer != null && !AssetRef.HiddenPlaceholderRender)
            MeshRenderer.enabled = true;
        }
        else
        {
          if (MeshRenderer != null)
            MeshRenderer.enabled = false;
        }
      }
    }
    public LevelDynamicModel GetSubModel(string name) 
    {
      foreach (var item in SubModelRef)
      {
        if (item.SubObjName == name)
          return item;
      }
      return null;
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

        if (IsEditor)
          DestroyModulEditorPart();
      }
    }

    public void ReInstantiateModul(Transform ScenseRoot, bool editor)
    {
      DestroyModul();
      InstantiateModul(ScenseRoot, editor, false);
    }
    public void InstantiateModul(Transform ScenseRoot, bool editor, bool isNew)
    {
      if (InstanceHost != null)
        return;
      try 
      {
        IsEditor = editor;
        InstanceHost = CloneUtils.CreateEmptyObjectWithParent(ScenseRoot, Name);
        if (AssetRef != null && AssetRef.Loaded && AssetRef.Prefab != null)
        {
          InstanceRef = CloneUtils.CloneNewObjectWithParent(AssetRef.Prefab, InstanceHost.transform, Name);
          ConfigueRef = InstanceRef.GetComponent<LevelDynamicModelAssetConfigue>();
          if (!InstanceRef.activeSelf)
            InstanceRef.SetActive(true);
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

        //配置相关
        if (ConfigueRef == null)
          ConfigueRef = InstanceRef.AddComponent<LevelDynamicModelAssetConfigue>();
        Load();
        ConfigueRef.OnInit(this, editor, isNew);
        if (editor && isNew)
          ConfigueRef.OnEditorAdd(this);
        var modulConfig = ConfigueRef.GetModulConfigue(this);
        var items = ConfigueRef.GetConfigueItems(this, modulConfig);
        foreach (var item in items)
          ConfigueItemsRef.Add(item.Key, item);
        if (modulConfig.FixedActiveSector != 0)
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
            if (!c.NoIntitalUpdate)
              c.OnValueChanged(item.Value);
        }

        //编辑器特殊处理
        if (editor)
          InstantiateModulEditorPart();

        //初始化完成事件
        ConfigueRef.OnAfterInit(this, editor, isNew);
      } 
      catch (Exception e)
      {
        Log.E("LevelDynamicModel:" + Name, "InstantiateModul failed: " + e.ToString());
      }
    }

    private Mesh FixMesh(Vector3 trans, Vector3 rot, Vector3 scale, Mesh orginalMesh)
    {
      if (rot == Vector3.zero) 
        return orginalMesh;
      var mesh = new Mesh();
      var rotationMatrix = Matrix4x4.Translate(trans) * Matrix4x4.Scale(scale) * Matrix4x4.Rotate(Quaternion.Euler(rot));
      var vertices = new List<Vector3>();
      var normals = new List<Vector3>();
      foreach (var vertex in orginalMesh.vertices)
        vertices.Add(rotationMatrix.MultiplyPoint(vertex));
      foreach (var normal in orginalMesh.normals)
        normals.Add(rotationMatrix.MultiplyVector(normal));

      mesh.vertices = vertices.ToArray();
      mesh.normals = normals.ToArray();
      mesh.triangles = orginalMesh.triangles;
      mesh.uv = orginalMesh.uv;
      return mesh;
    }

    private void DestroyModulEditorPart()
    {
    }
    private void InstantiateModulEditorPart()
    {
      //添加碰撞器以使其可以选择
      var meshFilter = InstanceRef.GetComponent<MeshFilter>();
      if (meshFilter != null && meshFilter.sharedMesh != null)
        InstanceHost.AddComponent<MeshFilter>().mesh = meshFilter.sharedMesh;
      else
      {
        Mesh mesh = null;
        //在占位符寻找一个可用的mesh
        if (meshFilter == null)
          meshFilter = InstanceHost.AddComponent<MeshFilter>();
        //组合原件，没有mesh，但如果是modul，则使用Modul的占位符
        if (ModulRef != null)
        {
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
          //只有机关渲染占位符
          if (mesh != null)
          {
            MeshRenderer = InstanceHost.AddComponent<MeshRenderer>();
            MeshRenderer.material = LevelEditorManager.Instance.TransparentMaterial;
            MeshRenderer.enabled = !AssetRef.HiddenPlaceholderRender;
          }
        }
        else
        {
          //非modul，则尝试使用第一个子对象的mesh
          for (int i = 0; i < InstanceRef.transform.childCount; i++)
          {
            var child = InstanceRef.transform.GetChild(i);
            var innerMeshFilter = child.gameObject.GetComponent<MeshFilter>();
            if (innerMeshFilter != null && innerMeshFilter.sharedMesh != null)
            {
              mesh = FixMesh(child.localPosition, child.localEulerAngles, child.lossyScale, innerMeshFilter.sharedMesh);
              break;
            }
          }
        }

        if (mesh != null)
          meshFilter.mesh = mesh;
        else
          Log.W("LevelDynamicModel", $"Modul {Asset} that does not set PlaceHolderPrefab, it unable to select.");
      }

      //添加浮动UI控制
      ObjectScenseIconRef = CloneUtils.CloneNewObjectWithParentAndGetGetComponent<LevelEditorObjectScenseIcon>(
        LevelEditorManager.Instance.LevelEditorObjectScenseIconPrefab,
        InstanceHost.transform,
        "ObjectScenseIcon"
      );
      ObjectScenseIconRef.BindCamera = LevelEditorManager.Instance.LevelEditorCamera;
      ObjectScenseIconRef.BindCanvas.worldCamera = LevelEditorManager.Instance.LevelEditorCamera;
      ObjectScenseIconRef.BindModel = this;
      ObjectScenseIconRef.UpdateBindModel();
      SetNameTagVisible(true);

      var selectionData = InstanceHost.AddComponent<LevelEditorObjectSelectionData>();
      selectionData.LevelDynamicModel = this;

      var expose = InstanceHost.AddComponent<ExposeToEditor>();
      expose.CanDelete = CanDelete;

      //机关切换至预览模式
      if (ModulRef != null)
        ModulRef.ActiveForPreview();
    }

    public void Load()
    {
      Configues.Clear();
      foreach (var item in ConfiguesSave)
        Configues.Add(item.Key, ConfigueRef.OnConfigueLoad(item.Key, item.Value));
    }
    public void Save()
    {
      Name = InstanceHost.name;
      Position = InstanceHost.transform.position;
      EulerAngles = InstanceHost.transform.eulerAngles;
      Scale = InstanceHost.transform.localScale;

      ConfiguesSave.Clear();
      foreach (var item in Configues)
        ConfiguesSave.Add(item.Key, ConfigueRef.OnConfigueSave(item.Key, item.Value));
    }

  }
}

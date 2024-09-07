
using Ballance2.Base;
using Ballance2.Game.LevelBuilder;
using Ballance2.Game.LevelEditor.Exceptions;
using Ballance2.Services;
using Ballance2.Services.I18N;
using Ballance2.Utils;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicLoader : GameSingletonBehavior<LevelDynamicLoader>
  {
    public const string TAG = "LevelDynamicLoader"; 

    public List<Texture2D> InternalTextures = new List<Texture2D>();
    public List<Material> InternalMaterials = new List<Material>();

    private Dictionary<string, Texture2D> InternalTexturesMap = new Dictionary<string, Texture2D>();
    private Dictionary<string, Material> InternalMaterialsMap = new Dictionary<string, Material>();

    private Transform DynamicModelRoot;

    private void Awake()
    {
      DynamicModelRoot = CloneUtils.CreateEmptyObjectWithParent(transform, "DynamicModelRoot").transform;
      foreach (var tex in InternalTextures)
        InternalTexturesMap.Add(tex.name, tex);
      foreach (var mat in InternalMaterials)
        InternalMaterialsMap.Add(mat.name, mat);
    }

    public Texture2D IsInterinalTexture(string name)
    {
      if (InternalTexturesMap.TryGetValue(name, out var tex))
        return tex;
      return null;
    }
    public Material IsInterinalMaterial(string name)
    {
      if (InternalMaterialsMap.TryGetValue(name, out var mat))
        return mat;
      return null;
    }

    public class LevelDynamicLoaderResult
    {
      public bool Success = false;
      public string Error = "";
    }

    public void DestroyAllTempDynamicAsset()
    {
      DynamicModelRoot.DestroyAllChildren();
    }

    public IEnumerator LoadAsset(LevelDynamicLoaderResult result, LevelDynamicAssembe level, LevelDynamicModelAsset asset)
    {
      Log.D(TAG, $"Load asset {asset.SourcePath}");
      SaveableObjectRoot sroot = null;

      yield return new WaitForSeconds(0.1f);

      //读取文件
      if (asset.SourceType == LevelDynamicModelSource.Embed && asset.SourcePath.StartsWith("levelasset:"))
      {
        var sourceName = asset.SourcePath.Substring(11);
        //从ZIP文件中读取
        if (level.IsZip)
        {
          ZipInputStream zip;
          try
          {
            zip = ZipUtils.OpenZipFile(level.LevelDirPath);
          }
          catch (Exception e)
          {
            HandleExceptionAndGetMessages(e, result);
            yield break;
          }

          try
          {
            sourceName = $"assets/{sourceName}.bmodel";
            ZipEntry theEntry;
            while ((theEntry = zip.GetNextEntry()) != null)
            {
              if (ZipUtils.MatchRootName(sourceName, theEntry))
              {
                var task = ZipUtils.ReadZipFileToMemoryAsync(zip);
                yield return task.AsIEnumerator();
                var ms = task.Result;

                try
                {
                  ms.Seek(0, SeekOrigin.Begin);
                  sroot = Serializer.Deserialize<SaveableObjectRoot>(ms);
                  break;
                }
                catch (Exception e)
                {
                  HandleExceptionAndGetMessages(e, result);
                  yield break;
                }
                finally
                {
                  ms.Close();
                  ms.Dispose();
                }
              }
            }
          }
          finally
          {
            zip.Close();
            zip.Dispose();
          }
        }
        //从文件直接读取
        else
        {
          var path = $"{level.LevelDirPath}/assets/{sourceName}.bmodel";
          if (!File.Exists(path))
          {
            result.Error = I18N.Tr("core.editor.messages.FileMissing") + " " + path;
            yield break;
          }
          FileStream fs;
          try
          {
            fs = new FileStream(path, FileMode.Open, FileAccess.Read);
          }
          catch (Exception e)
          {
            HandleExceptionAndGetMessages(e, result);
            yield break;
          }

          try
          {
            sroot = Serializer.Deserialize<SaveableObjectRoot>(fs);
          }
          catch (Exception e)
          {
            HandleExceptionAndGetMessages(e, result);
            yield break;
          }
          finally
          {
            fs.Close();
            fs.Dispose();
          }
        }
      }

      yield return new WaitForSeconds(0.1f);

      if (sroot == null)
      {
        result.Error = I18N.Tr("core.editor.messages.FileMissing") + " " + asset.Name;
        yield break;
      }
      if (sroot.childs == null)
      {
        result.Error = I18N.Tr("core.editor.messages.ModelBroken") + " " + asset.Name;
        yield break;
      }

      //创建Prefab
      try
      {
        GameObject prefab = new GameObject(sroot.name);
        prefab.SetActive(false);
        prefab.transform.SetParent(DynamicModelRoot);
        prefab.transform.localScale = new Vector3(sroot.scale, sroot.scale, sroot.scale);
        foreach (var child in sroot.childs)
        {
          GameObject ochild = new GameObject(child.name);
          ochild.transform.SetParent(prefab.transform);
          ochild.transform.localPosition = child.position.Vector3;
          ochild.transform.localEulerAngles = child.rotation.Vector3;
          ochild.transform.localScale = child.scale.Vector3;

          if (child.mesh.isNull == 0)
          {
            var meshFilter = ochild.AddComponent<MeshFilter>();
            meshFilter.mesh = child.mesh.Mesh;

            if (child.mats.Count > 0)
            {
              var mats = new List<Material>();
              var meshRenderer = ochild.AddComponent<MeshRenderer>();
              foreach (var mat in child.mats)
                mats.Add(mat.Material);
              meshRenderer.materials = mats.ToArray();
            }
          }
        }

        asset.Loaded = true;
        asset.Prefab = prefab;
      }
      catch (Exception e)
      {
        HandleExceptionAndGetMessages(e, result);
        yield break;
      }

      result.Success = true;
    }
    public IEnumerator SaveAsset(LevelDynamicLoaderResult result, LevelDynamicModelAsset asset, GameObject root, bool reMapMat, float scale)
    {
      Log.D(TAG, $"Save asset {asset.Name}");
      var level = LevelEditorManager.Instance.LevelCurrent;
      var sroot = new SaveableObjectRoot();
      try
      {
        if (level.IsZip)
          throw new LevelDynamicLoaderPackLevelAlreadyPackedException();

        sroot.name = root.name;
        sroot.reMapMat = reMapMat;
        sroot.scale = scale;
        sroot.childs = new List<SaveableObject>();
        //序列化对象
        for (int i = 0; i < root.transform.childCount; i++)
        {
          var child = root.transform.GetChild(i);
          var schild = new SaveableObject();

          schild.position = new SaveableVec3(child.localPosition);
          schild.rotation = new SaveableVec3(child.localEulerAngles);
          schild.scale = new SaveableVec3(child.localScale);
          schild.name = child.gameObject.name;
          schild.mats = new List<SaveableMat>();

          var meshFilter = child.gameObject.GetComponent<MeshFilter>();
          if (meshFilter != null)
            schild.mesh = new SaveableMesh(meshFilter.mesh);
          else
            schild.mesh = new SaveableMesh(null);

          var meshRenderer = child.gameObject.GetComponent<MeshRenderer>();
          if (meshRenderer != null)
          {
            foreach (var mat in meshRenderer.materials)
              schild.mats.Add(new SaveableMat(mat, reMapMat));
          }

          sroot.childs.Add(schild);
        }
      }
      catch (Exception e)
      {
        HandleExceptionAndGetMessages(e, result);
        yield break;
      }
      yield return new WaitForSeconds(0.1f);

      //保存文件
      try
      {

        var sourceName = StringUtils.RemoveSpeicalChars(asset.Name);
        var sourcePath = $"levelasset:{sourceName}";
        var path = level.LevelDirPath + "/assets/" + sourceName + ".bmodel";
        var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

        Log.D(TAG, $"Save asset to {path}");

        Serializer.Serialize(fs, sroot);
        fs.Close();
        fs.Dispose();
        asset.SourcePath = sourcePath;
        asset.SourceType = LevelDynamicModelSource.Embed;
        //加入关卡资源中
        level.LevelData.LevelAssets.Add(asset);
      }
      catch (Exception e)
      {
        HandleExceptionAndGetMessages(e, result);
        yield break;
      }

      result.Success = true;
    }

    public void DeleteAsset(LevelDynamicLoaderResult result, LevelDynamicModelAsset asset)
    {
      Log.D(TAG, $"Delete asset {asset.SourcePath}");
      try
      {
        var level = LevelEditorManager.Instance.LevelCurrent;
        if (level.IsZip)
          throw new LevelDynamicLoaderPackLevelAlreadyPackedException();
        if (asset.SourceType == LevelDynamicModelSource.Embed && asset.SourcePath.StartsWith("levelasset:"))
        {
          var sourceName = asset.SourcePath.Substring(11);
          var path = level.LevelDirPath + "/assets/" + sourceName;
          if (File.Exists(path))
            File.Delete(path);
        }
        result.Success = true;
      }
      catch (Exception e)
      {
        HandleExceptionAndGetMessages(e, result);
      }
    }
    public IEnumerator PackLevel(LevelDynamicLoaderResult result, LevelDynamicAssembe level, string logoImage, string previewImage)
    {
      Log.D(TAG, $"Start pack level {level.LevelDirPath}");

      var levelDir = level.LevelDirPath;
      if (level.IsZip)
        throw new LevelDynamicLoaderPackLevelAlreadyPackedException();
      if (!Directory.Exists(levelDir))
        throw new DirectoryNotFoundException();

      var outDir = $"{levelDir}/output";
      var task = level.Save();
      yield return task.AsIEnumerator();

      try
      {
        if (!Directory.Exists(outDir))
        {
          Log.D(TAG, $"Create output dir {outDir}");
          Directory.CreateDirectory(outDir);
        }
      }
      catch (Exception e)
      {
        HandleExceptionAndGetMessages(e, result);
        yield break;
      }

      ZipOutputStream zip = null;
      try
      {
        var crc32 = new Crc32();
        var path = outDir + "/upload.blevel";
        Log.D(TAG, $"Output file to {path}");

        zip = ZipUtils.CreateZipFile(path);

        ZipUtils.AddFileToZip(zip, $"{levelDir}/assets.json", levelDir.Length, ref crc32);
        ZipUtils.AddFileToZip(zip, $"{levelDir}/Level.json", levelDir.Length, ref crc32);
        ZipUtils.AddDirFileToZip(zip, levelDir, $"{levelDir}/assets", "*", ref crc32);
        if (!string.IsNullOrEmpty(logoImage))
          ZipUtils.AddFileToZip(zip, logoImage, "/LevelLogo.png", ref crc32);
        if (!string.IsNullOrEmpty(previewImage))
          ZipUtils.AddFileToZip(zip, previewImage, "/LevelPreview.png", ref crc32);
      }
      catch (Exception e)
      {
        HandleExceptionAndGetMessages(e, result);
        yield break;
      }
      finally
      {
        if (zip != null)
        {
          zip.Close();
          zip.Dispose();
        }
      }

      result.Success = true;
    }

    public void LoadAllInternalAsset(Action<LevelDynamicModelAsset> addLevelAsset)
    {
      var internalAssets = LevelInternalAssets.Instance.LoadAll();
      foreach (var item in internalAssets)
        if (!item.HiddenInContentSelector)
          addLevelAsset(item);
      var packages = GamePackageManager.Instance.GetLoadedPackages();
      if (packages != null)
      {
        foreach (var package in packages)
        {
          try
          {
            var assets = package.PackageEntry.OnLevelEditorLoadAssets?.Invoke(package);
            if (assets != null)
            {
              foreach (var item in assets)
                addLevelAsset(new LevelDynamicModelAsset(item, package));
            }
          }
          catch (Exception e)
          {
            Log.E("LevelEditorManager.LoadAllAssets", "Failed load assets for mod " + package.PackageName + " :" + e.ToString());
          }
        }
      }
    }

    public IEnumerator LoadLevel(
      LevelDynamicLoaderResult result, 
      LevelDynamicAssembe level, 
      Transform instaceRoot, 
      Dictionary<string, LevelDynamicModelAsset> LevelAssets,
      Action<LevelDynamicModelAsset> addLevelAsset,
      bool fromEditor
    )
    {
      Log.D(TAG, $"Start load level {level.LevelDirPath}");

      var task = level.Load();
      yield return task.AsIEnumerator();

      if (!task.Result.Success)
      {
        result.Error = task.Result.Error;
        yield break;
      }
      yield return new WaitForSeconds(1);

      var loadCount = 0;
      var loadMissingAssets = new List<LevelDynamicModel>();

      //加载内嵌资源至系统中
      foreach (var item in level.LevelData.LevelAssets)
      {
        if (item.SourceType == LevelDynamicModelSource.Embed)
          addLevelAsset(item);
        loadCount++;
        if (loadCount % 8 == 0)
          yield return new WaitForEndOfFrame();
      }

      Log.D(TAG, $"Load objects stage 1");

      //加载资源
      foreach (var item in level.LevelData.LevelModels)
      {
        if (LevelAssets.TryGetValue(item.Asset, out var asset))
        {
          item.AssetRef = asset;
          item.CanDelete = asset.CanDelete;

          //如果对象没有加载，则现在加载
          if (!asset.Loaded && asset.SourceType == LevelDynamicModelSource.Embed)
          {
            var result2 = new LevelDynamicLoaderResult();
            yield return StartCoroutine(LoadAsset(result2, level, asset));
            if (!result2.Success)
              Log.E(TAG, "Failed to load asset {0}, error: {1}.", item.Asset, result2.Error);
          }
        }
        else
        {
          loadMissingAssets.Add(item);
        }
        loadCount++;
        if (loadCount % 16 == 0)
          yield return new WaitForEndOfFrame();
      }

      Log.D(TAG, $"Load objects report : {loadMissingAssets.Count} missing.");

      //分类父子关系
      var modelsTempMap = new Dictionary<int, LevelDynamicModel>();
      foreach (var item in level.LevelData.LevelModels)
      {
        if (!modelsTempMap.ContainsKey(item.Uid)) 
          modelsTempMap.Add(item.Uid, item);
      }
      foreach (var item in level.LevelData.LevelModels)
      {
        if (item.ParentUid != 0 && modelsTempMap.TryGetValue(item.ParentUid, out var parentModel))
        {
          parentModel.SubModelRef.Add(item);
          item.IsSubObj = true;
          item.CanDelete = false;
        }
      }

      Log.D(TAG, $"Load objects stage 2");

      //加载实例
      foreach (var item in level.LevelData.LevelModels)
      {
        item.InstantiateModul(instaceRoot, fromEditor, false);
        loadCount++;
        if (loadCount % 8 == 0)
          yield return new WaitForEndOfFrame();
      }
      //子模型层级修改
      foreach (var item in level.LevelData.LevelModels)
      {
        if (item.SubModelRef.Count > 0)
        {
          for (int i = 0; i < item.SubModelRef.Count; i++)
          {
            var subItem = item.SubModelRef[i];
            subItem.InstanceHost.transform.SetParent(item.InstanceHost.transform);
            subItem.SubObjName = subItem.AssetRef.ObjName;
          }
        }
      }
      modelsTempMap.Clear();

      yield return new WaitForEndOfFrame();

      Log.D(TAG, $"Load objects done. All {loadCount} objects");

      result.Success = true;
      yield break;
    }

    /// <summary>
    /// 通过异常获取错误信息
    /// </summary>
    /// <param name="e"></param>
    /// <param name="result"></param>
    public static void HandleExceptionAndGetMessages(Exception e, LevelDynamicLoaderResult result)
    {
      Debug.LogException(e);
      if (e is IOException)
      {
        result.Success = false;
        result.Error = I18N.TrF("core.editor.messages.IOException", "", e.Message);
      }
      else if (e is UnauthorizedAccessException)
      {
        result.Success = false;
        result.Error = I18N.Tr("core.editor.messages.AccessFileFailed");
      }
      else if (e is PathTooLongException)
      {
        result.Success = false;
        result.Error = I18N.Tr("core.editor.messages.PathTooLongException");
      }
      else if (e is DirectoryNotFoundException || e is FileNotFoundException)
      {
        result.Success = false;
        result.Error = I18N.Tr("core.editor.messages.FileMissing") + " " + e.Message;
      }
      else if (e is LevelDynamicLoaderPackLevelAlreadyPackedException)
      {
        result.Success = false;
        result.Error = I18N.Tr("core.editor.messages.LevelAlreadyPacked");
      }
      else
      {
        result.Success = false;
        result.Error = I18N.TrF("core.editor.messages.UnknowException", "", e.ToString());
      }
    }

    /**
     * obj
     *    mesh
     *    material
     *       texture
     */

    [ProtoContract]
    private class SaveableObjectRoot
    {
      [ProtoMember(1)]
      public string name { get; set; }
      [ProtoMember(2)]
      public bool reMapMat { get; set; }
      [ProtoMember(3)]
      public float scale { get; set; }
      [ProtoMember(4)]
      public List<SaveableObject> childs { get; set; }
    }
    [ProtoContract]
    private class SaveableObject
    {
      [ProtoMember(1)]
      public string name { get; set; }
      [ProtoMember(2)]
      public SaveableVec3 position { get; set; }
      [ProtoMember(3)]
      public SaveableVec3 rotation { get; set; }
      [ProtoMember(4)]
      public SaveableVec3 scale { get; set; }
      [ProtoMember(5)]
      public SaveableMesh mesh { get; set; }
      [ProtoMember(6)]
      public List<SaveableMat> mats { get; set; }
      [ProtoMember(7)]
      public List<SaveableObject> childs { get; set; } = new List<SaveableObject>();
    }
    [ProtoContract]
    private class SaveableMat
    {
      [ProtoMember(1)]
      public string name { get; set; } = "";
      [ProtoMember(2)]
      public SaveableVec4 _Color { get; set; } = new SaveableVec4();
      [ProtoMember(3)]
      public SaveableTex _MainTex { get; set; } = new SaveableTex(null);
      [ProtoMember(4)]
      public float _Glossiness { get; set; } = 0;
      [ProtoMember(5)]
      public float _Metallic { get; set; } = 0;
      [ProtoMember(6)]
      public float _BumpScale { get; set; } = 0;
      [ProtoMember(7)]
      public SaveableTex _BumpMap { get; set; } = new SaveableTex(null);
      [ProtoMember(8)]
      public SaveableVec4 _EmissionColor { get; set; } = new SaveableVec4();
      [ProtoMember(9)]
      public SaveableTex _DetailAlbedoMap { get; set; } = new SaveableTex(null);
      [ProtoMember(10)]
      public float _DetailNormalMapScale { get; set; } = 0;
      [ProtoMember(11)]
      public SaveableTex _DetailNormalMap { get; set; } = new SaveableTex(null);
      [ProtoMember(12)]
      public int isNull { get; set; }
      [ProtoMember(13)]
      public int isInternal { get; set; }

      public Material Material
      {
        get
        {
          if (isNull == 1) 
            return null;
          if (isInternal == 1)
            return Instance.IsInterinalMaterial(name);
          var mat = new Material(Shader.Find("Standard"));
          mat.name = name;
          mat.SetVector("_Color", _Color.Color);
          mat.SetTexture("_MainTex", _MainTex.Texture2D);
          mat.SetFloat("_Glossiness", _Glossiness);
          mat.SetFloat("_Metallic", _Metallic);
          mat.SetFloat("_BumpScale", _BumpScale);
          mat.SetTexture("_BumpMap", _BumpMap.Texture2D);
          mat.SetVector("_EmissionColor", _EmissionColor.Color);
          mat.SetTexture("_DetailAlbedoMap", _DetailAlbedoMap.Texture2D);
          mat.SetFloat("_DetailNormalMapScale", _DetailNormalMapScale);
          mat.SetTexture("_DetailNormalMap", _DetailNormalMap.Texture2D);
          return mat;
        }
      }

      public SaveableMat()
      {
      }
      public SaveableMat(Material mat, bool reMapMat)
      {
        if (mat != null && mat.shader.name == "Standard")
        {
          isNull = 0;
          isInternal = 0;
          name = mat.name;
          if (reMapMat && Instance.IsInterinalMaterial(mat.name) != null)
          {
            isInternal = 1;
            return;
          }
          _Color = new SaveableVec4(mat.GetVector("_Color"));
          _MainTex = new SaveableTex(mat.GetTexture("_MainTex"));
          _Glossiness = mat.GetFloat("_Glossiness");
          _Metallic = mat.GetFloat("_Metallic");
          _BumpScale = mat.GetFloat("_BumpScale");
          _BumpMap = new SaveableTex(mat.GetTexture("_BumpMap"));
          _EmissionColor = new SaveableVec4(mat.GetVector("_EmissionColor"));
          _DetailAlbedoMap = new SaveableTex(mat.GetTexture("_DetailAlbedoMap"));
          _DetailNormalMapScale = mat.GetFloat("_DetailNormalMapScale");
          _DetailNormalMap = new SaveableTex(mat.GetTexture("_DetailNormalMap"));
        }
      }
    }
    [ProtoContract]
    private class SaveableTex
    {
      [ProtoMember(1)]
      public string name { get; set; } = "";
      [ProtoMember(2)]
      public int width { get; set; } = 0;
      [ProtoMember(3)]
      public int height { get; set; } = 0;
      [ProtoMember(4)]
      public List<byte> data { get; set; } = new List<byte>();
      [ProtoMember(5)]
      public int isNull { get; set; }
      [ProtoMember(6)]
      public int isInternal { get; set; }
      [ProtoMember(7)]
      public int wrapMode { get; set; }

      public Texture2D Texture2D
      {
        get
        {
          if (isInternal == 1)
            return Instance.IsInterinalTexture(name);
          if (isNull == 1 || width == 0 || height == 0)
            return null;
          var tex = new Texture2D(width, height, TextureFormat.RGBA32, true);
          tex.wrapMode = (TextureWrapMode)wrapMode;
          var color32 = new List<Color32>();
          for (int i = 0; i < data.Count; i += 4)
          {
            color32.Add(new Color32(
              data[i],
              data[i + 1],
              data[i + 2],
              data[i + 3]
            ));
          }
          tex.SetPixels32(color32.ToArray());
          tex.Apply();
          return tex;
        }
      }

      public SaveableTex()
      {
      }
      public SaveableTex(Texture _tex)
      {
        if (_tex != null && _tex is Texture2D)
        {
          var tex = _tex as Texture2D;
          name = tex.name;
          isNull = 0;
          isInternal = 0;
          if (Instance.IsInterinalTexture(tex.name) != null)
          {
            isInternal = 1;
            return;
          }
          width = tex.width;
          height = tex.height;
          wrapMode = (int)tex.wrapMode;
          if (width > 0 && height > 0 && (int)tex.format != 0)
            foreach (var tex2 in tex.GetPixels32())
            {
              data.Add(tex2.r);
              data.Add(tex2.g);
              data.Add(tex2.b);
              data.Add(tex2.a);
            }
        }
      }
    }
    [ProtoContract]
    private class SaveableMesh
    {
      [ProtoMember(1)]
      public string name { get; set; } = "";
      [ProtoMember(2)]
      public List<SaveableVec3> vertices { get; set; } = new List<SaveableVec3>();
      [ProtoMember(3)]
      public List<SaveableVec3> normals { get; set; } = new List<SaveableVec3>();
      [ProtoMember(4)]
      public List<SaveableVec2> uvs { get; set; } = new List<SaveableVec2>();
      [ProtoMember(5)]
      public List<SaveableSubMesh> submeshes { get; set; } = new List<SaveableSubMesh>();
      [ProtoMember(6)]
      public List<int> triangles { get; set; } = new List<int>();
      [ProtoMember(7)]
      public int isNull { get; set; }

      public SaveableMesh()
      {
      }
      public SaveableMesh(Mesh mesh)
      {
        name = mesh.name;
        if (mesh == null)
          return;
        else
          isNull = 0;
        foreach (var vertex in mesh.vertices)
          vertices.Add(new SaveableVec3(vertex));
        foreach (var item in mesh.normals)
          normals.Add(new SaveableVec3(item));
        foreach (var item in mesh.uv)
          uvs.Add(new SaveableVec2(item));
        foreach (var item in mesh.triangles)
          triangles.Add(item);
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
          var d = new SaveableSubMesh();
          var submesh = mesh.GetSubMesh(i);
          d.baseVertex = submesh.baseVertex;
          d.indexStart = submesh.indexStart;
          d.indexCount = submesh.indexCount;
          //不知道为什么从mesh里面拿到的indexStart会超出范围
          if (d.indexStart == mesh.triangles.Length)
            d.indexStart = mesh.triangles.Length - d.indexStart;
          submeshes.Add(d);
        }
      }

      public Mesh Mesh
      {
        get
        {
          if (isNull == 1) 
            return null;
          var result = new Mesh();
          result.name = name;

          var vertices = new List<Vector3>();
          var normals = new List<Vector3>();
          var triangles = new List<int>();
          var uv = new List<Vector2>();
          var subMeshs = new List<SubMeshDescriptor>();

          foreach (var v in this.vertices)
            vertices.Add(v.Vector3);
          foreach (var v in this.normals)
            normals.Add(v.Vector3);
          foreach (var v in this.triangles)
            triangles.Add(v);
          foreach (var v in uvs)
            uv.Add(v.Vector2);
          foreach (var v in submeshes)
            subMeshs.Add(new SubMeshDescriptor(v.indexStart, v.indexCount)
            {
              baseVertex = v.baseVertex,
            });

          result.Clear();
          result.vertices = vertices.ToArray();
          result.normals = normals.ToArray();
          result.triangles = triangles.ToArray();
          result.uv = uv.ToArray();
          result.SetSubMeshes(subMeshs.ToArray());
          result.RecalculateBounds();

          return result;
        }
      }
    }
    [ProtoContract]
    private class SaveableSubMesh
    {
      [ProtoMember(1)]
      public int indexStart { get; set; }
      [ProtoMember(2)]
      public int indexCount { get; set; }
      [ProtoMember(3)]
      public int baseVertex { get; set; }
    }
    [ProtoContract]
    private class SaveableVec2
    {
      public SaveableVec2()
      {
        x = 0;
        y = 0;
      }
      public SaveableVec2(Vector2 i)
      {
        x = i.x;
        y = i.y;
      }

      public Vector2 Vector2 => new Vector2(x, y);

      [ProtoMember(1)]
      public float x { get; set; }
      [ProtoMember(2)]
      public float y { get; set; }
    }
    [ProtoContract]
    private class SaveableVec3
    {
      public SaveableVec3()
      {
      }
      public SaveableVec3(Vector3 i)
      {
        x = i.x;
        y = i.y;
        z = i.z;
      }

      public Vector3 Vector3 => new Vector3(x, y, z);

      [ProtoMember(1)]
      public float x { get; set; }
      [ProtoMember(2)]
      public float y { get; set; }
      [ProtoMember(3)]
      public float z { get; set; } = 0;
    }
    [ProtoContract]
    private class SaveableVec4
    {
      public SaveableVec4()
      {
      }
      public SaveableVec4(Color i)
      {
        x = i.r;
        y = i.g;
        z = i.b;
        w = i.a;
      }
      public SaveableVec4(Vector4 i)
      {
        x = i.x;
        y = i.y;
        z = i.z;
        w = i.w;
      }

      public Color Color => new Color(x, y, z, w);
      public Vector4 Vector4 => new Vector4(x, y, z, w);

      [ProtoMember(1)]
      public float x { get; set; }
      [ProtoMember(2)]
      public float y { get; set; }
      [ProtoMember(3)]
      public float z { get; set; } = 0;
      [ProtoMember(4)]
      public float w { get; set; } = 0;
    }
  }
}

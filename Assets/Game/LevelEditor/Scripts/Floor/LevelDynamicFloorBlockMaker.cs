using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Ballance2;
using Ballance2.Base;
using Ballance2.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicFloorBlockMaker : GameSingletonBehavior<LevelDynamicFloorBlockMaker> 
  {
    [SerializeField]
    private SerializedDictionary<string, InputFloorSchemeDefine> inputFloorSchemes;

    /// <summary>
    /// 定义路面组合类型
    /// </summary>
    public enum InputFloorSchemeType
    {
      /// <summary>
      /// 四周圈和中心
      /// </summary>
      BorderAndCenter,
      /// <summary>
      /// 无边圈2x2
      /// </summary>
      Bordeless,
      /// <summary>
      /// 静态网格3x3
      /// </summary>
      Grid,
    }
    public enum InputFloorBlockType
    {
      SideW,
      SideE,
      SideN,
      SideS,
      SideW2,
      SideE2,
      SideN2,
      SideS2,
      CornerNW,
      CornerNE,
      CornerSW,
      CornerSE,
      Inner,
      InnerNW,
      InnerNE,
      InnerSW,
      InnerSE,
      Grid00 = 100, //3X3
      Grid01,
      Grid02,
      Grid10 = 200,
      Grid11,
      Grid12,
      Grid20 = 300,
      Grid21,
      Grid22,
    }

    [Serializable]
    public class InputFloorSchemeDefine
    {
      public InputFloorSchemeType Type = InputFloorSchemeType.BorderAndCenter;
      public List<InputFloorBlockDefine> Prefabs;
    }
    [Serializable]
    public class InputFloorBlockDefine
    {
      public InputFloorBlockType Type;
      public GameObject Prefab;
    }
    
    private void Awake() 
    {
      PrepareMesh();
      HideBlocks();
    }
    private void HideBlocks()
    {
      transform.ForeachAllChildren((child) => child.gameObject.SetActive(false));
    }
    private void PrepareMesh()
    {
      foreach (var item in inputFloorSchemes)
        FloorSchemes.Add(item.Key, new PreparedMeshGroup(item.Value));
    }

    public Dictionary<string, PreparedMeshGroup> FloorSchemes = new Dictionary<string, PreparedMeshGroup>();

    public class PreparedMeshGroupCombineByGrid
    {
      public int z;
      public int x;
      public bool left;
      public bool top;
      public bool right;
      public bool bottom;
    }
    public class PreparedMeshGroup
    {
      public PreparedMeshGroup(InputFloorSchemeDefine define)
      {
        Define = define;
        foreach (var item in define.Prefabs)
          PreparedMeshes.Add(item.Type, new PreparedMesh(item.Prefab));
      }

      public readonly InputFloorSchemeDefine Define;
      public Dictionary<InputFloorBlockType, PreparedMesh> PreparedMeshes = new Dictionary<InputFloorBlockType, PreparedMesh>();

      public Material[] CombineGetMeshMaterials()
      {
        return new List<Material>(combineMeshMats.Values).ToArray();
      }

      private Dictionary<string, Material> combineMeshMats = new Dictionary<string, Material>();
      private Dictionary<string, List<PreparedMeshCombine>> combineMeshPrepare = new Dictionary<string, List<PreparedMeshCombine>>();

      public class PointSolverResult
      {
        public Vector3 Vertex;
        public Vector3 RawVertex;
        public Vector3 Translate;
        public Vector3 Normal;
        public Vector2 UV;
      } 
      public delegate void PointSolverCallback(PointSolverResult inp);

      public void CombineMeshReset()
      {
        combineMeshMats.Clear();
        foreach (var item in combineMeshPrepare)
          item.Value.Clear();
        combineMeshPrepare.Clear();
      }
      public void CombineMeshAddByGrid(PreparedMeshGroupCombineByGrid p, Vector3 transformPos)
      {
        switch (Define.Type)
        {
          case InputFloorSchemeType.BorderAndCenter: {
            if (p.left && p.top)
              CombineMeshAdd(PreparedMeshes[InputFloorBlockType.CornerNW], transformPos);
            else if (p.left && p.bottom)
              CombineMeshAdd(PreparedMeshes[InputFloorBlockType.CornerSW], transformPos);
            else if (p.right && p.bottom)
              CombineMeshAdd(PreparedMeshes[InputFloorBlockType.CornerSE], transformPos);
            else if (p.right && p.top)
              CombineMeshAdd(PreparedMeshes[InputFloorBlockType.CornerNE], transformPos);
            else if (p.left)
              CombineMeshAdd(PreparedMeshes[InputFloorBlockType.SideW], transformPos);
            else if (p.top)
              CombineMeshAdd(PreparedMeshes[InputFloorBlockType.SideN], transformPos);
            else if (p.right)
              CombineMeshAdd(PreparedMeshes[InputFloorBlockType.SideE], transformPos);
            else if (p.bottom)
              CombineMeshAdd(PreparedMeshes[InputFloorBlockType.SideS], transformPos);
            else
              CombineMeshAdd(PreparedMeshes[InputFloorBlockType.Inner], transformPos);
            break;
          }
          case InputFloorSchemeType.Bordeless: {
            var x2 = p.x % 2 == 0;
            var z2 = p.z % 2 == 0;
            if (x2 && z2)
              CombineMeshAdd(PreparedMeshes[InputFloorBlockType.InnerNE], transformPos);
            else if (x2)
              CombineMeshAdd(PreparedMeshes[InputFloorBlockType.InnerSE], transformPos);
            else if (z2)
              CombineMeshAdd(PreparedMeshes[InputFloorBlockType.InnerNW], transformPos);
            else
              CombineMeshAdd(PreparedMeshes[InputFloorBlockType.InnerSW], transformPos);
            break;
          }
          case InputFloorSchemeType.Grid: {
            CombineMeshAdd(PreparedMeshes[(InputFloorBlockType)((p.x + 1) * 100 + p.z)], transformPos);
            break;
          }
          default:
            break;
        }
        
       
      }
      public void CombineMeshAdd(PreparedMesh mesh, Vector3 transformPos)
      {
        foreach (var item in mesh.prepareSubMeshes)
        {
          if (!combineMeshPrepare.TryGetValue(item.Name, out var combineMeshPrepareSub))
          {
            combineMeshMats[item.Name] = item.Material;
            combineMeshPrepareSub = new List<PreparedMeshCombine>();
            combineMeshPrepare.Add(item.Name, combineMeshPrepareSub);
          }

          combineMeshPrepareSub.Add(new PreparedMeshCombine() {
            Translate = transformPos,
            MeshSub = item
          });
        }
      }
      public void CombineMeshFinish(Mesh result, Vector3 modelRotate, PointSolverCallback pointSolver = null, bool reverseTrangles = false)
      {
        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var triangles = new List<int>();
        var uv = new List<Vector2>();
        var subMeshs = new List<SubMeshDescriptor>();

        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(modelRotate));

        foreach (var subGroup in combineMeshPrepare)
        {
          var indexStart = triangles.Count;
          var indexCount = 0;
          var vertexStart = vertices.Count;
          var vertexCount = 0;
          var indexOffset = vertexStart;

          foreach (var combine in subGroup.Value)
          {
            if (pointSolver == null)
            {
              foreach (var item in combine.MeshSub.Vertices)
                vertices.Add(rotationMatrix.MultiplyPoint(item) + combine.Translate);
              foreach (var item in combine.MeshSub.Normals)
                normals.Add(rotationMatrix.MultiplyVector(item));
              uv.AddRange(combine.MeshSub.UV);
            }
            else
            {
              PointSolverResult pointSolverResult = new PointSolverResult();
              for (var i = 0; i < combine.MeshSub.Vertices.Count; i++)
              {
                pointSolverResult.RawVertex = rotationMatrix.MultiplyPoint(combine.MeshSub.Vertices[i]);
                pointSolverResult.Translate = combine.Translate;
                pointSolverResult.Vertex = pointSolverResult.RawVertex + pointSolverResult.Translate;
                pointSolverResult.Normal = rotationMatrix.MultiplyVector(combine.MeshSub.Normals[i]);
                pointSolverResult.UV = combine.MeshSub.UV[i];

                pointSolver(pointSolverResult);
                vertices.Add(pointSolverResult.Vertex);
                normals.Add(pointSolverResult.Normal);
                uv.Add(pointSolverResult.UV);
              }
            }

            vertexCount += combine.MeshSub.Vertices.Count;

            if (reverseTrangles)
            {
              for (var i = 0; i < combine.MeshSub.Triangles.Count; i += 3)
              {
                triangles.Add(combine.MeshSub.Triangles[i + 2] + indexOffset);
                triangles.Add(combine.MeshSub.Triangles[i + 1] + indexOffset);
                triangles.Add(combine.MeshSub.Triangles[i] + indexOffset);
              }
            }
            else
            {
              foreach (var item in combine.MeshSub.Triangles)
                triangles.Add(item + indexOffset);
            }
            indexCount += combine.MeshSub.Triangles.Count;
            indexOffset += combine.MeshSub.Vertices.Count;
          }
        
          subMeshs.Add(new SubMeshDescriptor() {
            indexStart = indexStart,
            indexCount = indexCount,
            topology = MeshTopology.Triangles,
            baseVertex = 0,
            firstVertex = vertexStart,
            vertexCount = vertexCount,
          });
        }

        result.Clear();
        result.vertices = vertices.ToArray();
        result.normals = normals.ToArray();
        result.triangles = triangles.ToArray();
        result.uv = uv.ToArray();
        result.SetSubMeshes(subMeshs.ToArray());
        result.RecalculateNormals();
        result.RecalculateBounds();
      }
    }
    public class PreparedMeshCombine
    {
      public PreparedMeshSub MeshSub;
      public Vector3 Translate;
    }
    public class PreparedMeshSub
    {
      public string Name;
      public Material Material;
      public List<int> Triangles = new List<int>();
      public List<Vector3> Vertices = new List<Vector3>();
      public List<Vector3> Normals = new List<Vector3>();
      public List<Vector2> UV = new List<Vector2>();
    }
    public class PreparedMesh
    {
      public PreparedMesh(GameObject obj)
      {
        var render = obj.GetComponent<MeshRenderer>();
        var mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        for (var i = 0; i < mesh.subMeshCount; i++)
        {
          var sub = new PreparedMeshSub();
          sub.Name = render.sharedMaterials[i].name;
          sub.Material = render.sharedMaterials[i];

          var subMesh = mesh.GetSubMesh(i);
          var indexEnd = subMesh.indexStart + subMesh.indexCount;
          var vertexEnd = subMesh.firstVertex + subMesh.vertexCount;

          for (var j = subMesh.indexStart; j < indexEnd; j++)
          {
            sub.Triangles.Add(mesh.triangles[j] - subMesh.firstVertex);
          }
          for (var j = subMesh.firstVertex; j < vertexEnd; j++)
          {
            var k = j + subMesh.baseVertex;
            sub.Vertices.Add(mesh.vertices[k]);
            sub.Normals.Add(mesh.normals[k]);
            sub.UV.Add(mesh.uv[k]);
          }
          if (sub.Vertices.Count == 0)
            Debug.LogWarning("Empty sub mesh " + obj.name + " " + i);
          prepareSubMeshes.Add(sub);
        }
      }

      public List<PreparedMeshSub> prepareSubMeshes = new List<PreparedMeshSub>();
    }

  } 
}
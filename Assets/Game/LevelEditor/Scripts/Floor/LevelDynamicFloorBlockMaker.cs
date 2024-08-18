using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Ballance2.Base;
using Ballance2.Utils;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Progress;

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
      /// 四周圈和中心（由边块，边角，内块组成，可重复扩展）
      /// </summary>
      BorderAndCenter,
      /// <summary>
      /// 无边圈（2x2网格可重复扩展）
      /// </summary>
      Bordeless,
      /// <summary>
      /// 同B ordeless 但带侧墙
      /// </summary>
      BordelessWithSide,
      /// <summary>
      /// 静态网格（最大支持3x3大小）
      /// </summary>
      Grid,
      /// <summary>
      /// 由节和封口组成，专用于轨道
      /// </summary>
      Rail,
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
      InnerCornerNW,
      InnerCornerNE,
      InnerCornerSW,
      InnerCornerSE,
      RailSeg,
      RailSealStart,
      RailSealEnd,
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
      public bool NeedSecondSwitch = false;
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
      public int zl;
      public int xl;
      public bool left;
      public bool top;
      public bool right;
      public bool bottom;
      public Vector2 leftHoleRange = new Vector2(-1, -1);
      public Vector2 topHoleRange = new Vector2(-1, -1);
      public Vector2 rightHoleRange = new Vector2(-1, -1);
      public Vector2 bottomHoleRange = new Vector2(-1, -1);
    }
    public class PreparedMeshGroupCombineTraslateProps
    {
      public Vector3 transformPos;
      public float lengthScale = 1;
    }
    public class PreparedMeshGroup
    {
      public PreparedMeshGroup(InputFloorSchemeDefine define)
      {
        Define = define;
        foreach (var item in define.Prefabs)
        {
          if (!PreparedMeshes.TryGetValue(item.Type, out var list))
          {
            list = new List<PreparedMesh>();
            PreparedMeshes.Add(item.Type, list);
          }
          list.Add(new PreparedMesh(item.Prefab));
        }
      }

      public readonly InputFloorSchemeDefine Define;
      public Dictionary<InputFloorBlockType, List<PreparedMesh>> PreparedMeshes = new Dictionary<InputFloorBlockType, List<PreparedMesh>>();

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

      private void BuildMeshGridBorderAndCenter(PreparedMeshGroupCombineByGrid p, PreparedMeshGroupCombineTraslateProps o, bool needInner)
      {
        //左上角
        if (p.left && p.top && PreparedMeshes.ContainsKey(InputFloorBlockType.CornerNW))
        {
          if (p.topHoleRange.x == 0 && p.leftHoleRange.y == p.zl)
            CombineMeshAdd(InputFloorBlockType.InnerCornerNW, o);
          else if (p.leftHoleRange.y == p.zl)
            CombineMeshAdd(InputFloorBlockType.SideN, o);
          else if (p.topHoleRange.x == 0)
            CombineMeshAdd(InputFloorBlockType.SideW, o);
          else
            CombineMeshAdd(InputFloorBlockType.CornerNW, o);
          return;
        }
        //左下角
        if (p.left && p.bottom && PreparedMeshes.ContainsKey(InputFloorBlockType.CornerSW))
        {
          if (p.leftHoleRange.x == 0 && p.bottomHoleRange.x == 0)
            CombineMeshAdd(InputFloorBlockType.InnerCornerSW, o);
          else if (p.bottomHoleRange.x == 0)
            CombineMeshAdd(InputFloorBlockType.SideW, o);
          else if (p.leftHoleRange.x == 0)
            CombineMeshAdd(InputFloorBlockType.SideS, o);
          else
            CombineMeshAdd(InputFloorBlockType.CornerSW, o);
          return;
        }
        //右上角
        if (p.right && p.top && PreparedMeshes.ContainsKey(InputFloorBlockType.CornerNE))
        {
          if (p.rightHoleRange.y == p.zl && p.topHoleRange.y == p.xl)
            CombineMeshAdd(InputFloorBlockType.InnerCornerNE, o);
          else if (p.rightHoleRange.y == p.zl)
            CombineMeshAdd(InputFloorBlockType.SideN, o);
          else if (p.topHoleRange.y == p.xl)
            CombineMeshAdd(InputFloorBlockType.SideE, o);
          else
            CombineMeshAdd(InputFloorBlockType.CornerNE, o);
          return;
        }
        //右下角
        if (p.right && p.bottom && PreparedMeshes.ContainsKey(InputFloorBlockType.CornerSE))
        {
          if (p.rightHoleRange.x == 0 && p.bottomHoleRange.y == p.xl)
            CombineMeshAdd(InputFloorBlockType.InnerCornerSE, o);
          else if (p.rightHoleRange.x == 0)
            CombineMeshAdd(InputFloorBlockType.SideS, o);
          else if (p.bottomHoleRange.y == p.xl)
            CombineMeshAdd(InputFloorBlockType.SideE, o);
          else
            CombineMeshAdd(InputFloorBlockType.CornerSE, o);
          return;
        }
        //上
        if (p.top)
        {
          if (p.topHoleRange.x > p.x && p.topHoleRange.y < p.x)
            CombineMeshAdd(InputFloorBlockType.Inner, o);
          else if (p.topHoleRange.x == p.x)
            CombineMeshAdd(InputFloorBlockType.InnerCornerNW, o);
          else if (p.topHoleRange.y == p.x)
            CombineMeshAdd(InputFloorBlockType.InnerCornerNE, o);
          else
            CombineMeshAdd(Define.NeedSecondSwitch && p.x % 2 == 0 ? InputFloorBlockType.SideN2 : InputFloorBlockType.SideN, o);
        }
        //下
        if (p.bottom)
        {
          if (p.bottomHoleRange.x > p.x && p.bottomHoleRange.y < p.x)
            CombineMeshAdd(InputFloorBlockType.Inner, o);
          else if (p.bottomHoleRange.x == p.x)
            CombineMeshAdd(InputFloorBlockType.InnerCornerSW, o);
          else if (p.bottomHoleRange.y == p.x)
            CombineMeshAdd(InputFloorBlockType.InnerCornerSE, o);
          else
            CombineMeshAdd(Define.NeedSecondSwitch && p.x % 2 == 0 ? InputFloorBlockType.SideS2 : InputFloorBlockType.SideS, o);
        }
        //左
        if (p.left)
        {
          if (p.leftHoleRange.x > p.z && p.leftHoleRange.y < p.z)
            CombineMeshAdd(InputFloorBlockType.Inner, o);
          else if (p.leftHoleRange.x == p.z)
            CombineMeshAdd(InputFloorBlockType.InnerCornerSW, o);
          else if (p.leftHoleRange.y == p.z)
            CombineMeshAdd(InputFloorBlockType.InnerCornerNW, o);
          else
            CombineMeshAdd(Define.NeedSecondSwitch && p.z % 2 == 0 ? InputFloorBlockType.SideW2 : InputFloorBlockType.SideW, o);
        }
        //右
        if (p.right)
        {
          if (p.rightHoleRange.x > p.z && p.rightHoleRange.y < p.z)
            CombineMeshAdd(InputFloorBlockType.Inner, o);
          else if (p.rightHoleRange.x == p.z)
            CombineMeshAdd(InputFloorBlockType.InnerCornerSE, o);
          else if (p.rightHoleRange.y == p.z)
            CombineMeshAdd(InputFloorBlockType.InnerCornerNE, o);
          else
            CombineMeshAdd(Define.NeedSecondSwitch && p.z % 2 == 0 ? InputFloorBlockType.SideE2 : InputFloorBlockType.SideE, o);
        }
        //中心
        if (needInner && !p.top && !p.right && !p.bottom && !p.left)
        {
          CombineMeshAdd(InputFloorBlockType.Inner, o);
        }
      }

      public void CombineMeshAddByGrid(PreparedMeshGroupCombineByGrid p, PreparedMeshGroupCombineTraslateProps o)
      {
        switch (Define.Type)
        {
          case InputFloorSchemeType.BorderAndCenter: {
            BuildMeshGridBorderAndCenter(p, o, true);
            break;
          }
          case InputFloorSchemeType.BordelessWithSide:
          case InputFloorSchemeType.Bordeless: {
              /*
               1 1 
               0 1
               */
            var x2 = p.x % 2 == 0;
            var z2 = p.z % 2 == 0;
            if (x2 && z2)
              CombineMeshAdd(InputFloorBlockType.InnerSW, o);
            else if (x2)
              CombineMeshAdd(InputFloorBlockType.InnerNW, o);
            else if (z2)
              CombineMeshAdd(InputFloorBlockType.InnerSE, o);
            else
              CombineMeshAdd(InputFloorBlockType.InnerNE, o);
            if (Define.Type == InputFloorSchemeType.BordelessWithSide)
              BuildMeshGridBorderAndCenter(p, o, false);
            break;
          }
          case InputFloorSchemeType.Grid: {
            CombineMeshAdd((InputFloorBlockType)((p.x + 1) * 100 + p.z), o);
            break;
            }
          case InputFloorSchemeType.Rail:
            {
              if (p.top)
                CombineMeshAdd(InputFloorBlockType.RailSealEnd, o);
              if (p.bottom)
                CombineMeshAdd(InputFloorBlockType.RailSealStart, o);
              CombineMeshAdd(InputFloorBlockType.RailSeg, o);
              break;
            }
          default:
            break;
        }
        
       
      }
      public void CombineMeshAdd(PreparedMesh mesh, PreparedMeshGroupCombineTraslateProps o)
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
            Translate = o.transformPos,
            ZScale = o.lengthScale,
            MeshSub = item
          });
        }
      }
      public void CombineMeshAdd(List<PreparedMesh> meshs, PreparedMeshGroupCombineTraslateProps o)
      {
        foreach (var mesh in meshs)
         CombineMeshAdd(mesh, o);
      }
      public void CombineMeshAdd(InputFloorBlockType type, PreparedMeshGroupCombineTraslateProps o)
      {
        if (PreparedMeshes.TryGetValue(type, out var meshs))
          CombineMeshAdd(meshs, o);
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
              {
                var pt = rotationMatrix.MultiplyPoint(item);
                pt.z *= combine.ZScale;
                vertices.Add(pt + combine.Translate);
              }
              foreach (var item in combine.MeshSub.Normals)
                normals.Add(rotationMatrix.MultiplyVector(item));
              uv.AddRange(combine.MeshSub.UV);
            }
            else
            {
              PointSolverResult pointSolverResult = new PointSolverResult();
              for (var i = 0; i < combine.MeshSub.Vertices.Count; i++)
              {
                var pt = rotationMatrix.MultiplyPoint(combine.MeshSub.Vertices[i]);
                pt.z *= combine.ZScale;
                pointSolverResult.RawVertex = pt;
                pointSolverResult.Translate = combine.Translate;
                pointSolverResult.Vertex = pt + pointSolverResult.Translate;
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
      public float ZScale;
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
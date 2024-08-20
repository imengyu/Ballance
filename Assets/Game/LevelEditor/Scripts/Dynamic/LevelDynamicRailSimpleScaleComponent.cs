using Ballance2.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  /// <summary>
  /// 简单缩放动态组件（通常适用于轨道）
  /// </summary>
  public class LevelDynamicRailSimpleScaleComponent : LevelDynamicComponent
  {
    public Vector3 ModelRotate = new Vector3(-90, 180, 0);
    public Vector3 ModelTranslate = new Vector3(0, 0, 0);
    public Vector3 ScalePoint = Vector3.zero;
    public bool ScaleXEnable = false;
    public bool ScaleYEnable = false;
    public bool ScaleZEnable = false;

    public Vector3 ControlPoint1 = new Vector3(10, 0, 0);
    public Vector3 ControlPoint2 = new Vector3(0, 10, 0);
    public Vector3 ControlPoint3 = new Vector3(0, 0, 10);

    [HideInInspector]
    public LevelDynamicRailSimpleScaleEditor Editor;
    public GameObject EditorPrefab;
    protected override void OnUpdateControllers()
    {
      Editor?.UpdateControllers();
    }

    private MeshFilter meshFilter;
    private Mesh orginalMesh;
    private Mesh mesh;

    private void Awake()
    {
      meshFilter = this.GetOrAddComponent<MeshFilter>();
      orginalMesh = meshFilter.mesh;
      mesh = new Mesh();
      meshFilter.mesh = mesh;
    }
    private void Start() 
    {
      UpdateShape();
    }
    public override void UpdateShape()
    {
      GenerateMesh();
    }
    private void GenerateMesh()
    {
      var rotationMatrix = Matrix4x4.Rotate(Quaternion.Euler(ModelRotate));
      var translateMatrix = Matrix4x4.Translate(ModelTranslate) * rotationMatrix;
      var vertices = new List<Vector3>();
      var normals = new List<Vector3>();
      foreach (var vertex in orginalMesh.vertices)
      {
        var pt = translateMatrix.MultiplyPoint(vertex);
        var x = pt.x;
        var y = pt.y;
        var z = pt.z;
        if (ScaleXEnable && x > ScalePoint.x)
          x = x + ControlPoint1.x - ScalePoint.x;
        else if (ScaleXEnable && x > ControlPoint1.x)
          x = ControlPoint1.x;

        if (ScaleYEnable && y > ScalePoint.y)
          y = y + ControlPoint2.y - ScalePoint.y;
        else if (ScaleYEnable && y > ControlPoint2.y)
          y = ControlPoint2.y;

        if (ScaleZEnable && z > ScalePoint.z)
          z = z + ControlPoint3.z - ScalePoint.y;
        else if (ScaleZEnable && z > ControlPoint3.z)
          z = ControlPoint3.z;

        vertices.Add(new Vector3(x, y, z));
      }
      foreach (var normal in orginalMesh.normals)
        normals.Add(rotationMatrix.MultiplyPoint(normal));

      mesh.vertices = vertices.ToArray();
      mesh.normals = normals.ToArray();
      mesh.triangles = orginalMesh.triangles;
      mesh.uv = orginalMesh.uv;
      meshFilter.mesh = mesh;
      UpdateEditorMesh(mesh);
    }
  }
}
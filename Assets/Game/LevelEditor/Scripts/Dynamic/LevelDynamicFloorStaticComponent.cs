using Ballance2.Utils;
using System.Collections.Generic;
using UnityEngine;
using static Ballance2.Game.LevelEditor.LevelDynamicFloorBlockMaker;

namespace Ballance2.Game.LevelEditor
{
  /// <summary>
  /// 静态拼块路面
  /// </summary>
  public class LevelDynamicFloorStaticComponent : LevelDynamicComponent
  {
    public Vector3 ModelRotate = new Vector3(-90, 180, 0);
    public float CompSize = 2.5f;
    public float Width = 5f;
    public float Length = 5f;
    public string FloorScheme = "";
    public List<Transform> SnapPoints = new List<Transform>();

    [HideInInspector]
    public LevelDynamicFloorStaticEditor Editor;
    public GameObject EditorPrefab;
    protected override void OnUpdateControllers()
    {
      Editor?.UpdateControllers();
    }

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private PreparedMeshGroup pMesh;

    private void Awake()
    {
      if (!string.IsNullOrEmpty(FloorScheme))
      {
        meshRenderer = this.GetOrAddComponent<MeshRenderer>();
        meshFilter = this.GetOrAddComponent<MeshFilter>();
        pMesh = LevelDynamicFloorBlockMaker.Instance.FloorSchemes[FloorScheme];
      }
      UpdateShape();
    }

    public override void UpdateShape()
    {
      GenerateMesh();
    }
    private void GenerateMesh()
    {
      if (string.IsNullOrEmpty(FloorScheme))
        return;

      var mesh = meshFilter.mesh;
      if (mesh == null)
      {
        mesh = new Mesh();
        meshFilter.mesh = mesh;
      }

      pMesh.CombineMeshReset();

      var xl = Mathf.Floor(Width / CompSize);
      var zl = Mathf.Floor(Length / CompSize);

      //生成Mesh
      var posOff = new Vector3(CompSize / 2, 0, CompSize / 2);
      var temp = new PreparedMeshGroupCombineByGrid();
      var otemp = new PreparedMeshGroupCombineTraslateProps()
      {
        transformPos = Vector3.zero,
      };
      for (temp.x = 0; temp.x < xl; temp.x++)
      {
        for (temp.z = 0; temp.z < zl; temp.z++)
        {
          temp.left = temp.x == 0;
          temp.top = temp.z == zl - 1;
          temp.right = temp.x == xl - 1;
          temp.bottom = temp.z == 0;
          otemp.transformPos = new Vector3(temp.x * CompSize - Width / 2, 0, temp.z * CompSize) + posOff;
          pMesh.CombineMeshAddByGrid(temp, otemp);
        }
      }

      pMesh.CombineMeshFinish(mesh, ModelRotate);
      meshRenderer.materials = pMesh.CombineGetMeshMaterials();
    }
  }
}
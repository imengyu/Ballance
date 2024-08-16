using Ballance2.Utils;
using UnityEngine;
using static Ballance2.Game.LevelEditor.LevelDynamicFloorBlockMaker;

namespace Ballance2.Game.LevelEditor
{
  /// <summary>
  /// 线性动态路面
  /// </summary>
  public class LevelDynamicFloorBlockComponent : MonoBehaviour 
  {
    public LevelDynamicComponentType Type = LevelDynamicComponentType.Strait;
    public LevelDynamicComponentArcType ArcDirection = LevelDynamicComponentArcType.X;
    public float Width = 5f;
    public Vector3 ControlPoint1 = Vector3.zero;
    public Vector3 ControlPoint2 = new Vector3(0, 0, 10);
    public Vector3 ControlPoint3 = Vector3.zero;
    public Vector3 ControlPoint4 = Vector3.zero;

    public Vector3 ModelRotate = new Vector3(-90, 180, 0);
    public float CompSize = 2.5f;
    public string FloorScheme = "";

    [HideInInspector]
    public LevelDynamicFloorBlockEditor Editor;
    public GameObject EditorPrefab;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private PreparedMeshGroup pMesh;

    public float straitLength = 0;
    public float arcRadius = 0;
    public float arcDeg = 0;
    public float arcLength = 0;
    public float bizerLength = 0;
    public float bizerEndDeg = 0;

    private void Awake() 
    {
      meshRenderer = this.GetOrAddComponent<MeshRenderer>();
      meshFilter = this.GetOrAddComponent<MeshFilter>();
      pMesh = LevelDynamicFloorBlockMaker.Instance.FloorSchemes[FloorScheme];
      UpdateShape();
    }

    [SerializeField]
    private bool enableEdit = false;
    public bool EnableEdit { 
      get => enableEdit; 
      set {
        if (enableEdit != value)
        {
          enableEdit = value;
          Editor?.UpdateControllers();
        }
      }
    }

    public void UpdateShape()
    {
      if (ReadControlPoint())
      {
        /*Debug.Log("Type: " + Type);
        switch (Type)
        {
          case LevelDynamicComponentType.Strait:
            Debug.Log("straitLength: " + straitLength);
            break;
          case LevelDynamicComponentType.Arc:
            Debug.Log("arcRadius: " + arcRadius);
            Debug.Log("arcDeg: " + arcDeg);
            Debug.Log("arcLength: " + arcLength);
            break;
          case LevelDynamicComponentType.Bizer:
            Debug.Log("bizerLength: " + bizerLength);
            break;
        }*/
        
        GenerateMesh();
        if (EnableEdit)
          Editor.UpdateRuler();
      }
    }

    public Vector3 CalcArcPoint(float inRadiusRef, float angle)
    {
      var center = ControlPoint3;
      switch (ArcDirection)
      {
        case LevelDynamicComponentArcType.X:
          {
            var radius = inRadiusRef + Mathf.Abs(arcRadius);
            var x = center.x + (radius * Mathf.Cos(angle * Mathf.Deg2Rad) * (arcRadius < 0 ? -1 : 1));
            var z = center.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            return new Vector3(x, 0, z);
          }
        case LevelDynamicComponentArcType.Y:
          {
            var radius = inRadiusRef + Mathf.Abs(arcRadius);
            var y = center.y + (radius * Mathf.Cos(angle * Mathf.Deg2Rad) * (arcRadius < 0 ? -1 : 1));
            var z = center.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            return new Vector3(0, y, z);
          }
      }
      return Vector3.zero;
    }
    public bool ReadControlPoint()
    {
      switch (Type)
      {
        //直线
        case LevelDynamicComponentType.Strait: {
          straitLength = ControlPoint2.z - ControlPoint1.z;
          if (straitLength < CompSize)
            return false;
          return true;
        }
        //圆弧
        case LevelDynamicComponentType.Arc: {
          var vec1 = ControlPoint1 - ControlPoint3;
          var vec2 = ControlPoint2 - ControlPoint3;

          float dot = Vector3.Dot(vec1.normalized, vec2.normalized);
          arcDeg = Mathf.Acos(dot) * Mathf.Rad2Deg;

          if (ControlPoint2.z < ControlPoint1.z)
            arcDeg = 180 - arcDeg + 180;//已经超出一个圆弧了，认为是钝角

          switch (ArcDirection)
          {
            case LevelDynamicComponentArcType.X:
              arcRadius = ControlPoint1.x - ControlPoint3.x;
              break;
            case LevelDynamicComponentArcType.Y:
              arcRadius = ControlPoint1.y - ControlPoint3.y;
              break;
            default:
              return false;
          }
          arcLength = Mathf.Abs(arcDeg / 360 * 2 * Mathf.PI * arcRadius);
          if (arcLength < CompSize)
            return false;
          return true;
        }
        //贝塞尔曲线
        case LevelDynamicComponentType.Bizer: {
          var pt1 = ControlPoint1;
          var pt2 = ControlPoint2;
          var pt3 = pt1 + ControlPoint3;
          var pt4 = pt2 + ControlPoint4;
          
          var vec1 = ControlPoint4 - ControlPoint2;
          float dot = Vector3.Dot(vec1.normalized, Vector3.left);
          bizerEndDeg = Mathf.Acos(dot) * Mathf.Rad2Deg;
          bizerLength = BezierUtils.BezierLength(pt1, pt3, pt4, pt2);
          if (bizerLength < CompSize)
            return false;
          return true;
        }
      }
      return false;
    }
    private void GenerateMesh()
    {
      var mesh = meshFilter.mesh;
      if (mesh == null)
      {
        mesh = new Mesh();
        meshFilter.mesh = mesh;
      }

      pMesh.CombineMeshReset();

      var xl = Mathf.Floor(Width / CompSize);
      var zl = 0f;

      switch (Type)
      {
        //直线
        case LevelDynamicComponentType.Strait: {
          zl = Mathf.Abs(Mathf.Floor(straitLength / CompSize));
          break;
        }
        //圆弧
        case LevelDynamicComponentType.Arc: {
          zl = Mathf.Abs(Mathf.Floor(arcLength / CompSize));
          break;
        }
        //贝塞尔曲线
        case LevelDynamicComponentType.Bizer: {
          zl = Mathf.Abs(Mathf.Floor(bizerLength / CompSize));
          break;
        }
      }
      
      //生成Mesh
      var posOff = new Vector3(CompSize / 2, 0, CompSize / 2);
      var temp = new PreparedMeshGroupCombineByGrid();
      var transformPos = Vector3.zero;
      for (temp.x = 0; temp.x < xl; temp.x++)
      {
        for (temp.z = 0; temp.z < zl; temp.z++)
        {
          temp.left = temp.x == 0;
          temp.top = temp.z == zl - 1;
          temp.right = temp.x == xl - 1;
          temp.bottom = temp.z == 0;
          transformPos = new Vector3(temp.x * CompSize - Width / 2, 0, temp.z * CompSize) + posOff;
          pMesh.CombineMeshAddByGrid(temp, transformPos);
        }
      }

      //拼接与缩放
      switch (Type)
      {
        //直线
        case LevelDynamicComponentType.Strait: {
          pMesh.CombineMeshFinish(mesh, ModelRotate);
          break;
        }
        //圆弧
        case LevelDynamicComponentType.Arc: {
          var zld = zl * CompSize;
          pMesh.CombineMeshFinish(mesh, ModelRotate, (p) => {
            var angle = p.Vertex.z / zld * arcDeg;
            switch (ArcDirection)
            {
              case LevelDynamicComponentArcType.X:
                {
                  var point = CalcArcPoint(p.Vertex.x, angle);
                  p.Vertex = new Vector3(point.x, p.Vertex.y, point.z);
                  break;
                }
              case LevelDynamicComponentArcType.Y:
                {
                  var point = CalcArcPoint(p.Vertex.y,angle);
                  p.Vertex = new Vector3(p.Vertex.x, point.y, point.z);
                }
                break;
            }
          }, arcRadius < 0);
          break;
        }
        //贝塞尔曲线
        case LevelDynamicComponentType.Bizer:
          { 
            var pt1 = ControlPoint1;
            var pt2 = ControlPoint2;
            var pt3 = pt1 + ControlPoint3;
            var pt4 = pt2 + ControlPoint4;

            var _IsStrait = true;
            var _BendVector = new Vector3(0, 0, 0);

            if (pt2.x != 0 || pt2.y != 0)
            {
              _BendVector.x = pt2.x;
              _BendVector.y = pt2.y;
              _BendVector.z = pt2.z;
              _IsStrait = false;
            }

            pMesh.CombineMeshFinish(mesh, ModelRotate, _IsStrait ? null : (p) => {
              var pec = p.Vertex.z / bizerLength;

              var _BezierPos = BezierUtils.Bezier(pt1, pt3, pt4, pt2, pec);

              if (pec <= 0.05 || pec >= 0.95)
              {
                p.Vertex = _BezierPos + new Vector3(p.Vertex.x, p.Vertex.y, 0);
              }
              else
              {
                Vector3 _VerticalVector = new Vector3(p.Vertex.x, p.Vertex.y, 0) - Vector3.Project(new Vector3(p.Vertex.x, p.Vertex.y, 0), _BendVector); //获取顶点在曲线上应有的垂直偏移向量
                p.Vertex = _BezierPos + _VerticalVector;
              }
            });
            break;
          }
      }
      
      meshRenderer.materials = pMesh.CombineGetMeshMaterials();

      //更新编辑器用于选择的MeshCollider
      var meshCol = transform.parent?.gameObject?.GetComponent<MeshCollider>();
      if (meshCol != null)
        meshCol.sharedMesh = mesh;
    }
  } 


  public enum LevelDynamicComponentType 
  {
    Strait,
    Arc,
    Bizer,
  }
  public enum LevelDynamicComponentArcType 
  {
    X,
    Y,
    Z,
  }
}
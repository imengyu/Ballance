using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DrawCube : MonoBehaviour
{
  private Mesh mesh;
  private MeshFilter meshFilter;
  private MeshRenderer meshRenderer;

  // ����һ��������� Mesh
  private Mesh CreateCubeMesh()
  {
    Mesh mesh = new Mesh();
    mesh.vertices = new Vector3[]
    {
            new Vector3(-1, -1, -1), // 0
            new Vector3(1, -1, -1),  // 1
            new Vector3(1, 1, -1),   // 2
            new Vector3(-1, 1, -1),  // 3
            new Vector3(-1, -1, 1),  // 4
            new Vector3(1, -1, 1),   // 5
            new Vector3(1, 1, 1),    // 6
            new Vector3(-1, 1, 1)    // 7
    };

    mesh.SetIndices(new int[]
    {
            0, 1, 1, 2, 2, 3, 3, 0, // ǰ��������
            4, 5, 5, 6, 6, 7, 7, 4, // ����������
            0, 4, 1, 5, 2, 6, 3, 7  // ����ǰ���������������
    }, MeshTopology.Lines, 0);

    return mesh;
  }


  private void Start()
  {
    meshFilter = GetComponent<MeshFilter>();
    meshRenderer = GetComponent<MeshRenderer>();

    mesh = CreateCubeMesh();
    meshFilter.mesh = mesh;

    var material = new Material(Shader.Find("Unlit/Color"));
    material.color = Color.green;
    meshRenderer.material = material;
  }
}
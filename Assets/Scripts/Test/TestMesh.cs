using UnityEngine;
using System.IO;

public class TestMesh : MonoBehaviour {
  public Mesh mesh;

  private void Start() {

    StreamWriter sw = new StreamWriter("C:\\Users\\快乐的梦鱼\\Desktop\\a.txt", false);
    sw.Write("vertices.Length: ");
    sw.Write(mesh.vertices.Length);
    sw.Write("\n");

    foreach(Vector3 pt in mesh.vertices) {
      sw.Write(pt.x);
      sw.Write(",");
      sw.Write(pt.y);
      sw.Write(",");
      sw.Write(pt.z);
      sw.Write(",");
    }

    sw.Write("\ntriangles.Length: ");
    sw.Write(mesh.triangles.Length);
    sw.Write("\n");

    foreach(int i in mesh.triangles) {
      sw.Write(i);
      sw.Write(",");
    }
    sw.Close();
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVector : MonoBehaviour
{
  // Start is called before the first frame update

  public Mesh PanelMesh;
  public GameObject Cube1;
  public GameObject Cube2;
  public Vector3 PointTest = new Vector3(10, 6, 0);

  void Start()
  {

  }

  private Vector3 tangent;
  private Vector3 binNormal;

  private void Update()
  {
    /*Vector3 norm = Cube1.transform.position;
    //����norm��������ֱ����
    Vector3.OrthoNormalize(ref norm, ref tangent, ref binNormal);

    Debug.DrawLine(Vector3.zero, norm);
    Debug.DrawLine(Vector3.zero, tangent, Color.red);
    Debug.DrawLine(Vector3.zero, binNormal, Color.green);*/

    Vector3 vec = Cube2.transform.position;
    Debug.DrawLine(Vector3.zero, vec, Color.red);

    var vector = vec.normalized;

    var dy = PointTest.y;
    var dx = PointTest.x;

    var �� = Mathf.Atan2(vector.z, Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y));
    var �� = Mathf.Atan(vector.z / vector.x);

    var ty = Mathf.Sin(��) * dy;
    var da = Mathf.Cos(��) * dy;
    var ez = Mathf.Sin(��) * da;
    var ex = Mathf.Cos(��) * da;
    var cz = Mathf.Sin(Mathf.PI / 2 - ��) * dx;
    var cx = Mathf.Cos(Mathf.PI / 2 - ��) * dx;

    var tx = ex + cx;
    var tz = ez + cz;

    Debug.DrawLine(Vector3.zero, new Vector3(tx, ty, tz), Color.green);
  }

  /*private void OnDrawGizmos()
  {
    Vector3 vec = Cube2.transform.position;
    Gizmos.DrawMesh(PanelMesh, Vector3.zero, Quaternion.LookRotation(vec), new Vector3(1, 0.1f, 1));
  }*/
}

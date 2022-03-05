namespace Ballance2
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  
  public class PureAimPlayer : MonoBehaviour {
    public TextAsset PositionArray;
    public TextAsset RotationArray;
    public bool RotationIsRightHand = true;
    public int MaxIndex = 0;
    public int index = 0;


    private List<Vector3> pos = new List<Vector3>();
    private List<Quaternion> rot = new List<Quaternion>();

    private void Start() {
      StartCoroutine(Load());
    }

    IEnumerator Load() {
      int count = 0;
      if(PositionArray) {
        foreach(string v in PositionArray.text.Split('|', System.StringSplitOptions.RemoveEmptyEntries)) {
          string[] xyz = v.Split(',');
          pos.Add(new Vector3(float.Parse(xyz[0]), float.Parse(xyz[1]), float.Parse(xyz[2])));
          count++;
          if(count > 128) {
            count = 0;
            yield return new WaitForSeconds(0.01f);
          }
        }
      }
      if(RotationArray) {
        foreach(string v in RotationArray.text.Split('|', System.StringSplitOptions.RemoveEmptyEntries)) {
          string[] xyzw = v.Split(',');
          if(RotationIsRightHand)
            rot.Add(new Quaternion(float.Parse(xyzw[2]), float.Parse(xyzw[1]), -float.Parse(xyzw[0]), -float.Parse(xyzw[3])));
          else
            rot.Add(new Quaternion(float.Parse(xyzw[0]), float.Parse(xyzw[1]), float.Parse(xyzw[2]), float.Parse(xyzw[3])));
          count++;
          if(count > 128) {
            count = 0;
            yield return new WaitForSeconds(0.01f);
          }
        }
      }
    }

    private void Update() {
      if(pos.Count > 0) {
        if(index < pos.Count - 1) {
          index++;

          if(MaxIndex > 0 && index >= MaxIndex)
            index = 0;
            
          transform.localPosition = pos[index];
          
          if(index < rot.Count)
            transform.rotation = rot[index];

        } else {
          index = 0;
        }
      }
    }
  }
}
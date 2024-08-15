using TMPro;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicStraitRuler : MonoBehaviour 
  {
    public TMP_Text Text;
    public GameObject Cylinder;

    public void FitInTowPoint(Vector3 point1, Vector3 point2)
    {
      var vector = (point2 - point1);
      var length =  Vector3.Distance(point1, point2);
      var oldScale = Cylinder.transform.localScale;

      transform.position = vector / 2 + point1;
      transform.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
      Cylinder.transform.localScale = new Vector3(oldScale.x, length / 2, oldScale.z);
    }
    public void SetText(string text)
    {
      Text.text = text;
      Text.gameObject.SetActive(!string.IsNullOrEmpty(text));
    }
  }
}
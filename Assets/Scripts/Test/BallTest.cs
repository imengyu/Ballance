using System.Collections;
using UnityEngine;

public class BallTest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LateHide());
    }
    IEnumerator LateHide()
    {
        yield return new WaitForSeconds(10);
        Destroy(this);
    } 
}
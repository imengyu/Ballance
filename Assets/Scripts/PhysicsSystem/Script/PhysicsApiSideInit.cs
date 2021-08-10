using System.Collections;
using UnityEngine;

namespace PhysicsRT
{
    public class PhysicsApiSideInit : MonoBehaviour {

        private void Start() {
            StartCoroutine(A());
        }
        private IEnumerator A() {
            yield return new WaitForSeconds(0.8f);
            PhysicsApi.API.SetName(Application.buildGUID);
        }
    }
}
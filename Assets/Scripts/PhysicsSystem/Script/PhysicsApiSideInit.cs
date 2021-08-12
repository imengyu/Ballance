using UnityEngine;

namespace PhysicsRT
{
    public class PhysicsApiSideInit : MonoBehaviour {
        private void Start() {
            PhysicsApi.InitFinish += () => {
                PhysicsApi.API.SetName(Application.buildGUID);
            };
        }
    }
}
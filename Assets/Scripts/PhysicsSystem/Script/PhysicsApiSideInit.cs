using UnityEngine;

namespace PhysicsRT
{
    public class PhysicsApiSideInit : MonoBehaviour {
        private void Start() {
            PhysicsApi.InitFinish += () => {
                PhysicsApi.API.SetName("666dccad4ae697b45aac145f18f49c5b");
            };
        }
    }
}
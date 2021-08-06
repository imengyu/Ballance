using UnityEngine;

namespace PhysicsRT
{
    public static class PhysicsSystemInit
    {
        public static void Init()
        {
            DoInit();
            Application.quitting += () => {
                DoDestroy();
            };
        }

        public static void DoDestroy()
        {
            PhysicsApi.PhysicsApiDestroy();
        }
        public static void DoInit()
        {
            PhysicsApi.PhysicsApiInit();
        }
    }
}
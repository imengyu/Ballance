using System.Collections.Generic;
using UnityEngine;

namespace PhysicsRT
{
    public static class PhysicsSystemInit
    {
        public static void Init()
        {
            #if !UNITY_EDITOR
            DoInit();
            Application.quitting += () => {
                DoDestroy();
            };
            #endif
        }

        internal static List<PhysicsWorld> Worlds = new List<PhysicsWorld>();

        public static void DoPreDestroy()
        {
            for(int i = Worlds.Count - 1; i >= 0; i--)
                Object.DestroyImmediate(Worlds[i]);
            Worlds.Clear(); 
        }
        public static void DoDestroy()
        {
            DoPreDestroy();
            PhysicsApi.PhysicsApiDestroy();
        }
        public static void DoInit()
        {
            PhysicsApi.PhysicsApiInit();
        }
    }
}
using System.Collections.Generic;
using Ballance2.LuaHelpers;
using SLua;
using UnityEngine;

namespace Ballance2.Sys.Utils
{
    [CustomLuaClass]
    [LuaApiDescription("对象变换状态保存器")]
    public static class ObjectStateBackupUtils
    {
        private struct ObjectStateBackup {
            public Vector3 Pos;
            public Quaternion Rot;
        }
        private static Dictionary<int, ObjectStateBackup> objectBackup = new Dictionary<int, ObjectStateBackup>();

        public static void ClearAll() {
            objectBackup.Clear();
        }
        
        /// <summary>
        /// 清除对象的备份
        /// </summary>
        /// <param name="gameObject"></param>
        [LuaApiDescription("清除对象的备份")]
        public static void ClearObjectBackUp(GameObject gameObject) {
            objectBackup.Remove(gameObject.GetInstanceID());
        }

        /// <summary>
        /// 备份对象的变换状态
        /// </summary>
        /// <param name="gameObject"></param>
        [LuaApiDescription("备份对象的变换状态")]
        public static void BackUpObject(GameObject gameObject) {
            var key = gameObject.GetInstanceID();
            if(!objectBackup.TryGetValue(key, out var st)) 
                st = new ObjectStateBackup();

            st.Pos = gameObject.transform.position;
            st.Rot = gameObject.transform.rotation;

            objectBackup[key] = st;
        }

        /// <summary>
        /// 备份对象和他的一级子对象的变换状态
        /// </summary>
        /// <param name="gameObject"></param>
        [LuaApiDescription("备份对象和他的一级子对象的变换状态")]
        public static void BackUpObjectAndChilds(GameObject gameObject) {
            BackUpObject(gameObject);
            for (int i = 0; i < gameObject.transform.childCount; i++)
                BackUpObject(gameObject.transform.GetChild(i).gameObject);
        }   

        /// <summary>
        /// 从备份还原对象的变换状态
        /// </summary>
        /// <param name="gameObject"></param>
        [LuaApiDescription("从备份还原对象的变换状态")]
        public static void RestoreObject(GameObject gameObject) {
            var key = gameObject.GetInstanceID();
            if(objectBackup.TryGetValue(key, out var st)) {
                gameObject.transform.position = st.Pos;
                gameObject.transform.rotation = st.Rot;
            }
        }

        /// <summary>
        /// 从备份还原对象和他的一级子对象的变换状态
        /// </summary>
        /// <param name="gameObject"></param>
        [LuaApiDescription("从备份还原对象和他的一级子对象的变换状态")]
        public static void RestoreObjectAndChilds(GameObject gameObject) {
            RestoreObject(gameObject);
            for (int i = 0; i < gameObject.transform.childCount; i++)
                RestoreObject(gameObject.transform.GetChild(i).gameObject);
        }

    }
}
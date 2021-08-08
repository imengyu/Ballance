using System.Collections;
using System.Collections.Generic;
using Ballance.LuaHelpers;
using UnityEngine;

namespace Ballance2.Utils
{
    [LuaApiDescription("一个小组件，让物体在激活后指定秒内自动失活")]
    [SLua.CustomLuaClass]
    public class ActiveInTime : MonoBehaviour
    {
        public float ActiveTime = 1;

        private float mActiveTimeInUpdate = 1;

        void Start()
        {
            mActiveTimeInUpdate = ActiveTime;
        }
        private void OnEnable() {
            mActiveTimeInUpdate = ActiveTime;
        }
        private void Update() {
            if(mActiveTimeInUpdate > 0)
                mActiveTimeInUpdate -= Time.deltaTime;
            else
                gameObject.SetActive(false);
        }
    }
}

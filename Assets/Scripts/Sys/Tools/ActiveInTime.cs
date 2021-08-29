using System.Collections;
using System.Collections.Generic;
using Ballance2.LuaHelpers;
using UnityEngine;

namespace Ballance2.Sys.Tools
{
    [LuaApiDescription("一个小组件，让物体在激活后指定秒内自动失活")]
    [SLua.CustomLuaClass]
    public class ActiveInTime : MonoBehaviour
    {
        public float ActiveTime = 1;

        private float mActiveTimeInUpdate = 1;
        private bool mActiveTesting = false;

        void Start()
        {
            mActiveTimeInUpdate = ActiveTime;
            mActiveTesting = true;
        }
        private void OnEnable() {
            mActiveTimeInUpdate = ActiveTime;
            mActiveTesting = true;
        }
        private void Update() {
            if(mActiveTesting) {
                if(mActiveTimeInUpdate > 0)
                    mActiveTimeInUpdate -= Time.deltaTime;
                else {
                    gameObject.SetActive(false);
                    mActiveTesting = false;
                }
            }
        }
    }
}

using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* CustomData.cs
* 
* 用途：
* 自定义对象数据
*
* 作者：
* mengyu
*
*/

namespace Ballance2.Sys.Utils
{
    [SLua.CustomLuaClass]
    public class CustomData : MonoBehaviour
    {
        public string customTag;
        public object customData;
    }
}

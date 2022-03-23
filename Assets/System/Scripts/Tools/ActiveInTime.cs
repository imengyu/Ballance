using UnityEngine;

/*
* Copyright(c) 2021 imengyu
*
* 模块名：     
* ObjectStateBackupUtils.cs
* 
* 用途：
* 一个小组件，让物体在激活后指定秒内自动失活。
*
* 作者：
* mengyu
*/

namespace Ballance2.Sys.Tools
{
  /// <summary>
  /// 一个小组件，让物体在激活后指定秒内自动失活
  /// </summary>
  [LuaApiDescription("一个小组件，让物体在激活后指定秒内自动失活")]
  [SLua.CustomLuaClass]
  [LuaApiNoDoc]
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
    private void OnEnable()
    {
      mActiveTimeInUpdate = ActiveTime;
      mActiveTesting = true;
    }
    private void Update()
    {
      if (mActiveTesting)
      {
        if (mActiveTimeInUpdate > 0)
          mActiveTimeInUpdate -= Time.deltaTime;
        else
        {
          gameObject.SetActive(false);
          mActiveTesting = false;
        }
      }
    }
  }
}

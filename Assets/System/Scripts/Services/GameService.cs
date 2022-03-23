/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameService.cs
* 
* 用途：
* 系统服务的基类
*
* 作者：
* mengyu
*/

using UnityEngine;

namespace Ballance2.Services
{
  /// <summary>
  /// 系统服务基类
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("系统服务基类")]
  public class GameService : MonoBehaviour 
  {
    [SLua.DoNotToLua]
    public GameService(string name)
    {
      Name = name;
    }

    /// <summary>
    /// 服务名称
    /// </summary>
    [LuaApiDescription("服务名称")]
    public string Name { get; private set; }

    /// <summary>
    /// 初始化时被调用。
    /// </summary>
    /// <returns>返回初始化是否成功</returns>
    [SLua.DoNotToLua]
    public virtual bool Initialize()
    {
      return false;
    }
    /// <summary>
    /// 释放时被调用。
    /// </summary>
    [SLua.DoNotToLua]
    public virtual void Destroy()
    {
      Object.Destroy(this);
    }

    protected virtual void Update() {

    }
  }
}

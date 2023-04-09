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

using Ballance2.Base;
using UnityEngine;

namespace Ballance2.Services
{
  /// <summary>
  /// 系统服务基类
  /// </summary>
  public class GameService<T> : GameSingletonBehavior<T> where T : class   
  {
    
    public GameService(string name)
    {
      Name = name;
    }

    /// <summary>
    /// 服务名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 初始化时被调用。
    /// </summary>
    /// <returns>返回初始化是否成功</returns>
    public virtual bool Initialize()
    {
      return false;
    }
    /// <summary>
    /// 释放时被调用。
    /// </summary>
    public virtual void Destroy()
    {
      Object.Destroy(this);
    }

    /// <summary>
    /// 更新函数
    /// </summary>
    protected virtual void Update() {

    }
    /// <summary>
    /// 更新函数
    /// </summary>
    protected virtual void FixedUpdate() {

    }
  }
}

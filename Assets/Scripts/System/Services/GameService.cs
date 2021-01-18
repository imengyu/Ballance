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
*
* 更改历史：
* 2021-1-15 创建
*
*/

namespace Ballance2.System.Services
{
    /// <summary>
    /// 服务基类
    /// </summary>
    [SLua.CustomLuaClass]
    public class GameService
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
        /// 初始化时被调用。此方法重写后无需调用。
        /// </summary>
        /// <returns>返回初始化是否成功</returns>
        [SLua.DoNotToLua]
        public virtual bool Initialize()
        {
            Debug.GameErrorChecker.LastError = Debug.GameError.NotImplemented;
            return false;
        }
        /// <summary>
        /// 释放时被调用。此方法重写后无需调用。
        /// </summary>
        [SLua.DoNotToLua]
        public virtual void Destroy()
        {

        }
    }
}

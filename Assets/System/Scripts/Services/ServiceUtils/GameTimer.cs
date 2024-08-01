using static Ballance2.Services.GameManager;

namespace Ballance2.Services
{
  /// <summary>
  /// 定时器服务，该类是对 GameManager 的封装
  /// </summary>
  public static class GameTimer
  {
    /// <summary>
    /// 设置一个延时执行回调
    /// </summary>
    /// <param name="sec">延时时长，秒</param>
    /// <param name="callback">回调</param>
    public static int Delay(float sec, VoidDelegate callback)
    {
      return GameManager.Instance.Delay(sec, callback);
    }

    /// <summary>
    /// 通过ID删除指定的延时执行回调
    /// </summary>
    /// <param name="timerId">Delay 函数返回的 ID </param>
    /// <returns>返回是否删除成功</returns>
    public static bool DeleteDelay(int timerId)
    {
      return GameManager.Instance?.DeleteDelay(timerId) ?? false;
    }

    /// <summary>
    /// 设置一个定时器
    /// </summary>
    /// <param name="loopSec">定时器循环时间</param>
    /// <param name="callback">回调</param>
    /// <returns>返回ID, 可以使用 DeleteTimer 来删除定时器</returns>
    public static int Timer(float loopSec, VoidDelegate callback)
    {
      return GameManager.Instance.Timer(loopSec, callback);
    }

    /// <summary>
    /// 设置一个定时器
    /// </summary>
    /// <param name="delaySec">定时器第一次启动延迟时间</param>
    /// <param name="loopSec">定时器循环时间</param>
    /// <param name="callback">回调</param>
    /// <returns>返回ID, 可以使用 DeleteTimer 来删除定时器</returns>
    public static int Timer(float loopSec, float delaySec, VoidDelegate callback)
    {
      return GameManager.Instance.Timer(loopSec, delaySec, callback);
    }
       
    /// <summary>
    /// 通过ID删除指定的定时器
    /// </summary>
    /// <param name="timerId">Timer 函数返回的 ID </param>
    /// <returns>返回是否删除成功</returns>
    public static bool DeleteTimer(int timerId)
    {
      return GameManager.Instance.DeleteTimer(timerId);
    }
  }
}
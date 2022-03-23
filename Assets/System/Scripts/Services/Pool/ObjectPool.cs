using System.Collections.Generic;
using UnityEngine.Events;

namespace Ballance2.Services.Pool
{
  /// <summary>
  /// 对象池泛型版
  /// </summary>
  /// <typeparam name="T">泛型类</typeparam>
  public class ObjectPool<T> where T : class
  {
    private readonly Stack<T> m_Stack = new Stack<T>();
    private readonly UnityAction<T> m_ActionOnGet;
    private readonly UnityAction<T> m_ActionOnRelease;

    public int countAll { get; private set; }
    public int countActive { get { return countAll - countInactive; } }
    public int countInactive { get { return m_Stack.Count; } }

    /// <summary>
    /// 使用指定回调创建池
    /// </summary>
    /// <param name="actionOnGet">创建回调</param>
    /// <param name="actionOnRelease">释放回调</param>
    public ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
    {
      m_ActionOnGet = actionOnGet;
      m_ActionOnRelease = actionOnRelease;
    }

    /// <summary>
    /// 获取一个可用对象
    /// </summary>
    /// <returns>返回对象，如果没有可用对象，则返回null</returns>
    public T Get()
    {
      T element = m_Stack.Pop();
      if (m_ActionOnGet != null)
        m_ActionOnGet(element);
      return element;
    }

    /// <summary>
    /// 释放对象并回退
    /// </summary>
    /// <param name="element">对象</param>
    public void Release(T element)
    {
      if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element))
        UnityEngine.Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
      if (m_ActionOnRelease != null)
        m_ActionOnRelease(element);
      m_Stack.Push(element);
    }
  }
}
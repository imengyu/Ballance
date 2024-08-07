using System;
using System.Collections.Generic;
using Ballance2.Services;

namespace AillieoUtils
{
  public class SimpleObjPool<T>
  {
    private Stack<T> m_Stack;
    protected Func<T> m_ctor;
    protected Action<T> m_OnRecycle;
    protected Action<T> m_OnClear;
    private int m_Size;
    private int m_UsedCount;

    public SimpleObjPool(int max = 5, Action<T> actionOnReset = null, Func<T> ctor = null, Action<T> actionOnClear = null)
    {
      m_Stack = new Stack<T>(max);
      m_Size = max;
      m_OnRecycle = actionOnReset;
      m_ctor = ctor;
    }

    public T Get()
    {
      T item;
      if (m_Stack.Count == 0)
      {
        if (null != m_ctor)
        {
          item = m_ctor();
        }
        else
        {
          item = Activator.CreateInstance<T>();
        }
      }
      else
      {
        item = m_Stack.Pop();
      }
      m_UsedCount++;
      return item;
    }

    public void Recycle(T item)
    {
      if (m_OnRecycle != null)
      {
        m_OnRecycle.Invoke(item);
      }
      if (m_Stack.Count < m_Size)
      {
        m_Stack.Push(item);
      }
      m_UsedCount--;
    }

    public void Clear()
    {
      foreach (var item in m_Stack)
        m_OnClear?.Invoke(item);
      m_Stack.Clear();
    }

    public override string ToString()
    {
      return string.Format("SimpleObjPool: item=[{0}], inUse=[{1}], restInPool=[{2}/{3}] ", typeof(T), m_UsedCount, m_Stack.Count, m_Size);
    }

  }
}

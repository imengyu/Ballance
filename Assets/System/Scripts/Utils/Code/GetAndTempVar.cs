using System;
using UnityEngine;

namespace Ballance2.Utils
{
  public class GetAndTempVar<T>
  {
    private Func<T> firstGet;
    private T var;

    public GetAndTempVar(Func<T> firstGet) 
    {
      this.firstGet = firstGet;
    }

    public T Get() { 
      if (var == null)
        var = firstGet.Invoke();
      return var; 
    }
  }

  public class GetComponentAndTempVar<T> : GetAndTempVar<T>
  { 
    public GetComponentAndTempVar(Component component) : base(() => component.GetComponent<T>()) {}
  }
}
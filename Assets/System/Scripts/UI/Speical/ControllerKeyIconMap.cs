using System;
using System.Collections.Generic;
using Ballance2.Base;
using SubjectNerd.Utilities;
using UnityEngine;

namespace Ballance2.UI
{
  public class ControllerKeyIconMap : GameSingletonBehavior<ControllerKeyIconMap> 
  {
    [SerializeField]
    [Reorderable("IconMap", true, "Binding")]
    public List<ControllerKeyIcon> IconMap;

    [Serializable]
    public class ControllerKeyIcon
    {
      public string Binding;
      public string Type;
      public Sprite Icon;
    }
  
    private Dictionary<string, Sprite> cache = new Dictionary<string, Sprite>();

    public Sprite GetControllerKeyIcon(string type, string binding) 
    {
      if (cache.ContainsKey(binding))
        return cache[binding];
      var result = IconMap.Find((p) => p.Binding == binding && p.Type == type);
      if (result == null) {
        Debug.LogWarning($"Missing Controller Key Icon {type}/{binding}!");
        return null;
      }
      cache.Add(binding, result.Icon);
      return result.Icon;
    }
  }
}
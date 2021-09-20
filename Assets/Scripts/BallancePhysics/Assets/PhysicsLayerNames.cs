using System;
using System.Collections.Generic;
using System.Linq;
using BallancePhysics.Data;
using UnityEngine;

namespace BallancePhysics
{
  [SLua.CustomLuaClass]
  [CreateAssetMenu(menuName = "PhysicsRT/Physics Layer Names", fileName = "PhysicsLayerNames")]
  public sealed class PhysicsLayerNames : ScriptableObject, ITagNames
  {
    PhysicsLayerNames() { }

    IReadOnlyList<string> ITagNames.TagNames => LayerNames;

    public IReadOnlyList<string> LayerNames => m_LayerNames;
    public GroupFilter[] GroupFilter => m_GroupFilter;

    [SerializeField]
    string[] m_LayerNames = Enumerable.Range(0, 32).Select(i => string.Empty).ToArray();

    [SerializeField]
    GroupFilter[] m_GroupFilter = Enumerable.Range(0, 32).Select(i => new GroupFilter()).ToArray();

    void OnValidate()
    {
      if (m_LayerNames.Length != 32)
        Array.Resize(ref m_LayerNames, 32);
    }

    public int[] GetGroupFilterMasks()
    {
      return Enumerable.Range(0, 32).Select(i =>
      {
        return m_GroupFilter[i].GetMask();
      }).ToArray();
    }
  }

  [Serializable]
  public class GroupFilter
  {
    [SerializeField]
    public bool[] m_GroupFilter = Enumerable.Range(0, 32).Select(i => true).ToArray();

    public int GetMask()
    {
      int rs = 0x1;
      for (int i = 1; i < 32; i++)
      {
        if (m_GroupFilter[i])
          rs |= (1 << i);
      }
      return rs;
    }
  }
}
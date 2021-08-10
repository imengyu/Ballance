using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public uint[] GetGroupFilterMasks() {
        //对角线上的数据是重复的，直接拷贝一下
        for(int i = 1; i < 32; i++) {
            for(int j = 31 - i; j >= 0; j--) 
                if(i > 0 && j > 0)
                    m_GroupFilter[i].m_GroupFilter[j] = m_GroupFilter[i - 1].m_GroupFilter[j - 1];
        }
        return Enumerable.Range(0, 32).Select(i => m_GroupFilter[i].GetMask()).ToArray();
    }
}

[Serializable]
public class GroupFilter {
    [SerializeField]
    public bool[] m_GroupFilter = new bool[32];

    public uint GetMask() {
        uint rs = 0;
        for(int i = 0; i < 32; i++) {
            if(m_GroupFilter[i])
                rs &= (uint)(1 << i);
        }
        return rs;
    }
}


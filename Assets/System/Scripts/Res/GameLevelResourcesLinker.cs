using UnityEngine;
using System.Collections.Generic;
using SubjectNerd.Utilities;

/*
* Copyright(c) 2022 mengyu
*
* 模块名：     
* GameLevelResourcesLinker.cs
* 
* 用途：
* 此脚本用于自定义关卡加入自定义资源使用。
*
* 作者：
* mengyu
*
*/

namespace Ballance2 {
  /// <summary>
  /// 关卡资源索引器
  /// </summary>
  [SLua.CustomLuaClass]
  public class GameLevelResourcesLinker : MonoBehaviour {

    private static Dictionary<string, GameLevelResourcesLinker> allRes = new Dictionary<string, GameLevelResourcesLinker>();

    private void Start() {
      if(allRes.ContainsKey(Name))
        Log.W("GameLevelResourcesLinker", "The name " + Name + " already exists in allRes!");
      else
        allRes.Add(Name, this);
    }

    /// <summary>
    /// 查找指定名称的关卡资源索引器
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    public GameLevelResourcesLinker GetLevelResourcesLinker(string name) {
      if(allRes.TryGetValue(name, out var res))
        return res;
      return null;
    }

    /// <summary>
    /// 当前关卡资源索引器的名称
    /// </summary>
    public string Name = "";
    /// <summary>
    /// 声音资源
    /// </summary>
    [Reorderable("AudioAssets", false)]
    public List<AudioClip> AudioAssets = null;
    /// <summary>
    /// 贴图资源
    /// </summary>
    [Reorderable("TextureAssets", false)]
    public List<Texture> TextureAssets = null;
  }
}
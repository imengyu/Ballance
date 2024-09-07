using UnityEngine;

/*
* Copyright(c) 2022 mengyu
*
* 模块名：     
* GameDebugModulEntry.cs
* 
* 用途：
* 自定义调试场景(机关/关卡)的配置。
*
* 作者：
* mengyu
*/

namespace Ballance2.Entry
{
  public class GameDebugEntry : MonoBehaviour
  {
    public static GameDebugEntry Instance { get; private set; }

    public GameDebugEntry() {
      Instance = this;
    }
    
    [Tooltip("机关名称, 将查找此名称的占位物体并把机关替换上去")]
    public string ModulName = "";
    [Tooltip("机关实例")]
    public GameObject ModulInstance = null;
    [Tooltip("机关测试路面的实例")]
    public GameObject ModulTestFloor = null; 
    [Tooltip("机关测试UI的实例")]
    public GameObject ModulTestUI = null; 
    
    [Tooltip("LevelCustomDebug 加载的关卡名称")]
    public string LevelName = "Level01";
    [Tooltip("LevelEditorDebugManager 加载的关卡名称")]
    public string EditLevelName = "levelmake";
  }
}

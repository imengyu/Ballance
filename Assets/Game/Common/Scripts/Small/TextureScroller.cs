using UnityEngine;

/*
* Copyright(c) 2021 imengyu
*
* 模块名：     
* TextureScroller.cs
* 
* 用途：
* 材质贴图滚动组件。在游戏中用于云层的滚动。
* 
* 作者：
* mengyu
*/

namespace Ballance2.Game
{
  /// <summary>
  /// 材质贴图滚动组件。在游戏中用于云层的滚动。
  /// </summary>
  public class TextureScroller : MonoBehaviour
  {
    [Tooltip("设置需要滚动的材质")]
    /// <summary>
    /// 设置需要滚动的材质
    /// </summary>
    public Material Material;
    [Tooltip("设置材质滚动的速度")]
    /// <summary>
    /// 设置材质滚动的速度
    /// </summary>
    public Vector2 ScrollSpeed;

    private Vector2 scrollOff;

    private void Start()
    {
      if (Material == null)
      {
        var renderer = GetComponent<Renderer>();
        if (renderer)
          Material = renderer.sharedMaterial;
      }
      if (Material != null)
      {
        scrollOff = Material.GetTextureOffset("_MainTex");
      }
    }
    private void OnEnable() {
      var renderer = GetComponent<Renderer>();
      if (renderer)
        Material = renderer.sharedMaterial;
    }
    private void Update()
    {
      if (Material != null)
      {
        var x = scrollOff.x + ScrollSpeed.x * Time.deltaTime;
        var y = scrollOff.y + ScrollSpeed.y * Time.deltaTime;
        if (x < 1) x += 1; if (x > 1) x -= 1;
        if (y < 1) y += 1; if (y > 1) y -= 1;
        scrollOff = new Vector2(x, y);
        Material.SetTextureOffset("_MainTex", scrollOff);
      }
    }
  }
}
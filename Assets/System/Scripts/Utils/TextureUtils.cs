using System.IO;
using UnityEngine;

namespace Ballance2.Utils
{
  public static class TextureUtils
  {
    /// <summary>
    /// 从 RenderTexture 创建贴图
    /// </summary>
    /// <param name="rt"></param>
    /// <returns></returns>
    public static Texture2D UpdateTextureFromRenderTexture(RenderTexture rt)
    {
      // 创建一个新的Texture2D
      Texture2D texture2D = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);

      // 从RenderTexture中读取像素数据
      RenderTexture.active = rt;
      texture2D.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
      texture2D.Apply();

      // 清除当前激活的RenderTexture
      RenderTexture.active = null;

      return texture2D;
    }
    /// <summary>
    /// 从文件中加载图片
    /// </summary>
    /// <param name="path">要加载的路径</param>
    /// <returns></returns>
    public static Texture2D LoadTexture2dFromFile(string path, int width, int height)
    {
      Texture2D t2d = new Texture2D(width, height);
      t2d.LoadImage(File.ReadAllBytes(path));
      t2d.Apply();
      return t2d;
    } 
    /// <summary>
    /// 从文件中加载图片
    /// </summary>
    /// <param name="path">要加载的路径</param>
    /// <returns></returns>
    public static Sprite LoadSpriteFromFile(string path, int width, int height)
    {
      return Sprite.Create(LoadTexture2dFromFile(path, width, height), new Rect(0.0f, 0.0f, width, height), new Vector2(0.5f, 0.5f), 100.0f);
    }
  }
  
}
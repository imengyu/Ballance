using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using StbImageSharp;
using System.Threading.Tasks;

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
      if (!File.Exists(path))
        return null;
      Texture2D t2d = new Texture2D(width, height);
      t2d.LoadImage(File.ReadAllBytes(path));
      t2d.Apply();
      return t2d;
    }
    /// <summary>
    /// 从文件中加载图片(Stb)
    /// </summary>
    /// <param name="path"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static Texture2D LoadTexture2dFromFile(string path)
    {
      if (File.Exists(path))
      {
        ImageInfo? info = null;
        using (var stream = File.OpenRead(path))
          info = ImageInfo.FromStream(stream);
        if (info != null)
        {
          Texture2D t2d = new Texture2D(info.Value.Width, info.Value.Height, TextureFormat.RGBA32, true);
          t2d.LoadImage(File.ReadAllBytes(path));
          t2d.Apply();
          return t2d;
        }
      }
      return null;
    }
    /// <summary>
    /// WWW加载图片
    /// </summary>
    /// <param name="path">URL</param>
    /// <param name="outTex">返回信息</param>
    /// <returns></returns>
    public static IEnumerator LoadTexture2dFromFile(string url, EnumeratorResultPacker<Texture2D> outTex)
    {
      UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
      yield return request.SendWebRequest();

      if (request.result == UnityWebRequest.Result.Success)
      {
        outTex.Result = DownloadHandlerTexture.GetContent(request);
        outTex.Success = true;
      }
      else
      {
        outTex.Error = request.error;
      }
    } 

    /// <summary>
    /// 从文件中加载图片
    /// </summary>
    /// <param name="path">要加载的路径</param>
    /// <returns></returns>
    public static Sprite LoadSpriteFromFile(string path, int width = 0, int height = 0)
    {
      return Sprite.Create(
        width == 0 && height == 0 ? LoadTexture2dFromFile(path) : LoadTexture2dFromFile(path, width, height), 
        new Rect(0.0f, 0.0f, width, height), 
        new Vector2(0.5f, 0.5f), 100.0f
       );
    }
    public static Sprite CreateSpriteFromTexture(Texture2D tex)
    {
      return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
    public static Texture2D TextureToTexture2D(Texture texture)
    {
      if (texture == null)
        return null;
      if (texture is Texture2D)
        return (Texture2D)texture;

      Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
      RenderTexture currentRT = RenderTexture.active;
      RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
      Graphics.Blit(texture, renderTexture);

      RenderTexture.active = renderTexture;
      texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
      texture2D.Apply();

      RenderTexture.active = currentRT;
      RenderTexture.ReleaseTemporary(renderTexture);
      return texture2D;
    }
  }
  
}
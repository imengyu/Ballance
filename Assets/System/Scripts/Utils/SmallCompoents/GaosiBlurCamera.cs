using UnityEngine;

namespace Ballance2.Utils.SmallCompoents
{
  public class GaosiBlurCamera : MonoBehaviour
  {
    public Shader gaosiShader;

    private Material gaosiMaterial;
    // 模糊处理的次数
    [Range(0, 4)]
    public int iterations = 3;

    // 越大越模糊
    [Range(0.2f, 3.0f)]
    public float blurSpread = 0.6f;

    //控制模糊采样的图片分辨率,提高性能降低质量
    [Range(1, 8)]
    public int downSample = 2;

    private void GaosiBlur(RenderTexture source, RenderTexture destination)
    {
      int rtW = source.width / downSample;
      int rtH = source.height / downSample;

      RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
      buffer0.filterMode = FilterMode.Bilinear;

      Graphics.Blit(source, buffer0);

      for (int i = 0; i < iterations; i++)
      {
        gaosiMaterial.SetFloat("_BlurSize", 1.0f + i * blurSpread);

        RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);

        // Render the vertical pass
        Graphics.Blit(buffer0, buffer1, gaosiMaterial, 0);

        RenderTexture.ReleaseTemporary(buffer0);
        buffer0 = buffer1;
        buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);

        // Render the horizontal pass
        Graphics.Blit(buffer0, buffer1, gaosiMaterial, 1);

        RenderTexture.ReleaseTemporary(buffer0);
        buffer0 = buffer1;
      }

      Graphics.Blit(buffer0, destination);
      RenderTexture.ReleaseTemporary(buffer0);
    }

  }
}
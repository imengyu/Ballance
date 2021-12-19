using UnityEngine;

/*
* Copyright(c) 2021 imengyu
*
* 模块名：     
* MaterialUtils.cs
* 
* 用途：
* 材质工具类。材质相关的一些工具方法
*
* 作者：
* mengyu
*/

namespace Ballance2.Utils
{
  /// <summary>
  /// 材质工具类。材质相关的一些工具方法。
  /// </summary>
  public static class MaterialUtils
  {
    /// <summary>
    /// 材质渲染模式，对应Unity编辑器中的4个渲染模式。
    /// </summary>
    public enum RenderingMode
    {
      Opaque,
      Cutout,
      Fade,
      Transparent,
    }

    /// <summary>
    /// 设置材质的渲染模式
    /// </summary>
    /// <param name="material">材质</param>
    /// <param name="renderingMode">渲染模式</param>
    public static void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
      switch (renderingMode)
      {
        case RenderingMode.Opaque:
          material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
          material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
          material.SetInt("_ZWrite", 1);
          material.DisableKeyword("_ALPHATEST_ON");
          material.DisableKeyword("_ALPHABLEND_ON");
          material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
          material.renderQueue = -1;
          break;
        case RenderingMode.Cutout:
          material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
          material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
          material.SetInt("_ZWrite", 1);
          material.EnableKeyword("_ALPHATEST_ON");
          material.DisableKeyword("_ALPHABLEND_ON");
          material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
          material.renderQueue = 2450;
          break;
        case RenderingMode.Fade:
          material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
          material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
          material.SetInt("_ZWrite", 0);
          material.DisableKeyword("_ALPHATEST_ON");
          material.EnableKeyword("_ALPHABLEND_ON");
          material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
          material.renderQueue = 3000;
          break;
        case RenderingMode.Transparent:
          material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
          material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
          material.SetInt("_ZWrite", 0);
          material.DisableKeyword("_ALPHATEST_ON");
          material.DisableKeyword("_ALPHABLEND_ON");
          material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
          material.renderQueue = 3000;
          break;
      }
    }
    /// <summary>
    /// 获取材质的渲染模式
    /// </summary>
    /// <param name="material"></param>
    /// <returns></returns>
    public static RenderingMode GetMaterialRenderingMode(Material material) {
      if(material.renderQueue == -1) return RenderingMode.Opaque;
      else if(material.renderQueue == 2450) return RenderingMode.Cutout;
      else {
        var _SrcBlend = material.renderQueue == 0 ? 0 : material.GetInt("_SrcBlend");
        var _DstBlend = material.renderQueue == 0 ? 0 : material.GetInt("_DstBlend");
        if(_SrcBlend == (int)UnityEngine.Rendering.BlendMode.SrcAlpha && _DstBlend == (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha)
          return RenderingMode.Fade;
        else if(_SrcBlend == (int)UnityEngine.Rendering.BlendMode.One && _DstBlend == (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha)
          return RenderingMode.Fade;
        return RenderingMode.Opaque;
      }
    } 
  }
}
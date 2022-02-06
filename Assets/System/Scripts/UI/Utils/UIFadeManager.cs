using System.Collections.Generic;
using Ballance2.Utils;
using UnityEngine;
using UnityEngine.UI;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* UIFadeManager.cs
* 
* 用途：
* 渐变自管理类。
* 负责渐变自动化脚本的管理与执行。
* 该组件有绑定一个实例至 GameUIManager 上，可快速访问。
*
* 作者：
* mengyu
*/

namespace Ballance2.UI.Utils
{
  /// <summary>
  /// 渐变自管理类
  /// </summary>
  [SLua.CustomLuaClass]
  [LuaApiDescription("渐变自管理类")]
  public class UIFadeManager : MonoBehaviour
  {
    [LuaApiDescription("当前的渐变对象")]
    public List<FadeObject> fadeObjects = new List<FadeObject>();

    private void Update()
    {
      if (fadeObjects.Count > 0)
      {
        FadeObject fadeObject = null;
        for (int i = fadeObjects.Count - 1; i >= 0; i--)
        {
          fadeObject = fadeObjects[i];
          if (fadeObject.runEnd) fadeObjects.Remove(fadeObject);
          else
          {
            if (fadeObject.fadeType == FadeType.FadeIn)
            {
              if (fadeObject.alpha < 1)
              {
                fadeObject.alpha += Time.deltaTime / fadeObject.timeInSecond * 1;
                if (fadeObject.material != null)
                  fadeObject.material.color = new Color(fadeObject.material.color.r, fadeObject.material.color.g, fadeObject.material.color.b, fadeObject.alpha);
                else if (fadeObject.materials != null && fadeObject.materials.Length > 0)
                {
                  foreach (Material m in fadeObject.materials)
                    m.color = new Color(m.color.r, m.color.g, m.color.b, fadeObject.alpha);
                }
                else if (fadeObject.image != null)
                  fadeObject.image.color = new Color(fadeObject.image.color.r, fadeObject.image.color.g, fadeObject.image.color.b, fadeObject.alpha);
                else if (fadeObject.text != null)
                  fadeObject.text.color = new Color(fadeObject.text.color.r, fadeObject.text.color.g, fadeObject.text.color.b, fadeObject.alpha);
                else if (fadeObject.audio != null)
                  fadeObject.audio.volume = fadeObject.alpha;
              }
              else fadeObject.runEnd = true;
            }
            else if (fadeObject.fadeType == FadeType.FadeOut)
            {
              if (fadeObject.alpha > 0)
              {
                fadeObject.alpha -= Time.deltaTime / fadeObject.timeInSecond * 1;
                if (fadeObject.material != null)
                  fadeObject.material.color = new Color(fadeObject.material.color.r, fadeObject.material.color.g, fadeObject.material.color.b, fadeObject.alpha);
                else if (fadeObject.materials != null && fadeObject.materials.Length > 0)
                {
                  foreach (Material m in fadeObject.materials)
                    m.color = new Color(m.color.r, m.color.g, m.color.b, fadeObject.alpha);
                }
                else if (fadeObject.image != null)
                  fadeObject.image.color = new Color(fadeObject.image.color.r, fadeObject.image.color.g, fadeObject.image.color.b, fadeObject.alpha);
                else if (fadeObject.text != null)
                  fadeObject.text.color = new Color(fadeObject.text.color.r, fadeObject.text.color.g, fadeObject.text.color.b, fadeObject.alpha);
                else if (fadeObject.audio != null)
                  fadeObject.audio.volume = fadeObject.alpha;
              }
              else
              {
                if (fadeObject.endReactive)
                {
                  if (fadeObject.gameObject != null)
                    fadeObject.gameObject.SetActive(false);
                }

                //恢复材质渲染模式为原来的
                if (fadeObject.material != null && fadeObject.oldMatRenderMode != MaterialUtils.RenderingMode.Fade)
                {
                  MaterialUtils.SetMaterialRenderingMode(fadeObject.material, fadeObject.oldMatRenderMode);
                  fadeObject.oldMatRenderMode = MaterialUtils.RenderingMode.Fade;
                }
                fadeObject.runEnd = true;
              }
            }
          }
        }
      }
    }

    private FadeObject FindFadeObjectByGameObject(GameObject gameObject, FadeType fadeType)
    {
      foreach (FadeObject o in fadeObjects)
      {
        if (o.gameObject == gameObject && fadeType == o.fadeType)
          return o;
      }
      return null;
    }
    private FadeObject FindFadeObjectByImage(Image image, FadeType fadeType)
    {
      foreach (FadeObject o in fadeObjects)
      {
        if (o.image == image && fadeType == o.fadeType)
          return o;
      }
      return null;
    }
    private FadeObject FindFadeObjectByText(Text text, FadeType fadeType)
    {
      foreach (FadeObject o in fadeObjects)
      {
        if (o.text == text && fadeType == o.fadeType)
          return o;
      }
      return null;
    }
    private FadeObject FindFadeObjectByAudio(AudioSource audio, FadeType fadeType)
    {
      foreach (FadeObject o in fadeObjects)
      {
        if (o.audio == audio && fadeType == o.fadeType)
          return o;
      }
      return null;
    }

    /// <summary>
    /// 运行淡出动画
    /// </summary>
    /// <param name="gameObject">执行对象</param>
    /// <param name="timeInSecond">执行时间</param>
    /// <param name="hidden">执行完毕是否将对象设置为不激活</param>
    /// <param name="material">执行材质</param>
    [LuaApiDescription("运行淡出动画")]
    [LuaApiParamDescription("gameObject", "执行对象")]
    [LuaApiParamDescription("timeInSecond", "执行时间")]
    [LuaApiParamDescription("material", "执行材质")]
    [LuaApiParamDescription("hidden", "执行完毕是否将对象设置为不激活")]
    public FadeObject AddFadeOut(GameObject gameObject, float timeInSecond, bool hidden, Material material)
    {
      FadeObject fadeObject = FindFadeObjectByGameObject(gameObject, FadeType.FadeOut);
      if (fadeObject != null)
        fadeObjects.Remove(fadeObject);

      fadeObject = new FadeObject(this);
      fadeObject.gameObject = gameObject;
      fadeObject.timeInSecond = timeInSecond;
      fadeObject.alpha = 1;
      if (material == null)
      {
        var meshRender = gameObject.GetComponent<MeshRenderer>();
        fadeObject.material = meshRender.material;
      }
      else fadeObject.material = material;
      fadeObject.fadeType = FadeType.FadeOut;
      if (fadeObject.material != null)
      {

        fadeObject.material.color = new Color(fadeObject.material.color.r, fadeObject.material.color.g, fadeObject.material.color.b, 1);

        //设置材质渲染模式为Fade
        var oldRenderMode = MaterialUtils.GetMaterialRenderingMode(fadeObject.material);
        if (oldRenderMode != MaterialUtils.RenderingMode.Fade)
        {
          fadeObject.oldMatRenderMode = oldRenderMode;
          MaterialUtils.SetMaterialRenderingMode(fadeObject.material, MaterialUtils.RenderingMode.Fade);
        }
      }
      fadeObject.endReactive = hidden;
      fadeObjects.Add(fadeObject);
      return fadeObject;
    }
    /// <summary>
    /// 运行淡入动画
    /// </summary>
    /// <param name="gameObject">执行对象</param>
    /// <param name="timeInSecond">执行时间</param>
    /// <param name="material">执行材质</param>
    [LuaApiDescription("运行淡入动画")]
    [LuaApiParamDescription("gameObject", "执行对象")]
    [LuaApiParamDescription("timeInSecond", "执行时间")]
    [LuaApiParamDescription("material", "执行材质")]
    public FadeObject AddFadeIn(GameObject gameObject, float timeInSecond, Material material)
    {
      FadeObject fadeObject = FindFadeObjectByGameObject(gameObject, FadeType.FadeIn);
      if (fadeObject != null)
        fadeObjects.Remove(fadeObject);

      fadeObject = new FadeObject(this);
      fadeObject.gameObject = gameObject;
      fadeObject.timeInSecond = timeInSecond;
      fadeObject.alpha = 0;
      if (material == null)
      {
        var meshRender = gameObject.GetComponent<MeshRenderer>();
        fadeObject.material = meshRender.material;
      }
      else fadeObject.material = material;
      fadeObject.fadeType = FadeType.FadeIn;
      if (fadeObject.material != null)
      {
        fadeObject.material.color = new Color(fadeObject.material.color.r, fadeObject.material.color.g, fadeObject.material.color.b, 0);

        //设置材质渲染模式为Fade
        var oldRenderMode = MaterialUtils.GetMaterialRenderingMode(fadeObject.material);
        if (oldRenderMode != MaterialUtils.RenderingMode.Fade)
        {
          fadeObject.oldMatRenderMode = oldRenderMode;
          MaterialUtils.SetMaterialRenderingMode(fadeObject.material, MaterialUtils.RenderingMode.Fade);
        }
      }

      if (!gameObject.activeSelf)
        gameObject.SetActive(true);
      fadeObjects.Add(fadeObject);
      return fadeObject;
    }

    /// <summary>
    /// 运行淡出动画
    /// </summary>
    /// <param name="gameObject">执行对象</param>
    /// <param name="timeInSecond">执行时间</param>
    /// <param name="hidden">执行完毕是否将对象设置为不激活</param>
    /// <param name="materials">执行材质数组</param>
    [LuaApiDescription("运行淡出动画")]
    [LuaApiParamDescription("gameObject", "执行对象")]
    [LuaApiParamDescription("timeInSecond", "执行时间")]
    [LuaApiParamDescription("materials", "执行材质数组")]
    [LuaApiParamDescription("hidden", "执行完毕是否将对象设置为不激活")]
    public FadeObject AddFadeOut2(GameObject gameObject, float timeInSecond, bool hidden, Material[] materials)
    {
      FadeObject fadeObject = FindFadeObjectByGameObject(gameObject, FadeType.FadeOut);
      if (fadeObject != null)
        fadeObjects.Remove(fadeObject);

      fadeObject = new FadeObject(this);
      fadeObject.gameObject = gameObject;
      fadeObject.timeInSecond = timeInSecond;
      fadeObject.alpha = 1;
      if (materials == null)
      {
        var meshRender = gameObject.GetComponent<MeshRenderer>();
        fadeObject.materials = meshRender.materials;
      }
      else fadeObject.materials = materials;
      fadeObject.fadeType = FadeType.FadeOut;
      if (fadeObject.materials != null && fadeObject.materials.Length > 0)
        foreach (Material m in fadeObject.materials)
          m.color = new Color(m.color.r, m.color.g, m.color.b, 1);
      fadeObject.endReactive = hidden;
      fadeObjects.Add(fadeObject);
      return fadeObject;
    }
    /// <summary>
    /// 运行淡入动画
    /// </summary>
    /// <param name="gameObject">执行对象</param>
    /// <param name="timeInSecond">执行时间</param>
    /// <param name="material">执行材质</param>
    [LuaApiDescription("运行淡入动画")]
    [LuaApiParamDescription("gameObject", "执行对象")]
    [LuaApiParamDescription("timeInSecond", "执行时间")]
    [LuaApiParamDescription("materials", "执行材质数组")]
    public FadeObject AddFadeIn2(GameObject gameObject, float timeInSecond, Material[] materials)
    {
      FadeObject fadeObject = FindFadeObjectByGameObject(gameObject, FadeType.FadeIn);
      if (fadeObject != null)
        fadeObjects.Remove(fadeObject);

      fadeObject = new FadeObject(this);
      fadeObject.gameObject = gameObject;
      fadeObject.timeInSecond = timeInSecond;
      fadeObject.alpha = 0;
      if (materials == null)
      {
        var meshRender = gameObject.GetComponent<MeshRenderer>();
        fadeObject.materials = meshRender.materials;
      }
      else fadeObject.materials = materials;
      fadeObject.fadeType = FadeType.FadeIn;
      if (fadeObject.materials != null && fadeObject.materials.Length > 0)
        foreach (Material m in fadeObject.materials)
          m.color = new Color(m.color.r, m.color.g, m.color.b, 0);
      if (!gameObject.activeSelf)
        gameObject.SetActive(true);
      fadeObjects.Add(fadeObject);
      return fadeObject;
    }

    /// <summary>
    /// 运行淡出动画
    /// </summary>
    /// <param name="image">执行对象</param>
    /// <param name="timeInSecond">执行时间</param>
    /// <param name="hidden">执行完毕是否将对象设置为不激活</param>
    [LuaApiDescription("运行淡出动画")]
    [LuaApiParamDescription("hidden", "执行完毕是否将对象设置为不激活")]
    [LuaApiParamDescription("image", "执行对象")]
    [LuaApiParamDescription("timeInSecond", "执行时间")]
    public FadeObject AddFadeOut(Image image, float timeInSecond, bool hidden)
    {
      if (image != null)
      {
        FadeObject fadeObject = FindFadeObjectByImage(image, FadeType.FadeOut);
        if (fadeObject != null)
          fadeObjects.Remove(fadeObject);

        fadeObject = new FadeObject(this);
        fadeObject.gameObject = image.gameObject;
        fadeObject.timeInSecond = timeInSecond;
        fadeObject.alpha = 1;
        fadeObject.material = null;
        fadeObject.image = image;
        fadeObject.fadeType = FadeType.FadeOut;
        fadeObject.endReactive = hidden;
        fadeObjects.Add(fadeObject);

        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        return fadeObject;
      }
      return null;
    }
    /// <summary>
    /// 运行淡入动画
    /// </summary>
    /// <param name="image">执行对象</param>
    /// <param name="timeInSecond">执行时间</param>
    [LuaApiDescription("运行淡入动画")]
    [LuaApiParamDescription("image", "执行对象")]
    [LuaApiParamDescription("timeInSecond", "执行时间")]
    public FadeObject AddFadeIn(Image image, float timeInSecond)
    {
      if (image != null)
      {
        FadeObject fadeObject = FindFadeObjectByImage(image, FadeType.FadeIn);
        if (fadeObject != null)
          fadeObjects.Remove(fadeObject);

        fadeObject = new FadeObject(this);
        fadeObject.gameObject = image.gameObject;
        fadeObject.timeInSecond = timeInSecond;
        fadeObject.alpha = 0;
        fadeObject.material = null;
        fadeObject.image = image;
        fadeObject.fadeType = FadeType.FadeIn;
        fadeObjects.Add(fadeObject);

        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        if (!image.gameObject.activeSelf)
          image.gameObject.SetActive(true);
        return fadeObject;
      }
      return null;
    }

    /// <summary>
    /// 运行淡出动画
    /// </summary>
    /// <param name="text">执行对象</param>
    /// <param name="timeInSecond">执行时间</param>
    /// <param name="hidden">执行完毕是否将对象设置为不激活</param>
    [LuaApiDescription("运行淡出动画")]
    [LuaApiParamDescription("text", "执行对象")]
    [LuaApiParamDescription("timeInSecond", "执行时间")]
    [LuaApiParamDescription("hidden", "执行完毕是否将对象设置为不激活")]
    public FadeObject AddFadeOut(Text text, float timeInSecond, bool hidden)
    {
      if (text != null)
      {
        FadeObject fadeObject = FindFadeObjectByText(text, FadeType.FadeOut);
        if (fadeObject != null)
          fadeObjects.Remove(fadeObject);

        fadeObject = new FadeObject(this);
        fadeObject.gameObject = text.gameObject;
        fadeObject.timeInSecond = timeInSecond;
        fadeObject.alpha = 1;
        fadeObject.material = null;
        fadeObject.text = text;
        fadeObject.fadeType = FadeType.FadeOut;
        fadeObject.endReactive = hidden;
        fadeObjects.Add(fadeObject);

        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        return fadeObject;
      }
      return null;
    }
    /// <summary>
    /// 运行淡入动画
    /// </summary>
    /// <param name="text">执行对象</param>
    /// <param name="timeInSecond">执行时间</param>
    [LuaApiDescription("运行淡入动画")]
    [LuaApiParamDescription("text", "执行对象")]
    [LuaApiParamDescription("timeInSecond", "执行时间")]
    public FadeObject AddFadeIn(Text text, float timeInSecond)
    {
      if (text != null)
      {
        FadeObject fadeObject = FindFadeObjectByText(text, FadeType.FadeIn);
        if (fadeObject != null)
          fadeObjects.Remove(fadeObject);

        fadeObject = new FadeObject(this);
        fadeObject.gameObject = text.gameObject;
        fadeObject.timeInSecond = timeInSecond;
        fadeObject.alpha = 0;
        fadeObject.material = null;
        fadeObject.text = text;
        fadeObject.fadeType = FadeType.FadeIn;
        fadeObjects.Add(fadeObject);

        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        if (!text.gameObject.activeSelf)
          text.gameObject.SetActive(true);
        return fadeObject;
      }
      return null;
    }

    /// <summary>
    /// 运行声音淡出
    /// </summary>
    /// <param name="audio">执行对象</param>
    /// <param name="timeInSecond">执行时间</param>
    [LuaApiDescription("运行声音淡出")]
    [LuaApiParamDescription("audio", "执行对象")]
    [LuaApiParamDescription("timeInSecond", "执行时间")]
    public FadeObject AddAudioFadeOut(AudioSource audio, float timeInSecond)
    {
      if (audio != null)
      {
        FadeObject fadeObject = FindFadeObjectByAudio(audio, FadeType.FadeOut);
        if (fadeObject != null)
          fadeObjects.Remove(fadeObject);

        fadeObject = new FadeObject(this);
        fadeObject.gameObject = audio.gameObject;
        fadeObject.timeInSecond = timeInSecond;
        fadeObject.alpha = 1;
        fadeObject.material = null;
        fadeObject.audio = audio;
        fadeObject.fadeType = FadeType.FadeOut;
        fadeObject.endReactive = false;
        fadeObjects.Add(fadeObject);

        audio.volume = 1;
        return fadeObject;
      }
      return null;
    }
    /// <summary>
    /// 运行声音淡入
    /// </summary>
    /// <param name="audio">执行对象</param>
    /// <param name="timeInSecond">执行时间</param>
    [LuaApiDescription("运行声音淡入")]
    [LuaApiParamDescription("audio", "执行对象")]
    [LuaApiParamDescription("timeInSecond", "执行时间")]
    public FadeObject AddAudioFadeIn(AudioSource audio, float timeInSecond)
    {
      if (audio != null)
      {
        FadeObject fadeObject = FindFadeObjectByAudio(audio, FadeType.FadeIn);
        if (fadeObject != null)
          fadeObjects.Remove(fadeObject);

        fadeObject = new FadeObject(this);
        fadeObject.gameObject = audio.gameObject;
        fadeObject.timeInSecond = timeInSecond;
        fadeObject.alpha = 0;
        fadeObject.material = null;
        fadeObject.audio = audio;
        fadeObject.fadeType = FadeType.FadeIn;
        fadeObjects.Add(fadeObject);

        audio.volume = 0;
        if (!audio.gameObject.activeSelf)
          audio.gameObject.SetActive(true);
        return fadeObject;
      }
      return null;
    }


  }

  /// <summary>
  /// 渐变类型
  /// </summary>
  [LuaApiDescription("渐变类型")]
  [SLua.CustomLuaClass]
  public enum FadeType
  {
    None,
    FadeIn,
    FadeOut,
  }
  /// <summary>
  /// 渐变管理对象信息
  /// </summary>
  [LuaApiDescription("渐变管理对象信息")]
  [SLua.CustomLuaClass]
  public class FadeObject
  {
    internal FadeObject(UIFadeManager fadeManager) {
      this.fadeManager = fadeManager;
    }

    [LuaApiDescription("渐变所属管理器")]
    public UIFadeManager fadeManager { get; }
    [LuaApiDescription("渐变所属对象")]
    public GameObject gameObject;
    [LuaApiDescription("渐变控制材质")]
    public Material material;
    [LuaApiDescription("渐变控制材质数组")]
    public Material[] materials;
    [LuaApiDescription("渐变控制UI图像")]
    public Image image;
    [LuaApiDescription("渐变控制UI文字")]
    public Text text;
    [LuaApiDescription("渐变控制音量")]
    public AudioSource audio;
    [LuaApiDescription("当前透明度值")]
    public float alpha;
    [LuaApiDescription("渐变时长(秒)")]
    public float timeInSecond;
    [LuaApiDescription("结束后是否重新激活对象")]
    public bool endReactive;
    [LuaApiDescription("渐变是否已经结束")]
    public bool runEnd = false;
    [LuaApiDescription("渐变开始之前的材质渲染类型")]
    public MaterialUtils.RenderingMode oldMatRenderMode = MaterialUtils.RenderingMode.Fade;
    [LuaApiDescription("渐变类型")]
    public FadeType fadeType;

    /// <summary>
    /// 强制重置当前渐变透明度至指定的值
    /// </summary>
    /// <param name="alpha">透明度值</param>
    [LuaApiDescription("强制重置当前渐变透明度至指定的值")]
    [LuaApiParamDescription("alpha", "透明度值")]
    public void ResetTo(float alpha) {
      this.alpha = alpha;

      if (material != null)
        material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);
      else if (materials != null && materials.Length > 0)
      {
        foreach (Material m in materials)
          m.color = new Color(m.color.r, m.color.g, m.color.b, alpha);
      }
      else if (image != null)
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
      else if (text != null)
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
      else if (audio != null)
        audio.volume = alpha;
    }
    /// <summary>
    /// 移除当前渐变
    /// </summary>
    [LuaApiDescription("移除当前渐变")]
    public void Delete() {
      fadeManager.fadeObjects.Remove(this);
    }
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * 渐变自管理类
 *  负责自动化脚本的管理与执行
 */

namespace Ballance2.Sys.UI.Utils
{
    [SLua.CustomLuaClass]
    /// <summary>
    /// 渐变自管理类
    /// </summary>
    public class UIFadeManager : MonoBehaviour
    {
        private List<FadeObject> fadeObjects = new List<FadeObject>();

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
                                    foreach(Material m in fadeObject.materials)
                                        m.color = new Color(m.color.r, m.color.g, m.color.b, fadeObject.alpha);
                                }
                                else if (fadeObject.image != null)
                                    fadeObject.image.color = new Color(fadeObject.image.color.r, fadeObject.image.color.g, fadeObject.image.color.b, fadeObject.alpha);
                                else if (fadeObject.text != null)
                                    fadeObject.text.color = new Color(fadeObject.text.color.r, fadeObject.text.color.g, fadeObject.text.color.b, fadeObject.alpha);

                            }
                            else
                            {
                                if (fadeObject.endReactive)
                                {
                                    if (fadeObject.gameObject != null)
                                        fadeObject.gameObject.SetActive(false);
                                }
                                fadeObject.runEnd = true;
                            }
                        }
                    }
                }
            }
        }

        [SLua.CustomLuaClass]
        public enum FadeType
        {
            None,
            FadeIn,
            FadeOut,
        }
        [SLua.CustomLuaClass]
        public class FadeObject
        {
            public GameObject gameObject;
            public Material material;
            public Material[] materials;
            public Image image;
            public Text text;
            public float alpha;
            public float timeInSecond;
            public bool endReactive;
            public bool runEnd = false;
            public FadeType fadeType;
        }

        private FadeObject FindFadeObjectByGameObject(GameObject gameObject, FadeType fadeType)
        {
            foreach(FadeObject o in fadeObjects)
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

        /// <summary>
        /// 运行淡出动画
        /// </summary>
        /// <param name="gameObject">执行对象</param>
        /// <param name="timeInSecond">执行时间</param>
        /// <param name="hidden">执行完毕是否将对象设置为不激活</param>
        /// <param name="material">执行材质</param>
        public FadeObject AddFadeOut(GameObject gameObject, float timeInSecond, bool hidden, Material material)
        {
            FadeObject fadeObject = FindFadeObjectByGameObject(gameObject, FadeType.FadeOut);
            if (fadeObject != null)
                fadeObjects.Remove(fadeObject);

            fadeObject = new FadeObject();
            fadeObject.gameObject = gameObject;
            fadeObject.timeInSecond = timeInSecond;
            fadeObject.alpha = 1;
            fadeObject.material = material;
            fadeObject.fadeType = FadeType.FadeOut;
            if (material != null)
                material.color = new Color(material.color.r, material.color.g, material.color.b, 1);
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
        public FadeObject AddFadeIn(GameObject gameObject, float timeInSecond, Material material)
        {
            FadeObject fadeObject = FindFadeObjectByGameObject(gameObject, FadeType.FadeIn);
            if(fadeObject != null)
                fadeObjects.Remove(fadeObject);

            fadeObject = new FadeObject();
            fadeObject.gameObject = gameObject;
            fadeObject.timeInSecond = timeInSecond;
            fadeObject.alpha = 0;
            fadeObject.material = material;
            fadeObject.fadeType = FadeType.FadeIn;
            if (material != null)
                material.color = new Color(material.color.r, material.color.g, material.color.b, 0);
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
        /// <param name="material">执行材质</param>
        public FadeObject AddFadeOut(GameObject gameObject, float timeInSecond, bool hidden, Material[] materials)
        {
            FadeObject fadeObject = FindFadeObjectByGameObject(gameObject, FadeType.FadeOut);
            if (fadeObject != null)
                fadeObjects.Remove(fadeObject);

            fadeObject = new FadeObject();
            fadeObject.gameObject = gameObject;
            fadeObject.timeInSecond = timeInSecond;
            fadeObject.alpha = 1;
            fadeObject.materials = materials;
            fadeObject.fadeType = FadeType.FadeOut;
            if (materials != null && materials.Length > 0)
                foreach (Material m in materials)
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
        public FadeObject AddFadeIn(GameObject gameObject, float timeInSecond, Material[] materials)
        {
            FadeObject fadeObject = FindFadeObjectByGameObject(gameObject, FadeType.FadeIn);
            if (fadeObject != null)
                fadeObjects.Remove(fadeObject);

            fadeObject = new FadeObject();
            fadeObject.gameObject = gameObject;
            fadeObject.timeInSecond = timeInSecond;
            fadeObject.alpha = 0;
            fadeObject.materials = materials;
            fadeObject.fadeType = FadeType.FadeIn;
            if (materials != null && materials.Length > 0)
                foreach (Material m in materials)
                    m.color = new Color(m.color.r, m.color.g, m.color.b, 0);
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
        /// <param name="material">执行材质</param>
        public FadeObject AddFadeOut(Image image, float timeInSecond, bool hidden)
        {
            if (image != null)
            {
                FadeObject fadeObject = FindFadeObjectByImage(image, FadeType.FadeOut);
                if (fadeObject != null)
                    fadeObjects.Remove(fadeObject);

                fadeObject = new FadeObject();
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
        /// <param name="gameObject">执行对象</param>
        /// <param name="timeInSecond">执行时间</param>
        /// <param name="material">执行材质</param>
        public FadeObject AddFadeIn(Image image, float timeInSecond)
        {
            if (image != null)
            {
                FadeObject fadeObject = FindFadeObjectByImage(image, FadeType.FadeIn);
                if (fadeObject != null)
                    fadeObjects.Remove(fadeObject);

                fadeObject = new FadeObject();
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
        /// <param name="gameObject">执行对象</param>
        /// <param name="timeInSecond">执行时间</param>
        /// <param name="hidden">执行完毕是否将对象设置为不激活</param>
        /// <param name="material">执行材质</param>
        public FadeObject AddFadeOut(Text text, float timeInSecond, bool hidden)
        {
            if (text != null)
            {
                FadeObject fadeObject = FindFadeObjectByText(text, FadeType.FadeOut);
                if (fadeObject != null)
                    fadeObjects.Remove(fadeObject);

                fadeObject = new FadeObject();
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
        /// <param name="gameObject">执行对象</param>
        /// <param name="timeInSecond">执行时间</param>
        /// <param name="material">执行材质</param>
        public FadeObject AddFadeIn(Text text, float timeInSecond)
        {
            if (text != null)
            {
                FadeObject fadeObject = FindFadeObjectByText(text, FadeType.FadeIn);
                if (fadeObject != null)
                    fadeObjects.Remove(fadeObject);

                fadeObject = new FadeObject();
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


    }
}
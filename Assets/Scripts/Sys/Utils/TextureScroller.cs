using SLua;
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
* TODO: 请移到其他地方
* 作者：
* mengyu
*/

namespace Ballance2.Sys.Utils
{
    [CustomLuaClass]
    public class TextureScroller : MonoBehaviour
    {
        public Material Material;
        public Vector2 ScrollSpeed;

        private Vector2 scrollOff;

        private void Start() {
            if(Material == null)
            {
                var renderer = GetComponent<Renderer>();
                if(renderer)
                    Material = renderer.material;
            }
            if(Material != null) {
                scrollOff = Material.GetTextureOffset("_MainTex");
            }
        }
        private void Update() {
            if(Material != null) {
                var x = scrollOff.x + ScrollSpeed.x * Time.deltaTime;
                var y = scrollOff.y + ScrollSpeed.y * Time.deltaTime;
                if(x < 1) x += 1; if(x > 1) x -= 1;
                if(y < 1) y += 1; if(y > 1) y -= 1;
                scrollOff = new Vector2(x, y);
                Material.SetTextureOffset("_MainTex", scrollOff);
            }
        }
    }
}
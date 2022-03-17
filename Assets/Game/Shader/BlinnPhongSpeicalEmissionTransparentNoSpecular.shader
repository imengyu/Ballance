Shader "LikeVirtools/BlinnPhongSpeicalEmissionTransparentNoSpecular"
{
  Properties
  {
    _AmbientColor ("Ambient", Color) = (0,0,0,1)
    _Color ("Diffuse", Color) = (1,1,1,1)
    _MainTex ("Texture", 2D) = "white" {}
    _Emission ("Emission", Color) = (0,0,0,1)
  }
  SubShader
  {
    Tags {  
      "Queue" = "Transparent" 
      "RenderType" = "Transparent"
      "IgnoreProjector" = "True"
    }

    LOD 100

    Pass
    {
      // 开启深度写入
      ZWrite On
      // 设置颜色通道的写掩码，0为不写入任何颜色
      ColorMask 0
    }

    Pass
    {
      Blend SrcAlpha OneMinusSrcAlpha
      ZWrite Off

      Name "FORWARD" 
      Tags { 
        "LightMode" = "ForwardBase" 
      }
      
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      // make fog work
      #pragma multi_compile_fog
      #pragma multi_compile_fwdbase
      #pragma multi_compile LIGHTPROBE_SH
      #define UNITY_PASS_FORWARDBASE
      #define NO_SPECULAR
      #define USE_TRANSPARENT


      //引入头文件
      #include "UnityCG.cginc"
      #include "Lighting.cginc"
      #include "AutoLight.cginc"
      #include "BlinnPhongSpeicalEmission.cginc"

      VertexOutputBase vert (VertexInput v) { return vertForwardBase(v); }
      fixed4 frag (VertexOutputBase i) : SV_Target { return fragForwardBase(i); }

      ENDCG
    }
  }
}

Shader "LikeVirtools/BlinnPhongSpeicalEmissionTransparent"
{
  Properties
  {
    _AmbientColor ("Ambient", Color) = (0,0,0,1)
    _Color ("Diffuse", Color) = (1,1,1,1)
    _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
    _Gloss ("Gloss", float) = 0
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

      Tags { "LightMode" = "ForwardBase" }

      CGPROGRAM
			#pragma target 3.0
      #pragma multi_compile_fog
      #pragma multi_compile_fwdbase
      #pragma multi_compile LIGHTPROBE_SH
      #pragma vertex vert
      #pragma fragment frag
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
  Fallback "VertexLit"
}

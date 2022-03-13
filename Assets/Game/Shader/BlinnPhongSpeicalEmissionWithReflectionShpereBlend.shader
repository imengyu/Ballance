Shader "LikeVirtools/BlinnPhongSpeicalEmissionWithReflectionShpereBlend"
{
  Properties
  {
    _AmbientColor ("Ambient", Color) = (0,0,0,1)
    _Color ("Diffuse", Color) = (1,1,1,1)
    _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
    _Gloss ("Gloss", float) = 0
    _MainTex ("Texture", 2D) = "white" {}
    _Emission ("Emission", Color) = (0,0,0,1)
    _ReflectionTex ("Reflection Texture", 2D) = "white" {}
  }
  SubShader
  {
    Tags {  "Queue"="Geometry" "RenderType" = "Opaque" }
    LOD 100

    Pass
    {
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
      #define USE_SPHERE
      #define USE_SPHERE_REFLECTION

      //引入头文件
      #include "UnityCG.cginc"
      #include "Lighting.cginc"
      #include "AutoLight.cginc"
      #include "BlinnPhongSpeicalEmission.cginc"

      VertexOutputBase vert (VertexInput v) { return vertForwardBase(v); }
      fixed4 frag (VertexOutputBase i) : SV_Target { return fragForwardBase(i); }

      ENDCG
    }
    Pass
    {
      Name "FORWARD_DELTA"
      Tags { 
        "LightMode" = "ForwardAdd"
      }

      Blend One One
			ZWrite Off
      Fog { Color (0,0,0,0) }

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_fwdadd 
      #define UNITY_PASS_FORWARDADD
      #define USE_SPHERE
      #define USE_SPHERE_REFLECTION

      //引入头文件
      #include "UnityCG.cginc"
      #include "Lighting.cginc"
      #include "AutoLight.cginc"
      #include "BlinnPhongSpeicalEmission.cginc"

      VertexOutputBase vert (VertexInput v) { return vertForwardAdd(v); }
      fixed4 frag (VertexOutputBase i) : SV_Target { return fragForwardAdd(i); }

      ENDCG
    }
  }
  Fallback "VertexLit"
}

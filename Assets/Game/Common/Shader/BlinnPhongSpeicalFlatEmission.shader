Shader "LikeVirtools/BlinnPhongSpeicalFlatEmission"
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
      "Queue" = "Geometry" 
      "RenderType" = "Opaque"
    }
    LOD 100

    Pass
    {
      Name "FORWARD" 

      Tags { "LightMode" = "ForwardBase" }

      CGPROGRAM
	#pragma target 3.0
      #pragma multi_compile_fog
      #pragma multi_compile_fwdbase
      #pragma multi_compile LIGHTPROBE_SH
      #pragma vertex vert
      #pragma fragment frag
      #define FLAT_EMISSION

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
      #pragma target 3.0
      #pragma multi_compile_fwdadd
      #pragma vertex vert
      #pragma fragment frag
      #define USING_LIGHT_MULTI_COMPILE
      #define FLAT_EMISSION

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

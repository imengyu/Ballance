Shader "LikeVirtools/BlinnPhongNormalEmission" {
	
  Properties {
    _AmbientColor ("Ambient", Color) = (0,0,0,1)
    _Color ("Diffuse", Color) = (1,1,1,1)
    _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
    _SpecPower ("Specular Power", float) = 0
    _Gloss ("Gloss", float) = 0
    _MainTex ("Texture", 2D) = "white" {}
    _BumpMap ("Normal Map", 2D) = "bump" {}
    _Emission ("Emission", 2D) = "white" {}
    _EmissionColor ("Emission Color", Color) = (0,0,0,1)
  }

  SubShader {
    //----------------------------------------------

    Tags { "RenderType" = "Opaque" }

    Cull Off
    LOD 200

    CGPROGRAM
    #pragma surface surf BlinnPhong
    #pragma target 3.0

    //----------------------------------------------

    sampler2D _MainTex;
    sampler2D _Emission;
    sampler2D _BumpMap;
    samplerCUBE _Cube;
    fixed4 _Color;
    fixed4 _AmbientColor;
    fixed4 _EmissionColor;
    half _SpecPower;
    half _Gloss;
    //----------------------------------------------

    struct Input {
      float2 uv_MainTex;
      float2 uv_Emission;
      float2 uv_BumpMap;
      float3 viewDir;
      float3 worldRefl;
      INTERNAL_DATA
    };

    //----------------------------------------------

    void surf (Input IN, inout SurfaceOutput o) {
      fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
      o.Albedo = c.rgb + _AmbientColor;
      o.Gloss = _Gloss;
      o.Specular = _SpecPower;
      o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
      o.Emission = tex2D(_Emission, IN.uv_Emission) * _EmissionColor;
    }

    //----------------------------------------------

    ENDCG
  }


  Fallback "Legacy Shaders/VertexLit"
}

Shader "LikeVirtools/BlinnPhongSpeicalEmissionNoSpecular"
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
    Tags { "RenderType"="Opaque" }
    LOD 100

    Pass
    {
      Tags { "LightingMode" = "ForwardBase" }

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      // make fog work
      #pragma multi_compile_fog

      //引入头文件
      #include "UnityCG.cginc"
      #include "Lighting.cginc"

      struct appdata
      {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float2 uv : TEXCOORD0;
      };

      struct v2f
      {
        float2 uv : TEXCOORD0;
        float3 worldNormal : NORMAL;
        float4 vertex : SV_POSITION;
        UNITY_FOG_COORDS(2)
      };

      sampler2D _MainTex;
      float4 _MainTex_ST;
      fixed4 _Color;
      fixed4 _AmbientColor;
      fixed4 _Emission;
      float _Gloss;

      v2f vert (appdata v)
      {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
        float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
        UNITY_TRANSFER_FOG(o,o.vertex);
        return o;
      }

      fixed4 frag (v2f i) : SV_Target
      { 
        fixed4 col = tex2D(_MainTex, i.uv) * _Color;
        fixed3 ambient = (UNITY_LIGHTMODEL_AMBIENT.xyz + _AmbientColor) * col;
        fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);
        fixed3 worldNormal = normalize(i.worldNormal);
        fixed3 diffuse = _LightColor0.rgb * col.rgb * saturate(dot(worldNormal, worldLight));
        fixed3 color = max(diffuse, _Emission.rgb * col.rgb * _Emission.a) + ambient;
        // apply fogsd
        UNITY_APPLY_FOG(i.fogCoord, color);
        return fixed4(color, 1.0);
      }
      ENDCG
    }
  }
}

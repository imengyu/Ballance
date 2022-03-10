Shader "LikeVirtools/BlinnPhongSpeicalEmissionShpere"
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
    Tags { "RenderType"="Opaque" }
    LOD 100

    Pass
    {
      Tags { "LightingMode" = "ForwardBase" }

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_fog

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
				float3 viewDir : TEXCOORD1;
        float4 vertex : SV_POSITION;
				float2 uv2 : TEXCOORD2;
        UNITY_FOG_COORDS(3)
      };

      sampler2D _MainTex;
      float4 _MainTex_ST;
			fixed4 _Color;
			fixed4 _AmbientColor;
			fixed4 _Emission;
			float _Gloss;

      float2 GetUV(float3 r)
      {
        float m = sqrt(r.x * r.x + r.y * r.y + (r.z + 1.0) * (r.z + 1.0)); 
        float3 n = float3(r.x / m, r.y / m, r.z / m);
        return float2(0.5 * n.x + 0.5,0.5 * n.y + 0.5);
      }

      v2f vert (appdata v)
      {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
        
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.viewDir = _WorldSpaceCameraPos - worldPos;
				
        float3 posEyeSpace = mul(UNITY_MATRIX_MV,v.vertex).xyz;
        float3 I = posEyeSpace - float3(0,0,0);
        float3 N = mul((float3x3)UNITY_MATRIX_MV,v.normal);
        N = normalize(N);
        float3 R = reflect(I,N);
        o.uv2 = GetUV(R);

        UNITY_TRANSFER_FOG(o,o.vertex);
        return o;
      }

      fixed4 frag (v2f i) : SV_Target
      { 
        fixed4 col = tex2D(_MainTex, i.uv2) * _Color;
				fixed3 ambient = (UNITY_LIGHTMODEL_AMBIENT.xyz + _AmbientColor) * col;
				fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 diffuse = _LightColor0.rgb * col.rgb * saturate(dot(worldNormal, worldLight));
				fixed3 viewDir = normalize(i.viewDir);
				fixed3 halfDir = normalize(worldLight + viewDir);
				fixed3 specular = _LightColor0.rgb * _SpecColor.rgb * pow(saturate(dot(halfDir, worldNormal)), _Gloss);
				fixed3 color = max(diffuse, _Emission.rgb * col.rgb * _Emission.a) + ambient + specular;
        UNITY_APPLY_FOG(i.fogCoord, color);
        return fixed4(color, 1.0);
      }
      ENDCG
    }
  }
}

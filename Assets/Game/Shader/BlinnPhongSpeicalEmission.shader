Shader "LikeVirtools/BlinnPhongSpeicalEmission"
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
        UNITY_FOG_COORDS(1)
        float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;
        float4 vertex : SV_POSITION;
      };

      sampler2D _MainTex;
      float4 _MainTex_ST;
      //定义Properties中的变量
			fixed4 _Color;
			fixed4 _AmbientColor;
			fixed4 _Emission;
			float _Gloss;

      v2f vert (appdata v)
      {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//法线转化到世界空间
				o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
				//顶点位置转化到世界空间 
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				//计算视线方向（相机位置 - 像素对应位置）
				o.viewDir = _WorldSpaceCameraPos - worldPos;
        UNITY_TRANSFER_FOG(o,o.vertex);
        return o;
      }

      fixed4 frag (v2f i) : SV_Target
      { 
        // sample the texture
        fixed4 col = tex2D(_MainTex, i.uv) * _Color;
        //环境光
				fixed3 ambient = (UNITY_LIGHTMODEL_AMBIENT.xyz + _AmbientColor) * col;
				//世界空间下光线方向
				fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);
				//需要再次normalize
				fixed3 worldNormal = normalize(i.worldNormal);
				//计算Diffuse
				fixed3 diffuse = _LightColor0.rgb * col.rgb * saturate(dot(worldNormal, worldLight));
				//normalize
				fixed3 viewDir = normalize(i.viewDir);
				//计算半角向量（光线方向 + 视线方向，结果归一化）
				fixed3 halfDir = normalize(worldLight + viewDir);
				//计算Specular（Blinn-Phong计算的是）
				fixed3 specular = _LightColor0.rgb * _SpecColor.rgb * pow(saturate(dot(halfDir, worldNormal)), _Gloss);
				//结果为diffuse + ambient + specular
				fixed3 color = max(diffuse, _Emission.rgb * col.rgb * _Emission.a) + ambient + specular;
        // apply fog
        UNITY_APPLY_FOG(i.fogCoord, color);
        return fixed4(color, 1.0);
      }
      ENDCG
    }
  }
}

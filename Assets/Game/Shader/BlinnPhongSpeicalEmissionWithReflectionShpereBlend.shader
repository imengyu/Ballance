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
      sampler2D _ReflectionTex;
      float4 _ReflectionTex_ST;
      float4 _MainTex_ST;
			fixed4 _Color;
			fixed4 _AmbientColor;
			fixed4 _Emission;
			float _Gloss;

      float2 GetUV(float3 r)
      {
        // 开根号对过渡向量m求模（反射向量r+摄像机向量(0,0,1)为过渡向量m）
        float m = sqrt(r.x * r.x + r.y * r.y + (r.z + 1.0) * (r.z + 1.0)); 
        // 求过渡向量m的单位向量
        float3 n = float3(r.x / m, r.y / m, r.z / m);
        // 顶点值域为[-1,1],转为UV的值域[0,1]
        // 在边缘处UV信息也是更靠近图片里面，才形成镜面水晶球的效果吧
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
				
        // 将顶点信息转换到摄像机坐标
        float3 posEyeSpace = mul(UNITY_MATRIX_MV,v.vertex).xyz;
        // 获取摄像机的入射向量
        float3 I = posEyeSpace - float3(0,0,0);
        // 将顶点的法线向量转换到摄像机坐标
        float3 N = mul((float3x3)UNITY_MATRIX_MV,v.normal);
        // 求法线单位向量
        N = normalize(N);
        // 根据入射向量和法线向量求反射向量
        float3 R = reflect(I,N);
        // 根据反射向量获取最终的UV信息
        o.uv2 = GetUV(R);

        UNITY_TRANSFER_FOG(o,o.vertex);
        return o;
      }

      fixed4 frag (v2f i) : SV_Target
      { 
        fixed4 col = tex2D(_MainTex, i.uv) * _Color * tex2D(_ReflectionTex, i.uv2);
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

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
    Tags {
      "Queue" = "Geometry" 
      "RenderType" = "Opaque"
    }
    LOD 100

    Pass
    {
      Tags { 
        "LightingMode" = "ForwardBase"
      }

      CGPROGRAM
			#pragma target 3.0
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_fog

      //引入头文件
      #include "UnityCG.cginc"
      #include "Lighting.cginc"
      #include "AutoLight.cginc"

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
        float3 worldPos: TEXCOORD1;
        float4 vertex : SV_POSITION;
        UNITY_FOG_COORDS(2)
        SHADOW_COORDS(3)
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
        o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
        UNITY_TRANSFER_FOG(o,o.vertex);
        TRANSFER_SHADOW(o);
        return o;
      }

      fixed4 frag (v2f i) : SV_Target
      { 
        fixed4 albedo = tex2D(_MainTex, i.uv) * _Color;
        float3 worldLight = normalize(UnityWorldSpaceLightDir(i.worldPos.xyz));
        float3 worldView = normalize(UnityWorldSpaceViewDir(i.worldPos.xyz));
        fixed3 diffuse = _LightColor0.rgb * albedo.rgb * saturate(dot(i.worldNormal, worldLight));
        /*
        // sample the texture
        fixed4 albedo = tex2D(_MainTex, i.uv) * _Color;
        //环境光
        fixed3 ambient = (UNITY_LIGHTMODEL_AMBIENT.xyz + _AmbientColor) * albedo;
        //世界空间下光线方向
        float3 worldLight = normalize(UnityWorldSpaceLightDir(i.worldPos.xyz));
        float3 worldView = normalize(UnityWorldSpaceViewDir(i.worldPos.xyz));
        //需要再次normalize
        fixed3 worldNormal = normalize(i.worldNormal);
        //计算Diffuse
        fixed3 diffuse = _LightColor0.rgb * albedo.rgb * saturate(dot(worldNormal, worldLight));
        //计算半角向量（光线方向 + 视线方向，结果归一化）
        fixed3 halfDir = normalize(worldLight + worldView);
        //计算Specular（Blinn-Phong计算的是）
        fixed3 specular = _LightColor0.rgb * _SpecColor.rgb * pow(saturate(dot(halfDir, worldNormal)), _Gloss);
        */
        //光照
        //UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos.xyz);
        float atten = 1.0; 

        //结果为diffuse + ambient + specular
        //fixed3 color = (max(diffuse, _Emission.rgb * albedo.rgb * _Emission.a) + specular) * atten + ambient;
        fixed3 color = (diffuse + 0) * atten;
                
        // apply fog
        //UNITY_APPLY_FOG(i.fogCoord, color);
        return fixed4(color, 1.0);
      }
      ENDCG
    }
    
    Pass
    {
      Tags { 
        "LightMode" = "ForwardAdd"
      }
      
      Blend One One
			ZWrite Off

      CGPROGRAM
			#pragma target 3.0
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_fwdadd_fullshadows

      //引入头文件
      #include "UnityCG.cginc"
      #include "Lighting.cginc"
      #include "AutoLight.cginc"

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
        float3 worldPos: TEXCOORD1;
        float4 vertex : SV_POSITION;
        SHADOW_COORDS(2)
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
        o.worldNormal = UnityObjectToWorldNormal(v.normal);
        o.worldPos = mul(unity_ObjectToWorld, v.vertex);
        TRANSFER_SHADOW(o);
        return o;
      }

      fixed4 frag (v2f i) : SV_Target
      { 
        /*
        fixed4 albedo = tex2D(_MainTex, i.uv) * _Color;    

        float3 worldLight = normalize(UnityWorldSpaceLightDir(i.worldPos.xyz));
        float3 worldView = normalize(UnityWorldSpaceViewDir(i.worldPos.xyz));
        fixed3 worldNormal = normalize(i.worldNormal);

        fixed3 diffuse = _LightColor0.rgb * albedo.rgb * saturate(dot(worldNormal, worldLight));
        fixed3 halfDir = normalize(worldLight + worldView);
        fixed3 specular = _LightColor0.rgb * _SpecColor.rgb * pow(saturate(dot(halfDir, worldNormal)), _Gloss);
        */

        fixed4 albedo = tex2D(_MainTex, i.uv) * _Color;
        float3 worldLight = normalize(UnityWorldSpaceLightDir(i.worldPos.xyz));
        float3 worldView = normalize(UnityWorldSpaceViewDir(i.worldPos.xyz));
        fixed3 diffuse = _LightColor0.rgb * albedo.rgb * saturate(dot(i.worldNormal, worldLight));

        //UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos.xyz);
        float atten = 1.0; 

        fixed3 color = (diffuse + 0) * atten;

        return fixed4(color, 1.0);
      }
      ENDCG
    }
  }
  Fallback "VertexLit"
}

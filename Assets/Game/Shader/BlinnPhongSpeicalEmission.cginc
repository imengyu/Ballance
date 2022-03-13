// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

#ifndef BLINN_PHONG_NORMAL_EMISSION_INCLUDED
#define BLINN_PHONG_NORMAL_EMISSION_INCLUDED

#ifdef USE_SPHERE

float2 GetSphereUV(float3 r)
{
  // 开根号对过渡向量m求模（反射向量r+摄像机向量(0,0,1)为过渡向量m）
  float m = sqrt(r.x * r.x + r.y * r.y + (r.z + 1.0) * (r.z + 1.0)); 
  // 求过渡向量m的单位向量
  float3 n = float3(r.x / m, r.y / m, r.z / m);
  // 顶点值域为[-1,1],转为UV的值域[0,1]
  // 在边缘处UV信息也是更靠近图片里面，才形成镜面水晶球的效果吧
  return float2(0.5 * n.x + 0.5,0.5 * n.y + 0.5);
}

#endif

struct VertexInput
{
  float4 vertex : POSITION;
  float3 normal : NORMAL;
  float2 uv : TEXCOORD0;
  float2 texcoord1 : TEXCOORD1;
};

struct VertexOutputBase
{
  float2 uv : TEXCOORD0;
  float3 worldNormal : NORMAL;
  float3 worldPos: TEXCOORD1;
  float4 pos : SV_POSITION;
  UNITY_FOG_COORDS(2)
  LIGHTING_COORDS(3, 4)
  #ifdef LIGHTPROBE_SH
  fixed3 SHLighting : COLOR;
  #endif 
  #ifdef USE_SPHERE
  float2 uv2 : TEXCOORD5;
  #endif
};

sampler2D _MainTex;
float4 _MainTex_ST;

#ifdef USE_SPHERE_REFLECTION
sampler2D _ReflectionTex;
float4 _ReflectionTex_ST;
#endif
      
//定义Properties中的变量
fixed4 _Color;
fixed4 _AmbientColor;
fixed4 _Emission;
float _Gloss;

VertexOutputBase vertForwardAdd (VertexInput v) 
{
  VertexOutputBase o;
  o.pos = UnityObjectToClipPos(v.vertex);
  o.uv = TRANSFORM_TEX(v.uv, _MainTex);
  o.worldNormal = UnityObjectToWorldNormal(v.normal);
  o.worldPos = mul(unity_ObjectToWorld, v.vertex);

  #ifdef USE_SPHERE
  float3 posEyeSpace = mul(UNITY_MATRIX_MV,v.vertex).xyz;
  float3 I = posEyeSpace - float3(0,0,0);
  float3 N = mul((float3x3)UNITY_MATRIX_MV,v.normal);
  N = normalize(N);
  float3 R = reflect(I,N);
  o.uv2 = GetSphereUV(R);
  #endif

  return o;
}
half4 fragForwardAdd (VertexOutputBase i) : SV_Target // backward compatibility (this used to be the fragment entry function)
{
  #ifdef USE_SPHERE_MAP
  fixed4 albedo = tex2D(_MainTex, i.uv2) * _Color;
  #else
  // sample the texture
  fixed4 albedo = tex2D(_MainTex, i.uv) * _Color;
  #endif

  //球型环境贴图
  #ifdef USE_SPHERE_REFLECTION
  albedo *= tex2D(_ReflectionTex, i.uv2);
  #endif 

  float3 worldLight = normalize(UnityWorldSpaceLightDir(i.worldPos.xyz));
  float3 worldView = normalize(UnityWorldSpaceViewDir(i.worldPos.xyz));
  fixed3 worldNormal = normalize(i.worldNormal);

  fixed3 diffuse = _LightColor0.rgb *  albedo.rgb * saturate(dot(worldNormal, worldLight));

  UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos.xyz);

  #ifdef NO_SPECULAR
  fixed3 color = diffuse * atten * _LightColor0.rgb;
  #else
  fixed3 halfDir = normalize(worldLight + worldView);
  fixed3 specular = _LightColor0.rgb * _SpecColor.rgb * pow(saturate(dot(halfDir, worldNormal)), _Gloss * 0.5);
  fixed3 color = (diffuse + specular) * atten * _LightColor0.rgb;
  #endif
  return fixed4(color, 1.0);
}

VertexOutputBase vertForwardBase (VertexInput v) 
{
  VertexOutputBase o;
  o.pos = UnityObjectToClipPos(v.vertex);
  o.uv = TRANSFORM_TEX(v.uv, _MainTex);
  //法线转化到世界空间
  o.worldNormal = UnityObjectToWorldNormal(v.normal);
  //顶点位置转化到世界空间 
  o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
  
  #ifdef LIGHTPROBE_SH
  o.SHLighting = ShadeSH9(float4(o.worldNormal, 1));
  #endif

  #ifdef USE_SPHERE
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
  o.uv2 = GetSphereUV(R);
  #endif

  UNITY_TRANSFER_LIGHTING(o,o.pos)
  UNITY_TRANSFER_FOG(o,o.pos);
  return o;
}

half4 fragForwardBase (VertexOutputBase i) : SV_Target  // backward compatibility (this used to be the fragment entry function)
{
  #ifdef USE_SPHERE_MAP
  fixed4 albedo = tex2D(_MainTex, i.uv2) * _Color;
  #else
  // sample the texture
  fixed4 albedo = tex2D(_MainTex, i.uv) * _Color;
  #endif

  //球型环境贴图
  #ifdef USE_SPHERE_REFLECTION
  albedo *= tex2D(_ReflectionTex, i.uv2);
  #endif

  //环境光
  fixed3 ambient = (UNITY_LIGHTMODEL_AMBIENT.xyz + _AmbientColor) * albedo;
  
  #ifdef LIGHTPROBE_SH
  ambient.rgb *= i.SHLighting;
  #endif

  //世界空间下光线方向
  float3 worldLight = normalize(UnityWorldSpaceLightDir(i.worldPos.xyz));
  float3 worldView = normalize(UnityWorldSpaceViewDir(i.worldPos.xyz));
  //需要再次normalize
  fixed3 worldNormal = normalize(i.worldNormal);
  //计算Diffuse
  fixed3 diffuse = _LightColor0.rgb * albedo.rgb * saturate(dot(worldNormal, worldLight));

  #ifdef FLAT_EMISSION
  fixed3 emission = _Emission.rgb * _Emission.a;
  #else
  fixed3 emission = _Emission.rgb * albedo.rgb * _Emission.a;
  #endif

  //光照
  UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos.xyz);

  #ifdef NO_SPECULAR
  fixed3 color = max(diffuse, emission) * atten + ambient;
  #else
  //计算半角向量（光线方向 + 视线方向，结果归一化）
  fixed3 halfDir = normalize(worldLight + worldView);
  //计算Specular（Blinn-Phong计算的是）
  fixed3 specular = _LightColor0.rgb * _SpecColor.rgb * pow(saturate(dot(halfDir, worldNormal)), _Gloss);
  //结果为diffuse + ambient + specular
  fixed3 color = (max(diffuse, emission) + specular) * atten + ambient;
  #endif

  // apply fog
  UNITY_APPLY_FOG(i.fogCoord, color);

  #ifdef USE_TRANSPARENT
  return fixed4(color, albedo.a);
  #else
  return fixed4(color, 1.0);
  #endif
}

#endif // BLINN_PHONG_NORMAL_EMISSION_INCLUDED
// MIT license - https://nvjob.github.io/mit-license
// #NVJOB Nicholas Veselov - https://nvjob.github.io
// #NVJOB SC Shaders v1.3 - https://nvjob.github.io/unity/nvjob-sc-shaders


Shader "#NVJOB/SC Shaders/Only Reflection, Cull Off" {


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



Properties{
//----------------------------------------------

[Header(Basic Settings)][Space(5)]
_Color("Main Color", Color) = (1,1,1,1)
_Saturation("Saturation", Range(0, 5)) = 1
_Brightness("Brightness", Range(0, 5)) = 1
_Contrast("Contrast", Range(0, 5)) = 1
[Header(Specular Settings)][Space(5)]
_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
_Shininess("Shininess", Range(0.03, 1)) = 0.078125
[Header(Reflection Settings)][Space(5)]
_ReflectColor("Reflection Color", Color) = (1,1,1,0.5)
_Cube("Reflection Cubemap", Cube) = "" {}
_IntensityRef("Intensity Reflection", Range(0, 20)) = 1
_BiasNormal("Bias Normal", Range(-5, 5)) = 1

//----------------------------------------------
}



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



SubShader{
//----------------------------------------------

Tags{ "RenderType" = "Opaque" }
Cull Off
LOD 200
CGPROGRAM
#pragma surface surf BlinnPhong exclude_path:prepass nolppv noforwardadd interpolateview novertexlights

//----------------------------------------------

fixed4 _Color, _ReflectColor;
half _Shininess, _BiasNormal, _IntensityRef, _Saturation, _Contrast, _Brightness;
samplerCUBE _Cube;

//----------------------------------------------

struct Input {
float3 viewDir;
float3 worldRefl;
INTERNAL_DATA
};

//----------------------------------------------

void surf(Input IN, inout SurfaceOutput o) {
fixed4 tex = _Color;
float Lum = dot(tex, float3(0.2126, 0.7152, 0.0722));
half3 color = lerp(Lum.xxx, tex, _Saturation);
color = color * _Brightness;
o.Albedo = (color - 0.5) * _Contrast + 0.5;
o.Gloss = tex.a;
o.Specular = _Shininess;
o.Normal *= _BiasNormal;
fixed4 reflcol = texCUBE(_Cube, WorldReflectionVector(IN, o.Normal));
reflcol *= _IntensityRef;
reflcol *= tex.a;
o.Emission = reflcol.rgb * _ReflectColor.rgb;
}

//----------------------------------------------

ENDCG

///////////////////////////////////////////////////////////////////////////////////////////////////////////////
}


Fallback "Legacy Shaders/VertexLit"

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
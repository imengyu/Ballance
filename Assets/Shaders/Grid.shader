Shader "Unlit/Grid"
{
    Properties
    {
        _GridColour ("Grid Colour", color) = (1, 1, 1, 1)
        _BaseColour ("Base Colour", color) = (1, 1, 1, 0)
        _GridSpacing ("Grid Spacing", float) = 1
        _LineThickness ("Line Thickness", float) = .1
        _ODistance ("Start Transparency Distance", float) = 5
        _TDistance ("Full Transparency Distance", float) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct appdata
            {
                float4 vertex : POSITION;
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };
 
            fixed4 _GridColour;
            fixed4 _BaseColour;
            float _GridSpacing;
            float _LineThickness;
            float _ODistance;
            float _TDistance;
 
            v2f vert (appdata_full v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = o.worldPos.xz / _GridSpacing;
                return o;
            }
     
            fixed4 frag (v2f i) : SV_Target
            {        
                float2 wrapped = frac(i.uv) - 0.5f;
                float2 range = abs(wrapped);
                float2 speeds;
                speeds = fwidth(i.uv);
                float2 pixelRange = range/speeds;
                float lineWeight = saturate(min(pixelRange.x, pixelRange.y) - _LineThickness);
                half4 param = lerp(_GridColour, _BaseColour, lineWeight);
             
                //distance falloff
                half3 viewDirW = _WorldSpaceCameraPos - i.worldPos;
                half viewDist = length(viewDirW);
                half falloff = saturate((viewDist - _ODistance) / (_TDistance - _ODistance) );
                param.a *= (1.0f - falloff);
                return param;
            }
            ENDCG
        }
    }
}
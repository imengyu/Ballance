// Matcap shader credit: https://forum.unity.com/threads/getting-normals-relative-to-camera-view.452631/#post-2933684
Shader "Hidden/RuntimeSceneGizmo"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_AlphaVal ("Alpha", Float) = 1.0
		_Matcap ("Matcap", 2D) = "white"
	}

	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile __ HIGHLIGHT_ON
			
			#include "UnityCG.cginc"
			
			struct appdata
			{
				float4 vertex : POSITION;
				half3 normal : NORMAL;
				fixed4 color : COLOR;
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 cap : TEXCOORD0;
				fixed4 color : COLOR;
			};
			
			sampler2D _Matcap;
			fixed _AlphaVal;
			#if HIGHLIGHT_ON
			fixed4 _Color;
			#endif
			
			v2f vert( appdata v )
			{
				v2f o;
				o.pos = UnityObjectToClipPos( v.vertex );
				
				float3 worldNorm = UnityObjectToWorldNormal( v.normal );
				float3 viewNorm = mul( (float3x3) UNITY_MATRIX_V, worldNorm );
				
				o.cap = viewNorm.xy * 0.5 + 0.5;
				o.color = v.color;
				return o;
			}
			
			fixed4 frag( v2f i ) : SV_Target
			{
				fixed4 col = tex2D( _Matcap, i.cap );
				#if HIGHLIGHT_ON
				col *= _Color;
				#else
				col *= i.color;
				#endif
				
				col.a = _AlphaVal;
				return col;
			}
			
			ENDCG
		}
	}
}
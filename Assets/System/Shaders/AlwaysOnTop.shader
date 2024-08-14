Shader "Custom/AlwaysOnTop" {
  Properties {
      _Color ("Color", Color) = (1,1,1,1)
  }
  SubShader {
      Tags { "Queue"="Transparent" "RenderType"="Opaque" }
      LOD 100
      ZTest off

      Pass {

          CGPROGRAM
          #pragma vertex vert
          #pragma fragment frag
          #include "UnityCG.cginc"

          struct appdata {
              float4 vertex : POSITION;
          };

          struct v2f {
              float4 pos : SV_POSITION;
          };

          fixed4 _Color;

          v2f vert (appdata v) {
              v2f o;
              o.pos = UnityObjectToClipPos(v.vertex);
              return o;
          }

          fixed4 frag (v2f i) : SV_Target {
              return _Color;
          }
          ENDCG
      }
  }
}
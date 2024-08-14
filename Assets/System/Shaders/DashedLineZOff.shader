Shader "Custom/DashedLineZOff" {
  Properties {
      _Color ("Color", Color) = (1,1,1,1)
      _DashSize ("Dash Size", Float) = 10.0
      _GapSize ("Gap Size", Float) = 5.0
  }
  SubShader {
      Tags { "RenderType"="Opaque" }
      LOD 100
      ZTest off

      Pass {
          CGPROGRAM
          #pragma vertex vert
          #pragma fragment frag

          #include "UnityCG.cginc"

          struct appdata {
              float4 vertex : POSITION;
              float2 uv : TEXCOORD0;
          };

          struct v2f {
              float2 uv : TEXCOORD0;
              float4 vertex : SV_POSITION;
          };

          fixed4 _Color;
          float _DashSize;
          float _GapSize;

          v2f vert (appdata v) {
              v2f o;
              o.vertex = UnityObjectToClipPos(v.vertex);
              o.uv = v.uv;
              return o;
          }

          fixed4 frag (v2f i) : SV_Target {
              // 计算当前片段在UV空间的位置
              float lineLength = _DashSize + _GapSize;
              float dashPos = fmod(i.uv.y * lineLength, _DashSize * 2.0);

              // 判断当前片段是否在虚线范围内
              if (dashPos > _DashSize) {
                  discard;
              }

              // 返回颜色
              return _Color;
          }
          ENDCG
      }
  }
  FallBack "Diffuse"
}
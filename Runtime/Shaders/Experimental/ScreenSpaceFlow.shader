Shader "Neodroid/ScreenSpaceFlow" {
  Properties {
    _MainTex("", 2D) = ""{}
  }
  Subshader {
    Pass {
      //ZTest Always
      //Cull Off
      //ZWrite Off
      //ZWrite On

      CGPROGRAM
        #include "UnityCG.cginc"
        #pragma multi_compile _ UNITY_COLORSPACE_GAMMA
        #pragma vertex CommonVertex
        #pragma fragment OverlayFragment
        #pragma target 3.0

        sampler2D _MainTex;
        float4 _MainTex_TexelSize;
        float4 _MainTex_ST;

        struct CommonAttributes{
          float4 position : POSITION;
          float2 uv : TEXCOORD;
        };

        struct CommonVaryings{
          float4 position : SV_POSITION;
          half2 uv0 : TEXCOORD0; // Screen space UV (supports stereo rendering)
          half2 uv1 : TEXCOORD1; // Alternative UV (supports v-flip case)
        };

        CommonVaryings CommonVertex(CommonAttributes input){
          float2 uv1 = input.uv;

          #if UNITY_UV_STARTS_AT_TOP
              if (_MainTex_TexelSize.y < 0) uv1.y = 1 - uv1.y;
          #endif

          CommonVaryings o;
          o.position = UnityObjectToClipPos(input.position);
          o.uv0 = UnityStereoScreenSpaceUVAdjust(input.uv, _MainTex_ST);
          o.uv1 = UnityStereoScreenSpaceUVAdjust(uv1, _MainTex_ST);
          return o;
        }

        half _Amplitude = 10;
        half _Blending = 0.5;
        half4 _BackgroundColor = (1,1,1,1);

        sampler2D_half _CameraMotionVectorsTexture;

        half4 VectorToColor(float2 mv) { // Convert a motion vector into RGBA color.
          half phi = atan2(mv.x, mv.y);
          half hue = (phi / UNITY_PI + 1) * 0.5;

          half r = abs(hue * 6 - 3) - 1;
          half g = 2 - abs(hue * 6 - 2);
          half b = 2 - abs(hue * 6 - 4);
          half a = length(mv);

          return saturate(half4(r, g, b, a));
        }

        // Motion vectors overlay shader
        half4 OverlayFragment(CommonVaryings input) : SV_Target {
          half2 mv = tex2D(_CameraMotionVectorsTexture, input.uv1).rg * _Amplitude;
          half4 mc = VectorToColor(mv);

          half3 out_rgb = mc.rgb;
          #if !UNITY_COLORSPACE_GAMMA
            out_rgb = GammaToLinearSpace(out_rgb);
          #endif

          half mc_ratio = saturate(_Blending * 2);
          out_rgb = lerp(_BackgroundColor, out_rgb, mc.a * mc_ratio);

          return half4(out_rgb, 1);
        }
      ENDCG
    }
  }
}

Shader "Neodroid/Segmentation"{ //Draws objects with shader on top always
    SubShader{
      //Tags
      //{
      //  "RenderType"="Opaque" // What RenderType shaders to replace there is a copy of this exact shame shader below with the "Transparent" RenderType
      //}
      //Tags{
      //  "Queue" = "Transparent"
      //}

      //ZTest Always // Ignore what is the depth buffer draw pixels anyway
      //ZWrite Off // has same effect as above ^, this never write to the depth buffer
      Lighting Off // Turn off lighting
      //Cull Off // No culling

      Pass{
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"

        struct appdata
        {
          float4 vertex : POSITION;
          float2 uv : TEXCOORD0;
        };

        struct v2f
        {
          float2 uv : TEXCOORD0;
          float4 vertex : SV_POSITION;
        };

        fixed4 _Color;

        v2f vert(appdata v)
        {
          v2f o;
          o.vertex = UnityObjectToClipPos(v.vertex);
          o.uv = v.uv;

          return o;
        }

        fixed4 frag(v2f i) : SV_Target
        {
          return _Color;
        }
        ENDCG
      }

      //Blend One One // additive blend

//      Pass {
//        CGPROGRAM
//        #pragma vertex vert
//        #pragma fragment frag
//
//        #include "UnityCG.cginc"
//
//        struct appdata
//        {
//          float4 vertex : POSITION;
//        };
//
//        struct v2f
//        {
//          float4 vertex : SV_POSITION;
//        };
//
//        v2f vert(appdata v)
//        {
//          v2f o;
//          o.vertex = UnityObjectToClipPos(v.vertex);
//          return o;
//        }
//
//        half4 _SegmentationColor;
//
//        fixed4 frag(v2f i) : SV_Target
//        {
//          return _SegmentationColor;
//        }
//        ENDCG
//      }

      //Pass{
      //  ZTest Greater
      //  Color[_SegmentationColor]
      //}
      //Pass{
      //  ZTest Less
      //  Color[_SegmentationColor]
      //}
  }
}
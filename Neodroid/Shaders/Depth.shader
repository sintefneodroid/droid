Shader "Neodroid/Depth"{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
	}

	SubShader{
		Tags{
			"RenderType"="Opaque" // What RenderType shaders to replace there is a copy of this exact shame shader below with the "Transparent" RenderType
		}

		ZWrite On

		Pass{
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
				float depth : DEPTH;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z *_ProjectionParams.w;
        o.depth = -UnityObjectToViewPos(v.vertex).z *_ProjectionParams.w; //Faster according to unity
				return o;
			}

			half4 _Color;

			fixed4 frag (v2f i) : SV_Target
			{
				float invert = 1 - i.depth;

        //return fixed4(invert, invert, invert, 1) * _Color; // With color
        return fixed4(invert, invert, invert, 1);
			}
			ENDCG
		}
	}

//  SubShader
//  {
//    Tags
//    {
//      "RenderType"="Transparent" // Replaces shaders with "Transparent" as RenderType
//    }
//
//    ZWrite On
//
//    Pass
//    {
//      CGPROGRAM
//      #pragma vertex vert
//      #pragma fragment frag
//
//      #include "UnityCG.cginc"
//
//      struct appdata
//      {
//        float4 vertex : POSITION;
//      };
//
//      struct v2f
//      {
//        float4 vertex : SV_POSITION;
//        float depth : DEPTH;
//      };
//
//      v2f vert (appdata v)
//      {
//        v2f o;
//        o.vertex = UnityObjectToClipPos(v.vertex);
//        o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z *_ProjectionParams.w;
//        return o;
//      }
//
//      half4 _Color;
//
//      fixed4 frag (v2f i) : SV_Target
//      {
//        float invert = 1 - i.depth;
//
//        //return fixed4(invert, invert, invert, 1) * _Color; // With color
//        return fixed4(invert, invert, invert, 1);
//      }
//      ENDCG
//    }
//  }

	SubShader{
		Tags{
			"RenderType"="Transparent" // Replaces shaders with "Transparent" as RenderType
		}

		ZWrite Off // Dont show depth on transparent objects
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
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
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			half4 _Color;

			fixed4 frag (v2f i) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}
}

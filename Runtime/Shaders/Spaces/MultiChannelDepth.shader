Shader "Neodroid/Spaces/MutliChannelDepth"{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader{
        	Tags { "RenderType"="Opaque" }
	Pass {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct v2f {
                    float4 pos : SV_POSITION;
                    float4 nz : TEXCOORD0;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                inline float Linear01FromEyeToLinear01FromNear(float depth01) {
                    float near = _ProjectionParams.y;
                    float far = _ProjectionParams.z;
                    return (depth01 - near/far) * (1 + near/far);
                }

                float4 Output(float depth01, float3 normal) {
                    // MultiChannelDepth // (depth01*256)-floor(depth01*256) // #RGB24 is 8 bit per channel, 2**8 = 256
                    float lowBits = frac(depth01 * 256);
                    float highBits = depth01 - lowBits / 256;
                    return float4(lowBits, highBits, depth01, 1);

                }

                v2f vert( appdata_base v ) {
                    v2f o;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.nz.xyz = COMPUTE_VIEW_NORMAL;
                    o.nz.w = COMPUTE_DEPTH_01;
                                return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    return Output (i.nz.w, i.nz.xyz);
                }

            ENDCG
        }
    }
/*
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
	*/

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

		ZWrite Off // Do not show depth on transparent objects
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

Shader "Neodroid/Spaces/Model/Us" {
Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            o.Emission = float3(IN.uv_MainTex.r,0,1);
            o.Alpha = 0;
        }
        ENDCG
    }
/*
    SubShader {
        //Lighting Off
        //Cull Off
		//ZWrite Off
		//ZTest Always

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // vertex input: position, UV
            struct appdata {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex );
                o.uv = float4( v.texcoord.xy, 0, 0 );
                return o;
            }

            half4 frag( v2f i ) : SV_Target {
                half4 c = frac( i.uv );
                if (any(saturate(i.uv) - i.uv))
                    c.b = 0.5;
                return c;
            }
            ENDCG
        }
    }
*/
	fallback "Unlit/Color"
}

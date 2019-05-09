Shader "Neodroid/Segmentation/OutlinedSegmentation" {

	Properties {
		_SegmentationColor ("_SegmentationColor", Color) = (.5,.5,.5,1.)
		_OutlineColor ("_OutlineColor", Color) = (1.,.0,1.,1.)
		_OutlineWidthFactor ("_OutlineWidthFactor", Range (0, 2)) = 1.1
	}


    CGINCLUDE
        #include "UnityCG.cginc"

        struct appdata {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
        };

        struct v2f {
            float4 pos : POSITION;
            float4 color : COLOR;
        };

        uniform float _OutlineWidthFactor;
        uniform float4 _OutlineColor;

        v2f vert(appdata v) {
            v2f o;


				//float3 scaleDir = normalize(v.vertex.xyz - float4(0,0,0,1));

					v.vertex.xyz += normalize(v.normal.xyz) * _OutlineWidthFactor;

				//	v.vertex.xyz += scaleDir *_OutlineWidthFactor;

//            float outline_width = _OutlineWidthFactor/(1/UnityObjectToClipPos(v.vertex).z);
     //       v.vertex *= 1+outline_width;

            o.pos = UnityObjectToClipPos(v.vertex);

            o.color = _OutlineColor;
            return o;



        }
    ENDCG

	SubShader {
        Lighting Off

        CGPROGRAM
            #pragma surface surf NoLighting noforwardadd

        uniform float4 _SegmentationColor;

            struct Input {
                float2 uv_MainTex;
            };

            void surf (Input IN, inout SurfaceOutput o) {
                fixed4 c =  _SegmentationColor;
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }

            fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten){
                 fixed4 c;
                 c.rgb = s.Albedo;
                 c.a = s.Alpha;
                 return c;
             }
        ENDCG

		Pass { 		// note that a vertex shader is specified here but its using the one above
            Lighting Off
			Name "OUTLINE"
			Cull Front
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                half4 frag(v2f i) :COLOR {
                    return i.color;
                }
			ENDCG
		}
	}

	fallback "Unlit/Color"
}

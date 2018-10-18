Shader "Neodroid/Segmentation/Outlined" {

	Properties {
		_SegmentationColor ("_SegmentationColor", Color) = (.5,.5,.5,1.)
		_OutlineColor ("_OutlineColor", Color) = (1.,.0,1.,1.)
		_OutlineWidthFactor ("_OutlineWidthFactor", Range (0, 1)) = 0.3
		_SkipOutline ("_SkipOutline", Float) = 0
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
        uniform float _SkipOutline;

        v2f vert(appdata v) {
            v2f o;
            if(_SkipOutline==1){
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color =(.0,.0,.0,1.);
                return o;
            }

            float mult = 1/UnityObjectToClipPos(v.vertex).z;
            v.vertex *= 1+((_OutlineWidthFactor)/(mult));

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

	Fallback "Unlit"
}

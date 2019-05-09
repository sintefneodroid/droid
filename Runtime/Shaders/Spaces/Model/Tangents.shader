Shader "Neodroid/Spaces/Model/Tangents" {
    SubShader {
        Lighting Off
    	Cull Off
		//ZWrite Off
		//ZTest Always

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // vertex input: position, tangent
            struct appdata {
                float4 vertex : POSITION;
                float4 tangent : TANGENT;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex );
                o.color = v.tangent * 0.5 + 0.5;
                //o.color = .5 * (v.tangent + 1);
                o.color.w = 1.0;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                return i.color;
            }
            ENDCG
        }
    }
	fallback "Unlit/Color"
}

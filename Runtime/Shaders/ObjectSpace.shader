// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'


Shader "Neodroid/ObjectSpace" {

	Properties {
		//_XColor ("_XColor", Color) = (1,0,0,1.)
		//_YColor ("_YColor", Color) = (0,1,0,1.)
		//_ZColor ("_ZColor", Color) = (0,0,1,1.)
	}

    SubShader {
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
            float4 objPos: COLOR;
        };

        v2f vert (appdata v) {
            v2f o;
            o.pos = UnityObjectToClipPos (v.vertex);
            o.objPos = v.vertex;
            //o.objPos = mul(unity_ObjectToWorld, v.vertex);
            //o.objPos = mul(unity_WorldToObject, o.objPos);
            return o;
        }

        //float4 _XColor;
        //float4 _YColor;
        //float4 _ZColor;

        fixed4 frag( v2f i) : SV_Target {
                float4 p = .5 * (i.objPos + 1.0);
                p.w = 1.0;
                return p;
             }
            ENDCG
        }
    }


	Fallback "Unlit"
}


Shader "Neodroid/Spaces/ObjectSpace" {

	Properties {

        _Scalar("Scaling Factor", float) = 1
		//_XColor ("_XColor", Color) = (1,0,0,1.)
		//_YColor ("_YColor", Color) = (0,1,0,1.)
		//_ZColor ("_ZColor", Color) = (0,0,1,1.)
	}
/*
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
                //                float4 p = .5 * (i.objPos + 1.0);
                //p.w = 1.0;
                float4 p = float4(i.objPos.xyz, 1.0);

                return p;
             }
            ENDCG
        }
    }
*/


   SubShader {
   //Blend One Zero
      Pass {
         CGPROGRAM

         #pragma vertex vert
         #pragma fragment frag

         void vert(float4 vertexPos : POSITION,
                     out float4 pos : SV_POSITION,
                     out float4 col : TEXCOORD0){
            pos =  UnityObjectToClipPos(vertexPos);
            col = vertexPos + float4(0.5, 0.5, 0.5, 0.0);
            return;
         }

            //float _Scalar;

         float4 frag(float4 pos : SV_POSITION,
                     float4 col : TEXCOORD0) : COLOR {
            return col;
         }

         ENDCG
      }
   }




/*
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        struct Input {
            float3 objPos;
        };

        void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.objPos = v.vertex;
        }

        void surf (Input IN, inout SurfaceOutput o) {
            o.Albedo = IN.objPos;
            o.Alpha = 1;
        }
        ENDCG
    }
*/

      Fallback "Unlit/Color"
}

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Neodroid/FlatColor"
{
    Properties
    {
        //_MainTex("Main Texture", 2D) = "black" {}
        _Color ("Main Color", COLOR) = (1,1,1,1)
    }

    /*
    CGINCLUDE

        #include "UnityCG.cginc"

        struct appdata {
          float4 vertex : POSITION;
          float2 uv : TEXCOORD0;
        };

        struct v2f {
          float2 uv : TEXCOORD0;
          float4 vertex : SV_POSITION;
        };


        v2f vert(appdata v){
          v2f o;
          o.vertex = UnityObjectToClipPos(v.vertex);
          o.uv = v.uv;

          return o;
        }

        fixed4 _Color;

        fixed4 frag(v2f i) : SV_Target{
            //fixed4 col = fixed4(0,0,0,1);
            //return col;
            return _Color;
        }

    ENDCG
    */

    SubShader{
        Tags{
          "Queue" = "Transparent"
        }

        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag

            float4 vert (float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }

            fixed4 _Color;

            fixed4 frag() : SV_Target{
                return _Color;
            }

            ENDCG
        }
    }
}

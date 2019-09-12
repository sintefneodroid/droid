Shader "Neodroid/Mask" {
    SubShader{
        Tags{
          "Queue" = "Transparent"
        }

        Cull Off
        ZWrite Off
        ZTest Always
        Blend One Zero

        Pass
        {
            CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag

            float4 vert (float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }

            fixed4 frag() : SV_Target{
                return 1;
            }

            ENDCG
        }
    }
}

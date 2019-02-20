Shader "Neodroid/Experimental/StereoEyeIndexColor"
{
   Properties
   {
       _LeftEyeColor("Left Eye Color", COLOR) = (0,1,0,1)
       _RightEyeColor("Right Eye Color", COLOR) = (1,0,0,1)
       _UseRight("Use Right Output",float)=0
   }

   SubShader
   {
      Tags { "RenderType" = "Opaque" }

      Pass
      {
         CGPROGRAM

         #pragma vertex vert
         #pragma fragment frag

         float4 _LeftEyeColor;
         float4 _RightEyeColor;
         float _UseRight;

         #include "UnityCG.cginc"

         struct appdata
         {
            float4 vertex : POSITION;

            UNITY_VERTEX_INPUT_INSTANCE_ID
         };

         struct v2f
         {
            float4 vertex : SV_POSITION;

            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
         };

         v2f vert (appdata v)
         {
            v2f o;

            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_OUTPUT(v2f, o);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

            o.vertex = UnityObjectToClipPos(v.vertex);

            return o;
         }

         fixed4 frag (v2f i) : SV_Target
         {
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

            return lerp(_LeftEyeColor, _RightEyeColor, _UseRight);
         }
         ENDCG
      }
   }
}
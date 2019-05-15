Shader "Neodroid/Spaces/Experimental/PositionShader" {
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				struct vertInput {
					float4 pos : POSITION;
				};

				struct vertOutput {
					float4 pos : SV_POSITION;
					float3 worldPos : TEXCOORD0;
				};

				vertOutput vert(vertInput input) {
					vertOutput o;
					o.pos = UnityObjectToClipPos(input.pos);
					o.worldPos = mul(unity_ObjectToWorld, input.pos).xyz;
					return o;
				}

				float4 frag(vertOutput output) : COLOR{
				    return float4(output.worldPos.xyz, 1.0);
				}
			ENDCG
		}
	}
}
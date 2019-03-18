Shader "Kumo Kairo/Gradient Moon Skybox"
{
    Properties
    {
        _SkyTint("Sky Tint", Color) = (.5, .5, .5, 1)
        _GroundColor("Ground", Color) = (.369, .349, .341, 1)
        _Exponent("Exponent", Range(0, 15)) = 1.0
        _SunPosition("Sun Position", Vector) = (0.0, 0.0, 1.0)

        _SunColor("Sun Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _SunSize("Sun Size", Range(0, 1)) = 0.04
        _SunHardness("Sun Hardness", Float) = 0.1
        _SunGradient("Sun Gradient", Range(0, 1)) = 0.0
    }

    CGINCLUDE

    #include "Lighting.cginc"
    #include "UnityCG.cginc"

    uniform half3 _SkyTint, _GroundColor, _SunPosition, _SunColor;
    uniform half _SunSize, _Exposure, _Exponent, _SunHardness, _SkyGradient;

    #define HARDNESS_EXPONENT_BASE 0.125

    struct appdata
    {
        float4 position : POSITION;
        float3 texcoord : TEXCOORD0;
    };

    struct v2f
    {
        float4 position : SV_POSITION;
        float3 texcoord : TEXCOORD0;
    };

    half calcSunSpot(half3 sunDirPos, half3 skyDirPos)
    {
        half3 delta = sunDirPos - skyDirPos;
        half dist = length(delta);
        half spot = 1.0 - smoothstep(0.0, _SunSize, dist);
        return 1.0 - pow(HARDNESS_EXPONENT_BASE, spot * _SunHardness);
    }

    v2f vert(appdata v)
    {
        v2f o;
        o.position = UnityObjectToClipPos(v.position);
        o.texcoord = v.texcoord;
        return o;
    }

    half4 frag(v2f i) : COLOR
    {
        half p = i.texcoord.y;

        float p1 = pow(min(1.0f, 1.0f - p), _Exponent);
        float p2 = 1.0f - p1;

        half3 mie = calcSunSpot(_SunPosition.xyz, i.texcoord.xyz) * _SunColor;
        half3 col = _GroundColor * p1 + mie * p2;
        col += _SkyTint * p2;

        return half4(col, 1.0);
    }

    ENDCG

    SubShader
    {
        Tags{ "RenderType" = "Skybox" "Queue" = "Background" }
            Pass
        {
            ZWrite Off
            Cull Off
            Fog { Mode Off }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
}
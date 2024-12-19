Shader "Custom/SpotlightCutoutShader"
{
    Properties
    {
        _SpotlightTex ("Spotlight Texture", 2D) = "white" {}
        _CutoutColor ("Cutout Color", Color) = (0, 0, 0, 1)
        _SpotlightScale ("Spotlight Scale", Range(0.1, 2.0)) = 1.0
        _Offset ("Spotlight Offset", Vector) = (0.5, 0.5, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _SpotlightTex;
            fixed4 _CutoutColor;
            float _SpotlightScale;
            float4 _Offset; // Position offset for the spotlight

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o = (v2f)0;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Apply scale and position offset
                o.uv = (v.uv - _Offset.xy) / _SpotlightScale + 0.5;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_SpotlightTex, i.uv);
                fixed alpha = texColor.a;
                return fixed4(_CutoutColor.rgb, 1 - alpha);
            }
            ENDCG
        }
    }
}

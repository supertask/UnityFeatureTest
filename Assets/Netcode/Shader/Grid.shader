Shader "Custom/GridShader"
{
    Properties
    {
        _GridColor("Grid Color", Color) = (1,1,1,1)
        _GridSpacing("Grid Spacing", Float) = 1.0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _GridColor;
            float _GridSpacing;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv / _GridSpacing;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 grid = fmod(i.uv, 1.0);
                float line2 = step(0.99, grid.x) + step(0.99, grid.y);
                return _GridColor * line2;
            }
            ENDCG
        }
    }
}

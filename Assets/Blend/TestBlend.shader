Shader "ImageEffect/TestBlend"
{
    Properties
    {
        [HideInInspector]
        _MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 0, 0, 1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM

            #include "UnityCG.cginc"
            #pragma vertex vert_img
            #pragma fragment frag

            sampler2D _MainTex;

            fixed4 frag(v2f_img input) : SV_Target
            {
                return tex2D(_MainTex, input.uv);
            }

            ENDCG
        }
        Pass
        {
			Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            #include "UnityCG.cginc"
            #include "Packages/jp.supertask.shaderlibcore/Shader/Lib/Noise/Noise.hlsl"
            //#include "Packages/jp.supertask.shaderlibcore/Shader/Lib/Noise/Random.hlsl"
            #pragma vertex vert_img
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _Color;

            fixed4 frag(v2f_img input) : SV_Target
            {
                float noise = (snoise(input.uv * 3) + 1.0) / 2.0;
                return noise * _Color;
                //return 1;

                //return tex2D(_MainTex, input.uv);
            }

            ENDCG
        }
    }
}
Shader "CommandBuffer/Mosaic" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _PixelNumber ("PixelNum", float) = 100
    }

    SubShader {
        Pass {
            Tags { "RenderType"="Opaque" }

            Stencil
            {
                Ref 1
                Comp Equal
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _PixelNumber;

            struct v2f {
                half4 pos : POSITION;
                half2 uv : TEXCOORD0;
            };

            float4 _MainTex_ST;

            v2f vert(appdata_base v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            half4 frag(v2f i) : COLOR {
                half2 uv = floor(i.uv * _PixelNumber) / _PixelNumber;
                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}



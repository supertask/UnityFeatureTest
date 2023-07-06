Shader "Custom/GradientXMask" {
    Properties {
        _CenterPointX("CenterPointX", Range(0, 1)) = 0.5
        _ExtentX("ExtentX", Range(0, 1)) = 0.5
        _GradientX("GradientX", Range(0, 1)) = 0.25
        _MainColor("Main Color", Color) = (1,1,1,1)
        _SecondaryColor("Secondary Color", Color) = (0,0,0,1)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _CenterPointX;
            float _ExtentX;
            float _GradientX;
            fixed4 _MainColor;
            fixed4 _SecondaryColor;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float rightEdge = _CenterPointX + (_ExtentX - _GradientX);
                float leftEdge = _CenterPointX - (_ExtentX - _GradientX);

                if (i.uv.x >= leftEdge && i.uv.x <= rightEdge) {
                    return _MainColor;
                }

                float t;
                if(i.uv.x < leftEdge) {
                    t = (leftEdge - i.uv.x) / (leftEdge - (_CenterPointX - _ExtentX));
                }
                else {
                    t = (i.uv.x - rightEdge) / ((_CenterPointX + _ExtentX) - rightEdge);
                }

                t = clamp(t, 0, 1);
                return lerp(_MainColor, _SecondaryColor, t);
            }
            ENDCG
        }
    }
}

Shader "Unlit/Bloom"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("Thresold", Range(0, 1)) = 0.8
        _SourceTex ("Source Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _Threshold;
            sampler2D _MainTex;

            half3 PreFilter(half3 c)
            {
                half brightness = max(c.r, max(c.g, c.b));
                half contribution = max(0, brightness - _Threshold);
                contribution /= max(brightness, 0.00001);
                return c * contribution;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(PreFilter(tex2D(_MainTex, i.uv)), 1);
            }
            ENDCG
        }
        Pass // 1 ·¥ëÃñÕå–
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            half3 BoxFliter(float2 uv, float t)
            {
                half2 upL, upR, downL, downR;

                //offset
                upL = _MainTex_TexelSize.xy * half2(t, 0);
                upR = _MainTex_TexelSize.xy * half2(0, t);
                downL = _MainTex_TexelSize.xy * half2(-t, 0);
                downR = _MainTex_TexelSize.xy * half2(0, -t);

                half3 col = 0;

                //
                col += tex2D(_MainTex, uv + upL).rgb * 0.25;
                col += tex2D(_MainTex, uv + upR).rgb * 0.25;
                col += tex2D(_MainTex, uv + downL).rgb * 0.25;
                col += tex2D(_MainTex, uv + downR).rgb * 0.25;

                return col;
            }
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(BoxFliter(i.uv, 1).rgb, 1);
            }
            ENDCG
        }
        Pass //effect plus
        {
            blend one one
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            half3 BoxFliter(float2 uv, float t)
            {
                half2 upL, upR, downL, downR;
                // offest
                upL = _MainTex_TexelSize.xy * half2(t, 0);
                upR = _MainTex_TexelSize.xy * half2(0, t);
                downL = _MainTex_TexelSize.xy * half2(-t, 0);
                downR = _MainTex_TexelSize.xy * half2(0, -t);

                half3 col = 0;
                // 
                col += tex2D(_MainTex, uv + upL).rgb * 0.25;
                col += tex2D(_MainTex, uv + upR).rgb * 0.25;
                col += tex2D(_MainTex, uv + downL).rgb * 0.25;
                col += tex2D(_MainTex, uv + downR).rgb * 0.25;

                return col;
            }
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(BoxFliter(i.uv, 1).rgb, 1);
            }
            ENDCG
        }

        Pass // çáïŸ
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _SourceTex;
            sampler2D _MainTex;
            
             v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            fixed4 frag (v2f i) : SV_TARGET
            {
                fixed3 source = tex2D(_SourceTex, i.uv).rgb;
                fixed3 blur = tex2D(_MainTex, i.uv).rgb;
                return fixed4(source + blur, 1);
            }

            ENDCG
        }
    }
}
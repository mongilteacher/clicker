Shader "Custom/FeverFlame"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _FlameWidth  ("Flame Width",   Range(0.01, 0.3)) = 0.08
        _Intensity   ("Intensity",     Range(0.5, 5))     = 2
        _Speed       ("Flame Speed",   Range(0.5, 8))     = 3
        _NoiseScale  ("Noise Scale",   Range(2, 30))      = 10
        _RainbowSpeed("Rainbow Speed", Range(0.2, 5))     = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"           = "Transparent-1"
            "RenderType"      = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType"     = "Plane"
        }

        Blend SrcAlpha One
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                float2 uv    : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float _FlameWidth;
            float _Intensity;
            float _Speed;
            float _NoiseScale;
            float _RainbowSpeed;

            // -------- vertex: 메쉬를 확장하고 UV를 리매핑 --------
            v2f vert(appdata v)
            {
                v2f o;

                // FlameWidth 비례로 메쉬 확장
                float expand = _FlameWidth * 2.5;
                float scale  = 1.0 + expand;

                v.vertex.xy *= scale;

                // UV도 같은 비율로 확장 → 바깥쪽 UV가 0~1 범위를 넘게 됨
                o.uv = 0.5 + (v.uv - 0.5) * scale;

                o.pos   = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
                return o;
            }

            // -------- noise --------
            float2 hash2(float2 p)
            {
                p = float2(dot(p, float2(127.1, 311.7)),
                           dot(p, float2(269.5, 183.3)));
                return -1.0 + 2.0 * frac(sin(p) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float2 u = f * f * (3.0 - 2.0 * f);

                float a = dot(hash2(i + float2(0, 0)), f - float2(0, 0));
                float b = dot(hash2(i + float2(1, 0)), f - float2(1, 0));
                float c = dot(hash2(i + float2(0, 1)), f - float2(0, 1));
                float d = dot(hash2(i + float2(1, 1)), f - float2(1, 1));

                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y) * 0.5 + 0.5;
            }

            float fbm(float2 p)
            {
                float v = 0.0;
                float a = 0.5;
                for (int j = 0; j < 3; j++)
                {
                    v += a * noise(p);
                    p *= 2.0;
                    a *= 0.5;
                }
                return v;
            }

            // -------- color --------
            float3 hsv2rgb(float3 c)
            {
                float3 p = abs(frac(c.xxx + float3(0.0, 2.0/3.0, 1.0/3.0)) * 6.0 - 3.0);
                return c.z * lerp(float3(1,1,1), saturate(p - 1.0), c.y);
            }

            // -------- helpers --------
            // UV가 0~1 안에 있으면 1, 밖이면 0
            float insideBounds(float2 uv)
            {
                float2 s = step(float2(0, 0), uv) * step(uv, float2(1, 1));
                return s.x * s.y;
            }

            // 텍스처를 안전하게 샘플링 (범위 밖이면 alpha = 0)
            float sampleAlpha(float2 uv)
            {
                return tex2D(_MainTex, saturate(uv)).a * insideBounds(uv);
            }

            // -------- fragment --------
            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // 현재 픽셀의 알파 (확장 영역은 0)
                float srcAlpha = sampleAlpha(uv);

                // 12방향 샘플링으로 근처에 스프라이트가 있는지 탐색
                float expanded = 0;
                const int SAMPLES = 12;
                const float TWO_PI = 6.28318;

                for (int s = 0; s < SAMPLES; s++)
                {
                    float ang = s * TWO_PI / SAMPLES;
                    float2 dir = float2(cos(ang), sin(ang));

                    float n = fbm(uv * _NoiseScale
                                  + float2(0, _Time.y * _Speed)
                                  + dir * 3.0);
                    float w = _FlameWidth * (0.4 + 0.6 * n);

                    expanded = max(expanded, sampleAlpha(uv + dir * w));
                }

                // 외곽선: 자기는 투명(srcAlpha≈0)이지만 근처에 알파가 있는 곳
                float flame = expanded * saturate(1.0 - srcAlpha);

                // 일렁임
                float flicker = fbm(uv * _NoiseScale * 0.7
                                    + _Time.y * _Speed * float2(0.3, 1.0));
                flame *= (0.5 + 0.5 * flicker);

                // 가장자리로 갈수록 페이드아웃
                float distFromCenter = length(uv - 0.5) * 2.0;
                flame *= saturate(1.2 - distFromCenter);

                // 무지개 색
                float hue = frac(_Time.y * _RainbowSpeed
                                 + uv.x * 0.5
                                 + uv.y * 0.3);
                float3 col = hsv2rgb(float3(hue, 0.85, 1.0));

                float alpha = saturate(flame * _Intensity);
                return fixed4(col * _Intensity, alpha) * i.color;
            }
            ENDCG
        }
    }
}

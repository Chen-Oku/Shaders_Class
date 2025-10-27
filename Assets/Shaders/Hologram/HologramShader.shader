Shader "MyShader/Unlit/HologramShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1,1,1,1)

        _Transparency ("Transparency", Range(0.2,0.5)) = 0.25
        _CutOut ("Cut Out", Range(0.0,1.0)) = 0.2

        _Distance("Distance", Float) = 1.0
        _Amplitude("Amplitude", Float) = 1.0
        _Speed("Speed", Float) = 1.0
        _MinBaseAmount("Min Base Amount", Range(0.0, 1.0)) = 0.0
        _MaxBaseAmount("Max Base Amount", Range(0.0, 1.0)) = 1.0
        _Interval("Interval", Float) = 1.0
        _SpikeDuration("Spike Duration", Float) = 0.5
        _MinSpikeChance("Min Spike Chance", Float) = 0.05
        _MaxSpikeChance("Max Spike Chance", Float) = 0.3

    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TintColor;
            float _Transparency;
            float _CutOut;
            float _Distance;
            float _Amplitude;
            float _Speed;
            float _MinBaseAmount;
            float _MaxBaseAmount;
            float _Interval;
            float _SpikeDuration;
            float _MinSpikeChance;
            float _MaxSpikeChance;

            float rand(float seed)
            {
                return frac(sin(seed) * 43758.5453);
            }

            float GetRandomSpikeChance(float seed)
            {
                return lerp(_MinSpikeChance, _MaxSpikeChance, rand(seed));
            }

            float GetRandomBaseAmount(float seed)
            {
                return lerp(_MinBaseAmount, _MaxBaseAmount, rand(seed));
            }

            float GetRandomAmount(float time)
            {
                float randomTime = floor(time / _Interval)*_Interval;
                float spikeChance = GetRandomSpikeChance(randomTime);
                float spikeTrigger = rand(randomTime + 1.0);

                if (spikeTrigger < spikeChance)
                {
                    float timeInterval = frac(time / _Interval)*_Interval;
                    if (timeInterval < _SpikeDuration)
                    {
                        return GetRandomBaseAmount(randomTime + 2.0);
                    }
                }
                return 0.0;
            }

            v2f vert (appdata v)
            {
                v2f o;

                float randomAmount = GetRandomAmount(_Time.y);

                v.vertex.x += (sin(_Time.y * _Speed + v.vertex.y * _Amplitude) * _Distance) * randomAmount;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) + _TintColor;
                clip( col.a - _CutOut );
                col.a = _Transparency;

                return col;
            }
            ENDCG
        }
    }
}

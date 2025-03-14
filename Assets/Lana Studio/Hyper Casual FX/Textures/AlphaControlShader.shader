Shader "Custom/AlphaControlShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaMultiplier ("Alpha Multiplier", Range(0, 1)) = 1.0
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass {
            // Habilita a mesclagem de transparência
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _AlphaMultiplier;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Pega a cor da textura
                fixed4 col = tex2D(_MainTex, i.uv);
                // Multiplica o alpha pelo valor definido
                col.a *= _AlphaMultiplier;
                return col;
            }
            ENDCG
        }
    }
}

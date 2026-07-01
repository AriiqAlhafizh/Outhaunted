Shader "Custom/SpriteWhiteFade"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Speed ("Fade Speed", Float) = 3.0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha // Menggunakan Premultiplied Alpha khas Sprite Unity

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _Speed;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // Ambil pixel dari tekstur asli sprite
                fixed4 c = tex2D(_MainTex, IN.texcoord);
                
                // Jika pixel asli transparan (alpha = 0), biarkan tetap transparan
                // Ini menjaga bentuk/outline asli dari sprite Anda
                float originalAlpha = c.a * IN.color.a;

                // Ubah warnanya menjadi PUTIH TOTAL (RGB = 1, 1, 1)
                c.rgb = float3(1.0, 1.0, 1.0);

                // LOGIKA LOOP TRANSPARANSI:
                // sin(_Time.y) menghasilkan nilai berosilasi antara -1 dan 1.
                // Kita manipulasi rumusnya agar menghasilkan nilai antara 0.5 dan 1.0.
                // Nilai -1 menjadi: (-1 * 0.25) + 0.75 = 0.5
                // Nilai  1 menjadi: (1 * 0.25) + 0.75 = 1.0
                float alphaLoop = sin(_Time.y * _Speed) * 0.25 + 0.25;

                // Gabungkan alpha hasil loop dengan alpha asli sprite
                c.a = originalAlpha * alphaLoop;

                // Kalikan RGB dengan Alpha (wajib untuk blend mode 'One OneMinusSrcAlpha')
                c.rgb *= c.a;

                return c;
            }
        ENDCG
        }
    }
}
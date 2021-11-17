Shader "Unlit/ParticleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // ç\ë¢ëÃíËã`
            struct Particle
            {
                float3 pos;
                float3 vel;
                float3 col;
            };

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 color : COLOR;
            };

            uniform StructuredBuffer<Particle> Particles;

            v2f vert(uint id : SV_VertexID)
            {
                Particle p = Particles[id];

                v2f o;
                o.vertex = UnityObjectToClipPos(p.pos);
                o.color = p.col;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return fixed4(i.color.rgb, 1);
            }
            ENDCG
        }
    }
}

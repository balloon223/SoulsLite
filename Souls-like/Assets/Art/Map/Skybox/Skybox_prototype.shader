Shader "Unlit/Skybox_prototype"
{
    Properties
    {
        _texCube ("cube map", Cube) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Background" "Queue" = "Background" "PreviewType" = "Skybox"}
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            samplerCUBE _texCube;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 objPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.objPos = v.vertex.xyz;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 col = 0;
                float3 sampleVec = normalize(i.objPos);
                col = texCUBElod(_texCube, float4(sampleVec, 0));
                return float4(col,1);
            }
            ENDCG
        }
    }
}

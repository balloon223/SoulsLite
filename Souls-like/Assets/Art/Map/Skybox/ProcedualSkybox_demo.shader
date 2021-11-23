Shader "Unlit/ProcedualSkybox_demo"
{
    Properties
    {
        _MainTex("Texture", Cube) = "white" {}
        _SkycolorHigh("Sky Color High", Color) = (0,0,0,0)
        _SkycolorLow("Sky Color Low", Color) = (0,0,0,0)
        _SkyGradient("Sky Gradient", Range(0,1)) = 0
        _HorizonContrast("Horizon", Range(0,100)) = 0
        _tmp("Dev Slider", Range(0,1)) = 0
    }
        SubShader
        {
            Tags { "RenderType" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox"}
            Cull off
            ZWrite off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"
                #include "Lighting.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 uv : TEXCOORD0;

                    float3 normal : NORMAL;
                };

                struct v2f
                {
                    float3 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;

                    float3 normal : TEXCOORD1;
                    float3 tangent : TEXCOORD2;
                    float3 bitangent : TEXCOORD3;
                };

                samplerCUBE _MainTex;
                //float4 _MainTex_ST;
                float4 _SkycolorHigh;
                float4 _SkycolorLow;
                float _SkyGradient;
                float _HorizonContrast;
                float _tmp;

                float circle(float3 localUV)
                {
                    return smoothstep(0, 0.01, 1 - length(localUV));
                }


                float rand(float3 uv) {
                    return frac(sin(dot(uv.xyz, float3(12.9898, 78.233, 53.2167))) * 4758.5453123);
                }

                /*float2 unity_gradientNoise_dir(float2 p)
                {
                    p = p % 289;
                    float x = (34 * p.x + 1) * p.x % 289 + p.y;
                    x = (34 * x + 1) * x % 289;
                    x = frac(x / 41) * 2 - 1;
                    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
                }*/

                float value_noise(float3 uv) {
                    float3 ip = floor(uv);
                    float3 fp = frac(uv);

                    float o = rand(ip);
                    float x = rand(ip + float3(1, 0, 0));
                    float y = rand(ip + float3(0, 1, 0));
                    float z = rand(ip + float3(0, 0, 1));
                    float xy = rand(ip + float3(1, 1, 0));
                    float xz = rand(ip + float3(1, 0, 1));
                    float yz = rand(ip + float3(0, 1, 1));
                    float xyz = rand(ip + float3(1, 1, 1));


                    float3 smooth = smoothstep(0, 1, fp);



                    return lerp(
                        lerp(lerp(o, x, smooth.x),
                            lerp(y, xy, smooth.x), smooth.y) * (sin(_Time.y * 2) * .3 + 0.5),
                        lerp(lerp(z, xz, smooth.x),
                            lerp(yz, xyz, smooth.x), smooth.y) * (sin(_Time.y * 3) * .2 + 0.5),
                        smooth.z
                    );
                }





                float3 drawStar(float3 uv, float3 color)
                {
                    float3 starGridUV = uv;
                    starGridUV *= _tmp * 100;
                    float3 star = 1 * value_noise(uv) * rand(starGridUV);
                    //star = clamp(star, _tmp, 1);
                    star = 1 - saturate(star);
                    star = pow(star, 10000);
                    float cir = circle(frac(starGridUV) * 2 - 1);
                    cir = 0;
                    //float polar = atan2(starGridUV)
                    color += star;
                    color = saturate(color);
                    //color = value_noise(starGridUV);
                    //return cir;
                    return color + cir;
                }

                float3 drawMoon(float3 uv)
                {
                    //float moon = distance(uv, )
                    return float3(0, 0, 0);
                }

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.normal = UnityObjectToWorldNormal(v.normal);
                    return o;
                }

                float4 frag(v2f i) : SV_Target
                {
                    float3 color = 0;
                    float3 uv = normalize(i.uv) * 0.5 + 0.5;
		    uv.z += normalize(_Time.x);




                    color = uv;
                    color = lerp(_SkycolorLow, _SkycolorHigh, pow(smoothstep(0,1,uv.y + _SkyGradient), _HorizonContrast));
                    // sample the texture
                    float4 col = texCUBElod(_MainTex, float4(i.uv, 0));
                    //return col;

                    color = saturate(color);

                    float3 lightDirection = _WorldSpaceLightPos0;
                    float3 lightColor = _LightColor0; // includes intensity

                    float3 tangentSpaceNormal = i.normal;
                    tangentSpaceNormal = normalize(lerp(float3(0, 0, 1), tangentSpaceNormal, 1));
                    float3x3 tangentToWorld = float3x3
                        (
                            i.tangent.x, i.bitangent.x, i.normal.x,
                            i.tangent.y, i.bitangent.y, i.normal.y,
                            i.tangent.z, i.bitangent.z, i.normal.z
                            );

                    float3 normal = mul(tangentToWorld, tangentSpaceNormal);
                    //return float4(normal, 1);
                    float directDiffuse = max(0, dot(-normal, lightDirection));
                    color = color * (directDiffuse * lightColor);
                    //color = drawStar(uv, color);

                    //color += drawMoon(uv);
                    //return circle(uv);
                    //return float4(uv.x, uv.y, uv.z, 1.0);
                    return float4(col * color,1.0);
                }
                ENDCG
            }
        }
}

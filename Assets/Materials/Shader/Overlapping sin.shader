Shader "Custom/RandomPattern"
{
    Properties
    {
        _PrimairyColor ("Primairy Color", Color) = (1,1,1,1)
        _SecondairyColor ("Secondairy Color", Color) = (1,1,1,1)
        _AnimateXY ("Animate XY", Vector) = (0,0,0,0)
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            // converted to HLSLPROGRAM as CGPROGRAM doesnt seem to work anymore with newer versions
            HLSLPROGRAM
            // CGPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            // changed to the newer version of the include?
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            float4 _AnimateXY;
            float4 _PrimairyColor;
            float4 _SecondairyColor;

            v2f Vert(MeshData IN)
            {
                v2f OUT;

                float4 worldPos = mul(UNITY_MATRIX_M, float4(IN.vertex.xyz, 1.0));
                float4 viewPos  = mul(UNITY_MATRIX_V, worldPos);
                float4 clipPos  = mul(UNITY_MATRIX_P, viewPos);

                // this is the same as what im doing above just in a single step
                // float4 clipPos = 
                //     mul(UNITY_MATRIX_P,
                //         mul(UNITY_MATRIX_V,
                //             mul(UNITY_MATRIX_M, float4(IN.vertex.xyz,1))
                //         )
                //     );

                OUT.vertex = clipPos;

                OUT.uv = IN.uv;
                OUT.uv += _AnimateXY.xy * _Time.yy;

                return OUT;
            }

            float4 Frag(v2f IN) : SV_Target
            {
                float u = IN.uv.x;
                float v = IN.uv.y;

                float cos1 = (cos(u * PI * 2.0) + 1.0) * 0.5;
                float cos2 = (sin(u * PI * 2.0 - PI / 1.5) + 1.0) * 0.5;
                float cos3 = (tan(u * PI * 2.0 + PI / 1.5) + 1.0) * 0.5;
            
                float4 col1 = float4(1,0,0,1);
                float4 col2 = float4(0,0,1,1);
                float4 col3 = float4(0,1,0,1);
            
                float4 color = 0;
            
                color += col1 * step(v, cos1);
                color += col2 * step(v, cos2);
                color += col3 * step(v, cos3);
            
                color.a = 1;
            
                return color;
            }

            ENDHLSL
            // ENDCG
        }
    }
}
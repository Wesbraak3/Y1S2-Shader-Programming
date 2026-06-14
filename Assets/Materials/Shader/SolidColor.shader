Shader "Custom/SolidColorURP"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
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


            float4 _Color;

            v2f Vert(MeshData IN)
            {
                v2f OUT;
                // using unity function to convert the localspace to worldspace and puts it in the right position in front of the camera
                OUT.vertex = TransformObjectToHClip(IN.vertex); // Model View Projection
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 Frag(v2f IN) : SV_Target
            {
                half4 color = half4(IN.uv, 0, 1);
                return color;
            }

            ENDHLSL
            // ENDCG
        }
    }
}
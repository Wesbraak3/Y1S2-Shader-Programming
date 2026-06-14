Shader "Custom/SolidColorURP"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
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
            
            float4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _AnimateXY;

            v2f Vert(MeshData IN)
            {
                v2f OUT;
                // using unity function to convert the localspace to worldspace and puts it in the right position in front of the camera
                OUT.vertex = TransformObjectToHClip(IN.vertex); // Model View Projection
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.uv += frac(_AnimateXY.xy * _MainTex_ST * _Time.yy);

                return OUT;
            }

            half4 Frag(v2f IN) : SV_Target
            {
                half4 textureColor = tex2D(_MainTex, IN.uv);
                return textureColor;
            }

            ENDHLSL
            // ENDCG
        }
    }
}
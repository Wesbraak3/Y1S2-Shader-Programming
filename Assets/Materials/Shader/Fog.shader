Shader "Custom/Fog"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MaxDistance("Max Distance", float) = 100
        _StepSize("Step Size", Range(0.1, 20)) = 1
        _DensityMultiplier("Density Multiplier", Range(0, 1)) = 0.5
        _NoiseOffset("Noise Offset", float) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            
            float4 _Color;
            float _MaxDistance;
            float _StepSize;
            float _NoiseOffset;
            float _DensityMultiplier;

            float GetDensity(){
                return _DensityMultiplier;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, IN.texcoord);
                float depth = SampleSceneDepth(IN.texcoord);
                // unity method 
                // float3 worldPos = ComputeWorldSpacePosition(IN.texcoord, depth, UNITY_MATRIX_I_VP);
                
                // TODO: Calc worldspace tyourtself. UV ? NDC ? Clip ? World
                float2 uv = IN.texcoord; // Screen Space
                float2 ndc = uv * 2.0 - 1.0; // Normalized Device Coordinates times 2 for 0?1 > 0?2 then -1 to shift it to -1?1

                float4 clipPos = float4(ndc, depth, 1.0); // Clip Space
                float4 world = mul(UNITY_MATRIX_I_VP, clipPos); // WorldSpace
                float3 worldPos = world.xyz / world.w;

                // Set raycast for fog calculation
                float3 viewDir = worldPos - _WorldSpaceCameraPos;
                float viewLenght = length(viewDir);

                float distLimit = min(viewLenght, _MaxDistance);
                
                // float distTraveled = 0;
                // added noise to the shader to get rid of the banding
                // the noise i use is IGN as unity has a default for this that can be used
                float2 pixelCoords = IN.texcoord * _BlitTexture_TexelSize.zw;
                float distTraveled = InterleavedGradientNoise(pixelCoords, _Time.y) * _StepSize;

                float transmittance = 1;

                while(distTraveled < distLimit){
                    float density = GetDensity();
                    
                    if (density > 0) {
                        // transmittance += density * _StepSize;
                        // Using beers law to make transmittance more gradual
                        transmittance *= exp(-density * _StepSize);
                    }
                    distTraveled += _StepSize;
                }

                return lerp(col, _Color, 1.0 - saturate(transmittance));
            }

            ENDHLSL
        }
    }
}
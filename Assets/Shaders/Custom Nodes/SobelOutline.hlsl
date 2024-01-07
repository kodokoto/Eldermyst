#if !defined(SHADERGRAPH_PREVIEW)

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"

#else

float SampleSceneDepth(float2 uv)
{
    return 0.0;
}

float3 SampleSceneNormals(float2 uv)
{
    return 0.0;
}

#endif


float GetDepth(float2 uv)
{
    #ifndef SHADERGRAPH_PREVIEW
    return LinearEyeDepth(SampleSceneDepth(uv), _ZBufferParams);
    #else
    return 0.0;
    #endif
}

float3 GetNormal(float2 uv)
{
    #ifndef SHADERGRAPH_PREVIEW
    return SampleSceneNormals(uv);
    #else
    return 0.0;
    #endif
}

void Outline_float(
    float2 UV, 
    float depthThreshold, 
    float normalThreshold, 
    float pos, 
    out float4 color
)
{
    // basic settings
    float3 normalEdgeBias = float3(1.0, 1.0, 1.0);

    // calculate the texel size using the screen position
    float2 texelSize = 1.0 / _ScreenParams.xy;

    float depth = GetDepth(UV);
    float3 normal = GetNormal(UV);

    float2 cross_uvs[4];
    cross_uvs[0] = UV + float2(0.0, texelSize.y);
    cross_uvs[1] = UV - float2(0.0, texelSize.y);
    cross_uvs[2] = UV + float2(texelSize.x, 0.0);
    cross_uvs[3] = UV - float2(texelSize.x, 0.0);
    
    // sample depths
    float depths[4];


    // sample the depth texture at the four cross positions
    for (int i = 0; i < 4; ++i)
    {
        depths[i] = GetDepth(cross_uvs[i]);
    }

    // roberts cross on depth
    float dtrbl = depths[0] - depths[1];
    float dtlbr = depths[2] - depths[3];

    float d = sqrt(dot(dtrbl, dtrbl) + dot(dtlbr, dtlbr));

    float depthEdge = step(depthThreshold, d);
    

    // sample normals

    float3 normals[4];

    float normalDifference = 0.0;
    for (int j = 0; j < 4; ++j)
    {
        normals[j] = GetNormal(cross_uvs[j]);
        float3 normalDifferenceVector = normal - normals[j];
        float normalBiasDiff = dot(normalEdgeBias, normalDifferenceVector);
        float normalIndicator = smoothstep(-.01, .01, normalBiasDiff);
        normalDifference += dot(normalDifferenceVector, normalDifferenceVector) * normalIndicator;
    }

    float indicator = sqrt(normalDifference);
    float normalEdge = step(normalThreshold, indicator);


    // generate colour using _MainTex and the edge value
    // if the edge is a depth edge, then the colour should be darker, otherwise it should be lighter
    // if the edge is both a depth and normal edge, then the colour should be darker still

    float4 mainTex = SAMPLE_TEXTURE2D(_BlitTexture,sampler_BlitTexture, UV);
    
    if (depthEdge > 0.0)
    {
        float g = 0.0;
        // if the maintex colour is dark, then darken it further, otherwise lighten it
        float l = dot(mainTex.rgb, float3(0.299, 0.587, 0.114));

        if (l < 0.01)
        {
            g = 1.4;
        }
        else
        {
            g = 0.9;
        }      
        g = 1.4;
        float3 gamma = float3(g, g, g);
        color = float4(pow(mainTex, gamma).rgb, 1.0);
  
        // make the outline colour red
        //color = float4(1.0, 0.0, 0.0, 1.0);
    } else if (normalEdge > 0.0) {
        float3 gamma = float3(0.8, 0.8, 0.8);
        color = float4(pow(mainTex, gamma).rgb, 1.0);
        //color = float4(0.0, 0.0, 1.0, 1.0);
    } else {
        color = mainTex;
        //color = float4(0.0, 0.0, 0.0, 1.0);
    }

    // normalize depth to 0-1;
    //color = float4(normal.xyz, 1.0);
}

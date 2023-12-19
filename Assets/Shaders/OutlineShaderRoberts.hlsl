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
    return SampleSceneDepth(uv);
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

float invLerp(float from, float to, float value){
  return value - from;
}

float remap(float origFrom, float origTo, float targetFrom, float targetTo, float value){
  float rel = invLerp(origFrom, origTo, value);
  return lerp(targetFrom, targetTo, rel);
}

void Outline_float(
    float2 UV, 
    float offsetMultiplier, 
    float rcMultiplier, 
    float depthThreshold, 
    float normalThreshold, 
    float pos, 
    float steepAngleThreshold, 
    float steepAngleMultiplier, 
    out float Outline,
    out float4 color
)
{
    // basic settings
    float3 normalEdgeBias = float3(1.0, 1.0, 1.0);
    float depthEdgeStrength = 6.6;
    float normalEdgeStrength = 6.4;

    // calculate the texel size using the screen position
    float2 texelSize = 1.0 / _ScreenParams.xy;

    float depth = GetDepth(UV);
    float3 normal = GetNormal(UV);



    float2 cross_uvs[4];
    cross_uvs[0] = UV + float2(0.0, texelSize.y);
    cross_uvs[1] = UV - float2(0.0, texelSize.y);
    cross_uvs[2] = UV + float2(texelSize.x, 0.0);
    cross_uvs[3] = UV - float2(texelSize.x, 0.0);

    //float2 normal_uvs[4];
    //normal_uvs[0] = UV + float2(texelSize.x, texelSize.y) * offsetMultiplier;
    //normal_uvs[1] = UV - float2(texelSize.x, texelSize.y) * offsetMultiplier;
    //normal_uvs[2] = UV + float2(-texelSize.x, texelSize.y) * offsetMultiplier;
    //normal_uvs[3] = UV - float2(texelSize.x, -texelSize.y ) * offsetMultiplier;

    
    // sample depths
    float depths[4];

    float depthDifference = 0.0;

    // sample the depth texture at the four cross positions
    for (int i = 0; i < 4; ++i)
    {
        depths[i] = GetDepth(cross_uvs[i]);
        depthDifference += depth - depths[i];
    }

    // roberts cross on depth
    float dtrbl = depths[0] - depths[1];
    float dtlbr = depths[2] - depths[3];

    float d = sqrt(dot(dtrbl, dtrbl) + dot(dtlbr, dtlbr));
    d *= rcMultiplier;

    d = 1-d;

    float o = depth * depthThreshold;
    

    // sample normals

    float3 normals[4];
    for (int j = 0; j < 4; ++j)
    {
        normals[j] = GetNormal(cross_uvs[j]);
    }

    // roberts cross on normals

    float ntrbl = normals[0] - normals[1];
    float ntlbr = normals[2] - normals[3];
    float n = sqrt(dot(ntrbl, ntrbl) + dot(ntlbr, ntlbr));

    float normalEdge = step(normalThreshold, n);

    // remap normal edge to 0-1
    // float nv = dot(remap(0.0, 1.0, -1.0, 1.0, normalEdge), remap(0.0, 1.0, 1.0, -1.0, pos));
    // float nvtransform = (smoothstep(steepAngleThreshold, 2.0, nv) * steepAngleMultiplier) + 1.0;

    // float depthEdge2 = 1.0 - step(nvtransform * o, depthDifference);  
    float depthEdge = step(depthThreshold, depthDifference);

    Outline = max(depthEdge * depthEdgeStrength, normalEdge * normalEdgeStrength);
    Outline = depthDifference < 0 ? 0 : (depthEdge > 0.0 ? (depthEdgeStrength * depthEdge) : (normalEdge * normalEdgeStrength));

    // generate colour using _MainTex and the edge value
    // if the edge is a depth edge, then the colour should be darker, otherwise it should be lighter
    // if the edge is both a depth and normal edge, then the colour should be darker still

    float4 mainTex = SAMPLE_TEXTURE2D(_BlitTexture,sampler_BlitTexture, UV);
    
    if (depthEdge > 0.0)
    {
        float3 gamma = float3(1.4, 1.4, 1.4);
        color = float4(pow(mainTex, gamma).rgb, 1.0);
    } else if (normalEdge > 0.0) {
        float3 gamma = float3(0.8, 0.8, 0.8);
        color = float4(pow(mainTex, gamma).rgb, 1.0);
    } else {
        color = mainTex;
    }
}

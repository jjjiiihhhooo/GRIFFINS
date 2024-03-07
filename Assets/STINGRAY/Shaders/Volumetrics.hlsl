#ifndef Volumetrics_Shader
#define Volumetrics_Shader
//#include "Assets/Ultimate_Shader_Pack/Includes/Noise.hlsl"

CBUFFER_START(UnityPerMaterial)
Texture2D _MainTex;
sampler sampler_MainTex;
float4 Albedo;
float density;
float scale;
float speed;
float lightScattering;
float minDarkness;
float steps;
float stepSize;
float jitter;
float twirl;
float lightSteps;
float3 pos;
float3 bounds;
float thickness;
float densityOffset;
bool shadows;
float godRayStrength;
Texture3D<float4> _NoiseTexture;
sampler sampler_NoiseTexture;
CBUFFER_END

float random3D(float3 uv)
{
    float Coord = (uv.x + uv.y + uv.z);
    float2 _uv = float2(Coord, Coord);
    float2 noise = (frac(sin(dot(_uv, float2(12.9898, 78.233) * 2.0)) * 43758.5453));
    return (abs(noise.x + noise.y) - 1);
}

float2 PerlinDirection(float2 p)
{
    p = p % 289;
    float x = float(34 * p.x + 1) * p.x % 289 + p.y;
    x = (34 * x + 1) * x % 289;
    x = frac(x / 41) * 2 - 1;
    return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
}

float Perlin(float2 UV)
{
    float2 p = UV * 0.1;
    float2 ip = floor(p);
    float2 fp = frac(p);
    float d00 = dot(PerlinDirection(ip), fp);
    float d01 = dot(PerlinDirection(ip + float2(0, 1)), fp - float2(0, 1));
    float d10 = dot(PerlinDirection(ip + float2(1, 0)), fp - float2(1, 0));
    float d11 = dot(PerlinDirection(ip + float2(1, 1)), fp - float2(1, 1));
    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    float Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
    return Out;
}

float2 rayBoxDst(float3 boundsMin, float3 boundsMax, float3 rayOrigin, float3 invRaydir)
{
    float3 t0 = (boundsMin - rayOrigin) * invRaydir;
    float3 t1 = (boundsMax - rayOrigin) * invRaydir;
    float3 tmin = min(t0, t1);
    float3 tmax = max(t0, t1);

    float dstA = max(max(tmin.x, tmin.y), tmin.z);
    float dstB = min(tmax.x, min(tmax.y, tmax.z));

    float dstToBox = max(0, dstA);
    float dstInsideBox = max(0, dstB - dstToBox);
    return float2(dstToBox, dstInsideBox);
}

float3 GetRay(float2 screenPos)
{
    float3 viewVector = mul(unity_CameraInvProjection, float4(screenPos * 2 - 1, 0, -1));
    float3 viewDir = mul(unity_CameraToWorld, float4(viewVector, 0));
    float viewLength = length(viewDir);
    float3 ray = viewDir / viewLength;

    return ray;
}

float SceneDepth(float2 UV)
{
    // return Linear01Depth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV), _ZBufferParams);
    return LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV), _ZBufferParams);
    //return SHADERGRAPH_SAMPLE_SCENE_DEPTH(UV), _ZBufferParams;
}

float3 saturation(float3 In, float Saturation)
{
    float luma = dot(In, float3(0.2126729, 0.7151522, 0.0721750));
    return luma.xxx + Saturation.xxx * (In - luma.xxx);
}

float GetDensity(float3 position)
{
    float2 offset = _Time * speed;

    float centre = pos.y - bounds.y;
    float height = position.y;
    float dist = (centre - height) / bounds.y / 2;
    dist *= 0.9;
    dist += 0.1;

    position *= scale * 0.0001;

    float perlin = (Perlin(position.xz * 10) - 0.5) * twirl;
    offset += float3(perlin, 0, perlin);

    float4 noiseTex = SAMPLE_TEXTURE3D(_NoiseTexture, sampler_NoiseTexture, (position / 4) + float3(offset.x, 0, offset.y));
    offset *= 0.2;
    noiseTex.yz = SAMPLE_TEXTURE3D(_NoiseTexture, sampler_NoiseTexture, (position / 4) + float3(offset.x, 0, offset.y)).yz;
    //offset *= 0.2;
    //noiseTex.z = SAMPLE_TEXTURE3D(_NoiseTexture, sampler_NoiseTexture, (position / 4) + float3(offset.x, 0, offset.y)).z;

    //float4 noiseTex = SAMPLE_TEXTURE3D(_NoiseTexture, sampler_NoiseTexture, (position / 4) + float3(offset.x, 0, offset.y));

    float3 multiplier = float3(1, 0.3, 0.5 * dist);
    noiseTex.xyz *= multiplier;
    float noise = (noiseTex.x + noiseTex.y + noiseTex.z) / (multiplier.x + multiplier.y + multiplier.z);

    return clamp(noise - densityOffset, 0, 1);
}

float Phase(float3 rayDir, float3 lightDir)
{
    float fade = dot(lightDir, float3(0, -1, 0));
    fade = max(fade, 0);
    fade = pow(1 - fade, 25);

    float phase = dot(lightDir, rayDir);
    phase += 1;
    phase /= 2;

    phase = pow(phase, 1) * 50;

    return (phase * fade) + 1;
}

float lightingAtPoint(float3 rayPos, float3 lightDir)
{
    float3 posBL = pos - bounds / 2;
    float3 posTR = pos + bounds / 2;

    float3 ray = normalize(lightDir);
    float2 boxDist = rayBoxDst(posBL, posTR, rayPos, 1 / ray);

    float distInBox = boxDist.y;

    float size = distInBox / lightSteps;

    float cumulativeDensity = 0;

    Light mainLight = GetMainLight(float4(0, 0, 0, 0), float3(0, 0, 0), 1);
    float3 dir = mainLight.direction;
    float timeDot = dot(dir, float3(0, 1, 0));
    timeDot = abs(timeDot);
    timeDot *= 0.125;
    timeDot += 0.875;
    lightScattering *= timeDot;

    [unroll(7)]
    for (int i = 0; i < 7; i++)
    {
        cumulativeDensity += GetDensity(rayPos) * size * density * lightScattering;

        if (exp(-cumulativeDensity) < 0.1)
        {
            break;
        }

        float _jitter = 0.8;
        float randOffset = (random3D(rayPos) * (2 * (1 - _jitter))) + (_jitter);
        rayPos += ray * size * randOffset;
    }

    return exp(-cumulativeDensity);
}

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;

    lightSteps = 3;
    pos.xz += _WorldSpaceCameraPos.xz;

    Light mainLight = GetMainLight(float4(0, 0, 0, 0), IN.WorldSpacePosition, 1);
    float3 dir = mainLight.direction;
    float3 color = mainLight.color;

    float3 background = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.ScreenPosition.xy / IN.ScreenPosition.w);
    background = float3(1, 1, 1);

    float3 posBL = pos - bounds / 2;
    float3 posTR = pos + bounds / 2;

    float3 viewVector = mul(unity_CameraInvProjection, float4(IN.ScreenPosition.xy * 2 - 1, 0, -1));
    float3 viewDir = mul(unity_CameraToWorld, float4(viewVector, 0));
    float viewLength = length(viewDir);

    float3 ray = GetRay(IN.ScreenPosition.xy);

    float2 boxDist = rayBoxDst(posBL, posTR, _WorldSpaceCameraPos, 1 / ray);

    float distToBox = boxDist.x;
    float distInBox = boxDist.y;

    float3 entryPoint = _WorldSpaceCameraPos + ray * distToBox;

    float3 rayPos = entryPoint + ray * viewLength;

    float depth = SceneDepth(IN.ScreenPosition.xy / IN.ScreenPosition.w) * viewLength;
    //depth = _ProjectionParams.z;

    if (distInBox == 0 || distToBox > depth)
    {
        surface.BaseColor = background;
        return surface;
    }

    float cumulativeDensity = 0;
    float distTravelled = 0;
    float light = 0;
    float transmittance = 1;
    int i = 0;
    UNITY_LOOP
        for (i = 0; i < steps && distTravelled + distToBox < depth && distTravelled < distInBox; i++)
        {
            float shadowAtten = 1;
            float shadowAttenL = 1;
            float shadowAttenR = 1;
            float pointDensity;
            float distMultiplier = 1;
            if (shadows)
            {
                float diff = 0.5;

#if SHADOWS_SCREEN
                half4 clipPos = TransformWorldToHClip(rayPos);
                half4 shadowCoord = ComputeScreenPos(clipPos);

                half4 clipPosL = TransformWorldToHClip(rayPos + float3(-diff, -diff, -diff));
                half4 shadowCoordL = ComputeScreenPos(clipPosL);

                half4 clipPosR = TransformWorldToHClip(rayPos + float3(diff, diff, diff));
                half4 shadowCoordR = ComputeScreenPos(clipPosR);
#else
                half4 shadowCoord = TransformWorldToShadowCoord(rayPos);
                half4 shadowCoordL = TransformWorldToShadowCoord(rayPos + float3(-diff, -diff, -diff));
                half4 shadowCoordR = TransformWorldToShadowCoord(rayPos + float3(diff, diff, diff));
#endif
                Light mainLight = GetMainLight(shadowCoord, rayPos, 1);
                shadowAtten = mainLight.shadowAttenuation;

                Light mainLightL = GetMainLight(shadowCoordL, rayPos, 1);
                shadowAttenL = mainLightL.shadowAttenuation;

                Light mainLightR = GetMainLight(shadowCoordR, rayPos, 1);
                shadowAttenR = mainLightR.shadowAttenuation;

                float diffL = abs(shadowAtten - shadowAttenL);
                float diffR = abs(shadowAtten - shadowAttenR);

                if (max(diffL, diffR) > 0.3 && distTravelled < 70)
                {
                    float distStrength = 1 - pow(distTravelled / 70, 6);
                    float timeStrength = pow(1 - abs(dot(_MainLightPosition.xyz, float3(0, 1, 0))), 0.1) * 0.7;
                    shadowAtten = godRayStrength * (timeStrength + 0.3) * distStrength;
                }
                else
                {
                    shadowAtten /= 2;
                }

                //            float additionalShadows = 0;
                //#ifdef _ADDITIONAL_LIGHTS
                //            int pixelLightCount = GetAdditionalLightsCount();
                //            for (int a = 0; a < pixelLightCount; ++a)
                //            {
                //                Light light = GetAdditionalLight(a, rayPos, 1);
                //
                //                additionalShadows += light.distanceAttenuation * light.shadowAttenuation * length(light.color);
                //            }
                //#endif
                //            shadowAtten = max(shadowAtten, min(additionalShadows, godRayStrength));

                float heightFalloff = 1 - ((rayPos.y - pos.y + (bounds.y / 2)) / bounds.y);
                pointDensity = density * heightFalloff * ((pow(shadowAtten, 20) * 0.9) + 0.1);
                cumulativeDensity += pointDensity * stepSize;
            }
            else
            {
                float cameraDist = distance(_WorldSpaceCameraPos, rayPos);
                cameraDist = 1 - (min(cameraDist, 100000) / 100000);
                cameraDist = pow(cameraDist, 5);
                pointDensity = GetDensity(rayPos) * density * cameraDist;
                cumulativeDensity += pointDensity * stepSize;
            }

            if (shadows)
            {
                light += 1;
            }
            else
            {
                //float cameraDist = distance(_WorldSpaceCameraPos, rayPos);
                //cameraDist = 1 - (min(cameraDist, 100000) / 100000);
                //cameraDist = pow(cameraDist, 5);

                float lightTransmittance = lightingAtPoint(rayPos, dir);
                light += pointDensity * stepSize * transmittance * lightTransmittance;
                transmittance *= exp(-cumulativeDensity);
            }

            float cameraDist = distance(_WorldSpaceCameraPos, rayPos);
            if (exp(-cumulativeDensity) < 0.1 || (cameraDist > 100000 && !shadows))
            {
                break;
            }

            float randOffset = (random3D(rayPos) * (2 * (1 - jitter))) + (jitter);

            distTravelled += stepSize * randOffset;

            rayPos += ray * stepSize * randOffset;

            if (distTravelled > 70 && shadows)
            {
                stepSize += 10 * distMultiplier;
                distMultiplier += 5;
            }
        }

    float phase = Phase(ray, dir);
    if (shadows)
    {
        light /= i;
        phase = 1;
    }

    float3 lighting = light / lightSteps * phase;

    if (!shadows)
    {
        lighting *= Albedo;
    }

    lighting *= 1 - minDarkness;
    lighting += minDarkness;


    if (shadows)
    {
        lighting *= Albedo;
    }

    float timeShadow = pow(1 - max(dot(dir, float3(0, -1, 0)), 0), 5);
    timeShadow *= 0.9;
    timeShadow += 0.1;

    surface.BaseColor = clamp(lerp(background, lighting * saturation(normalize(color), pow(clamp(lighting, 0.2, 1), 1.5) * 1.2) * 4 * timeShadow, 1 - exp(-cumulativeDensity)), 0, 3);
    surface.Alpha = 1 - exp(-cumulativeDensity);

    return surface;
}
#endif
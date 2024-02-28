Shader "GapperGames/STINGRAY_Volume"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        [HDR] Albedo("Albedo", Color) = (1, 1, 1, 1)
        density("Density", Range(0, 1)) = 0.5
        scale("Scale", Float) = 1
        speed("Speed", Float) = 0.2
        lightScattering("Light Scattering", Range(0, 1)) = 1
        minDarkness("Min Darkness", Range(0, 1)) = 0.5
        steps("Steps", Float) = 1000
        stepSize("Step Size", Float) = 0.5
        jitter("Jitter Amount", Range(0, 1)) = 0.1
        twirl("Twirl Amount", Range(0, 1)) = 0.1
        densityOffset("Density Offset", Range(0, 1)) = 0
        pos("Position", Vector) = (0, 0, 0, 0)
        bounds("Bounds", Vector) = (5, 5, 5, 0)
        [ToggleUI]shadows("Use Shadows", Float) = 0
        godRayStrength("God Ray Strength", Range(0, 1.5)) = 1
        _NoiseTexture("Noise Texture", 3D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "UniversalMaterialType" = "Lit"
            "Queue" = "Geometry"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

    Cull Back
    Blend One Zero
    ZTest LEqual
    ZWrite On

        HLSLPROGRAM

    #pragma target 4.5
    #pragma exclude_renderers gles gles3 glcore
    #pragma multi_compile_instancing
    #pragma multi_compile_fog
    #pragma multi_compile _ DOTS_INSTANCING_ON
    #pragma vertex vert
    #pragma fragment frag

    #pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
    #pragma multi_compile _ LIGHTMAP_ON
    #pragma multi_compile _ DIRLIGHTMAP_COMBINED
    #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
    #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
    #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
    #pragma multi_compile _ _SHADOWS_SOFT
    #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
    #pragma multi_compile _ SHADOWS_SHADOWMASK

        #define _NORMALMAP 1
        #define _NORMAL_DROPOFF_TS 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #define FEATURES_GRAPH_VERTEX

    #define SHADERPASS SHADERPASS_FORWARD
    #define REQUIRE_DEPTH_TEXTURE
    #define REQUIRE_OPAQUE_TEXTURE

        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        //#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

    struct Attributes
    {
        float3 positionOS : POSITION;
        float3 normalOS : NORMAL;
        float4 tangentOS : TANGENT;
        float4 uv1 : TEXCOORD1;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float3 positionWS;
        float3 normalWS;
        float4 tangentWS;
        float3 viewDirectionWS;
        #if defined(LIGHTMAP_ON)
        float2 lightmapUV;
        #endif
        #if !defined(LIGHTMAP_ON)
        float3 sh;
        #endif
        float4 fogFactorAndVertexLight;
        float4 shadowCoord;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
        float3 WorldSpaceNormal;
        float3 TangentSpaceNormal;
        float3 WorldSpaceViewDirection;
        float3 WorldSpacePosition;
        float4 ScreenPosition;
    };
    struct VertexDescriptionInputs
    {
        float3 ObjectSpaceNormal;
        float3 ObjectSpaceTangent;
        float3 ObjectSpacePosition;
    };
    struct PackedVaryings
    {
        float4 positionCS : SV_POSITION;
        float3 interp0 : TEXCOORD0;
        float3 interp1 : TEXCOORD1;
        float4 interp2 : TEXCOORD2;
        float3 interp3 : TEXCOORD3;
        #if defined(LIGHTMAP_ON)
        float2 interp4 : TEXCOORD4;
        #endif
        #if !defined(LIGHTMAP_ON)
        float3 interp5 : TEXCOORD5;
        #endif
        float4 interp6 : TEXCOORD6;
        float4 interp7 : TEXCOORD7;
        #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

    PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.positionWS;
        output.interp1.xyz = input.normalWS;
        output.interp2.xyzw = input.tangentWS;
        output.interp3.xyz = input.viewDirectionWS;
        #if defined(LIGHTMAP_ON)
        output.interp4.xy = input.lightmapUV;
        #endif
        #if !defined(LIGHTMAP_ON)
        output.interp5.xyz = input.sh;
        #endif
        output.interp6.xyzw = input.fogFactorAndVertexLight;
        output.interp7.xyzw = input.shadowCoord;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }
    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.positionWS = input.interp0.xyz;
        output.normalWS = input.interp1.xyz;
        output.tangentWS = input.interp2.xyzw;
        output.viewDirectionWS = input.interp3.xyz;
        #if defined(LIGHTMAP_ON)
        output.lightmapUV = input.interp4.xy;
        #endif
        #if !defined(LIGHTMAP_ON)
        output.sh = input.interp5.xyz;
        #endif
        output.fogFactorAndVertexLight = input.interp6.xyzw;
        output.shadowCoord = input.interp7.xyzw;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    struct VertexDescription
    {
        float3 Position;
        float3 Normal;
        float3 Tangent;
    };

    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
    {
        VertexDescription description = (VertexDescription)0;
        description.Position = IN.ObjectSpacePosition;
        description.Normal = IN.ObjectSpaceNormal;
        description.Tangent = IN.ObjectSpaceTangent;
        return description;
    }

    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
    {
        VertexDescriptionInputs output;
        ZERO_INITIALIZE(VertexDescriptionInputs, output);

        output.ObjectSpaceNormal = input.normalOS;
        output.ObjectSpaceTangent = input.tangentOS.xyz;
        output.ObjectSpacePosition = input.positionOS;

        return output;
    }

    struct SurfaceDescription
    {
        float3 BaseColor;
        float Alpha;
    };
    
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
    {
        SurfaceDescriptionInputs output;
        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        float3 unnormalizedNormalWS = input.normalWS;
        const float renormFactor = 1.0 / length(unnormalizedNormalWS);

        output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;
        output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


        output.WorldSpaceViewDirection = input.viewDirectionWS;
        output.WorldSpacePosition = input.positionWS;
        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS));
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

        return output;
    }

#include "Volumetrics.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
#include "UnlitShaderPass.hlsl"

    ENDHLSL
}
    }
        FallBack "Hidden/Shader Graph/FallbackError"
}
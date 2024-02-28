Shader "Custom/InteriorMapping - simple"
{
        Properties
        {
                _RoomTex("Room Atlas RGB (alpha is not needed)", 2D) = "gray" {}
                _RoomMaxDepth01("Room Max Depth define(0 to 1)", range(0.001,0.999)) = 0.5
                _RoomCount("Room Count(X count,Y count)", vector) = (1,1,0,0)
        }
        SubShader
        {
                Tags { "RenderType" = "Opaque" }
 
                Pass
                {
                        CGPROGRAM
                        #pragma vertex vert
                        #pragma fragment frag
 
                        #include "UnityCG.cginc"
                        #include "InteriorUVFunction.hlsl"
 
                        struct appdata
                        {
                                float4 vertex : POSITION;
                                float2 uv : TEXCOORD0;
                                float3 normal : NORMAL;
                                float4 tangent : TANGENT;
                        };
 
                        struct v2f
                        {
                                float4 pos : SV_POSITION;
                                float2 uv : TEXCOORD0;
                                float3 tangentViewDir : TEXCOORD1;
                        };
 
                        sampler2D _RoomTex;
                        float4 _RoomTex_ST;
 
                        float _RoomMaxDepth01;
                        float2 _RoomCount;
 
                        float3 DirObjectSpaceToTangentSpace(float3 inputDirOS, float3 normalOS, float4 tangentOS)
                        {
                                float tangentSign = tangentOS.w * unity_WorldTransformParams.w;
                                float3 bitangentOS = cross(normalOS, tangentOS.xyz) * tangentSign;
                                return float3(
                                        dot(inputDirOS, tangentOS.xyz),
                                        dot(inputDirOS, bitangentOS),
                                        dot(inputDirOS, normalOS)
                                        );
                        }
 
                        v2f vert(appdata v)
                        {
                                v2f o;
 
                                //regular
                                o.pos = UnityObjectToClipPos(v.vertex);
 
                                //tile uv base on room count in vertex shader
                                o.uv = v.uv * _RoomCount;
 
                                //find view dir Object Space
                                float3 camPosObjectSpace = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1.0)).xyz;
                                float3 viewDirObjectSpace = v.vertex.xyz - camPosObjectSpace;
 
                                //get tangent space view vector
                                o.tangentViewDir = DirObjectSpaceToTangentSpace(viewDirObjectSpace, v.normal, v.tangent);
 
                                return o;
                        }
 
                        fixed4 frag(v2f i) : SV_Target
                        {
                                float2 interiorUV = ConvertOriginalRawUVToInteriorUV(frac(i.uv), i.tangentViewDir, _RoomMaxDepth01);
                                interiorUV /= _RoomCount;
                                interiorUV = TRANSFORM_TEX(interiorUV, _RoomTex);
 
                                //map to differrent room if needed
                                float2 roomIndex = floor(i.uv);
                                interiorUV += roomIndex / _RoomCount;
 
                                return tex2D(_RoomTex, interiorUV);
                        }
                        ENDCG
                }
        }
}
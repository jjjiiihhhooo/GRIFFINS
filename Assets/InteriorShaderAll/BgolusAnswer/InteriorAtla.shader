//copied & refactor, source = https://forum.unity.com/threads/interior-mapping.424676/#post-2751518
Shader "Custom/InteriorMapping - 2D Atlas"
{
	Properties
	{
		_RoomTex("Room Atlas RGB (A - back wall depth01)", 2D) = "gray" {}
		_Rooms("Room Atlas Rows&Cols (XY)", Vector) = (1,1,0,0)
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
				#include "InteriorUVFunction.hlsl"
				#include "RandomFunction.hlsl"
				#include "TangentSpaceFunction.hlsl"

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
				float2 _Rooms;

				v2f vert(appdata v)
				{
					v2f o;

					//regular
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _RoomTex);

					//find view dir OS
					float3 camPosOS = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1.0)).xyz;
					float3 viewDirOS = v.vertex.xyz - camPosOS;

					// get tangent space view vector
					o.tangentViewDir = DirOSToTS(viewDirOS, v.normal, v.tangent);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// room uvs
					float2 roomUV = frac(i.uv);
					float2 roomIndexUV = floor(i.uv);

					// randomize the room
					float2 n = floor(rand2(roomIndexUV.x + roomIndexUV.y * (roomIndexUV.x + 1)) * _Rooms.xy);
					roomIndexUV += n; //colin: result = index XY + random (0,0)~(3,1)

					// get room depth from room atlas alpha
					fixed roomMaxDepth01 = tex2D(_RoomTex, (roomIndexUV + 0.5) / _Rooms).a;
					// sample room atlas texture
					float2 interiorUV = ConvertOriginalRawUVToInteriorUV(roomUV, i.tangentViewDir, roomMaxDepth01);
					fixed4 room = tex2D(_RoomTex, (roomIndexUV + interiorUV) / _Rooms);
					return fixed4(room.rgb, 1.0);
				}
				ENDCG
			}
		}
}
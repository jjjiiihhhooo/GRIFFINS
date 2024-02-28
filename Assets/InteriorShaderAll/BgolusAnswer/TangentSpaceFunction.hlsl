#ifndef TANGENTSPACE_FUNCTION_INCLUDED
#define TANGENTSPACE_FUNCTION_INCLUDED
float3 DirOSToTS(float3 inputDirOS, float3 normalOS, float4 tangentOS)
{
	float tangentSign = tangentOS.w * unity_WorldTransformParams.w;
	float3 bitangentOS = cross(normalOS, tangentOS.xyz) * tangentSign;
	return float3(
		dot(inputDirOS, tangentOS.xyz),
		dot(inputDirOS, bitangentOS),
		dot(inputDirOS, normalOS)
		);
}
#endif
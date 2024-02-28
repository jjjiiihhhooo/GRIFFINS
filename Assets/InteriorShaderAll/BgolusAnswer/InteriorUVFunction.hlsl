#ifndef INTERRIOR_UV_FUNCTION_INCLUDED
#define INTERRIOR_UV_FUNCTION_INCLUDED
//bgolus's original source code: https://forum.unity.com/threads/interior-mapping.424676/#post-2751518
//this reusable InteriorUVFunction.hlsl is created base on bgolus's original source code

//for param "roomMaxDepth01Define": input 0.0001 if room is a "near 0 volume" room, input 0.9999 if room is a "near inf depth" room
float2 ConvertOriginalRawUVToInteriorUV(float2 originalRawUV, float3 viewDirTangentSpace, float roomMaxDepth01Define)
{
	//remap [0,1] to [+inf,0]
	//->if input roomMaxDepth01Define = 0    -> depthScale = +inf   (0 volume room)
	//->if input roomMaxDepth01Define = 0.5  -> depthScale = 1
	//->if input roomMaxDepth01Define = 1    -> depthScale = 0              (inf depth room)
	float depthScale = rcp(roomMaxDepth01Define) - 1.0;

	//normalized box space is a space where room's min max corner = (-1,-1,-1) & (+1,+1,+1)
	//apply simple scale & translate to tangent space = transform tangent space to normalized box space

	//now prepare ray box intersection test's input data in normalized box space
	float3 viewRayStartPosBoxSpace = float3(originalRawUV * 2 - 1, -1); //normalized box space's ray start pos is on trinagle surface, where z = -1
	float3 viewRayDirBoxSpace = viewDirTangentSpace * float3(1, 1, -depthScale);//transform input ray dir from tangent space to normalized box space

	//do ray & axis aligned box intersection test in normalized box space (all input transformed to normalized box space)
	//intersection test function used = https://www.iquilezles.org/www/articles/intersectors/intersectors.htm
	//============================================================================
	float3 viewRayDirBoxSpaceRcp = rcp(viewRayDirBoxSpace);

	//hitRayLengthForSeperatedAxis means normalized box space depth hit per x/y/z plane seperated
	//(we dont care about near hit result here, we only want far hit result)
	float3 hitRayLengthForSeperatedAxis = abs(viewRayDirBoxSpaceRcp) - viewRayStartPosBoxSpace * viewRayDirBoxSpaceRcp;
	//shortestHitRayLength = normalized box space real hit ray length
	float shortestHitRayLength = min(min(hitRayLengthForSeperatedAxis.x, hitRayLengthForSeperatedAxis.y), hitRayLengthForSeperatedAxis.z);
	//normalized box Space real hit pos = rayOrigin + t * rayDir.
	float3 hitPosBoxSpace = viewRayStartPosBoxSpace + shortestHitRayLength * viewRayDirBoxSpace;
	//============================================================================

	// remap from [-1,1] to [0,1] room depth
	float interp = hitPosBoxSpace.z * 0.5 + 0.5;

	// account for perspective in "room" textures
	// assumes camera with an fov of 53.13 degrees (atan(0.5))
	//hard to explain, visual result = transform nonlinear depth back to linear
	float realZ = saturate(interp) / depthScale + 1;
	interp = 1.0 - (1.0 / realZ);
	interp *= depthScale + 1.0;

	//linear iterpolate from wall back to near
	float2 interiorUV = hitPosBoxSpace.xy * lerp(1.0, 1 - roomMaxDepth01Define, interp);

	//convert back to valid 0~1 uv, ready for user's tex2D() call
	interiorUV = interiorUV * 0.5 + 0.5;
	return interiorUV;
}
#endif
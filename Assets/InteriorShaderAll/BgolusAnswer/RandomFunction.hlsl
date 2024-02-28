#ifndef RANDOM_FUNCTION_INCLUDED
#define RANDOM_FUNCTION_INCLUDED
// psuedo random
float2 rand2(float co) {
	return frac(sin(co * float2(12.9898, 78.233)) * 43758.5453);
}
#endif
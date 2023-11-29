#if SHADERGRAPH_PREVIEW
// 미리보기 모드에서 사용할 코드
Direction = float3(1, 1, 1);

#else

Light light = GetMainLight();
Direction = light.direction;
#endif
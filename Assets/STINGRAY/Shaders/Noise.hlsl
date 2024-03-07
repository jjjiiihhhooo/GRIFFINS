#ifndef Noise_Functions
#define Noise_Functions

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
    //return Out * 0.02;
    return Out;
}

float random3D(float3 uv)
{
    float Coord = (uv.x + uv.y + uv.z);
    float2 _uv = float2(Coord, Coord);
    float2 noise = (frac(sin(dot(_uv, float2(12.9898, 78.233) * 2.0)) * 43758.5453));
    return (abs(noise.x + noise.y) - 1);
}

float2 random2D(float2 UV, float offset)
{
    float2x2 m = float2x2(15.27f, 47.63f, 99.41f, 89.98f);
    UV = frac(sin(mul(UV, m)));
    return float2(sin(UV.y * +offset) * 0.5f + 0.5f, cos(UV.x * offset) * 0.5f + 0.5f);
}

float3 randomVector(float3 uv)
{
    float scale = 0.005;

    float x = random2D(uv.xy * scale, 15);
    float y = random2D(uv.yz * scale, 15);
    float z = random2D(uv.zx * scale, 15);

    float3 Out = float3(x, y, z);

    Out *= 2;
    Out -= 1;

    return Out;
}

float2 Cellular(float2 UV)
{
    UV *= 0.2;
    float AngleOffset = 30;
    float2 g = floor(UV);
    float2 f = frac(UV);
    float2 res = float2(8, 8);

    //UNITY_LOOP
    [unroll]
    for (int y = -1; y <= 1; y++)
    {
        //UNITY_LOOP
        [unroll]
        for (int x = -1; x <= 1; x++)
        {
            float2 lattice = float2(x, y);
            float2 offset = random2D(lattice + g, AngleOffset);
            float dist = distance(lattice + offset, f);

            if (dist < res.x)
            {
                res.y = length(offset);
                res.x = dist;
            }
        }
    }

    return res;
}

float Cellular3D(float3 uv)
{
    float xy = Cellular(uv.xy);
    float yz = Cellular(uv.yz);
    float xz = Cellular(uv.xz);

    float yx = Cellular(uv.yx);
    float zy = Cellular(uv.zy);
    float zx = Cellular(uv.zx);

    float noise = (xy + xz + zy) * (yx + zy + zx);
    noise /= 3;

    return noise;
}

#endif
#ifndef DISPLACEMENT_INCLUDED
#define DISPLACEMENT_INCLUDED

struct Wave
{
    float frequency;
    float2 direction;
    float speed;
    float amplitude;
};

int wavesCount;
StructuredBuffer<Wave> waves;

void Displacement_float(float3 pos, float2 phaseOffset, float frequency, float2 direction, float time, float speed, out float displacement)
{
    float xPhase = pos.x * (-direction.x) + time * speed + phaseOffset.x;
    float zPhase = pos.z * (-direction.y) + time * speed + phaseOffset.y;

    displacement = sin((xPhase + zPhase) * frequency);
}

float4 Unity_DDX_float4(float4 In)
{
    return ddx(In);
}

void DisplacementStructured_float(float3 pos, float time, out float displacement)
{
    displacement = 0.0f;

    float posX = pos.x;
    float posZ = pos.z;
    
    for (int i = 0; i < wavesCount; i++)
    {
        float frequency = waves[i].frequency;
        float2 direction = waves[i].direction;
        float speed = waves[i].speed;
        float amplitude = waves[i].amplitude;

        float xPhase = posX * (-direction.x) + time * speed;
        float zPhase = posZ * (-direction.y) + time * speed;

        displacement += amplitude * exp(sin((xPhase + zPhase) * frequency));
    }
}

#endif 
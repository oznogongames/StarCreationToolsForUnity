#ifndef NOISE_HELPER
#define NOISE_HELPER

#include "WebGLNoisePort/noiseSimplex.cginc"

#define PI 3.141592653589793


inline float2 RadialCoords(float3 a_coords)
{
	float3 a_coords_n = normalize(a_coords);
	float lon = atan2(a_coords_n.z, a_coords_n.x);
	float lat = acos(a_coords_n.y);
	float2 sphereCoords = float2(lon, lat) * (1.0 / PI);
	return float2(sphereCoords.x * 0.5 + 0.5, 1 - sphereCoords.y);
}

float snoise_n1_to_1(float4 pos)
{
	return 2 * (snoise(pos) - 0.5);
}

float snoise_n1_to_1(float3 pos, float time)
{
	return snoise_n1_to_1(float4(pos.xyz, time));
}

float snoise_turbulence_multiplicative(float3 pos, float time, int raised)
{
	float val = 0;
	for (int i = 1; i <= raised; i++)
	{
		float power = pow(2.0, float(i + 1));
		val *= (0.5 / power) * snoise(float4(pos.xyz * power, time));
	}
	return val;
}

float snoise_turbulence_multiplicative(float2 pos, float time, int raised)
{
	float val = 0;
	for (int i = 1; i <= raised; i++)
	{
		float power = pow(2.0, float(i + 1));
		val *= (0.5 / power) * snoise(float3(pos.xy * power, time));
	}
	return val;
}

float snoise_turbulence_additive(float3 pos, float time, int raised)
{
	float val = 0;
	for (int i = 1; i <= raised; i++)
	{
		float power = pow(2.0, float(i + 1));
		val += (0.5 / power) * snoise(float4(pos.xyz * power, time));
	}
	return val;
}

float snoise_turbulence_additive(float2 pos, float time, int raised)
{
	float val = 0;
	for (int i = 1; i <= raised; i++)
	{
		float power = pow(2.0, float(i + 1));
		val += (0.5 / power) * snoise(float3(pos.xy * power, time));
	}
	return val;
}

float snoise_turbulence_minimized(float3 pos, float time, int raised)
{
	float val = 0;
	for (int i = 1; i <= raised; i++)
	{
		float power = pow(2.0, float(i + 1));
		val = min(val, (0.5 / power) * snoise(float4(pos.xyz * power, time)));
	}
	return val;
}

float snoise_turbulence_minimized(float2 pos, float time, int raised)
{
	float val = 0;
	for (int i = 1; i <= raised; i++)
	{
		float power = pow(2.0, float(i + 1));
		val = min(val, (0.5 / power) * snoise(float3(pos.xy * power, time)));
	}
	return val;
}

#endif
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "SunForge/CoronaShader"
{
	Properties
	{
		_StarCenter("Star Center", Vector) = (0, 0, 0, 0)	//x, y, z, radius
		_StarColor("Star Color", Color) = (1, 1, 1, 1)
		//_CoronaSettings("Corona settings", Vector) = (10, 5, 0, 0)
		_TimeScale("Time Scale", Float) = 0.05
		_Resolution("Resolution", Float) = 5
		_RotRate("Rotation Speed", Vector) = (0, -1, 0, 0)

	}
		SubShader
	{
		Tags{ "DisableBatching" = "True" "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True"
	}

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		LOD 100

		Pass
	{
		CGPROGRAM
		#pragma target 3.0
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"
		#include "noiseHelper.cginc"

		float4 _StarColor;
	float4 _StarCenter;
	//float4 _CoronaSettings;
	float _TimeScale;
	float _Resolution;
	float4 _RotRate;


	struct appdata
	{
		float4 vertex :
		POSITION;
	};

	struct v2f
	{
		float4 vertex :
		POSITION;
		float3 position_in_world_space :
		TEXCOORD0;
		float dist :
		float;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = mul((float4x4) unity_ObjectToWorld, v.vertex);
		o.dist = distance(o.vertex, _StarCenter.xyz);
		o.position_in_world_space = float3(o.vertex.x, o.vertex.y, o.vertex.z) - _StarCenter.xyz;
		o.vertex = UnityObjectToClipPos(v.vertex);

		float s = sin(_RotRate.x * _Time);
		float c = cos(_RotRate.x * _Time);
		float3x3 rotationMatrix_x = float3x3(1, 0, 0, 0, c, -s, 0, s, c);
		s = sin(_RotRate.y * _Time);
		c = cos(_RotRate.y * _Time);
		float3x3 rotationMatrix_y = float3x3(c, 0, s, 0, 1, 0, -s, 0, c);
		s = sin(_RotRate.z * _Time);
		c = cos(_RotRate.z * _Time);
		float3x3 rotationMatrix_z = float3x3(c, -s, 0, s, c, 0, 0, 0, 1);

		o.position_in_world_space = mul(o.position_in_world_space, rotationMatrix_x);
		o.position_in_world_space = mul(o.position_in_world_space, rotationMatrix_y);
		o.position_in_world_space = mul(o.position_in_world_space, rotationMatrix_z);

		return o;
	}

	float star_base_noise_color(float3 pos, float time)
	{
		float noise1 = snoise_turbulence_additive(pos / 5, time, 5);
		float noise2 = snoise_turbulence_additive(pos / 10, time + 25, 5);
		float noise3 = snoise_turbulence_minimized(pos * min(noise1, noise2), time + 50, 2) * 2;
		float noise4 = snoise_turbulence_additive(pos, time, 3);

		float noise5 = snoise_turbulence_additive(pos / 25, time * 0.3, 5);


		float noiseFinal = (noise1 + noise2 + noise3 + noise4 - (noise5 * 1.5));
		return noiseFinal;
	}

	float4 frag(v2f i) : COLOR
	{   //First get color
		float time_offset = _Time * _TimeScale;
	float3 pos_offset = i.position_in_world_space / _Resolution;

	float noise_base = (star_base_noise_color(pos_offset , time_offset));
	float4 color = _StarColor + float4(noise_base, noise_base, noise_base, 0);

	//Get distance as a ratio
	float distanceFromSurface = (i.dist);
	float distanceRatio = (_StarCenter.w) / distanceFromSurface;

	noise_base = pow(distanceRatio + 0.05, 10);
	color.w = noise_base;

	return color;
	}

		ENDCG
	}
	}
}

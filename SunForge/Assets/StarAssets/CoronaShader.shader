// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "SunForge/CoronaShader"
{
	//https://www.shadertoy.com/view/4dXGR4
	//https://www.shadertoy.com/view/MsVXWW
	//https://www.shadertoy.com/view/XlfGRj
	//https://alastaira.wordpress.com/2015/08/07/unity-shadertoys-a-k-a-converting-glsl-shaders-to-cghlsl/
	//https://www.seedofandromeda.com/blogs/49-procedural-gas-giant-rendering-with-gpu-noise
	Properties
	{
		_StarCenter("Star Center", Vector) = (0, 0, 0, 0)	//x, y, z, radius
		_StarColor("Star Color", Color) = (1, 1, 1, 1)
		_AbsMagnitude("Absolute Magnitude", Range(-12, 16)) = 4
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
	float _AbsMagnitude;

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
		o.position_in_world_space = float3(o.vertex.x, o.vertex.y, o.vertex.z);
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.dist = distance(o.position_in_world_space, _StarCenter.xyz);

		return o;
	}

	float star_base_noise_alpha(float3 pos, float time)
	{
		float noise1 = snoise_turbulence_additive(pos / 5, time, 5);
		float noise2 = snoise_turbulence_additive(pos * noise1, time, 5);
		float noiseFinal = noise2 * noise2 * noise2;
		return noiseFinal;
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
		float time_offset = _Time.y * 0.5;
	float3 pos_offset = i.position_in_world_space / 5;
	float noise_base = (star_base_noise_color(pos_offset , time_offset));
	float offset = 1 * (noise_base);
	float4 color = _StarColor + float4(offset, offset, offset, 0);

	//Get distance as a ratio
	float distanceFromSurface = (i.dist);
	float distanceRatio = (_StarCenter.w) / distanceFromSurface;

	offset = pow(distanceRatio, 10) * pow(color.x, 2);
	color.w = offset;

	return color;
	}

		ENDCG
	}
	}
}

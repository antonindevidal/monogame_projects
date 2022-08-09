#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;
float time;

struct InstancingVSInput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

struct InstancingVSOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
};

InstancingVSOutput InstancingVS(in InstancingVSInput input, float4 instanceTransform : POSITION1, float3 rotation: POSITION2)
{
	InstancingVSOutput output = (InstancingVSOutput)0;
	matrix  r = matrix(
		float4(cos(rotation.y)	, 0		,sin(rotation.y), 0),
		float4(0				, 1		,0				, 0),
		float4(-sin(rotation.y)	, 0		,cos(rotation.y), 0),
		float4(0				, 0		,0				, 1));
	float4 pos = mul(input.Position, r) ;
	float decal = sin(((instanceTransform.x+ instanceTransform.y)*5+time)*0.002) ;
	pos = pos+ instanceTransform;
	pos.x =pos.x + decal * input.Position.y;


	pos = mul(pos, WorldViewProjection);

	output.Position = pos;
	output.Color = input.Color;

	return output;
}

float4 InstancingPS(InstancingVSOutput input) : COLOR
{
	return input.Color;
}

technique Instancing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL InstancingVS();
		PixelShader = compile PS_SHADERMODEL InstancingPS();
	}
};
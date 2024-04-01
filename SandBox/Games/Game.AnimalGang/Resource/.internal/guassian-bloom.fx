#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

Texture2D PassTexture;

float Threshold;
float2 SamplingOffset;
float4 Color;
float Intensity;

SamplerState LinearSampler
{
    Texture = <PassTexture>;  // Assigning the texture named "PassTexture" to the sampler state

    MagFilter = LINEAR;         // Setting magnification filter to LINEAR
    MinFilter = LINEAR;         // Setting minification filter to LINEAR
    Mipfilter = LINEAR;         // Setting mipmapping filter to LINEAR

    AddressU = CLAMP;           // Setting addressing mode for U coordinate to CLAMP
    AddressV = CLAMP;           // Setting addressing mode for V coordinate to CLAMP
};

struct VertexInput
{
    float4 Position : POSITION0;
    float4 TexCoord : TEXCOORD0;
};

struct PixelInput
{
    float4 Position : SV_Position0;
    float4 TexCoord : TEXCOORD0;
};

PixelInput VS(VertexInput v)
{
    PixelInput output;

    output.Position = v.Position;
    output.TexCoord = v.TexCoord;
    return output;
}

float4 ExtractPS(PixelInput p) : SV_TARGET
{
	float4 texCol = PassTexture.Sample(LinearSampler, p.TexCoord.xy);
	float brightness = dot(texCol.rgb, float3(0.2126, 0.7152, 0.0722));

    if(brightness > Threshold)
        return texCol * Color;
    else
        return float4(0, 0, 0, 0);
}

float4 Box(float4 p0, float4 p1, float4 p2, float4 p3)
{
	return (p0 + p1 + p2 + p3) * 0.25f;
}

float4 DownsamplePS(PixelInput p) : SV_TARGET
{
	float4 cen = PassTexture.Sample(LinearSampler, p.TexCoord.xy);

	float4 in0 = PassTexture.Sample(LinearSampler, p.TexCoord.xy + SamplingOffset * float2(-1, -1));
	float4 in1 = PassTexture.Sample(LinearSampler, p.TexCoord.xy + SamplingOffset * float2(-1, +1));
	float4 in2 = PassTexture.Sample(LinearSampler, p.TexCoord.xy + SamplingOffset * float2(+1, +1));
	float4 in3 = PassTexture.Sample(LinearSampler, p.TexCoord.xy + SamplingOffset * float2(+1, -1));

	float4 out0 = PassTexture.Sample(LinearSampler, p.TexCoord.xy + SamplingOffset * float2(-2, -2));
	float4 out1 = PassTexture.Sample(LinearSampler, p.TexCoord.xy + SamplingOffset * float2(-2,  0));
	float4 out2 = PassTexture.Sample(LinearSampler, p.TexCoord.xy + SamplingOffset * float2(-2, +2));
	float4 out3 = PassTexture.Sample(LinearSampler, p.TexCoord.xy + SamplingOffset * float2( 0, +2));
	float4 out4 = PassTexture.Sample(LinearSampler, p.TexCoord.xy + SamplingOffset * float2(+2, +2));
	float4 out5 = PassTexture.Sample(LinearSampler, p.TexCoord.xy + SamplingOffset * float2(+2,  0));
	float4 out6 = PassTexture.Sample(LinearSampler, p.TexCoord.xy + SamplingOffset * float2(+2, -2));
	float4 out7 = PassTexture.Sample(LinearSampler, p.TexCoord.xy + SamplingOffset * float2( 0, -2));

	float4 result =
		Box(out0, out1, cen , out7) * 0.125f +
		Box(out1, out2, out3, cen ) * 0.125f +
		Box(cen , out3, out4, out5) * 0.125f +
		Box(out5, out6, out7, cen ) * 0.125f + 
		Box(in0 , in1 , in2 , in3 ) * 0.5f;

    return result;
} 


float4 UpsamplePS(PixelInput p) : SV_TARGET
{
	float4 texCol = PassTexture.Sample(LinearSampler, p.TexCoord.xy);
	return texCol;
}

technique
{
    pass Extract
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL ExtractPS();
    }

	pass Downsample
	{
		VertexShader = compile VS_SHADERMODEL VS();
		PixelShader = compile PS_SHADERMODEL DownsamplePS();
	}

	pass Upsample
	{
		VertexShader = compile VS_SHADERMODEL VS();
		PixelShader = compile PS_SHADERMODEL UpsamplePS();
	}
}
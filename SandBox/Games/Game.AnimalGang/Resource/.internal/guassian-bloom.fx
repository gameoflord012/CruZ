#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

Texture2D PassTexture;
Texture2D OriginalTexture;

float4 Color;
float Threshold;
float Exposure;

static const float weight[5] = { 0.227027, 0.1945946, 0.1216216, 0.054054, 0.016216 };

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
	float4 texCol = PassTexture.Sample(LinearSampler, p.TexCoord.xy) * Color;
	float brightness = dot(texCol.rgb, float3(0.2126, 0.7152, 0.0722));

    if(brightness > Threshold)
        return texCol * Color;
    else
        return float4(0.0, 0.0, 0.0, 0.0);
}

float4 BlurVerticalPS(PixelInput p) : SV_TARGET
{
	uint texWidth, texHeight;
	PassTexture.GetDimensions(texWidth, texHeight);

	float2 tex_offset = float2(1.0 / texWidth, 1.0 / texHeight);  // gets size of single texel

    float4 result = PassTexture.Sample(LinearSampler, p.TexCoord.xy) * weight[0]; // current fragment's contribution
    for(int i = 1; i < 5; ++i)
	{
		result += PassTexture.Sample(LinearSampler, p.TexCoord.xy + float2(tex_offset.x * i, 0.0)) * weight[i];
		result += PassTexture.Sample(LinearSampler, p.TexCoord.xy - float2(tex_offset.x * i, 0.0)) * weight[i];
	}

    return result;
}

float4 BlurHorizontalPS(PixelInput p) : SV_TARGET
{
	uint texWidth, texHeight;
	PassTexture.GetDimensions(texWidth, texHeight);

	float2 tex_offset = float2(1.0 / texWidth, 1.0 / texHeight);  // gets size of single texel

    float4 result = PassTexture.Sample(LinearSampler, p.TexCoord.xy) * weight[0]; // current fragment's contribution
	for(int i = 1; i < 5; ++i)
	{
		result += PassTexture.Sample(LinearSampler, p.TexCoord.xy + float2(0.0, tex_offset.y * i)) * weight[i];
		result += PassTexture.Sample(LinearSampler, p.TexCoord.xy - float2(0.0, tex_offset.y * i)) * weight[i];
	}

    return result;
}

float4 BlendPS(PixelInput p) : SV_TARGET
{
	float4 sourceColor = PassTexture.Sample(LinearSampler, p.TexCoord.xy);
	float4 destinationColor = OriginalTexture.Sample(LinearSampler, p.TexCoord.xy);

	float3 blend = sourceColor.rgb * sourceColor.a + destinationColor.rgb * (destinationColor.a - sourceColor.a);
	float3 hdr = blend;
	float3 mapped = float3(1.0, 1.0, 1.0) - exp(-hdr * Exposure);

	return float4(mapped, 1);
}

technique
{
    pass ExtractBrightPixels
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL ExtractPS();
    }

	pass BlurVertical
	{
		VertexShader = compile VS_SHADERMODEL VS();
		PixelShader = compile PS_SHADERMODEL BlurVerticalPS();
	}

	pass BlurHorizontal
	{
		VertexShader = compile VS_SHADERMODEL VS();
		PixelShader = compile PS_SHADERMODEL BlurHorizontalPS();
	}

	pass Blend
	{
		VertexShader = compile VS_SHADERMODEL VS();
		PixelShader = compile PS_SHADERMODEL BlendPS();
	}
}
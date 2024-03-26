#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

Texture2D PassTexture;
Texture2D OriginalTexture;
float BloomDistance;

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
	float4 texCol = PassTexture.Sample(LinearSampler, p.TexCoord.xy);
	float brightness = dot(texCol.rgb, float3(0.2126, 0.7152, 0.0722));

    if(brightness > 0.4)
        return float4(texCol.rgb, 1.0);
    else
        return float4(0.0, 0.0, 0.0, 1.0);
}

float4 BlurVerticalPS(PixelInput p) : SV_TARGET
{
	uint texWidth, texHeight;
	PassTexture.GetDimensions(texWidth, texHeight);

	float2 tex_offset = float2(1.0 / texWidth * BloomDistance, 1.0 / texHeight * BloomDistance);  // gets size of single texel

    float3 result = PassTexture.Sample(LinearSampler, p.TexCoord.xy).rgb * weight[0]; // current fragment's contribution
    for(int i = 1; i < 5; ++i)
	{
		result += PassTexture.Sample(LinearSampler, p.TexCoord.xy + float2(tex_offset.x * i, 0.0)).rgb * weight[i];
		result += PassTexture.Sample(LinearSampler, p.TexCoord.xy - float2(tex_offset.x * i, 0.0)).rgb * weight[i];
	}

    return float4(result, 1.0);
}

float4 BlurHorizontalPS(PixelInput p) : SV_TARGET
{
	uint texWidth, texHeight;
	PassTexture.GetDimensions(texWidth, texHeight);

	float2 tex_offset = float2(1.0 / texWidth * BloomDistance, 1.0 / texHeight * BloomDistance);  // gets size of single texel

    float3 result = PassTexture.Sample(LinearSampler, p.TexCoord.xy).rgb * weight[0]; // current fragment's contribution
	for(int i = 1; i < 5; ++i)
	{
		result += PassTexture.Sample(LinearSampler, p.TexCoord.xy + float2(0.0, tex_offset.y * i)).rgb * weight[i];
		result += PassTexture.Sample(LinearSampler, p.TexCoord.xy - float2(0.0, tex_offset.y * i)).rgb * weight[i];
	}

    return float4(result, 1.0);
}

float4 BlendingPS(PixelInput p) : SV_TARGET
{
	float4 original = OriginalTexture.Sample(LinearSampler, p.TexCoord.xy);
	float4 blurred = PassTexture.Sample(LinearSampler, p.TexCoord.xy);
	float4 result = float4(original.rgb + blurred.rgb, 1.0);

	return result;
}

technique ExtractBlur
{
    pass ExtractBrightPixels
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL ExtractPS();
    }

	pass BlurVerticalPS
	{
		VertexShader = compile VS_SHADERMODEL VS();
		PixelShader = compile PS_SHADERMODEL BlurVerticalPS();
	}

	pass BlurHorizontalPS
	{
		VertexShader = compile VS_SHADERMODEL VS();
		PixelShader = compile PS_SHADERMODEL BlurHorizontalPS();
	}
}

technique Blending
{
	pass 
	{
		VertexShader = compile VS_SHADERMODEL VS();
		PixelShader = compile PS_SHADERMODEL BlendingPS();
	}
}
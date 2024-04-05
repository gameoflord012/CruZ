#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

float MaxLuminance;
float4 Color;
Texture2D Texture;

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

float luminance(float4 v)
{
    return dot(v.rgb, float3(1, 1, 1));
}

float4 change_luminance(float4 c_in, float l_out)
{
    float l_in = luminance(c_in);
    return float4(c_in.rgb * (l_out / l_in), c_in.a);
}

float4 ReinHardPS(PixelInput p) : SV_TARGET
{
    float4 v = Color * Texture.Sample(LinearSampler, p.TexCoord.xy);
    float l_old = luminance(v);
    float numerator = l_old * (1.0f + (l_old / (MaxLuminance * MaxLuminance)));
    float l_new = numerator / (1.0f + l_old);
    return change_luminance(v, l_new);
}

technique
{
    pass Extract
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL ReinHardPS();
    }
}
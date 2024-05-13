#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

Texture2D ScreenTexture;
float MaxLuminance;
float Brightness;

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
    return dot(v.rgb, float3(0.2126, 0.7152, 0.0722));
}

float4 ReinHardPS(PixelInput p) : SV_TARGET
{
    float4 v = ScreenTexture.Sample(LinearSampler, p.TexCoord.xy) * Brightness;
    float l_old = luminance(v);

    if(l_old == 0) return v;

    float numerator = l_old * (1.0 + (l_old / (MaxLuminance * MaxLuminance)));
    float l_new = numerator / (1.0 + l_old);

    return float4(v.rgb / l_old * l_new, 1);
}

technique
{
    pass Extract
    {
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL ReinHardPS();
    }
}
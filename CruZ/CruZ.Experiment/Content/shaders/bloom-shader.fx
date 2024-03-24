#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

Texture2D ScreenTexture;

SamplerState LinearSampler
{
    Texture = <ScreenTexture>;  // Assigning the texture named "ScreenTexture" to the sampler state

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

PixelInput SpriteVertexShader(VertexInput v)
{
    PixelInput output;

    output.Position = v.Position;
    output.TexCoord = v.TexCoord;
    return output;
}

float4 SpritePixelShader(PixelInput p) : SV_TARGET
{
    return ScreenTexture.Sample(LinearSampler, p.TexCoord.xy);
}

technique SpriteBatch
{
    pass
    {
        VertexShader = compile VS_SHADERMODEL SpriteVertexShader();
        PixelShader = compile PS_SHADERMODEL SpritePixelShader();
    }
}

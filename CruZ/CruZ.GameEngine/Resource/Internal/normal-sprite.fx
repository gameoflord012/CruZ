#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

float4x4 view_projection;
float4 hdrColor;
Texture2D ScreenTexture : register(t0);

SamplerState LinearSampler
{
    Texture = <ScreenTexture>;

    MagFilter = LINEAR;
    MinFilter = LINEAR;
    Mipfilter = LINEAR; 

    AddressU = CLAMP;
    AddressV = CLAMP;
};

struct VertexInput {
    float4 Position : POSITION0;
    float4 TexCoord : TEXCOORD0;
};
struct PixelInput {
    float4 Position : SV_Position0;
    float4 TexCoord : TEXCOORD0;
};

PixelInput SpriteVertexShader(VertexInput v) {
    PixelInput output;

    output.Position = mul(v.Position, view_projection);
    output.TexCoord = v.TexCoord;
    return output;
}

float4 SpritePixelShader(PixelInput p) : SV_TARGET {
    float4 diffuse = ScreenTexture.Sample(LinearSampler, p.TexCoord.xy);
    diffuse *= hdrColor;
    return diffuse;
}

technique {
    pass {
        VertexShader = compile VS_SHADERMODEL SpriteVertexShader();
        PixelShader = compile PS_SHADERMODEL SpritePixelShader();
    }
}
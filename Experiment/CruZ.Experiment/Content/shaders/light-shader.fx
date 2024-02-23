#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

float4x4 view_projection;

sampler TextureSampler : register(s0);

cbuffer LightBuffer : register(b0)
{
    float2 LightPosition;
    float LightRadius;
};

struct VertexInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float4 TexCoord : TEXCOORD0;
};

struct PixelInput
{
    float4 Position : SV_Position0;
    float4 Color : COLOR0;
    float4 TexCoord : TEXCOORD0;
};

PixelInput SpriteVertexShader(VertexInput v)
{
    PixelInput output;

    output.Position = mul(v.Position, view_projection);
    output.Color = v.Color;
    output.TexCoord = v.TexCoord;
    return output;
}

float4 SpritePixelShader(PixelInput p) : SV_TARGET
{
    float4 lightPosition = mul(float4(LightPosition, 0, 1), view_projection);
    float distanceToLight = length(p.TexCoord.xy - LightPosition);
    
    float attenuation = (1 - pow(distanceToLight, 1) / LightRadius);
    float3 finalColor = saturate(p.Color.xyz * attenuation);
    
    float4 diffuse = tex2D(TextureSampler, p.TexCoord.xy);
    
    if(distanceToLight < LightRadius)
    {
        diffuse.xyz += finalColor;
        return diffuse;
    }
    
    return float4(0, 1, 0, 1);
}

technique SpriteBatch
{
    pass
    {
        VertexShader = compile VS_SHADERMODEL SpriteVertexShader();
        PixelShader = compile PS_SHADERMODEL SpritePixelShader();
    }
}

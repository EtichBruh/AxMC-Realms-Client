#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
float2 uvPix : VPOS;
float4 xOutlineColour = float4(0, 0, 0, 255);

sampler2D InputSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 UV : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    
    float4 currentPixel = tex2D(InputSampler, input.UV) * input.Color;
    //float2 UVP = float2(ddx_fine(input.UV.x) / ddx(input.UV.x), ddy_fine(input.UV.y) / ddy(input.UV.y));
    /*if ((currentPixel.a == 0.0f) &&
            tex2D(InputSampler, input.UV - UVP).a +
            tex2D(InputSampler, float2(input.UV.x - UVP.x, input.UV.y + UVP.y)).a +
            tex2D(InputSampler, float2(input.UV.x + UVP.x, input.UV.y - UVP.y)).a +
            tex2D(InputSampler, input.UV + UVP).a != 0
            )
    {
            currentPixel = xOutlineColour;
    }*/
    return currentPixel;
}

technique SpriteOutline
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
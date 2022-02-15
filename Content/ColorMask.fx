#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

//float2 uvPix : VPOS;
//float4 xOutlineColour = float4(0, 0, 0, 255);

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
   // float RedColorBackUp = tex2D(InputSampler, input.UV).r;
    if (currentPixel.a != 0 && currentPixel.r == currentPixel.g && currentPixel.r == currentPixel.b)
    {
        
        currentPixel.r = ddx(input.UV).x;
        currentPixel.g = ddy(input.UV).y;
    }

    //if(currentPixel.)
    return currentPixel;
}

technique SpriteOutline
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
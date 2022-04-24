#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
float t;

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
/*float spiral(float2 m)
{
    float r = length(m);
    float a = atan2(m.y, m.x);
    float v = sin(100. * (sqrt(r) - 0.02 * a - .3 * t));
    return clamp(v, 0., 1.);
}*//*
float3 rain(float2 fragCoord)
{
    fragCoord.x *= 800;
    fragCoord.x = floor(fragCoord.x *.0625); // This is the exact replica of the calculation in text function for getting the cell ids. Here we want the id for the columns 
                         
    float offset = sin(fragCoord.x * 15.); // Each drop of rain needs to start at a different point. The column id  plus a sin is used to generate a different offset for each columm
    float speed = cos(fragCoord.x * 3.) * .15 + .35; // Same as above, but for speed. Since we dont want the columns travelling up, we are adding the 0.7. Since the cos *0.3 goes between -0.3 and 0.3 the 0.7 ensures that the speed goes between 0.4 mad 1.0. This is also control parameters for min and max speed
    float y = frac(-fragCoord.y // This maps the screen again so that top is 1 and button is 0. The addition with time and frac would cause an entire bar moving from button to top
                                             + t* speed + offset); // the speed and offset would cause the columns to move down at different speeds. Which causes the rain drop effect
                         
    return float3(.1, 1., .35) / (y * 20.); // adjusting the retun color based on the columns calculations. 
}
float cosRange(float degrees, float range, float minimum)
{
    return (((1.0 + cos(degrees * 0.017453292519943295)) * 0.5) * range) + minimum;
}*/
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 currentPixel = tex2D(InputSampler, input.UV);
    
    if (currentPixel.r == currentPixel.g && currentPixel.r == currentPixel.b)
    {
        /*float2 p = (2.0 * input.UV.xy);
        float ct = cosRange(t * 5.0, 3.0, 1.1);
        float xBoost = cosRange(t * 0.2, 5.0, 5.0);
        float yBoost = cosRange(t * 0.1, 10.0, 5.0);
        float fScale = cosRange(t * 15.5, 1.25, 0.5);
	
        for (int i = 1; i < 100; i++)
        { 
            p.x += 0.25 / i * sin(i * p.y + t * cos(ct) * 0.5 * 0.05 + 0.005 * i) * fScale + xBoost;
            p.y += 0.25 / i * sin(i * p.x + t * ct * 0.3 * 0.025 + 0.03 * float(i + 15)) * fScale + yBoost;
        }
        currentPixel *= float4(0.5 * sin(3.0 * p.x) + 0.5, 0.5 * sin(3.0 * p.y) + 0.5, sin(p.x + p.y), 1);*/ //INSANE RAINBOW SHADER

            float3 o = -3.1416 * float3(.0, .5, 1.);

        float g = input.UV.x + input.UV.y + t;
            float3 col = 0.5 + .5 * -sin(g) * cos(g + o);
            col.g += .25;
            col = .5 + (col * 2. - 1.);
            col = .175 + .75 * col;
        currentPixel *= float4(col, 1);
    }
    return currentPixel;
}

technique SpriteOutline
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
// ParticlePhysics2D.fx (MGFXGL-compatible)

sampler2D particleDataTex;
float2 screenSize;
float dt;
float2 gravity;

struct PSInput
{
    float4 Position : POSITION;
    float2 TexCoord : TEXCOORD0;
};

float4 MainPS(PSInput input) : COLOR0
{
    float4 data = tex2D(particleDataTex, input.TexCoord);
    float2 pos = data.xy;
    float type = data.z;

    float2 vel = float2(0,0);
    vel += gravity * dt;
    pos += vel * dt;

    pos = clamp(pos, float2(0,0), screenSize);

    float4 color = float4(1,1,1,1);
    if (type == 0)      color = float4(0,0,0,0);
    else if (type == 1) color = float4(1,1,0,1);
    else if (type == 2) color = float4(0,0.5,1,1);
    else if (type == 3) color = float4(0.5,0.5,0.5,1);

    return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 MainPS();
    }
}

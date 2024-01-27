#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

int changeR;
int changeG;
int changeB;


sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	// return tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
    float4 col = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
	
    if (col.r != 0 && col.g != 0 && col.b != 0)
    {
        if (col.r == 153 && col.g == 78 && col.b == 65)
        {
            col.r = 203;
            col.g = 158;
            col.b = 65;
        }
        
        // FUNKAR INTE VARFÖR??!!
        // breakpoints nås aldrig
        else if (col.r == 117 && col.g == 60 && col.b == 50)
        {
            // && col.g == 60 && col.b == 50
            col.r = 167;
            col.g = 120;
            col.b = 50;
        }
        else
        {
            col.r *= 1;
            col.g *= 0.8682;
            col.b *= 1.5057;
        }
        
    }
    
	
    return col;

}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
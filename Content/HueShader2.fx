#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

float rChange;
float gChange;
float bChange;



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
        //p2s handskar
        if (col.r == 0.6)
        {
            //153, 78, 65
            col.r = 0.796;
            col.g = 0.62;
            col.b = 0.255;
            //203, 158, 65
        }               
        else if (col.r >= 0.45 && col.r <= 0.46)
        {   
            //117, 60, 50            
            col.r = .65;
            col.g = .47;
            col.b = .19;
            //167, 120, 50
        }   
        
        
        else
        {
            col.r *= rChange;
            col.g *= gChange;
            col.b *= bChange;

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
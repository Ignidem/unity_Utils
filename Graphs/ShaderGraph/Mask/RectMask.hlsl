void ShaderRectMask_float(float2 p, float2 o, float2 s, out float m)
{
	float x = clamp(p.x - o.x, 0, 1) * clamp(o.x + s.x - p.x, 0, 1);
	float y = clamp(p.y - o.y, 0, 1) * clamp(o.y + s.y - p.y, 0, 1);
	m = ceil(x * y);
}

void GetMaskColor_float(float3 background, float4 color, float mask, out float3 value) {
	mask = mask * color[3];
	float3 foreground = color;
	value = lerp(background, foreground, mask);
}
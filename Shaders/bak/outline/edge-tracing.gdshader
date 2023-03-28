shader_type canvas_item;

//uniform sampler2D tex;
uniform float width : hint_range(0, 10) = 1.0;
uniform float intensity;
uniform vec4 color : source_color = vec4(1.0);
uniform float speed;

float sample(in sampler2D tex, in vec2 st){
	return texture(tex, st).a;
}

void fragment(){
	float t = TIME*speed;
	vec2 px = vec2(1.)/vec2(textureSize(TEXTURE, 0));

	vec2 uv = UV;
	
	vec2 texture_pixel_size = vec2(1.0) / (vec2(1.0) / TEXTURE_PIXEL_SIZE + vec2(width * 2.0));
		
	uv = (uv - texture_pixel_size * width) * TEXTURE_PIXEL_SIZE / texture_pixel_size;
	
	float neighbor = 
		sample(TEXTURE, uv+px*vec2( 1.,  0.))+
		sample(TEXTURE, uv+px*vec2( 0.,  1.))+
		sample(TEXTURE, uv+px*vec2(-1.,  0.))+
		sample(TEXTURE, uv+px*vec2( 0., -1.));
	
	float center = sample(TEXTURE, uv)*4.;
	
	float edge = clamp(neighbor-center, 0., 1.);
	float lit = dot(UV-vec2(.5), vec2(cos(t), sin(t)))-t;
	lit = mod(lit, 1.);
	lit = pow(lit, 12.);
	
	//COLOR = vec4(color.rgb*vec3(intensity), edge*lit);
	COLOR = vec4(color.rgb*vec3(intensity), edge);
}

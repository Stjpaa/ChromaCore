shader_type canvas_item;

uniform vec4 color : source_color = vec4(0, 1, 0, 1);
uniform vec4 background_color : source_color = vec4(0.3, 0.3, 0.3, 1.0);
uniform float strength : hint_range(0.0, 50.0) = 20.0;
uniform float scale : hint_range(0.0, 20.0) = 2.5;
uniform float speed : hint_range(0.0, 10.0) = 0.5;
uniform float border : hint_range(0.0, 1.0) = 0.4;

vec2 Unity_Twirl_float(vec2 my_UV, vec2 Center, float Strength, vec2 Offset){
    vec2 delta = my_UV - Center;
    float angle = Strength * length(delta);
    float x = cos(angle) * delta.x - sin(angle) * delta.y;
    float y = sin(angle) * delta.x + cos(angle) * delta.y;
    return vec2(x + Center.x + Offset.x, y + Center.y + Offset.y);
}

vec2 unity_voronoi_noise_randomVector (vec2 my_UV, float offset){
    mat2 m = mat2(vec2(15.27, 47.63), vec2(99.41, 89.98));
    my_UV = fract(sin(my_UV * m) * 46839.32);
    return vec2(sin(my_UV.y*+offset)*0.5+0.5, cos(my_UV.x*offset)*0.5+0.5);
}

vec2 Unity_Voronoi_float(vec2 my_UV, float AngleOffset, float CellDensity){
	float Out; 
	float Cells;
    vec2 g = floor(my_UV * CellDensity);
    vec2 f = fract(my_UV * CellDensity);
    float t = 8.0;
    vec3 res = vec3(8.0, 0.0, 0.0);

    for(int y=-1; y<=1; y++){
        for(int x=-1; x<=1; x++){
            vec2 lattice = vec2(float(x),float(y));
            vec2 offset = unity_voronoi_noise_randomVector(lattice + g, AngleOffset);
            float d = distance(lattice + offset, f);
            if(d < res.x){
                res = vec3(d, offset.x, offset.y);
                Out = res.x;
                Cells = res.y;
            }
        }
    }
	return vec2(Out, Cells);
}

void fragment(){
	vec2 new_position = Unity_Twirl_float(UV, vec2(0.5, 0.5), strength, vec2(TIME * speed));
	float voronoi = Unity_Voronoi_float(new_position, 1.0, scale).x;
	float border_distance = distance(UV, vec2(0.5, 0.5));
    vec4 background = vec4(
		background_color.xyz,
		background_color.a * (pow(border_distance * 2.0, 8.0) * -1.0 + 1.0)
	);
    vec4 portal = vec4(color.xyz, voronoi * border - distance(UV, vec2(0.5, 0.5)));
	COLOR = mix(
		background,
		portal,
		portal.a
	);
}
shader_type canvas_item;
render_mode blend_mix;

uniform float pixels : hint_range(10,100);
uniform float rotation : hint_range(0.0, 6.28) = 0.0;
uniform vec2 light_origin = vec2(0.39, 0.39);
uniform float light_distance1 = 0.5;
uniform float light_distance2 = 0.75;
uniform float time_speed : hint_range(0.0, 1.0) = 0.2;
uniform float dither_size : hint_range(0.0, 10.0) = 2.0;
uniform sampler2D colors;
uniform float size = 50.0;
uniform int OCTAVES : hint_range(0, 20, 1);
uniform float seed: hint_range(1, 10);
uniform float time = 0.0;
uniform bool should_dither = true;

float rand(vec2 coord) {
	// land has to be tiled
	// tiling only works for integer values, thus the rounding
	// it would probably be better to only allow integer sizes
	// multiply by vec2(2,1) to simulate planet having another side
	coord = mod(coord, vec2(2.0,1.0)*round(size));
	return fract(sin(dot(coord.xy ,vec2(12.9898,78.233))) * 43758.5453 * seed);
}

float noise(vec2 coord){
	vec2 i = floor(coord);
	vec2 f = fract(coord);
	
	float a = rand(i);
	float b = rand(i + vec2(1.0, 0.0));
	float c = rand(i + vec2(0.0, 1.0));
	float d = rand(i + vec2(1.0, 1.0));

	vec2 cubic = f * f * (3.0 - 2.0 * f);

	return mix(a, b, cubic.x) + (c - a) * cubic.y * (1.0 - cubic.x) + (d - b) * cubic.x * cubic.y;
}

float fbm(vec2 coord){
	float value = 0.0;
	float scale = 0.5;

	for(int i = 0; i < OCTAVES ; i++){
		value += noise(coord) * scale;
		coord *= 2.0;
		scale *= 0.5;
	}
	return value;
}

bool dither(vec2 uv1, vec2 uv2) {
	return mod(uv1.x+uv2.y,2.0/pixels) <= 1.0 / pixels;
}

vec2 rotate(vec2 coord, float angle){
	coord -= 0.5;
	coord *= mat2(vec2(cos(angle),-sin(angle)),vec2(sin(angle),cos(angle)));
	return coord + 0.5;
}

vec2 spherify(vec2 uv) {
	vec2 centered= uv *2.0-1.0;
	float z = sqrt(1.0 - dot(centered.xy, centered.xy));
	vec2 sphere = centered/(z + 1.0);
	return sphere * 0.5+0.5;
}


void fragment() {
	//pixelize uv
	vec2 uv = floor(UV*pixels)/pixels;
	bool dith = dither(uv, UV);
	
	// cut out a circle
	float d_circle = distance(uv, vec2(0.5));
	float a = step(d_circle, 0.49999);
	
	uv = spherify(uv);
	
	// check distance distance to light
	float d_light = distance(uv , vec2(light_origin));
	
	uv = rotate(uv, rotation);
	
	// noise
	float f = fbm(uv*size+vec2(time*time_speed * 0.2, 0.0));
	
	// remap light
	d_light = smoothstep(-0.3, 1.2, d_light);
	
	if (d_light < light_distance1) {
		d_light *= 0.9;
	}
	if (d_light < light_distance2) {
		d_light *= 0.9;
	}
	
	
	float c = d_light*pow(f,0.8)*3.5; // change the magic nums here for different light strengths
	
	// apply dithering
	if (dith || !should_dither) {
		c += 0.02;
		c *= 1.05;
	}
	
	// now we can assign colors based on distance to light origin
	float posterize = floor(c*4.0)/4.0;
	vec4 col = texture(colors, vec2(posterize, 0.0));
	
	COLOR = vec4(col.rgb, a * col.a);
}

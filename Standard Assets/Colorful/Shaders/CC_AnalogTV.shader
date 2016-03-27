Shader "Hidden/CC_AnalogTV"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}

		_phase ("Phase (time)", Float) = 0.01
		_grayscale ("Grayscale", Float) = 0.0
		_noiseIntensity ("Static noise intensity", Float) = 0.5
		_scanlinesIntensity ("Scanlines intensity", Float) = 2.0
		_scanlinesCount ("Scanlines count", Float) = 1024

		_distortion ("Distortion", Float) = 0.2
		_cubicDistortion ("Cubic Distortion", Float) = 0.6
		_scale ("Scale (Zoom)", Float) = 0.8
	}

	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest 
				#include "UnityCG.cginc"
				#include "Colorful.cginc"

				sampler2D _MainTex;

				float _phase;
				float _grayscale;
				float _noiseIntensity;
				float _scanlinesIntensity;
				float _scanlinesCount;

				float _distortion;
				float _cubicDistortion;
				float _scale;

				float2 barrelDistortion(float2 coord) 
				{
					// Inspired by SynthEyes lens distortion algorithm
					// See http://www.ssontech.com/content/lensalg.htm

					float r2 = (coord.x - 0.5) * (coord.x - 0.5) + (coord.y - 0.5) * (coord.y - 0.5);
					float f = 0.0;

					if (_cubicDistortion == 0.0)	f = 1.0 + r2 * _distortion;
					else							f = 1.0 + r2 * (_distortion + _cubicDistortion * sqrt(r2));

					return f * _scale * (coord.xy - 0.5) + 0.5;
				}

				fixed4 frag(v2f_img i):COLOR
				{
					float2 coord = barrelDistortion(i.uv);
					fixed4 color = tex2D(_MainTex, coord);

					float n = simpleNoise(coord.x, coord.y, 1234, _phase);
					float dx = fmod(n, 0.01);

					float3 result = color.rgb + color.rgb * clamp(0.1 + dx * 100.0, 0.0, 1.0);
					float2 sc = float2(sin(coord.y * _scanlinesCount), cos(coord.y * _scanlinesCount));
					result += color.rgb * float3(sc.x, sc.y, sc.x) * _scanlinesIntensity;
					result = color.rgb + clamp(_noiseIntensity, 0.0, 1.0) * (result - color.rgb);

					if(_grayscale != 0)
						result = fixed3(luminance(result));

					return fixed4(result, color.a);
				}

			ENDCG
		}
	}

	FallBack off
}

Shader "Hidden/CC_Led"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_scale ("Scale", Float) = 80.0
		_brightness ("Brightness", Float) = 1.0
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
				float _scale;
				fixed _brightness;

				fixed4 frag(v2f_img i):COLOR
				{
					fixed4 color = pixelate(_MainTex, i.uv, _scale) * _brightness;
					fixed2 coord = i.uv * _scale;
					fixed mvx = abs(sin(coord.s * 3.1415)) * 1.5;
					fixed mvy = abs(sin(coord.t * 3.1415)) * 1.5;

					if (mvx * mvy < 1.0)
						color *= (mvx * mvy);

					return color;
				}

			ENDCG
		}
	}

	FallBack off
}

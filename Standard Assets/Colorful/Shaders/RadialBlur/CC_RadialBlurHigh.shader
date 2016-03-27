Shader "Hidden/CC_RadialBlurHigh"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_amount ("Amount", Range(0.0, 1.0)) = 0.1
		_center ("Center Point", Vector) = (0.5, 0.5, 0.0, 0.0)
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
				#pragma target 3.0
				#include "UnityCG.cginc"
				#include "../Colorful.cginc"

				sampler2D _MainTex;
				float amount;
				float2 center;

				fixed4 frag(v2f_img i):COLOR
				{
					float2 coord = i.uv - center;
					float4 color = float4(0.0);
					float scale;

					for (int i = 0; i < 15; i++)
					{
						scale = 1.0 + amount * (i / 14.0);
						color += tex2D(_MainTex, coord * scale + center);
					}

					color /= 15.0;
					return fixed4(color);
				}

			ENDCG
		}
	}

	FallBack off
}

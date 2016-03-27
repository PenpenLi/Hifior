Shader "Hidden/CC_DoubleVision"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_displace ("Displace", Vector) = (0.7, 0.0, 0.0, 0.0)
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
				fixed2 _displace;

				fixed4 frag(v2f_img i):COLOR
				{
					fixed4 c = tex2D(_MainTex, i.uv);

					c += tex2D(_MainTex, i.uv + half2(_displace.x * 16, _displace.y * 16)) * 0.6;
					c += tex2D(_MainTex, i.uv + half2(_displace.x * 24, _displace.y * 24)) * 0.4;
					c += tex2D(_MainTex, i.uv + half2(_displace.x * 32, _displace.y * 32)) * 0.2;

					c /= 2.0f;

					return fixed4(c);
				}

			ENDCG
		}
	}

	FallBack off
}

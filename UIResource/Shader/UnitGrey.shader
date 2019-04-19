Shader "UI/UnitGrey"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_LuminosityAmount("GrayScale Amount", Range(0.0,1.0)) = 1.0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}
		Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord  : TEXCOORD0;
			};//

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif
				return OUT;
			}

			sampler2D _MainTex; fixed _LuminosityAmount;

			fixed4 frag(v2f i) : SV_Target
			{
			 fixed4 renderTex = tex2D(_MainTex, i.texcoord) * i.color;				
			renderTex.rgb *= renderTex.a;
			float luminosity = 0.299 * renderTex.r + 0.587 * renderTex.g + 0.114 * renderTex.b;
			fixed4 col = lerp(renderTex, luminosity, _LuminosityAmount);
			return fixed4(col.r,col.g,col.b,renderTex.a);
		}
		ENDCG
	}
	}
}

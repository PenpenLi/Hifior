Shader "Shader Forge/MapTile" {
    Properties {
        _Color ("Color", Color) = (0.1972318,0.7058823,0.558549,0.6)
        _Texture ("Texture", 2D) = "black" {}
        _ShinessSpeed ("ShinessSpeed", Float ) = 2
        _Shiness ("Shiness", Float ) = 0.4
        _OutlineColor ("OutlineColor", Color) = (0.5,0.5,0,0.5)
        _OutlineWidth ("OutlineWidth", Range(0, 0.5)) = 0.025
        _OutlineOpacity ("OutlineOpacity", Range(0, 1)) = 0.680289
        _RotationDegrees ("RotationDegrees", Range(0, 10)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _ShinessSpeed;
            uniform float _Shiness;
            uniform float4 _OutlineColor;
            uniform float _OutlineWidth;
            uniform float _OutlineOpacity;
            uniform float _RotationDegrees;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
				float2 node_3158 = i.uv0 - 0.5;
                float s = sin ( _RotationDegrees);
                float c = cos ( _RotationDegrees);
           
                float2x2 rotationMatrix = float2x2( c, -s, s, c);
                rotationMatrix *=0.5;
                rotationMatrix +=0.5;
                rotationMatrix = rotationMatrix * 2-1;

				node_3158 =mul(node_3158,rotationMatrix);
                node_3158+=0.5;
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(node_3158, _Texture));
                float4 node_4189 = _Time + _TimeEditor;
                float node_4743 = (1.0 - _OutlineWidth);
                float3 emissive = lerp(((_Color.rgb*(1.0 - _Texture_var.a))+saturate(( (_Texture_var.rgb*_Texture_var.a) > 0.5 ? (1.0-(1.0-2.0*((_Texture_var.rgb*_Texture_var.a)-0.5))*(1.0-(_Shiness*abs(sin((_ShinessSpeed*node_4189.g)))))) : (2.0*(_Texture_var.rgb*_Texture_var.a)*(_Shiness*abs(sin((_ShinessSpeed*node_4189.g))))) ))),_OutlineColor.rgb,(_OutlineOpacity*(1.0 - (step(_OutlineWidth,i.uv0.r)*step(_OutlineWidth,i.uv0.g)*step(i.uv0.r,node_4743)*step(i.uv0.g,node_4743)))));
                float3 finalColor = emissive;
                return fixed4(finalColor,max(_Color.a,_Texture_var.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

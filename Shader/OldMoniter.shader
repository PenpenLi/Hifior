// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:1,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:1,hqsc:True,nrmq:1,nrsp:0,vomd:1,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:6,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:1,qpre:4,rntp:5,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:2865,x:32358,y:33004,varname:node_2865,prsc:2|emission-4512-OUT,voffset-9173-OUT;n:type:ShaderForge.SFN_Color,id:4354,x:31726,y:32929,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_4354,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.427451,c2:0.6784314,c3:0.5411765,c4:1;n:type:ShaderForge.SFN_Tex2d,id:727,x:31726,y:33085,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_727,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4512,x:32007,y:33030,varname:node_4512,prsc:2|A-4354-RGB,B-727-RGB;n:type:ShaderForge.SFN_TexCoord,id:6221,x:31652,y:33275,varname:node_6221,prsc:2,uv:0;n:type:ShaderForge.SFN_RemapRange,id:1259,x:31823,y:33275,varname:node_1259,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-6221-UVOUT;n:type:ShaderForge.SFN_ProjectionParameters,id:8327,x:31652,y:33499,varname:node_8327,prsc:2;n:type:ShaderForge.SFN_Append,id:4020,x:31823,y:33453,varname:node_4020,prsc:2|A-5569-OUT,B-8327-SGN;n:type:ShaderForge.SFN_Vector1,id:5569,x:31652,y:33415,varname:node_5569,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:3508,x:32013,y:33363,varname:node_3508,prsc:2|A-1259-OUT,B-4020-OUT;n:type:ShaderForge.SFN_Multiply,id:9173,x:32198,y:33484,varname:node_9173,prsc:2|A-3508-OUT,B-2031-OUT;n:type:ShaderForge.SFN_Vector2,id:2031,x:32003,y:33509,varname:node_2031,prsc:2,v1:1,v2:-1;proporder:4354-727;pass:END;sub:END;*/

Shader "Shader Forge/OldMoniter" {
    Properties {
        _Color ("Color", Color) = (0.427451,0.6784314,0.5411765,1)
        _MainTex ("MainTex", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Overlay+1"
            "RenderType"="Overlay"
        }
		Pass {
			Name "FORWARD"
			Tags {
				"LightMode" = "ForwardBase"
			}
			Cull Off
			ZTest Always
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#define UNITY_PASS_FORWARDBASE
			#define _GLOSSYENV 1
			#include "UnityCG.cginc"
			#include "UnityPBSLighting.cginc"
			#include "UnityStandardBRDF.cginc"
			#pragma multi_compile_fwdbase
			#pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
			#pragma target 3.0
			uniform float4 _Color;
			uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
			struct VertexInput {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord0 : TEXCOORD0;
			};
			struct VertexOutput {
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
			};
			VertexOutput vert(VertexInput v) {
				VertexOutput o = (VertexOutput)0;
				o.uv0 = v.texcoord0;
				o.normalDir = UnityObjectToWorldNormal(v.normal);
				v.vertex.xyz = float3((((o.uv0*2.0 + -1.0)*float2(1.0,_ProjectionParams.r))*float2(1,-1)),0.0);
				o.posWorld = mul(_Object2World, v.vertex);
				o.pos = v.vertex;
				return o;
			}
			float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
				float isFrontFace = (facing >= 0 ? 1 : 0);
				float faceSign = (facing >= 0 ? 1 : -1);
				i.normalDir = normalize(i.normalDir);
				i.normalDir *= faceSign;
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				float3 normalDirection = i.normalDir;
				float3 viewReflectDirection = reflect(-viewDirection, normalDirection);
				////// Lighting:
				////// Emissive:
								float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
								float3 emissive = (_Color.rgb*_MainTex_var.rgb);
								float3 finalColor = emissive;
								return fixed4(finalColor,1);
							}
							ENDCG
								}
    }
    CustomEditor "ShaderForgeMaterialInspector"
}

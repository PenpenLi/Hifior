// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33791,y:32229,varname:node_3138,prsc:2|emission-1280-OUT;n:type:ShaderForge.SFN_TexCoord,id:658,x:32447,y:32308,varname:node_658,prsc:2,uv:0;n:type:ShaderForge.SFN_RemapRange,id:4239,x:32722,y:32309,varname:node_4239,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-658-UVOUT;n:type:ShaderForge.SFN_ComponentMask,id:3501,x:32940,y:32309,varname:node_3501,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-4239-OUT;n:type:ShaderForge.SFN_ArcTan2,id:8064,x:33221,y:32288,varname:node_8064,prsc:2,attp:2|A-3501-G,B-3501-R;n:type:ShaderForge.SFN_Slider,id:4958,x:32657,y:32076,ptovrint:False,ptlb:node_4958,ptin:_node_4958,varname:node_4958,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8461539,max:1;n:type:ShaderForge.SFN_Step,id:1280,x:33433,y:32205,varname:node_1280,prsc:2|A-8064-OUT,B-4958-OUT;proporder:4958;pass:END;sub:END;*/

Shader "Shader Forge/Unlit" {
    Properties {
        _node_4958 ("node_4958", Range(0, 1)) = 0.8461539
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float _node_4958;
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
                float2 node_4239 = (i.uv0*2.0+-1.0);
                float2 node_3501 = node_4239.rg;
                float node_1280 = step(((atan2(node_3501.g,node_3501.r)/6.28318530718)+0.5),_node_4958);
                float3 emissive = float3(node_1280,node_1280,node_1280);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

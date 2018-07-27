// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32746,y:32693,varname:node_3138,prsc:2|emission-7765-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:31791,y:32623,ptovrint:False,ptlb:Color2,ptin:_Color2,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:1,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:8321,x:31791,y:32457,ptovrint:False,ptlb:Color1,ptin:_Color1,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Lerp,id:8638,x:32273,y:32726,varname:node_8638,prsc:2|A-8321-RGB,B-7241-RGB,T-1603-OUT;n:type:ShaderForge.SFN_Multiply,id:7765,x:32582,y:32791,varname:node_7765,prsc:2|A-8638-OUT,B-3987-OUT;n:type:ShaderForge.SFN_TexCoord,id:1264,x:30984,y:32856,varname:node_1264,prsc:2,uv:0;n:type:ShaderForge.SFN_RemapRange,id:8786,x:31160,y:32856,varname:node_8786,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-1264-UVOUT;n:type:ShaderForge.SFN_ArcTan2,id:3921,x:31529,y:32873,varname:node_3921,prsc:2,attp:2|A-2274-G,B-2274-R;n:type:ShaderForge.SFN_ComponentMask,id:2274,x:31357,y:32856,varname:node_2274,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-8786-OUT;n:type:ShaderForge.SFN_Length,id:3920,x:31401,y:33137,varname:node_3920,prsc:2|IN-8786-OUT;n:type:ShaderForge.SFN_Floor,id:8570,x:31744,y:33210,varname:node_8570,prsc:2|IN-3920-OUT;n:type:ShaderForge.SFN_Slider,id:9627,x:31357,y:33070,ptovrint:False,ptlb:Thickness,ptin:_Thickness,varname:node_9627,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2905983,max:1;n:type:ShaderForge.SFN_Add,id:9669,x:31744,y:33087,varname:node_9669,prsc:2|A-9627-OUT,B-3920-OUT;n:type:ShaderForge.SFN_Floor,id:3966,x:31948,y:33072,varname:node_3966,prsc:2|IN-9669-OUT;n:type:ShaderForge.SFN_OneMinus,id:1943,x:31948,y:33210,varname:node_1943,prsc:2|IN-8570-OUT;n:type:ShaderForge.SFN_Multiply,id:3987,x:32404,y:33067,varname:node_3987,prsc:2|A-3966-OUT,B-1943-OUT,C-6839-OUT;n:type:ShaderForge.SFN_Slider,id:1603,x:31536,y:32773,ptovrint:False,ptlb:Percent,ptin:_Percent,varname:node_1603,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7692308,max:1;n:type:ShaderForge.SFN_OneMinus,id:4660,x:31693,y:32873,varname:node_4660,prsc:2|IN-3921-OUT;n:type:ShaderForge.SFN_Subtract,id:1250,x:31884,y:32902,varname:node_1250,prsc:2|A-4660-OUT,B-1603-OUT;n:type:ShaderForge.SFN_Ceil,id:5924,x:32034,y:32929,varname:node_5924,prsc:2|IN-1250-OUT;n:type:ShaderForge.SFN_OneMinus,id:6839,x:32220,y:32929,varname:node_6839,prsc:2|IN-5924-OUT;proporder:7241-8321-9627-1603;pass:END;sub:END;*/

Shader "Shader Forge/RadicalHPBar" {
    Properties {
        _Color2 ("Color2", Color) = (0.07843138,1,0,1)
        _Color1 ("Color1", Color) = (1,0,0,1)
        _Thickness ("Thickness", Range(0, 1)) = 0.2905983
        _Percent ("Percent", Range(0, 1)) = 0.7692308
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
            #pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _Color2;
            uniform float4 _Color1;
            uniform float _Thickness;
            uniform float _Percent;
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
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float2 node_8786 = (i.uv0*2.0+-1.0);
                float node_3920 = length(node_8786);
                float2 node_2274 = node_8786.rg;
                float node_4660 = (1.0 - ((atan2(node_2274.g,node_2274.r)/6.28318530718)+0.5));
                float3 emissive = (lerp(_Color1.rgb,_Color2.rgb,_Percent)*(floor((_Thickness+node_3920))*(1.0 - floor(node_3920))*(1.0 - ceil((node_4660-_Percent)))));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

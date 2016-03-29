// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:1,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:1,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:1,hqsc:True,nrmq:1,nrsp:0,vomd:1,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:6,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:1,qpre:4,rntp:5,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:2865,x:32354,y:32895,varname:node_2865,prsc:2|emission-3749-OUT,voffset-4177-OUT;n:type:ShaderForge.SFN_TexCoord,id:6793,x:31727,y:33365,varname:node_6793,prsc:2,uv:0;n:type:ShaderForge.SFN_ProjectionParameters,id:8707,x:31713,y:33578,varname:node_8707,prsc:2;n:type:ShaderForge.SFN_RemapRange,id:9496,x:31912,y:33365,varname:node_9496,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-6793-UVOUT;n:type:ShaderForge.SFN_Append,id:6538,x:31912,y:33537,varname:node_6538,prsc:2|A-5896-OUT,B-8707-SGN;n:type:ShaderForge.SFN_Vector1,id:5896,x:31713,y:33519,varname:node_5896,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:4177,x:32111,y:33435,varname:node_4177,prsc:2|A-9496-OUT,B-6538-OUT;n:type:ShaderForge.SFN_Tex2d,id:8994,x:31411,y:32812,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_8994,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:76a1572c73453431aacb9f1f816e1fed,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:4104,x:31542,y:32939,ptovrint:False,ptlb:MainTex2,ptin:_MainTex2,varname:node_4104,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0c4c3240630b08d41a56a5142684b7af,ntxv:0,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:6948,x:30211,y:33385,varname:node_6948,prsc:2,uv:0;n:type:ShaderForge.SFN_Lerp,id:3749,x:32029,y:32987,varname:node_3749,prsc:2|A-8994-RGB,B-4104-RGB,T-7833-OUT;n:type:ShaderForge.SFN_Add,id:3796,x:30762,y:33403,varname:node_3796,prsc:2|A-7445-OUT,B-8434-OUT;n:type:ShaderForge.SFN_Floor,id:1454,x:31078,y:33392,varname:node_1454,prsc:2|IN-3796-OUT;n:type:ShaderForge.SFN_Slider,id:8434,x:30352,y:33550,ptovrint:False,ptlb:_Blend,ptin:_Blend,varname:node_8434,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4526221,max:1;n:type:ShaderForge.SFN_Subtract,id:6471,x:30803,y:33238,varname:node_6471,prsc:2|A-7445-OUT,B-8434-OUT;n:type:ShaderForge.SFN_Ceil,id:5375,x:30970,y:33238,varname:node_5375,prsc:2|IN-6471-OUT;n:type:ShaderForge.SFN_OneMinus,id:5182,x:31134,y:33238,varname:node_5182,prsc:2|IN-5375-OUT;n:type:ShaderForge.SFN_If,id:7833,x:31405,y:33162,varname:node_7833,prsc:2|A-4808-OUT,B-7688-OUT,GT-5182-OUT,EQ-5182-OUT,LT-1454-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:4808,x:30657,y:33063,ptovrint:False,ptlb:Reverse,ptin:_Reverse,varname:node_4808,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;n:type:ShaderForge.SFN_Vector1,id:7688,x:30196,y:33282,varname:node_7688,prsc:2,v1:0.5;n:type:ShaderForge.SFN_ToggleProperty,id:5295,x:30211,y:33196,ptovrint:False,ptlb:Direction,ptin:_Direction,varname:node_5295,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;n:type:ShaderForge.SFN_If,id:7445,x:30509,y:33298,varname:node_7445,prsc:2|A-5295-OUT,B-7688-OUT,GT-6948-U,EQ-6948-U,LT-6948-V;proporder:4104-8994-8434-4808-5295;pass:END;sub:END;*/

Shader "Shader Forge/Blend2Camera Scroll" {
    Properties {
        _MainTex2 ("MainTex2", 2D) = "white" {}
        _MainTex ("MainTex", 2D) = "white" {}
        _Blend ("_Blend", Range(0, 1)) = 0.4526221
        [MaterialToggle] _Reverse ("Reverse", Float ) = 0
        [MaterialToggle] _Direction ("Direction", Float ) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Overlay+1"
            "RenderType"="Overlay"
            "PreviewType"="Plane"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
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
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _MainTex2; uniform float4 _MainTex2_ST;
            uniform float _Blend;
            uniform fixed _Reverse;
            uniform fixed _Direction;
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
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                v.vertex.xyz = float3(((o.uv0*2.0+-1.0)*float2(1.0,_ProjectionParams.r)),0.0);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = v.vertex;
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _MainTex2_var = tex2D(_MainTex2,TRANSFORM_TEX(i.uv0, _MainTex2));
                float node_7688 = 0.5;
                float node_7833_if_leA = step(_Reverse,node_7688);
                float node_7833_if_leB = step(node_7688,_Reverse);
                float node_7445_if_leA = step(_Direction,node_7688);
                float node_7445_if_leB = step(node_7688,_Direction);
                float node_7445 = lerp((node_7445_if_leA*i.uv0.g)+(node_7445_if_leB*i.uv0.r),i.uv0.r,node_7445_if_leA*node_7445_if_leB);
                float node_5182 = (1.0 - ceil((node_7445-_Blend)));
                float3 emissive = lerp(_MainTex_var.rgb,_MainTex2_var.rgb,lerp((node_7833_if_leA*floor((node_7445+_Blend)))+(node_7833_if_leB*node_5182),node_5182,node_7833_if_leA*node_7833_if_leB));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:1,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:1,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:1,hqsc:True,nrmq:1,nrsp:0,vomd:1,spxs:False,tesm:0,olmd:1,culm:2,bsrc:4,bdst:1,dpts:6,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:1,qpre:4,rntp:5,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:2865,x:32358,y:33004,varname:node_2865,prsc:2|emission-3749-OUT,voffset-4177-OUT;n:type:ShaderForge.SFN_TexCoord,id:6793,x:31747,y:33224,varname:node_6793,prsc:2,uv:0;n:type:ShaderForge.SFN_ProjectionParameters,id:8707,x:31733,y:33437,varname:node_8707,prsc:2;n:type:ShaderForge.SFN_RemapRange,id:9496,x:31932,y:33224,varname:node_9496,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-6793-UVOUT;n:type:ShaderForge.SFN_Append,id:6538,x:31932,y:33396,varname:node_6538,prsc:2|A-5896-OUT,B-8707-SGN;n:type:ShaderForge.SFN_Vector1,id:5896,x:31733,y:33378,varname:node_5896,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:4177,x:32131,y:33294,varname:node_4177,prsc:2|A-9496-OUT,B-6538-OUT;n:type:ShaderForge.SFN_Tex2d,id:8994,x:30868,y:33125,ptovrint:False,ptlb:node_8994,ptin:_node_8994,varname:node_8994,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:76a1572c73453431aacb9f1f816e1fed,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:4104,x:31518,y:33260,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_4104,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0c4c3240630b08d41a56a5142684b7af,ntxv:0,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:6948,x:30551,y:33313,varname:node_6948,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:541,x:31153,y:33053,ptovrint:False,ptlb:node_541,ptin:_node_541,varname:node_541,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bb749aa859e8f6b40a44b9edd6dfb65b,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Lerp,id:3749,x:32029,y:32987,varname:node_3749,prsc:2|A-8994-RGB,B-4104-RGB,T-7836-OUT;n:type:ShaderForge.SFN_Vector1,id:5541,x:30565,y:33467,varname:node_5541,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Time,id:2092,x:30778,y:33554,varname:node_2092,prsc:2;n:type:ShaderForge.SFN_Sin,id:9776,x:31001,y:33574,varname:node_9776,prsc:2|IN-2092-T;n:type:ShaderForge.SFN_Multiply,id:4460,x:31351,y:33216,varname:node_4460,prsc:2|A-541-RGB,B-4482-OUT;n:type:ShaderForge.SFN_Clamp01,id:7836,x:31589,y:33093,varname:node_7836,prsc:2|IN-4460-OUT;n:type:ShaderForge.SFN_OneMinus,id:9904,x:31100,y:33403,varname:node_9904,prsc:2|IN-1454-OUT;n:type:ShaderForge.SFN_Add,id:3796,x:30762,y:33403,varname:node_3796,prsc:2|A-6948-U,B-4482-OUT;n:type:ShaderForge.SFN_Floor,id:1454,x:30936,y:33403,varname:node_1454,prsc:2|IN-3796-OUT;n:type:ShaderForge.SFN_Abs,id:4482,x:31182,y:33574,varname:node_4482,prsc:2|IN-9776-OUT;proporder:4104-8994-541;pass:END;sub:END;*/

Shader "Shader Forge/Blend" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _node_8994 ("node_8994", 2D) = "white" {}
        _node_541 ("node_541", 2D) = "white" {}
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
            Blend DstColor Zero
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
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_8994; uniform float4 _node_8994_ST;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform sampler2D _node_541; uniform float4 _node_541_ST;
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
                float4 _node_8994_var = tex2D(_node_8994,TRANSFORM_TEX(i.uv0, _node_8994));
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float4 _node_541_var = tex2D(_node_541,TRANSFORM_TEX(i.uv0, _node_541));
                float4 node_2092 = _Time + _TimeEditor;
                float node_4482 = abs(sin(node_2092.g));
                float3 emissive = lerp(_node_8994_var.rgb,_Texture_var.rgb,saturate((_node_541_var.rgb*node_4482)));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_8994; uniform float4 _node_8994_ST;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform sampler2D _node_541; uniform float4 _node_541_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                v.vertex.xyz = float3(((o.uv0*2.0+-1.0)*float2(1.0,_ProjectionParams.r)),0.0);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : SV_Target {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 _node_8994_var = tex2D(_node_8994,TRANSFORM_TEX(i.uv0, _node_8994));
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float4 _node_541_var = tex2D(_node_541,TRANSFORM_TEX(i.uv0, _node_541));
                float4 node_2092 = _Time + _TimeEditor;
                float node_4482 = abs(sin(node_2092.g));
                o.Emission = lerp(_node_8994_var.rgb,_Texture_var.rgb,saturate((_node_541_var.rgb*node_4482)));
                
                float3 diffColor = float3(0,0,0);
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}

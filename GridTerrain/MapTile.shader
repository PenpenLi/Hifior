// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:34288,y:32775,varname:node_3138,prsc:2|emission-777-OUT,alpha-6400-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32162,y:32823,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.875,c3:0.6215517,c4:0.5;n:type:ShaderForge.SFN_Tex2d,id:6498,x:32197,y:33003,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_6498,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:697a24be6aa830649998710c3b4523fe,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:7117,x:32590,y:33061,varname:node_7117,prsc:2|A-6498-RGB,B-6498-A;n:type:ShaderForge.SFN_ValueProperty,id:8466,x:32163,y:33271,ptovrint:False,ptlb:ShinessSpeed,ptin:_ShinessSpeed,varname:node_8466,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Time,id:4189,x:32163,y:33326,varname:node_4189,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7197,x:32382,y:33290,varname:node_7197,prsc:2|A-8466-OUT,B-4189-T;n:type:ShaderForge.SFN_ValueProperty,id:3504,x:32691,y:33214,ptovrint:False,ptlb:Shiness,ptin:_Shiness,varname:node_3504,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.4;n:type:ShaderForge.SFN_Sin,id:5310,x:32549,y:33290,varname:node_5310,prsc:2|IN-7197-OUT;n:type:ShaderForge.SFN_Abs,id:8686,x:32740,y:33290,varname:node_8686,prsc:2|IN-5310-OUT;n:type:ShaderForge.SFN_Multiply,id:9236,x:32919,y:33258,varname:node_9236,prsc:2|A-3504-OUT,B-8686-OUT;n:type:ShaderForge.SFN_Blend,id:6058,x:33101,y:33206,varname:node_6058,prsc:2,blmd:10,clmp:True|SRC-9236-OUT,DST-7117-OUT;n:type:ShaderForge.SFN_Add,id:989,x:33438,y:32859,varname:node_989,prsc:2|A-1969-OUT,B-6058-OUT;n:type:ShaderForge.SFN_Multiply,id:1969,x:32646,y:32822,varname:node_1969,prsc:2|A-7241-RGB,B-8021-OUT;n:type:ShaderForge.SFN_OneMinus,id:8021,x:32422,y:32895,varname:node_8021,prsc:2|IN-6498-A;n:type:ShaderForge.SFN_Max,id:8694,x:32778,y:32996,varname:node_8694,prsc:2|A-7241-A,B-6498-A;n:type:ShaderForge.SFN_Tex2d,id:1586,x:32865,y:32681,ptovrint:False,ptlb:Width,ptin:_Width,varname:node_1586,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a20375a76425f13458d8a6fddb79759e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5542,x:33639,y:32735,varname:node_5542,prsc:2|A-1586-RGB,B-989-OUT;n:type:ShaderForge.SFN_Color,id:1981,x:32865,y:32519,ptovrint:False,ptlb:OutlineColor,ptin:_OutlineColor,varname:node_1981,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_OneMinus,id:7807,x:33048,y:32855,varname:node_7807,prsc:2|IN-1586-R;n:type:ShaderForge.SFN_Add,id:6400,x:33223,y:32979,varname:node_6400,prsc:2|A-7807-OUT,B-8694-OUT;n:type:ShaderForge.SFN_Multiply,id:4319,x:33284,y:32541,varname:node_4319,prsc:2|A-1981-RGB,B-7807-OUT;n:type:ShaderForge.SFN_Add,id:777,x:33838,y:32649,varname:node_777,prsc:2|A-4319-OUT,B-5542-OUT;proporder:7241-6498-8466-3504-1586-1981;pass:END;sub:END;*/

Shader "Shader Forge/MapTile" {
    Properties {
        _Color ("Color", Color) = (0,0.875,0.6215517,0.5)
        _Texture ("Texture", 2D) = "white" {}
        _ShinessSpeed ("ShinessSpeed", Float ) = 2
        _Shiness ("Shiness", Float ) = 0.4
        _Width ("Width", 2D) = "white" {}
        _OutlineColor ("OutlineColor", Color) = (0,0,0,1)
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
            uniform sampler2D _Width; uniform float4 _Width_ST;
            uniform float4 _OutlineColor;
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
                float4 _Width_var = tex2D(_Width,TRANSFORM_TEX(i.uv0, _Width));
                float node_7807 = (1.0 - _Width_var.r);
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float4 node_4189 = _Time + _TimeEditor;
                float3 emissive = ((_OutlineColor.rgb*node_7807)+(_Width_var.rgb*((_Color.rgb*(1.0 - _Texture_var.a))+saturate(( (_Texture_var.rgb*_Texture_var.a) > 0.5 ? (1.0-(1.0-2.0*((_Texture_var.rgb*_Texture_var.a)-0.5))*(1.0-(_Shiness*abs(sin((_ShinessSpeed*node_4189.g)))))) : (2.0*(_Texture_var.rgb*_Texture_var.a)*(_Shiness*abs(sin((_ShinessSpeed*node_4189.g))))) )))));
                float3 finalColor = emissive;
                return fixed4(finalColor,(node_7807+max(_Color.a,_Texture_var.a)));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

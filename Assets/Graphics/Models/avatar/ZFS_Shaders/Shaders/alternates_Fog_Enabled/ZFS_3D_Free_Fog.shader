// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable

// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:0,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,dith:2,ufog:True,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:37386,y:32712,varname:node_1,prsc:2|emission-2476-OUT,alpha-7736-OUT;n:type:ShaderForge.SFN_Multiply,id:890,x:36754,y:32726,varname:node_890,prsc:2|A-1876-RGB,B-1665-OUT;n:type:ShaderForge.SFN_Add,id:892,x:34535,y:33293,varname:node_892,prsc:2|A-1805-OUT,B-1771-OUT;n:type:ShaderForge.SFN_Fresnel,id:893,x:34069,y:33164,varname:node_893,prsc:2|EXP-1173-OUT;n:type:ShaderForge.SFN_Multiply,id:895,x:35441,y:32816,varname:node_895,prsc:2|A-1316-R,B-892-OUT;n:type:ShaderForge.SFN_Tex2d,id:896,x:34274,y:32379,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_9824,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-2533-OUT;n:type:ShaderForge.SFN_Add,id:974,x:35849,y:32793,varname:node_974,prsc:2|A-1898-OUT,B-1771-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1173,x:33909,y:33182,ptovrint:False,ptlb:Fresnel_Exponent,ptin:_Fresnel_Exponent,varname:node_7044,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:1316,x:35180,y:32767,ptovrint:False,ptlb:Gradient_Texture_Decay,ptin:_Gradient_Texture_Decay,varname:node_3666,prsc:2,ntxv:0,isnm:False|UVIN-1319-OUT;n:type:ShaderForge.SFN_Append,id:1319,x:35003,y:32767,varname:node_1319,prsc:2|A-1948-OUT,B-1359-OUT;n:type:ShaderForge.SFN_TexCoord,id:1336,x:33957,y:32669,varname:node_1336,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:1340,x:34135,y:32755,varname:node_1340,prsc:2|A-1336-V,B-1342-OUT;n:type:ShaderForge.SFN_Vector1,id:1342,x:33957,y:32817,varname:node_1342,prsc:2,v1:0;n:type:ShaderForge.SFN_Add,id:1359,x:34309,y:32857,varname:node_1359,prsc:2|A-1340-OUT,B-2056-OUT;n:type:ShaderForge.SFN_Add,id:1497,x:34560,y:32767,varname:node_1497,prsc:2|A-896-R,B-1805-OUT;n:type:ShaderForge.SFN_TexCoord,id:1618,x:33894,y:33504,varname:node_1618,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:1620,x:34054,y:33504,ptovrint:False,ptlb:Gradient_Edge_Fake,ptin:_Gradient_Edge_Fake,varname:node_8258,prsc:2,tex:dd02884263250074096b0d1643f90f41,ntxv:0,isnm:False|UVIN-1618-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1652,x:36394,y:32995,varname:node_1652,prsc:2,uv:0;n:type:ShaderForge.SFN_Append,id:1654,x:36575,y:32935,varname:node_1654,prsc:2|A-1665-OUT,B-1652-V;n:type:ShaderForge.SFN_Tex2d,id:1656,x:36765,y:32935,ptovrint:False,ptlb:Gradient_Color,ptin:_Gradient_Color,varname:node_8718,prsc:2,ntxv:0,isnm:False|UVIN-1654-OUT;n:type:ShaderForge.SFN_Clamp,id:1665,x:36414,y:32869,varname:node_1665,prsc:2|IN-2450-OUT,MIN-1667-OUT,MAX-1666-OUT;n:type:ShaderForge.SFN_Vector1,id:1666,x:36246,y:32932,varname:node_1666,prsc:2,v1:0.95;n:type:ShaderForge.SFN_Vector1,id:1667,x:36246,y:32878,varname:node_1667,prsc:2,v1:0.05;n:type:ShaderForge.SFN_SwitchProperty,id:1771,x:34217,y:33504,ptovrint:False,ptlb:Edge_Detection_Fake,ptin:_Edge_Detection_Fake,varname:node_4964,prsc:2,on:True|A-1772-OUT,B-1620-R;n:type:ShaderForge.SFN_Vector1,id:1772,x:34013,y:33384,varname:node_1772,prsc:2,v1:0;n:type:ShaderForge.SFN_SwitchProperty,id:1805,x:34226,y:33146,ptovrint:False,ptlb:Fresnel,ptin:_Fresnel,varname:node_7026,prsc:2,on:True|A-1772-OUT,B-893-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:1858,x:37003,y:32576,ptovrint:False,ptlb:Gradient_Or_Solid_Color,ptin:_Gradient_Or_Solid_Color,varname:node_6130,prsc:2,on:True|A-890-OUT,B-1656-RGB;n:type:ShaderForge.SFN_Color,id:1876,x:36547,y:32660,ptovrint:False,ptlb:Solid_Color,ptin:_Solid_Color,varname:node_2921,prsc:2,glob:False,c1:0.1764706,c2:0.5229208,c3:1,c4:1;n:type:ShaderForge.SFN_SwitchProperty,id:1898,x:35665,y:32793,ptovrint:False,ptlb:Make_Same_As_Fresnel,ptin:_Make_Same_As_Fresnel,varname:node_814,prsc:2,on:True|A-1316-R,B-895-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:1948,x:34791,y:32767,ptovrint:False,ptlb:Soft_Texture,ptin:_Soft_Texture,varname:node_720,prsc:2,on:False|A-1497-OUT,B-896-R;n:type:ShaderForge.SFN_Slider,id:2056,x:33978,y:32939,ptovrint:False,ptlb:Decay,ptin:_Decay,varname:node_1536,prsc:2,min:0.05,cur:0.3,max:0.95;n:type:ShaderForge.SFN_Time,id:2434,x:33454,y:32424,varname:node_2434,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2436,x:33674,y:32392,varname:node_2436,prsc:2|A-2434-T,B-2593-OUT;n:type:ShaderForge.SFN_Multiply,id:2450,x:36076,y:32835,varname:node_2450,prsc:2|A-974-OUT,B-2452-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2452,x:35849,y:32952,ptovrint:False,ptlb:Intensity,ptin:_Intensity,varname:node_846,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:2476,x:37154,y:32792,varname:node_2476,prsc:2|A-1858-OUT,B-2477-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2477,x:36975,y:32853,ptovrint:False,ptlb:Brightness,ptin:_Brightness,varname:node_1698,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Time,id:2496,x:33465,y:32203,varname:node_2496,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2500,x:33674,y:32273,varname:node_2500,prsc:2|A-2496-T,B-2594-OUT;n:type:ShaderForge.SFN_TexCoord,id:2527,x:33674,y:32089,varname:node_2527,prsc:2,uv:0;n:type:ShaderForge.SFN_TexCoord,id:2530,x:33829,y:32797,varname:node_2530,prsc:2,uv:0;n:type:ShaderForge.SFN_Append,id:2533,x:34090,y:32319,varname:node_2533,prsc:2|A-2580-OUT,B-2590-OUT;n:type:ShaderForge.SFN_Add,id:2580,x:33882,y:32261,varname:node_2580,prsc:2|A-2527-U,B-2500-OUT;n:type:ShaderForge.SFN_Add,id:2590,x:33882,y:32392,varname:node_2590,prsc:2|A-2527-V,B-2436-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2593,x:33454,y:32571,ptovrint:False,ptlb:Pan_SpeedY,ptin:_Pan_SpeedY,varname:node_2953,prsc:2,glob:False,v1:0.1;n:type:ShaderForge.SFN_ValueProperty,id:2594,x:33465,y:32353,ptovrint:False,ptlb:Pan_SpeedX,ptin:_Pan_SpeedX,varname:node_593,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Color,id:2705,x:36917,y:33378,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_3152,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:2707,x:37191,y:33424,varname:node_2707,prsc:2|A-2705-RGB,B-2709-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2709,x:36917,y:33534,ptovrint:False,ptlb:Emissive_Brightness,ptin:_Emissive_Brightness,varname:node_3205,prsc:2,glob:False,v1:0.8;n:type:ShaderForge.SFN_ValueProperty,id:2711,x:36950,y:33057,ptovrint:False,ptlb:Transparency,ptin:_Transparency,varname:node_1674,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:7736,x:37209,y:32971,varname:node_7736,prsc:2|A-1665-OUT,B-2711-OUT;proporder:2477-2452-1858-1656-1876-896-1316-2056-1805-1898-1173-1771-1620-1948-2593-2594-2711;pass:END;sub:END;*/

Shader "ZFS Shaders/ZFS_3D_Free_Fog" {
    Properties {
        _Brightness ("Brightness", Float ) = 1
        _Intensity ("Intensity", Float ) = 1
        [MaterialToggle] _Gradient_Or_Solid_Color ("Gradient_Or_Solid_Color", Float ) = 1
        _Gradient_Color ("Gradient_Color", 2D) = "white" {}
        _Solid_Color ("Solid_Color", Color) = (0.1764706,0.5229208,1,1)
        _Texture ("Texture", 2D) = "white" {}
        _Gradient_Texture_Decay ("Gradient_Texture_Decay", 2D) = "white" {}
        _Decay ("Decay", Range(0.05, 0.95)) = 0.3
        [MaterialToggle] _Fresnel ("Fresnel", Float ) = 1
        [MaterialToggle] _Make_Same_As_Fresnel ("Make_Same_As_Fresnel", Float ) = 0
        _Fresnel_Exponent ("Fresnel_Exponent", Float ) = 1
        [MaterialToggle] _Edge_Detection_Fake ("Edge_Detection_Fake", Float ) = 0.2901961
        _Gradient_Edge_Fake ("Gradient_Edge_Fake", 2D) = "white" {}
        [MaterialToggle] _Soft_Texture ("Soft_Texture", Float ) = 1.698039
        _Pan_SpeedY ("Pan_SpeedY", Float ) = 0.1
        _Pan_SpeedX ("Pan_SpeedX", Float ) = 0
        _Transparency ("Transparency", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
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
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            // float4 unity_LightmapST;
            #ifdef DYNAMICLIGHTMAP_ON
                // float4 unity_DynamicLightmapST;
            #endif
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _Fresnel_Exponent;
            uniform sampler2D _Gradient_Texture_Decay; uniform float4 _Gradient_Texture_Decay_ST;
            uniform sampler2D _Gradient_Edge_Fake; uniform float4 _Gradient_Edge_Fake_ST;
            uniform sampler2D _Gradient_Color; uniform float4 _Gradient_Color_ST;
            uniform fixed _Edge_Detection_Fake;
            uniform fixed _Fresnel;
            uniform fixed _Gradient_Or_Solid_Color;
            uniform float4 _Solid_Color;
            uniform fixed _Make_Same_As_Fresnel;
            uniform fixed _Soft_Texture;
            uniform float _Decay;
            uniform float _Intensity;
            uniform float _Brightness;
            uniform float _Pan_SpeedY;
            uniform float _Pan_SpeedX;
            uniform float _Transparency;
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
                UNITY_FOG_COORDS(3)
                #ifndef LIGHTMAP_OFF
                    float4 uvLM : TEXCOORD4;
                #else
                    float3 shLight : TEXCOORD4;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 node_2496 = _Time + _TimeEditor;
                float4 node_2434 = _Time + _TimeEditor;
                float2 node_2533 = float2((i.uv0.r+(node_2496.g*_Pan_SpeedX)),(i.uv0.g+(node_2434.g*_Pan_SpeedY)));
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(node_2533, _Texture));
                float node_1772 = 0.0;
                float _Fresnel_var = lerp( node_1772, pow(1.0-max(0,dot(normalDirection, viewDirection)),_Fresnel_Exponent), _Fresnel );
                float2 node_1319 = float2(lerp( (_Texture_var.r+_Fresnel_var), _Texture_var.r, _Soft_Texture ),((i.uv0.g*0.0)+_Decay));
                float4 _Gradient_Texture_Decay_var = tex2D(_Gradient_Texture_Decay,TRANSFORM_TEX(node_1319, _Gradient_Texture_Decay));
                float4 _Gradient_Edge_Fake_var = tex2D(_Gradient_Edge_Fake,TRANSFORM_TEX(i.uv0, _Gradient_Edge_Fake));
                float _Edge_Detection_Fake_var = lerp( node_1772, _Gradient_Edge_Fake_var.r, _Edge_Detection_Fake );
                float node_1665 = clamp(((lerp( _Gradient_Texture_Decay_var.r, (_Gradient_Texture_Decay_var.r*(_Fresnel_var+_Edge_Detection_Fake_var)), _Make_Same_As_Fresnel )+_Edge_Detection_Fake_var)*_Intensity),0.05,0.95);
                float2 node_1654 = float2(node_1665,i.uv0.g);
                float4 _Gradient_Color_var = tex2D(_Gradient_Color,TRANSFORM_TEX(node_1654, _Gradient_Color));
                float3 emissive = (lerp( (_Solid_Color.rgb*node_1665), _Gradient_Color_var.rgb, _Gradient_Or_Solid_Color )*_Brightness);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(node_1665*_Transparency));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

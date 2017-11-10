#include "UnityCG.cginc"
#include "UnityStandardConfig.cginc"
#include "UnityLightingCommon.cginc"

Shader "Cel Shading" {

Properties {
    _Color ("Diffuse Material Color", Color) = (1,1,1,1)
    _UnlitColor ("Unlit Color", Color) = (0.5,0.5,0.5,1)
    _DiffuseThreshold ("Lighting Threshold", Range(-1.1,1)) = 0.1
    _SpecColor ("Specular Material Color", Color) = (1,1,1,1)
    _Shininess ("Shininess", Range(0.5,1)) = 1
    _OutlineThickness ("Outline Thickness", Range (0,1)) = 0.1
    _MainTex ("Main Texture", 2D) = "white" {}
}

SubShader{
    Pass {

    CGPROGRAM

    #pragma vertex vert
    //tells the cg to use a vertex-shader called vert
    #pragma fragment frag
    //tells the cg to use a fragment-shader called frag

    //User defined var

    //Toon shading uniforms
    uniform float4 _Color;
    uniform float4 _UnlitColor;
    uniform float _DiffuseThreshold;
    uniform float4 _SpecColor;
    uniform float _Shininess;
    uniform float _OutlineThickness;

    //Unity Defined
    uniform float4 _LightColor0;
    uniform sampler2D _MainTex;
    uniform float4 _MainTex_ST;

    struct vertexInput {
        //Toon Shading Var
        float4 vertex : POSITION;
        float3 normal : NORMAL;
        float4 texcoord : TEXCOORD0;
    };

    struct vertexOutput {
        float4 pos : SV_POSITION;
        float3 normalDir : TEXCOORD1;
        float4 lightDir : TEXCOORD2;
        float3 viewDir : TEXCOORD3;
        float2 uv : TEXCOORD0;
    };

    vertexOutput vert(vertexInput input)
    {

        vertexOutput output;

        //normalDirection
        output.normalDir = normalize (mul(float4(input.normal,0.0), unity_WorldToObject).xyz );

        //Unity tranform position

        //world Pos
        float4 posWorld = mul(unity_ObjectToWorld, input.vertex);

        //viewDirection 
        output.viewDir = normalize( _WorldSpaceCameraPos.xyz - posWorld.xyz ); // vector from object to the camera

        //light direction
        float fragmentToLightSource = (_WorldSpaceCameraPos.xyz = posWorld.xyz);
        output.lightDir = float4(
            normalize( lerp(_WorldSpaceLightPos0.xyz , fragmentToLightSource, _WorldSpaceLightPos0.w)),
            lerp(1.0, 1.0/length(fragmentToLightSource), _WorldSpaceLightPos0.w)
        );

        //fragmentInput output;
        output.pos = UnityObjectToClipPos( input.vertex);
        output.uv = input.texcoord;

        return output;
    }

    float4 frag(vertexOutput input) : COLOR
{
    float nDotL = dot(input.normalDir, -input.lightDir.xyz);

    float3 thresholds = float3(_DiffuseThreshold, 0.3 + 0.7 * _DiffuseThreshold, 0.6 + 0.4 * _DiffuseThreshold);
    float3 steps = step(thresholds, float3(nDotL, nDotL, nDotL));

    //Diffuse threshold calculation
    float diffuseCutoff = (steps.x + steps.y + steps.z) * 0.3;
    //specular threshold calculation
    float specularCoutoff = step(_Shininess, dot(reflect(-input.lightDir.xyz, input.normalDir), input.viewDir));
    //Calculate Outlines
    float outlineStrength = step(_OutlineThickness, dot(input.normalDir, input.viewDir));

    float3 ambientLight = (1-diffuseCutoff) * _UnlitColor.xyz; //adds general ambient illumination
    float3 diffuseReflection = (1-specularCoutoff) * _Color.xyz * diffuseCutoff;
    float3 specularReflection = _SpecColor.xyz * specularCoutoff;

    float3 combinedLight = (ambientLight + diffuseReflection) * outlineStrength + specularReflection;

    return float4(combinedLight, 1.0);
}

    ENDCG
    }
}
}
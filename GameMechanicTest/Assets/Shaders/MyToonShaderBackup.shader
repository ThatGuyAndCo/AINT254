// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MyToonShader" 
{
	// Text in comments surrounded by '??' indicates something I am essentially guessing at (due to not being able to find sufficient explinations/documentation on what is being used).

	Properties
	{
		c_colour("Diffuse Material Colour", Color) = (1,1,1,1)
		c_unlitColour("Shadows", Color) = (0.3,0.3,0.3,1)
		c_diffuseThreshold("Lighting Threshold", Range(-1.1, 1)) = 0.1
		c_specColour("Reflection Colour", Color) = (1,1,1,1)
		c_shininess("Shininess", Range(0.1,1)) = 1
		c_outlineThickness("Outline Thickness", Range(0,1)) = 0.1
		c_cuts("Highlight Cuts", Range(1,3)) = 1
		c_mainTexture("Main Texture", 2D) = "Test" {}
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert //Runs for each vertex in the model
			#pragma fragment frag //Runs for each pixel on screen

			uniform float4 c_colour;
			uniform float4 c_unlitColour;
			uniform float c_diffuseThreshold;
			uniform float4 c_specColour;
			uniform float c_shininess;
			uniform float c_outlineThickness;
			uniform int c_cuts;

			uniform float4 _LightColor0;
			uniform sampler2D c_mainTexture;
			uniform float4 c_mainTexture_ST;

			struct st_vertexInput{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct st_vertexOutput{
				float4 pos : SV_POSITION; // vertex position in world (xyzw)
				float3 normalDir : TEXCOORD1; //position of the vertex in the UVW map
				float4 lightDir : TEXCOORD2;
				float3 viewDir : TEXCOORD3;
				float2 uv : TEXCOORD0;
			};

			st_vertexOutput vert(st_vertexInput l_input){ //This is the shader method for the vertex shader we set earlier (#pragma vertex vert)
				st_vertexOutput l_output;

				l_output.normalDir = normalize ( mul( float4(l_input.normal, 0.0), unity_WorldToObject).xyz); 
				//World2Object takes the vertex position in world space (float4 matrix) and converts to a position in local (relative to model) space.
				//This is then multiplied by the normal and normalised to produce a UV coordinate for the direction of the normal (e.g. like Vector3.up)

				float4 l_posWorld = mul(unity_ObjectToWorld, l_input.vertex);
				//This takes the vertex position in object(local) space and multiplies it by the matrix that controls how objects are translated from local space to world space.

				l_output.viewDir = normalize(_WorldSpaceCameraPos.xyz - l_posWorld.xyz);
				//This calculates a direction (vector) between the vertex's world position and the camera's position

				float3 l_fragmentToLightSource = _WorldSpaceCameraPos.xyz - l_posWorld.xyz;
				//??Position of pixel on camera 'lens'??

				l_output.lightDir = float4(
					normalize(lerp(_WorldSpaceLightPos0.xyz, l_fragmentToLightSource, _WorldSpaceLightPos0.w)), 
					//Linear interpolation between position of directional light in world and position of ??pixel on lens??, by the ??range(scale) of light??, normalised to give a float3 dir
					lerp(1.0, 1.0/length(l_fragmentToLightSource), _WorldSpaceLightPos0.w)
					//Linear interpolation between 1, and 1/length of the vector ??(float3 == 3)??, by the ??range(scale) of light??
				);
				//This returns a direction between the light and the ??pixel on screen?? and a scalar float for ??how much the light is affecting that pixel (based on proximity and range)??

				l_output.pos = UnityObjectToClipPos(l_input.vertex);
				//This multiplies the vertex position by the (model*view*projection) matrix 
				//??(matrix that controls how objects are transformed from object space to view (camera) to projection (perspective.orthographic))???. 

				l_output.uv = l_input.texcoord; //2D pos of the vertex in the UVW map

				return l_output;
			}

			float4 frag(st_vertexOutput l_input) : COLOR//This is the shader method for the pixel (fragment) shader we set earlier (#pragma fragment frag). This runs after the vertex shader, using the output from the vertex shader
			{
				float l_nDotL = saturate(dot(l_input.normalDir, l_input.lightDir.xyz));
				//nDotL is the dot product (multiplying the same indexes of a vector then adding them together to return a single number) clamped to a value between 0 and 1 (saturate, scales the value from 0-inf to 0-1 where 1=inf)
				//In this case, the two vectors are the direction of the normal and the direction of the light, returning a value of ??how directly the light is shining on the normal (for working out shadows/cuts)??

				float l_diffuseCutoff = saturate((max(c_diffuseThreshold, l_nDotL) - c_diffuseThreshold) * 1000);
				//If nDotL returns less than the threshold, the value is set to 0. This is what I will need to play with to introduce cuts into the equation. The multiplier gives the sharp edge.

				float l_reflectionCutoff = saturate((max(c_shininess, dot(reflect(-l_input.lightDir.xyz, l_input.normalDir), l_input.viewDir)) - c_shininess) * 1000);
				//The reflect essentially reverses the negative of the light direction (without the ??scalar??) around the pivot of the normal direction.
				//This then creates a dot product of the reflection and the view direction. This is compared to the shininess, and if the shininess is greater, the value will return 0.
				//Finally, this is all clamped to 0,1 and multiplies by 1000 to get the sharp edge(why the inconsistancy? requires testing.)

				float l_outlineStrength = saturate((dot(l_input.normalDir, l_input.viewDir) - c_outlineThickness) * 1000);
				//This produces a dot product of the normal direction and view direction of the fragment, which gives a basic outline of the shape (if the view dir and the normal directions aren't close enough, produces an outline as this is likely the edge of the shape (in relation to the view))

				float3 l_ambientLight = (1-l_diffuseCutoff) * c_unlitColour.xyz; 
				//This reduces the RGB values relative to (??how directly the light is shining on the normal?? compared to the threshold weset in the inspector). 
				//This produces a more faded, darker light that works as ambient light. It helps stop the unlit areas becoming too dark.

				float3 l_diffuseReflection = (1-l_reflectionCutoff) * c_colour.xyz * l_diffuseCutoff;
				//If the diffuse cutoff value is 0 (nDotL < threshold) then this will not produce a 'reflection'. The higher the threshold, the fewer pixels' nDotL will be able to pass this threshold and will reflect.

				float3 l_specularReflection = c_specColour.xyz * l_reflectionCutoff;
				//If the reflection cutoff is a low value, this will make the specular darker and will show up as reflection on the object.

				float3 l_combinedLight = (l_ambientLight + l_diffuseReflection) * l_outlineStrength + l_specularReflection;
				//

				return float4(l_combinedLight, 1.0);
			}

			ENDCG
		}
	}

	FallBack "Diffuse"
}

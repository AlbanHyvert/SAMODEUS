// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water"
{
    Properties
    {
		_RenderTexture("RenderTexture", 2D) = "white" {}
		_FoamTexture("FoamTexture", 2D) = "white" {}
		_Waves("Waves", 2D) = "white" {}
		_WaveHeight("Wave Height", Range( 0 , 5)) = 0.2
		_Distorsion("Distorsion", 2D) = "bump" {}
		_Panner_1("Panner_1", Float) = 2
		_Panner_2("Panner_2", Float) = 3.5
		_UV1_TilexyScalezw("UV1_Tile (xy) Scale (zw)", Vector) = (1,1,5,5)
		_UV2_TilexyScalezw("UV2_Tile (xy) Scale (zw)", Vector) = (0.5,0.5,5,5)
		_DistorsionBlend("DistorsionBlend", Range( 0 , 1)) = 0.5
		_FresnelPower("FresnelPower", Range( 0 , 4)) = 1
		_FresnelNormal_Influence("FresnelNormal_Influence", Range( 0 , 1)) = 0.3
		_RippleHardness("RippleHardness", Float) = 0.5
		_FoamThickness("FoamThickness", Range( 0.075 , 1)) = 0.1
		_FoamColor("FoamColor", Color) = (0.2509804,0.3921569,0.4823529,0)
		[Toggle(_RIPPLEFADE_ON)] _RippleFade("RippleFade", Float) = 0
		_FadeRange("FadeRange", Float) = 10

    }

    SubShader
    {
		LOD 0
        
		

		Tags { "RenderPipeline"="HDRenderPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZTest LEqual
		ZWrite Off
		ZClip [_ZClip]
		
		HLSLINCLUDE
			#pragma target 4.5
            #pragma only_renderers d3d11 ps4 xboxone vulkan metal switch
            #pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
            #pragma multi_compile _ LOD_FADE_CROSSFADE
            
			struct GlobalSurfaceDescription
            {
                float3 Albedo;
                float3 Normal;
                float3 BentNormal;
				float3 Specular;
                float CoatMask;
                float Metallic;
                float3 Emission;
                float Smoothness;
                float Occlusion;
                float Alpha;
				float AlphaClipThreshold;
				float SpecularAAScreenSpaceVariance;
				float SpecularAAThreshold;
				float SpecularOcclusion;
				//Refraction
				float RefractionIndex;
                float3 RefractionColor;
                float RefractionDistance;
				//SSS/Translucent
				float Thickness;
				float SubsurfaceMask;
                uint DiffusionProfile;
				//Anisotropy
				float Anisotropy;
				float3 Tangent;
				//Iridescent
				float IridescenceMask;
				float IridescenceThickness;
            };

			struct AlphaSurfaceDescription
            {
                float Alpha;
				float AlphaClipThreshold;
            };

			struct SmoothSurfaceDescription
            {
                float Smoothness;
                float Alpha;
				float AlphaClipThreshold;
            };

			struct DistortionSurfaceDescription
            {
                float Alpha;
                float2 Distortion;
                float DistortionBlur;
				float AlphaClipThreshold;
            };
		ENDHLSL
		
        Pass
        {
			
            Name "GBuffer"
            Tags { "LightMode"="GBuffer" }
           
			Stencil
			{
				Ref 2
				WriteMask 7
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}

        
            HLSLPROGRAM

				#define _DECALS 1
				#define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
				#define _ENABLE_FOG_ON_TRANSPARENT 1
				#define _SURFACE_TYPE_TRANSPARENT 1
				#define _BLENDMODE_ALPHA 1
				#define ASE_SRP_VERSION 41000
				#define REQUIRE_OPAQUE_TEXTURE 1


				#pragma vertex Vert
				#pragma fragment Frag
        
				#define ASE_NEEDS_FRAG_RELATIVE_WORLD_POS
				#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
				#define ASE_NEEDS_FRAG_NORMAL
				#pragma shader_feature _RIPPLEFADE_ON


			    //#define UNITY_MATERIAL_LIT
            
				#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
				#define OUTPUT_SPLIT_LIGHTING
				#endif
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Wind.hlsl"
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
                #define SHADERPASS SHADERPASS_GBUFFER
                #pragma multi_compile _ LIGHTMAP_ON
                #pragma multi_compile _ DIRLIGHTMAP_COMBINED
                #pragma multi_compile _ DYNAMICLIGHTMAP_ON
                #pragma multi_compile _ SHADOWS_SHADOWMASK
                #pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
                #pragma multi_compile _ LIGHT_LAYERS
        
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD1
                #define ATTRIBUTES_NEED_TEXCOORD2
                #define VARYINGS_NEED_POSITION_WS
                #define VARYINGS_NEED_TANGENT_TO_WORLD
                #define VARYINGS_NEED_TEXCOORD1
                #define VARYINGS_NEED_TEXCOORD2
        
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"       
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
				
				struct AttributesMesh 
				{
					float3 positionOS : POSITION;
					float3 normalOS : NORMAL;
					float4 tangentOS : TANGENT;
					float4 uv1 : TEXCOORD1;
					float4 uv2 : TEXCOORD2;
					
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif
				};
        
				struct PackedVaryingsMeshToPS 
				{
					float4 positionCS : SV_Position;
					float3 interp00 : TEXCOORD0;
					float3 interp01 : TEXCOORD1;
					float4 interp02 : TEXCOORD2;
					float4 interp03 : TEXCOORD3;
					float4 interp04 : TEXCOORD4;
					float4 ase_texcoord5 : TEXCOORD5;
					float3 ase_normal : NORMAL;
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif
				};

				sampler2D _Waves;
				sampler2D _Distorsion;
				sampler2D _FoamTexture;
				sampler2D _RenderTexture;
				float3 _CamPosition;
				float _CamSize;
				float3 _ComponentPosition;
				CBUFFER_START( UnityPerMaterial )
				float4 _UV1_TilexyScalezw;
				float4 _UV2_TilexyScalezw;
				float4 _FoamColor;
				float _WaveHeight;
				float _Panner_1;
				float _Panner_2;
				float _DistorsionBlend;
				float _FresnelNormal_Influence;
				float _FresnelPower;
				float _FoamThickness;
				float _RippleHardness;
				float _FadeRange;
				CBUFFER_END


				inline float4 ASE_ComputeGrabScreenPos( float4 pos )
				{
					#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
					#else
					float scale = 1.0;
					#endif
					float4 o = pos;
					o.y = pos.w * 0.5f;
					o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
					return o;
				}
				
				float4 ASEHDSampleSceneColor( float2 uv )
				{
					#if defined(REQUIRE_OPAQUE_TEXTURE) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(SHADERPASS) && (SHADERPASS != SHADERPASS_LIGHT_TRANSPORT)
					return float4( SampleCameraColor(uv), 1.0 );
					#endif
					return float4(0.0, 0.0, 0.0, 1.0);
				}
				
    
				void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData, out float3 bentNormalWS)
				{
					ZERO_INITIALIZE(SurfaceData, surfaceData);
        
					surfaceData.baseColor =                 surfaceDescription.Albedo;
					surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
			#ifdef _SPECULAR_OCCLUSION_CUSTOM
					surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
			#endif
					surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
					surfaceData.metallic =                  surfaceDescription.Metallic;
					surfaceData.coatMask =                  surfaceDescription.CoatMask;
			
			#ifdef _MATERIAL_FEATURE_IRIDESCENCE		
					surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
					surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
			#endif
					surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
			#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
			#endif
			#ifdef _MATERIAL_FEATURE_TRANSMISSION
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
			#endif
			#ifdef _MATERIAL_FEATURE_ANISOTROPY
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
			#endif

			#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
			#endif

			#ifdef _MATERIAL_FEATURE_IRIDESCENCE
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
			#endif
			#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
					surfaceData.specularColor = surfaceDescription.Specular;
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
			#endif
        
			#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
					surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
			#endif
        
					float3 normalTS = float3(0.0f, 0.0f, 1.0f);
					normalTS = surfaceDescription.Normal;
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS,doubleSidedConstants);
        
					bentNormalWS = surfaceData.normalWS;
					surfaceData.geomNormalWS = fragInputs.worldToTangent[2];
			
			#ifdef ASE_BENT_NORMAL
					GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS,doubleSidedConstants);
			#endif
			
			#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
					surfaceData.thickness =	                 surfaceDescription.Thickness;
			#endif

			#ifdef _HAS_REFRACTION
					if (_EnableSSRefraction)
					{
						surfaceData.ior =                       surfaceDescription.RefractionIndex;
						surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
						surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
						surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
						surfaceDescription.Alpha = 1.0;
					}
					else
					{
						surfaceData.ior = 1.0;
						surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
						surfaceData.atDistance = 1.0;
						surfaceData.transmittanceMask = 0.0;
						surfaceDescription.Alpha = 1.0;
					}
			#else
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
			#endif

			#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
					surfaceData.thickness =	                 surfaceDescription.Thickness;
			#endif

			#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
			#endif
			
			#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
					surfaceData.diffusionProfile = surfaceDescription.DiffusionProfile;
			#endif
					surfaceData.tangentWS = normalize(fragInputs.worldToTangent[0].xyz);    // The tangent is not normalize in worldToTangent for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
			#ifdef _MATERIAL_FEATURE_ANISOTROPY	
					surfaceData.anisotropy = surfaceDescription.Anisotropy;
					surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.worldToTangent);
			#endif
					surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
			
			#if defined(_SPECULAR_OCCLUSION_CUSTOM)
			#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
			#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
			#else
					surfaceData.specularOcclusion = 1.0;
			#endif
			#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
					surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.worldToTangent[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
			#endif
				}
        
				void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
				{
			#ifdef LOD_FADE_CROSSFADE
					uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx));
					LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
			#endif

			#ifdef _ALPHATEST_ON
					DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
			#endif

					float3 bentNormalWS;
					BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData, bentNormalWS);
        
			#if HAVE_DECALS
					if (_EnableDecals)
					{
						DecalSurfaceData decalSurfaceData = GetDecalSurfaceData (posInput, surfaceDescription.Alpha);
						ApplyDecalToSurfaceData (decalSurfaceData, surfaceData);
					}
			#endif
        
					InitBuiltinData(surfaceDescription.Alpha, bentNormalWS, -fragInputs.worldToTangent[2], fragInputs.positionRWS, fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
        
					builtinData.emissiveColor = surfaceDescription.Emission;
        
					builtinData.depthOffset = 0.0;
        
			#if (SHADERPASS == SHADERPASS_DISTORTION)
					builtinData.distortion = surfaceDescription.Distortion;
					builtinData.distortionBlur = surfaceDescription.DistortionBlur;
			#else
					builtinData.distortion = float2(0.0, 0.0);
					builtinData.distortionBlur = 0.0;
			#endif
        
					PostInitBuiltinData(V, posInput, surfaceData, builtinData);
				}
        
			PackedVaryingsMeshToPS Vert(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);

				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				float2 panner356 = ( ( _Time.y * 0.35 ) * float2( 1,-1 ) + ((ase_worldPos).xz*0.2 + 0.0));
				float3 appendResult367 = (float3(0.0 , ( ( tex2Dlod( _Waves, float4( panner356, 0, 1.0) ).r - 0.5 ) * _WaveHeight ) , 0.0));
				
				float4 ase_clipPos = TransformWorldToHClip( TransformObjectToWorld(inputMesh.positionOS));
				float4 screenPos = ComputeScreenPos( ase_clipPos , _ProjectionParams.x );
				outputPackedVaryingsMeshToPS.ase_texcoord5 = screenPos;
				
				outputPackedVaryingsMeshToPS.ase_normal = inputMesh.normalOS;
				float3 vertexValue = appendResult367;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz =	positionRWS;
				outputPackedVaryingsMeshToPS.interp01.xyz =	normalWS;
				outputPackedVaryingsMeshToPS.interp02.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.interp03.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.interp04.xyzw = inputMesh.uv2;
			
				return outputPackedVaryingsMeshToPS;
			}

			void Frag(  PackedVaryingsMeshToPS packedInput,
						OUTPUT_GBUFFER(outGBuffer)
						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
						#endif
						 
						)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.worldToTangent = k_identity3x3;
				float3 positionRWS = packedInput.interp00.xyz;
				float3 normalWS = packedInput.interp01.xyz;
				float4 tangentWS = packedInput.interp02.xyzw;

				input.positionSS = packedInput.positionCS;
				input.positionRWS = positionRWS;
				input.worldToTangent = BuildWorldToTangent(tangentWS, normalWS);
				input.texCoord1 = packedInput.interp03.xyzw;
				input.texCoord2 = packedInput.interp04.xyzw;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);
				float3 normalizedWorldViewDir = GetWorldSpaceNormalizeViewDir(input.positionRWS);
				SurfaceData surfaceData;
				BuiltinData builtinData;

				GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
				float4 screenPos = packedInput.ase_texcoord5;
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float2 appendResult6 = (float2(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g));
				float3 ase_worldPos = GetAbsolutePositionWS( positionRWS );
				float2 temp_output_18_0 = (ase_worldPos).xz;
				float2 appendResult57 = (float2(_UV1_TilexyScalezw.x , _UV1_TilexyScalezw.y));
				float2 appendResult58 = (float2(_UV1_TilexyScalezw.z , _UV1_TilexyScalezw.w));
				float2 UV131 = ( ( ( temp_output_18_0 + ( _Time.x * _Panner_1 ) ) * appendResult57 ) / appendResult58 );
				float2 appendResult60 = (float2(_UV2_TilexyScalezw.x , _UV2_TilexyScalezw.y));
				float2 appendResult61 = (float2(_UV2_TilexyScalezw.z , _UV2_TilexyScalezw.w));
				float2 UV239 = ( ( ( temp_output_18_0 + ( _Time.x * _Panner_2 ) ) * appendResult60 ) / appendResult61 );
				float3 lerpResult7 = lerp( UnpackNormalmapRGorAG( tex2D( _Distorsion, UV131 ), 1.0f ) , UnpackNormalmapRGorAG( tex2D( _Distorsion, UV239 ), 1.0f ) , _DistorsionBlend);
				float3 Distorsion9 = lerpResult7;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth540 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth540 = abs( ( screenDepth540 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 200.0 ) );
				float clampResult543 = clamp( distanceDepth540 , 0.0 , 1.0 );
				float2 lerpResult542 = lerp( ( (Distorsion9).xy * 0.1 ) , float2( 0,0 ) , clampResult543);
				float2 ScreenUV11 = ( appendResult6 - lerpResult542 );
				float4 fetchOpaqueVal1 = ASEHDSampleSceneColor(ScreenUV11);
				float4 color65 = IsGammaSpace() ? float4(0.1076451,0.4492441,0.5566038,0) : float4(0.01125052,0.1700325,0.2702231,0);
				float2 appendResult400 = (float2(packedInput.ase_normal.x , packedInput.ase_normal.y));
				float3 appendResult404 = (float3(saturate( ( appendResult400 - (Distorsion9).xy ) ) , packedInput.ase_normal.z));
				float3 lerpResult407 = lerp( packedInput.ase_normal , appendResult404 , _FresnelNormal_Influence);
				float dotResult68 = dot( normalizedWorldViewDir , lerpResult407 );
				float temp_output_72_0 = pow( ( 1.0 - saturate( abs( dotResult68 ) ) ) , _FresnelPower );
				float4 lerpResult64 = lerp( fetchOpaqueVal1 , color65 , temp_output_72_0);
				
				float screenDepth312 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth312 = abs( ( screenDepth312 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _FoamThickness ) );
				float screenDepth302 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth302 = abs( ( screenDepth302 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 0.5 ) );
				float lerpResult319 = lerp( 0.0 , tex2D( _FoamTexture, UV131 ).r , ( 1.0 - distanceDepth302 ));
				float screenDepth309 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth309 = abs( ( screenDepth309 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 4.0 ) );
				float lerpResult328 = lerp( 0.0 , tex2D( _FoamTexture, UV239 ).r , ( 1.0 - distanceDepth309 ));
				float smoothstepResult453 = smoothstep( _RippleHardness , ( _RippleHardness + 0.05 ) , tex2D( _RenderTexture, ( ( ( (ase_worldPos).xz - (_CamPosition).xz ) / ( _CamSize * 2.0 ) ) + float2( 0.5,0.5 ) ) ).b);
				float clampResult481 = clamp( ( distance( (ase_worldPos).xz , (_ComponentPosition).xz ) - _FadeRange ) , 0.0 , 1.0 );
				#ifdef _RIPPLEFADE_ON
				float staticSwitch500 = clampResult481;
				#else
				float staticSwitch500 = 0.0;
				#endif
				float lerpResult482 = lerp( smoothstepResult453 , 0.0 , staticSwitch500);
				float clampResult343 = clamp( ( ( ( step( 0.0 , ( 1.0 - distanceDepth312 ) ) + step( 0.35 , lerpResult319 ) ) + step( 0.65 , lerpResult328 ) ) + lerpResult482 ) , 0.0 , 1.0 );
				float4 Emission345 = ( clampResult343 * _FoamColor );
				
                surfaceDescription.Albedo = lerpResult64.rgb;
                surfaceDescription.Normal = float3( 0, 0, 1 );
                surfaceDescription.BentNormal = float3( 0, 0, 1 );
                surfaceDescription.CoatMask = 0;
                surfaceDescription.Metallic = 0;
				
				#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
				surfaceDescription.Specular = 0;
				#endif
                
				surfaceDescription.Emission = Emission345.rgb;
                surfaceDescription.Smoothness = 0.5;
                surfaceDescription.Occlusion = 1;
				surfaceDescription.Alpha = 1;
				
				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = 0;
				#endif

				#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
				surfaceDescription.SpecularAAScreenSpaceVariance = 0;
				surfaceDescription.SpecularAAThreshold = 0;
				#endif

				#ifdef _SPECULAR_OCCLUSION_CUSTOM
				surfaceDescription.SpecularOcclusion = 0;
				#endif

				#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
				surfaceDescription.Thickness = 1;
				#endif

				#ifdef _HAS_REFRACTION
				surfaceDescription.RefractionIndex = 1;
                surfaceDescription.RefractionColor = float3(1,1,1);
                surfaceDescription.RefractionDistance = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
				surfaceDescription.SubsurfaceMask = 1;
				#endif
			
				#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
				surfaceDescription.DiffusionProfile = 0;
				#endif

				#ifdef _MATERIAL_FEATURE_ANISOTROPY
				surfaceDescription.Anisotropy = 1;
				surfaceDescription.Tangent = float3(1,0,0);
				#endif

				#ifdef _MATERIAL_FEATURE_IRIDESCENCE
				surfaceDescription.IridescenceMask = 0;
				surfaceDescription.IridescenceThickness = 0;
				#endif

				GetSurfaceAndBuiltinData(surfaceDescription,input, normalizedWorldViewDir, posInput, surfaceData, builtinData);
				ENCODE_INTO_GBUFFER(surfaceData, builtinData, posInput.positionSS, outGBuffer);
			#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
			#endif
			}
     
            ENDHLSL
        }  

		
        Pass
        {
			
            Name "META"
            Tags { "LightMode"="Meta" }
            Cull Off
            
            HLSLPROGRAM
        
				#define _DECALS 1
				#define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
				#define _ENABLE_FOG_ON_TRANSPARENT 1
				#define _SURFACE_TYPE_TRANSPARENT 1
				#define _BLENDMODE_ALPHA 1
				#define ASE_SRP_VERSION 41000
				#define REQUIRE_OPAQUE_TEXTURE 1

        
				#pragma vertex Vert
				#pragma fragment Frag
        
				#define ASE_NEEDS_FRAG_NORMAL
				#pragma shader_feature _RIPPLEFADE_ON


				//#define UNITY_MATERIAL_LIT   

				#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
				#define OUTPUT_SPLIT_LIGHTING
				#endif
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Wind.hlsl"
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
				#define SHADERPASS SHADERPASS_LIGHT_TRANSPORT
        
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define ATTRIBUTES_NEED_TEXCOORD1
                #define ATTRIBUTES_NEED_TEXCOORD2
                #define ATTRIBUTES_NEED_COLOR
        
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
				struct AttributesMesh 
				{
					float3 positionOS : POSITION;
					float3 normalOS : NORMAL;
					float4 tangentOS : TANGENT;
					float4 uv0 : TEXCOORD0;
					float4 uv1 : TEXCOORD1;
					float4 uv2 : TEXCOORD2;
					float4 color : COLOR;
					
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif
				};
        
				struct PackedVaryingsMeshToPS 
				{
					float4 positionCS : SV_Position; 
					float4 ase_texcoord : TEXCOORD0;
					float4 ase_texcoord1 : TEXCOORD1;
					float3 ase_normal : NORMAL;
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif
				};

				sampler2D _Waves;
				sampler2D _Distorsion;
				sampler2D _FoamTexture;
				sampler2D _RenderTexture;
				float3 _CamPosition;
				float _CamSize;
				float3 _ComponentPosition;
				CBUFFER_START( UnityPerMaterial )
				float4 _UV1_TilexyScalezw;
				float4 _UV2_TilexyScalezw;
				float4 _FoamColor;
				float _WaveHeight;
				float _Panner_1;
				float _Panner_2;
				float _DistorsionBlend;
				float _FresnelNormal_Influence;
				float _FresnelPower;
				float _FoamThickness;
				float _RippleHardness;
				float _FadeRange;
				CBUFFER_END

				
				inline float4 ASE_ComputeGrabScreenPos( float4 pos )
				{
					#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
					#else
					float scale = 1.0;
					#endif
					float4 o = pos;
					o.y = pos.w * 0.5f;
					o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
					return o;
				}
				
				float4 ASEHDSampleSceneColor( float2 uv )
				{
					#if defined(REQUIRE_OPAQUE_TEXTURE) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(SHADERPASS) && (SHADERPASS != SHADERPASS_LIGHT_TRANSPORT)
					return float4( SampleCameraColor(uv), 1.0 );
					#endif
					return float4(0.0, 0.0, 0.0, 1.0);
				}
				
                
				void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData, out float3 bentNormalWS)
				{
					ZERO_INITIALIZE(SurfaceData, surfaceData);
					surfaceData.baseColor =                 surfaceDescription.Albedo;
					surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
			#ifdef _SPECULAR_OCCLUSION_CUSTOM
					surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
			#endif
					surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
					surfaceData.metallic =                  surfaceDescription.Metallic;
					surfaceData.coatMask =                  surfaceDescription.CoatMask;
		
			#ifdef _MATERIAL_FEATURE_IRIDESCENCE		
					surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
					surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
			#endif
					surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
			#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
			#endif
			#ifdef _MATERIAL_FEATURE_TRANSMISSION
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
			#endif
			#ifdef _MATERIAL_FEATURE_ANISOTROPY
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
			#endif

        	#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
			#endif

			#ifdef _MATERIAL_FEATURE_IRIDESCENCE
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
			#endif
			#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
					surfaceData.specularColor = surfaceDescription.Specular;
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
			#endif
        
			#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
					surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
			#endif
					float3 normalTS = float3(0.0f, 0.0f, 1.0f);
					normalTS = surfaceDescription.Normal;
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);

					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS,doubleSidedConstants);
					bentNormalWS = surfaceData.normalWS;
					surfaceData.geomNormalWS = fragInputs.worldToTangent[2];
			
			#ifdef ASE_BENT_NORMAL
					GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS,doubleSidedConstants);
			#endif
			
			#ifdef _HAS_REFRACTION
					if (_EnableSSRefraction)
					{
						surfaceData.ior =                       surfaceDescription.RefractionIndex;
						surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
						surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
						surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
						surfaceDescription.Alpha = 1.0;
					}
					else
					{
						surfaceData.ior = 1.0;
						surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
						surfaceData.atDistance = 1.0;
						surfaceData.transmittanceMask = 0.0;
						surfaceDescription.Alpha = 1.0;
					}
			#else
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
			#endif

			#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
					surfaceData.thickness =	                 surfaceDescription.Thickness;
			#endif

			#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
			#endif
			
			#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
					surfaceData.diffusionProfile = surfaceDescription.DiffusionProfile;
			#endif
					surfaceData.tangentWS = normalize(fragInputs.worldToTangent[0].xyz);    // The tangent is not normalize in worldToTangent for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
			#ifdef _MATERIAL_FEATURE_ANISOTROPY	
					surfaceData.anisotropy = surfaceDescription.Anisotropy;
					surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.worldToTangent);
			#endif
					surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
			#if defined(_SPECULAR_OCCLUSION_CUSTOM)
			#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
			#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
			#else
					surfaceData.specularOcclusion = 1.0;
			#endif
			#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
					surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.worldToTangent[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
			#endif
        
				}
        
				void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription,FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
				{
			#ifdef LOD_FADE_CROSSFADE
					uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); // Quantize V to _ScreenSize values
					LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
			#endif
        
					#ifdef _ALPHATEST_ON
						DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
					#endif

					float3 bentNormalWS;
					BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData, bentNormalWS);
        
			#if HAVE_DECALS
					if (_EnableDecals)
					{
						DecalSurfaceData decalSurfaceData = GetDecalSurfaceData (posInput, surfaceDescription.Alpha);
						ApplyDecalToSurfaceData (decalSurfaceData, surfaceData);
					}
			#endif
        
					InitBuiltinData(surfaceDescription.Alpha, bentNormalWS, -fragInputs.worldToTangent[2], fragInputs.positionRWS, fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
        
					builtinData.emissiveColor = surfaceDescription.Emission;
        
					builtinData.depthOffset = 0.0;                        
        
			#if (SHADERPASS == SHADERPASS_DISTORTION)
					builtinData.distortion = surfaceDescription.Distortion;
					builtinData.distortionBlur = surfaceDescription.DistortionBlur;
			#else
					builtinData.distortion = float2(0.0, 0.0);
					builtinData.distortionBlur = 0.0;
			#endif
        
					PostInitBuiltinData(V, posInput, surfaceData, builtinData);
				}
        
				CBUFFER_START(UnityMetaPass)
				bool4 unity_MetaVertexControl;
				bool4 unity_MetaFragmentControl;
				CBUFFER_END

				float unity_OneOverOutputBoost;
				float unity_MaxOutputValue;

				PackedVaryingsMeshToPS Vert(AttributesMesh inputMesh  )
				{
					PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

					UNITY_SETUP_INSTANCE_ID(inputMesh);
					UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);

					float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
					float2 panner356 = ( ( _Time.y * 0.35 ) * float2( 1,-1 ) + ((ase_worldPos).xz*0.2 + 0.0));
					float3 appendResult367 = (float3(0.0 , ( ( tex2Dlod( _Waves, float4( panner356, 0, 1.0) ).r - 0.5 ) * _WaveHeight ) , 0.0));
					
					float4 ase_clipPos = TransformWorldToHClip( TransformObjectToWorld(inputMesh.positionOS));
					float4 screenPos = ComputeScreenPos( ase_clipPos , _ProjectionParams.x );
					outputPackedVaryingsMeshToPS.ase_texcoord = screenPos;
					outputPackedVaryingsMeshToPS.ase_texcoord1.xyz = ase_worldPos;
					
					outputPackedVaryingsMeshToPS.ase_normal = inputMesh.normalOS;
					
					//setting value to unused interpolator channels and avoid initialization warnings
					outputPackedVaryingsMeshToPS.ase_texcoord1.w = 0;
					float3 vertexValue = appendResult367;
					#ifdef ASE_ABSOLUTE_VERTEX_POS
					inputMesh.positionOS.xyz = vertexValue;
					#else
					inputMesh.positionOS.xyz += vertexValue;
					#endif
					
					inputMesh.normalOS =  inputMesh.normalOS ;

					float2 uv;

					if (unity_MetaVertexControl.x)
					{
						uv = inputMesh.uv1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
					}
					else if (unity_MetaVertexControl.y)
					{
						uv = inputMesh.uv2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
					}

					outputPackedVaryingsMeshToPS.positionCS = float4(uv * 2.0 - 1.0, inputMesh.positionOS.z > 0 ? 1.0e-4 : 0.0, 1.0);
					return outputPackedVaryingsMeshToPS;
				}

				float4 Frag(PackedVaryingsMeshToPS packedInput  ) : SV_Target
				{			
					UNITY_SETUP_INSTANCE_ID( packedInput );
					FragInputs input;
					ZERO_INITIALIZE(FragInputs, input);
					input.worldToTangent = k_identity3x3;
					input.positionSS = packedInput.positionCS;
         
					PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);
					float3 V = float3(1.0, 1.0, 1.0);

					SurfaceData surfaceData;
					BuiltinData builtinData;
					GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
					float4 screenPos = packedInput.ase_texcoord;
					float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
					float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
					float2 appendResult6 = (float2(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g));
					float3 ase_worldPos = packedInput.ase_texcoord1.xyz;
					float2 temp_output_18_0 = (ase_worldPos).xz;
					float2 appendResult57 = (float2(_UV1_TilexyScalezw.x , _UV1_TilexyScalezw.y));
					float2 appendResult58 = (float2(_UV1_TilexyScalezw.z , _UV1_TilexyScalezw.w));
					float2 UV131 = ( ( ( temp_output_18_0 + ( _Time.x * _Panner_1 ) ) * appendResult57 ) / appendResult58 );
					float2 appendResult60 = (float2(_UV2_TilexyScalezw.x , _UV2_TilexyScalezw.y));
					float2 appendResult61 = (float2(_UV2_TilexyScalezw.z , _UV2_TilexyScalezw.w));
					float2 UV239 = ( ( ( temp_output_18_0 + ( _Time.x * _Panner_2 ) ) * appendResult60 ) / appendResult61 );
					float3 lerpResult7 = lerp( UnpackNormalmapRGorAG( tex2D( _Distorsion, UV131 ), 1.0f ) , UnpackNormalmapRGorAG( tex2D( _Distorsion, UV239 ), 1.0f ) , _DistorsionBlend);
					float3 Distorsion9 = lerpResult7;
					float4 ase_screenPosNorm = screenPos / screenPos.w;
					ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
					float screenDepth540 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
					float distanceDepth540 = abs( ( screenDepth540 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 200.0 ) );
					float clampResult543 = clamp( distanceDepth540 , 0.0 , 1.0 );
					float2 lerpResult542 = lerp( ( (Distorsion9).xy * 0.1 ) , float2( 0,0 ) , clampResult543);
					float2 ScreenUV11 = ( appendResult6 - lerpResult542 );
					float4 fetchOpaqueVal1 = ASEHDSampleSceneColor(ScreenUV11);
					float4 color65 = IsGammaSpace() ? float4(0.1076451,0.4492441,0.5566038,0) : float4(0.01125052,0.1700325,0.2702231,0);
					float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
					ase_worldViewDir = normalize(ase_worldViewDir);
					float2 appendResult400 = (float2(packedInput.ase_normal.x , packedInput.ase_normal.y));
					float3 appendResult404 = (float3(saturate( ( appendResult400 - (Distorsion9).xy ) ) , packedInput.ase_normal.z));
					float3 lerpResult407 = lerp( packedInput.ase_normal , appendResult404 , _FresnelNormal_Influence);
					float dotResult68 = dot( ase_worldViewDir , lerpResult407 );
					float temp_output_72_0 = pow( ( 1.0 - saturate( abs( dotResult68 ) ) ) , _FresnelPower );
					float4 lerpResult64 = lerp( fetchOpaqueVal1 , color65 , temp_output_72_0);
					
					float screenDepth312 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
					float distanceDepth312 = abs( ( screenDepth312 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _FoamThickness ) );
					float screenDepth302 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
					float distanceDepth302 = abs( ( screenDepth302 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 0.5 ) );
					float lerpResult319 = lerp( 0.0 , tex2D( _FoamTexture, UV131 ).r , ( 1.0 - distanceDepth302 ));
					float screenDepth309 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
					float distanceDepth309 = abs( ( screenDepth309 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 4.0 ) );
					float lerpResult328 = lerp( 0.0 , tex2D( _FoamTexture, UV239 ).r , ( 1.0 - distanceDepth309 ));
					float smoothstepResult453 = smoothstep( _RippleHardness , ( _RippleHardness + 0.05 ) , tex2D( _RenderTexture, ( ( ( (ase_worldPos).xz - (_CamPosition).xz ) / ( _CamSize * 2.0 ) ) + float2( 0.5,0.5 ) ) ).b);
					float clampResult481 = clamp( ( distance( (ase_worldPos).xz , (_ComponentPosition).xz ) - _FadeRange ) , 0.0 , 1.0 );
					#ifdef _RIPPLEFADE_ON
					float staticSwitch500 = clampResult481;
					#else
					float staticSwitch500 = 0.0;
					#endif
					float lerpResult482 = lerp( smoothstepResult453 , 0.0 , staticSwitch500);
					float clampResult343 = clamp( ( ( ( step( 0.0 , ( 1.0 - distanceDepth312 ) ) + step( 0.35 , lerpResult319 ) ) + step( 0.65 , lerpResult328 ) ) + lerpResult482 ) , 0.0 , 1.0 );
					float4 Emission345 = ( clampResult343 * _FoamColor );
					
					surfaceDescription.Albedo = lerpResult64.rgb;
					surfaceDescription.Normal = float3( 0, 0, 1 );
					surfaceDescription.BentNormal = float3( 0, 0, 1 );
					surfaceDescription.CoatMask = 0;
					surfaceDescription.Metallic = 0;
					
					#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
					surfaceDescription.Specular = 0;
					#endif
					
					surfaceDescription.Emission = Emission345.rgb;
					surfaceDescription.Smoothness = 0.5;
					surfaceDescription.Occlusion = 1;
					surfaceDescription.Alpha = 1;
					
					#ifdef _ALPHATEST_ON
					surfaceDescription.AlphaClipThreshold = 0;
					#endif

					#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
					surfaceDescription.SpecularAAScreenSpaceVariance = 0;
					surfaceDescription.SpecularAAThreshold = 0;
					#endif

					#ifdef _SPECULAR_OCCLUSION_CUSTOM
					surfaceDescription.SpecularOcclusion = 0;
					#endif

					#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
					surfaceDescription.Thickness = 1;
					#endif

					#ifdef _HAS_REFRACTION
					surfaceDescription.RefractionIndex = 1;
					surfaceDescription.RefractionColor = float3(1,1,1);
					surfaceDescription.RefractionDistance = 0;
					#endif

					#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceDescription.SubsurfaceMask = 1;
					#endif

					#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
					surfaceDescription.DiffusionProfile = 0;
					#endif

					#ifdef _MATERIAL_FEATURE_ANISOTROPY
					surfaceDescription.Anisotropy = 1;
					surfaceDescription.Tangent = float3(1,0,0);
					#endif

					#ifdef _MATERIAL_FEATURE_IRIDESCENCE
					surfaceDescription.IridescenceMask = 0;
					surfaceDescription.IridescenceThickness = 0;
					#endif

					GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

					BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);
					LightTransportData lightTransportData = GetLightTransportData(surfaceData, builtinData, bsdfData);

					float4 res = float4(0.0, 0.0, 0.0, 1.0);
					if (unity_MetaFragmentControl.x)
					{
						res.rgb = clamp(pow(abs(lightTransportData.diffuseColor), saturate(unity_OneOverOutputBoost)), 0, unity_MaxOutputValue);
					}

					if (unity_MetaFragmentControl.y)
					{
						res.rgb = lightTransportData.emissiveColor;
					}

					return res;
				}

            ENDHLSL
        }

		
		Pass
        {
			
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }
			ColorMask 0
        
            HLSLPROGRAM
				#define _DECALS 1
				#define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
				#define _ENABLE_FOG_ON_TRANSPARENT 1
				#define _SURFACE_TYPE_TRANSPARENT 1
				#define _BLENDMODE_ALPHA 1
				#define ASE_SRP_VERSION 41000

				#pragma vertex Vert
				#pragma fragment Frag
        
				

				//#define UNITY_MATERIAL_LIT
        
				#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
				#define OUTPUT_SPLIT_LIGHTING
				#endif
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Wind.hlsl"
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
                #define SHADERPASS SHADERPASS_SHADOWS
                #define USE_LEGACY_UNITY_MATRIX_VARIABLES
        
			    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
        
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
				struct AttributesMesh 
				{
					float3 positionOS : POSITION;
					float3 normalOS : NORMAL;
					
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif 
				};
        
				struct PackedVaryingsMeshToPS 
				{
					float4 positionCS : SV_Position;
					
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif
				};

				sampler2D _Waves;
				CBUFFER_START( UnityPerMaterial )
				float4 _UV1_TilexyScalezw;
				float4 _UV2_TilexyScalezw;
				float4 _FoamColor;
				float _WaveHeight;
				float _Panner_1;
				float _Panner_2;
				float _DistorsionBlend;
				float _FresnelNormal_Influence;
				float _FresnelPower;
				float _FoamThickness;
				float _RippleHardness;
				float _FadeRange;
				CBUFFER_END

				
				                
				void BuildSurfaceData(FragInputs fragInputs, inout AlphaSurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData, out float3 bentNormalWS)
				{
					ZERO_INITIALIZE(SurfaceData, surfaceData);
        
					surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
			#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
			#endif
			#ifdef _MATERIAL_FEATURE_TRANSMISSION
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
			#endif
			#ifdef _MATERIAL_FEATURE_ANISOTROPY
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
			#endif
        
			#ifdef _MATERIAL_FEATURE_IRIDESCENCE
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
			#endif
			#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
			#endif
        
			#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
					surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
			#endif
        
					float3 normalTS = float3(0.0f, 0.0f, 1.0f);
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS,doubleSidedConstants);
					bentNormalWS = surfaceData.normalWS;
					surfaceData.geomNormalWS = fragInputs.worldToTangent[2];
        
			#ifdef _HAS_REFRACTION
					if (_EnableSSRefraction)
					{
        
						surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
						surfaceDescription.Alpha = 1.0;
					}
					else
					{
						surfaceData.ior = 1.0;
						surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
						surfaceData.atDistance = 1.0;
						surfaceData.transmittanceMask = 0.0;
						surfaceDescription.Alpha = 1.0;
					}
			#else
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
		 #endif
        
					surfaceData.tangentWS = normalize(fragInputs.worldToTangent[0].xyz);    // The tangent is not normalize in worldToTangent for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
					surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
			#if defined(_SPECULAR_OCCLUSION_CUSTOM)
			#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
			#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
			#else
					surfaceData.specularOcclusion = 1.0;
			#endif
			#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
					surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.worldToTangent[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
			#endif
        
				}
        
            void GetSurfaceAndBuiltinData(AlphaSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
            {
        #ifdef LOD_FADE_CROSSFADE
                uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx));
                LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
        #endif
    
		#ifdef _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
		#endif
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData, bentNormalWS);
        
        #if HAVE_DECALS
				if (_EnableDecals)
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData (posInput, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData (decalSurfaceData, surfaceData);
				}
        #endif
        
                InitBuiltinData(surfaceDescription.Alpha, bentNormalWS, -fragInputs.worldToTangent[2], fragInputs.positionRWS, fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
                builtinData.depthOffset = 0.0;
        
        #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
        #else
                builtinData.distortion = float2(0.0, 0.0);
                builtinData.distortionBlur = 0.0;
        #endif
        
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
            }
        
			PackedVaryingsMeshToPS Vert(AttributesMesh inputMesh  )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;
				
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);

				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				float2 panner356 = ( ( _Time.y * 0.35 ) * float2( 1,-1 ) + ((ase_worldPos).xz*0.2 + 0.0));
				float3 appendResult367 = (float3(0.0 , ( ( tex2Dlod( _Waves, float4( panner356, 0, 1.0) ).r - 0.5 ) * _WaveHeight ) , 0.0));
				
				float3 vertexValue = appendResult367;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif 
				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS.xyz);
				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				return outputPackedVaryingsMeshToPS;
			}

			void Frag(  PackedVaryingsMeshToPS packedInput
						#ifdef WRITE_NORMAL_BUFFER
						, out float4 outNormalBuffer : SV_Target0
							#ifdef WRITE_MSAA_DEPTH
						, out float1 depthColor : SV_Target1
							#endif
						#endif

						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
						#endif
						
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.worldToTangent = k_identity3x3;
				input.positionSS = packedInput.positionCS;
			
				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);
				float3 V = float3(1.0, 1.0, 1.0);

				SurfaceData surfaceData;
				BuiltinData builtinData;
				AlphaSurfaceDescription surfaceDescription = (AlphaSurfaceDescription)0;
				
				surfaceDescription.Alpha = 1;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = 0;
				#endif

				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

			#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
			#endif

			#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), posInput.positionSS, outNormalBuffer);
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
			#endif
			}

            ENDHLSL
        }

			
		Pass
        {
			
            Name "SceneSelectionPass"
            Tags { "LightMode"="SceneSelectionPass" }
            ColorMask 0
        	
            HLSLPROGRAM
				#define _DECALS 1
				#define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
				#define _ENABLE_FOG_ON_TRANSPARENT 1
				#define _SURFACE_TYPE_TRANSPARENT 1
				#define _BLENDMODE_ALPHA 1
				#define ASE_SRP_VERSION 41000

				#pragma vertex Vert
				#pragma fragment Frag
        
				

				//#define UNITY_MATERIAL_LIT
        
				#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
				#define OUTPUT_SPLIT_LIGHTING
				#endif
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Wind.hlsl"
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
            
                #define SHADERPASS SHADERPASS_DEPTH_ONLY
                #define SCENESELECTIONPASS
        
			    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
				int _ObjectId;
				int _PassValue;
        
				struct AttributesMesh 
				{
					float3 positionOS : POSITION;
					float3 normalOS : NORMAL;
					
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif
				};
        
				struct PackedVaryingsMeshToPS 
				{
					float4 positionCS : SV_Position;
					
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif
				};

				sampler2D _Waves;
				CBUFFER_START( UnityPerMaterial )
				float4 _UV1_TilexyScalezw;
				float4 _UV2_TilexyScalezw;
				float4 _FoamColor;
				float _WaveHeight;
				float _Panner_1;
				float _Panner_2;
				float _DistorsionBlend;
				float _FresnelNormal_Influence;
				float _FresnelPower;
				float _FoamThickness;
				float _RippleHardness;
				float _FadeRange;
				CBUFFER_END


				        
				void BuildSurfaceData(FragInputs fragInputs, inout AlphaSurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData, out float3 bentNormalWS)
				{
					ZERO_INITIALIZE(SurfaceData, surfaceData);

					surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
			#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
			#endif
			#ifdef _MATERIAL_FEATURE_TRANSMISSION
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
			#endif
			#ifdef _MATERIAL_FEATURE_ANISOTROPY
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
			#endif
        
			#ifdef _MATERIAL_FEATURE_IRIDESCENCE
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
			#endif
			#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
			#endif
        
			#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
					surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
			#endif
        
					float3 normalTS = float3(0.0f, 0.0f, 1.0f);
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS,doubleSidedConstants);
        
					bentNormalWS = surfaceData.normalWS;
					surfaceData.geomNormalWS = fragInputs.worldToTangent[2];
        
			#ifdef _HAS_REFRACTION
					if (_EnableSSRefraction)
					{
        
						surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
						surfaceDescription.Alpha = 1.0;
					}
					else
					{
						surfaceData.ior = 1.0;
						surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
						surfaceData.atDistance = 1.0;
						surfaceData.transmittanceMask = 0.0;
						surfaceDescription.Alpha = 1.0;
					}
			#else
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
			#endif
        
					surfaceData.tangentWS = normalize(fragInputs.worldToTangent[0].xyz);    // The tangent is not normalize in worldToTangent for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
					surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
			#if defined(_SPECULAR_OCCLUSION_CUSTOM)
			#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
			#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
			#else
					surfaceData.specularOcclusion = 1.0;
			#endif
			#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
					surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.worldToTangent[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
			#endif

				}
        
            void GetSurfaceAndBuiltinData(AlphaSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
            {
        #ifdef LOD_FADE_CROSSFADE 
                uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx));
                LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
        #endif
        
		#ifdef _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
		#endif

                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData, bentNormalWS);
        
        #if HAVE_DECALS
				if (_EnableDecals)
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData (posInput, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData (decalSurfaceData, surfaceData);
				}
        #endif
        
                InitBuiltinData(surfaceDescription.Alpha, bentNormalWS, -fragInputs.worldToTangent[2], fragInputs.positionRWS, fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
        
                builtinData.depthOffset = 0.0;
        
        #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
        #else
                builtinData.distortion = float2(0.0, 0.0);
                builtinData.distortionBlur = 0.0;
        #endif
        
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
            }
        
			PackedVaryingsMeshToPS Vert(AttributesMesh inputMesh  )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;
				
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);

				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				float2 panner356 = ( ( _Time.y * 0.35 ) * float2( 1,-1 ) + ((ase_worldPos).xz*0.2 + 0.0));
				float3 appendResult367 = (float3(0.0 , ( ( tex2Dlod( _Waves, float4( panner356, 0, 1.0) ).r - 0.5 ) * _WaveHeight ) , 0.0));
				
				float3 vertexValue = appendResult367;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif 
				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				return outputPackedVaryingsMeshToPS;
			}

			void Frag(  PackedVaryingsMeshToPS packedInput
						#ifdef WRITE_NORMAL_BUFFER
						, out float4 outNormalBuffer : SV_Target0
							#ifdef WRITE_MSAA_DEPTH
						, out float1 depthColor : SV_Target1
							#endif
						#elif defined(SCENESELECTIONPASS)
						, out float4 outColor : SV_Target0
						#endif

						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
						#endif
						
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
				input.worldToTangent = k_identity3x3;
				input.positionSS = packedInput.positionCS;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = float3(1.0, 1.0, 1.0); // Avoid the division by 0

				SurfaceData surfaceData;
				BuiltinData builtinData;
				AlphaSurfaceDescription surfaceDescription = (AlphaSurfaceDescription)0;
				
				surfaceDescription.Alpha = 1;
				
				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = 0;
				#endif

				GetSurfaceAndBuiltinData(surfaceDescription,input, V, posInput, surfaceData, builtinData);

			#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
			#endif

			#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), posInput.positionSS, outNormalBuffer);
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
			#elif defined(SCENESELECTIONPASS)
				outColor = float4(_ObjectId, _PassValue, 1.0, 1.0);
			#endif
			}

            ENDHLSL
        } 

		
        Pass
        {
			
            Name "DepthOnly"
            Tags { "LightMode"="DepthOnly" }
        
            HLSLPROGRAM
				#define _DECALS 1
				#define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
				#define _ENABLE_FOG_ON_TRANSPARENT 1
				#define _SURFACE_TYPE_TRANSPARENT 1
				#define _BLENDMODE_ALPHA 1
				#define ASE_SRP_VERSION 41000

				#pragma vertex Vert
				#pragma fragment Frag
        
				

				//#define UNITY_MATERIAL_LIT
        
				#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
				#define OUTPUT_SPLIT_LIGHTING
				#endif
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Wind.hlsl"
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
                #define SHADERPASS SHADERPASS_DEPTH_ONLY
                #pragma multi_compile _ WRITE_NORMAL_BUFFER
                #pragma multi_compile _ WRITE_MSAA_DEPTH
        
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define ATTRIBUTES_NEED_TEXCOORD1
                #define ATTRIBUTES_NEED_TEXCOORD2
                #define ATTRIBUTES_NEED_TEXCOORD3
                #define ATTRIBUTES_NEED_COLOR
                #define VARYINGS_NEED_POSITION_WS
                #define VARYINGS_NEED_TANGENT_TO_WORLD
                #define VARYINGS_NEED_TEXCOORD0
                #define VARYINGS_NEED_TEXCOORD1
                #define VARYINGS_NEED_TEXCOORD2
                #define VARYINGS_NEED_TEXCOORD3
                #define VARYINGS_NEED_COLOR
        
        
			    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
				struct AttributesMesh 
				{
					float3 positionOS : POSITION;
					float3 normalOS : NORMAL;
					float4 tangentOS : TANGENT;
					float4 uv0 : TEXCOORD0;
					float4 uv1 : TEXCOORD1;
					float4 uv2 : TEXCOORD2;
					float4 uv3 : TEXCOORD3;
					float4 color : COLOR;
					
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif
				};
        
				struct PackedVaryingsMeshToPS 
				{
					float4 positionCS : SV_Position;
					float3 interp00 : TEXCOORD0;
					float3 interp01 : TEXCOORD1;
					float4 interp02 : TEXCOORD2;
					float4 interp03 : TEXCOORD3;
					float4 interp04 : TEXCOORD4;
					float4 interp05 : TEXCOORD5;
					float4 interp06 : TEXCOORD6;
					float4 interp07 : TEXCOORD7;
					
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif
				};
      
				sampler2D _Waves;
				CBUFFER_START( UnityPerMaterial )
				float4 _UV1_TilexyScalezw;
				float4 _UV2_TilexyScalezw;
				float4 _FoamColor;
				float _WaveHeight;
				float _Panner_1;
				float _Panner_2;
				float _DistorsionBlend;
				float _FresnelNormal_Influence;
				float _FresnelPower;
				float _FoamThickness;
				float _RippleHardness;
				float _FadeRange;
				CBUFFER_END


				                    
				void BuildSurfaceData(FragInputs fragInputs, inout SmoothSurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData, out float3 bentNormalWS)
				{
					ZERO_INITIALIZE(SurfaceData, surfaceData);
					surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                
					surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
			#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
			#endif
			#ifdef _MATERIAL_FEATURE_TRANSMISSION
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
			#endif
			#ifdef _MATERIAL_FEATURE_ANISOTROPY
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
			#endif
        
			#ifdef _MATERIAL_FEATURE_IRIDESCENCE
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
			#endif
			#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
			#endif
        
			#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
					surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
			#endif
        
					float3 normalTS = float3(0.0f, 0.0f, 1.0f);
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS,doubleSidedConstants);
					bentNormalWS = surfaceData.normalWS;
					surfaceData.geomNormalWS = fragInputs.worldToTangent[2];

			#ifdef _HAS_REFRACTION
					surfaceData.transmittanceMask = 1.0 - surfaceDescription.Alpha;
					surfaceDescription.Alpha = 1.0;
			#endif
        
					surfaceData.tangentWS = normalize(fragInputs.worldToTangent[0].xyz);    // The tangent is not normalize in worldToTangent for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
					surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
			#if defined(_SPECULAR_OCCLUSION_CUSTOM)
			#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
			#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
			#else
					surfaceData.specularOcclusion = 1.0;
			#endif
			#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
					surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.worldToTangent[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
			#endif
        
				}
        
            void GetSurfaceAndBuiltinData(SmoothSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
            {
        #ifdef LOD_FADE_CROSSFADE
                uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx)); 
                LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
        #endif
        
		#ifdef _ALPHATEST_ON
				DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
		#endif

                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData, bentNormalWS);
        
        #if HAVE_DECALS
				if (_EnableDecals)
				{
					DecalSurfaceData decalSurfaceData = GetDecalSurfaceData (posInput, surfaceDescription.Alpha);
					ApplyDecalToSurfaceData (decalSurfaceData, surfaceData);
				}
        #endif
        
                InitBuiltinData(surfaceDescription.Alpha, bentNormalWS, -fragInputs.worldToTangent[2], fragInputs.positionRWS, fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
                builtinData.depthOffset = 0.0;
        
        #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
        #else
                builtinData.distortion = float2(0.0, 0.0);
                builtinData.distortionBlur = 0.0;
        #endif
        
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
            }
        
			PackedVaryingsMeshToPS Vert(AttributesMesh inputMesh )
			{
				PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;
				
				UNITY_SETUP_INSTANCE_ID(inputMesh);
				UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);
				
				float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
				float2 panner356 = ( ( _Time.y * 0.35 ) * float2( 1,-1 ) + ((ase_worldPos).xz*0.2 + 0.0));
				float3 appendResult367 = (float3(0.0 , ( ( tex2Dlod( _Waves, float4( panner356, 0, 1.0) ).r - 0.5 ) * _WaveHeight ) , 0.0));
				
				float3 vertexValue = appendResult367;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
				inputMesh.positionOS.xyz = vertexValue;
				#else
				inputMesh.positionOS.xyz += vertexValue;
				#endif 

				inputMesh.normalOS =  inputMesh.normalOS ;

				float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
				float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
				float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

				outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
				outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
				outputPackedVaryingsMeshToPS.interp01.xyz = normalWS;
				outputPackedVaryingsMeshToPS.interp02.xyzw = tangentWS;
				outputPackedVaryingsMeshToPS.interp03.xyzw = inputMesh.uv0;
				outputPackedVaryingsMeshToPS.interp04.xyzw = inputMesh.uv1;
				outputPackedVaryingsMeshToPS.interp05.xyzw = inputMesh.uv2;
				outputPackedVaryingsMeshToPS.interp06.xyzw = inputMesh.uv3;
				outputPackedVaryingsMeshToPS.interp07.xyzw = inputMesh.color;
				
				return outputPackedVaryingsMeshToPS;
			}

			void Frag(  PackedVaryingsMeshToPS packedInput
						#ifdef WRITE_NORMAL_BUFFER
						, out float4 outNormalBuffer : SV_Target0
							#ifdef WRITE_MSAA_DEPTH
						, out float1 depthColor : SV_Target1
							#endif
						#endif

						#ifdef _DEPTHOFFSET_ON
						, out float outputDepth : SV_Depth
						#endif
						
					)
			{
				UNITY_SETUP_INSTANCE_ID( packedInput );

				float3 positionRWS  = packedInput.interp00.xyz;
				float3 normalWS = packedInput.interp01.xyz;
				float4 tangentWS = packedInput.interp02.xyzw;
				float4 texCoord0 = packedInput.interp03.xyzw;
				float4 texCoord1 = packedInput.interp04.xyzw;
				float4 texCoord2 = packedInput.interp05.xyzw;
				float4 texCoord3 = packedInput.interp06.xyzw;
				float4 vertexColor = packedInput.interp07.xyzw;
		
					
				FragInputs input;
				ZERO_INITIALIZE(FragInputs, input);
        
				input.worldToTangent = k_identity3x3;
				input.positionSS = packedInput.positionCS;
        
				input.positionRWS = positionRWS;
				input.worldToTangent = BuildWorldToTangent(tangentWS, normalWS);
				input.texCoord0 = texCoord0;
				input.texCoord1 = texCoord1;
				input.texCoord2 = texCoord2;
				input.texCoord3 = texCoord3;
				input.color = vertexColor;

				PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

				float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);

				SurfaceData surfaceData;
				BuiltinData builtinData;
				SmoothSurfaceDescription surfaceDescription = (SmoothSurfaceDescription)0;
				
				surfaceDescription.Smoothness = 1;
				surfaceDescription.Alpha = 1;

				#ifdef _ALPHATEST_ON
				surfaceDescription.AlphaClipThreshold = 0;
				#endif

				GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

			#ifdef _DEPTHOFFSET_ON
				outputDepth = posInput.deviceDepth;
			#endif

			#ifdef WRITE_NORMAL_BUFFER
				EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), posInput.positionSS, outNormalBuffer);
				#ifdef WRITE_MSAA_DEPTH
				depthColor = packedInput.positionCS.z;
				#endif
			#endif
			}

            ENDHLSL
        }

		
		Pass
        {
			
            Name "Motion Vectors"
            Tags { "LightMode"="MotionVectors" }
			Stencil
			{
				Ref 128
				WriteMask 128
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}

        
            HLSLPROGRAM
				#define _DECALS 1
				#define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
				#define _ENABLE_FOG_ON_TRANSPARENT 1
				#define _SURFACE_TYPE_TRANSPARENT 1
				#define _BLENDMODE_ALPHA 1
				#define ASE_SRP_VERSION 41000

				#pragma vertex Vert
				#pragma fragment Frag

				
        
				//#define UNITY_MATERIAL_LIT
        
				#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
				#define OUTPUT_SPLIT_LIGHTING
				#endif
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Wind.hlsl"
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
                #define SHADERPASS SHADERPASS_VELOCITY
                #pragma multi_compile _ WRITE_NORMAL_BUFFER
                #pragma multi_compile _ WRITE_MSAA_DEPTH
        
                #define VARYINGS_NEED_POSITION_WS
        
			    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        		
				struct AttributesMesh 
				{
					float3 positionOS : POSITION;
					float3 normalOS : NORMAL;
					
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif
				};
        
				struct VaryingsMeshToPS 
				{
					float4 positionCS : SV_Position;
					float3 positionRWS;
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif 
				};

				struct AttributesPass
				{
					float3 previousPositionOS : TEXCOORD4;
				};

				struct VaryingsPassToPS
				{
					float4 positionCS;
					float4 previousPositionCS;
				};

				#define VARYINGS_NEED_PASS
				struct VaryingsToPS
				{
					VaryingsMeshToPS vmesh;
					VaryingsPassToPS vpass;
				};

				struct PackedVaryingsToPS
				{
					float4 vmeshPositionCS : SV_Position;
					float3 vmeshInterp00 : TEXCOORD0; 
					float3 vpassInterpolators0 : TEXCOORD1;
					float3 vpassInterpolators1 : TEXCOORD2;
					
					#if INSTANCING_ON
					uint vmeshInstanceID : INSTANCEID_SEMANTIC; 
					#endif 
				};

				sampler2D _Waves;
				CBUFFER_START( UnityPerMaterial )
				float4 _UV1_TilexyScalezw;
				float4 _UV2_TilexyScalezw;
				float4 _FoamColor;
				float _WaveHeight;
				float _Panner_1;
				float _Panner_2;
				float _DistorsionBlend;
				float _FresnelNormal_Influence;
				float _FresnelPower;
				float _FoamThickness;
				float _RippleHardness;
				float _FadeRange;
				CBUFFER_END


				        
				void BuildSurfaceData(FragInputs fragInputs, inout SmoothSurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData, out float3 bentNormalWS)
				{
					ZERO_INITIALIZE(SurfaceData, surfaceData);
        
					surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
                
					surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
			#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
			#endif
			#ifdef _MATERIAL_FEATURE_TRANSMISSION
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
			#endif
			#ifdef _MATERIAL_FEATURE_ANISOTROPY
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
			#endif
        
			#ifdef _MATERIAL_FEATURE_IRIDESCENCE
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
			#endif
			#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
			#endif
        
			#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
					surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
			#endif
        
					float3 normalTS = float3(0.0f, 0.0f, 1.0f);
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS,doubleSidedConstants);
					bentNormalWS = surfaceData.normalWS;
					surfaceData.geomNormalWS = fragInputs.worldToTangent[2];

			#ifdef _HAS_REFRACTION
					if (_EnableSSRefraction)
					{
        
						surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
						surfaceDescription.Alpha = 1.0;
					}
					else
					{
						surfaceData.ior = 1.0;
						surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
						surfaceData.atDistance = 1.0;
						surfaceData.transmittanceMask = 0.0;
						surfaceDescription.Alpha = 1.0;
					}
			#else
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
			#endif
        
					surfaceData.tangentWS = normalize(fragInputs.worldToTangent[0].xyz);    // The tangent is not normalize in worldToTangent for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
					surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
			#if defined(_SPECULAR_OCCLUSION_CUSTOM)
			#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
			#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
			#else
					surfaceData.specularOcclusion = 1.0;
			#endif
			#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
					surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.worldToTangent[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
			#endif

				}
        
				void GetSurfaceAndBuiltinData( SmoothSurfaceDescription surfaceDescription, FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
				{
			#ifdef LOD_FADE_CROSSFADE
					uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx));
					LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
			#endif
        
			#ifdef _ALPHATEST_ON
					DoAlphaTest ( surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold );
			#endif
					float3 bentNormalWS;
					BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData, bentNormalWS);
        
			#if HAVE_DECALS
					if (_EnableDecals)
					{
						DecalSurfaceData decalSurfaceData = GetDecalSurfaceData (posInput, surfaceDescription.Alpha);
						ApplyDecalToSurfaceData (decalSurfaceData, surfaceData);
					}
			#endif
        
					InitBuiltinData(surfaceDescription.Alpha, bentNormalWS, -fragInputs.worldToTangent[2], fragInputs.positionRWS, fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
					builtinData.depthOffset = 0.0;
        
			#if (SHADERPASS == SHADERPASS_DISTORTION)
					builtinData.distortion = surfaceDescription.Distortion;
					builtinData.distortionBlur = surfaceDescription.DistortionBlur;
			#else
					builtinData.distortion = float2(0.0, 0.0);
					builtinData.distortionBlur = 0.0;
			#endif
        
					PostInitBuiltinData(V, posInput, surfaceData, builtinData);
				}
        
				VaryingsPassToPS UnpackVaryingsPassToPS(PackedVaryingsToPS input)
				{
					VaryingsPassToPS output;
					output.positionCS = float4(input.vpassInterpolators0.xy, 0.0, input.vpassInterpolators0.z);
					output.previousPositionCS = float4(input.vpassInterpolators1.xy, 0.0, input.vpassInterpolators1.z);

					return output;
				}

				float3 TransformPreviousObjectToWorldNormal(float3 normalOS)
				{
				#ifdef UNITY_ASSUME_UNIFORM_SCALING
					return normalize(mul((float3x3)unity_MatrixPreviousM, normalOS));
				#else
					return normalize(mul(normalOS, (float3x3)unity_MatrixPreviousMI));
				#endif
				}

				float3 TransformPreviousObjectToWorld(float3 positionOS)
				{
					float4x4 previousModelMatrix = ApplyCameraTranslationToMatrix(unity_MatrixPreviousM);
					return mul(previousModelMatrix, float4(positionOS, 1.0)).xyz;
				}

				void VelocityPositionZBias(VaryingsToPS input)
				{
				#if defined(UNITY_REVERSED_Z)
					input.vmesh.positionCS.z -= unity_MotionVectorsParams.z * input.vmesh.positionCS.w;
				#else
					input.vmesh.positionCS.z += unity_MotionVectorsParams.z * input.vmesh.positionCS.w;
				#endif
				}

				PackedVaryingsToPS Vert(AttributesMesh inputMesh,
										AttributesPass inputPass
										 )
				{
					PackedVaryingsToPS outputPackedVaryingsToPS;
					VaryingsToPS varyingsType;
					VaryingsMeshToPS outputVaryingsMeshToPS;

					UNITY_SETUP_INSTANCE_ID(inputMesh);
					UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputVaryingsMeshToPS);

					float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
					float2 panner356 = ( ( _Time.y * 0.35 ) * float2( 1,-1 ) + ((ase_worldPos).xz*0.2 + 0.0));
					float3 appendResult367 = (float3(0.0 , ( ( tex2Dlod( _Waves, float4( panner356, 0, 1.0) ).r - 0.5 ) * _WaveHeight ) , 0.0));
					
					float3 vertexValue = appendResult367;
					#ifdef ASE_ABSOLUTE_VERTEX_POS
					inputMesh.positionOS.xyz = vertexValue;
					#else
					inputMesh.positionOS.xyz += vertexValue;
					#endif 
					inputMesh.normalOS =  inputMesh.normalOS ;

					float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);

					outputVaryingsMeshToPS.positionRWS = positionRWS;
					outputVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
					
					varyingsType.vmesh = outputVaryingsMeshToPS;

					VelocityPositionZBias(varyingsType);
					varyingsType.vpass.positionCS = mul(_NonJitteredViewProjMatrix, float4(varyingsType.vmesh.positionRWS, 1.0));

					bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
					if (forceNoMotion)
					{
						varyingsType.vpass.previousPositionCS = float4(0.0, 0.0, 0.0, 1.0);
					}
					else
					{
						bool hasDeformation = unity_MotionVectorsParams.x > 0.0; // Skin or morph target
						float3 previousPositionRWS = TransformPreviousObjectToWorld(hasDeformation ? inputPass.previousPositionOS : inputMesh.positionOS);
						varyingsType.vpass.previousPositionCS = mul(_PrevViewProjMatrix, float4(previousPositionRWS, 1.0));
					}

					outputPackedVaryingsToPS.vmeshPositionCS = varyingsType.vmesh.positionCS;
					outputPackedVaryingsToPS.vmeshInterp00.xyz = varyingsType.vmesh.positionRWS;
					#if INSTANCING_ON
					outputPackedVaryingsToPS.vmeshInstanceID = varyingsType.vmeshInstanceID;
					#endif 
					
					outputPackedVaryingsToPS.vpassInterpolators0 = float3(varyingsType.vpass.positionCS.xyw);
					outputPackedVaryingsToPS.vpassInterpolators1 = float3(varyingsType.vpass.previousPositionCS.xyw);
					return outputPackedVaryingsToPS;
				}

				void Frag(  PackedVaryingsToPS packedInput
							, out float4 outVelocity : SV_Target0
							#ifdef WRITE_NORMAL_BUFFER
							, out float4 outNormalBuffer : SV_Target1
								#ifdef WRITE_MSAA_DEPTH
								, out float1 depthColor : SV_Target2
								#endif
							#endif

							#ifdef _DEPTHOFFSET_ON
							, out float outputDepth : SV_Depth
							#endif
							
						)
				{
					
					UNITY_SETUP_INSTANCE_ID( packedInput );
					FragInputs input;
					ZERO_INITIALIZE(FragInputs, input);
					input.worldToTangent = k_identity3x3;
					input.positionSS = packedInput.vmeshPositionCS; 
					input.positionRWS = packedInput.vmeshInterp00.xyz;
					
					PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

					float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);
				
					SurfaceData surfaceData;
					BuiltinData builtinData;
					
					SmoothSurfaceDescription surfaceDescription = (SmoothSurfaceDescription)0;
                    
					surfaceDescription.Smoothness = 1;
					surfaceDescription.Alpha = 1;
					
					#ifdef _ALPHATEST_ON
					surfaceDescription.AlphaClipThreshold = 0;
                    #endif

					GetSurfaceAndBuiltinData(surfaceDescription, input, V, posInput, surfaceData, builtinData);

					VaryingsPassToPS inputPass = UnpackVaryingsPassToPS(packedInput);
				#ifdef _DEPTHOFFSET_ON
					inputPass.positionCS.w += builtinData.depthOffset;
					inputPass.previousPositionCS.w += builtinData.depthOffset;
				#endif

					float2 velocity = CalculateVelocity(inputPass.positionCS, inputPass.previousPositionCS);

					EncodeVelocity(velocity * 0.5, outVelocity);

					bool forceNoMotion = unity_MotionVectorsParams.y == 0.0;
					if (forceNoMotion)
						outVelocity = float4(0.0, 0.0, 0.0, 0.0);

				#ifdef WRITE_NORMAL_BUFFER
					EncodeIntoNormalBuffer(ConvertSurfaceDataToNormalData(surfaceData), posInput.positionSS, outNormalBuffer);

					#ifdef WRITE_MSAA_DEPTH
					depthColor = packedInput.vmeshPositionCS.z;
					#endif
				#endif

				#ifdef _DEPTHOFFSET_ON
					outputDepth = posInput.deviceDepth;
				#endif
				}

            ENDHLSL
        }

		
		Pass
        {
			
            Name "Forward"
            Tags { "LightMode"="Forward" }
			Stencil
			{
				Ref 2
				WriteMask 7
				Comp Always
				Pass Replace
				Fail Keep
				ZFail Keep
			}

        
            HLSLPROGRAM
                #define _DECALS 1
                #define _BLENDMODE_PRESERVE_SPECULAR_LIGHTING 1
                #define _ENABLE_FOG_ON_TRANSPARENT 1
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _BLENDMODE_ALPHA 1
                #define ASE_SRP_VERSION 41000
                #define REQUIRE_OPAQUE_TEXTURE 1

                #define _DECALS 1
        
				#pragma vertex Vert
				#pragma fragment Frag
				
				#define ASE_NEEDS_FRAG_RELATIVE_WORLD_POS
				#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
				#define ASE_NEEDS_FRAG_NORMAL
				#pragma shader_feature _RIPPLEFADE_ON

        
				//#define UNITY_MATERIAL_LIT
        
				#if defined(_MATERIAL_FEATURE_SUBSURFACE_SCATTERING) && !defined(_SURFACE_TYPE_TRANSPARENT)
				#define OUTPUT_SPLIT_LIGHTING
				#endif
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Wind.hlsl"
        
				#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
                #define SHADERPASS SHADERPASS_FORWARD
                #pragma multi_compile _ LIGHTMAP_ON
                #pragma multi_compile _ DIRLIGHTMAP_COMBINED
                #pragma multi_compile _ DYNAMICLIGHTMAP_ON
                #pragma multi_compile _ SHADOWS_SHADOWMASK
                #pragma multi_compile DECALS_OFF DECALS_3RT DECALS_4RT
                #define LIGHTLOOP_TILE_PASS
                #pragma multi_compile USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
                #pragma multi_compile SHADOW_LOW SHADOW_MEDIUM SHADOW_HIGH
        
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD1
                #define ATTRIBUTES_NEED_TEXCOORD2
                #define VARYINGS_NEED_POSITION_WS
                #define VARYINGS_NEED_TANGENT_TO_WORLD
                #define VARYINGS_NEED_TEXCOORD1
                #define VARYINGS_NEED_TEXCOORD2
        
        
			    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
                #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/Lighting.hlsl"
        
				#define HAS_LIGHTLOOP
        
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoopDef.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/Lit.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightLoop/LightLoop.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Decal/DecalUtilities.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Lit/LitDecalData.hlsl"
				#include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
				int _ObjectId;
				int _PassValue;
        
				//float3x3 BuildWorldToTangent(float4 tangentWS, float3 normalWS)
				//{
				//	float3 unnormalizedNormalWS = normalWS;
				//	float renormFactor = 1.0 / length(unnormalizedNormalWS);
				//	float3x3 worldToTangent = CreateWorldToTangent(unnormalizedNormalWS, tangentWS.xyz, tangentWS.w > 0.0 ? 1.0 : -1.0);
				//	worldToTangent[0] = worldToTangent[0] * renormFactor;
				//	worldToTangent[1] = worldToTangent[1] * renormFactor;
				//	worldToTangent[2] = worldToTangent[2] * renormFactor;
				//	return worldToTangent;
				//}
        
				struct AttributesMesh 
				{
					float3 positionOS : POSITION;
					float3 normalOS : NORMAL; 
					float4 tangentOS : TANGENT; 
					float4 uv1 : TEXCOORD1;
					float4 uv2 : TEXCOORD2;
					
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif
				};
        
				struct PackedVaryingsMeshToPS 
				{
					float4 positionCS : SV_Position;
					float3 interp00 : TEXCOORD0;
					float3 interp01 : TEXCOORD1;
					float4 interp02 : TEXCOORD2;
					float4 interp03 : TEXCOORD3;
					float4 interp04 : TEXCOORD4;
					float4 ase_texcoord5 : TEXCOORD5;
					float3 ase_normal : NORMAL;
					#if INSTANCING_ON
					uint instanceID : INSTANCEID_SEMANTIC;
					#endif 
				};

				sampler2D _Waves;
				sampler2D _Distorsion;
				sampler2D _FoamTexture;
				sampler2D _RenderTexture;
				float3 _CamPosition;
				float _CamSize;
				float3 _ComponentPosition;
				CBUFFER_START( UnityPerMaterial )
				float4 _UV1_TilexyScalezw;
				float4 _UV2_TilexyScalezw;
				float4 _FoamColor;
				float _WaveHeight;
				float _Panner_1;
				float _Panner_2;
				float _DistorsionBlend;
				float _FresnelNormal_Influence;
				float _FresnelPower;
				float _FoamThickness;
				float _RippleHardness;
				float _FadeRange;
				CBUFFER_END


				inline float4 ASE_ComputeGrabScreenPos( float4 pos )
				{
					#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
					#else
					float scale = 1.0;
					#endif
					float4 o = pos;
					o.y = pos.w * 0.5f;
					o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
					return o;
				}
				
				float4 ASEHDSampleSceneColor( float2 uv )
				{
					#if defined(REQUIRE_OPAQUE_TEXTURE) && defined(_SURFACE_TYPE_TRANSPARENT) && defined(SHADERPASS) && (SHADERPASS != SHADERPASS_LIGHT_TRANSPORT)
					return float4( SampleCameraColor(uv), 1.0 );
					#endif
					return float4(0.0, 0.0, 0.0, 1.0);
				}
				
                  
				void BuildSurfaceData(FragInputs fragInputs, inout GlobalSurfaceDescription surfaceDescription, float3 V, out SurfaceData surfaceData, out float3 bentNormalWS)
				{
					ZERO_INITIALIZE(SurfaceData, surfaceData);
        
					surfaceData.baseColor =                 surfaceDescription.Albedo;
					surfaceData.perceptualSmoothness =      surfaceDescription.Smoothness;
			#ifdef _SPECULAR_OCCLUSION_CUSTOM
					surfaceData.specularOcclusion =         surfaceDescription.SpecularOcclusion;
			#endif
					surfaceData.ambientOcclusion =          surfaceDescription.Occlusion;
					surfaceData.metallic =                  surfaceDescription.Metallic;
					surfaceData.coatMask =                  surfaceDescription.CoatMask;
			
			#ifdef _MATERIAL_FEATURE_IRIDESCENCE		
					surfaceData.iridescenceMask =           surfaceDescription.IridescenceMask;
					surfaceData.iridescenceThickness =      surfaceDescription.IridescenceThickness;
			#endif
					surfaceData.materialFeatures = MATERIALFEATUREFLAGS_LIT_STANDARD;
			#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SUBSURFACE_SCATTERING;
			#endif
			#ifdef _MATERIAL_FEATURE_TRANSMISSION
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_TRANSMISSION;
			#endif
			#ifdef _MATERIAL_FEATURE_ANISOTROPY
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_ANISOTROPY;
			#endif
			
			#ifdef ASE_LIT_CLEAR_COAT
				surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_CLEAR_COAT;
			#endif
			
			#ifdef _MATERIAL_FEATURE_IRIDESCENCE
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_IRIDESCENCE;
			#endif
			#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
					surfaceData.specularColor = surfaceDescription.Specular;
					surfaceData.materialFeatures |= MATERIALFEATUREFLAGS_LIT_SPECULAR_COLOR;
			#endif
        
			#if defined (_MATERIAL_FEATURE_SPECULAR_COLOR) && defined (_ENERGY_CONSERVING_SPECULAR)
					surfaceData.baseColor *= (1.0 - Max3(surfaceData.specularColor.r, surfaceData.specularColor.g, surfaceData.specularColor.b));
			#endif
        
					float3 normalTS = float3(0.0f, 0.0f, 1.0f);
					normalTS = surfaceDescription.Normal;
					float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
					GetNormalWS(fragInputs, normalTS, surfaceData.normalWS,doubleSidedConstants);
        
					bentNormalWS = surfaceData.normalWS;
					surfaceData.geomNormalWS = fragInputs.worldToTangent[2];

			#ifdef ASE_BENT_NORMAL
					GetNormalWS(fragInputs, surfaceDescription.BentNormal, bentNormalWS,doubleSidedConstants);
			#endif
        
			#ifdef _HAS_REFRACTION
					if (_EnableSSRefraction)
					{
						surfaceData.ior =                       surfaceDescription.RefractionIndex;
						surfaceData.transmittanceColor =        surfaceDescription.RefractionColor;
						surfaceData.atDistance =                surfaceDescription.RefractionDistance;
        
						surfaceData.transmittanceMask = (1.0 - surfaceDescription.Alpha);
						surfaceDescription.Alpha = 1.0;
					}
					else
					{
						surfaceData.ior = 1.0;
						surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
						surfaceData.atDistance = 1.0;
						surfaceData.transmittanceMask = 0.0;
						surfaceDescription.Alpha = 1.0;
					}
			#else
					surfaceData.ior = 1.0;
					surfaceData.transmittanceColor = float3(1.0, 1.0, 1.0);
					surfaceData.atDistance = 1.0;
					surfaceData.transmittanceMask = 0.0;
			#endif
        
			#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
					surfaceData.thickness =	                 surfaceDescription.Thickness;
			#endif

			#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceData.subsurfaceMask =            surfaceDescription.SubsurfaceMask;
			#endif

			#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
					surfaceData.diffusionProfile = surfaceDescription.DiffusionProfile;
			#endif
					surfaceData.tangentWS = normalize(fragInputs.worldToTangent[0].xyz);    // The tangent is not normalize in worldToTangent for mikkt. TODO: Check if it expected that we normalize with Morten. Tag: SURFACE_GRADIENT
			#ifdef _MATERIAL_FEATURE_ANISOTROPY
					surfaceData.anisotropy = surfaceDescription.Anisotropy;
					surfaceData.tangentWS = TransformTangentToWorld(surfaceDescription.Tangent, fragInputs.worldToTangent);
			#endif
					surfaceData.tangentWS = Orthonormalize(surfaceData.tangentWS, surfaceData.normalWS);
        
			#if defined(_SPECULAR_OCCLUSION_CUSTOM)
			#elif defined(_SPECULAR_OCCLUSION_FROM_AO_BENT_NORMAL)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromBentAO(V, bentNormalWS, surfaceData.normalWS, surfaceData.ambientOcclusion, PerceptualSmoothnessToPerceptualRoughness(surfaceData.perceptualSmoothness));
			#elif defined(_AMBIENT_OCCLUSION) && defined(_SPECULAR_OCCLUSION_FROM_AO)
					surfaceData.specularOcclusion = GetSpecularOcclusionFromAmbientOcclusion(ClampNdotV(dot(surfaceData.normalWS, V)), surfaceData.ambientOcclusion, PerceptualSmoothnessToRoughness(surfaceData.perceptualSmoothness));
			#else
					surfaceData.specularOcclusion = 1.0;
			#endif
			#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
					surfaceData.perceptualSmoothness = GeometricNormalFiltering(surfaceData.perceptualSmoothness, fragInputs.worldToTangent[2], surfaceDescription.SpecularAAScreenSpaceVariance, surfaceDescription.SpecularAAThreshold);
			#endif
        
				}
        
				void GetSurfaceAndBuiltinData(GlobalSurfaceDescription surfaceDescription,FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
				{
			#ifdef LOD_FADE_CROSSFADE
					uint3 fadeMaskSeed = asuint((int3)(V * _ScreenSize.xyx));
					LODDitheringTransition(fadeMaskSeed, unity_LODFade.x);
			#endif
        
			#ifdef _ALPHATEST_ON
						DoAlphaTest(surfaceDescription.Alpha, surfaceDescription.AlphaClipThreshold);
			#endif
        
					float3 bentNormalWS;
					BuildSurfaceData(fragInputs, surfaceDescription, V, surfaceData, bentNormalWS);
        
			#if HAVE_DECALS
					if (_EnableDecals)
					{
						DecalSurfaceData decalSurfaceData = GetDecalSurfaceData (posInput, surfaceDescription.Alpha);
						ApplyDecalToSurfaceData (decalSurfaceData, surfaceData);
					}
			#endif
        
					InitBuiltinData(surfaceDescription.Alpha, bentNormalWS, -fragInputs.worldToTangent[2], fragInputs.positionRWS, fragInputs.texCoord1, fragInputs.texCoord2, builtinData);
        
					builtinData.emissiveColor = surfaceDescription.Emission;
        
					builtinData.depthOffset = 0.0;
        
			#if (SHADERPASS == SHADERPASS_DISTORTION)
					builtinData.distortion = surfaceDescription.Distortion;
					builtinData.distortionBlur = surfaceDescription.DistortionBlur;
			#else
					builtinData.distortion = float2(0.0, 0.0);
					builtinData.distortionBlur = 0.0;
			#endif
        
					PostInitBuiltinData(V, posInput, surfaceData, builtinData);
				}
    
				PackedVaryingsMeshToPS Vert(AttributesMesh inputMesh )
				{
				
					PackedVaryingsMeshToPS outputPackedVaryingsMeshToPS;

					UNITY_SETUP_INSTANCE_ID(inputMesh);
					UNITY_TRANSFER_INSTANCE_ID(inputMesh, outputPackedVaryingsMeshToPS);

					float3 ase_worldPos = GetAbsolutePositionWS( TransformObjectToWorld( (inputMesh.positionOS).xyz ) );
					float2 panner356 = ( ( _Time.y * 0.35 ) * float2( 1,-1 ) + ((ase_worldPos).xz*0.2 + 0.0));
					float3 appendResult367 = (float3(0.0 , ( ( tex2Dlod( _Waves, float4( panner356, 0, 1.0) ).r - 0.5 ) * _WaveHeight ) , 0.0));
					
					float4 ase_clipPos = TransformWorldToHClip( TransformObjectToWorld(inputMesh.positionOS));
					float4 screenPos = ComputeScreenPos( ase_clipPos , _ProjectionParams.x );
					outputPackedVaryingsMeshToPS.ase_texcoord5 = screenPos;
					
					outputPackedVaryingsMeshToPS.ase_normal = inputMesh.normalOS;
					float3 vertexValue = appendResult367;
					#ifdef ASE_ABSOLUTE_VERTEX_POS
					inputMesh.positionOS.xyz = vertexValue;
					#else
					inputMesh.positionOS.xyz += vertexValue;
					#endif
					inputMesh.normalOS =  inputMesh.normalOS ;

					float3 positionRWS = TransformObjectToWorld(inputMesh.positionOS);
					float3 normalWS = TransformObjectToWorldNormal(inputMesh.normalOS);
					float4 tangentWS = float4(TransformObjectToWorldDir(inputMesh.tangentOS.xyz), inputMesh.tangentOS.w);

					outputPackedVaryingsMeshToPS.positionCS = TransformWorldToHClip(positionRWS);
					outputPackedVaryingsMeshToPS.interp00.xyz = positionRWS;
					outputPackedVaryingsMeshToPS.interp01.xyz = normalWS;
					outputPackedVaryingsMeshToPS.interp02.xyzw = tangentWS;
					outputPackedVaryingsMeshToPS.interp03.xyzw = inputMesh.uv1;
					outputPackedVaryingsMeshToPS.interp04.xyzw = inputMesh.uv2;
				
					return outputPackedVaryingsMeshToPS;
				}

				void Frag(PackedVaryingsMeshToPS packedInput,
						#ifdef OUTPUT_SPLIT_LIGHTING
							out float4 outColor : SV_Target0,
							out float4 outDiffuseLighting : SV_Target1,
							OUTPUT_SSSBUFFER(outSSSBuffer)
						#else
							out float4 outColor : SV_Target0
						#endif
						#ifdef _DEPTHOFFSET_ON
							, out float outputDepth : SV_Depth
						#endif
						
						  )
				{
					UNITY_SETUP_INSTANCE_ID( packedInput );
					float3 positionRWS = packedInput.interp00.xyz;
					float3 normalWS = packedInput.interp01.xyz;
					float4 tangentWS = packedInput.interp02.xyzw;
				
					FragInputs input;
					ZERO_INITIALIZE(FragInputs, input);
					input.worldToTangent = k_identity3x3;
					input.positionSS = packedInput.positionCS;
					input.positionRWS = positionRWS;
					input.worldToTangent = BuildWorldToTangent(tangentWS, normalWS);
					input.texCoord1 = packedInput.interp03.xyzw;
					input.texCoord2 = packedInput.interp04.xyzw;
                
					uint2 tileIndex = uint2(input.positionSS.xy) / GetTileSize ();
				#if defined(UNITY_SINGLE_PASS_STEREO)
					tileIndex.x -= unity_StereoEyeIndex * _NumTileClusteredX;
				#endif
					PositionInputs posInput = GetPositionInput_Stereo(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS.xyz, tileIndex , unity_StereoEyeIndex);

					float3 normalizedWorldViewDir = GetWorldSpaceNormalizeViewDir(input.positionRWS);
			
					SurfaceData surfaceData;
					BuiltinData builtinData;
					GlobalSurfaceDescription surfaceDescription = (GlobalSurfaceDescription)0;
					float4 screenPos = packedInput.ase_texcoord5;
					float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
					float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
					float2 appendResult6 = (float2(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g));
					float3 ase_worldPos = GetAbsolutePositionWS( positionRWS );
					float2 temp_output_18_0 = (ase_worldPos).xz;
					float2 appendResult57 = (float2(_UV1_TilexyScalezw.x , _UV1_TilexyScalezw.y));
					float2 appendResult58 = (float2(_UV1_TilexyScalezw.z , _UV1_TilexyScalezw.w));
					float2 UV131 = ( ( ( temp_output_18_0 + ( _Time.x * _Panner_1 ) ) * appendResult57 ) / appendResult58 );
					float2 appendResult60 = (float2(_UV2_TilexyScalezw.x , _UV2_TilexyScalezw.y));
					float2 appendResult61 = (float2(_UV2_TilexyScalezw.z , _UV2_TilexyScalezw.w));
					float2 UV239 = ( ( ( temp_output_18_0 + ( _Time.x * _Panner_2 ) ) * appendResult60 ) / appendResult61 );
					float3 lerpResult7 = lerp( UnpackNormalmapRGorAG( tex2D( _Distorsion, UV131 ), 1.0f ) , UnpackNormalmapRGorAG( tex2D( _Distorsion, UV239 ), 1.0f ) , _DistorsionBlend);
					float3 Distorsion9 = lerpResult7;
					float4 ase_screenPosNorm = screenPos / screenPos.w;
					ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
					float screenDepth540 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
					float distanceDepth540 = abs( ( screenDepth540 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 200.0 ) );
					float clampResult543 = clamp( distanceDepth540 , 0.0 , 1.0 );
					float2 lerpResult542 = lerp( ( (Distorsion9).xy * 0.1 ) , float2( 0,0 ) , clampResult543);
					float2 ScreenUV11 = ( appendResult6 - lerpResult542 );
					float4 fetchOpaqueVal1 = ASEHDSampleSceneColor(ScreenUV11);
					float4 color65 = IsGammaSpace() ? float4(0.1076451,0.4492441,0.5566038,0) : float4(0.01125052,0.1700325,0.2702231,0);
					float2 appendResult400 = (float2(packedInput.ase_normal.x , packedInput.ase_normal.y));
					float3 appendResult404 = (float3(saturate( ( appendResult400 - (Distorsion9).xy ) ) , packedInput.ase_normal.z));
					float3 lerpResult407 = lerp( packedInput.ase_normal , appendResult404 , _FresnelNormal_Influence);
					float dotResult68 = dot( normalizedWorldViewDir , lerpResult407 );
					float temp_output_72_0 = pow( ( 1.0 - saturate( abs( dotResult68 ) ) ) , _FresnelPower );
					float4 lerpResult64 = lerp( fetchOpaqueVal1 , color65 , temp_output_72_0);
					
					float screenDepth312 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
					float distanceDepth312 = abs( ( screenDepth312 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _FoamThickness ) );
					float screenDepth302 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
					float distanceDepth302 = abs( ( screenDepth302 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 0.5 ) );
					float lerpResult319 = lerp( 0.0 , tex2D( _FoamTexture, UV131 ).r , ( 1.0 - distanceDepth302 ));
					float screenDepth309 = LinearEyeDepth(SampleCameraDepth( ase_screenPosNorm.xy ),_ZBufferParams);
					float distanceDepth309 = abs( ( screenDepth309 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( 4.0 ) );
					float lerpResult328 = lerp( 0.0 , tex2D( _FoamTexture, UV239 ).r , ( 1.0 - distanceDepth309 ));
					float smoothstepResult453 = smoothstep( _RippleHardness , ( _RippleHardness + 0.05 ) , tex2D( _RenderTexture, ( ( ( (ase_worldPos).xz - (_CamPosition).xz ) / ( _CamSize * 2.0 ) ) + float2( 0.5,0.5 ) ) ).b);
					float clampResult481 = clamp( ( distance( (ase_worldPos).xz , (_ComponentPosition).xz ) - _FadeRange ) , 0.0 , 1.0 );
					#ifdef _RIPPLEFADE_ON
					float staticSwitch500 = clampResult481;
					#else
					float staticSwitch500 = 0.0;
					#endif
					float lerpResult482 = lerp( smoothstepResult453 , 0.0 , staticSwitch500);
					float clampResult343 = clamp( ( ( ( step( 0.0 , ( 1.0 - distanceDepth312 ) ) + step( 0.35 , lerpResult319 ) ) + step( 0.65 , lerpResult328 ) ) + lerpResult482 ) , 0.0 , 1.0 );
					float4 Emission345 = ( clampResult343 * _FoamColor );
					
					surfaceDescription.Albedo = lerpResult64.rgb;
					surfaceDescription.Normal = float3( 0, 0, 1 );
					surfaceDescription.BentNormal = float3( 0, 0, 1 );
					surfaceDescription.CoatMask = 0;
					surfaceDescription.Metallic = 0;
					
					#ifdef _MATERIAL_FEATURE_SPECULAR_COLOR
					surfaceDescription.Specular = 0;
					#endif
					
					surfaceDescription.Emission = Emission345.rgb;
					surfaceDescription.Smoothness = 0.5;
					surfaceDescription.Occlusion = 1;
					surfaceDescription.Alpha = 1;
					
					#ifdef _ALPHATEST_ON
					surfaceDescription.AlphaClipThreshold = 0;
					#endif

					#ifdef _ENABLE_GEOMETRIC_SPECULAR_AA
					surfaceDescription.SpecularAAScreenSpaceVariance = 0;
					surfaceDescription.SpecularAAThreshold = 0;
					#endif

					#ifdef _SPECULAR_OCCLUSION_CUSTOM
					surfaceDescription.SpecularOcclusion = 0;
					#endif

					#if defined(_HAS_REFRACTION) || defined(_MATERIAL_FEATURE_TRANSMISSION)
					surfaceDescription.Thickness = 1;
					#endif

					#ifdef _HAS_REFRACTION
					surfaceDescription.RefractionIndex = 1;
					surfaceDescription.RefractionColor = float3(1,1,1);
					surfaceDescription.RefractionDistance = 0;
					#endif

					#ifdef _MATERIAL_FEATURE_SUBSURFACE_SCATTERING
					surfaceDescription.SubsurfaceMask = 1;
					#endif

					#if defined( _MATERIAL_FEATURE_SUBSURFACE_SCATTERING ) || defined( _MATERIAL_FEATURE_TRANSMISSION )
					surfaceDescription.DiffusionProfile = 0;
					#endif

					#ifdef _MATERIAL_FEATURE_ANISOTROPY
					surfaceDescription.Anisotropy = 1;
					surfaceDescription.Tangent = float3(1,0,0);
					#endif

					#ifdef _MATERIAL_FEATURE_IRIDESCENCE
					surfaceDescription.IridescenceMask = 0;
					surfaceDescription.IridescenceThickness = 0;
					#endif

					GetSurfaceAndBuiltinData(surfaceDescription,input, normalizedWorldViewDir, posInput, surfaceData, builtinData);

					BSDFData bsdfData = ConvertSurfaceDataToBSDFData(input.positionSS.xy, surfaceData);

					PreLightData preLightData = GetPreLightData(normalizedWorldViewDir, posInput, bsdfData);

					outColor = float4(0.0, 0.0, 0.0, 0.0);

					{
				#ifdef _SURFACE_TYPE_TRANSPARENT
						uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_TRANSPARENT;
				#else
						uint featureFlags = LIGHT_FEATURE_MASK_FLAGS_OPAQUE;
				#endif
						float3 diffuseLighting;
						float3 specularLighting;

						LightLoop(normalizedWorldViewDir, posInput, preLightData, bsdfData, builtinData, featureFlags, diffuseLighting, specularLighting);

				#ifdef OUTPUT_SPLIT_LIGHTING
						if (_EnableSubsurfaceScattering != 0 && ShouldOutputSplitLighting(bsdfData))
						{
							outColor = float4(specularLighting, 1.0);
							outDiffuseLighting = float4(TagLightingForSSS(diffuseLighting), 1.0);
						}
						else
						{
							outColor = float4(diffuseLighting + specularLighting, 1.0);
							outDiffuseLighting = 0;
						}
						ENCODE_INTO_SSSBUFFER(surfaceData, posInput.positionSS, outSSSBuffer);
				#else
						outColor = ApplyBlendMode(diffuseLighting, specularLighting, builtinData.opacity);
						outColor = EvaluateAtmosphericScattering(posInput, normalizedWorldViewDir, outColor);
				#endif
					}

				#ifdef _DEPTHOFFSET_ON
					outputDepth = posInput.deviceDepth;
				#endif
				}

            ENDHLSL
        }
		
    }
    CustomEditor "ASEMaterialInspector"   
	
	
}
/*ASEBEGIN
Version=18100
1920;0;1920;1018;1505.974;461.9072;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;552;-6013.101,-2285.896;Inherit;False;2359.133;2724.395;;3;553;554;555;UVs;1,0.93734,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;555;-5895.849,-2158.251;Inherit;False;1878.521;1015.216;Panner;22;31;39;36;28;61;27;58;35;60;19;34;57;23;59;41;56;18;52;54;62;17;22;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TimeNode;22;-5845.849,-1636.526;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;54;-5423.761,-1575.877;Inherit;False;Property;_Panner_2;Panner_2;6;0;Create;True;0;0;False;0;False;3.5;3.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;62;-5645.314,-1956.035;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;17;-5589.721,-1936.233;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;52;-5437.931,-2064.21;Inherit;False;Property;_Panner_1;Panner_1;5;0;Create;True;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;59;-5070.351,-1418.206;Inherit;False;Property;_UV2_TilexyScalezw;UV2_Tile (xy) Scale (zw);8;0;Create;True;0;0;False;0;False;0.5,0.5,5,5;0.5,0.5,5,5;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;56;-5025.426,-1958.65;Inherit;False;Property;_UV1_TilexyScalezw;UV1_Tile (xy) Scale (zw);7;0;Create;True;0;0;False;0;False;1,1,5,5;1,1,5,5;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;18;-5383.851,-1944.192;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-5214.258,-1635.674;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-5229.499,-2084.7;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;57;-4780.57,-1951.509;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-4976.928,-1662.056;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;60;-4744.484,-1424.611;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-4962.438,-2108.251;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;61;-4742.546,-1324.321;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-4590.01,-1662.531;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-4638.294,-2107.513;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;58;-4778.632,-1851.219;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;28;-4445.27,-1877.017;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;36;-4449.546,-1347.344;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;-4281.092,-1880.297;Inherit;False;UV1;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;553;-5893.895,-1093.882;Inherit;False;1249.483;628.3232;Distorsion;7;9;7;396;395;8;50;49;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;-4264.468,-1348.725;Inherit;False;UV2;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;49;-5843.895,-1022.668;Inherit;False;31;UV1;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;50;-5843.251,-801.8716;Inherit;False;39;UV2;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;396;-5546.977,-829.7816;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Instance;395;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;395;-5548.276,-1043.882;Inherit;True;Property;_Distorsion;Distorsion;4;0;Create;True;0;0;False;0;False;-1;2293bc9778902d649b431c4603b005cc;2293bc9778902d649b431c4603b005cc;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-5526.434,-581.5585;Inherit;False;Property;_DistorsionBlend;DistorsionBlend;9;0;Create;True;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;559;-6030.072,747.3865;Inherit;False;3633.816;3177.417;;8;345;348;343;349;452;556;557;558;EMISSIVE;0.6839622,1,0.9999999,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;558;-5938.466,2236.254;Inherit;False;2396.445;663.6602;Ripples Render Texture;14;474;468;467;466;459;461;460;454;463;455;422;453;482;425;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;7;-5095.435,-826.5582;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;557;-5980.072,3160.801;Inherit;False;1876.326;695.7397;(in case of attached camera);9;502;485;489;488;501;494;495;481;500;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;9;-4868.412,-829.796;Inherit;False;Distorsion;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;560;-3377.141,-183.3129;Inherit;False;2356.029;578.0673;Fresnel;17;72;71;73;70;69;68;66;407;409;408;404;405;402;400;401;67;586;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;425;-5888.466,2568.915;Inherit;True;Global;_CamPosition;_CamPosition;17;0;Create;True;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;474;-5859.622,2292.317;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ComponentMaskNode;467;-5554.273,2569.147;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector3Node;502;-5930.072,3525.54;Inherit;True;Global;_ComponentPosition;_ComponentPosition;17;0;Create;True;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;556;-5857.844,828.1348;Inherit;False;1947.083;1184.267;Foam;18;336;302;350;311;310;319;329;331;351;309;320;316;328;334;305;312;318;325;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;468;-5532.772,2662.191;Inherit;False;Global;_CamSize;_CamSize;17;0;Create;True;0;0;False;0;False;7;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;485;-5895.824,3253.583;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalVertexDataNode;67;-3320.215,96.48585;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;466;-5566.621,2286.254;Inherit;False;True;False;True;False;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;401;-3327.142,280.8814;Inherit;False;9;Distorsion;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;350;-5787.725,1145.471;Inherit;False;31;UV1;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;402;-3120.142,281.8814;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;488;-5592.22,3527.412;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;461;-5271.884,2665.948;Inherit;False;2;2;0;FLOAT;20;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;459;-5295.278,2398.115;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;489;-5609.409,3251.473;Inherit;False;True;False;True;False;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;305;-5695.976,918.5242;Float;False;Property;_FoamThickness;FoamThickness;13;0;Create;True;0;0;False;0;False;0.1;0.075;0.075;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;302;-5799.668,1385.663;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;400;-3079.142,121.8811;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;310;-5441.885,1381.023;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;554;-5886.932,-405.6126;Inherit;False;1510.107;749.9434;Screen;11;4;11;10;6;542;13;543;377;14;540;12;;1,1,1,1;0;0
Node;AmplifyShaderEditor.DepthFade;309;-5734.334,1877.401;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;501;-5201.262,3516.9;Inherit;False;Property;_FadeRange;FadeRange;16;0;Create;True;0;0;False;0;False;10;15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;460;-5051.134,2400.418;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;351;-5807.844,1641.801;Inherit;False;39;UV2;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;311;-5537.996,1120.218;Inherit;True;Property;_FoamTexture;FoamTexture;1;0;Create;True;0;0;False;0;False;-1;cd460ee4ac5c1e746b7a734cc7cc64dd;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DistanceOpNode;494;-5286.473,3358.104;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;405;-2843.141,168.8811;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DepthFade;312;-5320.718,899.524;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;454;-4382.615,2614.008;Inherit;False;Property;_RippleHardness;RippleHardness;12;0;Create;True;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;495;-4987.203,3360.914;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;586;-2775.134,304.8206;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;320;-5430.333,1877.401;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;318;-5026.615,895.9371;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;12;-5806.678,-97.06313;Inherit;False;9;Distorsion;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;561;-1888.972,1105.602;Inherit;False;2534.331;475.7617;;11;367;360;359;358;357;356;354;355;352;353;362;VERTEX DISPLACEMENT;0.780085,0.5235849,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;319;-4998.884,1225.296;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;316;-5546.595,1616.546;Inherit;True;Property;_TextureSample2;Texture Sample 2;1;0;Create;True;0;0;False;0;False;-1;cd460ee4ac5c1e746b7a734cc7cc64dd;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Instance;311;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;463;-4759.932,2397.817;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;377;-5625.251,-96.58083;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StepOpNode;329;-4753.997,1200.217;Inherit;True;2;0;FLOAT;0.35;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;455;-4170.879,2615.908;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;422;-4478.344,2370.109;Inherit;True;Property;_RenderTexture;RenderTexture;0;0;Create;True;0;0;False;0;False;-1;None;26ade252012e5e547961d0adea918209;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;481;-4723.771,3361.471;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;404;-2611.943,172.705;Inherit;False;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;408;-2649.537,-10.41843;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;362;-1838.972,1197.041;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;328;-4967.36,1607.194;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-5721.078,-1.174521;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;540;-5821.126,139.4796;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;200;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;409;-2350.809,233.2398;Inherit;False;Property;_FresnelNormal_Influence;FresnelNormal_Influence;11;0;Create;True;0;0;False;0;False;0.3;0.46;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;325;-4778.315,878.1351;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;353;-1424.774,1409.685;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;66;-2191.891,-133.3128;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClampOpNode;543;-5396.487,137.9179;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-5416.078,-96.17445;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StepOpNode;334;-4727.361,1575.194;Inherit;True;2;0;FLOAT;0.65;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;453;-4018.772,2448.847;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;331;-4283.917,1064.417;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;4;-5715.078,-310.1746;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;500;-4406.767,3325.727;Inherit;False;Property;_RippleFade;RippleFade;15;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;352;-1504.243,1189.79;Inherit;True;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;407;-2164.091,57.63477;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;542;-5199.014,-97.68721;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;-5415.078,-261.1747;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;354;-1229.72,1194.305;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;0.2;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;68;-1935.891,-69.31277;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;336;-4062.761,1543.745;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;482;-3724.022,2447.818;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;355;-1166.81,1432.343;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.35;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;69;-1727.891,-69.31277;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;356;-982.2455,1196.79;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;10;-4959.977,-258.2748;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;452;-3345.368,1556.918;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;70;-1583.892,-69.31277;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;349;-3126.219,1731.142;Inherit;False;Property;_FoamColor;FoamColor;14;0;Create;True;0;0;False;0;False;0.2509804,0.3921569,0.4823529,0;0.2509798,0.3921563,0.4823529,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;343;-3058.301,1561.132;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;11;-4652.186,-265.2103;Inherit;False;ScreenUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;357;-641.1783,1155.602;Inherit;True;Property;_Waves;Waves;2;0;Create;True;0;0;False;0;False;-1;31890676c5b178840848afa665cb5a2f;31890676c5b178840848afa665cb5a2f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;73;-1519.892,42.68716;Inherit;False;Property;_FresnelPower;FresnelPower;10;0;Create;True;0;0;False;0;False;1;1;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;15;-697.2516,-387.8269;Inherit;True;11;ScreenUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;71;-1423.891,-69.31277;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;348;-2853.875,1560.785;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;358;-310.677,1445.528;Float;False;Property;_WaveHeight;Wave Height;3;0;Create;True;0;0;False;0;False;0.2;0.2;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;359;-162.5979,1182.173;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;72;-1231.892,-69.31277;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;65;-523.7589,-182.8493;Inherit;False;Constant;_Color;Color;5;0;Create;True;0;0;False;0;False;0.1076451,0.4492441,0.5566038,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;360;125.0563,1184.593;Inherit;True;2;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;1;-386.4314,-380.5515;Inherit;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;True;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;345;-2714.815,1556.661;Float;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;585;-879.9009,83.16864;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;137;550.1243,31.5561;Inherit;False;345;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;367;454.8898,1158.846;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;64;14.98123,-142.5809;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;581;748.3954,-79.38608;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;091c43ba8bd92c9459798d59b089ce4e;True;Distortion;0;6;Distortion;2;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;True;0;False;-1;False;False;True;1;False;-1;True;3;False;-1;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;True;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;True;1;False;-1;5;False;-1;False;False;False;False;False;True;3;False;-1;False;True;1;LightMode=DistortionVectors;False;0;;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;577;748.3954,-79.38608;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;091c43ba8bd92c9459798d59b089ce4e;True;ShadowCaster;0;2;ShadowCaster;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;True;0;False;-1;False;False;True;1;False;-1;True;3;False;-1;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;True;1;LightMode=ShadowCaster;False;0;;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;582;748.3954,-79.38608;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;091c43ba8bd92c9459798d59b089ce4e;True;TransparentBackface;0;7;TransparentBackface;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;True;0;False;-1;False;False;True;1;False;-1;True;3;False;-1;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;True;1;False;-1;False;False;False;False;False;True;1;LightMode=TransparentBackface;False;0;;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;579;748.3954,-79.38608;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;091c43ba8bd92c9459798d59b089ce4e;True;DepthOnly;0;4;DepthOnly;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;True;0;False;-1;False;False;True;1;False;-1;True;3;False;-1;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=DepthOnly;False;0;;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;575;748.3954,-79.38608;Float;False;True;-1;2;ASEMaterialInspector;0;2;Water;091c43ba8bd92c9459798d59b089ce4e;True;GBuffer;0;0;GBuffer;26;True;1;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;True;2;False;-1;False;False;True;2;False;-1;True;3;False;-1;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;5;0;False;False;False;False;False;True;True;2;False;-1;255;False;-1;7;False;-1;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;True;1;LightMode=GBuffer;False;0;;0;0;Standard;18;Material Type,InvertActionOnDeselection;0;Energy Conserving Specular,InvertActionOnDeselection;0;Transmission,InvertActionOnDeselection;0;Surface Type;1;Receive Decals;1;Alpha Cutoff;0;Receives SSR;1;Specular AA;0;Specular Occlusion Mode;0;Distortion;0;Distortion Mode;0;Distortion Depth Test;0;Back Then Front Rendering;0;Blend Preserves Specular;1;Fog;1;Draw Before Refraction;0;Refraction Model;0;Vertex Position,InvertActionOnDeselection;1;0;9;True;True;True;True;True;True;False;False;True;False;;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;576;748.3954,-79.38608;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;091c43ba8bd92c9459798d59b089ce4e;True;META;0;1;META;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;True;0;False;-1;False;False;True;1;False;-1;True;3;False;-1;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;True;2;False;-1;False;False;False;False;False;True;1;LightMode=Meta;False;0;;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;578;748.3954,-79.38608;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;091c43ba8bd92c9459798d59b089ce4e;True;SceneSelectionPass;0;3;SceneSelectionPass;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;True;0;False;-1;False;False;True;1;False;-1;True;3;False;-1;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;0;;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;580;748.3954,-79.38608;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;091c43ba8bd92c9459798d59b089ce4e;True;Motion Vectors;0;5;Motion Vectors;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;True;0;False;-1;False;False;True;1;False;-1;True;3;False;-1;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;True;True;128;False;-1;255;False;-1;128;False;-1;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;True;1;LightMode=MotionVectors;False;0;;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;583;748.3954,-79.38608;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;091c43ba8bd92c9459798d59b089ce4e;True;Forward;0;8;Forward;0;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;True;0;False;-1;False;False;True;1;False;-1;True;3;False;-1;False;True;3;RenderPipeline=HDRenderPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;5;0;False;False;False;False;False;True;True;2;False;-1;255;False;-1;7;False;-1;7;False;-1;3;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;True;1;LightMode=Forward;False;0;;0;0;Standard;0;0
WireConnection;62;0;22;1
WireConnection;18;0;17;0
WireConnection;41;0;22;1
WireConnection;41;1;54;0
WireConnection;23;0;62;0
WireConnection;23;1;52;0
WireConnection;57;0;56;1
WireConnection;57;1;56;2
WireConnection;34;0;18;0
WireConnection;34;1;41;0
WireConnection;60;0;59;1
WireConnection;60;1;59;2
WireConnection;19;0;18;0
WireConnection;19;1;23;0
WireConnection;61;0;59;3
WireConnection;61;1;59;4
WireConnection;35;0;34;0
WireConnection;35;1;60;0
WireConnection;27;0;19;0
WireConnection;27;1;57;0
WireConnection;58;0;56;3
WireConnection;58;1;56;4
WireConnection;28;0;27;0
WireConnection;28;1;58;0
WireConnection;36;0;35;0
WireConnection;36;1;61;0
WireConnection;31;0;28;0
WireConnection;39;0;36;0
WireConnection;396;1;50;0
WireConnection;395;1;49;0
WireConnection;7;0;395;0
WireConnection;7;1;396;0
WireConnection;7;2;8;0
WireConnection;9;0;7;0
WireConnection;467;0;425;0
WireConnection;466;0;474;0
WireConnection;402;0;401;0
WireConnection;488;0;502;0
WireConnection;461;0;468;0
WireConnection;459;0;466;0
WireConnection;459;1;467;0
WireConnection;489;0;485;0
WireConnection;400;0;67;1
WireConnection;400;1;67;2
WireConnection;310;0;302;0
WireConnection;460;0;459;0
WireConnection;460;1;461;0
WireConnection;311;1;350;0
WireConnection;494;0;489;0
WireConnection;494;1;488;0
WireConnection;405;0;400;0
WireConnection;405;1;402;0
WireConnection;312;0;305;0
WireConnection;495;0;494;0
WireConnection;495;1;501;0
WireConnection;586;0;405;0
WireConnection;320;0;309;0
WireConnection;318;0;312;0
WireConnection;319;1;311;1
WireConnection;319;2;310;0
WireConnection;316;1;351;0
WireConnection;463;0;460;0
WireConnection;377;0;12;0
WireConnection;329;1;319;0
WireConnection;455;0;454;0
WireConnection;422;1;463;0
WireConnection;481;0;495;0
WireConnection;404;0;586;0
WireConnection;404;2;67;3
WireConnection;328;1;316;1
WireConnection;328;2;320;0
WireConnection;325;1;318;0
WireConnection;543;0;540;0
WireConnection;13;0;377;0
WireConnection;13;1;14;0
WireConnection;334;1;328;0
WireConnection;453;0;422;3
WireConnection;453;1;454;0
WireConnection;453;2;455;0
WireConnection;331;0;325;0
WireConnection;331;1;329;0
WireConnection;500;0;481;0
WireConnection;352;0;362;0
WireConnection;407;0;408;0
WireConnection;407;1;404;0
WireConnection;407;2;409;0
WireConnection;542;0;13;0
WireConnection;542;2;543;0
WireConnection;6;0;4;1
WireConnection;6;1;4;2
WireConnection;354;0;352;0
WireConnection;68;0;66;0
WireConnection;68;1;407;0
WireConnection;336;0;331;0
WireConnection;336;1;334;0
WireConnection;482;0;453;0
WireConnection;482;2;500;0
WireConnection;355;0;353;2
WireConnection;69;0;68;0
WireConnection;356;0;354;0
WireConnection;356;1;355;0
WireConnection;10;0;6;0
WireConnection;10;1;542;0
WireConnection;452;0;336;0
WireConnection;452;1;482;0
WireConnection;70;0;69;0
WireConnection;343;0;452;0
WireConnection;11;0;10;0
WireConnection;357;1;356;0
WireConnection;71;0;70;0
WireConnection;348;0;343;0
WireConnection;348;1;349;0
WireConnection;359;0;357;1
WireConnection;72;0;71;0
WireConnection;72;1;73;0
WireConnection;360;0;359;0
WireConnection;360;1;358;0
WireConnection;1;0;15;0
WireConnection;345;0;348;0
WireConnection;585;0;72;0
WireConnection;367;1;360;0
WireConnection;64;0;1;0
WireConnection;64;1;65;0
WireConnection;64;2;72;0
WireConnection;575;0;64;0
WireConnection;575;6;137;0
WireConnection;575;11;367;0
ASEEND*/
//CHKSM=1A6663ABBFF279F632E63F34CA017C77E506F81F
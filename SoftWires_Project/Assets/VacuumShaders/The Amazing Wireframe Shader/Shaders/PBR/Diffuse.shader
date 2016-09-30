// VacuumShaders 2015
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/The Amazing Wireframe/Physically Based" 
{
	Properties 
	{
		//Tag         
		[V_WIRE_Tag] _V_WIRE_Tag("", float) = 0 
		 
		//Rendering Options
		[V_WIRE_RenderingOptions_PBR] _V_WIRE_RenderingOptions_PBREnumID("", float) = 0

		[MaterialEnum(Off,0,Front,1,Back,2)] _Cull("Cull", Int) = 2

		//Visual Options 
		[V_WIRE_Title] _V_WIRE_Title_V_Options("Visual Options", float) = 0   

		//Base
		_Color("Color (RGB)", color) = (1, 1, 1, 1)
		_MainTex("Base (RGB)", 2D) = "white"{}			
		[V_WIRE_UVScroll] _V_WIRE_MainTex_Scroll("    ", vector) = (0, 0, 0, 0)

		//Bump
	    [V_WIRE_BumpPBR]  _V_WIRE_BumpEnumID ("", Float) = 0	
		[HideInInspector] _V_WIRE_NormalMap ("", 2D) = "bump" {}

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0


		//Vertex Color
		[V_WIRE_VertexColor] _V_WIRE_VertexColor ("", float) = 0	

		 
		//Wire
		[V_WIRE_Title] _V_WIRE_Title_W_Options("Wire Options", float) = 0  		
		
		[V_WIRE_HDRColor] _V_WIRE_Color("", color) = (0, 0, 0, 1)
		_V_WIRE_Size("Size", Range(0, 0.5)) = 0.05			

		//Light
		[V_WIRE_IncludeLight] _V_WIRE_IncludeLightEnumID ("", float) = 0
					  
		//Transparency 
		[V_WIRE_Transparency] _V_WIRE_TransparencyEnumID("", float) = 0 
		[HideInInspector]     _V_WIRE_TransparentTex("", 2D) = "white"{}		
		[HideInInspector]	  _V_WIRE_TransparentTex_Scroll("    ", vector) = (0, 0, 0, 0)
		[HideInInspector]	  _V_WIRE_TransparentTex_UVSet("    ", float) = 0
		[HideInInspector]	  _V_WIRE_TransparentTex_Invert("    ", float) = 0
		[HideInInspector]	  _V_WIRE_TransparentTex_Alpha_Offset("    ", Range(-1, 1)) = 0
				 
		//Fresnel
	    [V_WIRE_Fresnel]  _V_WIRE_FresnelEnumID ("Fresnel", Float) = 0	
		[HideInInspector] _V_WIRE_FresnelInvert("", float) = 0
		[HideInInspector] _V_WIRE_FresnelBias("", Range(-1, 1)) = 0
		[HideInInspector] _V_WIRE_FresnelPow("", Range(1, 16)) = 1

		//Dynamic Mask
		[V_WIRE_Title]		 _V_WIRE_Title_M_Options("Dynamic Mask Options", float) = 0  
		[V_WIRE_DynamicMask] _V_WIRE_DynamicMaskEnumID("", float) = 0
		[HideInInspector]    _V_WIRE_DynamicMaskInvert("", float) = 0
		[HideInInspector]    _V_WIRE_DynamicMaskEffectsBaseTexEnumID("", int) = 0
		[HideInInspector]    _V_WIRE_DynamicMaskEffectsBaseTexInvert("", float) = 0	
	}
	 
	SubShader 
	{
		Tags { "RenderType"="Opaque" }  
		LOD 200    
		Cull [_Cull]    
		CGPROGRAM    
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert finalcolor:WireFinalColor

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0  
		  
		   
		
		 
		#pragma shader_feature V_WIRE_LIGHT_OFF V_WIRE_LIGHT_ON
		#ifdef UNITY_PASS_DEFERRED
			#ifndef V_WIRE_LIGHT_ON
			#define V_WIRE_LIGHT_ON
			#endif
		#endif
		#pragma shader_feature V_WIRE_FRESNEL_OFF V_WIRE_FRESNEL_ON
		#pragma shader_feature V_WIRE_TRANSPARENCY_OFF V_WIRE_TRANSPARENCY_ON		

		#pragma shader_feature V_WIRE_DYNAMIC_MASK_OFF V_WIRE_DYNAMI_MASK_PLANE V_WIRE_DYNAMIC_MASK_SPHERE 
		   
		
		#define V_WIRE_PBR
		   		   
		#include "../cginc/Wireframe_PBR.cginc" 
		ENDCG 
	}
	
	FallBack "Hidden/VacuumShaders/The Amazing Wireframe/Mobile/Vertex Lit"
}

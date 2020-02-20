// Upgrade NOTE: upgraded instancing buffer 'Offset_cube' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Offset_cube"
{
	Properties
	{
		_Range("Range", Range( 0 , 1)) = 1
		_RandomOffset("_RandomOffset", Range( 0 , 1)) = 1
		_Float0("Float 0", Float) = 1
		_Remap_Min("Remap_Min", Range( 0 , 1)) = 0
		_Remap_Max("Remap_Max", Range( 0 , 1)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			half filler;
		};

		uniform float _Range;
		uniform float _Float0;
		uniform float _Remap_Min;
		uniform float _Remap_Max;

		UNITY_INSTANCING_BUFFER_START(Offset_cube)
			UNITY_DEFINE_INSTANCED_PROP(float, _RandomOffset)
#define _RandomOffset_arr Offset_cube
		UNITY_INSTANCING_BUFFER_END(Offset_cube)

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime198 = _Time.y * _Float0;
			float _RandomOffset_Instance = UNITY_ACCESS_INSTANCED_PROP(_RandomOffset_arr, _RandomOffset);
			float3 appendResult195 = (float3(0.0 , ( _Range * (_Remap_Min + (sin( ( mulTime198 * _RandomOffset_Instance ) ) - 0.0) * (_Remap_Max - _Remap_Min) / (1.0 - 0.0)) ) , 0.0));
			v.vertex.xyz += appendResult195;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color197 = IsGammaSpace() ? float4(0.7688679,0.9870393,1,0) : float4(0.5523984,0.9707692,1,0);
			o.Albedo = color197.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "2"
}
/*ASEBEGIN
Version=17500
353;406;992;557;-4170.309;-1135.135;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;212;4187.626,1234.309;Inherit;False;Property;_Float0;Float 0;2;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;211;4248.066,1399.545;Inherit;False;InstancedProperty;_RandomOffset;_RandomOffset;1;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;198;4380.218,1234.724;Inherit;False;1;0;FLOAT;0.75;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;199;4556.717,1292.478;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;216;4725.839,1545.539;Inherit;False;Property;_Remap_Min;Remap_Min;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;217;4726.839,1630.539;Inherit;False;Property;_Remap_Max;Remap_Max;4;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;201;4758.652,1278.073;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;203;5082.494,1211.459;Inherit;False;Property;_Range;Range;0;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;213;5097.392,1387.195;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;5385.818,1299.001;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;195;5691.972,1214.148;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;2;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;197;5889.163,859.2081;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;0.7688679,0.9870393,1,0;1,0.4951983,0.1273585,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;6246.41,939.9792;Float;False;True;-1;2;2;0;0;Standard;Offset_cube;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;198;0;212;0
WireConnection;199;0;198;0
WireConnection;199;1;211;0
WireConnection;201;0;199;0
WireConnection;213;0;201;0
WireConnection;213;3;216;0
WireConnection;213;4;217;0
WireConnection;202;0;203;0
WireConnection;202;1;213;0
WireConnection;195;1;202;0
WireConnection;0;0;197;0
WireConnection;0;11;195;0
ASEEND*/
//CHKSM=83B3C72BDF3FACC53CBD8AA18EA9480CD88108C2
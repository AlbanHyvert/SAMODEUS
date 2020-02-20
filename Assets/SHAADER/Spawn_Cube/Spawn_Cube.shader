// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Spawn_Cube"
{
	Properties
	{
		_Y("Y", Float) = -30000
		_Scale_petit("Scale_petit", Float) = -1
		_Scale_grand("Scale_grand", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
		};

		uniform float _Scale_petit;
		uniform float _Y;
		uniform float _Scale_grand;
		uniform float3 Player;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float4 appendResult166 = (float4(0.0 , _Y , 0.0 , 0.0));
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float smoothstepResult174 = smoothstep( -3.0 , 3.0 , ( distance( ase_worldPos , Player ) - 30.0 ));
			float clampResult175 = clamp( smoothstepResult174 , 0.0 , 1.0 );
			float3 lerpResult160 = lerp( (ase_vertex3Pos*_Scale_petit + appendResult166.xyz) , (ase_vertex3Pos*_Scale_grand + 0.0) , ( 1.0 - clampResult175 ));
			v.vertex.xyz += lerpResult160;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color178 = IsGammaSpace() ? float4(0.764151,0.616367,0.616367,0) : float4(0.5448383,0.3379856,0.3379856,0);
			o.Albedo = color178.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17500
419;480;978;481;2329.381;-326.7227;1.747063;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;131;-1362.646,742.3585;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;153;-1361.589,931.2576;Inherit;False;Global;Player;Player;8;0;Create;True;0;0;False;0;0,0,0;-3.53,-2.89,7.22;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DistanceOpNode;170;-1084.973,733.1687;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;171;-879.4723,732.9685;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;30;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;174;-663.6044,688.0131;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-3;False;2;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;164;-1962.971,447.5378;Inherit;False;Property;_Y;Y;6;0;Create;True;0;0;False;0;-30000;-30000;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;157;-1510.85,142.4768;Inherit;False;Property;_Scale_petit;Scale_petit;7;0;Create;True;0;0;False;0;-1;-1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;154;-1812.376,189.5094;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;175;-405.7736,658.1105;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;166;-1647.171,424.9281;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;67;-5792.653,-264.2595;Inherit;False;2318.511;1298.96;Comment;8;39;37;66;60;59;58;57;19;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-1475.883,583.3444;Inherit;False;Property;_Scale_grand;Scale_grand;8;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;39;-5099.783,-217.4639;Inherit;False;1051.347;555.6987;Scale;5;45;40;15;41;30;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;130;-4735.191,-1347.584;Inherit;False;1275.58;432.307;Comment;7;114;115;116;117;118;119;120;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;156;-1249.391,255.0738;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;176;-217.5108,641.7729;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;159;-1249.321,495.1183;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;129;-4692.505,-1959.239;Inherit;False;1275.581;432.3071;Comment;7;71;73;97;76;79;77;81;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;37;-4551.343,532.5927;Inherit;False;725.0946;224.0643;Y;3;22;23;29;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-4522.951,586.0422;Inherit;False;Property;_Float1;Float 1;0;0;Create;True;0;0;False;0;30000;30000;0;30000;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;125;-1191.258,-874.4402;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-1566.208,-808.4762;Inherit;False;Property;_Float7;Float 7;3;0;Create;True;0;0;False;0;-100;-100;-100;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;127;-1720.349,-889.2391;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;66;-5011.042,701.079;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-4967.154,18.82746;Inherit;False;Constant;_Float2;Float 2;2;0;Create;True;0;0;False;0;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;83;-2035.657,-1023.187;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;114;-4685.191,-1261.524;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-3651.924,-1779.932;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;71;-4642.505,-1873.179;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-3714.772,393.1863;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;73;-4318.707,-1778.105;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;57;-5646.873,670.1329;Inherit;False;1;0;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-4204.964,622.6737;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-5462.882,744.5825;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;160;10.72342,388.2701;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StepOpNode;76;-4132.488,-1685.374;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;116;-4559.337,-1046.941;Inherit;False;Property;_Second_Ofsset;Second_Ofsset;5;0;Create;True;0;0;False;0;0;-6.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;90;-1139.621,-1549.608;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-4516.651,-1658.594;Inherit;False;Property;_Offset;Offset;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;69;-1576.759,-1843.388;Inherit;False;Constant;_Color0;Color 0;3;1;[HDR];Create;True;0;0;False;0;0,1.223529,2.980392,0.007843138;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;120;-3694.61,-1168.278;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;40;-4717.489,-92.83058;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;118;-4018.805,-1086.926;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;119;-4213.37,-1297.584;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-5639.715,807.3502;Inherit;False;Property;_Vitesse;Vitesse;1;0;Create;True;0;0;False;0;0.5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;23;-3983.247,588.5925;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;89;-1289.865,-1186.041;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;117;-4175.175,-1073.72;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;77;-3976.12,-1698.58;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;79;-4170.683,-1909.239;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-2133.428,-1340.814;Inherit;False;Property;_Emission;Emission;2;0;Create;True;0;0;False;0;-100;-100;-100;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;128;-1676.292,-1281.026;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;45;-4478.261,-58.33686;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;115;-4361.393,-1166.451;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;124;-2868.641,-1512.334;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;59;-5286.231,671.024;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;94;-934.5049,-1476.836;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;15;-5004.139,-165.6747;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;178;-10.52269,-29.25756;Inherit;False;Constant;_Color1;Color 1;9;0;Create;True;0;0;False;0;0.764151,0.616367,0.616367,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-4281.017,59.13545;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-1301.156,-1665.388;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;353.6572,12.05292;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Spawn_Cube;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;170;0;131;0
WireConnection;170;1;153;0
WireConnection;171;0;170;0
WireConnection;174;0;171;0
WireConnection;175;0;174;0
WireConnection;166;1;164;0
WireConnection;156;0;154;0
WireConnection;156;1;157;0
WireConnection;156;2;166;0
WireConnection;176;0;175;0
WireConnection;159;0;154;0
WireConnection;159;1;158;0
WireConnection;125;0;127;0
WireConnection;125;1;126;0
WireConnection;127;0;83;3
WireConnection;66;0;59;0
WireConnection;81;0;79;0
WireConnection;81;1;77;0
WireConnection;19;0;30;0
WireConnection;19;1;23;0
WireConnection;73;0;71;1
WireConnection;29;0;22;0
WireConnection;29;1;66;0
WireConnection;58;0;57;0
WireConnection;58;1;60;0
WireConnection;160;0;156;0
WireConnection;160;1;159;0
WireConnection;160;2;176;0
WireConnection;76;0;97;0
WireConnection;76;1;73;0
WireConnection;90;1;68;0
WireConnection;90;2;89;0
WireConnection;120;0;119;0
WireConnection;120;1;118;0
WireConnection;40;0;15;0
WireConnection;40;1;41;0
WireConnection;118;0;117;0
WireConnection;119;0;116;0
WireConnection;119;1;114;1
WireConnection;23;1;29;0
WireConnection;89;0;83;3
WireConnection;89;1;128;0
WireConnection;117;0;116;0
WireConnection;117;1;115;0
WireConnection;77;0;76;0
WireConnection;79;0;97;0
WireConnection;79;1;71;1
WireConnection;128;0;85;0
WireConnection;45;0;40;0
WireConnection;115;0;114;1
WireConnection;124;0;81;0
WireConnection;124;1;120;0
WireConnection;59;0;58;0
WireConnection;94;1;90;0
WireConnection;94;2;125;0
WireConnection;30;0;45;0
WireConnection;30;1;66;0
WireConnection;68;0;69;0
WireConnection;68;1;124;0
WireConnection;0;0;178;0
WireConnection;0;11;160;0
ASEEND*/
//CHKSM=3EC114DC7C1D15B36A36B007E1F61EDDC60841EB
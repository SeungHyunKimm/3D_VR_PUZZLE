using UnityEngine;

namespace SpaceGraphicsToolkit
{
	/// <summary>This class caches all shader property IDs used by SGT shaders.</summary>
	public static class SgtShader
	{
		public static int _Age;
		public static int _AmbientColor;
		public static int _AnimOffset;
		public static int _BumpScale;
		public static int _CameraRollAngle;
		public static int _Center;
		public static int _ClampSizeMin;
		public static int _ClampSizeScale;
		public static int _ClipPower;
		public static int _Color;
		public static int _CornerBL;
		public static int _CornerBR;
		public static int _CornerTL;
		public static int _CubeTex;
		public static int _DepthTex;
		public static int _DetailMapA;
		public static int _DetailMapB;
		public static int _DetailOffset;
		public static int _DetailScale;
		public static int _DetailScaleA;
		public static int _DetailScaleB;
		public static int _DetailStrength;
		public static int _DetailTex;
		public static int _DetailTiling;
		public static int _DetailTwist;
		public static int _DetailTwistBias;
		public static int _DisplacementColor;
		public static int _DistortOffset;
		public static int _DistortScale;
		public static int _DistortStrength;
		public static int _DistortTex;
		public static int _DstBlend;
		public static int _DstMode;
		public static int _EdgeFadePower;
		public static int _FadePower;
		public static int _FarRadius;
		public static int _FarScale;
		public static int _FarTex;
		public static int _Gau;
		public static int _GauDat;
		public static int _GauPos;
		public static int _HasWater;
		public static int _HeightMap;
		public static int _HeightMapScale;
		public static int _HeightMapSize;
		public static int _HighlightColor;
		public static int _HighlightPower;
		public static int _HighlightScale;
		public static int _HoleColor;
		public static int _HolePower;
		public static int _HoleSize;
		public static int _HorizonLengthRecip;
		public static int _InnerRatio;
		public static int _InnerScale;
		public static int _LightingTex;
		public static int _LocalToWorld;
		public static int _MainTex;
		public static int _MaskMap;
		public static int _NearScale;
		public static int _NearTex;
		public static int _NormalMap;
		public static int _NormalStep;
		public static int _NormalStrength;
		public static int _Offset;
		public static int _Output;
		public static int _PinchOffset;
		public static int _PinchPower;
		public static int _PinchScale;
		public static int _Point;
		public static int _Power;
		public static int _PulseOffset;
		public static int _Radius;
		public static int _RadiusMin;
		public static int _RadiusSize;
		public static int _Rip;
		public static int _RipDat;
		public static int _RipPos;
		public static int _Scale;
		public static int _ScaleRecip;
		public static int _ScatteringMie;
		public static int _ScatteringRayleigh;
		public static int _ScatteringTex;
		public static int _Side;
		public static int _Size;
		public static int _Sky;
		public static int _SoftParticlesFactor;
		public static int _SrcMode;
		public static int _Step;
		public static int _StretchDirection;
		public static int _StretchLength;
		public static int _StretchVector;
		public static int _Tile;
		public static int _TintColor;
		public static int _TintPower;
		public static int _Twi;
		public static int _TwiDat;
		public static int _TwiMat;
		public static int _TwiPos;
		public static int _WaterLevel;
		public static int _WorldPosition;
		public static int _WorldToLocal;
		public static int _WrapScale;
		public static int _WrapSize;
		public static int _ZTest;
		public static int _ZWriteMode;

		static SgtShader()
		{
			_Age = Shader.PropertyToID("_Age");
			_AmbientColor = Shader.PropertyToID("_AmbientColor");
			_AnimOffset = Shader.PropertyToID("_AnimOffset");
			_BumpScale = Shader.PropertyToID("_BumpScale");
			_CameraRollAngle = Shader.PropertyToID("_CameraRollAngle");
			_Center = Shader.PropertyToID("_Center");
			_ClampSizeMin = Shader.PropertyToID("_ClampSizeMin");
			_ClampSizeScale = Shader.PropertyToID("_ClampSizeScale");
			_ClipPower = Shader.PropertyToID("_ClipPower");
			_Color = Shader.PropertyToID("_Color");
			_CornerBL = Shader.PropertyToID("_CornerBL");
			_CornerBR = Shader.PropertyToID("_CornerBR");
			_CornerTL = Shader.PropertyToID("_CornerTL");
			_CubeTex = Shader.PropertyToID("_CubeTex");
			_DepthTex = Shader.PropertyToID("_DepthTex");
			_DetailMapA = Shader.PropertyToID("_DetailMapA");
			_DetailMapB = Shader.PropertyToID("_DetailMapB");
			_DetailOffset = Shader.PropertyToID("_DetailOffset");
			_DetailScale = Shader.PropertyToID("_DetailScale");
			_DetailScaleA = Shader.PropertyToID("_DetailScaleA");
			_DetailScaleB = Shader.PropertyToID("_DetailScaleB");
			_DetailStrength = Shader.PropertyToID("_DetailStrength");
			_DetailTex = Shader.PropertyToID("_DetailTex");
			_DetailTiling = Shader.PropertyToID("_DetailTiling");
			_DetailTwist = Shader.PropertyToID("_DetailTwist");
			_DetailTwistBias = Shader.PropertyToID("_DetailTwistBias");
			_DisplacementColor = Shader.PropertyToID("_DisplacementColor");
			_DistortOffset = Shader.PropertyToID("_DistortOffset");
			_DistortScale = Shader.PropertyToID("_DistortScale");
			_DistortStrength = Shader.PropertyToID("_DistortStrength");
			_DistortTex = Shader.PropertyToID("_DistortTex");
			_DstBlend = Shader.PropertyToID("_DstBlend");
			_DstMode = Shader.PropertyToID("_DstMode");
			_EdgeFadePower = Shader.PropertyToID("_EdgeFadePower");
			_FadePower = Shader.PropertyToID("_FadePower");
			_FarRadius = Shader.PropertyToID("_FarRadius");
			_FarScale = Shader.PropertyToID("_FarScale");
			_FarTex = Shader.PropertyToID("_FarTex");
			_Gau = Shader.PropertyToID("_Gau");
			_GauDat = Shader.PropertyToID("_GauDat");
			_GauPos = Shader.PropertyToID("_GauPos");
			_HasWater = Shader.PropertyToID("_HasWater");
			_HeightMap = Shader.PropertyToID("_HeightMap");
			_HeightMapScale = Shader.PropertyToID("_HeightMapScale");
			_HeightMapSize = Shader.PropertyToID("_HeightMapSize");
			_HighlightColor = Shader.PropertyToID("_HighlightColor");
			_HighlightPower = Shader.PropertyToID("_HighlightPower");
			_HighlightScale = Shader.PropertyToID("_HighlightScale");
			_HoleColor = Shader.PropertyToID("_HoleColor");
			_HolePower = Shader.PropertyToID("_HolePower");
			_HoleSize = Shader.PropertyToID("_HoleSize");
			_HorizonLengthRecip = Shader.PropertyToID("_HorizonLengthRecip");
			_InnerRatio = Shader.PropertyToID("_InnerRatio");
			_InnerScale = Shader.PropertyToID("_InnerScale");
			_LightingTex = Shader.PropertyToID("_LightingTex");
			_LocalToWorld = Shader.PropertyToID("_LocalToWorld");
			_MainTex = Shader.PropertyToID("_MainTex");
			_MaskMap = Shader.PropertyToID("_MaskMap");
			_NearScale = Shader.PropertyToID("_NearScale");
			_NearTex = Shader.PropertyToID("_NearTex");
			_NormalMap = Shader.PropertyToID("_NormalMap");
			_NormalStep = Shader.PropertyToID("_NormalStep");
			_NormalStrength = Shader.PropertyToID("_NormalStrength");
			_Offset = Shader.PropertyToID("_Offset");
			_Output = Shader.PropertyToID("_Output");
			_PinchOffset = Shader.PropertyToID("_PinchOffset");
			_PinchPower = Shader.PropertyToID("_PinchPower");
			_PinchScale = Shader.PropertyToID("_PinchScale");
			_Point = Shader.PropertyToID("_Point");
			_Power = Shader.PropertyToID("_Power");
			_PulseOffset = Shader.PropertyToID("_PulseOffset");
			_Radius = Shader.PropertyToID("_Radius");
			_RadiusMin = Shader.PropertyToID("_RadiusMin");
			_RadiusSize = Shader.PropertyToID("_RadiusSize");
			_Rip = Shader.PropertyToID("_Rip");
			_RipDat = Shader.PropertyToID("_RipDat");
			_RipPos = Shader.PropertyToID("_RipPos");
			_Scale = Shader.PropertyToID("_Scale");
			_ScaleRecip = Shader.PropertyToID("_ScaleRecip");
			_ScatteringMie = Shader.PropertyToID("_ScatteringMie");
			_ScatteringRayleigh = Shader.PropertyToID("_ScatteringRayleigh");
			_ScatteringTex = Shader.PropertyToID("_ScatteringTex");
			_Side = Shader.PropertyToID("_Side");
			_Size = Shader.PropertyToID("_Size");
			_Sky = Shader.PropertyToID("_Sky");
			_SoftParticlesFactor = Shader.PropertyToID("_SoftParticlesFactor");
			_SrcMode = Shader.PropertyToID("_SrcMode");
			_Step = Shader.PropertyToID("_Step");
			_StretchDirection = Shader.PropertyToID("_StretchDirection");
			_StretchLength = Shader.PropertyToID("_StretchLength");
			_StretchVector = Shader.PropertyToID("_StretchVector");
			_Tile = Shader.PropertyToID("_Tile");
			_TintColor = Shader.PropertyToID("_TintColor");
			_TintPower = Shader.PropertyToID("_TintPower");
			_Twi = Shader.PropertyToID("_Twi");
			_TwiDat = Shader.PropertyToID("_TwiDat");
			_TwiMat = Shader.PropertyToID("_TwiMat");
			_TwiPos = Shader.PropertyToID("_TwiPos");
			_WaterLevel = Shader.PropertyToID("_WaterLevel");
			_WorldPosition = Shader.PropertyToID("_WorldPosition");
			_WorldToLocal = Shader.PropertyToID("_WorldToLocal");
			_WrapScale = Shader.PropertyToID("_WrapScale");
			_WrapSize = Shader.PropertyToID("_WrapSize");
			_ZTest = Shader.PropertyToID("_ZTest");
			_ZWriteMode = Shader.PropertyToID("_ZWriteMode");
		}
	}
}
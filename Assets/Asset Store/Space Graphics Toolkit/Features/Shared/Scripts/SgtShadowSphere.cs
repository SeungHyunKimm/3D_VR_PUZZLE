using UnityEngine;
using FSA = UnityEngine.Serialization.FormerlySerializedAsAttribute;

namespace SpaceGraphicsToolkit
{
	/// <summary>This component allows you to cast a sphere shadow from the current GameObject.</summary>
	[ExecuteInEditMode]
	[HelpURL(SgtHelper.HelpUrlPrefix + "SgtShadowSphere")]
	[AddComponentMenu(SgtHelper.ComponentMenuPrefix + "Shadow Sphere")]
	public class SgtShadowSphere : SgtShadow
	{
		/// <summary>The width of the generated texture. A higher value can result in a smoother transition.</summary>
		public int Width { set { if (width != value) { width = value; DirtyTexture(); } } get { return width; } } [FSA("Width")] [SerializeField] private int width = 256;

		/// <summary>The format of the generated texture.</summary>
		public TextureFormat Format { set { if (format != value) { format = value; DirtyTexture(); } } get { return format; } } [FSA("Format")] [SerializeField] private TextureFormat format = TextureFormat.ARGB32;

		/// <summary>The sharpness of the sunset red channel transition.</summary>
		public float SharpnessR { set { if (sharpnessR != value) { sharpnessR = value; DirtyTexture(); } } get { return sharpnessR; } } [FSA("SharpnessR")] [SerializeField] private float sharpnessR = 1.0f;

		/// <summary>The power of the sunset green channel transition.</summary>
		public float SharpnessG { set { if (sharpnessG != value) { sharpnessG = value; DirtyTexture(); } } get { return sharpnessG; } } [FSA("SharpnessG")] [SerializeField] private float sharpnessG = 1.0f;

		/// <summary>The power of the sunset blue channel transition.</summary>
		public float SharpnessB { set { if (sharpnessB != value) { sharpnessB = value; DirtyTexture(); } } get { return sharpnessB; } } [FSA("SharpnessB")] [SerializeField] private float sharpnessB = 1.0f;

		/// <summary>The opacity of the shadow.</summary>
		public float Opacity { set { if (opacity != value) { opacity = value; DirtyTexture(); } } get { return opacity; } } [FSA("Opacity")] [SerializeField] [Range(0.0f, 1.0f)] private float opacity = 1.0f;

		/// <summary>The inner radius of the sphere in local space.</summary>
		public float RadiusMin { set { if (radiusMin != value) { radiusMin = value; DirtyTexture(); } } get { return radiusMin; } } [FSA("RadiusMin")] [SerializeField] private float radiusMin = 1.0f;

		/// <summary>The outer radius of the sphere in local space.</summary>
		public float RadiusMax { set { if (radiusMax != value) { radiusMax = value; DirtyTexture(); } } get { return radiusMax; } } [FSA("RadiusMax")] [SerializeField] private float radiusMax = 1.1f;

		[System.NonSerialized]
		private Texture2D generatedTexture;

		[SerializeField]
		[HideInInspector]
		private bool startCalled;

		public Texture2D GeneratedTexture
		{
			get
			{
				return generatedTexture;
			}
		}

		public override Texture GetTexture()
		{
			if (generatedTexture == false)
			{
				UpdateTexture();
			}

			return generatedTexture;
		}

		public void DirtyTexture()
		{
			UpdateTexture();
		}

		[ContextMenu("Update Textures")]
		public void UpdateTexture()
		{
			if (width > 0)
			{
				// Destroy if invalid
				if (generatedTexture != null)
				{
					if (generatedTexture.width != width || generatedTexture.height != 1 || generatedTexture.format != format)
					{
						generatedTexture = SgtHelper.Destroy(generatedTexture);
					}
				}

				// Create?
				if (generatedTexture == null)
				{
					generatedTexture = SgtHelper.CreateTempTexture2D("Sphere Shadow (Generated)", width, 1, format);

					generatedTexture.wrapMode = TextureWrapMode.Clamp;
				}

				var color = Color.clear;
				var stepX = 1.0f / (width - 1);

				for (var x = 0; x < width; x++)
				{
					var u = x * stepX;

					WriteTexture(u, x);
				}
			
				generatedTexture.Apply();
			}
		}

		private void WriteTexture(float u, int x)
		{
			var color = default(Color);

			color.r = SgtHelper.Sharpness(u, sharpnessR) * opacity + (1.0f - opacity);
			color.g = SgtHelper.Sharpness(u, sharpnessG) * opacity + (1.0f - opacity);
			color.b = SgtHelper.Sharpness(u, sharpnessB) * opacity + (1.0f - opacity);
			color.a = 1.0f;

			generatedTexture.SetPixel(x, 0, SgtHelper.Saturate(color));
		}

		public override void CalculateShadow(SgtLight light)
		{
			var direction = default(Vector3);
			var position  = default(Vector3);
			var color     = default(Color);

			SgtLight.Calculate(light, transform.position, null, null, ref position, ref direction, ref color);

			var dot      = Vector3.Dot(direction, transform.up);
			var radiusXZ = (transform.lossyScale.x + transform.lossyScale.z) * 0.5f * radiusMax;
			var radiusY  = transform.lossyScale.y * radiusMax;
			var radius   = GetRadius(radiusY, radiusXZ, dot * Mathf.PI * 0.5f);
			var rotation = Quaternion.FromToRotation(direction, Vector3.back);
			var vector   = rotation * transform.up;
			var spin     = Quaternion.LookRotation(Vector3.forward, new Vector2(-vector.x, vector.y)); // Orient the shadow ellipse
			var scale    = SgtHelper.Reciprocal3(new Vector3(radiusXZ, radius, 1.0f));
			var shadowT  = Matrix4x4.Translate(-transform.position);
			var shadowR  = Matrix4x4.Rotate(spin * rotation);
			var shadowS  = Matrix4x4.Scale(scale);

			cachedActive  = true;
			cachedMatrix  = shadowS * shadowR * shadowT;
			cachedRatio   = SgtHelper.Divide(radiusMax, radiusMax - radiusMin);
			cachedRadius  = SgtHelper.UniformScale(transform.lossyScale) * radiusMax;
			cachedTexture = generatedTexture;
		}

		private float GetRadius(float a, float b, float theta)
		{
			var s = Mathf.Sin(theta);
			var c = Mathf.Cos(theta);
			var z = Mathf.Sqrt((a*a)*(s*s)+(b*b)*(c*c));

			if (z != 0.0f)
			{
				return (a * b) / z;
			}

			return a;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			if (startCalled == true)
			{
				CheckUpdateCalls();
			}
		}

		protected virtual void Start()
		{
			if (startCalled == false)
			{
				startCalled = true;

				CheckUpdateCalls();
			}
		}
	
		protected virtual void OnDestroy()
		{
			SgtHelper.Destroy(generatedTexture);
		}

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			var mask   = 1 << gameObject.layer;
			var lights = SgtLight.Find(true, mask, transform.position);

			if (SgtHelper.Enabled(this) == true && lights.Count > 0)
			{
				Gizmos.matrix = transform.localToWorldMatrix;

				Gizmos.DrawWireSphere(Vector3.zero, radiusMin);
				Gizmos.DrawWireSphere(Vector3.zero, radiusMax);

				CalculateShadow(lights[0]);

				if (cachedActive == true)
				{
					Gizmos.matrix = cachedMatrix.inverse;

					var distA = 0.0f;
					var distB = 1.0f;
					var scale = 1.0f * Mathf.Deg2Rad;

					for (var i = 0; i < 10; i++)
					{
						var posA  = new Vector3(0.0f, 0.0f, distA);
						var posB  = new Vector3(0.0f, 0.0f, distB);

						Gizmos.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Pow(0.75f, i) * 0.125f);

						for (var a = 1; a <= 360; a++)
						{
							posA.x = posB.x = Mathf.Sin(a * scale);
							posA.y = posB.y = Mathf.Cos(a * scale);

							Gizmos.DrawLine(posA, posB);
						}

						distA = distB;
						distB = distB * 2.0f;
					}
				}
			}
		}
#endif

		private void CheckUpdateCalls()
		{
			if (generatedTexture == null)
			{
				UpdateTexture();
			}
		}
	}
}

#if UNITY_EDITOR
namespace SpaceGraphicsToolkit
{
	using TARGET = SgtShadowSphere;

	[UnityEditor.CanEditMultipleObjects]
	[UnityEditor.CustomEditor(typeof(TARGET))]
	public class SgtShadowSphere_Editor : SgtEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			var dirtyTexture = false;

			BeginError(Any(tgts, t => t.Width < 1));
				Draw("width", ref dirtyTexture, "The width of the generated texture. A higher value can result in a smoother transition.");
			EndError();
			Draw("format", ref dirtyTexture, "The format of the generated texture.");
			Draw("sharpnessR", ref dirtyTexture, "The sharpness of the sunset red channel transition.");
			Draw("sharpnessG", ref dirtyTexture, "The sharpness of the sunset green channel transition.");
			Draw("sharpnessB", ref dirtyTexture, "The sharpness of the sunset blue channel transition.");
			BeginError(Any(tgts, t => t.Opacity < 0.0f));
				Draw("opacity", ref dirtyTexture, "The opacity of the shadow.");
			EndError();
			BeginError(Any(tgts, t => t.RadiusMin < 0.0f || t.RadiusMin >= t.RadiusMax));
				Draw("radiusMin", "The inner radius of the sphere in local space.");
			EndError();
			BeginError(Any(tgts, t => t.RadiusMax < 0.0f || t.RadiusMin >= t.RadiusMax));
				Draw("radiusMax", "The outer radius of the sphere in local space.");
			EndError();

			if (dirtyTexture == true) Each(tgts, t => t.DirtyTexture(), true);
		}
	}
}
#endif
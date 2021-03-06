using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

namespace SpaceGraphicsToolkit
{
	/// <summary>This allows you to convert color (RGB) + height (A) textures into a height array and a weight array. The weight array can contain any number of splat layers.</summary>
	public class SgtTerrainAreas : ScriptableObject
	{
		public enum ChannelType
		{
			Red,
			Green,
			Blue,
			Alpha
		}

		[System.Serializable]
		public class Splat
		{
			public string Name { set { name = value; } get { return name; } } [SerializeField] private string name;

			public Color Color { set { color = value; } get { return color; } } [SerializeField] private Color color = Color.white;
		}

		public Texture2D Texture { set { texture = value; } get { return texture; } } [SerializeField] private Texture2D texture;

		/// <summary>The splat map layers that will be extracted from the source textures.</summary>
		public List<Splat> Splats { get { if (splats == null) splats = new List<Splat>(); return splats; } } [SerializeField] private List<Splat> splats;

		/// <summary>This allows you to get how many splat maps are contained in the array data.</summary>
		public int SplatCount { get { UpdateArrays(); return splatCount; } } [System.NonSerialized] private int splatCount;

		/// <summary>This tells you how many pixels are in each array layer.</summary>
		public int2 Size { get { UpdateArrays(); return size; } } [System.NonSerialized] private int2 size;

		/// <summary>This allows you to get the extracted splat data.
		/// NOTE: This array size will be LayerCount * Size.x * Size.Y * SplatCount.</summary>
		public NativeArray<float> Weights { get { UpdateArrays(); return weights; } } [System.NonSerialized] private NativeArray<float> weights;

		private static List<Color32> tempColors = new List<Color32>();

		[System.NonSerialized]
		private bool dirty = true;

		[ContextMenu("Mark As Dirty")]
		public void MarkAsDirty()
		{
			dirty = true;
		}

		protected virtual void OnEnable()
		{
			weights    = new NativeArray<float>(0, Allocator.Persistent);
			splatCount = 0;
			size       = int2.zero;

			UpdateArrays();
		}

		protected virtual void OnDisable()
		{
			weights.Dispose();

			splatCount = 0;
			size       = default(int2);
		}

		private void UpdateArrays()
		{
			if (dirty == true)
			{
				dirty = false;

				if (GenerateAreas() == true)
				{
					return;
				}

				splatCount = 0;
				size       = int2.zero;

				SgtHelper.UpdateNativeArray(ref weights, 0);
			}
		}

		private bool GenerateAreas()
		{
#if UNITY_EDITOR
			SgtHelper.MakeTextureReadable(texture);
			SgtHelper.MakeTextureTruecolor(texture);
#endif
			if (texture != null && splats != null)
			{
				splatCount = splats.Count;
				size       = new int2(texture.width, texture.height);

				SgtHelper.UpdateNativeArray(ref weights, size.x * size.y * splatCount);

				tempColors.Clear();

				foreach (var splat in splats)
				{
					tempColors.Add(splat.Color);
				}

				var data = texture.GetRawTextureData<byte>();
				var offS = SgtHelper.GetStride(texture.format);
				var offR = SgtHelper.GetOffset(texture.format, 0);
				var offG = SgtHelper.GetOffset(texture.format, 1);
				var offB = SgtHelper.GetOffset(texture.format, 2);

				for (var y = 0; y < size.y; y++)
				{
					for (var x = 0; x < size.x; x++)
					{
						var index  = x + y * size.x;
						var r      = data[index * offS + offR];
						var g      = data[index * offS + offG];
						var b      = data[index * offS + offB];

						for (var s = 0; s < splatCount; s++)
						{
							weights[index * splatCount + s] = GetDistance(tempColors[s], r, g, b);
						}
					}
				}

				return true;
			}

			return false;
		}

		private float GetDistance(Color32 color, byte r, byte g, byte b)
		{
			var distanceR = color.r - r;
			var distanceG = color.g - g;
			var distanceB = color.b - b;

			distanceR *= distanceR;
			distanceG *= distanceG;
			distanceB *= distanceB;

			return distanceR + distanceG + distanceB;
		}
	}
}

#if UNITY_EDITOR
namespace SpaceGraphicsToolkit
{
	using TARGET = SgtTerrainAreas;

	//[UnityEditor.CanEditMultipleObjects]
	[UnityEditor.CustomEditor(typeof(TARGET))]
	public class SgtTerrainAreas_Editor : SgtEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("texture", "");

			Separator();

			Draw("splats", "The splat map layers that will be extracted from the source textures.");
		}
	}
}
#endif
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MetroSlice9 : MonoBehaviour
{
	private Transform _pivot;

	private MeshFilter _meshFilter;

	private MeshRenderer _meshRenderer;

	private float _width;

	private float _height;

	private MetroSlice9Texture slice9;

	public MetroSlice9 Setup(MetroSlice9Texture slice9Texture)
	{
		slice9 = slice9Texture;
		base.gameObject.layer = 8;
		_meshFilter = GetComponent<MeshFilter>();
		_meshFilter.sharedMesh = null;
		_meshRenderer = GetComponent<MeshRenderer>();
		_meshRenderer.sharedMaterial = slice9Texture.Material;
		_width = 1f;
		_height = 1f;
		UpdateMesh();
		return this;
	}

	private void UpdateMesh()
	{
		_meshFilter.sharedMesh = MakeMesh();
	}

	private Mesh MakeMesh()
	{
		float num = slice9.Material.mainTexture.width;
		float num2 = slice9.Material.mainTexture.height;
		float num3 = (float)slice9.Left / num;
		float num4 = (float)slice9.Right / num;
		float num5 = 1f - (num3 + num4);
		float num6 = (float)slice9.Top / num2;
		float num7 = (float)slice9.Bottom / num2;
		float num8 = 1f - (num6 + num7);
		float num9 = 0f;
		float num10 = num9 + num3;
		float num11 = num10 + num5;
		float x = num11 + num4;
		float num12 = 0f;
		float num13 = num12 + num7;
		float num14 = num13 + num8;
		float y = num14 + num6;
		num3 = (float)slice9.Left * 0.05f / _width;
		num4 = (float)slice9.Right * 0.05f / _width;
		num5 = 1f - (num3 + num4);
		num6 = (float)slice9.Top * 0.05f / _height;
		num7 = (float)slice9.Bottom * 0.05f / _height;
		num8 = 1f - (num6 + num7);
		float num15 = -0.5f;
		float num16 = num15 + num3;
		float num17 = num16 + num5;
		float x2 = num17 + num4;
		float num18 = -0.5f;
		float num19 = num18 + num7;
		float num20 = num19 + num8;
		float y2 = num20 + num6;
		Vector3[] vertices = new Vector3[16]
		{
			new Vector3(num15, y2),
			new Vector3(num16, y2),
			new Vector3(num17, y2),
			new Vector3(x2, y2),
			new Vector3(num15, num20),
			new Vector3(num16, num20),
			new Vector3(num17, num20),
			new Vector3(x2, num20),
			new Vector3(num15, num19),
			new Vector3(num16, num19),
			new Vector3(num17, num19),
			new Vector3(x2, num19),
			new Vector3(num15, num18),
			new Vector3(num16, num18),
			new Vector3(num17, num18),
			new Vector3(x2, num18)
		};
		Vector2[] uv = new Vector2[16]
		{
			new Vector2(num9, y),
			new Vector2(num10, y),
			new Vector2(num11, y),
			new Vector2(x, y),
			new Vector2(num9, num14),
			new Vector2(num10, num14),
			new Vector2(num11, num14),
			new Vector2(x, num14),
			new Vector2(num9, num13),
			new Vector2(num10, num13),
			new Vector2(num11, num13),
			new Vector2(x, num13),
			new Vector2(num9, num12),
			new Vector2(num10, num12),
			new Vector2(num11, num12),
			new Vector2(x, num12)
		};
		int[] triangles = new int[54]
		{
			0,
			1,
			4,
			1,
			5,
			4,
			1,
			2,
			5,
			2,
			6,
			5,
			2,
			3,
			6,
			3,
			7,
			6,
			4,
			5,
			8,
			5,
			9,
			8,
			5,
			6,
			9,
			6,
			10,
			9,
			6,
			7,
			10,
			7,
			11,
			10,
			8,
			9,
			12,
			9,
			13,
			12,
			9,
			10,
			13,
			10,
			14,
			13,
			10,
			11,
			14,
			11,
			15,
			14
		};
		Mesh mesh = new Mesh();
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		return mesh;
	}

	public void SetSize(float w, float h)
	{
		_width = w;
		_height = h;
		base.transform.localScale = new Vector3(w, h, 1f);
		UpdateMesh();
	}

	public static MetroSlice9 Create(Transform parent, MetroSlice9Texture texture)
	{
		GameObject gameObject = new GameObject("MetroSlice9");
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = Vector3.zero;
		return gameObject.AddComponent<MetroSlice9>().Setup(texture);
	}

	public static MetroSlice9 Create(Transform parent, string resourceName)
	{
		MetroSlice9Texture component = (Resources.Load(resourceName) as GameObject).GetComponent<MetroSlice9Texture>();
		return Create(parent, component);
	}
}

using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MetroPie : MonoBehaviour
{
	private MeshFilter _meshFilter;

	private MeshRenderer _meshRenderer;

	public Material _material;

	public float _fill = 1f;

	public float Width
	{
		get;
		private set;
	}

	public float Height
	{
		get;
		private set;
	}

	public void Awake()
	{
		Setup(_fill);
	}

	public MetroPie Setup(float fill)
	{
		base.gameObject.layer = 8;
		Mesh mesh = new Mesh();
		mesh.Clear();
		int num = 32;
		int num2 = num * 3;
		Vector3[] array = new Vector3[num2];
		Vector2[] array2 = new Vector2[num2];
		Color[] array3 = new Color[num2];
		for (int i = 0; (float)i < (float)num2 * fill; i++)
		{
			array3[i] = Color.white;
		}
		for (int j = 0; (float)j < (float)num2 * fill; j += 3)
		{
			array[j] = Vector3.zero;
			float num3 = -(float)Math.PI / 2f;
			float f = num3 - (float)j / (float)num2 * (float)Math.PI * 2f;
			array[j + 2] = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f);
			float f2 = num3 - (float)(j + 3) / (float)num2 * (float)Math.PI * 2f;
			array[j + 1] = new Vector3(Mathf.Cos(f2), Mathf.Sin(f2), 0f);
			float x = (float)j / (float)num2;
			array2[j] = new Vector2(x, 0f);
			array2[j + 1] = new Vector2(x, 1f);
			array2[j + 2] = new Vector2(x, 1f);
		}
		int[] array4 = new int[num2];
		for (int k = 0; (float)k < (float)num2 * fill; k++)
		{
			array4[k] = k;
		}
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.colors = array3;
		mesh.triangles = array4;
		_meshFilter = GetComponent<MeshFilter>();
		_meshFilter.sharedMesh = mesh;
		_meshRenderer = GetComponent<MeshRenderer>();
		_meshRenderer.sharedMaterial = _material;
		Width = 1f;
		Height = 1f;
		return this;
	}

	public MetroPie SetMaterial(Material mat)
	{
		_material = mat;
		_meshRenderer.sharedMaterial = mat;
		return this;
	}

	public void SetSize(float w, float h)
	{
		Width = w;
		Height = h;
		base.transform.localScale = new Vector3(w, h, 1f);
	}

	public static MetroPie Create(Transform parent, float fill)
	{
		GameObject gameObject = new GameObject("MetroPie");
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = Vector3.zero;
		return gameObject.AddComponent<MetroPie>().Setup(fill);
	}

	public static MetroPie Create(Transform parent, Material mat, float fill)
	{
		MetroPie metroPie = Create(parent, fill);
		metroPie.SetMaterial(mat);
		return metroPie;
	}
}

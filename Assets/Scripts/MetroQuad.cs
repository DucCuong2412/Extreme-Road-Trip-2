using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MetroQuad : MonoBehaviour
{
	private MeshFilter _meshFilter;

	private MeshRenderer _meshRenderer;

	private Color _color1;

	private Color _color2;

	private static Material _sharedMaterial;

	public Renderer Renderer => _meshRenderer;

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

	static MetroQuad()
	{
		_sharedMaterial = new Material(Shader.Find("Roofdog/Vertex Colored"));
	}

	public void Awake()
	{
		base.gameObject.layer = 8;
		Mesh mesh = new Mesh();
		mesh.Clear();
		Vector3[] array = new Vector3[4];
		Vector2[] array2 = new Vector2[4];
		Color[] colors = new Color[4]
		{
			Color.red,
			Color.green,
			Color.blue,
			Color.white
		};
		array[0] = new Vector3(-0.5f, -0.5f, 0f);
		array[1] = new Vector3(0.5f, -0.5f, 0f);
		array[2] = new Vector3(0.5f, 0.5f, 0f);
		array[3] = new Vector3(-0.5f, 0.5f, 0f);
		array2[0] = new Vector2(0f, 0f);
		array2[1] = new Vector2(1f, 0f);
		array2[2] = new Vector2(1f, 1f);
		array2[3] = new Vector2(0f, 1f);
		int[] triangles = new int[6]
		{
			2,
			1,
			0,
			3,
			2,
			0
		};
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.colors = colors;
		mesh.triangles = triangles;
		_meshFilter = GetComponent<MeshFilter>();
		_meshFilter.sharedMesh = mesh;
		_meshRenderer = GetComponent<MeshRenderer>();
		_meshRenderer.sharedMaterial = _sharedMaterial;
		Width = 1f;
		Height = 1f;
	}

	public MetroQuad SetColor(Color color)
	{
		Mesh sharedMesh = _meshFilter.sharedMesh;
		_color1 = color;
		Color[] colors = sharedMesh.colors;
		for (int i = 0; i < colors.Length; i++)
		{
			colors[i] = color;
		}
		sharedMesh.colors = colors;
		return this;
	}

	public MetroQuad SetGradient(Color down, Color up)
	{
		Mesh sharedMesh = _meshFilter.sharedMesh;
		_color1 = down;
		_color2 = up;
		Color[] colors = sharedMesh.colors;
		colors[0] = down;
		colors[1] = down;
		colors[2] = up;
		colors[3] = up;
		sharedMesh.colors = colors;
		return this;
	}

	public void SetColorOverTime(Color color, float time, Action after)
	{
		StartCoroutine(SetGradientOverTimeCR(color, color, time, after));
	}

	public void SetGradientOverTime(Color _color1, Color _color2, float time, Action after)
	{
		StartCoroutine(SetGradientOverTimeCR(_color1, _color2, time, after));
	}

	private IEnumerator SetGradientOverTimeCR(Color to1, Color to2, float time, Action after)
	{
		Color from3 = _color1;
		Color from2 = _color2;
		yield return null;
		RealtimeDuration delay = new RealtimeDuration(time);
		while (!delay.IsDone())
		{
			float delayVal = delay.Value01();
			SetGradient(Color.Lerp(from3, to1, delayVal), Color.Lerp(from2, to2, delayVal));
			yield return null;
		}
		SetGradient(to1, to2);
		after?.Invoke();
	}

	public MetroQuad SetMaterial(Material mat)
	{
		_meshRenderer.sharedMaterial = mat;
		SetColor(Color.white);
		return this;
	}

	public void SetSize(float w, float h)
	{
		Width = w;
		Height = h;
		base.transform.localScale = new Vector3(w, h, 1f);
	}

	public void Crop(float wRatio, float hRatio)
	{
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(wRatio, 0f),
			new Vector2(wRatio, hRatio),
			new Vector2(0f, hRatio)
		};
		_meshFilter.sharedMesh.uv = uv;
	}

	public static MetroQuad Create(Transform parent)
	{
		GameObject gameObject = new GameObject("MetroQuad");
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = Vector3.zero;
		return gameObject.AddComponent<MetroQuad>();
	}

	public static MetroQuad Create(Transform parent, Material mat)
	{
		MetroQuad metroQuad = Create(parent);
		metroQuad.SetMaterial(mat);
		return metroQuad;
	}
}

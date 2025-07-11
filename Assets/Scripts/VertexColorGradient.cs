using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Quad))]
public class VertexColorGradient : MonoBehaviour
{
	public Color _color1 = Color.red;

	public Color _color2 = Color.blue;

	public Texture2D _texture;

	public Material _material;

	private Color _c1 = Color.white;

	private Color _c2 = Color.black;

	public void SetColorGradient(Color color1, Color color2)
	{
		Mesh sharedMesh = GetComponent<MeshFilter>().sharedMesh;
		Color[] colors = sharedMesh.colors;
		int num = colors.Length;
		for (int i = 0; i < num; i++)
		{
			colors[i] = ((i >= num / 2) ? color1 : color2);
		}
		sharedMesh.colors = colors;
	}

	public void Awake()
	{
		if (_material == null)
		{
			_material = new Material(Shader.Find("Roofdog/Sprite Double"));
			_material.mainTexture = _texture;
		}
		GetComponent<Quad>().Setup(_material);
	}

	public void Update()
	{
		if (_c1 != _color1 || _c2 != _color2)
		{
			_c1 = _color1;
			_c2 = _color2;
			SetColorGradient(_c1, _c2);
		}
	}
}

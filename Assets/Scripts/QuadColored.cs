using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Quad))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class QuadColored : MonoBehaviour
{
	public Color _color = Color.black;

	public void SetColor(Color color)
	{
		return;
		Mesh sharedMesh = GetComponent<MeshFilter>().sharedMesh;
		Color[] colors = sharedMesh.colors;
		for (int i = 0; i < colors.Length; i++)
		{
			colors[i] = color;
		}
		sharedMesh.colors = colors;
	}

	public void Awake()
	{
        //GetComponent<Quad>().Setup(new Material(Shader.Find("Roofdog/Vertex Colored")));
        GetComponent<Quad>().Setup(new Material(Shader.Find("Unlit/Transparent Colored")));
    }

	public void Update()
	{
		SetColor(_color);
	}
}

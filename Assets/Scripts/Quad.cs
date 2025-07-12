using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Quad : MonoBehaviour
{
	private Material _material;

	private MeshFilter _meshFilter;

	private MeshRenderer _meshRenderer;

	public Renderer Renderer => _meshRenderer;

	public void Setup(Material material)
	{
		_material = material;
		_meshRenderer = GetComponent<MeshRenderer>();
		_meshRenderer.material = _material;
		_meshFilter = GetComponent<MeshFilter>();
		_meshFilter.sharedMesh = new Mesh();
		_meshFilter.sharedMesh.Clear();
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
		_meshFilter.sharedMesh.vertices = array;
		_meshFilter.sharedMesh.uv = array2;
		_meshFilter.sharedMesh.colors = colors;
		int[] triangles = new int[6]
		{
			2,
			1,
			0,
			3,
			2,
			0
		};
		_meshFilter.sharedMesh.triangles = triangles;
	}
}

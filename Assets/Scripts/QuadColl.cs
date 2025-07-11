using UnityEngine;

public class QuadColl : MonoBehaviour
{
	public Material _material;

	private MeshFilter _meshFilter;

	private MeshRenderer _meshRenderer;

	public void Setup(Material material)
	{
		_material = material;
		_meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
		_meshRenderer.material = _material;
		_meshFilter = base.gameObject.AddComponent<MeshFilter>();
		_meshFilter.mesh = new Mesh();
		_meshFilter.mesh.Clear();
		Vector3[] array = new Vector3[4];
		Vector2[] array2 = new Vector2[4];
		array[0] = new Vector3(-0.5f, -0.5f, 0f);
		array[1] = new Vector3(0.5f, -0.5f, 0f);
		array[2] = new Vector3(0.5f, 0.5f, 0f);
		array[3] = new Vector3(-0.5f, 0.5f, 0f);
		array2[0] = new Vector2(0f, 0f);
		array2[1] = new Vector2(1f, 0f);
		array2[2] = new Vector2(1f, 1f);
		array2[3] = new Vector2(0f, 1f);
		_meshFilter.mesh.vertices = array;
		_meshFilter.mesh.uv = array2;
		int[] triangles = new int[6]
		{
			2,
			1,
			0,
			3,
			2,
			0
		};
		_meshFilter.mesh.triangles = triangles;
		MeshCollider meshCollider = GetComponent<Collider>() as MeshCollider;
		if (meshCollider != null)
		{
			UnityEngine.Debug.Log("Setting collider mesh");
			meshCollider.sharedMesh = _meshFilter.sharedMesh;
		}
	}

	public void Awake()
	{
		if (_material != null)
		{
			Setup(_material);
		}
	}
}

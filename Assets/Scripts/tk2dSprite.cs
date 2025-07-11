using UnityEngine;

[AddComponentMenu("2D Toolkit/Sprite/tk2dSprite")]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class tk2dSprite : tk2dBaseSprite
{
	private Mesh mesh;

	private Vector3[] meshVertices;

	private Vector3[] meshNormals;

	private Vector4[] meshTangents;

	private Color32[] meshColors;

	private new void Awake()
	{
		base.Awake();
		mesh = new Mesh();
		mesh.MarkDynamic();
		mesh.hideFlags = HideFlags.DontSave;
		GetComponent<MeshFilter>().mesh = mesh;
		if ((bool)base.Collection)
		{
			if (_spriteId < 0 || _spriteId >= base.Collection.Count)
			{
				_spriteId = 0;
			}
			Build();
		}
	}

	protected void OnDestroy()
	{
		if ((bool)mesh)
		{
			UnityEngine.Object.Destroy(mesh);
		}
		if ((bool)meshColliderMesh)
		{
			UnityEngine.Object.Destroy(meshColliderMesh);
		}
	}

	public override void Build()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = collectionInst.spriteDefinitions[base.spriteId];
		meshVertices = new Vector3[tk2dSpriteDefinition.positions.Length];
		meshColors = new Color32[tk2dSpriteDefinition.positions.Length];
		meshNormals = new Vector3[0];
		meshTangents = new Vector4[0];
		if (tk2dSpriteDefinition.normals != null && tk2dSpriteDefinition.normals.Length > 0)
		{
			meshNormals = new Vector3[tk2dSpriteDefinition.normals.Length];
		}
		if (tk2dSpriteDefinition.tangents != null && tk2dSpriteDefinition.tangents.Length > 0)
		{
			meshTangents = new Vector4[tk2dSpriteDefinition.tangents.Length];
		}
		SetPositions(meshVertices, meshNormals, meshTangents);
		SetColors(meshColors);
		if (mesh == null)
		{
			mesh = new Mesh();
			mesh.MarkDynamic();
			mesh.hideFlags = HideFlags.DontSave;
			GetComponent<MeshFilter>().mesh = mesh;
		}
		mesh.Clear();
		mesh.vertices = meshVertices;
		mesh.normals = meshNormals;
		mesh.tangents = meshTangents;
		mesh.colors32 = meshColors;
		mesh.uv = tk2dSpriteDefinition.uvs;
		mesh.triangles = tk2dSpriteDefinition.indices;
		mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(GetBounds(), renderLayer);
		UpdateMaterial();
		CreateCollider();
	}

	public static tk2dSprite AddComponent(GameObject go, tk2dSpriteCollectionData spriteCollection, int spriteId)
	{
		return tk2dBaseSprite.AddComponent<tk2dSprite>(go, spriteCollection, spriteId);
	}

	public static tk2dSprite AddComponent(GameObject go, tk2dSpriteCollectionData spriteCollection, string spriteName)
	{
		return tk2dBaseSprite.AddComponent<tk2dSprite>(go, spriteCollection, spriteName);
	}

	public static GameObject CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, Rect region, Vector2 anchor)
	{
		return tk2dBaseSprite.CreateFromTexture<tk2dSprite>(texture, size, region, anchor);
	}

	protected override void UpdateGeometry()
	{
		UpdateGeometryImpl();
	}

	protected override void UpdateColors()
	{
		UpdateColorsImpl();
	}

	protected override void UpdateVertices()
	{
		UpdateVerticesImpl();
	}

	protected void UpdateColorsImpl()
	{
		if (!(mesh == null) && meshColors != null && meshColors.Length != 0)
		{
			SetColors(meshColors);
			mesh.colors32 = meshColors;
		}
	}

	protected void UpdateVerticesImpl()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition = collectionInst.spriteDefinitions[base.spriteId];
		if (!(mesh == null) && meshVertices != null && meshVertices.Length != 0)
		{
			if (tk2dSpriteDefinition.normals.Length != meshNormals.Length)
			{
				meshNormals = ((tk2dSpriteDefinition.normals == null || tk2dSpriteDefinition.normals.Length <= 0) ? new Vector3[0] : new Vector3[tk2dSpriteDefinition.normals.Length]);
			}
			if (tk2dSpriteDefinition.tangents.Length != meshTangents.Length)
			{
				meshTangents = ((tk2dSpriteDefinition.tangents == null || tk2dSpriteDefinition.tangents.Length <= 0) ? new Vector4[0] : new Vector4[tk2dSpriteDefinition.tangents.Length]);
			}
			SetPositions(meshVertices, meshNormals, meshTangents);
			mesh.vertices = meshVertices;
			mesh.normals = meshNormals;
			mesh.tangents = meshTangents;
			mesh.uv = tk2dSpriteDefinition.uvs;
			mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(GetBounds(), renderLayer);
		}
	}

	protected void UpdateGeometryImpl()
	{
		if (!(mesh == null))
		{
			tk2dSpriteDefinition tk2dSpriteDefinition = collectionInst.spriteDefinitions[base.spriteId];
			if (meshVertices == null || meshVertices.Length != tk2dSpriteDefinition.positions.Length)
			{
				meshVertices = new Vector3[tk2dSpriteDefinition.positions.Length];
				meshColors = new Color32[tk2dSpriteDefinition.positions.Length];
			}
			if (meshNormals == null || (tk2dSpriteDefinition.normals != null && meshNormals.Length != tk2dSpriteDefinition.normals.Length))
			{
				meshNormals = new Vector3[tk2dSpriteDefinition.normals.Length];
			}
			else if (tk2dSpriteDefinition.normals == null)
			{
				meshNormals = new Vector3[0];
			}
			if (meshTangents == null || (tk2dSpriteDefinition.tangents != null && meshTangents.Length != tk2dSpriteDefinition.tangents.Length))
			{
				meshTangents = new Vector4[tk2dSpriteDefinition.tangents.Length];
			}
			else if (tk2dSpriteDefinition.tangents == null)
			{
				meshTangents = new Vector4[0];
			}
			SetPositions(meshVertices, meshNormals, meshTangents);
			SetColors(meshColors);
			mesh.Clear();
			mesh.vertices = meshVertices;
			mesh.normals = meshNormals;
			mesh.tangents = meshTangents;
			mesh.colors32 = meshColors;
			mesh.uv = tk2dSpriteDefinition.uvs;
			mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(GetBounds(), renderLayer);
			mesh.triangles = tk2dSpriteDefinition.indices;
		}
	}

	protected override void UpdateMaterial()
	{
		Renderer component = GetComponent<Renderer>();
		if (component.sharedMaterial != collectionInst.spriteDefinitions[base.spriteId].materialInst)
		{
			component.material = collectionInst.spriteDefinitions[base.spriteId].materialInst;
		}
	}

	protected override int GetCurrentVertexCount()
	{
		if (meshVertices == null)
		{
			return 0;
		}
		return meshVertices.Length;
	}

	public override void ForceBuild()
	{
		base.ForceBuild();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	public override void ReshapeBounds(Vector3 dMin, Vector3 dMax)
	{
		float num = 0.1f;
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		Vector3 b = new Vector3(Mathf.Abs(_scale.x), Mathf.Abs(_scale.y), Mathf.Abs(_scale.z));
		Vector3 b2 = Vector3.Scale(currentSprite.untrimmedBoundsData[0], _scale) - 0.5f * Vector3.Scale(currentSprite.untrimmedBoundsData[1], b);
		Vector3 a = Vector3.Scale(currentSprite.untrimmedBoundsData[1], b);
		Vector3 vector = a + dMax - dMin;
		vector.x /= currentSprite.untrimmedBoundsData[1].x;
		vector.y /= currentSprite.untrimmedBoundsData[1].y;
		if (currentSprite.untrimmedBoundsData[1].x * vector.x < currentSprite.texelSize.x * num && vector.x < b.x)
		{
			dMin.x = 0f;
			vector.x = b.x;
		}
		if (currentSprite.untrimmedBoundsData[1].y * vector.y < currentSprite.texelSize.y * num && vector.y < b.y)
		{
			dMin.y = 0f;
			vector.y = b.y;
		}
		Vector2 vector2 = new Vector3((!Mathf.Approximately(b.x, 0f)) ? (vector.x / b.x) : 0f, (!Mathf.Approximately(b.y, 0f)) ? (vector.y / b.y) : 0f);
		Vector3 b3 = new Vector3(b2.x * vector2.x, b2.y * vector2.y);
		Vector3 position = dMin + b2 - b3;
		position.z = 0f;
		base.transform.position = base.transform.TransformPoint(position);
		base.scale = new Vector3(_scale.x * vector2.x, _scale.y * vector2.y, _scale.z);
	}
}

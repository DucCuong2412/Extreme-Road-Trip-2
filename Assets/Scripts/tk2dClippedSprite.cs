using UnityEngine;

[AddComponentMenu("2D Toolkit/Sprite/tk2dClippedSprite")]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class tk2dClippedSprite : tk2dBaseSprite
{
	private Mesh mesh;

	private Vector2[] meshUvs;

	private Vector3[] meshVertices;

	private Color32[] meshColors;

	private Vector3[] meshNormals;

	private Vector4[] meshTangents;

	private int[] meshIndices;

	public Vector2 _clipBottomLeft = new Vector2(0f, 0f);

	public Vector2 _clipTopRight = new Vector2(1f, 1f);

	private Rect _clipRect = new Rect(0f, 0f, 0f, 0f);

	[SerializeField]
	protected bool _createBoxCollider;

	private Vector3 boundsCenter = Vector3.zero;

	private Vector3 boundsExtents = Vector3.zero;

	public Rect ClipRect
	{
		get
		{
			_clipRect.Set(_clipBottomLeft.x, _clipBottomLeft.y, _clipTopRight.x - _clipBottomLeft.x, _clipTopRight.y - _clipBottomLeft.y);
			return _clipRect;
		}
		set
		{
			Vector2 clipTopRight = clipBottomLeft = new Vector2(value.x, value.y);
			clipTopRight.x += value.width;
			clipTopRight.y += value.height;
			this.clipTopRight = clipTopRight;
		}
	}

	public Vector2 clipBottomLeft
	{
		get
		{
			return _clipBottomLeft;
		}
		set
		{
			if (value != _clipBottomLeft)
			{
				_clipBottomLeft = new Vector2(value.x, value.y);
				Build();
				UpdateCollider();
			}
		}
	}

	public Vector2 clipTopRight
	{
		get
		{
			return _clipTopRight;
		}
		set
		{
			if (value != _clipTopRight)
			{
				_clipTopRight = new Vector2(value.x, value.y);
				Build();
				UpdateCollider();
			}
		}
	}

	public bool CreateBoxCollider
	{
		get
		{
			return _createBoxCollider;
		}
		set
		{
			if (_createBoxCollider != value)
			{
				_createBoxCollider = value;
				UpdateCollider();
			}
		}
	}

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
	}

	protected new void SetColors(Color32[] dest)
	{
		if (base.CurrentSprite.positions.Length == 4)
		{
			tk2dSpriteGeomGen.SetSpriteColors(dest, 0, 4, _color, collectionInst.premultipliedAlpha);
		}
	}

	protected void SetGeometry(Vector3[] vertices, Vector2[] uvs)
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		float num;
		if (boxCollider != null)
		{
			Vector3 center = boxCollider.center;
			num = center.z;
		}
		else
		{
			num = 0f;
		}
		float colliderOffsetZ = num;
		float num2;
		if (boxCollider != null)
		{
			Vector3 size = boxCollider.size;
			num2 = size.z * 0.5f;
		}
		else
		{
			num2 = 0.5f;
		}
		float colliderExtentZ = num2;
		tk2dSpriteGeomGen.SetClippedSpriteGeom(meshVertices, meshUvs, 0, out boundsCenter, out boundsExtents, currentSprite, _scale, _clipBottomLeft, _clipTopRight, colliderOffsetZ, colliderExtentZ);
		if (meshNormals.Length > 0 || meshTangents.Length > 0)
		{
			tk2dSpriteGeomGen.SetSpriteVertexNormals(meshVertices, meshVertices[0], meshVertices[3], currentSprite.normals, currentSprite.tangents, meshNormals, meshTangents);
		}
		if (currentSprite.positions.Length != 4 || currentSprite.complexGeometry)
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = Vector3.zero;
			}
		}
	}

	public override void Build()
	{
		tk2dSpriteDefinition currentSprite = base.CurrentSprite;
		meshUvs = new Vector2[4];
		meshVertices = new Vector3[4];
		meshColors = new Color32[4];
		meshNormals = new Vector3[0];
		meshTangents = new Vector4[0];
		if (currentSprite.normals != null && currentSprite.normals.Length > 0)
		{
			meshNormals = new Vector3[4];
		}
		if (currentSprite.tangents != null && currentSprite.tangents.Length > 0)
		{
			meshTangents = new Vector4[4];
		}
		SetGeometry(meshVertices, meshUvs);
		SetColors(meshColors);
		if (mesh == null)
		{
			mesh = new Mesh();
			mesh.MarkDynamic();
			mesh.hideFlags = HideFlags.DontSave;
		}
		else
		{
			mesh.Clear();
		}
		mesh.vertices = meshVertices;
		mesh.colors32 = meshColors;
		mesh.uv = meshUvs;
		mesh.normals = meshNormals;
		mesh.tangents = meshTangents;
		int[] array = new int[6];
		tk2dSpriteGeomGen.SetClippedSpriteIndices(array, 0, 0, base.CurrentSprite);
		mesh.triangles = array;
		mesh.RecalculateBounds();
		mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(mesh.bounds, renderLayer);
		GetComponent<MeshFilter>().mesh = mesh;
		UpdateCollider();
		UpdateMaterial();
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
		UpdateGeometryImpl();
	}

	protected void UpdateColorsImpl()
	{
		if (meshColors == null || meshColors.Length == 0)
		{
			Build();
			return;
		}
		SetColors(meshColors);
		mesh.colors32 = meshColors;
	}

	protected void UpdateGeometryImpl()
	{
		if (meshVertices == null || meshVertices.Length == 0)
		{
			Build();
			return;
		}
		SetGeometry(meshVertices, meshUvs);
		mesh.vertices = meshVertices;
		mesh.uv = meshUvs;
		mesh.normals = meshNormals;
		mesh.tangents = meshTangents;
		mesh.RecalculateBounds();
		mesh.bounds = tk2dBaseSprite.AdjustedMeshBounds(mesh.bounds, renderLayer);
	}

	protected override void UpdateCollider()
	{
		if (!CreateBoxCollider)
		{
			return;
		}
		if (base.CurrentSprite.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics3D)
		{
			if (boxCollider != null)
			{
				boxCollider.size = 2f * boundsExtents;
				boxCollider.center = boundsCenter;
			}
		}
		else if (base.CurrentSprite.physicsEngine == tk2dSpriteDefinition.PhysicsEngine.Physics2D && boxCollider2D != null)
		{
			boxCollider2D.size = 2f * boundsExtents;
			boxCollider2D.offset = boundsCenter;
		}
	}

	protected override void CreateCollider()
	{
		UpdateCollider();
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
		return 4;
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

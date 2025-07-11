using UnityEngine;

public class PuppetLabel : MonoBehaviour
{
	private TextMesh[] _textMeshes;

	private tk2dSprite _triangle;

	private Transform _transform;

	private Transform _pivot;

	private string _text;

	public void Awake()
	{
		_transform = base.transform;
		_pivot = _transform.Find("Pivot");
		_textMeshes = GetComponentsInChildren<TextMesh>();
		_triangle = GetComponentInChildren<tk2dSprite>();
	}

	public void Start()
	{
		SetText(_text);
	}

	public void SetVisible(bool visible)
	{
		base.gameObject.SetActive(visible);
	}

	private void SetText(string text)
	{
		if (text != null)
		{
			TextMesh[] textMeshes = _textMeshes;
			foreach (TextMesh textMesh in textMeshes)
			{
				textMesh.text = text;
			}
		}
	}

	public void Update()
	{
		_transform.rotation = Quaternion.identity;
		_pivot.localScale = Vector3.one * PrefabSingleton<CameraGame>.Instance.GetZoomFactor();
		Vector3 position = _transform.position;
		float x = position.x;
		if (x < 80f)
		{
			SetAlpha(0f);
		}
		else if (x < 110f)
		{
			SetAlpha(Mathf.InverseLerp(80f, 110f, x));
		}
		else if (x < 120f)
		{
			SetAlpha(1f);
		}
	}

	private void SetAlpha(float alpha)
	{
		TextMesh[] textMeshes = _textMeshes;
		foreach (TextMesh textMesh in textMeshes)
		{
			Renderer component = textMesh.GetComponent<Renderer>();
			Color color = component.material.color;
			color.a = alpha;
			component.material.color = color;
		}
		_triangle.color = new Color(1f, 1f, 1f, alpha);
	}

	public static PuppetLabel Create(Transform attach, string text)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("PuppetLabel"), Vector3.zero, Quaternion.identity);
		if (gameObject != null)
		{
			PuppetLabel component = gameObject.GetComponent<PuppetLabel>();
			component.transform.parent = attach;
			component._text = text;
			return component;
		}
		return null;
	}
}

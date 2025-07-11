using System.Collections;
using UnityEngine;

public class MetroMenuTutorial : MetroMenuPage
{
	private enum TutorialStep
	{
		stepIntro,
		stepControls,
		stepBoost,
		stepSlam,
		stepDone
	}

	public Transform[] _tutorialTransforms;

	private MetroLabel _title;

	private MetroLabel _description;

	private MetroSpacer _tutorialPivot;

	private MetroButton _prevBtn;

	private MetroButton _nextBtn;

	private TutorialStep _step;

	private bool _isAnimating;

	private float _xOffset;

	protected override void OnAwake()
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		_title = MetroLabel.Create("TITLE");
		_title.SetColor(MetroSkin.TextNormalColor, MetroSkin.TextOutlineColor);
		metroLayout.Add(_title);
		_tutorialPivot = MetroSpacer.Create(7f);
		metroLayout.Add(_tutorialPivot);
		_description = MetroLabel.Create("DESCRIPTION");
		_description.SetFont(MetroSkin.MediumFont);
		_description.SetColor(MetroSkin.TextNormalColor, MetroSkin.TextOutlineColor);
		metroLayout.Add(_description);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
		metroLayout2.SetMass(2f);
		metroLayout.Add(metroLayout2);
		_prevBtn = MetroSkin.CreateMenuButton(MetroSkin.IconBack, "PREVIOUS");
		_prevBtn.OnButtonClicked += PrevBtnCmd;
		metroLayout2.Add(_prevBtn);
		metroLayout2.Add(MetroSpacer.Create(3f));
		_nextBtn = MetroSkin.CreateMenuButton(MetroSkin.IconPlay, "NEXT", MetroSkin.Slice9ButtonRed);
		_nextBtn.SetGradient(MetroSkin.ButtonColorAlert1, MetroSkin.ButtonColorAlert2);
		_nextBtn.OnButtonClicked += NextBtnCmd;
		metroLayout2.Add(_nextBtn);
		base.OnAwake();
	}

	protected override void OnStart()
	{
		_xOffset = 2f * PrefabSingleton<CameraGUI>.Instance.ScreenWidth;
		for (int i = 0; i < _tutorialTransforms.Length; i++)
		{
			_tutorialTransforms[i].parent = _tutorialPivot.transform;
			_tutorialTransforms[i].localPosition = new Vector3((float)i * _xOffset, 0f, 0f);
		}
		UpdateUI();
		base.OnStart();
	}

	private void PrevBtnCmd()
	{
		if (!_isAnimating)
		{
			_step--;
			HandleNewStep(next: false);
		}
	}

	private void NextBtnCmd()
	{
		if (!_isAnimating)
		{
			_step++;
			HandleNewStep(next: true);
		}
	}

	private void HandleNewStep(bool next)
	{
		if (_step != TutorialStep.stepDone)
		{
			StartCoroutine(LerpTutorial(next));
		}
		UpdateUI();
	}

	private void UpdateUI()
	{
		_prevBtn.SetActive(_step > TutorialStep.stepIntro);
		_prevBtn.gameObject.SetActive(_step > TutorialStep.stepIntro);
		_nextBtn.SetText((_step < TutorialStep.stepSlam) ? "NEXT" : "PLAY");
		switch (_step)
		{
		case TutorialStep.stepIntro:
			StepIntro();
			break;
		case TutorialStep.stepControls:
			StepControls();
			break;
		case TutorialStep.stepBoost:
			StepBoost();
			break;
		case TutorialStep.stepSlam:
			StepSlam();
			break;
		case TutorialStep.stepDone:
			StepDone();
			break;
		}
	}

	private void StepIntro()
	{
		_title.SetText("Your gas pedal is stuck!");
		_description.SetText("Try to get as far as you can before crashing!");
	}

	private void StepControls()
	{
		_title.SetText("Press left and right to tilt your car.");
		_description.SetText("Take it easy! Don't hold for too long.");
	}

	private void StepBoost()
	{
		_title.SetText("Do stunts to get a nitro boost.");
		_description.SetText("Land BOTH WHEELS at the SAME TIME for EXTRA BOOST!");
	}

	private void StepSlam()
	{
		_title.SetText("Press left and right at the same time to slam.");
		_description.SetText("PERFECT SLAMS are the key to achieving OVERDRIVE.");
	}

	private void StepDone()
	{
		LoadConfigTutorial loadConfigTutorial = AutoSingleton<LoadingManager>.Instance.GetCurrentConfig() as LoadConfigTutorial;
		AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(loadConfigTutorial?.NextMenuPage ?? LoadConfigMenu.NextMenuPage.main));
	}

	private IEnumerator LerpTutorial(bool next)
	{
		_isAnimating = true;
		Transform[] tutorialTransforms = _tutorialTransforms;
		foreach (Transform tutorial in tutorialTransforms)
		{
			tutorial.GetComponent<Tutorial>().Stop();
		}
		Transform t = _tutorialPivot.transform;
		Vector3 from = t.position;
		Vector3 to = new Vector3((!next) ? (from.x + _xOffset) : (from.x - _xOffset), from.y, from.z);
		Duration delay = new Duration(0.4f);
		t.position = from;
		while (!delay.IsDone())
		{
			//t.position = Vector3.Lerp(t: Mathfx.Hermite(0f, 1f, delay.Value01()), from: from, to: to);
			yield return null;
		}
		t.position = to;
		Transform[] tutorialTransforms2 = _tutorialTransforms;
		foreach (Transform tutorial2 in tutorialTransforms2)
		{
			tutorial2.GetComponent<Tutorial>().Start();
		}
		_isAnimating = false;
	}
}

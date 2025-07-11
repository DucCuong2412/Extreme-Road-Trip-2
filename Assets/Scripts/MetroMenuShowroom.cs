using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroMenuShowroom : MetroMenuPage
{
	private List<MetroButtonShowroomCar> _buttons;

	private MetroLayout _bottomLayout;

	private bool _showNotLoggedPopup = true;

	private MetroLayout _flashWidget;

	protected override void OnAwake()
	{
		AutoSingleton<PersistenceManager>.Instance.HasSeenShowroom = true;
		LoadConfigShowroom loadConfigShowroom = AutoSingleton<LoadingManager>.Instance.GetCurrentConfig() as LoadConfigShowroom;
		bool isLocalShowroom = loadConfigShowroom?.Setup.LocalShowroom ?? true;
		_buttons = new List<MetroButtonShowroomCar>();
		MetroLayout metroLayout = MetroLayout.Create(Direction.vertical);
		Add(metroLayout);
		MetroLayout metroLayout2 = MetroLayout.Create(Direction.vertical);
		metroLayout.Add(metroLayout2);
		metroLayout2.SetMass(8f);
		metroLayout2.Add(CreateShowroom());
		_bottomLayout = MetroLayout.Create(Direction.vertical);
		metroLayout.Add(_bottomLayout);
		_bottomLayout.SetMass(2f);
		MetroLayout metroLayout3 = MetroLayout.Create(Direction.horizontal);
		_bottomLayout.Add(metroLayout3);
		metroLayout3.SetMass(2f);
		MetroButton metroButton = MetroSkin.CreateMenuButton(MetroSkin.IconBack, "BACK", MetroSkin.Slice9Button);
		metroLayout3.Add(metroButton);
		metroButton.OnButtonClicked += delegate
		{
			if (isLocalShowroom)
			{
				AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigMenu(LoadConfigMenu.NextMenuPage.options));
				AutoSingleton<ShowroomManager>.Instance.SaveBackend();
			}
			else
			{
				AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigShowroom(AutoSingleton<ShowroomManager>.Instance.CurrentSetup));
			}
		};
		MetroButton metroButton2 = MetroSkin.CreateMenuButton(MetroSkin.IconFriends, "FRIENDS");
		metroLayout3.Add(metroButton2);
		metroButton2.OnButtonClicked += delegate
		{
			AutoSingleton<MetroMenuStack>.Instance.Push(MetroMenuPage.Create<MetroPopupSelectFriendShowroom>().Setup(), MetroAnimation.popup);
		};
		if (isLocalShowroom)
		{
			if (AutoSingleton<PlatformCapabilities>.Instance.UseSocialSharing())
			{
				MetroButton metroButton3 = MetroSkin.CreateMenuButton(MetroSkin.IconCamera, "SHARE");
				metroLayout3.Add(metroButton3);
				metroButton3.OnButtonClicked += delegate
				{
					StartCoroutine(ShareShowroom());
				};
			}
			MetroButton metroButton4 = MetroSkin.CreateMenuButton(MetroSkin.IconShowroom, "LOCATIONS");
			metroLayout3.Add(metroButton4);
			metroButton4.OnButtonClicked += delegate
			{
				AutoSingleton<LoadingManager>.Instance.Load(new LoadConfigShowroom(LoadConfigShowroom.NextMenuPage.chooseShowroom));
			};
		}
		if (loadConfigShowroom != null)
		{
			AutoSingleton<LeaderboardsManager>.Instance.SubmitShowroomValue(loadConfigShowroom.Setup.GetValue());
		}
		base.OnAwake();
	}

	public override void OnFocus()
	{
		base.OnFocus();
		ShowNoLoggedInPopup();
	}

	private void ShowNoLoggedInPopup()
	{
		if (!AutoSingleton<BackendManager>.Instance.IsLoggedIn() && _showNotLoggedPopup && AutoSingleton<PersistenceManager>.Instance.ShowroomNotLoggedInPopup)
		{
			_showNotLoggedPopup = false;
			bool usingFacebook = AutoSingleton<PlatformCapabilities>.Instance.UseFacebookAsSocialPlatform();
			string str = (!usingFacebook) ? "Game Center" : "Facebook";
			string titleString = "NOT LOGGED IN";
			string messageString = "You must be logged in to " + str + " in order to share and see your friend's showroom. Would you like to login now?";
			string buttonString = "LOG IN";
			Action buttonAction = delegate
			{
				if (usingFacebook)
				{
					Action onLoginWithPermissionSucceeded = delegate
					{
						AutoSingleton<BackendManager>.Instance.Authenticate(SocialPlatform.facebook);
					};
					AutoSingleton<GameFacebookManager>.Instance.Login(onLoginWithPermissionSucceeded, isPublishNeeded: false);
				}
				AutoSingleton<MetroMenuStack>.Instance.Pop(MetroAnimation.popup);
			};
			MetroPopupMessage page = MetroMenuPage.Create<MetroPopupMessage>().Setup(titleString, messageString, buttonString, MetroSkin.Slice9ButtonRed, buttonAction);
			AutoSingleton<MetroMenuStack>.Instance.Push(page, MetroAnimation.popup);
		}
	}

	private MetroWidget CreateShowroom()
	{
		MetroWidget metroWidget = MetroLayout.Create(Direction.vertical);
		LoadConfigShowroom loadConfigShowroom = AutoSingleton<LoadingManager>.Instance.GetCurrentConfig() as LoadConfigShowroom;
		List<CarLevel> list;
		Showroom showroom;
		bool flag;
		int showroomValue;
		string text;
		if (loadConfigShowroom != null)
		{
			ShowroomSetup setup = loadConfigShowroom.Setup;
			list = setup.Cars;
			showroom = setup.Showroom;
			flag = setup.LocalShowroom;
			showroomValue = setup.GetValue();
			text = ((!flag) ? setup.OwnerName : AutoSingleton<BackendManager>.Instance.PlayerAlias());
		}
		else
		{
			List<CarLevel> list2 = new List<CarLevel>();
			list2.Add(new CarLevel(AutoSingleton<CarManager>.Instance.GetRandomCar(), 0));
			list2.Add(new CarLevel(null, 0));
			list2.Add(new CarLevel(AutoSingleton<CarManager>.Instance.GetRandomCar(), 3));
			list = list2;
			showroom = AutoSingleton<ShowroomManager>.Instance.CurrentSetup.Showroom;
			text = "DEBUG";
			flag = true;
			showroomValue = 1234;
		}
		if (text != null && text != string.Empty)
		{
			metroWidget.Add(CreateShowroomTitleWidget(text));
		}
		metroWidget.Add(CreateShowroomValueWidget(showroomValue));
		metroWidget.Add(MetroSpacer.Create(7f));
		Transform transform = (Transform)UnityEngine.Object.Instantiate(showroom.transform, Vector3.zero, Quaternion.identity);
		float screenWidth = PrefabSingleton<CameraGUI>.Instance.ScreenWidth;
		float num = 48f;
		float carPositionOffset = (screenWidth - num) / num + 1f;
		if (transform != null)
		{
			transform.parent = metroWidget.transform;
			Transform transform2 = transform.Find("ShowroomBG");
			transform2.localScale = new Vector3(screenWidth, screenWidth, 1f);
			Transform transform3 = transform.Find("CarSlots");
			if (transform3 != null)
			{
				for (int i = 0; i < transform3.childCount; i++)
				{
					Transform transform4 = transform3.Find("CarSlot" + i.ToString());
					CarLevel carLevel = (list.Count <= i) ? new CarLevel(null, 0) : list[i];
					if (!flag && carLevel._car == null)
					{
						Transform child = transform4.GetChild(0);
						UnityEngine.Object.Destroy(child.gameObject);
						continue;
					}
					int upgradeLevel = (!flag || carLevel._car == null) ? carLevel._level : AutoSingleton<CarManager>.Instance.GetCarProfile(carLevel._car).GetUpgradeLevel();
					MetroButtonShowroomCar metroButtonShowroomCar = MetroButtonShowroomCar.Create(transform4, carLevel._car, upgradeLevel, i, carPositionOffset, showroom._carIconScale);
					metroButtonShowroomCar.SetActive(flag);
					_buttons.Add(metroButtonShowroomCar);
				}
			}
		}
		return metroWidget;
	}

	private MetroWidget CreateShowroomTitleWidget(string ownerName)
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		metroLayout.AddSolidBackground().SetColor(new Color(1f, 1f, 1f, 0.75f));
		metroLayout.SetPadding(0f);
		string text = string.Empty;
		string text2 = StringUtil.Trunc(ownerName, 12);
		if (AutoSingleton<LocalizationManager>.Instance.Language == LanguageType.english)
		{
			text = text2 + "'S SHOWROOM";
		}
		else if (AutoSingleton<LocalizationManager>.Instance.Language == LanguageType.french)
		{
			text = "EXPOSITION DE " + text2;
		}
		MetroLabel metroLabel = MetroLabel.Create(text.ToUpper());
		metroLayout.Add(metroLabel);
		metroLabel.SetFont(MetroSkin.BigFont);
		metroLabel.SetColor(Color.black);
		return metroLayout;
	}

	private MetroWidget CreateShowroomValueWidget(int showroomValue)
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		metroLayout.AddSolidBackground().SetColor(new Color(1f, 1f, 1f, 0.75f));
		metroLayout.SetMass(0.8f);
		metroLayout.SetPadding(0f);
		MetroLabel metroLabel = MetroLabel.Create("Score: ".Localize() + showroomValue.ToString());
		metroLayout.Add(metroLabel);
		metroLabel.SetFont(MetroSkin.MediumFont);
		metroLabel.SetColor(Color.black);
		return metroLayout;
	}

	protected override void HandleFinger(Finger finger)
	{
		if (IsActive())
		{
			foreach (MetroButtonShowroomCar button in _buttons)
			{
				button.OnFinger(finger);
			}
		}
		base.HandleFinger(finger);
	}

	private IEnumerator ShareShowroom()
	{
		List<float> slotStartPosY = new List<float>();
		List<float> slotEndPosY = new List<float>();
		foreach (MetroButtonShowroomCar button2 in _buttons)
		{
			if (button2.Car == null)
			{
				Transform parentTransform = button2.transform.parent.transform;
				List<float> list = slotStartPosY;
				Vector3 localPosition = parentTransform.localPosition;
				list.Add(localPosition.y);
				List<float> list2 = slotEndPosY;
				float num = 0f - PrefabSingleton<CameraGUI>.Instance.HalfScreenHeight;
				Vector3 size = RendererBounds.ComputeBounds(parentTransform).size;
				list2.Add(num - size.y * 3f);
				StartCoroutine(LerpFromTo(parentTransform, slotStartPosY[slotStartPosY.Count - 1], slotEndPosY[slotEndPosY.Count - 1]));
			}
		}
		Vector3 localPosition2 = _bottomLayout.transform.localPosition;
		float buttonStartPosY = localPosition2.y;
		float num2 = 0f - PrefabSingleton<CameraGUI>.Instance.HalfScreenHeight;
		Vector3 size2 = RendererBounds.ComputeBounds(_bottomLayout.transform).size;
		float buttonEndPosY = num2 - size2.y;
		yield return StartCoroutine(LerpFromTo(_bottomLayout.transform, buttonStartPosY, buttonEndPosY));
		yield return StartCoroutine(Flash());
		AutoSingleton<SharingUtil>.Instance.ShareScreenshot(SocialHelper.GetSharingShowroomString());
		int i = 0;
		foreach (MetroButtonShowroomCar button in _buttons)
		{
			if (button.Car == null)
			{
				StartCoroutine(LerpFromTo(button.transform.parent.transform, slotEndPosY[i], slotStartPosY[i]));
				i++;
			}
		}
		yield return StartCoroutine(LerpFromTo(_bottomLayout.transform, buttonEndPosY, buttonStartPosY));
		OnShowRoomShared();
	}

	public void OnShowRoomShared()
	{
		UnityEngine.Debug.Log("Showroom shared");
		int value = AutoSingleton<ShowroomManager>.Instance.CurrentSetup.GetValue();
		ShareShowroomAchievement(value);
	}

	private void ShareShowroomAchievement(int value)
	{
		AutoSingleton<AchievementsManager>.Instance.ShareShowroom(value);
	}

	private IEnumerator LerpFromTo(Transform transform, float startPosY, float endPosY)
	{
		Duration delay = new Duration(0.3f);
		Vector3 transformPos = transform.localPosition;
		transform.localPosition = new Vector3(transformPos.x, startPosY, transformPos.z);
		while (!delay.IsDone())
		{
			transform.localPosition = new Vector3(y: Mathfx.Hermite(startPosY, endPosY, delay.Value01()), x: transformPos.x, z: transformPos.z);
			yield return null;
		}
		transform.localPosition = new Vector3(transformPos.x, endPosY, transformPos.z);
	}

	private IEnumerator Flash()
	{
		PrefabSingleton<GameSoundManager>.Instance.PlayFlashSound();
		_flashWidget = MetroLayout.Create(Direction.horizontal);
		_flashWidget.AddSolidBackground().SetColor(new Color(1f, 1f, 1f, 0f));
		_flashWidget.Layout(_zone);
		Vector3 widgetPos = _flashWidget.transform.localPosition;
		_flashWidget.transform.localPosition = new Vector3(widgetPos.x, widgetPos.y, GetZ() * 2f);
		Color flashColor = new Color(1f, 1f, 1f, 0.8f);
		Color currentColor = new Color(1f, 1f, 1f, 0f);
		yield return StartCoroutine(Fade(currentColor, flashColor, 0.2f));
		yield return StartCoroutine(Fade(flashColor, currentColor, 0.6f));
		_flashWidget.Destroy();
	}

	private IEnumerator Fade(Color color1, Color color2, float duration)
	{
		Duration delay = new Duration(duration);
		while (!delay.IsDone())
		{
			_flashWidget.SetColor(Color.Lerp(color1, color2, delay.Value01()));
			yield return null;
		}
	}
}

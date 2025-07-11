using System;
using System.Collections.Generic;
using UnityEngine;

public class MetroPopupRewards : MetroPopupPage
{
	public MetroPopupRewards Setup(string titleStr, string subtitleStr, List<Reward> rewards, Action onDismiss, string iconName = null)
	{
		_useCloseButton = false;
		AddSlice9Background(MetroSkin.Slice9PopupBackground);
		MetroLayout layout = MetroLayout.Create(Direction.vertical);
		Add(layout);
		layout.Add(MetroSpacer.Create(0.2f));
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		layout.Add(metroLayout);
		if (iconName != null)
		{
			MetroIcon child = MetroIcon.Create(iconName);
			metroLayout.Add(child);
		}
		MetroWidget child2 = MetroLabel.Create(titleStr).SetFont(MetroSkin.BigFont).SetColor(MetroSkin.TextAlertColor);
		metroLayout.Add(child2);
		if (iconName != null)
		{
			MetroIcon metroIcon = MetroIcon.Create(iconName);
			Vector3 localScale = metroIcon.transform.localScale;
			localScale.x = 0f - localScale.x;
			metroIcon.transform.localScale = localScale;
			metroLayout.Add(metroIcon);
		}
		string content = StringWrapper.WrapString(subtitleStr, 40, " ");
		MetroLabel child3 = MetroLabel.Create(content).SetFont(MetroSkin.MediumFont);
		layout.Add(child3);
		if (rewards != null && rewards.Count > 0)
		{
			int numberOfRewards = rewards.Count;
			MetroLabel instructions = MetroLabel.Create(string.Format("Break the crate{0} to get your reward!", (numberOfRewards >= 2) ? "s" : string.Empty));
			instructions.SetFont(MetroSkin.MediumFont);
			MetroLayout metroLayout2 = MetroLayout.Create(Direction.horizontal);
			metroLayout2.SetMass(3f);
			layout.Add(metroLayout2);
			metroLayout2.Add(MetroSpacer.Create(0.5f));
			int brokenCrates = 0;
			Action action = null;
			action = delegate
			{
				brokenCrates++;
				if (brokenCrates == numberOfRewards)
				{
					layout.Replace(instructions, MakeNextButton(onDismiss));
					instructions.Destroy();
					layout.Reflow();
				}
			};
			rewards.Shuffle();
			foreach (Reward reward in rewards)
			{
				metroLayout2.Add(MetroWidgetCrate.Create(reward, action, (numberOfRewards <= 3) ? 1f : 0.8f));
			}
			metroLayout2.Add(MetroSpacer.Create(0.5f));
			layout.Add(instructions);
			layout.Add(MetroSpacer.Create(0.2f));
		}
		else
		{
			layout.Add(MakeNextButton(onDismiss));
		}
		return this;
	}

	private MetroWidget MakeNextButton(Action onDismiss)
	{
		MetroLayout metroLayout = MetroLayout.Create(Direction.horizontal);
		metroLayout.Add(MetroSpacer.Create(1f));
		MetroButton metroButton = MetroButton.Create("Next");
		metroButton.AddSlice9Background(MetroSkin.Slice9ButtonRed);
		if (onDismiss != null)
		{
			metroButton.OnButtonClicked += onDismiss;
		}
		metroLayout.Add(metroButton);
		metroLayout.Add(MetroSpacer.Create(1f));
		return metroLayout;
	}
}

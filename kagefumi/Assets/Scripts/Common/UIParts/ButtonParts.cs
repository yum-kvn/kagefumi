using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonParts : BaseUIParts
{
	[SerializeField]
	private Text text;

	private Image image
	{
		get {return GetComponent<Image>();}
	}
	private Button button
	{
		get {return GetComponent<Button>();}
	}

	public System.Action<ButtonParts> buttonClick;

	public virtual void ButtonClick()
	{
		buttonClick(this);
	}

	public bool isEnabled
	{
		set
		{
			button.interactable = value;
		}
	}
}

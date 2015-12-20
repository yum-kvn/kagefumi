using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : GameMonoBehaviour
{
	private StageManager stageManager
	{
		get {return GetComponent<StageManager>();}
	}

	private LightManager lightManager
	{
		get {return GetComponent<LightManager>();}
	}

	private CharacterCamera characterCamera
	{
		get {return Camera.main.gameObject.GetComponent<CharacterCamera>();}
	}

	[SerializeField]
	private GameObject mainCharacterPrefab;
	private MainCharacter mainCharacter;

	private BaseObject diveTarget = null;
	private int stageId;

#region Init
	public void InitGame(object parameter = null)
	{
		if (parameter != null)
		{
			stageId = (int)parameter;
		}

		stageManager.Init(stageId);
		InitMainCharacter(stageManager.characterDefaultPosition);

		characterCamera.Init(mainCharacter.transform);
	}

	private void InitMainCharacter(Vector3 characterDefaultPosition)
	{
		if (mainCharacter == null || mainCharacter.isDead)
		{
			Transform characterTransform = Instantiate(mainCharacterPrefab).transform;
			characterTransform.SetParent(transform);
			mainCharacter = characterTransform.GetComponent<MainCharacter>();
			mainCharacter.Init(CharacterOnUpdate, OnKeyGet);
		}

		mainCharacter.Reset();
		mainCharacter.transform.position = characterDefaultPosition;
	}

	public void PrepareGame()
	{
		characterCamera.CalculateBounds();
		lightManager.Init(stageManager.stageObjects);
	}

	private void Restart()
	{
		InitGame();
		PrepareGame();
	}
#endregion

#region Action
	private void DiveToTarget()
	{
		mainCharacter.SetActive(false);
		diveTarget.Dive();

		characterCamera.SetCharacter(diveTarget.transform);
	}

	private void GetOutFromTarget()
	{
		diveTarget.GetOut();
		mainCharacter.GetOutFromObject(diveTarget.GetOutPosition());

		diveTarget = null;

		characterCamera.SetCharacter(mainCharacter.transform);
	}
#endregion

#region Event
	private void CharacterOnUpdate(Vector3 characterPosition)
	{
		BaseObject shadowObject = lightManager.GetShadowObject(characterPosition);

		if (shadowObject != null && diveTarget != shadowObject)
		{
			shadowObject.StartBlink();
		}

		if (diveTarget != null && diveTarget != shadowObject)
		{
			diveTarget.StopBlink();
		}

		diveTarget = shadowObject;
	}

	public void OnDoubleTap()
	{
		if (diveTarget == null) {return;}

		if (mainCharacter.isActive)
		{
			DiveToTarget();
		}
		else
		{
			GetOutFromTarget();
		}
	}

	public void OnRestartButtonClick()
	{
		Restart();
	}

	private void OnKeyGet()
	{
		stageManager.goalLight.LightOn();
	}
#endregion
}

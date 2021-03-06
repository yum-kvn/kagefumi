using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BaseObject : GameMonoBehaviour
{
	private Dictionary<ShadowDetector, List<Vector2>> shadowPointListDictionary;
	private Dictionary<ShadowDetector, Vector2?> shadowCenterPointDictionary;

	protected MainCharacterController mainCharacterController
	{
		get {return GetComponent<MainCharacterController>();}
	}

	public virtual bool isDivable
	{
		get {return true;}
	}

	private const float FADE_SPEED = 0.25f;

	protected virtual void Awake()
	{
		shadowPointListDictionary = new Dictionary<ShadowDetector, List<Vector2>>();
		shadowCenterPointDictionary = new Dictionary<ShadowDetector, Vector2?>();
	}

	public void SetLayer(string layerName)
	{
		gameObject.layer = LayerMask.NameToLayer(layerName);
	}

	public bool IsSide(Vector3 position)
	{
		return IsSide(position.y);
	}

	public bool IsSide(float positionY)
	{
		return positionY < transform.position.y;
	}

#region Action Event
	public void Dive()
	{
		OnDive();

		if (mainCharacterController == null)
		{
			StopBlink();
			AddMainCharacterController();
		}
	}

	protected virtual void AddMainCharacterController()
	{
		gameObject.AddComponent<MainCharacterController>();
	}

	protected virtual void OnDive() {}

	public void GetOut()
	{
		OnGetOut();

		if (mainCharacterController)
		{
			Destroy(mainCharacterController);
		}
	}

	public virtual Vector3 GetOutPosition()
	{
		foreach (Vector2? point in shadowCenterPointDictionary.Values)
		{
			if (point != null)
			{
				Vector2 shadowCenterPoint = (Vector2)point;
				return new Vector3(shadowCenterPoint.x, 0f, shadowCenterPoint.y);
			}
		}

		foreach (List<Vector2> list in shadowPointListDictionary.Values)
		{
			if (list.Count > 0)
			{
				return new Vector3(list[0].x, 0f, list[0].y);
			}
		}

		return new Vector3(transform.position.x, 0.1f, transform.position.z);
	}

	protected virtual void OnGetOut() {}
#endregion

#region Tween
	public void StartBlink()
	{
		OnBlinkStart();
		LeanTween.cancel(gameObject);
		LeanTween.color(gameObject, Color.red, FADE_SPEED).setLoopPingPong().setRepeat(-1);
	}

	protected virtual void OnBlinkStart() {}

	public void StopBlink()
	{
		OnBlinkStop();
		LeanTween.cancel(gameObject);
		LeanTween.color(gameObject, Color.white, FADE_SPEED);
	}

	protected virtual void OnBlinkStop() {}
#endregion

#region Shadow
	public void SetShadowPointList(ShadowDetector shadowDetector, List<Vector2> shadowPointList)
	{
		this.shadowPointListDictionary[shadowDetector] = shadowPointList;
	}

	public virtual List<Vector2> GetShadowPointList(ShadowDetector shadowDetector)
	{
		if (!shadowPointListDictionary.ContainsKey(shadowDetector))
		{
			shadowPointListDictionary.Add(shadowDetector, new List<Vector2>());
		}

		return shadowPointListDictionary[shadowDetector];
	}

	public void ClearPointList(ShadowDetector shadowDetector)
	{
		GetShadowPointList(shadowDetector).Clear();
		this.shadowCenterPointDictionary[shadowDetector] = null;
	}

	public void SetShadowCenterPoint(ShadowDetector shadowDetector, Vector2? shadowCenterPoint)
	{
		this.shadowCenterPointDictionary[shadowDetector] = shadowCenterPoint;
	}
#endregion
}

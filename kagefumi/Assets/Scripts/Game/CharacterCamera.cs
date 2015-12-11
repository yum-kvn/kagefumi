using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterCamera : GameMonoBehaviour
{
	private Transform characterTransform;

	private Quaternion lastRotation;
	private Vector3 min;
	private Vector3 max;

	private Camera camera
	{
		get {return GetComponent<Camera>();}
	}

	private const float CAMERA_SPEED = 5.0f;
	private const float CAMERA_ANGLE_X = 30f;
	private const float OFFSET = 1.0f;

	public void Init(Transform target)
	{
		this.characterTransform = target;
		CalculateBounds();
	}

	public void SetCharacter(Transform characterTransform)
	{
		this.characterTransform = characterTransform;
	}

	private void LateUpdate()
	{
		if (characterTransform == null) {return;}

		Quaternion rotation = Quaternion.LookRotation(characterTransform.position - transform.position);
		rotation.eulerAngles = new Vector3(CAMERA_ANGLE_X, rotation.eulerAngles.y, 0f);
		rotation = Quaternion.Slerp(transform.rotation, rotation, CAMERA_SPEED * Time.deltaTime);

		Vector3 minPos = camera.WorldToViewportPoint(min);
		Vector3 maxPos = camera.WorldToViewportPoint(max);

		if ((minPos.x > 0f && rotation.y < lastRotation.y) || (maxPos.x < 1.0f && rotation.y > lastRotation.y))
		{
			return;
		}

		transform.rotation = rotation;
		lastRotation = rotation;
	}

	private void CalculateBounds()
	{
		GameObject floorGameObject = GameObject.FindWithTag("Floor");
		Bounds bounds = floorGameObject.GetComponent<Collider>().bounds;
		min = new Vector3(bounds.min.x - OFFSET, 0f, bounds.min.z);
		max = new Vector3(bounds.max.x + OFFSET, 0f, bounds.min.z);
	}
}

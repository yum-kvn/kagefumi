﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CustomTransform
{
#region Transform
	public static void ScaleTo(this Transform transform, float xy)
	{
		Vector3 localScale = transform.localScale;
		localScale.x = xy;
		localScale.y = xy;
		transform.localScale = localScale;
	}

	public static void RotateLocalEulerAnglesY(this Transform transform, float y)
	{
		Vector3 angles = transform.localEulerAngles;
		angles.y = y;
		transform.localEulerAngles = angles;
	}

	public static void RotateLocalEulerAnglesX(this Transform transform, float x)
	{
		Vector3 angles = transform.localEulerAngles;
		angles.x = x;
		transform.localEulerAngles = angles;
	}

	public static void MoveY(this Transform transform, float y)
	{
		Vector3 position = transform.position;
		position.y = y;
		transform.position = position;
	}
#endregion

#region RectTransform
	public static void MoveLocalY(this RectTransform transform, float y)
	{
		Vector3 position = transform.anchoredPosition;
		position.y = y;
		transform.anchoredPosition = position;
	}
#endregion
}

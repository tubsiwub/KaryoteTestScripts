﻿using UnityEngine;

namespace Allele
{
	public static class Bezier 
	{
		public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
		{
			//return Vector3.Lerp (Vector3.Lerp (p0, p1, t), Vector3.Lerp (p1, p2, t), t);

			t = Mathf.Clamp01 (t);
			float oneMinusT = 1f - t;

			Vector3 point = (oneMinusT * oneMinusT * p0) + (2f * oneMinusT * t * p1) + (t * t * p2);
			return point;
		}

		public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
		{
			return 2f * (1f - t) * (p1 - p0) + 2f * t * (p2 - p1);
		}
	}
}

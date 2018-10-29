using UnityEngine;
using UnityEditor;

namespace Allele
{
	[CustomEditor(typeof(BezierCurve))]
	public class BezierCurveInspector : Editor 
	{
		BezierCurve curve;
		Transform handleTransform;
		Quaternion handleRotation;

		const int lineSteps = 10;
		
		void OnSceneGUI()
		{
			curve = target as BezierCurve;

			handleTransform = curve.transform;
			//handleRotation = Tools.pivotRotation = PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
			handleRotation = handleTransform.rotation;

			Vector3 p0 = ShowPoint (0);
			Vector3 p1 = ShowPoint (1);
			Vector3 p2 = ShowPoint (2);

			Handles.color = Color.gray;
			Handles.DrawLine (p0, p1);
			Handles.DrawLine (p1, p2);

			Vector3 lineStart = curve.GetPoint (0f);
			Handles.color = Color.green;
			Handles.DrawLine (lineStart, lineStart + curve.GetDirection (0f));
			for (int i = 1; i <= lineSteps; i++)
			{
				Vector3 lineEnd = curve.GetPoint (i / (float)lineSteps);
				Handles.color = curve.color;
				Handles.DrawLine (lineStart, lineEnd);
				Handles.color = Color.green; 
				Handles.DrawLine (lineEnd, lineEnd + curve.GetDirection (i / (float)lineSteps));
				lineStart = lineEnd;
			}
		}

		Vector3 ShowPoint(int index)
		{
			if (curve.points.Length <= index)
			{
				Debug.LogError ("Point 'index' of " + index + " doesn't exist.");
				return Vector3.zero;
			}

			Vector3 point = handleTransform.TransformPoint (curve.points [index]);
			EditorGUI.BeginChangeCheck ();
			point = Handles.DoPositionHandle(point, handleRotation);
			if (EditorGUI.EndChangeCheck ())
			{
				Undo.RecordObject (curve, "Move Point");
				EditorUtility.SetDirty (curve);
				curve.points [index] = handleTransform.InverseTransformPoint (point);
			}
			return point;
		}
	}
}

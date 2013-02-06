using UnityEngine;
using UnityEditor;
using System.Collections;

public class SplineDrawer {
	
	[DrawGizmo (GizmoType.NotSelected | GizmoType.Selected | GizmoType.Active | GizmoType.SelectedOrChild)]
	public static void DrawSpline (SplineComponent a_Spline, GizmoType a_Type) {
		Color l_GizmosColor = Gizmos.color;
		
		if (a_Spline.Spline.IsValid && a_Spline.Spline.Count >= 4) {
			
				// Start and end points
			Gizmos.color = SplineWindow.StartEndPointColor;
			Gizmos.DrawLine (a_Spline.Spline [0].transform.position, a_Spline.Spline [1].transform.position);
			Gizmos.DrawLine (a_Spline.Spline [a_Spline.Spline.Count - 2].transform.position, a_Spline.Spline [a_Spline.Spline.Count - 1].transform.position);
			
				// Spline curve
			Gizmos.color = SplineWindow.DefaultColor;
			float l_TDelta = (a_Spline.Spline.LastControlPoint.T - a_Spline.Spline.FirstControlPoint.T) / SplineWindow.CurveSegmentCount;
			SplineEnumerator <SplineControlPointComponent> l_Enumerator = new SplineEnumerator <SplineControlPointComponent> (a_Spline.Spline);
			l_Enumerator.MoveToStart ();
			Vector3 l_PositionA;
			Vector3 l_PositionB = l_Enumerator.CurrentPosition;
			while (!l_Enumerator.IsAtEnd ()) {
				l_Enumerator.MoveForward (l_TDelta);
				
				l_PositionA = l_PositionB;
				l_PositionB = l_Enumerator.CurrentPosition;
				
				Gizmos.DrawLine (l_PositionA, l_PositionB);
			}
			
				// Curve spheres
			if (SplineWindow.CurveSphereCount > 1) {
				Gizmos.color = SplineWindow.CurveSphereColor;
				l_TDelta = (a_Spline.Spline.LastControlPoint.T - a_Spline.Spline.FirstControlPoint.T) / (SplineWindow.CurveSphereCount - 1);
				l_Enumerator.MoveToStart ();
				Gizmos.DrawSphere (l_Enumerator.CurrentPosition, SplineWindow.SphereRadius);
				while (!l_Enumerator.IsAtEnd ()) {
					l_Enumerator.MoveForward (l_TDelta);
					Gizmos.DrawSphere (l_Enumerator.CurrentPosition, SplineWindow.SphereRadius);
				}
			}
		}
		
		Gizmos.color = l_GizmosColor;
	}
	
	[DrawGizmo (GizmoType.NotSelected | GizmoType.Selected | GizmoType.Pickable)]
	public static void DrawSplineControlPoint (SplineControlPointComponent a_ControlPoint, GizmoType a_Type) {
		Color l_GizmosColor = Gizmos.color;
		
		SplineComponent l_Spline = SplineOfControlPoint (a_ControlPoint);
		if (l_Spline != null && l_Spline.Spline.Count > 0) {
			if (a_ControlPoint == l_Spline.Spline [0] || a_ControlPoint == l_Spline.Spline [l_Spline.Spline.Count - 1]) {
				Gizmos.color = SplineWindow.StartEndPointColor;
			} else {
				Gizmos.color = SplineWindow.DefaultColor;
			}

			Gizmos.DrawSphere (a_ControlPoint.transform.position, SplineWindow.SelectableSphereRadius);
		}
		
		Gizmos.color = l_GizmosColor;
	}
	
	protected static SplineComponent SplineOfControlPoint (SplineControlPointComponent l_ControlPoint) {
		SplineComponent l_Result = null;
		Transform l_Transform = l_ControlPoint.transform;
		
		while (l_Result == null) {
			if (l_Transform.parent == null) {
				break;
			}
			l_Transform = l_Transform.parent;
			l_Result = l_Transform.GetComponent <SplineComponent> ();
		}
		
		return (l_Result);
	}
}

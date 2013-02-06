using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplineToLineMeshSupport {
	
	public static List <Vector3> PointsOfSpline <T> (Transform a_Transform, Spline <T> a_Spline, float a_StartT, float a_EndT, int a_SegmentCount) where T : ISplineControlPoint {
		List <Vector3> l_Result = new List <Vector3> ();
		
		float l_DeltaT = (a_EndT - a_StartT) / a_SegmentCount;
		SplineEnumerator <T> l_Enumerator = new SplineEnumerator <T> (a_Spline);
		if (l_DeltaT > 0) {
			l_Enumerator.MoveTo (a_StartT);
			while (l_Enumerator.CurrentT < a_EndT) {
				l_Result.Add (a_Transform.InverseTransformPoint (l_Enumerator.CurrentPosition));
				l_Enumerator.MoveForward (l_DeltaT);
			}
			l_Enumerator.MoveTo (a_EndT);
			l_Result.Add (a_Transform.InverseTransformPoint (l_Enumerator.CurrentPosition));
		} else {
			l_DeltaT = - l_DeltaT;
			
			l_Enumerator.MoveTo (a_StartT);
			while (l_Enumerator.CurrentT > a_EndT) {
				l_Result.Add (a_Transform.InverseTransformPoint (l_Enumerator.CurrentPosition));
				l_Enumerator.MoveBackward (l_DeltaT);
			}
			l_Enumerator.MoveTo (a_EndT);
			l_Result.Add (a_Transform.InverseTransformPoint (l_Enumerator.CurrentPosition));
		}
		
		return (l_Result);
	}
}
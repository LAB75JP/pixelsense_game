using UnityEngine;
using System.Collections;

public class MathSupport {
	
	public static Vector3 MirrorVectorAtPlaneWithNormal (Vector3 a_Vector, Vector3 a_Normal) {
		Vector3 l_Result;
		float l_Dot = Vector3.Dot (a_Vector, a_Normal);
		
		if (Mathf.Approximately (l_Dot, 0.0f)) {
			l_Result = a_Vector;
		} else {
			float l_NormalSqr = Vector3.SqrMagnitude (a_Normal);
			float l_Factor = - 2.0f * l_Dot / l_NormalSqr;
			l_Result = a_Vector + (l_Factor * a_Normal);
		}
		return (l_Result);
	}
}
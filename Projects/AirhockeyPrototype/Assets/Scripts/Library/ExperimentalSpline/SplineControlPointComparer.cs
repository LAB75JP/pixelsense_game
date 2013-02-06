using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplineControlPointComparer <T> : IComparer <T> where T : ISplineControlPoint {
	
	public int Compare (T a, T b) {
		if (a.T < b.T) {
			return (- 1);
		} else if (a.T == b.T) {
			return (0);
		} else {
			return (1);
		}
	}
}
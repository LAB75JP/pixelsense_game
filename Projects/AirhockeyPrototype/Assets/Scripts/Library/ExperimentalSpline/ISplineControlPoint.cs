using UnityEngine;
using System.Collections;

public interface ISplineControlPoint {

	float T {
		get;
		set;
	}
	
	Vector3 Position {
		get;
		set;
	}
	
	float TDivisionFactor {
		get;
		set;
	}
}

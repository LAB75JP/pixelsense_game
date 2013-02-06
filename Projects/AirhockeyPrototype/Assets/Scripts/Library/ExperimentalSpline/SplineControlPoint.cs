using UnityEngine;
using System.Collections;

public class SplineControlPoint : ISplineControlPoint {

	protected float t;
	protected Vector3 m_Position = Vector3.zero;
	protected float m_TDivisionFactor;
	
	public float T {
		get {
			return (t);
		}
		set {
			t = value;
		}
	}
	
	public Vector3 Position {
		get {
			return (m_Position);
		}
		set {
			m_Position = value;
		}
	}
	
	public float TDivisionFactor {
		get {
			return (m_TDivisionFactor);
		}
		set {
			m_TDivisionFactor = value;
		}
	}
}

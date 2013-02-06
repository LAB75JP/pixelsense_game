using UnityEngine;
using System.Collections;

public class SplineComponent : MonoBehaviour {
	
	protected Spline <SplineControlPointComponent> m_Spline;
	
	public Spline <SplineControlPointComponent> Spline {
		get {
			if (m_Spline == null) {
				Initialize ();
			}
			
			return (m_Spline);
		}
	}
	
	public SplineEnumerator <SplineControlPointComponent> SplineEnumerator {
		get {
			return (new SplineEnumerator <SplineControlPointComponent> (m_Spline));
		}
	}
	
	protected void CreateSpline () {
		if (m_Spline == null) {
			m_Spline = new Spline <SplineControlPointComponent> ();
		}
	}
	
	public void Initialize () {
		CreateSpline ();
		
		m_Spline.ControlPoints.Clear ();
		SplineControlPointComponent[] l_Points = GetComponentsInChildren <SplineControlPointComponent> ();
		
			// Add all 'SplineControlPoint's
		foreach (SplineControlPointComponent l_ControlPoint in l_Points) {
			m_Spline.ControlPoints.Add (l_ControlPoint);
		}
		
		m_Spline.Initialize ();
	}
	
	protected virtual void Start () {
		CreateSpline ();
	}
}

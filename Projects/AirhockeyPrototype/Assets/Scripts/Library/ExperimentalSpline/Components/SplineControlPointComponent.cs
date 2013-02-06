using UnityEngine;
using System.Collections;

public class SplineControlPointComponent : MonoBehaviour, ISplineControlPoint {

	protected Transform m_CachedTransform;
	[SerializeField] protected float t;
	protected float m_TDivisionFactor;
	
	public Transform CachedTransform {
		get {
			if (m_CachedTransform == null) {
				m_CachedTransform = transform;
			}
			return (m_CachedTransform);
		}
	}
	
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
			return (CachedTransform.position);
		}
		set {
			CachedTransform.position = value;
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
	
	protected virtual void Start () {
		m_CachedTransform = transform;
	}
}

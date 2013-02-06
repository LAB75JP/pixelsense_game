using UnityEngine;
using System.Collections;

public class SplineEnumerator <T> where T : ISplineControlPoint {
	
	protected Spline <T> m_Spline;
	protected int m_CurrentIndex;
	protected float m_CurrentT;
	protected float m_CurrentInternalParameter;
	
	/// <summary>
	/// The 'Spline' on which this 'SplineEnumerator' operates.
	/// It is not allowed to assign 'null' to 'Spline'!
	/// It is expected that if a 'Spline' object is assigned that
	/// this 'Spline.IsValid'!
	/// </summary>
	public Spline <T> Spline {
		get {
			return (m_Spline);
		}
		set {
			if (value == null) {
				throw new System.Exception ("'Spline' is not allowed to become 'null'!");
			}
			if (!value.IsValid) {
				throw new System.Exception ("The 'Spline' instance which should be assigned does not suffice the condition 'IsValid'!");
			}
			m_Spline = value;
			MoveToStart ();
		}
	}
	
	/// <summary>
	/// Actual index of the 'Spline' with which this 'SplineEnumerator'
	/// works. 
	/// </summary>
	public int CurrentIndex {
		get {
			return (m_CurrentIndex);
		}
	}
	
	/// <summary>
	/// Actual 't' with which this 'SplineEnumerator' works. 
	/// </summary>
	public float CurrentT {
		get {
			return (m_CurrentT);
		}
	}
	
	/// <summary>
	/// Actual internal parameter with which this 'SplineEnumerator' works.
	/// </summary>
	public float CurrentInternalParameter {
		get {
			return (m_CurrentInternalParameter);
		}
	}
	
	/// <summary>
	/// Create a new 'SplineEnumerator' object which operates on 'a_Spline'.
	/// </summary>
	/// <param name="a_Spline">
	/// 'a_Spline' is not allowed to be 'null'!
	/// 'a_Spline.IsValid' has to be 'true'!
	/// A <see cref="Spline"/>
	/// </param>
	public SplineEnumerator (Spline <T> a_Spline) {
		Spline = a_Spline;
	}
	
	/// <summary>
	/// Move the 'SplineEnumerator' to the first valid 't' value and
	/// index 1 of the 'Spline'.
	/// It is assumed that 'Spline.IsValid' holds. 
	/// </summary>
	public void MoveToStart () {
		m_CurrentIndex = Spline.IndexOfFirstSplineControlPoint;
		m_CurrentT = Spline [m_CurrentIndex].T;
		m_CurrentInternalParameter = 0.0f;
	}
	
	/// <summary>
	/// Is the spline enumerator at the lowest 't' value?
	/// </summary>
	/// <returns>
	/// A <see cref="System.Boolean"/>
	/// </returns>
	public bool IsAtStart () {
		return (Mathf.Approximately (m_CurrentT, Spline.FirstControlPoint.T));
	}
	
	/// <summary>
	/// Move the 'SplineEnumerator' to the last valid 't' value and
	/// index 'Spline.Count - 2' of the 'Spline'.
	/// It is assumed that 'Spline.IsValid' holds. 
	/// </summary>
	public void MoveToEnd () {
		m_CurrentIndex = Spline.IndexOfLastSplineControlPoint - 1;
		m_CurrentT = Spline [Spline.IndexOfLastSplineControlPoint].T;
		m_CurrentInternalParameter = 1.0f;
	}
	
	/// <summary>
	/// Is the spline enumerator at the greatest 't' value?
	/// </summary>
	/// <returns>
	/// A <see cref="System.Boolean"/>
	/// </returns>
	public bool IsAtEnd () {
		return (Mathf.Approximately (m_CurrentT, Spline.LastControlPoint.T));
	}
	
	public Vector3 CurrentPosition {
		get {
			return (Spline.GetPositionAtIndexWithInternalParameter (CurrentIndex, CurrentInternalParameter));
		}
	}
	
	public Vector3 CurrentTangent {
		get {
			return (Spline.GetTangentAtIndexWithInternalParameter (CurrentIndex, CurrentInternalParameter));
		}
	}
	
	/// <summary>
	/// Move the 'SplineEnumerator' forward by 'a_DeltaT'.
	/// 'a_DeltaT' is not allowed to be negative. 
	/// </summary>
	/// <param name="a_DeltaT">
	/// A <see cref="System.Single"/>
	/// </param>
	public void MoveForward (float a_DeltaT) {
		m_CurrentT = m_CurrentT + a_DeltaT;
		if (m_CurrentT > Spline.LastControlPoint.T) {
			m_CurrentIndex = Spline.IndexOfLastSplineControlPoint - 1;
			m_CurrentT = Spline.LastControlPoint.T;
			m_CurrentInternalParameter = 1.0f;
		} else {
			while (Spline [m_CurrentIndex + 1].T < m_CurrentT) {
				m_CurrentIndex = m_CurrentIndex + 1;
			}
			m_CurrentInternalParameter = Spline.GetInternalParameter (m_CurrentIndex, m_CurrentT);
		}
	}
	
	/// <summary>
	/// Move the 'SplineEnumerator' backward by 'a_DeltaT'.
	/// 'a_DeltaT' is not allowed to be negative.
	/// </summary>
	/// <param name="a_DeltaT">
	/// A <see cref="System.Single"/>
	/// </param>
	public void MoveBackward (float a_DeltaT) {
		m_CurrentT = m_CurrentT - a_DeltaT;
		if (m_CurrentT < Spline.FirstControlPoint.T) {
			m_CurrentIndex = Spline.IndexOfFirstSplineControlPoint;
			m_CurrentT = Spline.FirstControlPoint.T;
			m_CurrentInternalParameter = 0.0f;
		} else {
			while (Spline [m_CurrentIndex].T > m_CurrentT) {
				m_CurrentIndex = m_CurrentIndex - 1;
			}
			m_CurrentInternalParameter = Spline.GetInternalParameter (m_CurrentIndex, m_CurrentT);
		}
	}
	
	public void MoveTo (float a_T) {
		m_CurrentT = a_T;
		m_CurrentIndex = Spline.GetIndexAtT (CurrentT);
		m_CurrentInternalParameter = Spline.GetInternalParameter (CurrentIndex, CurrentT);
	}
}
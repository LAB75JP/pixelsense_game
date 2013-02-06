using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SplineEntryExitMode {
	Linear,
	Curve,
	SmoothedCurve
}

public class Spline <T> where T : ISplineControlPoint {
	
	protected List <T> m_ControlPoints = new List <T> ();
	protected static SplineControlPointComparer <T> m_Comparer = new SplineControlPointComparer <T> ();
	
	protected bool m_IsValid = false;
	protected float m_TCorrectionValue = 0.01f;
	
	/// <summary>
	/// Is this 'Spline' valid and can it be used? 
	/// </summary>
	public bool IsValid {
		get {
			return (m_IsValid);
		}
	}
	
	/// <summary>
	/// Number of available 'SplineControlPoint's. There are
	/// two more control points added automatically. One at
	/// the beginning and one at the end.
	/// </summary>
	public int Count {
		get {
			return (m_ControlPoints.Count);
		}
	}
	
	/// <summary>
	/// The index of the first control point which is used. The truly
	/// first control point is used to control the starting shape of
	/// the curve only. 
	/// </summary>
	public int IndexOfFirstSplineControlPoint {
		get {
			return (1);
		}
	}
	
	/// <summary>
	/// The index of the last control point which is used. The truly
	/// last control point is used to control the ending shape of
	/// the curve only. 
	/// </summary>
	public int IndexOfLastSplineControlPoint {
		get {
			return (Count - 2);
		}
	}
	
	/// <summary>
	/// Short for getting the control point at 'IndexOfFirstSplineControlPoint'. 
	/// </summary>
	public T FirstControlPoint {
		get {
			return (this [IndexOfFirstSplineControlPoint]);
		}
	}
	
	/// <summary>
	/// Short for getting the control point at 'IndexOfLastSplineControlPoint'. 
	/// </summary>
	public T LastControlPoint {
		get {
			return (this [IndexOfLastSplineControlPoint]);
		}
	}
	
	/// <summary>
	/// Get a 'SplineControlPoint'. There are
	/// two more control points added automatically. One at
	/// the beginning and one at the end.
	/// </summary>
	/// <param name="i">
	/// A <see cref="System.Int32"/>
	/// </param>
	public T this [int i] {
		get {
			return (m_ControlPoints [i]);
		}
	}
	
	public List <T> ControlPoints {
		get {
			return (m_ControlPoints);
		}
	}
	
	public bool Contains (T a_SplineControlPoint) {
		return (m_ControlPoints.Contains (a_SplineControlPoint));
	}
	
	public int IndexOf (T a_SplineControlPoint) {
		return (m_ControlPoints.IndexOf (a_SplineControlPoint));
	}
	
	/// <summary>
	/// Always call 'Initialize ()' if new 'SplineControlPoint' child objects are added
	/// or if 't' is changed in an existing 'SplineControlPoint'.
	/// </summary>
	public virtual void Initialize () {
		m_IsValid = true;
		
		m_ControlPoints.Sort (m_Comparer);
		
		if (Count < 4) {
			m_IsValid = false;
		} else {

				// Check the validity of the time deltas between all the 'SplineControlPoint's
				// and initialize the division factor as well.
			for (int i = 0; i < Count - 1; i = i + 1) {
				float l_TDelta = this [i + 1].T - this [i].T;
				if (Mathf.Approximately (l_TDelta, 0.0f)) {
					// Debug.LogError ("There are two 'SplineControlPoint's with the same 't'!");
					m_IsValid = false;
				}
				this [i].TDivisionFactor = 1.0f / l_TDelta;
			}
		}
	}
	
	/// <summary>
	/// Return the index position in this 'Spline' for 't'. 
	/// </summary>
	/// <param name="t">
	/// A <see cref="System.Single"/>
	/// </param>
	/// <returns>
	/// A <see cref="System.Int32"/>
	/// </returns>
	public int GetIndexAtT (float t) {
		int l_Result;
	
		if (t <= this [1].T) {
			l_Result = 1;
		} else if (t >= this [Count - 2].T) {
			l_Result = Count - 3;
		} else {
			for (l_Result = 1; l_Result < Count - 1; l_Result = l_Result + 1) {
				if (this [l_Result].T > t)
					break;
			}
			l_Result = l_Result - 1;
		}

		return (l_Result);
	}
	
	/// <summary>
	/// Get the internal parameter which is in [0.0f, 1.0f], for the
	/// 'Spline' index position 'i' for the spline parameter 't'. 
	/// </summary>
	/// <param name="i">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="t">
	/// A <see cref="System.Single"/>
	/// </param>
	/// <returns>
	/// A <see cref="System.Single"/>
	/// </returns>
	public float GetInternalParameter (int i, float t) {
		float l_Result;
		
		if (t < this [i].T) {
			t = this [i].T;
		} else if (t > this [i + 1].T) {
			t = this [i + 1].T;
		}
		
		float l_timeFactor = this [i].TDivisionFactor;
		l_Result = (t - this [i].T) * l_timeFactor;
		
		return (l_Result);
	}
	
	/// <summary>
	/// Get the position on the spline curve for the spline index position
	/// 'i' with 'a_InternalParameter'. 
	/// </summary>
	/// <param name="i">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="a_InternalParameter">
	/// A <see cref="System.Single"/>
	/// </param>
	/// <returns>
	/// A <see cref="Vector3"/>
	/// </returns>
	public Vector3 GetPositionAtIndexWithInternalParameter (int i, float a_InternalParameter) {
		float t = a_InternalParameter;
		float t2 = t * t;
		float t3 = t2 * t;

		Vector3 P0 = this [i - 1].Position;
		Vector3 P1 = this [i].Position;
		Vector3 P2 = this [i + 1].Position;
		Vector3 P3 = this [i + 2].Position;

		float l_Factor = 0.5f;

		Vector3 m1 = l_Factor * (P2 - P0);
		Vector3 m2 = l_Factor * (P3 - P1);

		float l_Factor1 = 2 * t3 - 3 * t2 + 1;
		float l_Factor2 = -2 * t3 + 3 * t2;
		float l_Factor3 = t3 - 2 * t2 + t;
		float l_Factor4 = t3 - t2;

		return (l_Factor1 * P1 + l_Factor2 * P2 + l_Factor3 * m1 + l_Factor4 * m2);
	}
	
	public Vector3 GetPositionAt (float t) {
		int l_Index = GetIndexAtT (t);
		float l_InternalParameter = GetInternalParameter (l_Index, t);
		return (GetPositionAtIndexWithInternalParameter (l_Index, l_InternalParameter));
	}
	
	public Vector3 GetTangentAtIndexWithInternalParameter (int i, float a_InternalParameter) {
		Vector3 l_Result;
		
		float l_Epsilon = 0.001f;
		if (i == 1 && a_InternalParameter - l_Epsilon < 0.0f) {
			Vector3 l_Position1 = this [i].Position;
			Vector3 l_Position2 = GetPositionAtIndexWithInternalParameter (i, l_Epsilon);
			l_Result = l_Position2 - l_Position1;			
		} else if (i == Count - 3 && a_InternalParameter + l_Epsilon > 1.0f) {
			Vector3 l_Position1 = GetPositionAtIndexWithInternalParameter (i, 1.0f - l_Epsilon);
			Vector3 l_Position2 = this [i + 1].Position;
			l_Result = l_Position2 - l_Position1;
		} else {
			Vector3 l_Position1;
			Vector3 l_Position2;
			if (a_InternalParameter - l_Epsilon < 0.0f) {
				l_Position1 = GetPositionAtIndexWithInternalParameter (i - 1, a_InternalParameter - l_Epsilon + 1.0f);
				l_Position2 = GetPositionAtIndexWithInternalParameter (i, a_InternalParameter + l_Epsilon);
			} else if (a_InternalParameter + l_Epsilon > 1.0f) {
				l_Position1 = GetPositionAtIndexWithInternalParameter (i, a_InternalParameter - l_Epsilon);
				l_Position2 = GetPositionAtIndexWithInternalParameter (i + 1, a_InternalParameter + l_Epsilon - 1.0f);
			} else {
				l_Position1 = GetPositionAtIndexWithInternalParameter (i, a_InternalParameter - l_Epsilon);
				l_Position2 = GetPositionAtIndexWithInternalParameter (i, a_InternalParameter + l_Epsilon);
			}
			l_Result = l_Position2 - l_Position1;
		}
		
		return (l_Result);
	}
	
	public Vector3 GetTangentAt (float t) {
		int l_Index = GetIndexAtT (t);
		float l_InternalParameter = GetInternalParameter (l_Index, t);
		
		return (GetTangentAtIndexWithInternalParameter (l_Index, l_InternalParameter));
	}
	
	public void CalculateTsForDistance (int a_SubStepsPerSegment) {
		if (a_SubStepsPerSegment < 1) {
			Debug.LogError ("'a_SubStepsPerSegment' has to be at least 1!");
		} else {
			Initialize ();
			if (!m_IsValid) {
				Debug.LogError ("The spline's parameters can not be recalculated as the curve is not valid!");
			} else {
				float l_SegmentLength = Vector3.Distance (this [0].Position, this [1].Position);
				if (l_SegmentLength < m_TCorrectionValue) {
					l_SegmentLength = m_TCorrectionValue;
				}
				this [0].T = - l_SegmentLength;
				this [1].T = 0.0f;
				
				for (int i = 1; i < Count - 2; i = i + 1) {
					l_SegmentLength = SegmentLengthAfterControlPointWithIndex (i, a_SubStepsPerSegment);
					if (l_SegmentLength < m_TCorrectionValue) {
						l_SegmentLength = m_TCorrectionValue;
					}
					
					this [i + 1].T = this [i].T + l_SegmentLength;
				}
				
				l_SegmentLength = Vector3.Distance (this [Count - 2].Position, this [Count - 1].Position);
				if (l_SegmentLength < m_TCorrectionValue) {
					l_SegmentLength = m_TCorrectionValue;
				}
				this [Count - 1].T = this [Count - 2].T + l_SegmentLength;
				
				Initialize ();
			}
		}
	}
	
	public void UniformParameterDistribution () {
		Initialize ();
		if (!m_IsValid) {
			Debug.LogError ("The spline's parameters can not be recalculated as the curve is not valid!");
		} else {
			float l_DeltaT = this [Count - 2].T - this [1].T;
			if (l_DeltaT > 0.0f) {
				l_DeltaT = l_DeltaT / (Count - 3);
				
				this [0].T = - l_DeltaT;
				this [1].T = 0.0f;
				
				for (int i = 1; i < Count - 1; i = i + 1) {
					this [i + 1].T = this [i].T + l_DeltaT;
				}
				
				Initialize ();
			} else {
				Debug.LogError ("The spline's parameters can only be evenly distributed, if the t's from the first to the last control point (excluding the entry and exit control points) are positive!");
			}
		}
	}
	
	protected float SegmentLengthAfterControlPointWithIndex (int a_Index, int a_SubStepsPerSegment) {
		float l_Result = 0.0f;
		
		float l_InverseFactor = 1.0f / a_SubStepsPerSegment;
		Vector3 l_PreviousPosition = GetPositionAtIndexWithInternalParameter (a_Index, 0.0f);
		Vector3 l_CurrentPosition;
		for (int i = 1; i <= a_SubStepsPerSegment; i = i + 1) {
			l_CurrentPosition = GetPositionAtIndexWithInternalParameter (a_Index, i * l_InverseFactor);
			l_Result = l_Result + Vector3.Distance (l_PreviousPosition, l_CurrentPosition);
			
			l_PreviousPosition = l_CurrentPosition;
		}
		
		return (l_Result);
	}
	
	public float SplineLength (int a_StepsPerSegmentCount) {
		float l_Result = 0.0f;
		
		for (int i = 1; i < Count - 2; i = i + 1) {
			l_Result = l_Result + SegmentLengthAfterControlPointWithIndex (i, a_StepsPerSegmentCount);
		}
		
		return (l_Result);
	}
	
	public void SetSplineEntryControlPoint (SplineEntryExitMode a_Mode) {
		if (!IsValid) {
			Debug.LogError ("Can not calculate entry control point for an invalid spline!");
		} else {
			SplineEntryExitMode l_Mode = a_Mode;
			if (Count == 4) {
				l_Mode = SplineEntryExitMode.Linear;
			}
			if (Count >= 4) {
				Vector3 l_ControlPointPosition;
				
				if (l_Mode == SplineEntryExitMode.Linear) {
					l_ControlPointPosition = LinearEntryControlPoint ();
				} else if (l_Mode == SplineEntryExitMode.Curve) {
					l_ControlPointPosition = CurvedEntryControlPoint ();
				} else {
					l_ControlPointPosition = SmoothedCurvedEntryControlPoint ();
				}
				this [0].Position = l_ControlPointPosition;
			}
		}
	}
	
	public void SetSplineExitControlPoint (SplineEntryExitMode a_Mode) {
		if (!IsValid) {
			Debug.LogError ("Can not calculate exit control point for an invalid spline!");
		} else {
			SplineEntryExitMode l_Mode = a_Mode;
			if (Count == 4) {
				l_Mode = SplineEntryExitMode.Linear;
			}
			if (Count >= 4) {
				Vector3 l_ControlPointPosition;
				
				if (l_Mode == SplineEntryExitMode.Linear) {
					l_ControlPointPosition = LinearExitControlPoint ();
				} else if (l_Mode == SplineEntryExitMode.Curve) {
					l_ControlPointPosition = CurvedExitControlPoint ();
				} else {
					l_ControlPointPosition = SmoothedCurvedExitControlPoint ();
				}
				this [Count - 1].Position = l_ControlPointPosition;
			}
		}
	}
	
	public void SetSplineEntryExitControlPoints (SplineEntryExitMode a_Mode) {
		if (!IsValid) {
			Debug.LogError ("Can not calculate entry and exit control points for an invalid spline!");
		} else {
			SetSplineEntryControlPoint (a_Mode);
			SetSplineExitControlPoint (a_Mode);
		}
	}
	
	public Vector3 LinearEntryControlPoint () {
		Vector3 l_Result;
		Vector3 l_Position1 = this [1].Position;
		Vector3 l_Position2 = this [2].Position;
		l_Result = l_Position1 + (l_Position1 - l_Position2);
		return (l_Result);
	}
	
	public Vector3 LinearExitControlPoint () {
		Vector3 l_Result;
		Vector3 l_Position1 = this [Count - 2].Position;
		Vector3 l_Position2 = this [Count - 3].Position;
		l_Result = l_Position1 + (l_Position1 - l_Position2);
		return (l_Result);
	}
	
	public Vector3 CurvedEntryControlPoint () {
		Vector3 l_Result;
		Vector3 l_Position1 = this [1].Position;
		Vector3 l_Position2 = this [2].Position;
		Vector3 l_Position3 = this [3].Position;
		Vector3 l_Normal = l_Position2 - l_Position1;
		Vector3 l_Offset = l_Position3 - l_Position2;
		l_Offset = MathSupport.MirrorVectorAtPlaneWithNormal (l_Offset, l_Normal);
		l_Result = l_Position1 + l_Offset;
		return (l_Result);
	}
	
	public Vector3 CurvedExitControlPoint () {
		Vector3 l_Result;
		Vector3 l_Position1 = this [Count - 2].Position;
		Vector3 l_Position2 = this [Count - 3].Position;
		Vector3 l_Position3 = this [Count - 4].Position;
		Vector3 l_Normal = l_Position2 - l_Position1;
		Vector3 l_Offset = l_Position3 - l_Position2;
		l_Offset = MathSupport.MirrorVectorAtPlaneWithNormal (l_Offset, l_Normal);
		l_Result = l_Position1 + l_Offset;
		return (l_Result);
	}
	
	public Vector3 SmoothedCurvedEntryControlPoint () {
		Vector3 l_Result = 0.5f * (LinearEntryControlPoint () + CurvedEntryControlPoint ());
		return (l_Result);
	}
	
	public Vector3 SmoothedCurvedExitControlPoint () {
		Vector3 l_Result = 0.5f * (LinearExitControlPoint () + CurvedExitControlPoint ());
		return (l_Result);
	}
}
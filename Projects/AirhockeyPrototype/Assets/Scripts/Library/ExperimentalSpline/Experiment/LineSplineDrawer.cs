using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (LineMeshGenerator))]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
public class LineSplineDrawer : MonoBehaviour {
	
	public SplineComponent spline;
	public int pointCount = 10;
	
	protected LineMeshGenerator m_LineMeshGenerator;
	protected MeshFilter m_MeshFilter;
	
	protected List <Vector3> m_Points = new List <Vector3> ();
	
	protected void Start () {
		if (spline == null) {
			spline = GetComponent <SplineComponent> ();
		}
		
		if (spline == null) {
			Debug.LogError ("'spline' in 'SplineDrawer' is null!");
		}
		
		m_LineMeshGenerator = GetComponent <LineMeshGenerator> ();
		m_MeshFilter = GetComponent <MeshFilter> ();
	}
	
	protected void Update () {
		Transform l_Transform = transform;
		
		if (pointCount > 0 && spline.Spline.Count > 0) {
			SplineEnumerator <SplineControlPointComponent> l_Enumerator = new SplineEnumerator <SplineControlPointComponent> (spline.Spline);
			
			float l_SplineLength = spline.Spline.LastControlPoint.T - spline.Spline.FirstControlPoint.T;
			float l_Delta = l_SplineLength / pointCount;
			
			m_Points.Clear ();
			
			l_Enumerator.MoveToStart ();
			while (!l_Enumerator.IsAtEnd ()) {
				m_Points.Add (l_Transform.InverseTransformPoint (l_Enumerator.CurrentPosition));
				l_Enumerator.MoveForward (l_Delta);
			}
			m_Points.Add (l_Transform.InverseTransformPoint (l_Enumerator.CurrentPosition));
			
			m_LineMeshGenerator.RecalculateMesh (m_Points, 1.0f, l_Transform.InverseTransformDirection (Vector3.up));
		}
		
		m_MeshFilter.mesh = m_LineMeshGenerator.GeneratedMesh;
	}
}

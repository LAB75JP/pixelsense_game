using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineMeshGenerator : MonoBehaviour {
	
	protected List <Vector3> m_Positions;
	
	protected Vector3[] m_Vertices;
	protected Vector2[] m_UVs;
	protected int[] m_Triangles;
	
	protected Mesh m_Mesh;
	
	public Mesh GeneratedMesh {
		get {
			return (m_Mesh);
		}
	}
	
	public void RecalculateMesh (List <Vector3> a_Positions, float a_LineThickness, Vector3 a_Normal) {
		m_Positions = a_Positions;
		
		int l_Count = a_Positions.Count * 2;
		if (m_Vertices == null || m_Vertices.Length != l_Count) {
			m_Vertices = new Vector3 [l_Count];
			m_UVs = new Vector2 [l_Count];
			m_Triangles = new int [(a_Positions.Count - 1) * 6];
		}
		
		Vector2 l_UV1 = new Vector2 (0.5f, 0.0f);
		Vector2 l_UV2 = new Vector2 (0.5f, 1.0f);
		
			// Vertices and UV's
		for (int i = 0; i < a_Positions.Count; i = i + 1) {
			Vector3 l_Tangent = TangetAtIndex (a_Positions, i);
			Vector3 l_Perpendicular = Vector3.Cross (a_Normal, l_Tangent);
			l_Perpendicular.Normalize ();
			l_Perpendicular = l_Perpendicular * (0.5f * a_LineThickness);
			
			Vector3 l_Position1 = a_Positions [i] + l_Perpendicular;
			Vector3 l_Position2 = a_Positions [i] - l_Perpendicular;
			
			int l_Index = i * 2;
			m_Vertices [l_Index + 0] = l_Position1;
			m_Vertices [l_Index + 1] = l_Position2;
			
			m_UVs [l_Index + 0] = l_UV1;
			m_UVs [l_Index + 1] = l_UV2;
		}
		
		for (int i = 0; i < a_Positions.Count - 1; i = i + 1) {
			int l_Index = i * 6;
			int l_VertexIndex = i * 2;
			m_Triangles [l_Index + 0] = l_VertexIndex + 2;
			m_Triangles [l_Index + 1] = l_VertexIndex + 0;
			m_Triangles [l_Index + 2] = l_VertexIndex + 1;
			m_Triangles [l_Index + 3] = l_VertexIndex + 2;
			m_Triangles [l_Index + 4] = l_VertexIndex + 1;
			m_Triangles [l_Index + 5] = l_VertexIndex + 3;
		}
		
		m_Mesh = new Mesh ();
		m_Mesh.vertices = m_Vertices;
		m_Mesh.triangles = m_Triangles;
		m_Mesh.uv = m_UVs;
		
		m_Mesh.RecalculateNormals ();
		
			// Do that as soon as it is part of a 'MeshFilter'.
		/*m_Mesh.RecalculateBounds ();*/
	}
	
	protected Vector3 TangetAtIndex (List <Vector3> a_Positions, int a_Index) {
		Vector3 l_Result;
		
		if (a_Index == 0) {
			l_Result = a_Positions [a_Index + 1] - a_Positions [a_Index];
		} else if (a_Index == a_Positions.Count - 1) {
			l_Result = a_Positions [a_Index] - a_Positions [a_Index - 1];
		} else {
			l_Result = a_Positions [a_Index + 1] - a_Positions [a_Index - 1];
		}
		
		return (l_Result);
	}
}

using UnityEngine;
using System.Collections;

// TODO: (High priority)
// - If we have tight curves, the normals point in the wrong direction as the triangle index order is wrong for that case.

// TODO: (Low priority)
// - There is still a UV problem, if the texture is not mirrored!

public class SplineMeshGenerator {

	public static Mesh SplineMesh <T> (Spline <T> a_Spline, int a_SegmentCount, float a_LineThickness, Vector3 a_Normal) where T : ISplineControlPoint {
		Mesh l_Result = new Mesh ();
		
		int l_Count = (a_SegmentCount + 1) * 2;
		Vector3[] l_Vertices = new Vector3 [l_Count];
		Vector3[] l_Normals = new Vector3 [l_Count];
		Vector2[] l_UVs = new Vector2 [l_Count];
		int[] l_Triangles = new int [(a_SegmentCount * 2) * 3];
		
		float l_DeltaT = (a_Spline.LastControlPoint.T - a_Spline.FirstControlPoint.T) / a_SegmentCount;
		float l_HalfLineThickness = 0.5f * a_LineThickness;
		
		SplineEnumerator <T> l_Enumerator = new SplineEnumerator <T> (a_Spline);
		l_Enumerator.MoveToStart ();
		
		Vector3 l_Position = l_Enumerator.CurrentPosition;
		Vector3 l_Tangent = l_Enumerator.CurrentTangent;
		Vector3 l_Side = Vector3.Cross (a_Normal, l_Tangent);
		l_Side.Normalize ();
		l_Side = l_Side * l_HalfLineThickness;
		Vector3 l_Position1 = l_Position + l_Side;
		Vector3 l_Position2 = l_Position - l_Side;
		
		l_Vertices [0] = l_Position1;
		l_Vertices [1] = l_Position2;
		l_Normals [0] = a_Normal;
		l_Normals [1] = a_Normal;
		l_UVs [0] = new Vector2 (0.5f, 1.0f);
		l_UVs [1] = new Vector2 (0.5f, 0.0f);
		l_Enumerator.MoveForward (l_DeltaT);
		
		Vector3 l_PreviousPosition1 = l_Position1;
		Vector3 l_PreviousPosition2 = l_Position2;
		Vector3 l_PreviousTangent = l_Tangent;
		
		bool standardUV = true;
		
		for (int i = 0; i < a_SegmentCount; i = i + 1) {
			
				// Vertices
			int l_VertexIndex = (i + 1) * 2;
			
			l_Position = l_Enumerator.CurrentPosition;
			l_Tangent = l_Enumerator.CurrentTangent;
			l_Side = Vector3.Cross (a_Normal, l_Tangent);
			l_Side.Normalize ();
			l_Side = l_Side * l_HalfLineThickness;
			l_Position1 = l_Position + l_Side;
			l_Position2 = l_Position - l_Side;
			
			l_Vertices [l_VertexIndex + 0] = l_Position1;
			l_Vertices [l_VertexIndex + 1] = l_Position2;
			
			l_Normals [l_VertexIndex + 0] = a_Normal;
			l_Normals [l_VertexIndex + 1] = a_Normal;
						
			l_Enumerator.MoveForward (l_DeltaT);
			
				// Triangles
			l_VertexIndex = i * 2;
			int l_TriangleIndex = i * 6;
			
			float l_DotTangents = Vector3.Dot (l_Tangent, l_PreviousTangent);
			float l_Dot1 = Vector3.Dot (l_PreviousTangent, l_Position1 - l_PreviousPosition1);
			float l_Dot2 = Vector3.Dot (l_PreviousTangent, l_Position2 - l_PreviousPosition1);
			float l_Dot3 = Vector3.Dot (l_Tangent, l_PreviousPosition1 - l_Position1);
			float l_Dot4 = Vector3.Dot (l_Tangent, l_PreviousPosition2 - l_Position1);
			
			if (l_Dot1 < 0.0f && l_Dot2 < 0.0f) {
				if (l_DotTangents < 0.0f) {
					standardUV = !standardUV;
					
					l_Triangles [l_TriangleIndex + 0] = l_VertexIndex + 3;
					l_Triangles [l_TriangleIndex + 1] = l_VertexIndex + 1;
					l_Triangles [l_TriangleIndex + 2] = l_VertexIndex + 0;
					l_Triangles [l_TriangleIndex + 3] = l_VertexIndex + 2;
					l_Triangles [l_TriangleIndex + 4] = l_VertexIndex + 1;
					l_Triangles [l_TriangleIndex + 5] = l_VertexIndex + 3;
				} else {
					l_Triangles [l_TriangleIndex + 0] = l_VertexIndex + 2;
					l_Triangles [l_TriangleIndex + 1] = l_VertexIndex + 1;
					l_Triangles [l_TriangleIndex + 2] = l_VertexIndex + 0;
					l_Triangles [l_TriangleIndex + 3] = l_VertexIndex + 1;
					l_Triangles [l_TriangleIndex + 4] = l_VertexIndex + 2;
					l_Triangles [l_TriangleIndex + 5] = l_VertexIndex + 3;
				}
			} else if (l_Dot3 < 0.0f && l_Dot4 < 0.0f) {
				if (l_DotTangents < 0.0f) {
					l_Triangles [l_TriangleIndex + 0] = l_VertexIndex + 1;
					l_Triangles [l_TriangleIndex + 1] = l_VertexIndex + 2;
					l_Triangles [l_TriangleIndex + 2] = l_VertexIndex + 0;
					l_Triangles [l_TriangleIndex + 3] = l_VertexIndex + 2;
					l_Triangles [l_TriangleIndex + 4] = l_VertexIndex + 1;
					l_Triangles [l_TriangleIndex + 5] = l_VertexIndex + 3;
				} else {
					l_Triangles [l_TriangleIndex + 0] = l_VertexIndex + 1;
					l_Triangles [l_TriangleIndex + 1] = l_VertexIndex + 2;
					l_Triangles [l_TriangleIndex + 2] = l_VertexIndex + 0;
					l_Triangles [l_TriangleIndex + 3] = l_VertexIndex + 2;
					l_Triangles [l_TriangleIndex + 4] = l_VertexIndex + 1;
					l_Triangles [l_TriangleIndex + 5] = l_VertexIndex + 3;
				}
			} else {
				standardUV = !standardUV;
				
				l_Triangles [l_TriangleIndex + 0] = l_VertexIndex + 1;
				l_Triangles [l_TriangleIndex + 1] = l_VertexIndex + 2;
				l_Triangles [l_TriangleIndex + 2] = l_VertexIndex + 0;
				l_Triangles [l_TriangleIndex + 3] = l_VertexIndex + 0;
				l_Triangles [l_TriangleIndex + 4] = l_VertexIndex + 2;
				l_Triangles [l_TriangleIndex + 5] = l_VertexIndex + 3;
			}
			
			if (standardUV) {
				l_UVs [l_VertexIndex + 2] = new Vector2 (0.5f, 1.0f);
				l_UVs [l_VertexIndex + 3] = new Vector2 (0.5f, 0.0f);
			} else {
				l_UVs [l_VertexIndex + 2] = new Vector2 (0.5f, 0.0f);
				l_UVs [l_VertexIndex + 3] = new Vector2 (0.5f, 1.0f);
			}
			
			l_PreviousPosition1 = l_Position1;
			l_PreviousPosition2 = l_Position2;
			l_PreviousTangent = l_Tangent;
		}
		
		l_Result.vertices = l_Vertices;
		l_Result.triangles = l_Triangles;
		l_Result.normals = l_Normals;
		l_Result.uv = l_UVs;
		
		return (l_Result);
	}
	
	public static float GetTAtRaycastHit <T>  (Spline <T> a_Spline, int a_SegmentCount, RaycastHit a_Hit) where T : ISplineControlPoint {
		int l_Segment = (a_Hit.triangleIndex / 2);
		float l_DeltaT = (a_Spline.LastControlPoint.T - a_Spline.FirstControlPoint.T) / a_SegmentCount;
		
			// HACK!!!
		return ((l_Segment + 0.5f) * l_DeltaT);
	}
}

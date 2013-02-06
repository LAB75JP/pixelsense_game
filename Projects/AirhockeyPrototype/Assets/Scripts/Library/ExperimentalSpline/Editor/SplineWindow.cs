using UnityEngine;
using UnityEditor;
using System.Collections;

public class SplineWindow : EditorWindow {

	protected static SplineComponent s_Spline;
	protected static SplineControlPointComponent s_SelectedControlPoint;
	
	protected static SplineEntryExitMode s_SplineEntryExitMode;
	
	protected static int s_CurveSegmentCount = 100;
	protected static int s_CurveSphereCount = 10;
	protected static float s_SelectableSphereRadius = 0.1f;
	protected static float s_SphereRadius = 0.05f;
	protected static Color s_StartEndPointColor = new Color (1.0f, 1.0f, 0.0f, 0.7f);
	protected static Color s_DefaultColor = new Color (0.0f, 0.0f, 1.0f, 0.7f);
	protected static Color s_CurveSphereColor = Color.black;
	protected static bool s_GizmoOptionsFoldout = true;
	
	protected const int c_SubStepsPerSegment = 3;
	
	public static SplineControlPointComponent SelectedControlPoint {
		get {
			return (s_SelectedControlPoint);
		}
	}
	
	public static int CurveSegmentCount {
		get {
			return (s_CurveSegmentCount);
		}
	}
	
	public static int CurveSphereCount {
		get {
			return (s_CurveSphereCount);
		}
	}
	
	public static float SelectableSphereRadius {
		get {
			return (s_SelectableSphereRadius);
		}
	}
	
	public static float SphereRadius {
		get {
			return (s_SphereRadius);
		}
	}
	
	public static Color StartEndPointColor {
		get {
			return (s_StartEndPointColor);
		}
	}
	
	public static Color DefaultColor {
		get {
			return (s_DefaultColor);
		}
	}
	
	public static Color CurveSphereColor {
		get {
			return (s_CurveSphereColor);
		}
	}
	
	protected static void FindSpline () {
		SplineComponent l_Spline = null;
		Transform l_Transform = Selection.activeTransform;
		
		if (l_Transform != null) {
			l_Spline = l_Transform.GetComponent <SplineComponent> ();
			if (l_Spline == null) {
				while (l_Spline == null) {
					if (l_Transform.parent == null) {
						break;
					}
					l_Transform = l_Transform.parent;
					l_Spline = l_Transform.GetComponent <SplineComponent> ();
				}
			}
		}
		s_Spline = l_Spline;
	}
	
	protected static void ValidateSelection () {
		if (s_SelectedControlPoint != null && s_Spline != null) {
			if (!s_Spline.Spline.Contains (s_SelectedControlPoint)) {
				s_SelectedControlPoint = null;
			}
		}
	}
	
	[MenuItem ("Window/Spline")]
	public static void CreateWindow () {
		SplineWindow l_Window = (SplineWindow) EditorWindow.GetWindow (typeof (SplineWindow));
		l_Window.Show ();
		
		Initialize ();
	}
	
	public static void Initialize () {
		FindSpline ();
		if (s_Spline != null) {
			s_Spline.Initialize ();
			ValidateSelection ();
		}
		
			// Force redraw
		if (s_Spline != null) {
			Vector3 l_Position = s_Spline.transform.position;
			s_Spline.transform.position = s_Spline.transform.position + Vector3.one;
			s_Spline.transform.position = l_Position;
		}
	}
	
	protected void OnGUI () {
		EditorGUILayout.BeginVertical ();
		if (s_Spline != null) {
			EditorGUILayout.LabelField ("Spline validity", s_Spline.Spline.IsValid.ToString ());
			
			ControlPointList ();
			
			if (s_Spline.Spline.Count == 0) {
				GUILayout.BeginHorizontal ();
				GUILayout.FlexibleSpace ();
				if (GUILayout.Button ("Create control points", EditorStyles.miniButtonRight)) {
					GameObject l_GameObject = new GameObject ("ControlPoint");
					l_GameObject.transform.parent = s_Spline.transform;
					SplineControlPointComponent l_SplineControlPoint = l_GameObject.AddComponent <SplineControlPointComponent> ();
					l_SplineControlPoint.T = - 1.0f;
					
					l_GameObject = new GameObject ("ControlPoint");
					l_GameObject.transform.parent = s_Spline.transform;
					l_SplineControlPoint = l_GameObject.AddComponent <SplineControlPointComponent> ();
					l_SplineControlPoint.T = 0.0f;
					
					l_GameObject = new GameObject ("ControlPoint");
					l_GameObject.transform.parent = s_Spline.transform;
					l_SplineControlPoint = l_GameObject.AddComponent <SplineControlPointComponent> ();
					l_SplineControlPoint.T = 1.0f;
					
					l_GameObject = new GameObject ("ControlPoint");
					l_GameObject.transform.parent = s_Spline.transform;
					l_SplineControlPoint = l_GameObject.AddComponent <SplineControlPointComponent> ();
					l_SplineControlPoint.T = 2.0f;
					
					s_Spline.Initialize ();
				}
				GUILayout.EndHorizontal ();
			} else if (s_SelectedControlPoint != null) {
					// Add spline control point after/before selected control point
					// TODO: Position should be calculated on spline, if possible
				GUILayout.BeginHorizontal ();
				GUILayout.FlexibleSpace ();
				if (GUILayout.Button ("Insert control point before selected one", EditorStyles.miniButtonRight)) {
					GameObject l_GameObject = new GameObject ("ControlPoint");
					l_GameObject.transform.parent = s_Spline.transform;
					SplineControlPointComponent l_ControlPoint = l_GameObject.AddComponent <SplineControlPointComponent> ();					
					
					int l_Index = s_Spline.Spline.IndexOf (s_SelectedControlPoint);
					if (l_Index == 0) {
						l_GameObject.transform.position = s_Spline.Spline [0].CachedTransform.position;
						l_ControlPoint.T = s_Spline.Spline [0].T - 1.0f;
					} else {
						SplineControlPointComponent l_PreControlPoint = s_Spline.Spline [l_Index - 1];
						SplineControlPointComponent l_PostControlPoint = s_Spline.Spline [l_Index];
						
						l_ControlPoint.CachedTransform.position = 0.5f * (l_PreControlPoint.CachedTransform.position + l_PostControlPoint.CachedTransform.position);
						l_ControlPoint.T = 0.5f * (l_PreControlPoint.T + l_PostControlPoint.T);
					}
					
					s_Spline.Initialize ();
				}
				GUILayout.EndHorizontal ();
				
				GUILayout.BeginHorizontal ();
				GUILayout.FlexibleSpace ();
				if (GUILayout.Button ("Insert control point after selected one", EditorStyles.miniButtonRight)) {
					GameObject l_GameObject = new GameObject ("ControlPoint");
					l_GameObject.transform.parent = s_Spline.transform;
					SplineControlPointComponent l_ControlPoint = l_GameObject.AddComponent <SplineControlPointComponent> ();					
					
					int l_Index = s_Spline.Spline.IndexOf (s_SelectedControlPoint);
					if (l_Index == s_Spline.Spline.Count - 1) {
						l_GameObject.transform.position = s_Spline.Spline [s_Spline.Spline.Count - 1].CachedTransform.position;
						l_ControlPoint.T = s_Spline.Spline [s_Spline.Spline.Count - 1].T + 1.0f;
					} else {
						SplineControlPointComponent l_PreControlPoint = s_Spline.Spline [l_Index];
						SplineControlPointComponent l_PostControlPoint = s_Spline.Spline [l_Index + 1];
						
						l_ControlPoint.CachedTransform.position = 0.5f * (l_PreControlPoint.CachedTransform.position + l_PostControlPoint.CachedTransform.position);
						l_ControlPoint.T = 0.5f * (l_PreControlPoint.T + l_PostControlPoint.T);
					}
					
					s_Spline.Initialize ();
				}
				GUILayout.EndHorizontal ();
			}
			
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Update", EditorStyles.miniButtonRight)) {
				Initialize ();
			}
			GUILayout.EndHorizontal ();
			
				// Parameter t
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Parameterize using Distance", EditorStyles.miniButtonRight)) {
				s_Spline.Spline.CalculateTsForDistance (c_SubStepsPerSegment);
			}
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Uniform parameter distribution", EditorStyles.miniButtonRight)) {
				s_Spline.Spline.UniformParameterDistribution ();
			}
			GUILayout.EndHorizontal ();
			
				// Entry exit
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			s_SplineEntryExitMode = (SplineEntryExitMode) EditorGUILayout.EnumPopup (s_SplineEntryExitMode);
			if (GUILayout.Button ("Recalculate Entry/Exit", EditorStyles.miniButtonRight)) {
				s_Spline.Spline.SetSplineEntryExitControlPoints (s_SplineEntryExitMode);
			}
			GUILayout.EndHorizontal ();
			
			GizmoOptions ();
		} else {
			
				// Create spline button
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button ("Create Spline", EditorStyles.miniButtonRight)) {
				GameObject l_GameObject = new GameObject ("Spline");
				l_GameObject.AddComponent <SplineComponent> ();
				Selection.activeObject = l_GameObject;
			}
			GUILayout.EndHorizontal ();
		}
		
		EditorGUILayout.EndVertical ();
		
		if (GUI.changed) {
				// Force redraw
			if (s_Spline != null) {
				Vector3 l_Position = s_Spline.transform.position;
				s_Spline.transform.position = s_Spline.transform.position + Vector3.one;
				s_Spline.transform.position = l_Position;
			}
		}
	}

	protected void ControlPointList () {
		bool l_MouseDown = (Event.current.type == EventType.mouseDown);
		Vector2 l_MousePosition = Event.current.mousePosition;
		
		EditorGUIUtility.LookLikeInspector ();
		
		for (int i = 0; i < s_Spline.Spline.ControlPoints.Count; i = i + 1) {
			SplineControlPointComponent l_ControlPoint = s_Spline.Spline.ControlPoints [i];
			Rect l_Rect = EditorGUILayout.BeginHorizontal ();
			GUI.SetNextControlName (l_ControlPoint.GetInstanceID ().ToString ());
			
			string l_Name = l_ControlPoint.name;
			if (i == 0) {
				l_Name = "(Entry) " + l_Name;
			} else if (i == s_Spline.Spline.Count - 1) {
				l_Name = "(Exit) " + l_Name;
			} else {
				l_Name = i.ToString () + " " + l_Name;
			}
			
			l_ControlPoint.T = EditorGUILayout.FloatField (l_Name, l_ControlPoint.T);
			EditorGUILayout.EndHorizontal ();
			
			if (l_MouseDown && l_Rect.Contains (l_MousePosition)) {
				s_SelectedControlPoint = l_ControlPoint;
				Selection.activeGameObject = s_SelectedControlPoint.gameObject;
			}
		}
		
		if (!l_MouseDown && s_SelectedControlPoint != null) {
			GUI.FocusControl (s_SelectedControlPoint.GetInstanceID ().ToString ());
		}
		
		EditorGUILayout.BeginHorizontal ();
		GUI.enabled = false;
		EditorGUILayout.FloatField ("Length", s_Spline.Spline.SplineLength (c_SubStepsPerSegment));
		GUI.enabled = true;
		EditorGUILayout.EndHorizontal ();
	}
	
	protected void GizmoOptions () {
		bool l_MouseDown = (Event.current.type == EventType.mouseDown);
		Vector2 l_MousePosition = Event.current.mousePosition;
		
		Rect l_Rect = EditorGUILayout.BeginVertical ();
		s_GizmoOptionsFoldout = EditorGUILayout.Foldout (s_GizmoOptionsFoldout, "Gizmo options");
		if (s_GizmoOptionsFoldout) {
			int l_CurveSegmentCount = EditorGUILayout.IntField ("Curve segments", s_CurveSegmentCount);
			if (l_CurveSegmentCount >= 1) {
				s_CurveSegmentCount = l_CurveSegmentCount;
			}
			int l_CurveSphereCount = EditorGUILayout.IntField ("Curve spheres", s_CurveSphereCount);
			if (l_CurveSphereCount >= 0) {
				s_CurveSphereCount = l_CurveSphereCount;
			}
			float l_SphereRadius = EditorGUILayout.FloatField ("Sphere radius", s_SphereRadius);
			if (l_SphereRadius > 0.0f) {
				s_SphereRadius = l_SphereRadius;
			}
		}
		EditorGUILayout.EndVertical ();
		
		if (l_MouseDown && l_Rect.Contains (l_MousePosition)) {
			Event.current.Use ();
			s_SelectedControlPoint = null;
		}
	}
	
	protected void OnHierarchyChange () {
		Initialize ();
		
		if (s_Spline != null) {
			s_Spline.Initialize ();
		}
	}
	
	protected void OnSelectionChange () {
		Initialize ();
		
		s_SelectedControlPoint = null;
		
		if (Selection.activeTransform != null) {
			SplineControlPointComponent l_SelectedControlPoint = Selection.activeTransform.GetComponent <SplineControlPointComponent> ();
			if (l_SelectedControlPoint != null) {
				s_SelectedControlPoint = l_SelectedControlPoint;
			}
		}
		
		Repaint ();
	}
}

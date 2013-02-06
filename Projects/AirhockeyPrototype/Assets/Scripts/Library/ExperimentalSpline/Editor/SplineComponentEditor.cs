using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (SplineComponent))]
public class SplineComponentEditor : Editor {

	public override void OnInspectorGUI () {
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Spline Window", EditorStyles.miniButtonRight)) {
			SplineWindow.CreateWindow ();
		}
		GUILayout.EndHorizontal ();
	}
}

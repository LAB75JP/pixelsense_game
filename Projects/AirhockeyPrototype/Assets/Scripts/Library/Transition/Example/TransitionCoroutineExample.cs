//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;
using Edelweiss.Utils;

public class TransitionCoroutineExample : MonoBehaviour {

	public TransitionEnum transition;
	private TransitionDelegate m_TransitionDelegate;
	
	public float transitionTime = 2.0f;
	
	private void Start () {
		m_TransitionDelegate = Transition.GetTransitionDelegate (transition);
	}
	
	private void OnGUI () {
		if (GUILayout.Button ("Start Transition")) {
			StartCoroutine (PerformTransition (OnTransitionCompleted));
		}
	}
	
	private IEnumerator PerformTransition (OnTransitionCompleted a_TransitionCompletedMessage) {
		float l_InverseTransitionTime = 1.0f / transitionTime;
		
		float l_StartValue = 0.0f;
		float l_TargetValue = 10.0f;
		
		float l_Value = 0.0f;
		while (l_Value < 1.0f) {
			l_Value = l_Value + (l_InverseTransitionTime * Time.deltaTime);
			float l_ComputedValue = m_TransitionDelegate (l_StartValue, l_TargetValue, l_Value);
			Debug.Log (l_ComputedValue);
			yield return (null);
		}
		
		a_TransitionCompletedMessage ();
	}
	
	private void OnTransitionCompleted () {
		Debug.Log ("Done");
	}
}

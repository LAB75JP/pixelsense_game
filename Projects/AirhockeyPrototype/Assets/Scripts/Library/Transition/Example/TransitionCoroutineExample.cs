//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012-2013 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;
using Edelweiss.Transition;
using Edelweiss.Utilities;

public class TransitionCoroutineExample : MonoBehaviour {

	public TransitionEnum transition;
	private TransitionDelegate m_TransitionDelegate;
	
	public float transitionTime = 2.0f;
	private Value01Timer m_Value01Timer = new Value01Timer ();
	
	private void Start () {
		m_TransitionDelegate = Transition.GetTransitionDelegate (transition);
	}
	
	private void OnGUI () {
		if (GUILayout.Button ("Start Transition")) {
			StartCoroutine (PerformTransition (OnTransitionCompleted));
		}
	}
	
	private IEnumerator PerformTransition (OnTransitionCompleted a_TransitionCompletedMessage) {
		m_Value01Timer.Initialize (transitionTime);
		
		float l_StartValue = 0.0f;
		float l_TargetValue = 10.0f;
		
		while (m_Value01Timer.Value01 < 1.0f) {
			m_Value01Timer.Progress (Time.deltaTime);
			float l_ComputedValue = m_TransitionDelegate (l_StartValue, l_TargetValue, m_Value01Timer.Value01);
			Debug.Log (l_ComputedValue);
			yield return (null);
		}
		
		a_TransitionCompletedMessage ();
	}
	
	private void OnTransitionCompleted () {
		Debug.Log ("Done");
	}
}

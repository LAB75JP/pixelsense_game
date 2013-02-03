//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2011-2013 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;
using Edelweiss.Transition;
using Edelweiss.Utilities;

public class TransitionExample : MonoBehaviour {

	public TransitionEnum transition;
	private TransitionDelegate m_TransitionDelegate;
	
	public float transitionTime = 2.0f;
	private bool m_Up;
	private Value01Timer m_Value01Timer = new Value01Timer ();
	
	private void Start () {
		m_TransitionDelegate = Transition.GetTransitionDelegate (transition);
		m_Value01Timer.Initialize (transitionTime);
	}
	
	private void Update () {
		if (m_Up) {
			m_Value01Timer.Progress (Time.deltaTime);
			if (m_Value01Timer.Value01 >= 1.0f) {
				m_Up = false;
			}
		} else {
			m_Value01Timer.Progress (- Time.deltaTime);
			if (m_Value01Timer.Value01 <= 0.0f) {
				m_Up = true;
			}
		}
		Debug.Log (m_Value01Timer.Value01 + ": " + m_TransitionDelegate (0.0f, 1.0f, m_Value01Timer.Value01));
	}
}

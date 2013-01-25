//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2011-2012 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;
using Edelweiss.Utils;

public class TransitionExample : MonoBehaviour {

	public TransitionEnum transition;
	private TransitionDelegate m_TransitionDelegate;
	
	public float transitionTime = 2.0f;
	private float m_InverseTransitionTime;
	private bool m_Up;
	private float m_Value = 0.0f;
	
	private void Start () {
		m_InverseTransitionTime = 1.0f / transitionTime;
		m_TransitionDelegate = Transition.GetTransitionDelegate (transition);
	}
	
	private void Update () {
		if (m_Up) {
			m_Value = m_Value + (Time.deltaTime * m_InverseTransitionTime);
			if (m_Value > 1.0f) {
				m_Up = false;
			}
		} else {
			m_Value = m_Value - (Time.deltaTime * m_InverseTransitionTime);
			if (m_Value < 0.0f) {
				m_Up = true;
			}
		}
		Debug.Log (m_Value + ": " + m_TransitionDelegate (0.0f, 1.0f, m_Value));
	}
}

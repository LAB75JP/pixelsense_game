using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;

public class ViewHighScoreState : ApplicationState <ViewHighScoreState> {
	
	private float m_Timeout = 60.0f;
	public float Timeout {
		get {
			return (m_Timeout);
		}
	}
	
	protected override void PreEnter () {
		m_Timeout = 60.0f;
		ViewHighScoreStateGUI.Instance.PreEnter ();
	}

	protected override void Enter () {
		m_Timeout = 60.0f;
		StartCoroutine (TimeoutCoroutineName ());
		ViewHighScoreStateGUI.Instance.Enter ();
	}

	protected override void PreLeave () {
		StopCoroutine (TimeoutCoroutineName ());
		ViewHighScoreStateGUI.Instance.PreLeave ();
	}

	protected override void Leave () {
		StopCoroutine (TimeoutCoroutineName ());
		ViewHighScoreStateGUI.Instance.Leave ();
	}

	protected override void PerformUpdate () {
	}

	protected override void PerformLateUpdate () {
	}
	
	private string TimeoutCoroutineName () {
		return ("TimeoutCoroutine");
	}
	
	private IEnumerator TimeoutCoroutine () {
		while (m_Timeout != 0.0f) {
			yield return (null);
			
			m_Timeout = m_Timeout - Time.deltaTime;
			if (m_Timeout < 0.0f) {
				m_Timeout = 0.0f;
			}
		}
		
		if
			(!ApplicationStateManager.Instance.IsStateChanging &&
			 ApplicationStateManager.Instance.CurrentApplicationState == this as IApplicationState)
		{
			ApplicationStateManager.Instance.TransitionToMode (GameIntroductionState.Instance, 1.0f);
		}
	}
}

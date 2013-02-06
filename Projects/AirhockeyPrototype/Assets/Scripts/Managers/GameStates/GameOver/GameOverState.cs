using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;

public class GameOverState : ApplicationState <GameOverState> {
	
	private float m_Timeout = 30.0f;
	public float Timeout {
		get {
			return (m_Timeout);
		}
	}
	
	protected override void PreEnter () {
		m_Timeout = 30.0f;
		GameOverStateGUI.Instance.PreEnter ();
	}

	protected override void Enter () {
		m_Timeout = 30.0f;
		StartCoroutine (TimeoutCoroutineName ());
		GameOverStateGUI.Instance.Enter ();
	}

	protected override void PreLeave () {
		StopCoroutine (TimeoutCoroutineName ());
		GameOverStateGUI.Instance.PreLeave ();
	}

	protected override void Leave () {
		StopCoroutine (TimeoutCoroutineName ());
		GameOverStateGUI.Instance.Leave ();
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
			ApplicationStateManager.Instance.TransitionToMode (HighScoreAndShareState.Instance, AirhockeyManager.Instance.guiTransitionDuration);
		}
	}
}

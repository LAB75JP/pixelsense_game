using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;

public class PlayGameState : ApplicationState <PlayGameState> {
	
	public GameObject gamePlayObjects;
	
	private bool m_IsRunning;
	public bool IsRunning {
		get {
			return (m_IsRunning);
		}
	}
	
	private float m_Duration = 60.0f;
	public float Duration {
		get {
			return (m_Duration);
		}
	}
	
	private float m_PassedTime;
	public float PassedTime {
		get {
			return (m_PassedTime);
		}
	}
	
	protected override void InitializeApplicationState () {
		gamePlayObjects.SetActive (false);
	}
	
	protected override void PreEnter () {
		PlayGameStateGUI.Instance.PreEnter ();
		
		m_PassedTime = 0.0f;
		m_IsRunning = false;
	}

	protected override void Enter () {
		PlayGameStateGUI.Instance.Enter ();
		gamePlayObjects.SetActive (true);
	}

	protected override void PreLeave () {
		PlayGameStateGUI.Instance.PreLeave ();
		gamePlayObjects.SetActive (false);
	}

	protected override void Leave () {
		PlayGameStateGUI.Instance.descriptionWindow.SetActive (true);
		PlayGameStateGUI.Instance.Leave ();
	}

	protected override void PerformUpdate () {
		if (m_IsRunning) {
			m_PassedTime = m_PassedTime + Time.deltaTime;
			
			if (m_PassedTime > m_Duration) {
				m_PassedTime = m_Duration;
				m_IsRunning = false;
				
				ApplicationStateManager.Instance.TransitionToMode (HighScoreAndShareState.Instance, AirhockeyManager.Instance.guiTransitionDuration);
			}
		}
	}

	protected override void PerformLateUpdate () {
	}
	
	public void StartGame () {
		if (!m_IsRunning && !ApplicationStateManager.Instance.IsStateChanging) {
			PlayGameStateGUI.Instance.descriptionWindow.SetActive (false);
			m_IsRunning = true;
		}
	}
}

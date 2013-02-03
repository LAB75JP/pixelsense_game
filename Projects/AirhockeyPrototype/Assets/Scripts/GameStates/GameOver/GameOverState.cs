using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;

public class GameOverState : ApplicationState <GameOverState> {
	
	protected override void PreEnter () {
		GameOverStateGUI.Instance.PreEnter ();
	}

	protected override void Enter () {
		GameOverStateGUI.Instance.Enter ();
	}

	protected override void PreLeave () {
		GameOverStateGUI.Instance.PreLeave ();
	}

	protected override void Leave () {
		GameOverStateGUI.Instance.Leave ();
	}

	protected override void PerformUpdate () {
	}

	protected override void PerformLateUpdate () {
	}
}

using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;

public class PlayGameState : ApplicationState <PlayGameState> {
	
	protected override void PreEnter () {
		PlayGameStateGUI.Instance.PreEnter ();
	}

	protected override void Enter () {
		PlayGameStateGUI.Instance.Enter ();
	}

	protected override void PreLeave () {
		PlayGameStateGUI.Instance.PreLeave ();
	}

	protected override void Leave () {
		PlayGameStateGUI.Instance.Leave ();
	}

	protected override void PerformUpdate () {
	}

	protected override void PerformLateUpdate () {
	}
}

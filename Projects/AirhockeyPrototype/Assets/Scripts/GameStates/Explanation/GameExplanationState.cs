using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;

public class GameExplanationState : ApplicationState <GameExplanationState> {
	
	protected override void PreEnter () {
		GameExplanationStateGUI.Instance.PreEnter ();
	}

	protected override void Enter () {
		GameExplanationStateGUI.Instance.Enter ();
	}

	protected override void PreLeave () {
		GameExplanationStateGUI.Instance.PreLeave ();
	}

	protected override void Leave () {
		GameExplanationStateGUI.Instance.Leave ();
	}

	protected override void PerformUpdate () {
	}

	protected override void PerformLateUpdate () {
	}
}

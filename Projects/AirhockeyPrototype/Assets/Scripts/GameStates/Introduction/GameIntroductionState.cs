using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;

public class GameIntroductionState : ApplicationState <GameIntroductionState> {
	
	protected override void InitializeApplicationState () {
		
			// We start with this state.
		ApplicationStateManager.Instance.CurrentApplicationState = this;
	}
	
	protected override void PreEnter () {
		GameIntroductionStateGUI.Instance.PreEnter ();
	}

	protected override void Enter () {
		GameIntroductionStateGUI.Instance.Enter ();
	}

	protected override void PreLeave () {
		GameIntroductionStateGUI.Instance.PreLeave ();
	}

	protected override void Leave () {
		GameIntroductionStateGUI.Instance.Leave ();
	}

	protected override void PerformUpdate () {
	}

	protected override void PerformLateUpdate () {
	}
}

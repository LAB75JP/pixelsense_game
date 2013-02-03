using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;

public class ExampleApplicationStateA : ApplicationState <ExampleApplicationStateA> {
	
	protected override void InitializeApplicationState () {
		
			// We start with this state.
		ApplicationStateManager.Instance.CurrentApplicationState = this;
	}
	
	protected override void PreEnter () {
		Debug.Log ("Pre Enter A");
	}

	protected override void Enter () {
		Debug.Log ("Enter A");
	}

	protected override void PreLeave () {
		Debug.Log ("Pre Leave A");
	}

	protected override void Leave () {
		Debug.Log ("Leave A");
	}

	protected override void PerformUpdate () {
		if (!ApplicationStateManager.Instance.IsStateChanging) {
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				ApplicationStateManager.Instance.TransitionToMode (ExampleApplicationStateB.Instance, 2.0f);
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				ApplicationStateManager.Instance.CurrentApplicationState = ExampleApplicationStateB.Instance;
			}
		}
	}

	protected override void PerformLateUpdate () {
	}
}

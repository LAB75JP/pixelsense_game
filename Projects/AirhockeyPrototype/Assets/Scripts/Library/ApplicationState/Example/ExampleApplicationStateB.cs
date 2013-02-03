using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;

public class ExampleApplicationStateB : ApplicationState <ExampleApplicationStateB> {
	
	protected override void PreEnter () {
		Debug.Log ("Pre Enter B");
	}

	protected override void Enter () {
		Debug.Log ("Enter B");
	}

	protected override void PreLeave () {
		Debug.Log ("Pre Leave B");
	}

	protected override void Leave () {
		Debug.Log ("Leave B");
	}

	protected override void PerformUpdate () {
		if (!ApplicationStateManager.Instance.IsStateChanging) {
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				ApplicationStateManager.Instance.TransitionToMode (ExampleApplicationStateA.Instance, 2.0f);
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				ApplicationStateManager.Instance.CurrentApplicationState = ExampleApplicationStateA.Instance;
			}
		}
	}

	protected override void PerformLateUpdate () {
	}
}

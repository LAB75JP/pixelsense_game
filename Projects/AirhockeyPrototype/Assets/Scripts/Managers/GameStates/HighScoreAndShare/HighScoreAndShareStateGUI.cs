using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;

public class HighScoreAndShareStateGUI : ApplicationStateGUI <HighScoreAndShareStateGUI> {
	
	public override bool IsDestroyedOnLoad {
		get {
			return (true);
		}
	}
	
	public override void PreEnter () {
		gameObject.SetActive (true);
		StartCoroutine (FlyInAnimation ());
	}

	public override void Enter () {
		gameObject.SetActive (true);
		SetToCenterPosition ();
	}

	public override void PreLeave () {
		StartCoroutine (FlyOutAnimation ());
	}

	public override void Leave () {
		gameObject.SetActive (false);
	}
}

using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;

public class PlayGameStateGUI : ApplicationStateGUI <PlayGameStateGUI> {
	
	public GameObject descriptionWindow;
	public Transform timerTransform;
	
	public override bool IsDestroyedOnLoad {
		get {
			return (true);
		}
	}
	
	public override void PreEnter () {
		gameObject.SetActive (true);
		StartCoroutine (FlyInAnimation ());
		
		Vector3 l_TimerPosition = timerTransform.localPosition;
		l_TimerPosition.y = 0.5f * NGUIGameLayoutManager.Instance.ScreenHeight - 20.0f;
		timerTransform.localPosition = l_TimerPosition;
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

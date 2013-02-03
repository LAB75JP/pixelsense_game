using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;

public class PlayGameStateGUI : ApplicationStateGUI <PlayGameStateGUI> {
	
	private Transform m_Transform;
	public Transform CachedTransform {
		get {
			if (m_Transform == null) {
				m_Transform = transform;
			}
			return (m_Transform);
		}
	}
	
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

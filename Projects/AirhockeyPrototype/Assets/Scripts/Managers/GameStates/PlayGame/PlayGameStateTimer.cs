using UnityEngine;
using System.Collections;

public class PlayGameStateTimer : MonoBehaviour {

	private UILabel m_Label;
	
	private void Start () {
		m_Label = GetComponent <UILabel> ();
	}
	
	private void Update () {
		int l_PassedTime = Mathf.FloorToInt (PlayGameState.Instance.PassedTime);
		int l_Duration = Mathf.FloorToInt (PlayGameState.Instance.Duration);
		m_Label.text = l_PassedTime.ToString () + " / " + l_Duration;
	}
}

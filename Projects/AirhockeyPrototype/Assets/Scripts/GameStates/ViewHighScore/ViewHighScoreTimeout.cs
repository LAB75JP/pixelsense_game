using UnityEngine;
using System.Collections;

public class ViewHighScoreTimeout : MonoBehaviour {

	private UILabel m_Label;
	
	private void Start () {
		m_Label = GetComponent <UILabel> ();
	}
	
	private void Update () {
		int l_TimeoutInt = Mathf.FloorToInt (ViewHighScoreState.Instance.Timeout);
		m_Label.text = l_TimeoutInt.ToString ();
	}
}

using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;
using Edelweiss.NGUI;

public class ContinueToHighScoreAndShareButtonClickSender : ButtonClickSender {

	protected override void ButtonClickAction () {
		ApplicationStateManager.Instance.TransitionToMode (HighScoreAndShareState.Instance, 1.0f);
	}
}

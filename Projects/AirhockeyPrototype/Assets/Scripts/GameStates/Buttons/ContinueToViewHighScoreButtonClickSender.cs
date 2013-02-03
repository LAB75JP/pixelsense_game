using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;
using Edelweiss.NGUI;

public class ContinueToViewHighScoreButtonClickSender : ButtonClickSender {

	protected override void ButtonClickAction () {
		ApplicationStateManager.Instance.TransitionToMode (ViewHighScoreState.Instance, 1.0f);
	}
}

using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;
using Edelweiss.NGUI;

public class HelpButtonClickSender : ButtonClickSender {

	protected override void ButtonClickAction () {
		ApplicationStateManager.Instance.TransitionToMode (GameExplanationState.Instance, AirhockeyManager.Instance.guiTransitionDuration);
	}
}

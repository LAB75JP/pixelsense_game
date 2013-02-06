using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;
using Edelweiss.NGUI;

public class StartGameButtonClickSender : ButtonClickSender {

	protected override void ButtonClickAction () {
		ApplicationStateManager.Instance.TransitionToMode (CameraFlyInState.Instance, AirhockeyManager.Instance.guiTransitionDuration);
	}
}

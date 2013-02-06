using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;
using Edelweiss.NGUI;

public class ContinueToCameraFlyOutButtonClickSender : ButtonClickSender {

	protected override void ButtonClickAction () {
		ApplicationStateManager.Instance.TransitionToMode (CameraFlyOutState.Instance, AirhockeyManager.Instance.guiTransitionDuration);
	}
}
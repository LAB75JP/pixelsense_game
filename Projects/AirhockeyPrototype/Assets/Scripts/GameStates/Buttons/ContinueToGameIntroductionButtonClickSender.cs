using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;
using Edelweiss.NGUI;

public class ContinueToGameIntroductionButtonClickSender : ButtonClickSender {

	protected override void ButtonClickAction () {
		ApplicationStateManager.Instance.TransitionToMode (GameIntroductionState.Instance, 1.0f);
	}
}
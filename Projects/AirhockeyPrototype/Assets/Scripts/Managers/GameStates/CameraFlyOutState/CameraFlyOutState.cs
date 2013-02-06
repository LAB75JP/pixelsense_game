using UnityEngine;
using System.Collections;
using Edelweiss.ApplicationState;
using Edelweiss.Transition;
using Edelweiss.Utilities;

public class CameraFlyOutState : ApplicationState <CameraFlyOutState> {
	
	private TransitionDelegate m_TransitionDelegate;
	private Value01Timer m_Value01Timer = new Value01Timer ();
	private SplineEnumerator <SplineControlPointComponent> m_SplineEnumerator;
	
	protected override void InitializeApplicationState () {
		m_TransitionDelegate = Transition.GetTransitionDelegate (TransitionEnum.SmoothInOut);
		
			// Dummy call
		if (AirhockeyManager.Instance.cameraPath.Spline != null) {
		}
		m_SplineEnumerator = AirhockeyManager.Instance.cameraPath.SplineEnumerator;
	}
	
	protected override void PreEnter () {
		m_Value01Timer.Initialize (AirhockeyManager.Instance.cameraFlightDuration);
	}

	protected override void Enter () {
		m_SplineEnumerator.MoveToEnd ();
	}

	protected override void PreLeave () {
	}

	protected override void Leave () {
	}

	protected override void PerformUpdate () {
		if (m_Value01Timer.Value01 < 1.0f) {
			m_Value01Timer.Progress (Time.deltaTime);
			float l_SmoothedValue = m_TransitionDelegate (0.0f, 1.0f, m_Value01Timer.Value01);
			
			m_SplineEnumerator.MoveTo ((1.0f - l_SmoothedValue) * m_SplineEnumerator.Spline.LastControlPoint.T);
			CameraManager.Instance.transform.position = m_SplineEnumerator.CurrentPosition;
			
			Quaternion l_IntroductionRotation = AirhockeyManager.Instance.introductionCameraTransform.rotation;
			Quaternion l_PlayRotation = AirhockeyManager.Instance.playCameraTransform.rotation;
			CameraManager.Instance.transform.rotation = Quaternion.Slerp (l_PlayRotation, l_IntroductionRotation, l_SmoothedValue);
			
		} else {
			if (!ApplicationStateManager.Instance.IsStateChanging) {
				ApplicationStateManager.Instance.TransitionToMode (GameIntroductionState.Instance, AirhockeyManager.Instance.guiTransitionDuration);
			}
		}
	}

	protected override void PerformLateUpdate () {
	}
}

using UnityEngine;
using System.Collections;
using Edelweiss.Pattern;
using Edelweiss.Transition;
using Edelweiss.ApplicationState;

public abstract class ApplicationStateGUI <G> : Manager <G> where G : MonoBehaviour {

	private TransitionDelegate m_InTransitionDelegate;
	private TransitionDelegate m_OutTransitionDelegate;
	
	public Transform topLeft;
	public Transform bottomRight;
	
	private Transform m_Transform;
	protected Transform CachedTransform {
		get {
			if (m_Transform == null) {
				m_Transform = transform;
			}
			return (m_Transform);
		}
	}
	
	protected override void InitializeManager () {
		gameObject.SetActive (false);
		
		m_InTransitionDelegate = Transition.GetTransitionDelegate (TransitionEnum.SmoothInOut);
		m_OutTransitionDelegate = Transition.GetTransitionDelegate (TransitionEnum.SmoothInOut);
	}
	
	public abstract void PreEnter ();
	public abstract void Enter ();
	public abstract void PreLeave ();
	public abstract void Leave ();
	
	protected IEnumerator FlyInAnimation () {
		Vector3 l_StartPosition = transform.localPosition;
		l_StartPosition.x = 0.5f * NGUIGameLayoutManager.Instance.ScreenWidth - topLeft.localPosition.x;
		Vector3 l_TargetPosition = transform.localPosition;
		l_TargetPosition.x = 0.0f;
		
		while (ApplicationStateManager.Instance.IsStateChanging) {
			float l_Value = m_InTransitionDelegate (0.0f, 1.0f, ApplicationStateManager.Instance.TransitionValue01);
			transform.localPosition = Vector3.Lerp (l_StartPosition, l_TargetPosition, l_Value);
			yield return (null);
		}
	}
	
	protected void SetToCenterPosition () {
		Vector3 l_Position = transform.localPosition;
		l_Position.x = 0.0f;
		transform.localPosition = l_Position;
	}
	
	protected IEnumerator FlyOutAnimation () {
		Vector3 l_StartPosition = transform.localPosition;
		l_StartPosition.x = 0.0f;
		Vector3 l_TargetPosition = transform.localPosition;
		l_TargetPosition.x = - 0.5f * NGUIGameLayoutManager.Instance.ScreenWidth - bottomRight.localPosition.x;
		
		while (ApplicationStateManager.Instance.IsStateChanging) {
			float l_Value = m_OutTransitionDelegate (0.0f, 1.0f, ApplicationStateManager.Instance.TransitionValue01);
			transform.localPosition = Vector3.Lerp (l_StartPosition, l_TargetPosition, l_Value);
			yield return (null);
		}
	}
}

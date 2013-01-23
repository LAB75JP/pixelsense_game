using UnityEngine;
using System.Collections;

public class Gameplay2Player : MonoBehaviour {
	
	private Transform m_Transform;
	private Rigidbody m_Rigidbody;
	private CapsuleCollider m_Collider;
	private float m_Height;
	
	private bool m_IsInputReady;
	private float m_PrevisousDeltaTime;
	private float m_StartTargetLerpFactor;
	private Vector3 m_StartPosition;
	private Vector3 m_TargetPosition;
	
	private bool m_IsSteered;
	private bool IsSteered {
		get {
			return (m_IsSteered);
		}
		set {
			m_IsSteered = value;
			m_Rigidbody.isKinematic = value;
		}
	}
	
	private void Start () {
		m_Transform = transform;
		m_Height = m_Transform.position.y;
		
		m_Rigidbody = rigidbody;
		
		m_Collider = GetComponent <CapsuleCollider> ();
		m_Collider.isTrigger = false;
		
		IsSteered = false;
	}
	
	private void UpdateInput () {
		m_StartTargetLerpFactor = 0.0f;
		m_StartPosition = m_Transform.position;
		m_TargetPosition = m_Transform.position;
		
		if (!m_IsSteered) {
			if (Input.GetMouseButtonDown (0)) {
				Ray l_Ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit l_RaycastHit;
				if (Physics.Raycast (l_Ray, out l_RaycastHit)) {
					if (l_RaycastHit.collider == m_Collider) {
						IsSteered = true;
					}
				}
			}
		} else {
			if (Input.GetMouseButton (0)) {
				Ray l_Ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				
					// Where is the intersection with y == m_Height?
					// => l_Ray.origin.y + l_Factor * l_Ray.direction.y == m_Height
				float l_Factor = (m_Height - l_Ray.origin.y) / l_Ray.direction.y;
				m_TargetPosition = l_Ray.origin + l_Factor * l_Ray.direction;
			} else {
				IsSteered = false;
			}
		}
		
		m_IsInputReady = true;
	}
	
	private void MoveToPosition (Vector3 a_NewPosition) {
		
			// TODO: Fake Continues Collission Detection
		
		m_Rigidbody.MovePosition (a_NewPosition);
	}
	
	private void FixedUpdate () {
		if (!m_IsInputReady) {
			UpdateInput ();
		}
		
		if (m_IsSteered) {
			if (m_StartTargetLerpFactor != 1.0f) {
				m_StartTargetLerpFactor = m_StartTargetLerpFactor + (Time.deltaTime / m_PrevisousDeltaTime);
				m_StartTargetLerpFactor = Mathf.Clamp01 (m_StartTargetLerpFactor);
				
				Vector3 l_IntermediatePosition = Vector3.Lerp (m_StartPosition, m_TargetPosition, m_StartTargetLerpFactor);
				MoveToPosition (l_IntermediatePosition);
			}
		}
	}
	
	private void Update () {
		if (m_IsSteered) {
			MoveToPosition (m_TargetPosition);
		}
		
		m_PrevisousDeltaTime = Time.smoothDeltaTime;
		m_IsInputReady = false;
	}
}

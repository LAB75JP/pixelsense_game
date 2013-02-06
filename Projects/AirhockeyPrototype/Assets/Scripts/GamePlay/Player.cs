using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
	private Transform m_Transform;
	private Rigidbody m_Rigidbody;
	private CapsuleCollider m_Collider;
	
	private Vector3 m_InitialPosition;
	private float m_Height;
	
	private bool m_IsInputReady;
	private float m_PrevisousDeltaTime;
	private float m_StartTargetLerpFactor;
	private Vector3 m_StartPosition;
	private Vector3 m_TargetPosition;
	private Vector3 m_Velocity;
	private List <RaycastHit> m_RaycastHits = new List <RaycastHit> ();
	
	private bool m_IsSteered;
	private bool IsSteered {
		get {
			return (m_IsSteered);
		}
		set {
			m_IsSteered = value;
		}
	}
	
	private void Start () {
		m_Transform = transform;
		m_InitialPosition = m_Transform.position;
		m_Height = m_InitialPosition.y;
		
		m_Rigidbody = rigidbody;
		m_Rigidbody.isKinematic = true;
		
		m_Collider = GetComponent <CapsuleCollider> ();
		m_Collider.isTrigger = false;
		
		IsSteered = false;
	}
	
	private void OnDisable () {
		m_Transform.position = m_InitialPosition;
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
				if (Physics.Raycast (l_Ray, out l_RaycastHit, Mathf.Infinity, AirhockeyManager.Instance.PuckLayerMask)) {
					if (l_RaycastHit.collider == m_Collider) {
						IsSteered = true;
						PlayGameState.Instance.StartGame ();
					}
				}
			}
		} else {
			if (Input.GetMouseButton (0)) {
				
				Ray l_Ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (l_Ray, Mathf.Infinity, AirhockeyManager.Instance.PuckAreaLayerMask)) {
						// Where is the intersection with y == m_Height?
						// => l_Ray.origin.y + l_Factor * l_Ray.direction.y == m_Height
					float l_Factor = (m_Height - l_Ray.origin.y) / l_Ray.direction.y;
					m_TargetPosition = l_Ray.origin + l_Factor * l_Ray.direction;
					m_Velocity = (m_TargetPosition - m_StartPosition) / m_PrevisousDeltaTime;
				}
			} else {
				IsSteered = false;
			}
		}
		
		m_IsInputReady = true;
	}
	
	private void MoveToPosition (Vector3 a_NewPosition) {
		
			// Fake Continuous Collision Detection
		Vector3 l_Direction = a_NewPosition - m_Transform.position;
		
		RaycastHit[] l_RaycastHits = m_Rigidbody.SweepTestAll (l_Direction.normalized, 1.5f * l_Direction.magnitude);
		if (l_RaycastHits.Length > 0) {
			foreach (RaycastHit l_RaycastHit in l_RaycastHits) {
				if
					(l_RaycastHit.rigidbody != null &&
					 l_RaycastHit.rigidbody != m_Rigidbody)
				{
						// Only one collision per rigidbody.
					bool l_IsRigidbodyAlreadyHandled = false;
					foreach (RaycastHit l_HandledRaycastHit in m_RaycastHits) {
						if (l_HandledRaycastHit.rigidbody == l_RaycastHit.rigidbody) {
							l_IsRigidbodyAlreadyHandled = true;
							break;
						}
					}
					
					if (!l_IsRigidbodyAlreadyHandled) {
						m_RaycastHits.Add (l_RaycastHit);
					}
				}
			}
			
			foreach (RaycastHit l_RaycastHit in m_RaycastHits) {
				Rigidbody l_HitRigidbody = l_RaycastHit.rigidbody;
				l_HitRigidbody.MovePosition (l_HitRigidbody.position + l_Direction);
				l_HitRigidbody.AddForceAtPosition (m_Velocity * 0.5f, l_RaycastHit.point, ForceMode.VelocityChange);
			}
			m_RaycastHits.Clear ();
		}
		
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

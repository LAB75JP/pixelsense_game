using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	public GameObject[] prefabs;
	
	public float initialOffset = 0.0f;
	public float interval = 3.0f;
	
	private float m_CurrentTime;
	
	private void OnEnable () {
		m_CurrentTime = initialOffset;
	}
	
	private void Update () {
		if (PlayGameState.Instance.IsRunning) {
			m_CurrentTime = m_CurrentTime + Time.deltaTime;
			
			if (m_CurrentTime > interval) {
				m_CurrentTime = 0.0f;
				SpawnObject ();
			}
		}
	}
	
	private void SpawnObject () {
		int l_Index = Random.Range (0, prefabs.Length);
		GameObject l_Prefab = prefabs [l_Index];
		
		Quaternion l_Rotation = Quaternion.Euler (Random.Range (0.0f, 360.0f), Random.Range (0.0f, 360.0f), Random.Range (0.0f, 360.0f));
		Instantiate (l_Prefab, transform.position, l_Rotation);
	}
}

using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {
	
	public GameObject[] prefabs;
	
	public float initialOffset = 0.0f;
	public float interval = 3.0f;
	
	private float m_CurrentTime;
	
	private void Start () {
		m_CurrentTime = initialOffset;
	}
	
	private void Update () {
		m_CurrentTime = m_CurrentTime + Time.deltaTime;
		
		if (m_CurrentTime > interval) {
			m_CurrentTime = 0.0f;
			SpawnObject ();
		}
	}
	
	private void SpawnObject () {
		int l_Index = Random.Range (0, prefabs.Length);
		GameObject l_Prefab = prefabs [l_Index];
		
		Instantiate (l_Prefab, transform.position, Quaternion.identity);
	}
}

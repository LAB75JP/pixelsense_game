using UnityEngine;
using System.Collections;

public class SpawnedObject : MonoBehaviour {

	private void Update () {
		if (!PlayGameState.Instance.IsRunning) {
			Destroy (gameObject);
		}
	}
}

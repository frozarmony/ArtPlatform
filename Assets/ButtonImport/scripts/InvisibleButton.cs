using UnityEngine;
using System.Collections;

public class SelectTest : MonoBehaviour {
	
	void OnTriggerEnter(Collider collider){
		transform.parent.renderer.enabled = false;
	}

	void OnTriggerExit(Collider collider){
		transform.parent.renderer.enabled = true;
	}
}

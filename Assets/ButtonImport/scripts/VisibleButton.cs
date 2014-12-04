using UnityEngine;
using System.Collections;

public class VisibleButton : MonoBehaviour {
	void Start ()
	{
		renderer.enabled = false;
	}
	void OnTriggerEnter(Collider collider){
		renderer.enabled = true;
	}
	
	void OnTriggerExit(Collider collider){
		renderer.enabled = false;
	}
}

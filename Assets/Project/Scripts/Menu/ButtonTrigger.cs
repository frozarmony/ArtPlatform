using UnityEngine;
using System.Collections;

public class ButtonTrigger : MonoBehaviour {

	// References
	private MainManager manager;
	private int handAnchorId;

	// Init
	public void InitButtonTrigger(MainManager mainManager, int anchorId){
		manager = mainManager;
		handAnchorId = anchorId;
	}

	// Trigger Handling
	void OnTriggerExit(Collider collid){
		manager.NotifyButtonPush (handAnchorId);
	}
}

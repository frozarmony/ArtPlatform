using UnityEngine;
using System.Collections;

public class MainManager : MonoBehaviour {

	/****************
	 *   Constants  *
	 ****************/

	// Hand's Anchors Ids
	public const int HAND_ANCHOR_PALM			= 0;
	public const int HAND_ANCHOR_THUMB_BASE		= 1;
	public const int HAND_ANCHOR_THUMB_MIDDLE	= 2;
	public const int HAND_ANCHOR_THUMB			= 3;
	public const int HAND_ANCHOR_INDEX			= 4;
	public const int HAND_ANCHOR_MIDDLE			= 5;
	public const int HAND_ANCHOR_RING			= 6;
	public const int HAND_ANCHOR_PINKY			= 7;
	public const int HAND_ANCHOR_COUNT			= 8;

	/****************
	 *   Prefabs    *
	 ****************/

	public GameObject testButton;

	/****************
	 *  Properties  *
	 ****************/

	// Main References
	private HandController handController;

	// Hand's Anchor References
	private Transform[] handAnchors;

	/****************
	 * Constructor  *
	 ****************/

	public MainManager(){
		handController = null;
		handAnchors = new Transform[HAND_ANCHOR_COUNT];
	}

	/****************
	 * Sync Methods *
	 ****************/

	public void SetHandController(HandController ctrl){
		handController = ctrl;
	}

	public void SyncHandAnchor(HandModel model){
		if (model == null) {
			for(int i=0; i<HAND_ANCHOR_COUNT; ++i)
				handAnchors[i] = null;
		}
		else {
			handAnchors[HAND_ANCHOR_PALM]			= model.transform.GetChild(2);
			handAnchors[HAND_ANCHOR_THUMB_BASE]		= model.transform.GetChild(5).GetChild(0);
			handAnchors[HAND_ANCHOR_THUMB_MIDDLE]	= model.transform.GetChild(5).GetChild(1);
			handAnchors[HAND_ANCHOR_THUMB]			= model.transform.GetChild(5).GetChild(2);
			handAnchors[HAND_ANCHOR_INDEX]			= model.transform.GetChild(0).GetChild(2);
			handAnchors[HAND_ANCHOR_MIDDLE]			= model.transform.GetChild(1).GetChild(2);
			handAnchors[HAND_ANCHOR_RING]			= model.transform.GetChild(4).GetChild(2);
			handAnchors[HAND_ANCHOR_PINKY]			= model.transform.GetChild(3).GetChild(2);

			AddHandButton(HAND_ANCHOR_PALM, testButton);
		}
	}
	
	/*****************
	 * Event Methods *
	 *****************/

	public void NotifyButtonPush(int handAnchorId){
		Debug.Log ("Button pushed on id : " + handAnchorId);
	}

	/****************
	 * Tool Methods *
	 ****************/

	private void AddHandButton(int handAnchorId, GameObject buttonPrefab){
		if (handAnchorId < 0 || handAnchorId >= HAND_ANCHOR_COUNT) {
			Debug.LogError ("HandAnchorId does not exist!");
		}
		else {
			// Create Hand Button in Hand Anchor
			GameObject tmp = (GameObject)Object.Instantiate(buttonPrefab);
			tmp.transform.parent = handAnchors[handAnchorId];
			tmp.transform.localPosition = Vector3.zero;

			// Create and Add ButtonTrigger script
			ButtonTrigger trig = tmp.AddComponent<ButtonTrigger>();
			trig.InitButtonTrigger(this, handAnchorId);
		}
	}

}

using UnityEngine;
using System.Collections;

public class HandManager {
	
	/****************
	 *   Constants  *
	 ****************/
	
	// Hand's Anchors Ids
	public static int HAND_ANCHOR_PALM			= 0;
	public static int HAND_ANCHOR_THUMB_BASE	= 1;
	public static int HAND_ANCHOR_THUMB_MIDDLE	= 2;
	public static int HAND_ANCHOR_THUMB			= 3;
	public static int HAND_ANCHOR_INDEX			= 4;
	public static int HAND_ANCHOR_MIDDLE		= 5;
	public static int HAND_ANCHOR_RING			= 6;
	public static int HAND_ANCHOR_PINKY			= 7;
	public static int HAND_ANCHOR_COUNT			= 8;

	// Picking Coeficient
	public static float PICKING_RANGE_COEF		= 2f;
	public static float PICKING_OFFSET_COEF		= 0.5f;
	
	// Opening Coeficient
	public static float OPENING_RANGE_COEF		= 7.4f;
	public static float OPENING_OFFSET_COEF		= 3.6f;

	// Desynchronized Id
	private static int DESYNCHRONIZED_ID		= -1;
	
	/****************
	 *  References  *
	 ****************/

	// MainManager
	private MainManager manager;

	// Leap HandModel Instance Id
	private int instanceId;

	// Hand's Anchor References
	private Transform[] handAnchors;
	
	/****************
	 * Constructor  *
	 ****************/

	public HandManager(MainManager mng){
		// Init References
		manager = mng;
		instanceId = DESYNCHRONIZED_ID;
		handAnchors = new Transform[HAND_ANCHOR_COUNT];
	}
	
	/****************
	 *    Getters   *
	 ****************/

	public bool IsSynchronized(){
		return instanceId != DESYNCHRONIZED_ID;
	}

	public Transform GetAnchor(int handAnchorId){
		if (handAnchorId < 0 || handAnchorId >= HAND_ANCHOR_COUNT) {
			Debug.LogError ("HandAnchorId does not exist!");
			return null;
		}
		else {
			return handAnchors[handAnchorId];

		}
	}
	
	/****************
	 * Sync Methods *
	 ****************/
	
	public void SyncHand(HandModel model){
		// On Synchronization
		if (!IsSynchronized()) {
			// Hand is selected
			instanceId = model.GetInstanceID();
			
			// Sync Hand's Anchors
			handAnchors[HAND_ANCHOR_PALM]			= model.transform.GetChild(2);
			handAnchors[HAND_ANCHOR_THUMB_BASE]		= model.transform.GetChild(5).GetChild(0);
			handAnchors[HAND_ANCHOR_THUMB_MIDDLE]	= model.transform.GetChild(5).GetChild(1);
			handAnchors[HAND_ANCHOR_THUMB]			= model.transform.GetChild(5).GetChild(2);
			handAnchors[HAND_ANCHOR_INDEX]			= model.transform.GetChild(0).GetChild(2);
			handAnchors[HAND_ANCHOR_MIDDLE]			= model.transform.GetChild(1).GetChild(2);
			handAnchors[HAND_ANCHOR_RING]			= model.transform.GetChild(4).GetChild(2);
			handAnchors[HAND_ANCHOR_PINKY]			= model.transform.GetChild(3).GetChild(2);
		}
		// On Desynchronization
		else if(instanceId == model.GetInstanceID()){
			// Hand is not selected
			instanceId = DESYNCHRONIZED_ID;
			
			// Sync Hand's Anchors
			for(int i=0; i<HAND_ANCHOR_COUNT; ++i)
				handAnchors[i] = null;
		}
		 
	}
	
	/*******************
	 * Gesture Methods *
	 *******************/
	
	public float PickingCoef(){
		// Compute Coef
		float dists = 0f;
		dists += Vector3.Distance (handAnchors [HAND_ANCHOR_THUMB].position, handAnchors [HAND_ANCHOR_INDEX].position);
		
		// Normalized Coef
		return Mathf.Clamp01((dists-PICKING_OFFSET_COEF)/PICKING_RANGE_COEF);
	}
	
	public float OpeningCoef(){
		// Compute Coef
		Vector3 palmPos = handAnchors [HAND_ANCHOR_PALM].position;
		float dists = 0f;
		dists += Vector3.Distance (palmPos, handAnchors [HAND_ANCHOR_INDEX].position);
		dists += Vector3.Distance (palmPos, handAnchors [HAND_ANCHOR_MIDDLE].position);
		dists += Vector3.Distance (palmPos, handAnchors [HAND_ANCHOR_RING].position);
		dists += Vector3.Distance (palmPos, handAnchors [HAND_ANCHOR_PINKY].position);
		
		// Normalized Coef
		return Mathf.Clamp01((dists-OPENING_OFFSET_COEF)/OPENING_RANGE_COEF);
	}
}

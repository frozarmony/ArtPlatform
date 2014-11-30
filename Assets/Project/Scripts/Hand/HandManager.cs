using UnityEngine;
using System.Collections;

public class HandManager {
	
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

	// Picking Coeficient
	public const float PICKING_RANGE_COEF		= 2f;
	public const float PICKING_OFFSET_COEF		= 0.5f;
	
	// Opening Coeficient
	public const float OPENING_RANGE_COEF		= 7.4f;
	public const float OPENING_OFFSET_COEF		= 3.6f;
	
	/****************
	 *  References  *
	 ****************/

	// MainManager
	private MainManager manager;

	// Hand's Anchor References
	private bool isSynchronized;
	private Transform[] handAnchors;
	
	/****************
	 * Constructor  *
	 ****************/

	public HandManager(MainManager mng){
		manager = mng;
		isSynchronized = false;
		handAnchors = new Transform[HAND_ANCHOR_COUNT];
	}
	
	/****************
	 *    Getters   *
	 ****************/

	public bool IsSynchronized(){
		return isSynchronized;
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
		if (model == null) {
			// Hand is not selected
			isSynchronized = false;
			
			// Sync Hand's Anchors
			for(int i=0; i<HAND_ANCHOR_COUNT; ++i)
				handAnchors[i] = null;
		}
		else {
			// Hand is selected
			isSynchronized = true;
			
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

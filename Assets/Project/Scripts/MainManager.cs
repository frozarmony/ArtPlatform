using UnityEngine;
using System.Collections.Generic;

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

	/*************************
	 *    Button's Prefabs   *
	 *************************/

	public GameObject openButton;
	public GameObject closeButton;
	public GameObject backButton;
	public GameObject nextButton;
	public GameObject previousButton;
	public GameObject matMenuButton;
	public GameObject modMenuButton;

	/****************
	 *  References  *
	 ****************/

	// Main References
	private HandController handController;

	// Hand's Anchor References
	private bool leftHandIsSynched;
	private Transform[] handAnchors;

	// Menu's Reference
	private HandMenu currentMenu;
	private Dictionary<string, HandMenu> menusIndex;

	/****************
	 * Constructor  *
	 ****************/

	public MainManager(){
		// Init References
		handController = null;
		leftHandIsSynched = false;
		handAnchors = new Transform[HAND_ANCHOR_COUNT];
		menusIndex = new Dictionary<string, HandMenu> ();

		// Init Menus Index
		currentMenu = null;
		InitMenusIndex ();

		// Load Main Menu
		LoadMenu("ClosedMenu");
	}

	private void InitMenusIndex(){
		menusIndex ["ClosedMenu"] = new ClosedMenu (this);
		menusIndex ["PaintMenu"] = new PaintMenu (this);
	}

	/****************
	 * Sync Methods *
	 ****************/

	public void SetHandController(HandController ctrl){
		handController = ctrl;
	}

	public void SyncHand(HandModel model){
		if (model == null) {
			leftHandIsSynched = false;
			for(int i=0; i<HAND_ANCHOR_COUNT; ++i)
				handAnchors[i] = null;
		}
		else {
			// Left Hand is selected
			leftHandIsSynched = true;

			// Sync Hand's Anchors
			handAnchors[HAND_ANCHOR_PALM]			= model.transform.GetChild(2);
			handAnchors[HAND_ANCHOR_THUMB_BASE]		= model.transform.GetChild(5).GetChild(0);
			handAnchors[HAND_ANCHOR_THUMB_MIDDLE]	= model.transform.GetChild(5).GetChild(1);
			handAnchors[HAND_ANCHOR_THUMB]			= model.transform.GetChild(5).GetChild(2);
			handAnchors[HAND_ANCHOR_INDEX]			= model.transform.GetChild(0).GetChild(2);
			handAnchors[HAND_ANCHOR_MIDDLE]			= model.transform.GetChild(1).GetChild(2);
			handAnchors[HAND_ANCHOR_RING]			= model.transform.GetChild(4).GetChild(2);
			handAnchors[HAND_ANCHOR_PINKY]			= model.transform.GetChild(3).GetChild(2);

			// Reload Current Menu if needed
			if(currentMenu != null)
				currentMenu.OnLoad();
		}
	}
	
	/*****************
	 * Event Methods *
	 *****************/

	public void NotifyButtonPush(int handAnchorId){
		if (currentMenu != null)
			currentMenu.OnTouch (handAnchorId);
	}

	/****************
	 * Tool Methods *
	 ****************/

	public void LoadMenu(string menuName){
		Debug.Log("Try to Load : " + menuName);
		if (!menusIndex.ContainsKey (menuName)) {
				Debug.LogError ("HandMenu '" + menuName + "' does not exist!");
		}
		else {
			// Unload Current Menu if needed
			if( currentMenu != null )
				for(int i=0; i<HAND_ANCHOR_COUNT; ++i)
					UnloadHandButton(i);

			// Load Menu
			currentMenu = menusIndex[menuName];
			currentMenu.OnLoad();
		}
	}

	public void LoadHandButton(int handAnchorId, GameObject buttonPrefab){
		if (leftHandIsSynched){
			if (handAnchorId < 0 || handAnchorId >= HAND_ANCHOR_COUNT) {
				Debug.LogError ("HandAnchorId does not exist!");
			} else if (buttonPrefab == null) {
				Debug.LogError ("ButtonPrefab reference is null for anchor : " + handAnchorId );
			}
			else {
				// Create Hand Button in Hand Anchor
				GameObject tmp = (GameObject)Object.Instantiate (buttonPrefab);
				tmp.transform.localRotation = handAnchors [handAnchorId].transform.rotation;
				tmp.transform.parent = handAnchors [handAnchorId];
				tmp.transform.localPosition = Vector3.zero;
			
				// Create and Add ButtonTrigger script
				ButtonTrigger trig = tmp.AddComponent<ButtonTrigger> ();
				trig.InitButtonTrigger (this, handAnchorId);
			}
		}
	}

	public void UnloadHandButton(int handAnchorId){
		if (leftHandIsSynched)
			if (handAnchorId < 0 || handAnchorId >= HAND_ANCHOR_COUNT) {
				Debug.LogError ("HandAnchorId does not exist!");
			}
			else {
				// If Anchor has Child, destroy it
				for(int i=0; i<handAnchors[handAnchorId].childCount; ++i){
					Object.Destroy(handAnchors[handAnchorId].GetChild(i).gameObject);
				}
			}
	}

}

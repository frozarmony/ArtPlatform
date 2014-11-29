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

	// Opening Coeficient
	public const float OPENING_RANGE_COEF		= 7.4f;
	public const float OPENING_OFFSET_COEF		= 3.6f;

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
	
	/*************************
	 *    Canvas's Prefab    *
	 *************************/
	
	public GameObject canvasPrefab;

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

	// Paint's Reference
	private GameObject currentCanvas;
	private Queue<AbstractAction> doneAction;
	private Queue<AbstractAction> undoneAction;
	
	/****************
	 * Constructor  *
	 ****************/

	public MainManager(){
		// Init References
		handController = null;
		leftHandIsSynched = false;
		handAnchors = new Transform[HAND_ANCHOR_COUNT];
		menusIndex = new Dictionary<string, HandMenu> ();
		currentCanvas = null;
		doneAction = new Queue<AbstractAction> ();
		undoneAction = new Queue<AbstractAction> ();

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
			// Left Hand is not selected
			leftHandIsSynched = false;
			
			// Sync Hand's Anchors
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

			// Create Empty Canvas
			CreateNewCanvas();
		}
	}

	/*****************
	 *    Update     *
	 *****************/

	public void Update(){

	}
	
	/*****************
	 * Event Methods *
	 *****************/

	public void NotifyButtonPush(int handAnchorId){
		if (currentMenu != null)
			currentMenu.OnTouch (handAnchorId);
	}

	/*******************
	 * Gesture Methods *
	 *******************/

	private float OpeningCoef(){
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

	/****************
	 * Menu Methods *
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
		if (leftHandIsSynched) {
			if (handAnchorId < 0 || handAnchorId >= HAND_ANCHOR_COUNT) {
					Debug.LogError ("HandAnchorId does not exist!");
			} else {
					// If Anchor has Child, destroy it
					for (int i=0; i<handAnchors[handAnchorId].childCount; ++i) {
							Object.Destroy (handAnchors [handAnchorId].GetChild (i).gameObject);
					}
			}
		}
	}
	
	/******************
	 * Paint Methods  *
	 ******************/

	public void CreateNewCanvas(){
		currentCanvas = (GameObject)Instantiate (canvasPrefab, Vector3.zero, Quaternion.identity);
	}

	public GameObject PaintOnCanvas(GameObject matPrefab, Vector3 pos, Quaternion rotation){
		if (currentCanvas == null) {
			Debug.LogError ("Can't paint : No canvas!");
			return null;
		}
		else {
			GameObject matInstance = (GameObject)Instantiate (matPrefab, pos, rotation);
			matInstance.transform.parent = currentCanvas.transform;
			return matInstance;
		}
	}

	/**********************
	 * Undo/Redo Methods  *
	 **********************/

	public void DoAction(AbstractAction action){
		action.Do ();
		doneAction.Enqueue (action);
	}

	public void UndoAction(){
		if (doneAction.Count != 0) {
			AbstractAction action = doneAction.Dequeue();
			action.Undo();
			undoneAction.Enqueue(action);
		}
	}
	
	public void RedoAction(){
		if (undoneAction.Count != 0) {
			AbstractAction action = undoneAction.Dequeue();
			action.Do ();
			doneAction.Enqueue(action);
		}
	}

}

using UnityEngine;
using System.Collections.Generic;
using Leap;

public class MainManager : MonoBehaviour {

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

	public GameObject matTest;

	/****************
	 *  References  *
	 ****************/

	// Main References
	private HandController handController;

	// Hand's Anchor References
	private HandManager leftHand;
	private HandManager rightHand;

	// Menu's Reference
	private HandMenu currentMenu;
	private Dictionary<string, HandMenu> menusIndex;

	// Paint's Reference
	private GameObject currentCanvas;
	private Queue<AbstractAction> doneAction;
	private Queue<AbstractAction> undoneAction;

	private GestureTracker test;
	
	/****************
	 * Constructor  *
	 ****************/

	public MainManager(){
		// Init References
		handController = null;
		leftHand = new HandManager (this);
		rightHand = new HandManager (this);
		menusIndex = new Dictionary<string, HandMenu> ();
		currentCanvas = null;
		doneAction = new Queue<AbstractAction> ();
		undoneAction = new Queue<AbstractAction> ();
	}

	public void Start(){
		// Init Menus Index
		currentMenu = null;
		InitMenusIndex ();
		
		// Load Main Menu
		LoadMenu("ClosedMenu");
		
		// Create Empty Canvas
		CreateNewCanvas();
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
		test = new SimplePickTracker (this, rightHand);
		test.OnLoad ();
	}

	/*****************
	 *    Update     *
	 *****************/

	public void Update(){
		test.OnUpdate ();
	}
	
	/*****************
	 * Event Methods *
	 *****************/

	public void NotifyButtonPush(int handAnchorId){
		if (currentMenu != null)
			currentMenu.OnTouch (handAnchorId);
	}

	public void SyncLeftHand(HandModel model){
		leftHand.SyncHand (model);

		// Reload Current Menu if needed
		if(leftHand.IsSynchronized() && currentMenu != null)
			currentMenu.OnLoad();
	}
	
	public void SyncRightHand(HandModel model){
		rightHand.SyncHand (model);
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
				for(int i=0; i<HandManager.HAND_ANCHOR_COUNT; ++i)
					UnloadHandButton(i);

			// Load Menu
			currentMenu = menusIndex[menuName];
			currentMenu.OnLoad();
		}
	}

	public void LoadHandButton(int handAnchorId, GameObject buttonPrefab){
		if (leftHand.IsSynchronized()){
			if (buttonPrefab == null) {
				Debug.LogError ("ButtonPrefab reference is null for anchor : " + handAnchorId );
			}
			else {
				// Create Hand Button in Hand Anchor
				GameObject tmp = (GameObject)Object.Instantiate (buttonPrefab);
				tmp.transform.localRotation = leftHand.GetAnchor(handAnchorId).rotation;
				tmp.transform.parent = leftHand.GetAnchor(handAnchorId);
				tmp.transform.localPosition = Vector3.zero;
			
				// Create and Add ButtonTrigger script
				ButtonTrigger trig = tmp.AddComponent<ButtonTrigger> ();
				trig.InitButtonTrigger (this, handAnchorId);
			}
		}
	}
	
	public void UnloadHandButton(int handAnchorId){
		if (leftHand.IsSynchronized()) {
			// Get Anchor
			Transform anchor = leftHand.GetAnchor(handAnchorId);

			// If Anchor has a Child, destroy it
			for (int i=0; i<anchor.childCount; ++i) {
				Object.Destroy (anchor.GetChild (i).gameObject);
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

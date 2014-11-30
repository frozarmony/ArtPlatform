using UnityEngine;
using System.Collections.Generic;
using Leap;

public class MainManager : MonoBehaviour {
	
	/*************************
	 *       Constants       *
	 *************************/

	public enum ContextOfGesture {Undefined, Menu, PaintMode};
	public enum PaintMode {SimplePicking};

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

	// Context of Gesture's Reference
	private ContextOfGesture currentContext;
	private List<GestureTracker> gestureTrackers;
	private SimplePickTracker simplePickTracker;

	// Paint's Reference
	private GameObject currentCanvas;
	private PaintMode currentPaintMode;
	private Queue<AbstractAction> doneAction;
	private Queue<AbstractAction> undoneAction;
	
	/****************
	 * Constructor  *
	 ****************/

	public MainManager(){
		// Init Main References
		handController = null;
		leftHand = new HandManager (this);
		rightHand = new HandManager (this);
		
		// Init Menu's References
		menusIndex = new Dictionary<string, HandMenu> ();

		// Init Context of Gesture's Reference
		gestureTrackers = new List<GestureTracker> ();
		
		// Init Paint's References
		currentCanvas = null;
		doneAction = new Queue<AbstractAction> ();
		undoneAction = new Queue<AbstractAction> ();
	}

	public void Start(){
		// Init Menu's Index
		InitMenusIndex ();

		// Init Contexts Of Gesture
		InitContextsOfGesture ();
		
		// Load Main Menu
		LoadMenu("ClosedMenu");
		
		// Create Empty Canvas
		CreateNewCanvas();
		currentPaintMode = PaintMode.SimplePicking;
	}
	
	private void InitMenusIndex(){
		currentMenu = null;
		menusIndex ["ClosedMenu"] = new ClosedMenu (this);
		menusIndex ["PaintMenu"] = new PaintMenu (this);
	}

	private void InitContextsOfGesture(){
		currentContext = ContextOfGesture.Undefined;
		simplePickTracker = new SimplePickTracker (this, rightHand);
	}

	/****************
	 * Sync Methods *
	 ****************/

	public void SetHandController(HandController ctrl){
		handController = ctrl;
	}

	/*****************
	 *    Update     *
	 *****************/

	public void Update(){
		// Update each GestureTracker
		foreach(GestureTracker tracker in gestureTrackers)
			tracker.OnUpdate();
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

			// Set Corresponding Context of Gesture
			SetContextOfGesture(currentMenu.GetContextOfGesture());
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
	
	/******************************
	 * Context of Gesture Methods *
	 ******************************/

	public void SetContextOfGesture(ContextOfGesture newContext){
		if (currentContext != newContext) {
			// Unload Old Context
			foreach(GestureTracker tracker in gestureTrackers)
				tracker.OnUnload();
			gestureTrackers.Clear();

			// Load New Context
			switch(newContext)
			{
				// Menu Context
				case ContextOfGesture.Menu:
					// Do Nothing
					break;
				// PaintMode
				case ContextOfGesture.PaintMode:
					if(currentPaintMode == PaintMode.SimplePicking)
						gestureTrackers.Add(simplePickTracker);
					break;
				default:
					// Do Nothing
					break;
			}
			foreach(GestureTracker tracker in gestureTrackers)
				tracker.OnLoad();
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

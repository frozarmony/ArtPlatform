using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

public class MainManager : MonoBehaviour {
	
	/*************************
	 *       Constants       *
	 *************************/

	public enum ContextOfGesture {Undefined, Menu, PaintMode};
	public enum PaintMode {SimplePicking, Painting};

	/*************************
	 *    Button's Prefabs   *
	 *************************/

	public GameObject openButton;
	public GameObject closeButton;
	public GameObject returnButton;
	public GameObject arrowButton;
	public GameObject matMenuButton;
	public GameObject modMenuButton;
	public GameObject pickingButton;
	public GameObject paintingButton;
	public GameObject choiceButton;
	public GameObject materialProjector;
	
	/*************************
	 *    General's Prefab    *
	 *************************/
	
	public GameObject canvasPrefab;
	public GameObject painterPrefab;
	public GameObject onTouchExplosionPrefab;

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
	private int handItemToBeUnloadedCounter;

	// Context of Gesture's Reference
	private ContextOfGesture currentContext;
	private List<GestureTracker> gestureTrackers;
	private SimplePickTracker simplePickTracker;
	private PaintTracker paintTracker;

	// Paint's Reference
	private GameObject currentCanvas;
	private PaintMode currentPaintMode;
	private Stack<AbstractAction> doneAction;
	private Stack<AbstractAction> undoneAction;

	// Palette's References
	private List<ArtMaterial> simplePickPalette;
	private List<ArtMaterial> paintingPalette;
	private int cursorSimplePickPalette;
	private int cursorPaintingPalette;
	
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
		handItemToBeUnloadedCounter = 0;

		// Init Context of Gesture's Reference
		gestureTrackers = new List<GestureTracker> ();
		
		// Init Paint's References
		currentCanvas = null;
		doneAction = new Stack<AbstractAction> ();
		undoneAction = new Stack<AbstractAction> ();
	}

	public void Start(){
		// Load & Init Palette
		LoadPalette ();

		// Init Menu's Index
		InitMenusIndex ();

		// Init Contexts Of Gesture
		InitContextsOfGesture ();
		
		// Create Empty Canvas
		CreateNewCanvas();
		currentPaintMode = PaintMode.Painting;
		
		// Load Main Menu
		LoadMenu("ClosedMenu");
	}
	
	private void InitMenusIndex(){
		currentMenu = null;
		menusIndex ["ClosedMenu"] = new ClosedMenu (this);
		menusIndex ["PaintMenu"] = new PaintMenu (this);
		menusIndex ["ModeMenu"] = new ModeMenu (this);
	}

	private void InitContextsOfGesture(){
		currentContext = ContextOfGesture.Undefined;
		simplePickTracker = new SimplePickTracker (this, rightHand);
		paintTracker = new PaintTracker (this, leftHand, rightHand);
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
	
	public PaintMode GetPaintMode(){
		return currentPaintMode;
	}
	
	public void SetPaintMode(PaintMode paintMode){
		currentPaintMode = paintMode;
	}

	public void LoadMenu(string menuName){
		if (!menusIndex.ContainsKey (menuName)) {
				Debug.LogError ("HandMenu '" + menuName + "' does not exist!");
		}
		else {
			// Unload Current Menu if needed
			UnloadMenu();

			// Set Menu to be loaded asynchronously
			currentMenu = menusIndex[menuName];
			
			// Set Corresponding Context of Gesture
			SetContextOfGesture(currentMenu.GetContextOfGesture());
		}
	}

	public void UnloadMenu(){
		if( currentMenu != null && leftHand.IsSynchronized() ){
			handItemToBeUnloadedCounter = 0;
			for(int i=0; i<HandManager.HAND_ANCHOR_COUNT; ++i){
				foreach(HandItem item in leftHand.GetAnchor(i).GetComponentsInChildren<HandItem>()){
					item.StartUnloading();
					++handItemToBeUnloadedCounter;
				}
			}
		}
	}
	
	public void NotifyHandItemUnloaded(){
		// Decrement hand item counter & wait 0
		--handItemToBeUnloadedCounter;
		if (handItemToBeUnloadedCounter <= 0) {
			// Unload Current Menu if needed
			if( currentMenu != null )
				for(int i=0; i<HandManager.HAND_ANCHOR_COUNT; ++i)
					UnloadHandItem(i);
			
			// Load Menu
			if(leftHand.IsSynchronized())
				currentMenu.OnLoad();
		}
	}

	public void LoadHandItem(HandItem button){
		LoadHandItem(button, Vector3.zero);
	}

	public void LoadHandItem(HandItem item, Vector3 rotation){
		if (leftHand.IsSynchronized()){
			if (item == null) {
				Debug.LogError ("ButtonPrefab reference is null");
			}
			else {
				// Load Hand Item in Hand Anchor
				Transform anchor = leftHand.GetAnchor(item.GetHandHanchorId());
				item.transform.localRotation = anchor.rotation;
				item.transform.Rotate(rotation);
				item.transform.parent = anchor;
				item.transform.localPosition = Vector3.zero;
			}
		}
	}
	
	public void UnloadHandItem(int handAnchorId){
		if (leftHand.IsSynchronized()) {
			// Get Anchor
			Transform anchor = leftHand.GetAnchor(handAnchorId);

			// If Anchor has a Child, destroy it
			foreach (HandItem button in anchor.GetComponentsInChildren<HandItem>()) {
				Object.Destroy (button.gameObject);
			}
		}
	}

	public void SelectButton(int handAnchorId, bool selected){
		if (leftHand.IsSynchronized()) {
			// Get Anchor
			Transform anchor = leftHand.GetAnchor(handAnchorId);

			// Select or Deselect Button
			anchor.GetComponentInChildren<ButtonItem>().SetSelected(selected);
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

				// Select correct PaintMode Tracker
				switch(currentPaintMode)
				{
				// Simple Picking
				case PaintMode.SimplePicking:
					gestureTrackers.Add(simplePickTracker);
					break;
					
				// Painting
				case PaintMode.Painting:
					gestureTrackers.Add(paintTracker);
					break;
				}
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

	public bool DrawPainter(ParticleSystem particlePainter, Material material, float size, float density, float granularity){
		// Compute Particle Count
		int particleCount = Mathf.FloorToInt(size*size*density/granularity + Random.Range (0f, 1f));

		if(particleCount != 0){
			// Generate Cloud
			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleCount];
			for(int i=0; i<particleCount; ++i){
				particles[i].position = Random.insideUnitSphere * size;
				particles[i].size = granularity;
				particles[i].color = new Color(0,90,190);
				particles[i].rotation = Random.Range(0f,360f);
			}

			// Load Cloud
			particlePainter.SetParticles (particles, particleCount);

			// Set Up Renderer
			particlePainter.renderer.material = material;

			return true;
		}
		else{
			return false;
		}
	}

	/**********************
	 * Undo/Redo Methods  *
	 **********************/

	public void DoAction(AbstractAction action){
		action.Do ();
		doneAction.Push (action);
		undoneAction.Clear ();
	}

	public void UndoAction(){
		if (doneAction.Count != 0) {
			AbstractAction action = doneAction.Pop();
			action.Undo();
			undoneAction.Push(action);
		}
	}
	
	public void RedoAction(){
		if (undoneAction.Count != 0) {
			AbstractAction action = undoneAction.Pop();
			action.Do ();
			doneAction.Push(action);
		}
	}
	
	/*******************
	 * Palette Methods *
	 *******************/

	private void LoadPalette(){
		// Load Palette's Materials
		ArtMaterial[] palette = Resources.LoadAll<ArtMaterial> ("Palette");
		
		// Init Sub-Palettes
		simplePickPalette = new List<ArtMaterial> ();
		paintingPalette = new List<ArtMaterial> ();
		cursorSimplePickPalette = 0;
		cursorPaintingPalette = 0;

		// Sort Palettes
		foreach (ArtMaterial mat in palette) {
			switch (mat.category) {
			case ArtMaterial.Category.SolidObject:
				simplePickPalette.Add(mat);
				break;
			case ArtMaterial.Category.ParticuleEmitter:
				simplePickPalette.Add(mat);
				break;
			case ArtMaterial.Category.PaintTexture:
				paintingPalette.Add(mat);
				break;
			}
		}
	}

	public void NextMaterial(){
		if (currentPaintMode == PaintMode.SimplePicking && simplePickPalette.Count > 0) {
			cursorSimplePickPalette = ++cursorSimplePickPalette % simplePickPalette.Count;
		}
		else if (currentPaintMode == PaintMode.Painting && paintingPalette.Count > 0) {
			cursorPaintingPalette = ++cursorPaintingPalette % paintingPalette.Count;
		}
	}

	public void PreviousMaterial(){
		if (currentPaintMode == PaintMode.SimplePicking && simplePickPalette.Count > 0) {
			cursorSimplePickPalette = (simplePickPalette.Count + cursorSimplePickPalette - 1) % simplePickPalette.Count;
		}
		else if (currentPaintMode == PaintMode.Painting && paintingPalette.Count > 0) {
			cursorPaintingPalette = (paintingPalette.Count + cursorPaintingPalette - 1) % paintingPalette.Count;
		}
	}

	public ArtMaterial GetCurrentSimplePickMaterial(){
		if (cursorSimplePickPalette == -1)
			return null;
		else
			return simplePickPalette [cursorSimplePickPalette];
	}
	
	public ArtMaterial GetCurrentPaintingMaterial(){
		if (cursorPaintingPalette == -1)
			return null;
		else
			return paintingPalette [cursorPaintingPalette];
	}

}

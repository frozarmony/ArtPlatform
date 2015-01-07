using UnityEngine;
using System.Collections;

public class ColorMenu  : HandMenu {
	
	/******************
	 *  Button's ID   *
	 ******************/
	
	private int backButtonId;
	private int nextPageButtonId;
	private int previousPageButtonId;
	
	private int materialProjectorId;
	
	/****************
	 *  References  *
	 ****************/

	private Color[] palette;
	private int page;
	private ChoiceButtonItem[] colorChoiceButtons;
	
	private MaterialProjectorItem materialProjector;
	private Transform previewInstance;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public ColorMenu(MainManager manager) : base(manager, MainManager.ContextOfGesture.Menu){
		// Buttons
		backButtonId = HandManager.HAND_ANCHOR_THUMB;
		previousPageButtonId = HandManager.HAND_ANCHOR_THUMB_BASE;
		nextPageButtonId = HandManager.HAND_ANCHOR_THUMB_MIDDLE;

		// Palette
		palette = manager.GetColorPalette ();

		// Page
		page = 0;
		
		// Material Projector
		materialProjectorId = HandManager.HAND_ANCHOR_PALM;
		previewInstance = null;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void OnLoad(){
		// Buttons
		manager.LoadHandItem (CreateChoiceButton (manager, backButtonId, manager.movingModeTexture), new Vector3(90,0,0));
		manager.LoadHandItem (CreateStandardButton(manager.arrowButton, manager, nextPageButtonId), new Vector3(0,0,90));
		manager.LoadHandItem (CreateStandardButton(manager.arrowButton, manager, previousPageButtonId), new Vector3(0,0,-90));

		// Load Color Choice Buttons
		colorChoiceButtons = new ChoiceButtonItem[4];
		for(int i=4; i<4; ++i){
			colorChoiceButtons[i] = CreateChoiceButton(manager, HandManager.HAND_ANCHOR_INDEX+i, manager.colorDisplayTexture);
			manager.LoadHandItem (colorChoiceButtons[i]);
		}
		
		// Material Projector
		materialProjector = CreateMaterialProjectorItem (manager, materialProjectorId);
		manager.LoadHandItem (materialProjector);
		
		// Update View
		UpdateCurrentColor ();
		UpdateCurrentMat ();
	}
	
	public override void OnTouch(int hanchorId){
		if (hanchorId == backButtonId){
			manager.LoadMenu ("PaletteMenu");
		} else if (hanchorId == nextPageButtonId && (page+1)*4 < palette.Length ) {
			++page;

		} else if (hanchorId == previousPageButtonId && page > 0) {
			--page;
		}
		
		// Update View
		UpdateCurrentColor ();
		UpdateCurrentMat ();
	}
	
	/******************
	 *  Tool Methods  *
	 ******************/
	
	private void UpdateCurrentColor(){
		int cursor = manager.GetCursorColorPalette();
		int offset = page * 4;
		for(int i=0; i<4; ++i){
			// Set Color
			colorChoiceButtons[i].SetMainColors(palette[offset+i], Color.black);

			// Set Selected/Unselected
			manager.SelectButton(i, ((i+offset) == cursor));
		}
	}
	
	private void UpdateCurrentMat(){
		// Destroy old instance
		if (previewInstance != null)
			Object.Destroy(previewInstance.gameObject);
		
		MainManager.PaintMode currentPaintMode = manager.GetPaintMode();
		if (currentPaintMode == MainManager.PaintMode.SimplePicking) {
			previewInstance = ((GameObject) Object.Instantiate(manager.GetCurrentSimplePickMaterial().gameObject)).transform;
		}
		else if (currentPaintMode == MainManager.PaintMode.Painting) {
			previewInstance = ((GameObject) Object.Instantiate(manager.painterPrefab)).transform;
			manager.DrawPainter(previewInstance.particleSystem, ((ArtPaintingMaterial) manager.GetCurrentPaintingMaterial()).material, 0.7f, 20.0f, 0.4f);
		}
		materialProjector.LoadMaterial (previewInstance);
	}
}

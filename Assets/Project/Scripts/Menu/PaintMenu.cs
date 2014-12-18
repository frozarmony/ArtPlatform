using UnityEngine;
using System.Collections;

public class PaintMenu : HandMenu {
	
	/******************
	 *  Button's ID   *
	 ******************/
	
	private int closeButtonId;
	private int nextMatButtonId;
	private int previousMatButtonId;
	private int pickingButtonId;
	private int paintingButtonId;

	private int materialProjectorId;
	
	/****************
	 *  References  *
	 ****************/

	private MaterialProjectorItem materialProjector;
	private Transform previewInstance;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public PaintMenu(MainManager manager) : base(manager, MainManager.ContextOfGesture.Menu){
		// Buttons
		closeButtonId = HandManager.HAND_ANCHOR_THUMB;
		previousMatButtonId = HandManager.HAND_ANCHOR_THUMB_BASE;
		nextMatButtonId = HandManager.HAND_ANCHOR_THUMB_MIDDLE;
		pickingButtonId = HandManager.HAND_ANCHOR_INDEX;
		paintingButtonId = HandManager.HAND_ANCHOR_MIDDLE;

		// Material Projector
		materialProjectorId = HandManager.HAND_ANCHOR_PALM;
		previewInstance = null;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void OnLoad(){
		// Buttons
		manager.LoadHandItem (CreateStandardButton(manager.closeButton, manager, closeButtonId));
		manager.LoadHandItem (CreateStandardButton(manager.arrowButton, manager, nextMatButtonId), new Vector3(0,0,90));
		manager.LoadHandItem (CreateStandardButton(manager.arrowButton, manager, previousMatButtonId), new Vector3(0,0,-90));
		manager.LoadHandItem (CreateChoiceButton(manager, pickingButtonId));
		manager.LoadHandItem (CreateChoiceButton(manager, paintingButtonId));

		// Material Projector
		materialProjector = CreateMaterialProjectorItem (manager, materialProjectorId);
		manager.LoadHandItem (materialProjector);

		// Update View
		UpdateCurrentMode ();
		UpdateCurrentMat ();
	}
	
	public override void OnTouch(int hanchorId){
		if (hanchorId == closeButtonId)
			manager.LoadMenu ("ClosedMenu");
		else if (hanchorId == pickingButtonId) {
			manager.SetPaintMode(MainManager.PaintMode.SimplePicking);
		} else if (hanchorId == paintingButtonId) {
			manager.SetPaintMode(MainManager.PaintMode.Painting);
		} else if (hanchorId == nextMatButtonId) {
			manager.NextMaterial();
		} else if (hanchorId == previousMatButtonId) {
			manager.PreviousMaterial();
		}

		// Update Views
		UpdateCurrentMode ();
		UpdateCurrentMat ();
	}
	
	/******************
	 *  Tool Methods  *
	 ******************/
	
	private void UpdateCurrentMode(){
		MainManager.PaintMode currentPaintMode = manager.GetPaintMode();
		if (currentPaintMode == MainManager.PaintMode.SimplePicking) {
			manager.SelectButton(pickingButtonId, true);
			manager.SelectButton(paintingButtonId, false);
		}
		else if (currentPaintMode == MainManager.PaintMode.Painting) {
			manager.SelectButton(pickingButtonId, false);
			manager.SelectButton(paintingButtonId, true);
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

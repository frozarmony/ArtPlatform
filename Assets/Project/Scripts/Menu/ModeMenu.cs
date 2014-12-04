using UnityEngine;
using System.Collections;

public class ModeMenu : HandMenu {
	
	/******************
	 *  Button's ID   *
	 ******************/
	
	private int closeButtonId;
	private int returnButtonId;
	private int pickingButtonId;
	private int paintingButtonId;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public ModeMenu(MainManager manager) : base(manager, MainManager.ContextOfGesture.Menu){
		closeButtonId = HandManager.HAND_ANCHOR_PALM;
		returnButtonId = HandManager.HAND_ANCHOR_THUMB_BASE;
		pickingButtonId = HandManager.HAND_ANCHOR_INDEX;
		paintingButtonId = HandManager.HAND_ANCHOR_MIDDLE;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void OnLoad(){
		manager.LoadHandButton (closeButtonId, manager.closeButton);
		manager.LoadHandButton (returnButtonId, manager.returnButton);
		manager.LoadHandButton (pickingButtonId, manager.pickingButton);
		manager.LoadHandButton (paintingButtonId, manager.paintingButton);

		SelectCurrentMode ();
	}
	
	public override void OnTouch(int hanchorId){
		if (hanchorId == closeButtonId)
			manager.LoadMenu ("ClosedMenu");
		else if (hanchorId == returnButtonId)
			manager.LoadMenu ("PaintMenu");
		else if (hanchorId == pickingButtonId) {
			manager.SetPaintMode(MainManager.PaintMode.SimplePicking);
			SelectCurrentMode ();
		} else if (hanchorId == paintingButtonId) {
			manager.SetPaintMode(MainManager.PaintMode.Painting);
			SelectCurrentMode ();
		}
	}
	
	/******************
	 *  Tool Methods  *
	 ******************/

	private void SelectCurrentMode(){
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

}

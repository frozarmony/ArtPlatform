using UnityEngine;
using System.Collections;

public class ClosedMenu : HandMenu {

	/******************
	 *  Button's ID   *
	 ******************/

	private int openButtonId;
	private int undoButtonId;
	private int redoButtonId;
	private int moveButtonId;
	
	/****************
	 *  References  *
	 ****************/

	private ChoiceButtonItem moveButton;
	
	/******************
	 *  Constructor   *
	 ******************/

	public ClosedMenu(MainManager manager) : base(manager, MainManager.ContextOfGesture.PaintMode){
		openButtonId = HandManager.HAND_ANCHOR_PALM;
		undoButtonId = HandManager.HAND_ANCHOR_THUMB_BASE;
		redoButtonId = HandManager.HAND_ANCHOR_THUMB_MIDDLE;
		moveButtonId = HandManager.HAND_ANCHOR_THUMB;
	}

	/******************
	 * Implementation *
	 ******************/

	public override void OnLoad(){
		manager.LoadHandItem (CreateStandardButton(manager.openButton, manager, openButtonId));
		manager.LoadHandItem (CreateStandardButton(manager.arrowButton, manager, undoButtonId), new Vector3(0,0,-90));
		manager.LoadHandItem (CreateStandardButton(manager.arrowButton, manager, redoButtonId), new Vector3(0,0,90));

		// Move Mode
		moveButton = CreateChoiceButton (manager, moveButtonId, manager.movingModeTexture);
		manager.LoadHandItem (moveButton, new Vector3(90,0,0));
		moveButton.SetSelected (manager.GetContextOfGesture() == MainManager.ContextOfGesture.Move);
	}

	public override void OnTouch(int hanchorId){
		if (hanchorId == openButtonId)
			manager.LoadMenu ("PaintMenu");
		else if (hanchorId == undoButtonId)
			manager.UndoAction ();
		else if (hanchorId == redoButtonId)
			manager.RedoAction ();
		else if (hanchorId == moveButtonId){
			if(manager.GetContextOfGesture() != MainManager.ContextOfGesture.Move){	// Move Mode
				manager.SetContextOfGesture(MainManager.ContextOfGesture.Move);
				moveButton.SetSelected(true);
			}
			else{																	// Back to Default Mode
				manager.SetContextOfGesture(this.GetContextOfGesture());
				moveButton.SetSelected(false);
			}
		}
	}
}

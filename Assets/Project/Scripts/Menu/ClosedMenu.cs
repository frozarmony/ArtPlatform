using UnityEngine;
using System.Collections;

public class ClosedMenu : HandMenu {

	/******************
	 *  Button's ID   *
	 ******************/

	private int openButtonId;
	private int undoButtonId;
	private int redoButtonId;
	
	/******************
	 *  Constructor   *
	 ******************/

	public ClosedMenu(MainManager manager) : base(manager, MainManager.ContextOfGesture.PaintMode){
		openButtonId = HandManager.HAND_ANCHOR_PALM;
		undoButtonId = HandManager.HAND_ANCHOR_THUMB_BASE;
		redoButtonId = HandManager.HAND_ANCHOR_THUMB_MIDDLE;
	}

	/******************
	 * Implementation *
	 ******************/

	public override void OnLoad(){
		manager.LoadHandItem (CreateStandardButton(manager.openButton, manager, openButtonId));
		manager.LoadHandItem (CreateStandardButton(manager.arrowButton, manager, undoButtonId), new Vector3(0,0,-90));
		manager.LoadHandItem (CreateStandardButton(manager.arrowButton, manager, redoButtonId), new Vector3(0,0,90));
	}

	public override void OnTouch(int hanchorId){
		if (hanchorId == openButtonId)
			manager.LoadMenu ("PaintMenu");
		else if (hanchorId == undoButtonId)
			manager.UndoAction ();
		else if (hanchorId == redoButtonId)
			manager.RedoAction ();
	}
}

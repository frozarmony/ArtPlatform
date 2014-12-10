using UnityEngine;
using System.Collections;

public class PaintMenu : HandMenu {
	
	/******************
	 *  Button's ID   *
	 ******************/
	
	private int closeButtonId;
	private int matButtonId;
	private int modButtonId;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public PaintMenu(MainManager manager) : base(manager, MainManager.ContextOfGesture.Menu){
		closeButtonId = HandManager.HAND_ANCHOR_PALM;
		matButtonId = HandManager.HAND_ANCHOR_THUMB_MIDDLE;
		modButtonId = HandManager.HAND_ANCHOR_THUMB_BASE;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void OnLoad(){
		manager.LoadHandButton (CreateStandardButton(manager.closeButton, manager, closeButtonId));
		manager.LoadHandButton (CreateStandardButton(manager.matMenuButton, manager, matButtonId));
		manager.LoadHandButton (CreateStandardButton(manager.modMenuButton, manager, modButtonId));
	}
	
	public override void OnTouch(int hanchorId){
		if (hanchorId == closeButtonId)
			manager.LoadMenu ("ClosedMenu");
		else if (hanchorId == matButtonId)
			manager.LoadMenu ("MatMenu");
		else if (hanchorId == modButtonId)
			manager.LoadMenu ("ModeMenu");
	}
}

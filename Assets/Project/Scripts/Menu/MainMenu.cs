using UnityEngine;
using System.Collections;

public class MainMenu : HandMenu {

	/******************
	 *  Button's ID   *
	 ******************/
	
	private int paintModeButtonId;
	private int interractModeButtonId;
	
	/****************
	 *  References  *
	 ****************/
	
	private ChoiceButtonItem moveButton;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public MainMenu(MainManager manager) : base(manager, MainManager.ContextOfGesture.Menu){
		paintModeButtonId = HandManager.HAND_ANCHOR_THUMB_BASE;
		interractModeButtonId = HandManager.HAND_ANCHOR_THUMB_MIDDLE;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void OnLoad(){
		manager.LoadHandItem (CreateChoiceButton(manager, paintModeButtonId, manager.paintingModeTexture));
		manager.LoadHandItem (CreateChoiceButton(manager, interractModeButtonId, manager.interractModeTexture));
	}
	
	public override void OnTouch(int hanchorId){
		if (hanchorId == paintModeButtonId)
			manager.LoadMenu ("PaintMenu");
		else if (hanchorId == interractModeButtonId)
			manager.LoadMenu ("InterractionMenu");
	}
}

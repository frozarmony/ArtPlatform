using UnityEngine;
using System.Collections;

public class ClosedMenu : HandMenu {

	/******************
	 *  Button's ID   *
	 ******************/

	private int openButtonId;
	
	/******************
	 *  Constructor   *
	 ******************/

	public ClosedMenu(MainManager manager) : base(manager){
		openButtonId = HandManager.HAND_ANCHOR_PALM;
	}

	/******************
	 * Implementation *
	 ******************/

	public override void OnLoad(){
		manager.LoadHandButton (openButtonId, manager.openButton);
	}

	public override void OnTouch(int hanchorId){
		if (hanchorId == openButtonId)
			manager.LoadMenu ("PaintMenu");
	}
}
